using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD
using static Contab.Global;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Net.Http;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.CodeDom.Compiler;
using System.Runtime.Remoting.Channels;
using System.Net.Mail;
using Contab.Properties;
using System.Security.Cryptography;

namespace Contab {
	public partial class FrmSesion : Form {

		private int lblResultadoMsShown = 0;
        private int connStatusLbl = -1;	//0 unsecure, 1 secure

        private HttpClient _httpClt = null;
        private String _httpFilename = null;
        private Task<byte[]> _httpTaskBytes = null; //MariaDB ODBC Connector

        private bool _cmbServerUpdating = false;
        //
        private HttpClient _serverUrlClt = null;
        private string _serverUrlRequesting = "";
        private Task<String> _serverUrlIndexTask = null;
        private ClsAppState.ServerDescIndex _serverUrlIndex = null;
        private Task<String> _serverUrlDescTask = null;
        //Updates
        private int _updAnlzWait = 1000;    //ms to wait for next update availability search
        private int _updVerifSeqLast = 0;

        public FrmSesion() {
			InitializeComponent();
            lblResultadoMsShown = 0;
            lblResultado.Text = "";
            //init app-state
            {
                ClsAppState.AppStateRoot state = ClsAppState.getAppState();
                _cmbServerUpdating = true;
                {
                    this.applyAppState(state, true /*allowDownload*/);
                    //apply custom-installer default value
                    if (cmbServer.Text == null || cmbServer.Text.Length <= 0) {
                        string defServer = ClsUpdaterCore.getDefaultServerUrl();
                        if (defServer != null && defServer.Length > 0) {
                            cmbServer.Text = defServer;
                            this.cmbServerApply(true, true);
                        }
                    }
                }
                _cmbServerUpdating = false;
            }
            //
            this.applyConnStatusLbl();
            //load last data
            /*{
                ClsAppState.ConfigUser last = ClsAppState.configUserLoadLast();
                if (cfgLast != null) {
                    if (cfgLast.lastDb != null && cfgLast.lastDb.Length > 0) {
                        cmbDbName.Text = cfgLast.lastDb;
                    }
                    if (cfgLast.lastUsername != null && cfgLast.lastUsername.Length > 0) {
                        txtAlias.Text = cfgLast.lastUsername;
                    }
                }
            }*/
            //set focus
            if (txtAlias.Text != null && txtAlias.Text.Length > 0) {
                this.ActiveControl = txtContrasena;
            } else {
                this.ActiveControl = txtAlias;
            }
        }

		private void txtAlias_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == 10 || e.KeyChar == 13) {
				btnIniciar_Click(null, null);
			}
		}

		private void txtContrasena_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == 10 || e.KeyChar == 13) {
				btnIniciar_Click(null, null);
			}
		}

		private void btnIniciar_Click(object sender, EventArgs e) {
            string db = cmbDbName.Text.Trim();
			string alias = txtAlias.Text.Trim();
			string contrasena = txtContrasena.Text.Trim();
			if (db == "" || alias == "" || contrasena == "") {
                lblResultadoMsShown = 0;
                lblResultado.Text = "Faltan datos";
            } else {
                OdbcConnection conn = new OdbcConnection();
                conn.ConnectionString = "DRIVER={MARIADB ODBC 3.1 Driver}; Server={127.0.0.1}; Port=" + (Global.tunnel != null ? Global.tunnelPort : 3306) + "; Database={" + db + "}; Uid={" + alias + "}; Pwd={" + contrasena + "};";
                //
                lblResultadoMsShown = 0;
                lblResultado.Text = "Conectando...";
                Application.DoEvents();
                //
                try {
                    conn.Open();
					Global.conn = conn;
                    Global.connDb = db;
                    Global.connUser = alias;
                    lblResultadoMsShown = 0;
                    lblResultado.Text = "";
                    //Save last state
                    {
                        string serverSel = cmbServer.Text;
                        ClsAppState.AppStateRootServer s = ClsAppState.getServerState(serverSel);
                        if (s != null) {
                            s.lastDb = db;
                            s.lastUsername = alias;
                            ClsAppState.setServerState(s, null);
                        }
                    }
                    this.Close();
                } catch (Exception excp) {
                    if (excp.Message.IndexOf("IM002") >= 0) {
                        //MessageBox.Show("Error al intentar conectar a la base de datos local, falta DRIVER:\n\n" + excp.Message + "\n\nInstale 'MariaDB Connector ODBC 3.1+'.");
                        lblResultadoMsShown = 0;
                        lblResultado.Text = "Instale MariaDB Connector";
                        if (_httpTaskBytes != null) {
                            MessageBox.Show("La descarga de 'MariaDB Odbc Connector' continúa en segundo plano.");
                        } else {
                            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                            int majorI = 0, minorI = 0, compileI = 0, portI = 0;
                            string protocolI = "", serverI = "", pathI = "", channelI = "";
                            ulong installDateI = 0, installBootDateI = 0;
                            updCore.getVersionInstalled(ref majorI, ref minorI, ref compileI, ref protocolI, ref serverI, ref portI, ref pathI, ref channelI, ref installDateI, ref installBootDateI);
                            if (protocolI != null && protocolI.Length > 0 && serverI != null && serverI.Length > 0 && pathI != null && pathI.Length > 0 && portI > 0) {
                                DialogResult rr = MessageBox.Show("Se requiere instalar 'MariaDB Odbc Connector', desea descargarlo desde '" + serverI + "' e instalarlo ahora?", "Descargar requisito?", MessageBoxButtons.YesNo);
                                switch (rr) {
                                    case DialogResult.Yes:
                                        try {
                                            _httpFilename = "";
                                            if (Environment.Is64BitProcess) {
                                                _httpFilename = "mariadb-connector-odbc-3.1.17-win64.msi";
                                            } else {
                                                _httpFilename = "mariadb-connector-odbc-3.1.17-win32.msi";
                                            }
                                            _httpClt = new HttpClient();
                                            _httpTaskBytes = _httpClt.GetByteArrayAsync(protocolI + serverI + ":" + portI + (pathI != null && pathI.Length > 0 ? pathI : "/") + "_ext/" + _httpFilename);
                                        } catch (Exception) {
                                            MessageBox.Show("Error al inicializar la descarga de 'MariaDB Odbc Connector'.");
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    } else if (excp.Message.IndexOf("08S01") >= 0) {
                        lblResultadoMsShown = 0;
                        lblResultado.Text = "Reintente";
                    } else {
                        MessageBox.Show("Error al intentar conectar a la base de datos local:\n\n" + excp.Message + "\n\nContacte a su administrador.");
                        lblResultadoMsShown = 0;
                        lblResultado.Text = "Fallo";
                    }
                }
            }
        }
        private void applyConnStatusLbl() {
            int connStatus = (Global.tunnel != null && Global.tunnel.isRunning() && Global.tunnelLysrList != null && Global.tunnelLysrList.Length > 0 ? 1 : 0);
            if (connStatusLbl != connStatus) {
                connStatusLbl = connStatus;
                //
                picConnSecure.Visible = (connStatus == 1);
                picConnPlain.Visible = (connStatus == 0);
                lblConnStatus.Text = (connStatus == 1 ? "Conexión es segura" : "Conexión no es segura");
            }
        }
        private void tmrAsyncFrm_Tick(object sender, EventArgs e) {
			this.applyConnStatusLbl();
            //login-attempt-result's text
            if (_httpTaskBytes != null) {
                lblResultadoMsShown = 0;
                lblResultado.Text = "Descargando...";
            } else {
                lblResultadoMsShown += tmrAsyncFrm.Interval;
                if (lblResultadoMsShown >= 3000 && lblResultado.Text != ""){
                    lblResultado.Text = "";
                }
            }
            //MariaDB Connector background download
            if (_httpTaskBytes != null) {
                if (_httpTaskBytes.Status == TaskStatus.Canceled || _httpTaskBytes.Status == TaskStatus.Faulted || _httpTaskBytes.Status == TaskStatus.RanToCompletion) {
                    if (_httpTaskBytes.Status == TaskStatus.Canceled || _httpTaskBytes.Status == TaskStatus.Faulted){
                        _httpTaskBytes = null; //early nullyfication, to avoid repeat execution of this code (timer)
                        MessageBox.Show("Error al descargar 'MariaDB Odbc Connector'.");
                    } else if (_httpTaskBytes.Status == TaskStatus.RanToCompletion) {
                        byte[] rr = _httpTaskBytes.Result;
                        _httpTaskBytes = null; //early nullyfication, to avoid repeat execution of this code (timer)
                        if (rr == null || rr.Length <= 0) {
                            MessageBox.Show("Error al descargar 'MariaDB Odbc Connector'.");
                        } else {
                            bool fileWritten = false;
                            string filepath = SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0);
                            if (filepath == null || filepath.Length <= 0) {
                                MessageBox.Show("Error al intentar obtener ruta para escribir 'MariaDB Odbc Connector'.");
                            } else {
                                if (filepath[filepath.Length - 1] != '\\') {
                                    filepath += "\\";
                                }
                                filepath += _httpFilename;
                                //write fresh-file
                                if (!fileWritten) {
                                    try {
                                        File.WriteAllBytes(filepath, rr);
                                        fileWritten = true;
                                    } catch (Exception excp) {
                                        //error
                                        MessageBox.Show("Error al escribir 'MariaDB Odbc Connector' en '" + filepath + "': " + excp.Message);
                                    }
                                }
                                //Execute
                                if (fileWritten) {
                                    ProcessStartInfo psi = new ProcessStartInfo();
                                    psi.UseShellExecute = false;
                                    //psi.FileName = "cmd.exe";
                                    //psi.Arguments = "/C \"" + filepath + "\"";
                                    psi.FileName = "msiexec.exe";
                                    psi.Arguments = "/i \"" + filepath + "\" /passive"; // ... /passive = "progress bar only", /qn = "silent mode", /norestart
                                    //psi.Verb = "runas";
                                    try {
                                        Process proc = new Process();
                                        proc.StartInfo = psi;
                                        //
                                        {
                                            lblResultadoMsShown = 0;
                                            lblResultado.Text = "Ejecutando...";
                                            Application.DoEvents();
                                        }
                                        //
                                        proc.Start();
                                        proc.WaitForExit();
                                        //
                                        {
                                            lblResultadoMsShown = 0;
                                            lblResultado.Text = "Instalado...";
                                            Application.DoEvents();
                                            btnIniciar_Click(null, null);
                                        }
                                    } catch (Exception excp) {
                                        MessageBox.Show("Error al ejecutar instalador de 'MariaDB Odbc Connector' en '" + filepath + "': " + excp.Message);
                                    }
                                }
                            }
                        }
                    }
                    _httpTaskBytes = null;
                }
            }
            //Server Description Background download
            {
                //payload
                if (_serverUrlDescTask != null) {
                    if (_serverUrlDescTask.IsCanceled || _serverUrlDescTask.IsFaulted || _serverUrlDescTask.IsCompleted) {
                        if (_serverUrlDescTask.IsCanceled) {
                            lblServerHelp.Text = "Descarga de configuracion cancelada.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else if (_serverUrlDescTask.IsFaulted) {
                            lblServerHelp.Text = "Descarga de configuracion ha fallado.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else if (_serverUrlIndex == null || _serverUrlIndex.version <= 0) {
                            lblServerHelp.Text = "Descarga de configuracion no valida.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else if (_serverUrlDescTask.Result.Length <= 0) {
                            lblServerHelp.Text = "Descarga de configuracion sin datos.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else {
                            //Apply
                            try {
                                ClsAppState.ServerDesc desc = _serverUrlDescTask.Result.FromJson<ClsAppState.ServerDesc>();
                                if (desc == null) {
                                    lblServerHelp.Text = "Interpretacion de configuracion nula.";
                                    lblServerHelp.ForeColor = Color.Red;
                                } else {
                                    ClsAppState.AppStateRootServer serverState = ClsAppState.getServerState(_serverUrlRequesting);
                                    if (serverState == null) {
                                        serverState = new ClsAppState.AppStateRootServer();
                                        serverState.url = _serverUrlRequesting; ;
                                    }
                                    serverState.descVersion = _serverUrlIndex.version;
                                    serverState.descPayload = desc;
                                    if (!ClsAppState.setServerState(serverState, _serverUrlDescTask.Result)) {
                                        Debug.WriteLine("Server desc NOT saved: '" + _serverUrlRequesting + "'");
                                    } else {
                                        Debug.WriteLine("Server desc saved: '" + _serverUrlRequesting + "'");
                                    }
                                    lblServerHelp.Text = "Configuracion descargada.";
                                    lblServerHelp.ForeColor = Color.Black;
                                    //Load
                                    {
                                        ClsAppState.AppStateRoot state = ClsAppState.getAppState();
                                        state.lastServerUrl = _serverUrlRequesting;
                                        if (!ClsAppState.setAppState(state)) {
                                            Debug.WriteLine("AppState NOT saved: '" + _serverUrlRequesting + "'");
                                        } else {
                                            Debug.WriteLine("AppState saved: '" + _serverUrlRequesting + "'");
                                        }
                                        _cmbServerUpdating = true;
                                        {
                                            this.applyAppState(state, false /*allowDownload*/);
                                        }
                                        _cmbServerUpdating = false;
                                    }
                                }
                            } catch (Exception) {
                                lblServerHelp.Text = "Interpretacion de configuracion fallida.";
                                lblServerHelp.ForeColor = Color.Red;
                            }
                        }
                        _serverUrlRequesting = "";
                        _serverUrlIndexTask = null;
                        _serverUrlDescTask = null;
                    }
                } else if (_serverUrlIndexTask != null) {
                    if (_serverUrlIndexTask.IsCanceled || _serverUrlIndexTask.IsFaulted || _serverUrlIndexTask.IsCompleted) {
                        _serverUrlIndex = null;
                        if (_serverUrlIndexTask.IsCanceled) {
                            lblServerHelp.Text = "Descarga de indice cancelada.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else if (_serverUrlIndexTask.IsFaulted) {
                            lblServerHelp.Text = "Descarga de indice ha fallado.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else if (_serverUrlIndexTask.Result.Length <= 0) {
                            lblServerHelp.Text = "Descarga de indice sin datos.";
                            lblServerHelp.ForeColor = Color.Red;
                        } else {
                            //Apply
                            try {
                                ClsAppState.ServerDescIndex idx = _serverUrlIndexTask.Result.FromJson<ClsAppState.ServerDescIndex>();
                                if (idx == null) {
                                    lblServerHelp.Text = "Interpretacion de indice nulo.";
                                    lblServerHelp.ForeColor = Color.Red;
                                } else if (idx.version <= 0) {
                                    lblServerHelp.Text = "Indice con version '" + idx.version + "' no valida.";
                                    lblServerHelp.ForeColor = Color.Red;
                                } else if (idx.filename == null || idx.filename.Length <= 0) {
                                    lblServerHelp.Text = "Indice con filename no valido.";
                                    lblServerHelp.ForeColor = Color.Red;
                                } else {
                                    ClsAppState.AppStateRootServer serverState = ClsAppState.getServerState(_serverUrlRequesting);
                                    if (serverState != null && serverState.descVersion == idx.version && serverState.descPayload != null) {
                                        //desc already donwloaded
                                        lblServerHelp.Text = "Configuracion no ha cambiado.";
                                        lblServerHelp.ForeColor = Color.Black;
                                        //Load
                                        {
                                            ClsAppState.AppStateRoot state = ClsAppState.getAppState();
                                            state.lastServerUrl = _serverUrlRequesting;
                                            if (!ClsAppState.setAppState(state)) {
                                                Debug.WriteLine("AppState NOT saved: '" + _serverUrlRequesting + "'");
                                            } else {
                                                Debug.WriteLine("AppState saved: '" + _serverUrlRequesting + "'");
                                            }
                                            _cmbServerUpdating = true;
                                            {
                                                this.applyAppState(state, false /*allowDownload*/);
                                            }
                                            _cmbServerUpdating = false;
                                        }
                                    } else {
                                        //download updated desc
                                        _serverUrlClt = new HttpClient();
                                        _serverUrlDescTask = _serverUrlClt.GetStringAsync(_serverUrlRequesting + idx.filename);
                                        if (_serverUrlDescTask == null) {
                                            lblServerHelp.Text = "Fallo al iniciar descarga de configuracion.";
                                            lblServerHelp.ForeColor = Color.Red;
                                        } else {
                                            _serverUrlIndex = idx;
                                        }
                                    }
                                }
                            } catch (Exception) {
                                lblServerHelp.Text = "Interpretacion de indice fallida.";
                                lblServerHelp.ForeColor = Color.Red;
                            }
                        }
                        //indice ha fallado
                        if (_serverUrlIndex == null) {
                            _serverUrlRequesting = "";
                            _serverUrlIndexTask = null;
                            _serverUrlDescTask = null;
                        }
                    }
                }
            }
            //Update availability
            {
                _updAnlzWait -= tmrAsyncFrm.Interval;
                if (_updAnlzWait <= 0) {
                    _updAnlzWait = 1000;
                    //analyze
                    {
                        ClsLaunchCfg launchCfg = ClsLaunchCfg.getSharedInstance();
                        ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                        if (updCore != null) {
                            long curTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                            {
                                int curSeq = updCore.getLastSyncEventSeq();
                                if (_updVerifSeqLast == curSeq) {
                                    //same sequence, trigger new-sync action and analyze soon
                                    updCore.syncLastVersionIfInactive();
                                } else if (!updCore.isSyncingActiveNow()) {
                                    int majorI = 0, minorI = 0, compileI = 0;
                                    int majorL = 0, minorL = 0, compileL = 0;
                                    ulong installDateI = 0, installBootDateI = 0;
                                    String protocolI = "", serverI = ""; int portI = 0; String pathI = "";
                                    String channelI = "";
                                    //
                                    String protocolSel = "", serverSel = ""; int portSel = 0, sourceSelSeq = 0; String pathSel = "";
                                    String channelSel = ""; int channelSelSeq = 0;
                                    //
                                    updCore.getSourceSelected(ref protocolSel, ref serverSel, ref portSel, ref pathSel, ref sourceSelSeq);
                                    updCore.getSourceChannelSelected(ref channelSel, ref channelSelSeq);
                                    updCore.getVersionInstalled(ref majorI, ref minorI, ref compileI, ref protocolI, ref serverI, ref portI, ref pathI, ref channelI, ref installDateI, ref installBootDateI);
                                    updCore.getVersionLatest(ref majorL, ref minorL, ref compileL);
                                    //
                                    if (majorI == 0 && minorI == 0 && compileI == 0) {
                                        //not installed, running as a portable version
                                        //do not offer update.
                                        //} else if (Global.conn == null) {
                                        //user not logged-in
                                        //do not offer update.
                                    } else if (majorL == 0 && minorL == 0 && compileL == 0) {
                                        //remote not synced yet
                                        //validate again sooner
                                    } else {
                                        bool updAvailable = false;
                                        if (serverI.Length > 0 && portI > 0 && pathI.Length > 0 && channelI.Length > 0 && serverSel.Length > 0 && portSel > 0 && pathSel.Length > 0 && channelSel.Length > 0 && (serverI != serverSel || portI != portSel || pathI != pathSel || channelI != channelSel)) {
                                            //update server migration (changed)
                                            updAvailable = true;
                                        } else if ((majorL != 0 || minorL != 0 || compileL != 0) && (majorI < majorL || (majorI == majorL && minorI < minorL) || (majorI == majorL && minorI == minorL && compileI < compileL))) {
                                            //explicit update available
                                            updAvailable = true;
                                        } else {
                                            //default
                                            updAvailable = false;
                                        }
                                        //show update window
                                        lnkUpdateAvailable.Visible = updAvailable;
                                    }
                                    _updVerifSeqLast = curSeq;
                                }
                            }
                        }
                    }
                }
            }
        }

        [DllImport("shell32",
        CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath(
        [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, int hToken = 0);

        private void lblConnStatus_MouseHover(object sender, EventArgs e){
            toolTipFrm.Show(connStatusLbl == 1 ? "Se ha establecido un tunnel encriptado: " + (Global.tunnelLysrList != null ? Global.tunnelLysrList : "no-lyrs") : "No se estableció un tunnel encriptado", lblConnStatus);
        }

        private void picConnPlain_MouseHover(object sender, EventArgs e){
            toolTipFrm.Show(connStatusLbl == 1 ? "Se ha establecido un tunnel encriptado: " + (Global.tunnelLysrList != null ? Global.tunnelLysrList : "no-lyrs") : "No se estableció un tunnel encriptado", picConnPlain);
        }

        private void picConnSecure_MouseHover(object sender, EventArgs e){
            toolTipFrm.Show(connStatusLbl == 1 ? "Se ha establecido un tunnel encriptado: " + (Global.tunnelLysrList != null ? Global.tunnelLysrList : "no-lyrs") : "No se estableció un tunnel encriptado", picConnSecure);
        }

        private void cmbServer_TextChanged(object sender, EventArgs e) {
            if (!_cmbServerUpdating) {
                _cmbServerUpdating = true;
                this.cmbServerApply(true, false);
                _cmbServerUpdating = false;
            }
        }

        private void cmbServer_KeyPress(object sender, KeyPressEventArgs e) {
            if (!_cmbServerUpdating && (e.KeyChar == 10 || e.KeyChar == 13)) {
                _cmbServerUpdating = true;
                this.cmbServerApply(true, true);
                _cmbServerUpdating = false;
            }
        }

        private void btnServerDownload_Click(object sender, EventArgs e) {
            if (!_cmbServerUpdating) {
                _cmbServerUpdating = true;
                this.cmbServerApply(true, true);
                _cmbServerUpdating = false;
            }
        }

        private void cmbServerApply(bool allowDownload, bool startDownloadIfNewUrl) {
            string serverUrl = cmbServer.Text;
            if (serverUrl == null || serverUrl.Length <= 0) {
                //empty
                grpSync.Visible = true;
                grpLogin.Visible = false;
                this.logoApply(null);
            } else {
                string fixedUrl = serverUrl;
                if (fixedUrl.Length > 0 && fixedUrl[fixedUrl.Length - 1] != '/') {
                    fixedUrl += "/";
                }
                //value
                bool doRequest = false;
                ClsAppState.AppStateRootServer serverState = ClsAppState.getServerState(fixedUrl);
                if (serverState != null && serverState.descVersion > 0 && serverState.descPayload != null) {
                    //already connected to this server at least once
                    grpSync.Visible = false;
                    grpLogin.Visible = true;
                    this.loginAutoFill(serverState);
                    this.logoApply(serverState.descPayload);
                    doRequest = allowDownload;
                } else {
                    if (startDownloadIfNewUrl) {
                        //search for protocol
                        int posProto = fixedUrl.IndexOf("//");
                        if (posProto > 0) {
                            int posDomain = fixedUrl.IndexOf(".", posProto + "//".Length);
                            if (posDomain > 0) {
                                int lenDomainName = posDomain - posProto - "//".Length;
                                int lenDomianExt = fixedUrl.Length - posDomain - ".".Length;
                                if (lenDomainName > 0 && lenDomianExt > 0) {
                                    doRequest = allowDownload;
                                }
                            }
                        }
                    }
                    grpSync.Visible = true;
                    grpLogin.Visible = false;
                    this.logoApply(null);
                }
                //start request
                if (doRequest) {
                    if (_serverUrlRequesting == null || _serverUrlRequesting != fixedUrl) {
                        _serverUrlRequesting = fixedUrl;
                        try {
                            if (_serverUrlClt != null) _serverUrlClt.CancelPendingRequests();
                            _serverUrlClt = new HttpClient();
                            _serverUrlIndexTask = _serverUrlClt.GetStringAsync(fixedUrl + "_index.json");
                            if (_serverUrlIndexTask == null) {
                                lblServerHelp.Text = "Fallo al iniciar descarga de indice.";
                                lblServerHelp.ForeColor = Color.Red;
                            } else {
                                _serverUrlDescTask = null;
                                Debug.WriteLine("Requesting index for: '" + fixedUrl + "_index.json" + "'");
                                lblServerHelp.Text = "Descargando indice...";
                                lblServerHelp.ForeColor = Color.Blue;
                            }
                        } catch (Exception) {
                            lblServerHelp.Text = "Fallo al iniciar descarga de indice.";
                            lblServerHelp.ForeColor = Color.Red;
                        }
                    }
                } else if(allowDownload) {
                    lblServerHelp.Text = "Establezca el url del servidor de configuración para descargar los datos iniciales.";
                    lblServerHelp.ForeColor = Color.Black;
                }
            }
        }

        private void logoApply(ClsAppState.ServerDesc desc) {
            bool logoApplied = false;
            if (desc != null && desc.logoPng128 != null && desc.logoPng128.Length > 0) {
                try {
                    byte[] pngData = System.Convert.FromBase64String(desc.logoPng128);
                    if (pngData.Length > 0) {
                        Stream stream = new MemoryStream(pngData);
                        Image img = Image.FromStream(stream);
                        if (img != null) {
                            picLogo128.Image = img;
                            picLogo128.Invalidate();
                            logoApplied = true;
                        }
                    }
                } catch (Exception) {
                    //
                }
            }
            //set default logo
            if (!logoApplied) {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSesion));
                this.picLogo128.Image = ((System.Drawing.Image)(resources.GetObject("picLogo128.Image")));
            }
        }
        private void loginAutoFill(ClsAppState.AppStateRootServer serverState) {
            ClsAppState.ServerDesc desc = null;
            if (serverState != null) {
                desc = serverState.descPayload;
            }
            //db
            {
                cmbDbName.BeginUpdate();
                {
                    cmbDbName.Items.Clear();
                    if (desc != null && desc.dbs != null) {
                        int iDb; for (iDb = 0; iDb < desc.dbs.Length; iDb++) {
                            cmbDbName.Items.Add(desc.dbs[iDb]);
                        }
                    }
                    if (serverState != null && serverState.lastDb != null && serverState.lastDb.Length > 0) {
                        cmbDbName.Text = serverState.lastDb;
                    } else if (desc != null && desc.dbs != null && desc.dbs.Length > 0) {
                        cmbDbName.Text = desc.dbs[0];
                    } else {
                        cmbDbName.Text = "";
                    }
                }
                cmbDbName.EndUpdate();
            }
            //userName
            {
                txtAlias.Text = (serverState != null && serverState.lastUsername != null && serverState.lastUsername.Length > 0 ? serverState.lastUsername : "");
            }
            //tunnel
            {
                //stop previous tunnel
                if (Global.tunnel != null) {
                    Global.tunnel.stopFlag();
                    Global.tunnel = null;
                }
                //start new tunnel
                if (Global.tunnelDllFound && (Global.tunnel == null || !Global.tunnel.isRunning()) && desc != null && desc.tunnel != null) {
                    ClsAppState.ServerDescTunnel tunDesc = desc.tunnel;
                    if (tunDesc.server != null && tunDesc.server.Length > 0 && tunDesc.port > 0) {
                        int attemptIdx = 0;
                        int attemptsMax = 3;
                        Random rnd = new Random();
                        string lastErr = "";
                        while ((Global.tunnel == null || !Global.tunnel.isRunning()) && attemptIdx < attemptsMax) {
                            int portLcl = rnd.Next(2048, 65000);
                            string lyrsList = "";
                            string cfg = "";
                            cfg += "{";
                            cfg += "\"port\": " + portLcl;
                            cfg += ", \"redir\": {";
                            cfg += "   \"server\": \"" + tunDesc.server + "\"";
                            cfg += "   , \"port\": " + tunDesc.port;
                            if (tunDesc.layers != null && tunDesc.layers.Length > 0) {
                                cfg += "   , \"layers\": [";
                                int iLyr, lyrsAddedCount = 0;
                                for (iLyr = 0; iLyr < tunDesc.layers.Length; iLyr++) {
                                    string lyr = tunDesc.layers[iLyr];
                                    if (lyr != null && lyr.Length > 0) {
                                        if (lyrsAddedCount != 0) {
                                            cfg += ", ";
                                            lyrsList += ", ";
                                        }
                                        cfg += "\"" + lyr + "\"";
                                        lyrsList += "\"" + lyr + "\"";
                                        lyrsAddedCount++;
                                    }
                                }
                                cfg += "]";
                            }
                            if (tunDesc.mask != null) {
                                cfg += "   , \"mask\": {";
                                cfg += "      \"seed\": " + tunDesc.mask.seed;
                                cfg += "   }";
                            }
                            cfg += " }";
                            cfg += "}";
                            ClsTunnel tunnel = new ClsTunnel();
                            if (!tunnel.addPort(cfg)) {
                                lastErr = "Error al intentar agregar puerto al tunnel.\n\nContacte a su administrador.\n\n" + cfg;
                                tunnel.stopFlag();
                            } else if (!tunnel.prepare()) {
                                lastErr = "Error al intentar preparar el tunnel.\n\nContacte a su administrador.";
                                tunnel.stopFlag();
                            } else if (!tunnel.startListening()) {
                                lastErr = "Error al intentar la escucha del tunnel.\n\nContacte a su administrador.";
                                tunnel.stopFlag();
                            } else if (!tunnel.runAtNewThread()) {
                                lastErr = "Error al intentar ejecutar el hilo del tunnel.\n\nContacte a su administrador.";
                                tunnel.stopFlag();
                            } else {
                                Global.tunnel = tunnel;
                                Global.tunnelPort = portLcl;
                                Global.tunnelLysrList = lyrsList;
                                //MessageBox.Show("Tunnel inicializado.");
                            }
                            attemptIdx++;
                        }
                        //Error
                        if (Global.tunnel == null || !Global.tunnel.isRunning()) {
                            MessageBox.Show("Error despues de " + attemptIdx + " intentos. " + lastErr);
                            //Application.Exit();
                        }
                    }
                }
            }
            //show/hide server selection (reduce GUI visual load)
            {
                lblServerSel.Text = (desc != null && desc.tunnel != null ? desc.tunnel.server : "...");
                lblServerSel.Visible = (desc != null);
                picServerSel.Visible = (desc != null);
                cmbServer.Visible = (desc == null);
            }
        }

        private void applyAppState(ClsAppState.AppStateRoot state, bool allowDownload) {
            //server list
            {
                cmbServer.BeginUpdate();
                cmbServer.Items.Clear();
                if (state != null && state.servers != null) {
                    int iSrv; for (iSrv = 0; iSrv < state.servers.Length; iSrv++) {
                        ClsAppState.AppStateRootServer s = state.servers[iSrv];
                        if (s != null && s.url != null && s.url.Length > 0) {
                            cmbServer.Items.Add(s.url);
                        }
                    }
                }
                if (state != null && state.lastServerUrl != null && state.lastServerUrl.Length > 0) {
                    cmbServer.Text = state.lastServerUrl;
                } else {
                    cmbServer.Text = "";
                }
                cmbServer.EndUpdate();
            }
            //apply server
            {
                cmbServerApply(allowDownload, true);
            }
        }

        private void lblServerSel_MouseClick(object sender, MouseEventArgs e) {
            lblServerSel.Visible = false;
            picServerSel.Visible = false;
            cmbServer.Visible = true;
        }

        private void picServerSel_MouseClick(object sender, MouseEventArgs e) {
            lblServerSel.Visible = false;
            picServerSel.Visible = false;
            cmbServer.Visible = true;
        }

        private void lnkUpdateAvailable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            bool frmFound = false;
            FormCollection frms = Application.OpenForms;
            int i; for (i = 0; i < frms.Count; i++) {
                if (frms[i] != null && frms[i].GetType() == typeof(FrmManager)) {
                    frmFound = true;
                    break;
                }
            }
            //open window
            if (!frmFound) {
                FrmManager frm = new FrmManager();
                frm.Show();
            }
        }
    }
}

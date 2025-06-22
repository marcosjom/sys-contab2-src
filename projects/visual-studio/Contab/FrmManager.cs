using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; //for ProcessStartInfo
using System.IO;
using System.Reflection; //for GetExecutingAssembly
using System.Resources;
using System.Diagnostics.Eventing.Reader;
using Contab.Properties;

namespace Contab {
    public partial class FrmManager : Form {

        private System.Timers.Timer _timer = null;
        private int actionCurLblValIdx = int.MaxValue;
        private int sourceSeqIdx = -1;
        private int channelSeqIdx = -1;
        private int channelsSeqIdx = -1;
        //
        private STEmbbededSourceHdr embeddedPkgHdr = null;
        private int embeddedMajor = 0, embeddedMinor = 0, embeddedCompile = 0;
        private byte[] embeddedPkgZip = null;
        //
        public FrmManager() {
            InitializeComponent();
            //first sync
            this.timerFired();
            //
            {
                //detect embedded pkg
                {
                    System.Resources.ResourceManager mngr = Resources.ResourceManager;
                    if (mngr != null) {
                        byte[] srcHead = (byte[])mngr.GetObject("src_head", Resources.Culture);
                        byte[] srcPkg = (byte[])mngr.GetObject("src_pkg", Resources.Culture);
                        if (srcHead != null && srcPkg != null) {
                            String srcHeadStr = System.Text.Encoding.Default.GetString(srcHead);
                            try {
                                STEmbbededSourceHdr loaded = srcHeadStr.FromJson<STEmbbededSourceHdr>();
                                if (loaded.srcProtocol != null && loaded.srcServer != null && loaded.srcPort > 0 && loaded.srcChannel != null && loaded.ver != null) {
                                    int major = 0, minor = 0, compile = 0;
                                    if (ClsUpdaterCore.parseVersion(loaded.ver, ref major, ref minor, ref compile)){
                                        embeddedPkgHdr = loaded;
                                        embeddedMajor = major;
                                        embeddedMinor = minor;
                                        embeddedCompile = compile;
                                        embeddedPkgZip = srcPkg;
                                    }
                                }
                            } catch (Exception) {
                                //
                            }
                        }
                    }
                }
                //apply state
                if (embeddedPkgHdr != null && embeddedPkgHdr.ver != null && embeddedPkgHdr.ver.Length > 0){
                    lblEmbedded.Visible = true;
                    lblEmbedded.Text = "Esta version: " + embeddedPkgHdr.ver;
                    btnOpenAsDriverGUI.Text = "Instalar";
                    btnOpenAsDriverGUI.Visible = true;
                } else {
                    lblEmbedded.Visible = false;
                    this.btnOpenAsDriverGUI.Visible = false;
                }
                //select default channel
                if(embeddedPkgHdr != null && embeddedPkgHdr.srcChannel != null) {
                    ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                    if (updCore != null) {
                        if (!updCore.channelIsSet()) {
                            updCore.setSourceChannel(embeddedPkgHdr.srcChannel);
                        }
                    }
                }
            }
            //apply changes (sync)
            this.timerFired();
            //start timer
            _timer = new System.Timers.Timer();
            _timer.Elapsed += (sender, e) => { timerFiredObj(this); };
            _timer.Interval = (1000 / 4); //4 ticks per second
            _timer.AutoReset = false;
            _timer.Start();
        }

        //

        private static void timerFiredObj(FrmManager instance) {
            if (instance != null) {
                try {
                    instance.Invoke((MethodInvoker)delegate {
                        // Running on the UI thread
                        instance.timerFired();
                    });
                } catch (Exception) { 
                    //
                }
            }
        }

        private void timerFired() {
            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
            //update visual state
            if (updCore != null) {
                ulong installDateI = 0, installBootDateI = 0;
                //source
                {
                    String protocol = "";
                    String server = "";
                    String path = "";
                    String channel = "";
                    String[] channels = null;
                    int port = 0, iSeqSource = 0, iSeqChannel = 0, iSeqChannels = 0;
                    //retrieve
                    updCore.getSourceSelected(ref protocol, ref server, ref port, ref path, ref iSeqSource);
                    updCore.getSourceChannelSelected(ref channel, ref iSeqChannel);
                    updCore.getSourceChannelsList(ref channels, ref iSeqChannels);
                    //apply (if necesary)
                    if (this.channelsSeqIdx != iSeqChannels || this.channelSeqIdx != iSeqChannel) {
                        //info
                        if (channel == null || channel.Length <= 0) {
                            this.sourceInfoLbl.Text = protocol + server + ":" + port + path;
                        } else {
                            this.sourceInfoLbl.Text = protocol + server + ":" + port + path + " (" + channel + ")";
                        }
                        //channels
                        {
                            this.sourceEditChannelCmb.BeginUpdate();
                            this.sourceEditChannelCmb.Items.Clear();
                            if(channels != null) {
                                int i; for (i = 0; i < channels.Length; i += 2) { //id + localized-name pairs
                                    String channelId = (String)channels[i];
                                    this.sourceEditChannelCmb.Items.Add(channelId);
                                }
                            }
                            this.sourceEditChannelCmb.Text = channel;
                            this.sourceEditChannelCmb.EndUpdate();
                        }
                        //
                        this.channelsSeqIdx = iSeqChannels;
                        this.channelSeqIdx = iSeqChannel;
                    }
                    //apply (if necesary)
                    if (this.sourceSeqIdx != iSeqSource || this.channelSeqIdx != iSeqChannel) {
                        //info
                        if (channel == null || channel.Length <= 0) {
                            this.sourceInfoLbl.Text = protocol + server + ":" + port + path;
                        } else {
                            this.sourceInfoLbl.Text = protocol + server + ":" + port + path + " (" + channel + ")";
                        }
                        //edit
                        this.sourceEditProtocolCmb.Text = protocol;
                        this.sourceEditServerTxt.Text = server + path;
                        this.sourceEditPortTxt.Text = "" + port;
                        this.sourceEditChannelCmb.Text = channel;
                        //keep track of changes (to reduce gui-update actions)
                        this.sourceSeqIdx = iSeqSource;
                        this.channelSeqIdx = iSeqChannel;
                    }
                }
                //Embedded installer
                if (embeddedPkgHdr != null && embeddedPkgHdr.ver != null && embeddedPkgHdr.ver.Length > 0){
                    int majorI = 0, minorI = 0, compileI = 0;
                    String protocolI = "", serverI = ""; int portI = 0; String pathI = "";
                    String channelI = "";
                    updCore.getVersionInstalled(ref majorI, ref minorI, ref compileI, ref protocolI, ref serverI, ref portI, ref pathI, ref channelI, ref installDateI, ref installBootDateI);
                    if (majorI == 0 && minorI == 0 && compileI == 0) {
                        btnOpenAsDriverGUI.Text = "Instalar";
                    } else if (majorI < embeddedMajor || (majorI == embeddedMajor && minorI < embeddedMinor) || (majorI == embeddedMajor && minorI == embeddedMinor && compileI < embeddedCompile)) {
                        btnOpenAsDriverGUI.Text = "Actualizar";
                    } else if (majorI > embeddedMajor || (majorI == embeddedMajor && minorI > embeddedMinor) || (majorI == embeddedMajor && minorI == embeddedMinor && compileI > embeddedCompile)) {
                        btnOpenAsDriverGUI.Text = "Degradar";
                    } else {
                        btnOpenAsDriverGUI.Text = "Reparar";
                    }
                }
                //versions
                {
                    int majorI = 0, minorI = 0, compileI = 0;
                    String protocolI = "", serverI = ""; int portI = 0; String pathI = ""; 
                    String channelI = "";
                    updCore.getVersionInstalled(ref majorI, ref minorI, ref compileI, ref protocolI, ref serverI, ref portI, ref pathI, ref channelI, ref installDateI, ref installBootDateI);
                    //if(installDateI <= 0){
                    this.versionCurLbl.Text = "Version instalada: " + majorI + "." + minorI + "." + compileI;
                    this.versionCurLbl.Visible = (majorI != 0 || minorI != 0 || compileI != 0);
                    //} else {
                    //    [this.versionCurLbl setStringValue:[NSString stringWithFormat:"Installed version: %d.%d.%d (installed %@)", majorI, minorI, compileI, installDateI]];
                    //}
                    //
                    if (!updCore.channelIsSet()) {
                        this.installBtn.Enabled = false;
                        this.installBtn.Text = "Esperando ...";
                        this.versionLatestLbl.Text = "Version online: selecciona un canal.";
                        this.versionLatestLbl.ForeColor = Color.Red;
                    } else {
                        int majorL = 0, minorL = 0, compileL = 0;
                        String channelSrc = ""; int channelSrcSeq = 0;
                        updCore.getSourceChannelSelected(ref channelSrc, ref channelSrcSeq);
                        updCore.getVersionLatest(ref majorL, ref minorL, ref compileL);
                        if (majorL == 0 && minorL == 0 && compileL == 0) {
                            this.installBtn.Enabled = false;
                            if (updCore.rootIsSyncing() || updCore.channelIsSyncing()) {
                                this.installBtn.Text = "Solicitando ...";
                                this.versionLatestLbl.Text = "Version online: obteniendo...";
                                this.versionLatestLbl.ForeColor = Color.Blue;
                            } else {
                                this.installBtn.Text = "Esperando ...";
                                this.versionLatestLbl.Text = "Version online: esperando...";
                                this.versionLatestLbl.ForeColor = Color.Gray;
                            }
                        } else {
                            this.installBtn.Enabled = true;
                            this.versionLatestLbl.Text = "Version online: " + majorL + "." + minorL + "." + compileL;
                            this.versionLatestLbl.ForeColor = Color.Black;
                            if (majorI == 0 && minorI == 0 && compileI == 0) {
                                this.installBtn.Text = "Descargar e instalar";
                            } else if (majorI < majorL || (majorI == majorL && minorI < minorL) || (majorI == majorL && minorI == minorL && compileI < compileL)) {
                                this.installBtn.Text = "Descargar y actualizar";
                            } else if (majorI > majorL || (majorI == majorL && minorI > minorL) || (majorI == majorL && minorI == minorL && compileI > compileL)) {
                                this.installBtn.Text = "Descargar y degradar";
                            } else {
                                this.installBtn.Text = "Descargar y reparar";
                            }
                        }
                    }
                    //Package version
                    /*{
                        int eMajorI = 0, eMinorI = 0, eCompileI = 0;
                        System.Reflection.Assembly ass = Assembly.GetExecutingAssembly();
                        if (ass != null) {
                            var assVer = ass.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                            if (assVer != null) {
                                String version = assVer.ToString();
                                if (version != null && version.Length > 0) {
                                    int major = 0, minor = 0, compile = 0;
                                    if (CAUpdaterCore.parseVersion(version, ref major, ref minor, ref compile)) {
                                        eMajorI = major;
                                        eMinorI = minor;
                                        eCompileI = compile;
                                    } else {
                                        Debug.WriteLine("Version (embedded): version('" + version + "')is not valid.\n");
                                    }
                                }
                            }
                        }
                        / *if (this.versionEmbLbl != null) {
                            this.versionEmbLbl.Text = "Installer version: " + eMajorI + "." + eMinorI + "." + eCompileI;
                        }
                        if (this.installEmbBtn != null) {
                            if (majorI == 0 && minorI == 0 && compileI == 0) {
                                this.installEmbBtn.Text = "Offline Install";
                            } else if (majorI == eMajorI && minorI == eMinorI && compileI == eCompileI) {
                                this.installEmbBtn.Text =  "Offline Update / Repair";
                            } else if (majorI < eMajorI || (majorI == eMajorI && minorI < eMinorI) || (majorI == eMajorI && minorI == eMinorI && compileI < eCompileI)) {
                                this.installEmbBtn.Text =  "Offline Update / Repair";
                            } else {
                                this.installEmbBtn.Text = "Offline Downgrade / Repair";
                            }
                        }* /
                    }*/
                    this.removeBtn.Enabled = !(majorI == 0 && minorI == 0 && compileI == 0);
                }
                //action-state
                ulong curBootDate = updCore.getCurBootDate();
                if (installBootDateI != 0 && curBootDate != 0 && installBootDateI == curBootDate) {
                    this.actionCurLbl.Text = "Se requiere reiniciar.\n";
                    this.actionCurLbl.ForeColor = Color.Blue;
                } else if (updCore.isLastSyncingAnError()) {
                    String errStr = updCore.getSyncLastError();
                    if (errStr == null) {
                        if (this.actionCurLblValIdx != -1) {
                            this.actionCurLblValIdx = -1;
                            this.actionCurLbl.Text = "Error no especificado.";
                            this.actionCurLbl.ForeColor = Color.Red;
                        }
                    } else {
                        if (this.actionCurLblValIdx != -2) {
                            this.actionCurLblValIdx = -2;
                            this.actionCurLbl.ForeColor = Color.Red;
                        }
                        this.actionCurLbl.Text = "Error: " + errStr;
                    }
                } else if (updCore.rootIsSyncing()) {
                    if (this.actionCurLblValIdx != 2) {
                        this.actionCurLblValIdx = 2;
                        this.actionCurLbl.Text = "Obteniendo indice remoto...";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else if (!updCore.channelIsSet()) {
                    if (this.actionCurLblValIdx != 3) {
                        this.actionCurLblValIdx = 3;
                        this.actionCurLbl.Text = "Por favor seleccione un canal.";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else if (updCore.channelIsSyncing()) {
                    if (this.actionCurLblValIdx != 4) {
                        this.actionCurLblValIdx = 4;
                        this.actionCurLbl.Text = "Obteniendo indice de canal...";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else if (!updCore.versionPayloadDownloadIsAllowed()) {
                    this.setDefaultText(updCore);
                } else if (updCore.versionPayloadIsDownloading()) {
                    if (this.actionCurLblValIdx != 5) {
                        this.actionCurLblValIdx = 5;
                        this.actionCurLbl.Text = "Descargando ...";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else if (!updCore.versionPayloadIsExtractIsAllowed()) {
                    this.setDefaultText(updCore);
                } else if (updCore.versionPayloadIsExtracting()) {
                    if (this.actionCurLblValIdx != 6) {
                        this.actionCurLblValIdx = 6;
                        this.actionCurLbl.Text = "Extrayendo ...";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else {
                    //Actions
                    if (updCore.versionPayloadIsInstallIsAllowed()) {
                        if (updCore.versionPayloadIsInstalling()) {
                            if (this.actionCurLblValIdx != 7) {
                                this.actionCurLblValIdx = 7;
                                this.actionCurLbl.Text = "Instalando ...";
                                this.actionCurLbl.ForeColor =Color.Blue;
                            }
                        } else if (updCore.versionPayloadIsInstalled()) {
                            if (this.actionCurLblValIdx != 8) {
                                this.actionCurLblValIdx = 8;
                                this.actionCurLbl.Text = "Instalacion completada, por favor reinicie.";
                                this.actionCurLbl.ForeColor =Color.Red;
                            }
                        } else {
                            if (this.actionCurLblValIdx != 9) {
                                this.actionCurLblValIdx = 9;
                                this.actionCurLbl.Text = "Estado desconocido.";
                                this.actionCurLbl.ForeColor = Color.Black;
                            }
                        }
                    } else if (updCore.versionPayloadIsUninstallIsAllowed()) {
                        if (updCore.versionPayloadIsUninstalling()) {
                            if (this.actionCurLblValIdx != 7) {
                                this.actionCurLblValIdx = 7;
                                this.actionCurLbl.Text = "Desinstalando ...";
                                this.actionCurLbl.ForeColor =Color.Blue;
                            }
                        } else if (updCore.versionPayloadIsUninstalled()) {
                            if (this.actionCurLblValIdx != 8) {
                                this.actionCurLblValIdx = 8;
                                this.actionCurLbl.Text = "Desinstalacion completada, por favor reinicie.";
                                this.actionCurLbl.ForeColor =Color.Red;
                            }
                        } else {
                            if (this.actionCurLblValIdx != 9) {
                                this.actionCurLblValIdx = 9;
                                this.actionCurLbl.Text = "Estado desconocido.";
                                this.actionCurLbl.ForeColor =Color.Black;
                            }
                        }
                    } else {
                        this.setDefaultText(updCore);
                    }
                }
            }
            //program new timer
            {
                _timer = new System.Timers.Timer();
                _timer.Elapsed += (sender, e) => { timerFiredObj(this); };
                _timer.Interval = (1000 / 4); //4 ticks per second
                _timer.AutoReset = false;
                _timer.Start();
            }
        }

        private void setDefaultText(ClsUpdaterCore updCore) {
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
            if(serverI.Length > 0 && portI > 0 && channelI.Length > 0 && pathI.Length > 0 && serverSel.Length > 0 && portSel > 0 && channelSel.Length > 0 && pathSel.Length> 0 && (serverI != serverSel || portI != portSel || pathI != pathSel  || channelI != channelSel)){
                this.actionCurLbl.Text = "Actualice para migrar canal.";
                this.actionCurLbl.ForeColor = Color.Blue;
            } else if(this.actionCurLblValIdx != (majorI + minorI + compileI + majorL + minorL + compileL)){
                this.actionCurLblValIdx = (majorI + minorI + compileI + majorL + minorL + compileL);
                if((majorI != 0 || minorI != 0 || compileI != 0) && (majorL != 0 || minorL != 0 || compileL != 0)){
                    if(majorI<majorL || (majorI == majorL && minorI<minorL) || (majorI == majorL && minorI == minorL && compileI<compileL)){
                        this.actionCurLbl.Text = "Actualizacion disponible!";
                        this.actionCurLbl.ForeColor = Color.Blue;
                    } else if (majorI == majorL && minorI == minorL && compileI == compileL) {
                        this.actionCurLbl.Text = "Esta actualizado!";
                        this.actionCurLbl.ForeColor = Color.Blue;
                    } else {
                        this.actionCurLbl.Text = "Por favor selecione una opcion.";
                        this.actionCurLbl.ForeColor = Color.Black;
                    }
                } else {
                    this.actionCurLbl.Text = "Por favor selecione una opcion.";
                    this.actionCurLbl.ForeColor = Color.Black;
                }
            }
        }

        private void sourceEditApply_Click(object sender, EventArgs e) {
            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
            if (updCore != null) {
                String protocol = this.sourceEditProtocolCmb.Text;
                String serverAndPath = this.sourceEditServerTxt.Text;
                String port = this.sourceEditPortTxt.Text;
                String channel = this.sourceEditChannelCmb.Text;
                String server = "", path = "";
                int posPath = serverAndPath.IndexOf("/");
                if (posPath < 0) {
                    server = serverAndPath;
                    path = "/"; //default
                } else {
                    server = serverAndPath.Substring(0, posPath);
                    path = serverAndPath.Substring(posPath);
                }
                //defaults
                {
                    if (protocol == null || protocol.Length == 0) {
                        protocol = "http://"; //default
                        this.sourceEditProtocolCmb.Text = protocol;
                    }
                    if (port == null || port.Length == 0) {
                        port = "80"; //default
                        this.sourceEditPortTxt.Text = port;
                    }
                }
                if (protocol != null && protocol.Length > 0 && server != null && server.Length > 0 && port != null && port.Length > 0 && path != null && path.Length > 0) {
                    try {
                        int portI = int.Parse(port);
                        if (portI > 0) {
                            updCore.setSourceSelected(protocol, server, portI, path);
                            if (channel != null && channel.Length > 0) {
                                updCore.setSourceChannel(channel);
                            }
                            this.sourceViewInfo.Visible = true;
                            this.sourceViewEdit.Visible = false;
                        }
                    } catch (Exception) { 
                        //
                    }
                }
            }
        }

        private void sourceInfoEditBtn_Click(object sender, EventArgs e) {
            this.sourceViewInfo.Visible = false;
            this.sourceViewEdit.Visible = true;
        }

        private void installEmbBtn_Click(object sender, EventArgs e) {
            if (DialogResult.OK == MessageBox.Show("Desea instalar usando este instalador como fuente?", "Instalar", MessageBoxButtons.OKCancel)) {
                ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                if (updCore != null) {
                    updCore.activateInstallAction();
                }
            }
        }

        private void installBtn_Click(object sender, EventArgs e) {
            if (DialogResult.OK == MessageBox.Show("Desea descargar e instalar la version mas reciente?", "Instalar", MessageBoxButtons.OKCancel)) {
                ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                if (updCore != null) {
                    updCore.activateInstallAction();
                }
            }
        }

        private void removeBtn_Click(object sender, EventArgs e) {
            if (embeddedPkgHdr != null && embeddedPkgHdr.ver != null && embeddedPkgHdr.ver.Length > 0){
                switch (MessageBox.Show("Desea desinstalar usando la version mas reciente? Seleccione 'No' para usar este desinstalador.", "Desinstalar", MessageBoxButtons.YesNoCancel)) {
                    case DialogResult.Yes:
                        {
                            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                            if (updCore != null){
                                updCore.activateUninstallAction(null, null);
                            }
                        }
                        break;
                    case DialogResult.No:
                        {
                            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                            if (updCore != null){
                                updCore.activateUninstallAction(embeddedPkgHdr, embeddedPkgZip);
                            }
                        }
                        break;
                    default:
                        break;
                }
            } else if (DialogResult.OK == MessageBox.Show("Desea desinstalar?", "Desinstalar", MessageBoxButtons.OKCancel)) {
                ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                if (updCore != null) {
                    updCore.activateUninstallAction(embeddedPkgHdr, embeddedPkgZip);
                }
            }
        }

        private void btnOpenAsDriverGUI_Click(object sender, EventArgs e) {
            if (embeddedPkgHdr != null && embeddedPkgHdr.ver != null && embeddedPkgHdr.ver.Length > 0){
                if (DialogResult.OK == MessageBox.Show("Desea instalar usando este instalador como origen?", "Instalar", MessageBoxButtons.OKCancel)){
                    ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                    if (updCore != null){
                        updCore.activateInstallActionEmbedded(embeddedPkgHdr, embeddedPkgZip);
                    }
                }
            } else {
                MessageBox.Show("Ninguna accion.");
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using System.Data.Odbc;    //para conexion a la BD
using System.Threading.Tasks;
using System.Diagnostics;
//
namespace Contab {
	public partial class FrmImportarMovs : Form {

		private string _cuentaPosDebe = "";
        private string _cuentaPosHaber = "";
        private string _cuentaNegDebe = "";
        private string _cuentaNegHaber = "";
        private bool _curTxtFmtAskedToUser = false;

        public FrmImportarMovs() {
			InitializeComponent();
		}

		private void FrmImportarMovs_Load(object sender, EventArgs e) {
			//
		}
        private void icoPosDebe_Click(object sender, EventArgs e) {
			this.selPosDebe();
        }

        private void lblPosDebe_Click(object sender, EventArgs e) {
			this.selPosDebe();
        }

		private void selPosDebe() {
			FrmSelCuenta frm = new FrmSelCuenta();
			frm.ShowDialog();
			{
				string cuentaId = frm.dameIdCuentaDetalleSel();
				string cuentaNom = frm.dameNombreCuentaDetalleSel();
				if (cuentaId != null && cuentaId.Length > 0) {
					_cuentaPosDebe = cuentaId;
					lblPosDebe.Text = cuentaNom;
                }
            }
        }

        private void icoPosHaber_Click(object sender, EventArgs e) {
			this.selPosHaber();
        }

        private void lblPosHaber_Click(object sender, EventArgs e) {
			this.selPosHaber();
        }

        private void selPosHaber() {
            FrmSelCuenta frm = new FrmSelCuenta();
            frm.ShowDialog();
            {
                string cuentaId = frm.dameIdCuentaDetalleSel();
                string cuentaNom = frm.dameNombreCuentaDetalleSel();
                if (cuentaId != null && cuentaId.Length > 0) {
                    _cuentaPosHaber = cuentaId;
                    lblPosHaber.Text = cuentaNom;
                }
            }
        }

        private void icoNegDebe_Click(object sender, EventArgs e) {
            this.selNegDebe();
        }

        private void lblNegDebe_Click(object sender, EventArgs e) {
			this.selNegDebe();
        }

        private void selNegDebe() {
            FrmSelCuenta frm = new FrmSelCuenta();
            frm.ShowDialog();
            {
                string cuentaId = frm.dameIdCuentaDetalleSel();
                string cuentaNom = frm.dameNombreCuentaDetalleSel();
                if (cuentaId != null && cuentaId.Length > 0) {
                    _cuentaNegDebe = cuentaId;
                    lblNegDebe.Text = cuentaNom;
                }
            }
        }

        private void icoNegHaber_Click(object sender, EventArgs e) {
			this.selNegHaber();
        }

        private void lblNegHaber_Click(object sender, EventArgs e) {
			this.selNegHaber();
        }

        private void selNegHaber() {
            FrmSelCuenta frm = new FrmSelCuenta();
            frm.ShowDialog();
            {
                string cuentaId = frm.dameIdCuentaDetalleSel();
                string cuentaNom = frm.dameNombreCuentaDetalleSel();
                if (cuentaId != null && cuentaId.Length > 0) {
                    _cuentaNegHaber = cuentaId;
                    lblNegHaber.Text = cuentaNom;
                }
            }
        }

        private void btnValidar_Click(object sender, EventArgs e) {
            string msgs = "";
            txtDatos.Enabled = false;
            btnValidar.Enabled = false;
            btnProcesar.Enabled = false;
            barProgress.Visible = true;
            lblProgress.Visible = true;
            barProgress.Minimum = barProgress.Maximum = barProgress.Value = 0;
            lblProgress.Text = "";
            {
                int linesCount = 0;
                if (!this.procesar(false, true, 0, ref linesCount, ref msgs)) {
                    MessageBox.Show("La validacion ha fallado (" + linesCount + " líneas)" + (msgs.Length > 0 ? ": " : ".") + msgs);
                } else {
                    MessageBox.Show("La validacion ha sido exitosa (" + linesCount + " líneas), puede proceder a importar" + (msgs.Length > 0 ? ": " : ".") + msgs);
                    _curTxtFmtAskedToUser = true;
                }
            }
            barProgress.Visible = false;
            lblProgress.Visible = false;
            txtDatos.Enabled = true;
            btnValidar.Enabled = true;
            btnProcesar.Enabled = true;
        }

        private void btnProcesar_Click(object sender, EventArgs e) {
            string msgs = "";
            txtDatos.Enabled = false;
            btnValidar.Enabled = false;
            btnProcesar.Enabled = false;
            barProgress.Visible = true;
            lblProgress.Visible = true;
            barProgress.Minimum = barProgress.Maximum = barProgress.Value = 0;
            lblProgress.Text = "";
            int linesCount = 0;
            if (!this.procesar(false, !_curTxtFmtAskedToUser, 0, ref linesCount, ref msgs)) {
                MessageBox.Show("La validacion ha fallado: " + msgs);
            } else {
                int linesCount2 = 0;
                barProgress.Minimum = 0;
                barProgress.Maximum = linesCount;
                barProgress.Value = 0;
                lblProgress.Text = "0 / " + linesCount;
                msgs = "";
                if (!this.procesar(true, false, linesCount, ref linesCount2, ref msgs)) {
                    MessageBox.Show("La importación ha fallado (" + linesCount2 + " de " + linesCount + " línea procesadas)" + (msgs.Length > 0 ? ": " : ".") + msgs);
                    if (linesCount2 != 0) {
                        MessageBox.Show("IMPORTANTE: la(s) primera(s) " + linesCount2 + " línea(s) fueron procesadas. Quítelas antes de reintentar.");
                    }
                } else {
                    //empty data to prevent double import
                    txtDatos.Text = "";
                    MessageBox.Show("Importación exitosa (" + linesCount2 + " de " + linesCount + " línea procesadas)" + (msgs.Length > 0 ? ": " : "." ) + msgs);
                }
            }
            _curTxtFmtAskedToUser = false; //ask again
            barProgress.Visible = false;
            lblProgress.Visible = false;
            txtDatos.Enabled = true;
            btnValidar.Enabled = true;
            btnProcesar.Enabled = true;
        }

        private bool procesar(bool aplicar, bool validateFmtsWithUser, int linesKnownTotal, ref int dstLinesCount, ref string dstMsgs) {
            bool r = false;
            int comprobantesCreados = 0;
            Int64 tasa10000 = 0;
            DateTime tasaFecha = DateTime.MinValue;
            OdbcCommand ssql; int linesCount = 0;
            {
                r = true;
                //tasa de cambio
                if (r && chkSonDolares.Checked) {
                    if (r && !chkTasaUsarBCN.Checked) {
                        string tasaStr = txtTasa.Text.Trim();
                        if (tasaStr.Length <= 0) {
                            dstMsgs += "Debe especificar tasa de cambio o permitir consultar BCN.";
                            r = false;
                        } else {
                            double tasaDouble = 0.0;
                            try {
                                tasaDouble = double.Parse(tasaStr);
                                tasa10000 = (Int64)(tasaDouble * 10000.0);
                            } catch (Exception ex) {
                                dstMsgs += "Debe especificar tasa de cambio valida o permitir consultar BCN.";
                                r = false;
                            }
                        }
                    }
                }
                //datos
                if (r) {
                    string datosStr = txtDatos.Text.Trim();
                    if (datosStr.Length <= 0) {
                        dstMsgs += "Los datos a importar estan vacios.";
                        r = false;
                    } else {
                        bool dateFmtAsked = false, amDecFmtAsked = false, am1000FmtAsked = false;
                        int pos = 0, datosStrLen = datosStr.Length;
                        //
                        if (r && validateFmtsWithUser) {
                            if (chkSonDolares.Checked) {
                                if (MessageBox.Show("La moneda es dólares. Es esta interpretación correcta?", "Validar moneda", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                                    dstMsgs += "Linea #" + (linesCount + 1) + ", usuario ha indicado que la moneda no es correcta; configuración debe corregirse.\n";
                                    r = false;
                                }
                            } else {
                                if (MessageBox.Show("La moneda es córdobas. Es esta interpretación correcta?", "Validar moneda", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                                    dstMsgs += "Linea #" + (linesCount + 1) + ", usuario ha indicado que la moneda no es correcta; configuración debe corregirse.\n";
                                    r = false;
                                }
                            }
                        }
                        //
                        while (r && pos < datosStrLen) {
                            int lnPos = datosStr.IndexOf("\n", pos);
                            if (lnPos < 0) lnPos = datosStrLen + 1;
                            string ln = datosStr.Substring(pos, lnPos - pos - 1).Trim();
                            if (ln.Length > 0) {
                                int sepDate = ln.IndexOf("\t", 0);
                                if (sepDate < 0) {
                                    dstMsgs += "Linea #" + (linesCount + 1) + " truncada despues de fecha.\n";
                                    r = false;
                                } else {
                                    int sepAmm = ln.IndexOf("\t", sepDate + 1);
                                    if (sepAmm < 0) {
                                        dstMsgs += "Linea #" + (linesCount + 1) + " truncada despues de monto.\n";
                                        r = false;
                                    } else {
                                        DateTime fecha = DateTime.MinValue;
                                        Int64 amm100 = 0;
                                        string fechaStr = ln.Substring(0, sepDate);
                                        string ammStr = ln.Substring(sepDate + 1, sepAmm - sepDate - 1);
                                        string desc = ln.Substring(sepAmm + 1);
                                        //fecha
                                        if (r) {
                                            try {
                                                fecha = DateTime.Parse(fechaStr);
                                                if (!dateFmtAsked && fecha.Day != fecha.Month && validateFmtsWithUser) {
                                                    dateFmtAsked = true;
                                                    if (MessageBox.Show("La fecha '" + fechaStr + "' fue interpretada como '" + fecha.Day + " de " + Global.monthsNames[fecha.Month - 1] + " de " + fecha.Year + "'. Es esta interpretación correcta?", "Validar formato fecha", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                                                        dstMsgs += "Linea #" + (linesCount + 1) + ", usuario ha indicado que la interpretación de fecha no es correcta; formato debe corregirse.\n";
                                                        r = false;
                                                    }
                                                }
                                            } catch (Exception) {
                                                dstMsgs += "Linea #" + (linesCount + 1) + ", fecha no valida.\n";
                                                r = false;
                                            }
                                        }
                                        //amm
                                        if (r) {
                                            try {
                                                double amm = double.Parse(ammStr);
                                                amm100 = (Int64)(amm * 100.0);
                                                if ((amm <= -1000 || amm >= 1000) && !am1000FmtAsked && validateFmtsWithUser) {
                                                    am1000FmtAsked = true;
                                                    if ((double)((Int64)amm) != amm) {
                                                        amDecFmtAsked = true;
                                                    }
                                                    if (MessageBox.Show("El monto '" + ammStr + "' fue interpretado como '" + String.Format("{0:0.00}", amm) + "'. Es esta interpretación correcta?", "Validar formato números-miles", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                                                        dstMsgs += "Linea #" + (linesCount + 1) + ", usuario ha indicado que la interpretación de números-miles no es correcta; formato debe corregirse.\n";
                                                        r = false;
                                                    }
                                                }
                                                if (!amDecFmtAsked && validateFmtsWithUser && (double)((Int64)amm) != amm) {
                                                    amDecFmtAsked = true;
                                                    if (MessageBox.Show("El monto '" + ammStr + "' fue interpretado como '" + String.Format("{0:0.00}", amm) + "'. Es esta interpretación correcta?", "Validar formato decimales", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                                                        dstMsgs += "Linea #" + (linesCount + 1) + ", usuario ha indicado que la interpretación de decimales no es correcta; formato debe corregirse.\n";
                                                        r = false;
                                                    }
                                                }
                                            } catch (Exception) {
                                                dstMsgs += "Linea #" + (linesCount + 1) + ", monto no valido.\n";
                                                r = false;
                                            }
                                        }
                                        //desc
                                        if(r) {
                                            desc = desc.Replace("\t", " ").Replace("\r", "").Replace("'", "").Replace("\"", "");
                                        }
                                        //tasa
                                        if (r && chkSonDolares.Checked && chkTasaUsarBCN.Checked) {
                                            if (tasaFecha.Year != fecha.Year || tasaFecha.Month != fecha.Month || tasaFecha.Day != fecha.Day) {
                                                Task<double> tasaTask = Global.queryTasaCambio_bcn_gob_niAsync(fecha);
                                                tasaTask.Wait();
                                                double tasa = tasaTask.Result;
                                                if (tasa <= 30.0) {
                                                    dstMsgs += "Linea #" + (linesCount + 1) + ", fallo al intentar obtener tasa BCN para fecha '" + fecha + "'.\n";
                                                    r = false;
                                                } else {
                                                    tasa10000 = (Int64)(tasa * 10000.0);
                                                    tasaFecha = fecha;
                                                }
                                            }
                                        }
                                        //
                                        if (r) {
                                            string cuentaDebe = "", cuentaHaber = "";
                                            Int64 debeCS = 0, haberCS = 0, debeUS = 0, haberUS = 0;
                                            if (amm100 < 0) {
                                                cuentaDebe = _cuentaNegDebe;
                                                cuentaHaber = _cuentaNegHaber;
                                                if (chkSonDolares.Checked) {
                                                    debeCS = haberCS = Global.convertCents(-amm100, tasa10000);
                                                    debeUS = haberUS = -amm100;
                                                } else {
                                                    debeCS = haberCS = -amm100;
                                                }
                                                //
                                                if (_cuentaNegDebe == null || _cuentaNegDebe.Length <= 0) {
                                                    dstMsgs += "Debe especificar la cuenta 'debe' para valores negativos.";
                                                    r = false;
                                                } else if (_cuentaNegHaber == null || _cuentaNegHaber.Length <= 0) {
                                                    dstMsgs += "Debe especificar la cuenta 'haber' para valores negativos.";
                                                    r = false;
                                                }
                                            } else {
                                                cuentaDebe = _cuentaPosDebe;
                                                cuentaHaber = _cuentaPosHaber;
                                                if (chkSonDolares.Checked) {
                                                    debeCS = haberCS = Global.convertCents(amm100, tasa10000);
                                                    debeUS = haberUS = amm100;
                                                } else {
                                                    debeCS = haberCS = amm100;
                                                }
                                                //
                                                if (_cuentaPosDebe == null || _cuentaPosDebe.Length <= 0) {
                                                    dstMsgs += "Debe especificar la cuenta 'debe' para valores positivos.";
                                                    r = false;
                                                } else if (_cuentaPosHaber == null || _cuentaPosHaber.Length <= 0) {
                                                    dstMsgs += "Debe especificar la cuenta 'haber' para valores positivos.";
                                                    r = false;
                                                }
                                            }
                                            //apply
                                            if (r && aplicar) {
                                                string idComprobanteDiario = Global.generaSiguienteComprobante(fecha);
                                                if (idComprobanteDiario == null || idComprobanteDiario.Length <= 0) {
                                                    dstMsgs += "Linea #" + (linesCount + 1) + ", fallo al crear comprobante.\n";
                                                    r = false;
                                                } else {
                                                    comprobantesCreados++;
                                                    if (r) {
                                                        string strSQL = "";
                                                        strSQL += " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
                                                        strSQL += " VALUES (";
                                                        strSQL += " '" + idComprobanteDiario + "'";
                                                        strSQL += ", '" + cuentaDebe + "'";
                                                        strSQL += ", " + debeCS.ToString() + "";
                                                        strSQL += ", " + "NULL" + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? 1 : 0) + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? tasa10000.ToString() : "NULL") + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? debeUS.ToString() : "NULL") + "";
                                                        strSQL += ", " + "NULL" + "";
                                                        strSQL += ", '" + desc + "'";
                                                        strSQL += " )";
                                                        strSQL += ", (";
                                                        strSQL += " '" + idComprobanteDiario + "'";
                                                        strSQL += ", '" + cuentaHaber + "'";
                                                        strSQL += ", " + "NULL" + "";
                                                        strSQL += ", " + haberCS.ToString() + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? 1 : 0) + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? tasa10000.ToString() : "NULL") + "";
                                                        strSQL += ", " + "NULL" + "";
                                                        strSQL += ", " + (chkSonDolares.Checked ? haberUS.ToString() : "NULL") + "";
                                                        strSQL += ", '" + desc + "'";
                                                        strSQL += " )";
                                                        try {
                                                            ssql = new OdbcCommand(strSQL, Global.conn);
                                                            ssql.ExecuteNonQuery();
                                                        } catch (Exception ex) {
                                                            dstMsgs += "Linea #" + (linesCount + 1) + ", fallo al intentar insertar registro DEBE: \n" + ex.Message;
                                                            r = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //nxt line
                            if (r) {
                                linesCount++;
                            }
                            pos = lnPos + 1; //+1 to exclude the '\n'
                            //progress
                            if (aplicar) {
                                barProgress.Value = linesCount;
                                lblProgress.Text = linesCount + " / " + linesKnownTotal;
                                Application.DoEvents();
                            } else {
                                lblProgress.Text = linesCount + " líneas";
                                Application.DoEvents();
                            }
                        }
                    }
                }
            }
            //
            if (aplicar) {
                if (comprobantesCreados > 0) {
                    bool accsUpdated = false;
                    string accList = "";
                    if (_cuentaPosDebe != null && _cuentaPosDebe.Length > 0) {
                        if (accList.Length > 0) accList += ", ";
                        accList += "'"+ _cuentaPosDebe + "'";
                    }
                    if (_cuentaPosHaber != null && _cuentaPosHaber.Length > 0) {
                        if (accList.Length > 0) accList += ", ";
                        accList += "'" + _cuentaPosHaber + "'";
                    }
                    if (_cuentaNegDebe != null && _cuentaNegDebe.Length > 0) {
                        if (accList.Length > 0) accList += ", ";
                        accList += "'" + _cuentaNegDebe + "'";
                    }
                    if (_cuentaNegHaber != null && _cuentaNegHaber.Length > 0) {
                        if (accList.Length > 0) accList += ", ";
                        accList += "'" + _cuentaNegHaber + "'";
                    }
                    try {
                        accsUpdated = Global.actualizarBalanceDeCuentas(accList);
                        if (accsUpdated) {
                            dstMsgs += "Comprobantes creados: " + comprobantesCreados + "\n";
                        } else {
                            dstMsgs += "Importación completada pero fallo al intentar actualizar el balance de cuentas. Actualize el balance de cuentas manualmente.";
                        }
                    } catch (Exception ex) {
                        dstMsgs += "Importación completada pero fallo al intentar actualizar el balance de cuentas. Actualize el balance de cuentas manualmente.\n\n" + ex.Message;
                    }
                }
            }
            //
            dstLinesCount = linesCount;
            //
            return r;
        }

        private void chkSonDolares_CheckedChanged(object sender, EventArgs e) {
            chkTasaUsarBCN.Enabled = chkSonDolares.Enabled;
            txtTasa.Enabled = (chkSonDolares.Enabled && !chkTasaUsarBCN.Checked);
        }

        private void chkTasaUsarBCN_CheckedChanged(object sender, EventArgs e) {
            chkTasaUsarBCN.Enabled = chkSonDolares.Enabled;
            txtTasa.Enabled = (chkSonDolares.Enabled && !chkTasaUsarBCN.Checked);
        }

        private void txtDatos_TextChanged(object sender, EventArgs e) {
            _curTxtFmtAskedToUser = false;
        }
    }
}

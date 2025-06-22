using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD
using System.Threading;     //para los hilos
using System.Threading.Tasks;

namespace Contab {
    public partial class FrmPrincipal : Form {

		//private ClsOrdenadorDeLista ordenadorListaPorColumna;
		private static FrmPrincipal ventanaCargada = null;

        public FrmPrincipal(){
            InitializeComponent();
			//
            //ordenadorListaPorColumna = new ClsOrdenadorDeLista();
            //
            {
				FrmSesion ventanaSesion = new FrmSesion();
				ventanaSesion.ShowDialog();
				if (Global.conn == null) {
                    Application.Exit();
                }
			}
        }

		private void FrmPrincipal_Load(object sender, EventArgs e) {
			if (Global.conn == null) {
                Application.Exit();
            } else {
				this.showContabFrm();
				//maximize
				this.WindowState = FormWindowState.Maximized;
			}
		}

		private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e) {
			if (Global.conn != null) {
				Global.conn.Close();
				Global.conn = null;
            }
            if (Global.tunnel != null) {
				Global.tunnel.stopFlag();
				Global.tunnel = null;
            }
        }

		private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e) {
            if (Global.tunnel != null) {
                Global.tunnel.stopFlag();
            }
            Application.Exit();
		}

		private void contabilidadToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void registrarEstadoDeCuentaToolStripMenuItem_Click(object sender, EventArgs e) {
			FrmTarjetaCredito frm = new FrmTarjetaCredito();
			frm.ShowDialog();
			frm = null;
		}

		//ToDo: implementar parametrizando cuentas origen/destino.
		/*private void moverSaldosHaciaPrincipalToolStripMenuItem_Click(object sender, EventArgs e) {
			FrmTarjetaCreditoCierreHaciaPrincipal frm = new FrmTarjetaCreditoCierreHaciaPrincipal();
			frm.ShowDialog();
			frm = null;
		}*/

		private class MovRecord {
			public int idMov = 0;
			public double csDebe = 0;
			public bool csDebeNull = true;
			public bool csDebeChanged = false;
			public double csHaber = 0;
			public bool csHaberNull = true;
			public bool csHaberChanged = false;
			public bool esDolares = false;
		};

        //ToDo: remove or re-enable
        /*private void conversionesDolaresA2DigitosToolStripMenuItem_Click(object sender, EventArgs e) {
			List<string> idComps = new List<string>();
			List<double> deltas = new List<double>();
			int conteoFilas = 0, conteoDeltas = 0; double deltasPos = 0, deltasNeg = 0;
			string filter = "";
			FrmSelCompPrefixFilter dialog = new FrmSelCompPrefixFilter();
			dialog.ShowDialog();
			if (dialog.actionConfirmed) {
				//Search for deltas
				{
					string strSQL = "";
					//strSQL += " SELECT ComprobantesDeDiario.idComprobanteDiario, (Sum(Round(Movimientos.montoDebeCS, 2)) - Sum(Round(Movimientos.montoHaberCS, 2))) AS delta ";
					strSQL += " SELECT ComprobantesDeDiario.idComprobanteDiario, (Sum(Movimientos.montoDebeCS) - Sum(Movimientos.montoHaberCS)) AS delta ";
					strSQL += " FROM ComprobantesDeDiario ";
					strSQL += " INNER JOIN Movimientos ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario ";
					if (dialog.selPrefix.Length > 0) {
						strSQL += " WHERE Movimientos.idComprobanteDiario LIKE '" + dialog.selPrefix + "%' ";
					}
					strSQL += " GROUP BY ComprobantesDeDiario.idComprobanteDiario ";
					//MessageBox.Show(strSQL);
					OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
					OdbcDataReader conC = ssql.ExecuteReader();
					while (conC.Read()) {
						conteoFilas++;
						string idComp = conC.GetString(0);
						double delta = conC.GetInt64(1);
						if (delta != 0) {
							idComps.Add(idComp);
							deltas.Add(delta);
							conteoDeltas++;
							if (delta < 0) {
								deltasNeg += delta;
							} else if (delta > 0) {
								deltasPos += delta;
							}
						}
					}
					conC.Close();
				}
				//Process result
				if (conteoDeltas == 0) {
					MessageBox.Show("Todos ("+ conteoFilas + ") los comprobantes concuerdan.\n");
				} else {
					if (MessageBox.Show(conteoDeltas + " de " + conteoFilas + " comprobantes con differencias que suman\npos(+" + deltasPos + ")\nneg(" + deltasNeg + ").\n\nDesea simular el truncado y distribucion las milesimas de conversiones de dolares?", "Diferencias", MessageBoxButtons.OKCancel) == DialogResult.OK) {
						int countSolved = runNormalizeCents(idComps, deltas, true);
						if (countSolved > 0) {
							if (MessageBox.Show("Desea aplicar el truncado y distribucion las milesimas de conversiones de dolares de " + countSolved + " comprobantes?", "Diferencias", MessageBoxButtons.OKCancel) == DialogResult.OK) {
								runNormalizeCents(idComps, deltas, false);
							}
						}
					}
				}
			}
		}*/

        //ToDo: remove, used only once
        /*
		private int runNormalizeCents(List<string> idComps, List<double> deltas, bool simulateOnly) {
			int conteoResuletosTruncando = 0, conteoResueltosDistCents = 0, cantCentsDists = 0, conteoNoResuletos = 0, conteoDeltasMayores = 0;
			string deltasNoResueltos = "";
			int i; for (i = 0; i < idComps.Count; i++) {
				string idComp = idComps.ElementAt<string>(i);
				double delta = deltas.ElementAt<double>(i);
				if (delta != 0) {
					if (delta < -0.02 || delta > 0.02) {
						if (deltasNoResueltos.Length != 0) deltasNoResueltos += ", ";
						deltasNoResueltos += String.Format("{0:0.000}", delta) + ": " + idComp;
						conteoDeltasMayores++;
					} else {
						bool applyChanges = false;
						double csDebeTotal = 0, csHaberTotal = 0, csDelta = 0;
						List<MovRecord> movs = new List<MovRecord>();
						//Load records
						{
							string strSQL = "";
							strSQL += " SELECT Movimientos.idMovimiento, Movimientos.montoDebeCS AS montoDebeCS, Movimientos.montoHaberCS AS montoHaberCS, Movimientos.esDolares ";
							//strSQL += " SELECT Movimientos.idMovimiento, Round(Movimientos.montoDebeCS, 2) AS montoDebeCS, Round(Movimientos.montoHaberCS, 2) AS montoHaberCS, Movimientos.esDolares ";
							strSQL += " FROM Movimientos ";
							strSQL += " WHERE idComprobanteDiario = '" + idComp + "'";
							OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
							OdbcDataReader conC = ssql.ExecuteReader();
							while (conC.Read()) {
								int idMov = conC.GetInt32(0);
								bool csDebeNull = true;
								bool csHaberNull = true;
								double csDebe = 0; if (!conC.IsDBNull(1)) { csDebeNull = false; csDebe = conC.GetInt64(1); }
								double csHaber = 0; if (!conC.IsDBNull(2)) { csHaberNull = false;  csHaber = conC.GetInt64(2); }
								bool esDolares = conC.GetBoolean(3);
								//
								MovRecord mov = new MovRecord();
								mov.idMov = idMov;
								mov.csDebe = csDebe;
								mov.csDebeNull = csDebeNull;
								mov.csDebeChanged = false;
								mov.csHaber = csHaber;
								mov.csHaberChanged = false;
								mov.csHaberNull = csHaberNull;
								mov.esDolares = esDolares;
								movs.Add(mov);
							}
							conC.Close();
						}
						//Remove decimals after two
						{
							int i2; for (i2 = 0; i2 < movs.Count; i2++) {
								MovRecord mov = movs.ElementAt(i2);
								if (mov.esDolares) {
									double csDebe2 = (double)((int)(mov.csDebe * 100.0)) / 100.0;
									double csHaber2 = (double)((int)(mov.csHaber * 100.0)) / 100.0;
									if (!mov.csDebeNull && mov.csDebe != csDebe2) {
										mov.csDebe = csDebe2;
										mov.csDebeChanged = true;
									}
									if (!mov.csHaberNull && mov.csHaber != csHaber2) {
										mov.csHaber = csHaber2;
										mov.csHaberChanged = true;
									}
								}
							}
						}
						//Analyze new delta
						{
							csDebeTotal = 0; csHaberTotal = 0;
							int i2; for (i2 = 0; i2 < movs.Count; i2++) {
								MovRecord mov = movs.ElementAt(i2);
								if(!mov.csDebeNull) csDebeTotal += mov.csDebe;
								if(!mov.csHaberNull) csHaberTotal += mov.csHaber;
							}
							csDelta = (csDebeTotal - csHaberTotal);
						}
						//Results
						if (csDelta >= -0.0001 && csDelta <= 0.0001) {
							conteoResuletosTruncando++;
							applyChanges = true;
						} else {
							//Distribute cents
							int conteoCentsDistss = 0;
							if (csDelta <= -0.009 || csDelta >= 0.009) {
								bool applied = true;
								while (applied && (csDelta <= -0.009 || csDelta >= 0.009)) {
									applied = false;
									//Move cent
									{
										int i2; for (i2 = 0; i2 < movs.Count; i2++) {
											MovRecord mov = movs.ElementAt(i2);
											if (mov.esDolares) {
												if (!mov.csDebeNull && mov.csDebe > 0.01) {
													if (csDelta > 0) {
														mov.csDebeChanged = true;
														mov.csDebe -= 0.01;
														csDelta -= 0.01;
														//
														conteoCentsDistss++;
														applied = true;
														break;
													} else {
														mov.csDebeChanged = true;
														mov.csDebe += 0.01;
														csDelta += 0.01;
														//
														conteoCentsDistss++;
														applied = true;
														break;
													}
												} else if (!mov.csHaberNull && mov.csHaber > 0.01) {
													if (csDelta > 0) {
														mov.csHaberChanged = true;
														mov.csHaber += 0.01;
														csDelta -= 0.01;
														//
														conteoCentsDistss++;
														applied = true;
														break;
													} else {
														mov.csHaberChanged = true;
														mov.csHaber -= 0.01;
														csDelta += 0.01;
														//
														conteoCentsDistss++;
														applied = true;
														break;
													}
												}
											}
										}
									}
								}
							}
							//Analyze new delta
							{
								csDebeTotal = 0; csHaberTotal = 0;
								int i2; for (i2 = 0; i2 < movs.Count; i2++) {
									MovRecord mov = movs.ElementAt(i2);
									if (!mov.csDebeNull) csDebeTotal += mov.csDebe;
									if (!mov.csHaberNull) csHaberTotal += mov.csHaber;
								}
								csDelta = (csDebeTotal - csHaberTotal);
							}
							if (csDelta >= -0.0001 && csDelta <= 0.0001) {
								conteoResueltosDistCents++;
								cantCentsDists += conteoCentsDistss;
								applyChanges = true;
							} else {
								conteoNoResuletos++;
							}
						}
						//applicar cambios
						if (applyChanges) {
							//Aplicar cambios
							if (!simulateOnly) {
								int i2; for (i2 = 0; i2 < movs.Count; i2++) {
									MovRecord mov = movs.ElementAt(i2);
									if (mov.csDebeChanged || mov.csHaberChanged) {
										string strSQL = ""; int countSet = 0;
										strSQL += " UPDATE Movimientos SET ";
										if (!mov.csDebeNull && mov.csDebeChanged) {
											if (countSet > 0) strSQL += " , ";
											strSQL += " montoDebeCS = " + String.Format("{0:0.00}", mov.csDebe)  + " ";
											countSet++;
										}
										if (!mov.csHaberNull && mov.csHaberChanged) {
											if (countSet > 0) strSQL += " , ";
											strSQL += " montoHaberCS = " + String.Format("{0:0.00}", mov.csHaber) + " ";
											countSet++;
										}
										if (countSet > 0) {
											strSQL += " WHERE idMovimiento = " + mov.idMov + "";
											//if (i == 0) MessageBox.Show(strSQL);
											OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
											ssql.ExecuteNonQuery();
										}
									}
								}
							}
						} else {
							if (deltasNoResueltos.Length != 0) deltasNoResueltos += ", ";
							deltasNoResueltos += String.Format("{0:0.0000000}", csDelta) + ": " + idComp;
						}
					}
				}
			}
			MessageBox.Show((simulateOnly ? "(SIMULACION)\n\n" : "") + conteoResuletosTruncando + " resueltos truncando decimales,\n" + conteoResueltosDistCents + " resueltos distribuyendo-" + cantCentsDists + "-cents\n" + conteoNoResuletos + " no-resueltos,\n" + conteoDeltasMayores + " deltas mayores a 2c.\n\nDeltas no resueltos: " + deltasNoResueltos);
			//
			return (conteoResuletosTruncando + conteoResueltosDistCents);
		}*/

        private void recalcularBalanceToolStripMenuItem_Click(object sender, EventArgs e) {
			if (Global.actualizarBalanceDeCuentas(null)) {
				MessageBox.Show("Balance de cuentas calculado.");
			} else {
                MessageBox.Show("Cálculo de balance de cuentas ha fallado.");
            }
        }

        private void importarMovimientosToolStripMenuItem_Click(object sender, EventArgs e) {
			FrmImportarMovs frm = new FrmImportarMovs();
			frm.ShowDialog();
        }

        private void actualizadorToolStripMenuItem_Click(object sender, EventArgs e) {
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

        private void salirToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Global.conn != null) {
                Global.conn.Close();
                Global.conn = null;
            }
            if (Global.tunnel != null) {
                Global.tunnel.stopFlag();
                Global.tunnel = null;
            }
			Application.Exit();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e) {
            {
                Form[] frms = this.MdiChildren;
                if (frms != null) {
                    int i; for (i = frms.Length - 1; i >= 0; i--) {
                        Form frm = frms[i];
                        if (frm != null) {
                            frm.Close();
                        }
                    }
                }

            }
            if (Global.conn != null) {
                Global.conn.Close();
                Global.conn = null;
            }
            if (Global.tunnel != null) {
                Global.tunnel.stopFlag();
                Global.tunnel = null;
            }
			{
				FrmSesion ventanaSesion = new FrmSesion();
				ventanaSesion.ShowDialog();
				if (Global.conn == null) {
					Application.Exit();
				} else {
                    this.showContabFrm();
                }
			}
        }

		private void showContabFrm() {
            bool frmFound = false;
            FormCollection frms = Application.OpenForms;
            int i; for (i = 0; i < frms.Count; i++) {
                if (frms[i] != null && frms[i].GetType() == typeof(FrmContab)) {
                    frmFound = true;
                    break;
                }
            }
            //open window
            if (!frmFound) {
                FrmContab frm = new FrmContab();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void contabilidadToolStripMenuItem1_Click(object sender, EventArgs e) {
            this.showContabFrm();
        }
    }
}

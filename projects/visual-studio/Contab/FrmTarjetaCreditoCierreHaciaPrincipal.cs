using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD
using System.Threading.Tasks;

namespace Contab {
	public partial class FrmTarjetaCreditoCierreHaciaPrincipal : Form {
		public FrmTarjetaCreditoCierreHaciaPrincipal() {
			InitializeComponent();
		}

		private void FrmTarjetaCreditoCierreHaciaPrincipal_Load(object sender, EventArgs e) {
			string strSQL = "";
			OdbcCommand ssql = null;
			OdbcDataReader conC = null;
			//Fill credit cards accounts
			{
				lstTarjetas.BeginUpdate();
				cmbTarjetaPrincipal.BeginUpdate();
				lstTarjetas.Items.Clear();
				cmbTarjetaPrincipal.Items.Clear();
				//Cargar las cuentas
				strSQL = "SELECT d.idCuentaDetalle, d.nombreCuentaDetalle ";
				strSQL += ", (SELECT SUM(IIF(ISNULL(d2.montoDebeUS), 0, d2.montoDebeUS) - IIF(ISNULL(d2.montoHaberUS), 0, d2.montoHaberUS)) FROM Movimientos d2 WHERE d2.idCuentaDetalle = d.idCuentaDetalle) as balanceUS";
				strSQL += ", (SELECT SUM(IIF(ISNULL(d2.montoDebeCS), 0, d2.montoDebeCS) - IIF(ISNULL(d2.montoHaberCS), 0, d2.montoHaberCS)) FROM Movimientos d2 WHERE d2.idCuentaDetalle = d.idCuentaDetalle AND esDolares = 0) as balanceCS";
				strSQL += " FROM CuentasDetalle d WHERE d.idCuentaSubSubSub = '02010103' ";
				ssql = new OdbcCommand(strSQL, Global.conn);
				conC = ssql.ExecuteReader();
				while (conC.Read()) {
					string accId = conC.GetString(0);
					string accDesc = conC.GetString(1);
					double balaceUS = conC.GetInt64(2);
					double balaceCS = conC.GetInt64(3);
					//
					cmbTarjetaPrincipal.Items.Add(accId + " - " + accDesc);
					//
					ListViewItem itm = lstTarjetas.Items.Add(accId); itm.Checked = true;
					itm.SubItems.Add(accDesc);
					itm.SubItems.Add(String.Format("{0:0.00}", balaceCS));
					itm.SubItems.Add(String.Format("{0:0.00}", balaceUS));
					itm.SubItems.Add("-");
				}
				conC.Close();
				//
				if (cmbTarjetaPrincipal.Items.Count > 0) { cmbTarjetaPrincipal.SelectedIndex = 0; }
				cmbTarjetaPrincipal.EndUpdate();
				lstTarjetas.EndUpdate();
			}
			//Fill Perdida/Utilidad cambiaria
			{
				{
					cmbCuentaCambPerdida.BeginUpdate();
					strSQL = "SELECT d.idCuentaDetalle, d.nombreCuentaDetalle FROM CuentasDetalle d WHERE d.idCuentaSubSubSub = '06010401' ";
					ssql = new OdbcCommand(strSQL, Global.conn);
					conC = ssql.ExecuteReader();
					int iAcc = -1, i = 0;
					while (conC.Read()) {
						string accId = conC.GetString(0);
						string accDesc = conC.GetString(1);
						cmbCuentaCambPerdida.Items.Add(accId + " - " + accDesc);
						if (accId.Equals("060104010001")) { 
							iAcc = i;
						}
						i++;
					}
					conC.Close();
					if (iAcc == -1) {
						MessageBox.Show("ADVERTENCIA: no se pudo autoseleccionar la cuenta de PERDIDA CAMBIARIA.");
					} else { 
						cmbCuentaCambPerdida.SelectedIndex = iAcc;
					}
					cmbCuentaCambPerdida.EndUpdate();
				}
				{
					cmbCuentaCambUtilidad.BeginUpdate();
					strSQL = "SELECT d.idCuentaDetalle, d.nombreCuentaDetalle FROM CuentasDetalle d WHERE d.idCuentaSubSubSub = '04010401' ";
					ssql = new OdbcCommand(strSQL, Global.conn);
					conC = ssql.ExecuteReader();
					int iAcc = -1, i = 0;
					while (conC.Read()) {
						string accId = conC.GetString(0);
						string accDesc = conC.GetString(1);
						cmbCuentaCambUtilidad.Items.Add(accId + " - " + accDesc);
						if (accId.Equals("040104010001")) {
							iAcc = i;
						}
						i++;
					}
					conC.Close();
					if (iAcc == -1) {
						MessageBox.Show("ADVERTENCIA: no se pudo autoseleccionar la cuenta de UTILIDAD CAMBIARIA.");
					} else {
						cmbCuentaCambUtilidad.SelectedIndex = iAcc;
					}
					cmbCuentaCambUtilidad.EndUpdate();
				}
			}
			//
			this.deseleccionarTarjetaPrincipalDeLista();
		}

		private void deseleccionarTarjetaPrincipalDeLista() {
			if (cmbTarjetaPrincipal.SelectedItem != null) {
				string strSel = cmbTarjetaPrincipal.Text;
				int iSep = strSel.IndexOf(" - ");
				if (iSep > 0) {
					string strId = strSel.Substring(0, iSep);
					if (strId.Length > 0) {
						//Asegurar que la tarjeta principal no esta seleccionada
						int i;
						for (i = 0; i < lstTarjetas.Items.Count; i++) {
							if (lstTarjetas.Items[i].Text.Equals(strId)) {
								if (lstTarjetas.Items[i].Checked) {
									lstTarjetas.Items[i].Checked = false;
								}
							}
						}
					}
				}
			}
		}

		private void cmbTarjetaPrincipal_SelectedIndexChanged(object sender, EventArgs e) {
			this.deseleccionarTarjetaPrincipalDeLista();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void cmbFechaMantValor_ValueChanged(object sender, EventArgs e) {
			//
		}

		private void btnTasaBCN_Click(object sender, EventArgs e) {
			Task<double> t = Global.queryTasaCambio_bcn_gob_niAsync(cmbFechaMantValor.Value);
			t.Wait();
			double tasa = t.Result;
			if (tasa >= 20) {
				txtTasaCambio.Text = String.Format("{0:0.0000}", tasa);
			} else {
				MessageBox.Show("No se pudo obtener la tasa de cambio de la fecha '" + cmbFechaMantValor.Value + "'.");
			}
		}

		private class AccBalance {
			public string accId = "";			//id de cuenta
			public string accName = "";			//nom de cuenta
			public double saldoTotalCS = 0;		//saldo total cordobas y dolares
			//Montos desde el ultimo balance cero
			public double movSoloCS = 0;		//movimientos solo cordobas
			public double movSoloUS = 0;		//movimientos solo dolares
			public double movSoloUSEnCS = 0;	//movimientos solo dolares aplicando su tasa del cambio del momento del movimiento
			//
			public void agregarMov(double montoCS, bool esDolares, double tasaCambio, double montoUS) {
				saldoTotalCS += montoCS;
				if (saldoTotalCS >= -0.01 && saldoTotalCS <= 0.01) {
					//Reset values
					movSoloCS = 0;
					movSoloUS = 0;
					movSoloUSEnCS = 0;
				} else { 
					//Add values
					if (esDolares) {
						movSoloUS += montoUS;
						movSoloUSEnCS += montoCS; //(montoUS * tasaCambio);
					} else {
						movSoloCS += montoCS;
					}
				}
			}
		}

		private class AccBalances {
			public AccBalance[] balances = null;
			public void agregarMov(string accId, string accName, double montoCS, bool esDolares, double tasaCambio, double montoUS) {
				//Init array
				if (balances == null) {
					balances = new AccBalance[1];
					balances[0] = new AccBalance();
					balances[0].accId = accId;
					balances[0].accName = accName;
				}
				//Open new account
				if (!balances[balances.Length - 1].accId.Equals(accId)) {
					AccBalance[] n = new AccBalance[balances.Length + 1];
					int i; for (i = 0; i < balances.Length; i++) n[i] = balances[i];
					n[i] = new AccBalance();
					n[i].accId = accId;
					n[i].accName = accName;
					balances = n;
				}
				//Add values
				balances[balances.Length - 1].agregarMov(montoCS, esDolares, tasaCambio, montoUS);
			}
		}

		private AccBalances calculateBalances(double tasaCambioSel) {
			AccBalances r = null;
			//Procesar
			if (cmbTarjetaPrincipal.SelectedItem != null) {
				string strSel = cmbTarjetaPrincipal.Text;
				int iSep = strSel.IndexOf(" - ");
				if (iSep > 0) {
					string dstId = strSel.Substring(0, iSep);
					if (dstId.Length > 0) {
						//
						string strSQL = "";
						strSQL += " SELECT CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, ";
						strSQL += " ComprobantesDeDiario.idComprobanteDiario, ComprobantesDeDiario.fechaComprobanteDiario, ";
						strSQL += " Movimientos.idMovimiento, Movimientos.montoDebeCS, Movimientos.montoHaberCS, Movimientos.esDolares, Movimientos.tasaCambio, Movimientos.montoDebeUS, Movimientos.montoHaberUS ";
						strSQL += " FROM ((((Cuentas INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
						strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
						strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
						strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub) ";
						strSQL += " INNER JOIN (ComprobantesDeDiario INNER JOIN Movimientos ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario) ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle";
						int i, iSelCount = 0;
						for (i = 0; i < lstTarjetas.Items.Count; i++) {
							if (lstTarjetas.Items[i].Checked) {
								if (lstTarjetas.Items[i].Text.Equals(dstId)) {
									MessageBox.Show("ERROR: la cuenta destino esta seleccionada como origen.");
									return r; //EXIT
								} else {
									if (iSelCount == 0) {
										strSQL += " WHERE CuentasDetalle.idCuentaDetalle = '" + lstTarjetas.Items[i].Text + "'";
									} else {
										strSQL += " OR CuentasDetalle.idCuentaDetalle = '" + lstTarjetas.Items[i].Text + "'";
									}
									iSelCount++;
								}
							}
						}
						strSQL += " ORDER BY CuentasDetalle.idCuentaDetalle, ComprobantesDeDiario.fechaComprobanteDiario, ComprobantesDeDiario.idComprobanteDiario, Movimientos.montoDebeCS DESC, Movimientos.montoHaberCS DESC ";
						//
						AccBalances balances = new AccBalances();
						OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
						OdbcDataReader conC = ssql.ExecuteReader();
						while (conC.Read()) {
							string accId = conC.GetString(0);
							string accName = conC.GetString(1);
							string idComp = conC.GetString(2);
							DateTime fechComp = conC.GetDateTime(3);
							int idMov = conC.GetInt32(4);
							double debeCS = 0; if (!conC.IsDBNull(5)) debeCS = conC.GetInt64(5);
							double haberCS = 0; if (!conC.IsDBNull(6)) haberCS = conC.GetInt64(6);
							bool esDolar = conC.GetBoolean(7);
							double tasa = 0; if (!conC.IsDBNull(8)) tasa = conC.GetInt64(8);
							double debeUS = 0; if (!conC.IsDBNull(9)) debeUS = conC.GetInt64(9);
							double haberUS = 0; if (!conC.IsDBNull(10)) haberUS = conC.GetInt64(10);
							//
							balances.agregarMov(accId, accName, (debeCS - haberCS), esDolar, tasa, (debeUS - haberUS));
						}
						conC.Close();
						r = balances;
					}
				}
			}
			return r;
		}

		private void btnValidar_Click(object sender, EventArgs e) {
			double tasaCambioSel = 20;
			try {
				tasaCambioSel = double.Parse(txtTasaCambio.Text);
			} catch (Exception excp) {
				MessageBox.Show("ERROR: la tasa de cambio ('" + txtTasaCambio.Text + "') no es valida: '" + excp.Message + "'.");
				return; //EXIT
			}
			//
			AccBalances balances = this.calculateBalances(tasaCambioSel);
			if (balances == null) {
				MessageBox.Show("ERROR: los balances no fueron calculados.");
			} else if (balances.balances == null) {
				MessageBox.Show("ERROR: los balances estan vacios.");
			} else {
				int i;
				for (i = 0; i < balances.balances.Length; i++) {
					AccBalance balance = balances.balances[i];
					//MessageBox.Show("Cuenta '" + balance.accId + "' (" + balance.accName + ")\nBalanceUS: U$" + String.Format("{0:0.00}", balance.movSoloUS) + "\nBalanceCS: C$" + String.Format("{0:0.00}", balance.movSoloCS) + "\nPerdida cambiaria: C$" + String.Format("{0:0.00}", (balance.movSoloUS * tasaCambioSel) - balance.movSoloUSEnCS) + "\n");
					int i2;
					for (i2 = 0; i2 < lstTarjetas.Items.Count; i2++) {
						if (lstTarjetas.Items[i2].Text.Equals(balance.accId)) {
							lstTarjetas.Items[i2].SubItems[2].Text = String.Format("{0:0.00}", balance.movSoloCS);
							lstTarjetas.Items[i2].SubItems[3].Text = String.Format("{0:0.00}", balance.movSoloUS);
							lstTarjetas.Items[i2].SubItems[4].Text = String.Format("{0:0.0000}", (balance.movSoloUS * tasaCambioSel) - balance.movSoloUSEnCS);
						}
					}
					MessageBox.Show("Los balances de las cuentas seleccionadas en la tabla han sido calculados y mostrados. Ahora puede revisar y aplicar los valores.");
					btnOK.Enabled = true;
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e) {
			double tasaCambioSel = 20;
			try {
				tasaCambioSel = double.Parse(txtTasaCambio.Text);
			} catch (Exception excp) {
				MessageBox.Show("ERROR: la tasa de cambio ('" + txtTasaCambio.Text + "') no es valida: '" + excp.Message + "'.");
				return; //EXIT
			}
			//
			string accDst = "", accPerdCamb = "", accUtilCamb = "";
			if (cmbTarjetaPrincipal.SelectedItem != null) {
				string strSel = cmbTarjetaPrincipal.Text;
				int iSep = strSel.IndexOf(" - ");
				if (iSep > 0) {
					accDst = strSel.Substring(0, iSep);
				}
			}
			if (cmbCuentaCambPerdida.SelectedItem != null) {
				string strSel = cmbCuentaCambPerdida.Text;
				int iSep = strSel.IndexOf(" - ");
				if (iSep > 0) {
					accPerdCamb = strSel.Substring(0, iSep);
				}
			}
			if (cmbCuentaCambUtilidad.SelectedItem != null) {
				string strSel = cmbCuentaCambUtilidad.Text;
				int iSep = strSel.IndexOf(" - ");
				if (iSep > 0) {
					accUtilCamb = strSel.Substring(0, iSep);
				}
			}
			//
			if (accDst.Length == 0 || accPerdCamb.Length == 0 || accUtilCamb.Length == 0) {
				MessageBox.Show("ERROR: falta especificar las cuentas destinos.");
				return; //EXIT
			}
			//
			AccBalances balances = this.calculateBalances(tasaCambioSel);
			if (balances == null) {
				MessageBox.Show("ERROR: los balances no fueron calculados.");
			} else if (balances.balances == null) {
				MessageBox.Show("ERROR: los balances estan vacios.");
			} else {
				int i;
				for (i = 0; i < balances.balances.Length; i++) {
					AccBalance balance = balances.balances[i];
					string idComprobante = Global.generaSiguienteComprobante(cmbFechaMantValor.Value);
					string strSQL = ""; OdbcCommand ssql = null;
					string strName = "Cierre tarjeta "; if (cmbFechaMantValor.Value.Month < 10) strName += "0"; strName += cmbFechaMantValor.Value.Month; strName += "/" + cmbFechaMantValor.Value.Year;
					//Aplicar perdida/utilidad cambiaria
					double perdCamb = (balance.movSoloUS * tasaCambioSel) - balance.movSoloUSEnCS;
					if (perdCamb < 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accPerdCamb + "' , " + String.Format("{0:0.00}", -perdCamb) + " , NULL , 0 , NULL , NULL , NULL , '" + strName + ". Perdida cambiaria.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "' , NULL, " + String.Format("{0:0.00}", -perdCamb) + " , 0 , NULL , NULL , NULL , '" + strName + ". Perdida cambiaria.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					} else if (perdCamb > 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accUtilCamb + "' , NULL, " + String.Format("{0:0.00}", perdCamb) + ", 0 , NULL , NULL , NULL , '" + strName + ". Utilidad cambiaria.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "' , " + String.Format("{0:0.00}", perdCamb) + ", NULL , 0 , NULL , NULL , NULL , '" + strName + ". Utilidad cambiaria.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					}
					//Aplicar monto cordobas
					if (balance.movSoloCS < 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accDst + "' , NULL , " + String.Format("{0:0.00}", -balance.movSoloCS) + ", 0 , NULL , NULL , NULL , '" + strName + ". Monto cordobas.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "' , " + String.Format("{0:0.00}", -balance.movSoloCS) + ", NULL , 0 , NULL , NULL , NULL , '" + strName + ". Monto cordobas.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					} else if (balance.movSoloCS > 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accDst + "'  , " + String.Format("{0:0.00}", balance.movSoloCS) + ", NULL, 0 , NULL , NULL , NULL , '" + strName + ". Monto cordobas.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "', NULL , " + String.Format("{0:0.00}", balance.movSoloCS) + " , 0 , NULL , NULL , NULL , '" + strName + ". Monto cordobas.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					}
					//Aplicar monto dolares
					if (balance.movSoloUS < 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accDst + "', NULL, " + String.Format("{0:0.00}", -balance.movSoloUS * tasaCambioSel) + ", 1 , " + String.Format("{0:0.00}", tasaCambioSel) + ", NULL , " + String.Format("{0:0.00}", -balance.movSoloUS) + ", '" + strName + ". Monto dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "', " + String.Format("{0:0.00}", -balance.movSoloUS * tasaCambioSel) + ", NULL, 1 , " + String.Format("{0:0.00}", tasaCambioSel) + ", " + String.Format("{0:0.00}", -balance.movSoloUS) + ", NULL, '" + strName + ". Monto dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					} else if (balance.movSoloUS > 0) {
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + accDst + "', " + String.Format("{0:0.00}", balance.movSoloUS * tasaCambioSel) + ", NULL, 1  , " + String.Format("{0:0.00}", tasaCambioSel) + ", " + String.Format("{0:0.00}", balance.movSoloUS) + ", NULL , '" + strName + ". Monto dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + balance.accId + "', NULL, " + String.Format("{0:0.00}", balance.movSoloUS * tasaCambioSel) + ", 1, " + String.Format("{0:0.00}", tasaCambioSel) + ", NULL , " + String.Format("{0:0.00}", balance.movSoloUS) + ", '" + strName + ". Monto dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
					}
					MessageBox.Show("Los balances de las cuentas seleccionadas han sido aplicados en el nuevo comprobante '" + idComprobante + "'.");
					btnOK.Enabled = false;
				}
			}
		}

		

		
	}
}

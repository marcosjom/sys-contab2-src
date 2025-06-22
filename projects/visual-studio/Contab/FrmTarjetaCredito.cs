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
//
namespace Contab {
	public partial class FrmTarjetaCredito : Form {
		public FrmTarjetaCredito() {
			InitializeComponent();
		}

		private void FrmTarjetaCredito_Load(object sender, EventArgs e) {
			cmbTarjetaDetalle.Items.Clear();
			cmbTarjetaPrincipal.Items.Clear();
			//Cargar las cuentas
			OdbcCommand ssql = new OdbcCommand("SELECT idCuentaDetalle, nombreCuentaDetalle FROM CuentasDetalle WHERE idCuentaSubSubSub = '02010103'", Global.conn);
			OdbcDataReader conC = ssql.ExecuteReader();
			while(conC.Read()) {
				string itm = conC.GetString(0) + " - " + conC.GetString(1);
				cmbTarjetaDetalle.Items.Add(itm);
				cmbTarjetaPrincipal.Items.Add(itm);
			}
			conC.Close();
			//
			if (cmbTarjetaDetalle.Items.Count > 0) { cmbTarjetaDetalle.SelectedIndex = cmbTarjetaDetalle.Items.Count - 1; }
			if (cmbTarjetaPrincipal.Items.Count > 0) { cmbTarjetaPrincipal.SelectedIndex = 0; }
		}

		private void btnProcesar_Click(object sender, EventArgs e) {
			float tasaIntUS = 0;
			float mantValor = 0, cargoAdminMora = 0, interesesMoraCS = 0, intereresMoraUS = 0, interesesCS = 0, intereresUS = 0, bonificablesCS = 0, bonificablesUS = 0;
			String cuentaDetalle = "", cuentaPrincipal = "";
			//
			try { tasaIntUS = float.Parse(txtTasaIntereses.Text); } catch { MessageBox.Show("Error: tasa de intereses no es numerica."); return; }
			//
			try { mantValor = float.Parse(txtMantValor.Text); } catch { MessageBox.Show("Error: mantenimiento a valor no es numerico."); return; }
			try { cargoAdminMora = float.Parse(txtCargoAdminMora.Text); } catch { MessageBox.Show("Error: cargo admin por mora no es numerico."); return; }
			try { interesesMoraCS = float.Parse(txtIntMoraCS.Text); } catch { MessageBox.Show("Error:intereses por mora cordobas no es numerico."); return; }
			try { intereresMoraUS = float.Parse(txtIntMoraUS.Text); } catch { MessageBox.Show("Error: intereses por mora dolares no es numerico."); return; }
			try { interesesCS = float.Parse(txtIntCS.Text); } catch { MessageBox.Show("Error:intereses corrientes cordobas no es numerico."); return; }
			try { intereresUS = float.Parse(txtIntUS.Text); } catch { MessageBox.Show("Error: intereses corrientes dolares no es numerico."); return; }
			try { bonificablesCS = float.Parse(txtBonificableCS.Text); } catch { MessageBox.Show("Error: intereses bonificables cordobas no es numerico."); return; }
			try { bonificablesUS = float.Parse(txtBonificableUS.Text); } catch { MessageBox.Show("Error: intereses bonificables dolares no es numerico."); return; }
			//
			cuentaDetalle = cmbTarjetaDetalle.Text; if(cuentaDetalle.Length < 12) cuentaDetalle = ""; else cuentaDetalle = cuentaDetalle.Substring(0, 12);
			cuentaPrincipal = cmbTarjetaPrincipal.Text; if(cuentaPrincipal.Length < 12) cuentaPrincipal = ""; else cuentaPrincipal = cuentaPrincipal.Substring(0, 12);
			if (cuentaDetalle.Length != 12) { MessageBox.Show("Error: cuenta detalle '"+cuentaDetalle+"' no es valida."); return; }
			if (cuentaPrincipal.Length != 12) { MessageBox.Show("Error: cuenta principal '" + cuentaPrincipal + "' no es valida."); return; }
			//
			if ((intereresUS > 0.0 || bonificablesUS > 0.0) && tasaIntUS < 24.0) { MessageBox.Show("Error: tasa de cambio de intereses en dolares no es valida."); return; }
			//
			if (mantValor < 0.0f) { MessageBox.Show("Error: mantenimiento al valor es negativo."); return; }
			if (cargoAdminMora < 0.0f) { MessageBox.Show("Error: cargo administrativo por mora es negativo."); return; }
			if (interesesMoraCS < 0.0f) { MessageBox.Show("Error: intereses por mora en cordobas es negativo."); return; }
			if (intereresMoraUS < 0.0f) { MessageBox.Show("Error: intereses por mora en dolares es negativo."); return; }
			if (interesesCS < 0.0f) { MessageBox.Show("Error: intereses corrientes en cordobas es negativo."); return; }
			if (intereresUS < 0.0f) { MessageBox.Show("Error: intereses corrientes en dolares es negativo."); return; }
			if (bonificablesCS < 0.0f) { MessageBox.Show("Error: intereses bonificables en cordobas es negativo."); return; }
			if (bonificablesUS < 0.0f) { MessageBox.Show("Error: intereses bonificables en dolares es negativo."); return; }
			//
			if (MessageBox.Show("Verificacion de formatos exitosa. Desea registrar los comprobantes?", "Registrar comprobantes?", MessageBoxButtons.OKCancel) == DialogResult.OK) {
				OdbcCommand ssql;
				string mensaje = "";
				//Mantenimiento al valor
				if (mantValor > 0.0 || cargoAdminMora > 0.0 || interesesMoraCS > 0 || intereresMoraUS > 0 || interesesCS > 0.0 || intereresUS > 0.0 || bonificablesCS > 0.0f || bonificablesUS > 0.0f) {
					string idComprobante = Global.generaSiguienteComprobante(cmbFechaMantValor.Value);
					mensaje += "Se genero comprobante '" + idComprobante + "'.\n";
					if (mantValor > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '060103010004' , " + mantValor + " , NULL , 0 , NULL , NULL , NULL , 'Mantenimiento al valor.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaPrincipal + "' , NULL, " + mantValor + " , 0 , NULL , NULL , NULL , 'Mantenimiento al valor.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo mantenimiento al valor.\n";
					} else {
						mensaje += "Se EXCLUYO mantenimiento al valor.\n";
					}
					if (cargoAdminMora > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '070101010003' , " + cargoAdminMora + " , NULL , 0 , NULL , NULL , NULL , 'Cargo administrativo por mora TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + cargoAdminMora + " , 0 , NULL , NULL , NULL , 'Cargo administrativo por mora TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo cargo admin por mora.\n";
					} else {
						mensaje += "Se EXCLUYO cargo admin por mora.\n";
					}
					if (interesesMoraCS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '070101010003' , " + interesesMoraCS + " , NULL , 0 , NULL , NULL , NULL , 'Intereses por mora saldo local TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + interesesMoraCS + " , 0 , NULL , NULL , NULL , 'Intereses por mora saldo local TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses por mora C$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses por mora C$.\n";
					}
					if (intereresMoraUS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '070101010003' , " + (tasaIntUS * intereresMoraUS) + " , NULL , 1, " + tasaIntUS + " , " + intereresMoraUS + ", NULL, 'Intereses por mora saldo dolares TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + (tasaIntUS * intereresMoraUS) + " , 1, " + tasaIntUS + ", NULL, " + intereresMoraUS + " , 'Intereses por mora saldo dolares TC.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses por mora U$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses por mora U$.\n";
					}
					if (interesesCS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '060103010006' , " + interesesCS + " , NULL , 0 , NULL , NULL , NULL , 'Intereses corrientes saldo local.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + interesesCS + " , 0 , NULL , NULL , NULL , 'Intereses corrientes saldo local.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses corrientes C$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses corrientes C$.\n";
					}
					if (intereresUS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '060103010006' , " + (tasaIntUS * intereresUS) + " , NULL , 1, " + tasaIntUS + " , " + intereresUS + ", NULL, 'Intereses corrientes saldo dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + (tasaIntUS * intereresUS) + " , 1, " + tasaIntUS + ", NULL, " + intereresUS + " , 'Intereses corrientes saldo dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses corrientes U$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses corrientes U$.\n";
					}
					if (bonificablesCS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '060103010006' , " + bonificablesCS + " , NULL , 0 , NULL , NULL , NULL , 'Intereses bonificables local.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + bonificablesCS + " , 0 , NULL , NULL , NULL , 'Intereses bonificables local.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses bonificables C$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses bonificables C$.\n";
					}
					if (bonificablesUS > 0.0) {
						string strSQL = "";
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '060103010006' , " + (tasaIntUS * bonificablesUS) + " , NULL , 1, " + tasaIntUS + ", " + bonificablesUS + ", NULL, 'Intereses bonificables dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						//
						strSQL = " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
						strSQL += " VALUES ('" + idComprobante + "' , '" + cuentaDetalle + "' , NULL, " + (tasaIntUS * bonificablesUS) + " , 1, " + tasaIntUS + ", NULL, " + bonificablesUS + " , 'Intereses bonificables dolares.')";
						ssql = new OdbcCommand(strSQL, Global.conn);
						ssql.ExecuteNonQuery();
						mensaje += "Se incluyo intereses bonificables U$.\n";
					} else {
						mensaje += "Se EXCLUYO intereses bonificables U$.\n";
					}
				}
				if (mensaje.Length > 0) {
					MessageBox.Show(mensaje);
					this.Close();
				} else {
					MessageBox.Show("No se realizo niguna operacion.");
				}
			}
		}

		private void btnTasaBCN_Click(object sender, EventArgs e) {
            Task<double> t = Global.queryTasaCambio_bcn_gob_niAsync(cmbFechaMantValor.Value);
			t.Wait();
			double tasa = t.Result;
			if (tasa >= 20) {
				txtTasaIntereses.Text = String.Format("{0:0.0000}", tasa);
			} else {
				MessageBox.Show("No se pudo obtener la tasa de cambio de la fecha '" + cmbFechaMantValor.Value + "'.");
			}
		}


	}
}

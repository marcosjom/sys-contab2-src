using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD

namespace Contab {
	public partial class FrmNuevoEmpleado : Form {
		public FrmNuevoEmpleado() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			this.Hide();
		}

		private void btnRegistrar_Click(object sender, EventArgs e) {
			try {
				string nombres = txtNombres.Text;
				string apellidos = txtApellidos.Text;
				double salario = double.Parse(txtSalario.Text);
				//
				OdbcCommand ssql = new OdbcCommand("SELECT idEmpleado FROM Empleados WHERE nombres LIKE '" + nombres + "' AND apellidos LIKE '" + apellidos + "'", Global.conn);
				OdbcDataReader conC = ssql.ExecuteReader();
				if (conC.Read()) {
					MessageBox.Show("Ya existe un empleado con esa combinacion de nombres y apellidos.");
				} else {
					/*OdbcCommand ssql2 = null;
					OdbcDataReader conC2 = null;
					//
					int maxIdEmpleado = 100000;
					ssql2 = new OdbcCommand("SELECT MAX(idEmpleado) FROM Empleados", Global.conn);
					conC2 = ssql2.ExecuteReader();
					if (conC2.Read()){
						if (!conC2.IsDBNull(0)) {
							maxIdEmpleado = conC2.GetInt32(0);
						}
					}
					//Cuenta de salario
					int maxNumCuentaSalario = 0;
					ssql2 = new OdbcCommand("SELECT MAX(numCuentaDetalle) FROM CuentasDetalles WHERE idCuentaSubSubSub='02010301'", Global.conn);
					conC2 = ssql2.ExecuteReader();
					if (conC2.Read()){
						if (!conC2.IsDBNull(0)) {
							maxNumCuentaSalario = conC2.GetInt32(0);
						}
					}
					//Cuenta de vacaciones
					int maxNumCuentaSalario = 0;
					ssql2 = new OdbcCommand("SELECT MAX(numCuentaDetalle) FROM CuentasDetalles WHERE idCuentaSubSubSub='02010301'", Global.conn);
					conC2 = ssql2.ExecuteReader();
					if (conC2.Read()){
						if (!conC2.IsDBNull(0)) {
							maxNumCuentaSalario = conC2.GetInt32(0);
						}
					}
					
					//
					OdbcCommand ssql3 = new OdbcCommand("INSERT INTO Empleados(idEmpleado, nombres, apellidos, salarioMensual, idCuentaDetalleSueldo, idCuentaDetalleVacaciones, idCuentaDetalleAguinaldo, idCuentaDetalleIndemnizacion)", Global.conn);
					 */ 
					this.Hide();
				}
				conC.Close();
			} catch(Exception){
				MessageBox.Show("Ha ocurrido un error. Verifique los datos.");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Contab {
    public partial class FrmMovNomina : Form{

		private bool _cancelada = false;

        public FrmMovNomina(){
            InitializeComponent();
			_cancelada = false;
        }

        private void txtSalario_TextChanged(object sender, EventArgs e){
			if (btnManual.Enabled) {
				try {
					double salario = double.Parse(txtSalario.Text);
					double INSSLaboral = salario * 0.0625;
					double INSSPatronal = salario * 0.16;
					double INATEC = salario * 0.02;
					double vacaciones = salario / 12;
					double aguinaldo = salario / 12;
					double indemnizacion = salario / 12;
					double IRlaboral = 0;
					//Calculo del IR
					double salarioAnual = (salario - INSSLaboral) * 12;
					double IRBase = 0;
					double porcentajeExcedente = 0;
					double exceso = 0;
					if (cmbTablaRetencion.SelectedIndex != 0) {
						//Tabla progresiva pre-2013
						if (salarioAnual <= 75000) {
							IRBase = 0; porcentajeExcedente = 0; exceso = salarioAnual;
						} else if (salarioAnual > 75000 && salarioAnual <= 100000) {
							IRBase = 0; porcentajeExcedente = 0.1; exceso = salarioAnual - 75000;
						} else if (salarioAnual > 100000 && salarioAnual <= 200000) {
							IRBase = 2500; porcentajeExcedente = 0.15; exceso = salarioAnual - 100000;
						} else if (salarioAnual > 200000 && salarioAnual <= 300000) {
							IRBase = 17500; porcentajeExcedente = 0.2; exceso = salarioAnual - 200000;
						} else if (salarioAnual > 300000 && salarioAnual <= 500000) {
							IRBase = 37500; porcentajeExcedente = 0.25; exceso = salarioAnual - 300000;
						} else if (salarioAnual > 500000) {
							IRBase = 87500; porcentajeExcedente = 0.3; exceso = salarioAnual - 500000;
						}
					} else {
						//Tabla progresiva 2013
						if (salarioAnual <= 100000) {
							IRBase = 0; porcentajeExcedente = 0; exceso = salarioAnual;
						} else if (salarioAnual > 100000 && salarioAnual <= 200000) {
							IRBase = 0; porcentajeExcedente = 0.15; exceso = salarioAnual - 100000;
						} else if (salarioAnual > 200000 && salarioAnual <= 350000) {
							IRBase = 15000; porcentajeExcedente = 0.2; exceso = salarioAnual - 200000;
						} else if (salarioAnual > 350000 && salarioAnual <= 500000) {
							IRBase = 45000; porcentajeExcedente = 0.25; exceso = salarioAnual - 350000;
						} else if (salarioAnual > 500000) {
							IRBase = 82500; porcentajeExcedente = 0.3; exceso = salarioAnual - 500000;
						}
					}
					IRlaboral = (IRBase + (exceso * porcentajeExcedente)) / 12;
					//llenar cuadros de texto
					txtINSSLaboral.Text = String.Format("{0:0.00}", INSSLaboral);
					txtIRLaboral.Text = String.Format("{0:0.00}", IRlaboral);
					txtINSSPatronal.Text = String.Format("{0:0.00}", INSSPatronal);
					txtINATEC.Text = String.Format("{0:0.00}", INATEC);
					txtVacaciones.Text = String.Format("{0:0.00}", vacaciones);
					txtAguinaldo.Text = String.Format("{0:0.00}", aguinaldo);
					txtIndemnizacion.Text = String.Format("{0:0.00}", indemnizacion);
				} catch (Exception) {
					txtINSSLaboral.Text = "0.00";
					txtIRLaboral.Text = "0.00";
					txtINSSPatronal.Text = "0.00";
					txtINATEC.Text = "0.00";
					txtVacaciones.Text = "0.00";
					txtAguinaldo.Text = "0.00";
					txtIndemnizacion.Text = "0.00";
				}
			}
        }

		public bool cancelada() {
			return _cancelada;
		}

		public string descripcion () {
			return txtDescripcion.Text;
		}

		public void dameValor(int indice, ref string guardaCuentaEn, ref double guardaValorCordobasEn, ref bool guardarEsDebeEn){
			if (indice ==0 ){//06-Sueldos
				guardaCuentaEn = "060101010001"; guardaValorCordobasEn = (txtSalario.Text == "" ? 0 : double.Parse(txtSalario.Text)); guardarEsDebeEn = true;
			} else if (indice == 1) {//06-Inss Patronal 
				guardaCuentaEn = "060101010006"; guardaValorCordobasEn = (txtINSSPatronal.Text == "" ? 0 : double.Parse(txtINSSPatronal.Text)); guardarEsDebeEn = true;
			} else if (indice == 2) {//06-Inatec
				guardaCuentaEn = "060101010007"; guardaValorCordobasEn = (txtINATEC.Text == "" ? 0 : double.Parse(txtINATEC.Text)); guardarEsDebeEn = true;
			} else if (indice == 3){//06-Vacaciones
				guardaCuentaEn = "060101010003"; guardaValorCordobasEn = (txtVacaciones.Text == "" ? 0 : double.Parse(txtVacaciones.Text)); guardarEsDebeEn = true;
			} else if (indice == 4){//06-Aguinaldos
				guardaCuentaEn = "060101010004"; guardaValorCordobasEn = (txtAguinaldo.Text == "" ? 0 : double.Parse(txtAguinaldo.Text)); guardarEsDebeEn = true;
			} else if (indice == 5){//06-Indemnizacion
				guardaCuentaEn = "060101010005"; guardaValorCordobasEn = (txtIndemnizacion.Text == "" ? 0 :double.Parse(txtIndemnizacion.Text)); guardarEsDebeEn = true;
			} else if (indice == 6){//02-Sueldos
				guardaCuentaEn = "020103010001"; guardaValorCordobasEn = (txtSalario.Text == "" ? 0 : double.Parse(txtSalario.Text)) - (txtINSSLaboral.Text == "" ? 0 : double.Parse(txtINSSLaboral.Text)) - (txtIRLaboral.Text == "" ? 0 : double.Parse(txtIRLaboral.Text)); guardarEsDebeEn = false;
			} else if (indice == 7) {//02-Inss Laboral
				guardaCuentaEn = "020102020003"; guardaValorCordobasEn = (txtINSSLaboral.Text == "" ? 0 : double.Parse(txtINSSLaboral.Text)); guardarEsDebeEn = false;
			} else if (indice == 8) {//02-I/R Empleados
				guardaCuentaEn = "020102020001"; guardaValorCordobasEn = (txtIRLaboral.Text == "" ? 0 : double.Parse(txtIRLaboral.Text)); guardarEsDebeEn = false;
			} else if (indice == 9) {//02-Inss Patronal 
				guardaCuentaEn = "020103010005"; guardaValorCordobasEn = (txtINSSPatronal.Text == "" ? 0 : double.Parse(txtINSSPatronal.Text)); guardarEsDebeEn = false;
			} else if (indice == 10) {//02-Inatec
				guardaCuentaEn = "020103010006"; guardaValorCordobasEn = (txtINATEC.Text == "" ? 0 : double.Parse(txtINATEC.Text)); guardarEsDebeEn = false;
			} else if (indice == 11){//02-Vacaciones
				guardaCuentaEn = "020103010002"; guardaValorCordobasEn = (txtVacaciones.Text == "" ? 0 :double.Parse(txtVacaciones.Text)); guardarEsDebeEn = false;
			} else if (indice == 12){//02-Aguinaldo
				guardaCuentaEn = "020103010003"; guardaValorCordobasEn = (txtAguinaldo.Text == "" ? 0 : double.Parse(txtAguinaldo.Text)); guardarEsDebeEn = false;
			} else if (indice == 13){//02-Indemnizacion
				guardaCuentaEn = "020103010004"; guardaValorCordobasEn = (txtIndemnizacion.Text == "" ? 0 : double.Parse(txtIndemnizacion.Text)); guardarEsDebeEn = false;
			} else {
				guardaCuentaEn = ""; guardaValorCordobasEn = 0; guardarEsDebeEn = false;
			}
		}

		private void btnCancelar_Click(object sender, EventArgs e) {
			_cancelada = true;
			this.Hide();
		}

		private void btnOK_Click(object sender, EventArgs e) {
			_cancelada = false;
			this.Hide();
		}

        private void cmbTablaRetencion_SelectedIndexChanged(object sender, EventArgs e) {
            txtSalario_TextChanged(null, null);
        }

        private void FrmMovNomina_Load(object sender, EventArgs e) {
            cmbTablaRetencion.SelectedIndex = 0;
        }

		private void btnManual_Click(object sender, EventArgs e) {
			btnManual.Enabled = false;
			txtINSSLaboral.Enabled = true;
			txtIRLaboral.Enabled = true;
			txtINSSPatronal.Enabled = true;
			txtINATEC.Enabled = true;
			txtVacaciones.Enabled = true;
			txtAguinaldo.Enabled = true;
			txtIndemnizacion.Enabled = true;
		}

    }
}

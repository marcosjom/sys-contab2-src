using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Contab {
    public partial class FrmServiciosBasicos : Form {

        public bool isCanceled = false;
        public Decimal agua = 0;
        public Decimal energia = 0;
        public Decimal internet = 0;

        public FrmServiciosBasicos() {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            isCanceled = true;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            bool errFnd = false;
            //agua
            if (!errFnd) {
                if (txtAgua.Text.Length <= 0) {
                    agua = 0;
                } else {
                    try {
                        agua = Math.Round(Decimal.Parse(txtAgua.Text), 2);
                    } catch (Exception) {
                        MessageBox.Show("Agua no es valida.");
                        errFnd = true;
                    }
                }
            }
            //energia
            if (!errFnd) {
                if (txtEnergia.Text.Length <= 0) {
                    energia = 0;
                } else {
                    try {
                        energia = Math.Round(Decimal.Parse(txtEnergia.Text), 2);
                    } catch (Exception) {
                        MessageBox.Show("Energia no es valida.");
                        errFnd = true;
                    }
                }
            }
            //internet
            if (!errFnd) {
                if (txtInternet.Text.Length <= 0) {
                    internet = 0;
                } else {
                    try {
                        internet = Math.Round(Decimal.Parse(txtInternet.Text), 2);
                    } catch (Exception) {
                        MessageBox.Show("Internet no es valida.");
                        errFnd = true;
                    }
                }
            }
            if (!errFnd) {
                isCanceled = false;
                this.Close();
            }
        }
    }
}

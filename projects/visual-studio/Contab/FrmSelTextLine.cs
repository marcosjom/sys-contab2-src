using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contab {
    public partial class FrmSelTextLine : Form {

        public string textSel = null;

        public FrmSelTextLine() {
            InitializeComponent();
            //focus
            txtSel.Focus();
        }

        public FrmSelTextLine(string title, string startTextSel) {
            InitializeComponent();
            //
            if (title != null && title.Length > 0) this.Text = title;
            if (startTextSel != null && startTextSel.Length > 0) txtSel.Text = startTextSel.Replace("\r", "").Replace("\n", "");
            //focus
            txtSel.Focus();
        }


        private void btnOk_Click(object sender, EventArgs e) {
            textSel = txtSel.Text;
            this.Close();
        }

        private void txtSel_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 10 || e.KeyChar == 13) {
                btnOk_Click(null, null);
            }
        }
    }
}

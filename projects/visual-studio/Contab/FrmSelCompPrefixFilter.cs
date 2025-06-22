using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Contab {
    public partial class FrmSelCompPrefixFilter : Form {

        public bool actionConfirmed = false;
        public String selPrefix = "";

        public FrmSelCompPrefixFilter() {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e) {
            actionConfirmed = true;
            selPrefix = txtPrefix.Text;
            this.Close();
        }
    }
}

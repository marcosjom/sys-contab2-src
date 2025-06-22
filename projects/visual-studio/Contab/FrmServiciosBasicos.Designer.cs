
namespace Contab {
    partial class FrmServiciosBasicos {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServiciosBasicos));
            this.lblAgua = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblInternet = new System.Windows.Forms.Label();
            this.txtAgua = new System.Windows.Forms.TextBox();
            this.txtEnergia = new System.Windows.Forms.TextBox();
            this.txtInternet = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAgua
            // 
            this.lblAgua.AutoSize = true;
            this.lblAgua.Location = new System.Drawing.Point(52, 20);
            this.lblAgua.Name = "lblAgua";
            this.lblAgua.Size = new System.Drawing.Size(35, 13);
            this.lblAgua.TabIndex = 0;
            this.lblAgua.Text = "Agua:";
            this.lblAgua.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Energia:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInternet
            // 
            this.lblInternet.AutoSize = true;
            this.lblInternet.Location = new System.Drawing.Point(41, 77);
            this.lblInternet.Name = "lblInternet";
            this.lblInternet.Size = new System.Drawing.Size(46, 13);
            this.lblInternet.TabIndex = 2;
            this.lblInternet.Text = "Internet:";
            this.lblInternet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAgua
            // 
            this.txtAgua.Location = new System.Drawing.Point(94, 17);
            this.txtAgua.MaxLength = 10;
            this.txtAgua.Name = "txtAgua";
            this.txtAgua.Size = new System.Drawing.Size(100, 20);
            this.txtAgua.TabIndex = 3;
            this.txtAgua.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtEnergia
            // 
            this.txtEnergia.Location = new System.Drawing.Point(94, 47);
            this.txtEnergia.MaxLength = 10;
            this.txtEnergia.Name = "txtEnergia";
            this.txtEnergia.Size = new System.Drawing.Size(100, 20);
            this.txtEnergia.TabIndex = 4;
            this.txtEnergia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtInternet
            // 
            this.txtInternet.Location = new System.Drawing.Point(94, 74);
            this.txtInternet.MaxLength = 10;
            this.txtInternet.Name = "txtInternet";
            this.txtInternet.Size = new System.Drawing.Size(100, 20);
            this.txtInternet.TabIndex = 5;
            this.txtInternet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(119, 111);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(38, 111);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmServiciosBasicos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 157);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtInternet);
            this.Controls.Add(this.txtEnergia);
            this.Controls.Add(this.txtAgua);
            this.Controls.Add(this.lblInternet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAgua);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmServiciosBasicos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmServiciosBasicos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAgua;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInternet;
        private System.Windows.Forms.TextBox txtAgua;
        private System.Windows.Forms.TextBox txtEnergia;
        private System.Windows.Forms.TextBox txtInternet;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
namespace Contab {
	partial class FrmSesion {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSesion));
            this.tmrAsyncFrm = new System.Windows.Forms.Timer(this.components);
            this.picLogo128 = new System.Windows.Forms.PictureBox();
            this.toolTipFrm = new System.Windows.Forms.ToolTip(this.components);
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.grpLogin = new System.Windows.Forms.Panel();
            this.lblResultado = new System.Windows.Forms.Label();
            this.cmbDbName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.picConnPlain = new System.Windows.Forms.PictureBox();
            this.picConnSecure = new System.Windows.Forms.PictureBox();
            this.grpSync = new System.Windows.Forms.Panel();
            this.lblServerHelp = new System.Windows.Forms.Label();
            this.btnServerDownload = new System.Windows.Forms.Button();
            this.picServerSel = new System.Windows.Forms.PictureBox();
            this.lblServerSel = new System.Windows.Forms.Label();
            this.lnkUpdateAvailable = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo128)).BeginInit();
            this.grpLogin.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnPlain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnSecure)).BeginInit();
            this.grpSync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServerSel)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrAsyncFrm
            // 
            this.tmrAsyncFrm.Enabled = true;
            this.tmrAsyncFrm.Interval = 250;
            this.tmrAsyncFrm.Tick += new System.EventHandler(this.tmrAsyncFrm_Tick);
            // 
            // picLogo128
            // 
            this.picLogo128.Image = ((System.Drawing.Image)(resources.GetObject("picLogo128.Image")));
            this.picLogo128.Location = new System.Drawing.Point(7, 31);
            this.picLogo128.Name = "picLogo128";
            this.picLogo128.Size = new System.Drawing.Size(128, 128);
            this.picLogo128.TabIndex = 15;
            this.picLogo128.TabStop = false;
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(4, 6);
            this.cmbServer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(341, 21);
            this.cmbServer.TabIndex = 19;
            this.cmbServer.TextChanged += new System.EventHandler(this.cmbServer_TextChanged);
            this.cmbServer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbServer_KeyPress);
            // 
            // grpLogin
            // 
            this.grpLogin.Controls.Add(this.lblResultado);
            this.grpLogin.Controls.Add(this.cmbDbName);
            this.grpLogin.Controls.Add(this.label4);
            this.grpLogin.Controls.Add(this.btnIniciar);
            this.grpLogin.Controls.Add(this.txtContrasena);
            this.grpLogin.Controls.Add(this.txtAlias);
            this.grpLogin.Controls.Add(this.label3);
            this.grpLogin.Controls.Add(this.label2);
            this.grpLogin.Controls.Add(this.panel1);
            this.grpLogin.Location = new System.Drawing.Point(21, 40);
            this.grpLogin.Margin = new System.Windows.Forms.Padding(2);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(324, 149);
            this.grpLogin.TabIndex = 20;
            // 
            // lblResultado
            // 
            this.lblResultado.ForeColor = System.Drawing.Color.Red;
            this.lblResultado.Location = new System.Drawing.Point(3, 122);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(167, 20);
            this.lblResultado.TabIndex = 24;
            this.lblResultado.Text = "...";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDbName
            // 
            this.cmbDbName.FormattingEnabled = true;
            this.cmbDbName.Items.AddRange(new object[] {
            "contab",
            "contab_test"});
            this.cmbDbName.Location = new System.Drawing.Point(173, 39);
            this.cmbDbName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbDbName.Name = "cmbDbName";
            this.cmbDbName.Size = new System.Drawing.Size(145, 21);
            this.cmbDbName.TabIndex = 27;
            this.cmbDbName.Text = "contab";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(124, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 18);
            this.label4.TabIndex = 26;
            this.label4.Text = "Datos:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(173, 120);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(145, 24);
            this.btnIniciar.TabIndex = 21;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // txtContrasena
            // 
            this.txtContrasena.Location = new System.Drawing.Point(173, 95);
            this.txtContrasena.MaxLength = 25;
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.PasswordChar = '*';
            this.txtContrasena.Size = new System.Drawing.Size(145, 20);
            this.txtContrasena.TabIndex = 20;
            this.txtContrasena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtContrasena_KeyPress);
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(173, 68);
            this.txtAlias.MaxLength = 25;
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(145, 20);
            this.txtAlias.TabIndex = 19;
            this.txtAlias.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAlias_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(124, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Clave:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(124, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "Usuario:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblConnStatus);
            this.panel1.Controls.Add(this.picConnPlain);
            this.panel1.Controls.Add(this.picConnSecure);
            this.panel1.Location = new System.Drawing.Point(124, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 27);
            this.panel1.TabIndex = 25;
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.Location = new System.Drawing.Point(19, 3);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(150, 23);
            this.lblConnStatus.TabIndex = 2;
            this.lblConnStatus.Text = "Conexion segura";
            this.lblConnStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblConnStatus.MouseHover += new System.EventHandler(this.lblConnStatus_MouseHover);
            // 
            // picConnPlain
            // 
            this.picConnPlain.Image = global::Contab.Properties.Resources.admiracion16;
            this.picConnPlain.Location = new System.Drawing.Point(175, 5);
            this.picConnPlain.Name = "picConnPlain";
            this.picConnPlain.Size = new System.Drawing.Size(16, 16);
            this.picConnPlain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picConnPlain.TabIndex = 1;
            this.picConnPlain.TabStop = false;
            this.picConnPlain.MouseHover += new System.EventHandler(this.picConnPlain_MouseHover);
            // 
            // picConnSecure
            // 
            this.picConnSecure.Image = global::Contab.Properties.Resources.candado16;
            this.picConnSecure.Location = new System.Drawing.Point(175, 5);
            this.picConnSecure.Name = "picConnSecure";
            this.picConnSecure.Size = new System.Drawing.Size(16, 16);
            this.picConnSecure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picConnSecure.TabIndex = 0;
            this.picConnSecure.TabStop = false;
            this.picConnSecure.MouseHover += new System.EventHandler(this.picConnSecure_MouseHover);
            // 
            // grpSync
            // 
            this.grpSync.Controls.Add(this.lblServerHelp);
            this.grpSync.Controls.Add(this.btnServerDownload);
            this.grpSync.Location = new System.Drawing.Point(139, 36);
            this.grpSync.Name = "grpSync";
            this.grpSync.Size = new System.Drawing.Size(205, 124);
            this.grpSync.TabIndex = 21;
            // 
            // lblServerHelp
            // 
            this.lblServerHelp.Location = new System.Drawing.Point(3, 37);
            this.lblServerHelp.Name = "lblServerHelp";
            this.lblServerHelp.Size = new System.Drawing.Size(201, 49);
            this.lblServerHelp.TabIndex = 1;
            this.lblServerHelp.Text = "Establezca el url del servidor de configuración para descargar los datos iniciale" +
    "s.";
            this.lblServerHelp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnServerDownload
            // 
            this.btnServerDownload.Location = new System.Drawing.Point(3, 6);
            this.btnServerDownload.Name = "btnServerDownload";
            this.btnServerDownload.Size = new System.Drawing.Size(201, 23);
            this.btnServerDownload.TabIndex = 0;
            this.btnServerDownload.Text = "Descargar configuración";
            this.btnServerDownload.UseVisualStyleBackColor = true;
            this.btnServerDownload.Click += new System.EventHandler(this.btnServerDownload_Click);
            // 
            // picServerSel
            // 
            this.picServerSel.Image = global::Contab.Properties.Resources.estado_proceso;
            this.picServerSel.Location = new System.Drawing.Point(325, 8);
            this.picServerSel.Name = "picServerSel";
            this.picServerSel.Size = new System.Drawing.Size(16, 16);
            this.picServerSel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picServerSel.TabIndex = 22;
            this.picServerSel.TabStop = false;
            this.picServerSel.Visible = false;
            this.picServerSel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picServerSel_MouseClick);
            // 
            // lblServerSel
            // 
            this.lblServerSel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblServerSel.Location = new System.Drawing.Point(6, 7);
            this.lblServerSel.Name = "lblServerSel";
            this.lblServerSel.Size = new System.Drawing.Size(315, 20);
            this.lblServerSel.TabIndex = 23;
            this.lblServerSel.Text = "...";
            this.lblServerSel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblServerSel.Visible = false;
            this.lblServerSel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblServerSel_MouseClick);
            // 
            // lnkUpdateAvailable
            // 
            this.lnkUpdateAvailable.AutoSize = true;
            this.lnkUpdateAvailable.Location = new System.Drawing.Point(202, 194);
            this.lnkUpdateAvailable.Name = "lnkUpdateAvailable";
            this.lnkUpdateAvailable.Size = new System.Drawing.Size(132, 13);
            this.lnkUpdateAvailable.TabIndex = 24;
            this.lnkUpdateAvailable.TabStop = true;
            this.lnkUpdateAvailable.Text = "Actualizacion disponible ...";
            this.lnkUpdateAvailable.Visible = false;
            this.lnkUpdateAvailable.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdateAvailable_LinkClicked);
            // 
            // FrmSesion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 215);
            this.Controls.Add(this.lnkUpdateAvailable);
            this.Controls.Add(this.lblServerSel);
            this.Controls.Add(this.picServerSel);
            this.Controls.Add(this.picLogo128);
            this.Controls.Add(this.grpLogin);
            this.Controls.Add(this.grpSync);
            this.Controls.Add(this.cmbServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSesion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Contab - sesión";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo128)).EndInit();
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnPlain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnSecure)).EndInit();
            this.grpSync.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picServerSel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.PictureBox picLogo128;
        private System.Windows.Forms.Timer tmrAsyncFrm;
        private System.Windows.Forms.ToolTip toolTipFrm;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.Panel grpLogin;
        private System.Windows.Forms.ComboBox cmbDbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.TextBox txtAlias;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.PictureBox picConnPlain;
        private System.Windows.Forms.PictureBox picConnSecure;
        private System.Windows.Forms.Panel grpSync;
        private System.Windows.Forms.Button btnServerDownload;
        private System.Windows.Forms.Label lblServerHelp;
        private System.Windows.Forms.PictureBox picServerSel;
        private System.Windows.Forms.Label lblServerSel;
        private System.Windows.Forms.LinkLabel lnkUpdateAvailable;
    }
}
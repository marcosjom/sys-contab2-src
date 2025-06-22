namespace Contab {
	partial class FrmImportarMovs {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImportarMovs));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTasa = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkSonDolares = new System.Windows.Forms.CheckBox();
            this.chkTasaUsarBCN = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDatos = new System.Windows.Forms.TextBox();
            this.lblPosDebe = new System.Windows.Forms.Label();
            this.lblPosHaber = new System.Windows.Forms.Label();
            this.lblNegDebe = new System.Windows.Forms.Label();
            this.lblNegHaber = new System.Windows.Forms.Label();
            this.icoPosDebe = new System.Windows.Forms.PictureBox();
            this.icoPosHaber = new System.Windows.Forms.PictureBox();
            this.icoNegDebe = new System.Windows.Forms.PictureBox();
            this.icoNegHaber = new System.Windows.Forms.PictureBox();
            this.btnValidar = new System.Windows.Forms.Button();
            this.toolTipFrm = new System.Windows.Forms.ToolTip(this.components);
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.icoPosDebe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoPosHaber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoNegDebe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoNegHaber)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(287, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Movimientos positivos:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cuenta debe:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(293, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Cuenta haber:";
            // 
            // txtTasa
            // 
            this.txtTasa.Enabled = false;
            this.txtTasa.Location = new System.Drawing.Point(145, 33);
            this.txtTasa.MaxLength = 7;
            this.txtTasa.Name = "txtTasa";
            this.txtTasa.Size = new System.Drawing.Size(53, 20);
            this.txtTasa.TabIndex = 20;
            this.txtTasa.Text = "0.0000";
            this.txtTasa.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTipFrm.SetToolTip(this.txtTasa, "Tasa de cambio para todos los montos.");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 308);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 24;
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(527, 359);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(148, 25);
            this.btnProcesar.TabIndex = 29;
            this.btnProcesar.Text = "Validar e importar";
            this.toolTipFrm.SetToolTip(this.btnProcesar, "Valida los movimientos y registra si son válidos.");
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(108, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Tasa:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(293, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Cuenta haber:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(293, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "Cuenta debe:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(287, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 15);
            this.label7.TabIndex = 42;
            this.label7.Text = "Movimientos negativos:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 15);
            this.label8.TabIndex = 47;
            this.label8.Text = "General:";
            // 
            // chkSonDolares
            // 
            this.chkSonDolares.AutoSize = true;
            this.chkSonDolares.Location = new System.Drawing.Point(21, 36);
            this.chkSonDolares.Name = "chkSonDolares";
            this.chkSonDolares.Size = new System.Drawing.Size(80, 17);
            this.chkSonDolares.TabIndex = 48;
            this.chkSonDolares.Text = "son dolares";
            this.toolTipFrm.SetToolTip(this.chkSonDolares, "Los montos son dólares.");
            this.chkSonDolares.UseVisualStyleBackColor = true;
            this.chkSonDolares.CheckedChanged += new System.EventHandler(this.chkSonDolares_CheckedChanged);
            // 
            // chkTasaUsarBCN
            // 
            this.chkTasaUsarBCN.AutoSize = true;
            this.chkTasaUsarBCN.Checked = true;
            this.chkTasaUsarBCN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTasaUsarBCN.Enabled = false;
            this.chkTasaUsarBCN.Location = new System.Drawing.Point(203, 35);
            this.chkTasaUsarBCN.Name = "chkTasaUsarBCN";
            this.chkTasaUsarBCN.Size = new System.Drawing.Size(48, 17);
            this.chkTasaUsarBCN.TabIndex = 49;
            this.chkTasaUsarBCN.Text = "BCN";
            this.toolTipFrm.SetToolTip(this.chkTasaUsarBCN, "Obtener la tasa de cada fecha desde bcn.gob.ni");
            this.chkTasaUsarBCN.UseVisualStyleBackColor = true;
            this.chkTasaUsarBCN.CheckedChanged += new System.EventHandler(this.chkTasaUsarBCN_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 15);
            this.label9.TabIndex = 50;
            this.label9.Text = "Datos:";
            // 
            // txtDatos
            // 
            this.txtDatos.Location = new System.Drawing.Point(12, 153);
            this.txtDatos.Multiline = true;
            this.txtDatos.Name = "txtDatos";
            this.txtDatos.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDatos.Size = new System.Drawing.Size(663, 200);
            this.txtDatos.TabIndex = 51;
            this.toolTipFrm.SetToolTip(this.txtDatos, "Líneas que incluyen fecha, monto y decripción separados por una tabulación (a com" +
        "o los copia Excel).");
            this.txtDatos.TextChanged += new System.EventHandler(this.txtDatos_TextChanged);
            // 
            // lblPosDebe
            // 
            this.lblPosDebe.Location = new System.Drawing.Point(387, 34);
            this.lblPosDebe.Name = "lblPosDebe";
            this.lblPosDebe.Size = new System.Drawing.Size(284, 18);
            this.lblPosDebe.TabIndex = 52;
            this.lblPosDebe.Text = "(seleccione)";
            this.lblPosDebe.Click += new System.EventHandler(this.lblPosDebe_Click);
            // 
            // lblPosHaber
            // 
            this.lblPosHaber.Location = new System.Drawing.Point(387, 57);
            this.lblPosHaber.Name = "lblPosHaber";
            this.lblPosHaber.Size = new System.Drawing.Size(284, 18);
            this.lblPosHaber.TabIndex = 53;
            this.lblPosHaber.Text = "(seleccione)";
            this.lblPosHaber.Click += new System.EventHandler(this.lblPosHaber_Click);
            // 
            // lblNegDebe
            // 
            this.lblNegDebe.Location = new System.Drawing.Point(387, 99);
            this.lblNegDebe.Name = "lblNegDebe";
            this.lblNegDebe.Size = new System.Drawing.Size(284, 18);
            this.lblNegDebe.TabIndex = 54;
            this.lblNegDebe.Text = "(seleccione)";
            this.lblNegDebe.Click += new System.EventHandler(this.lblNegDebe_Click);
            // 
            // lblNegHaber
            // 
            this.lblNegHaber.Location = new System.Drawing.Point(387, 122);
            this.lblNegHaber.Name = "lblNegHaber";
            this.lblNegHaber.Size = new System.Drawing.Size(284, 18);
            this.lblNegHaber.TabIndex = 55;
            this.lblNegHaber.Text = "(seleccione)";
            this.lblNegHaber.Click += new System.EventHandler(this.lblNegHaber_Click);
            // 
            // icoPosDebe
            // 
            this.icoPosDebe.Image = global::Contab.Properties.Resources.estado_proceso;
            this.icoPosDebe.Location = new System.Drawing.Point(369, 33);
            this.icoPosDebe.Name = "icoPosDebe";
            this.icoPosDebe.Size = new System.Drawing.Size(16, 16);
            this.icoPosDebe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.icoPosDebe.TabIndex = 56;
            this.icoPosDebe.TabStop = false;
            this.icoPosDebe.Click += new System.EventHandler(this.icoPosDebe_Click);
            // 
            // icoPosHaber
            // 
            this.icoPosHaber.Image = global::Contab.Properties.Resources.estado_proceso;
            this.icoPosHaber.Location = new System.Drawing.Point(369, 56);
            this.icoPosHaber.Name = "icoPosHaber";
            this.icoPosHaber.Size = new System.Drawing.Size(16, 16);
            this.icoPosHaber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.icoPosHaber.TabIndex = 57;
            this.icoPosHaber.TabStop = false;
            this.icoPosHaber.Click += new System.EventHandler(this.icoPosHaber_Click);
            // 
            // icoNegDebe
            // 
            this.icoNegDebe.Image = global::Contab.Properties.Resources.estado_proceso;
            this.icoNegDebe.Location = new System.Drawing.Point(369, 98);
            this.icoNegDebe.Name = "icoNegDebe";
            this.icoNegDebe.Size = new System.Drawing.Size(16, 16);
            this.icoNegDebe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.icoNegDebe.TabIndex = 58;
            this.icoNegDebe.TabStop = false;
            this.icoNegDebe.Click += new System.EventHandler(this.icoNegDebe_Click);
            // 
            // icoNegHaber
            // 
            this.icoNegHaber.Image = global::Contab.Properties.Resources.estado_proceso;
            this.icoNegHaber.Location = new System.Drawing.Point(369, 121);
            this.icoNegHaber.Name = "icoNegHaber";
            this.icoNegHaber.Size = new System.Drawing.Size(16, 16);
            this.icoNegHaber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.icoNegHaber.TabIndex = 59;
            this.icoNegHaber.TabStop = false;
            this.icoNegHaber.Click += new System.EventHandler(this.icoNegHaber_Click);
            // 
            // btnValidar
            // 
            this.btnValidar.Location = new System.Drawing.Point(373, 359);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(148, 25);
            this.btnValidar.TabIndex = 60;
            this.btnValidar.Text = "Validar";
            this.toolTipFrm.SetToolTip(this.btnValidar, "Valida los movimientos sin registrar.");
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(12, 359);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(264, 25);
            this.barProgress.TabIndex = 61;
            this.barProgress.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.Location = new System.Drawing.Point(282, 359);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(85, 23);
            this.lblProgress.TabIndex = 62;
            this.lblProgress.Text = "0000 / 0000";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProgress.Visible = false;
            // 
            // FrmImportarMovs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 396);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.icoNegHaber);
            this.Controls.Add(this.icoNegDebe);
            this.Controls.Add(this.icoPosHaber);
            this.Controls.Add(this.icoPosDebe);
            this.Controls.Add(this.lblNegHaber);
            this.Controls.Add(this.lblNegDebe);
            this.Controls.Add(this.lblPosHaber);
            this.Controls.Add(this.lblPosDebe);
            this.Controls.Add(this.txtDatos);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkTasaUsarBCN);
            this.Controls.Add(this.chkSonDolares);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtTasa);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImportarMovs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Importar movimientos";
            this.Load += new System.EventHandler(this.FrmImportarMovs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.icoPosDebe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoPosHaber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoNegDebe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoNegHaber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtTasa;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btnProcesar;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkSonDolares;
        private System.Windows.Forms.CheckBox chkTasaUsarBCN;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDatos;
        private System.Windows.Forms.Label lblPosDebe;
        private System.Windows.Forms.Label lblPosHaber;
        private System.Windows.Forms.Label lblNegDebe;
        private System.Windows.Forms.Label lblNegHaber;
        private System.Windows.Forms.PictureBox icoPosDebe;
        private System.Windows.Forms.PictureBox icoPosHaber;
        private System.Windows.Forms.PictureBox icoNegDebe;
        private System.Windows.Forms.PictureBox icoNegHaber;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.ToolTip toolTipFrm;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Label lblProgress;
    }
}
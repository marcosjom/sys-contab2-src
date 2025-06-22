namespace Contab {
	partial class FrmTarjetaCreditoCierreHaciaPrincipal {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTarjetaCreditoCierreHaciaPrincipal));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTarjetaPrincipal = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTasaCambio = new System.Windows.Forms.TextBox();
            this.cmbFechaMantValor = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.lstTarjetas = new System.Windows.Forms.ListView();
            this.colCuenta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSaldoCS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSaldoUS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPerdCamb = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTasaBCN = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCuentaCambPerdida = new System.Windows.Forms.ComboBox();
            this.cmbCuentaCambUtilidad = new System.Windows.Forms.ComboBox();
            this.btnValidar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mover saldos de tarjetas hacia principal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tarjeta principal:";
            // 
            // cmbTarjetaPrincipal
            // 
            this.cmbTarjetaPrincipal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarjetaPrincipal.FormattingEnabled = true;
            this.cmbTarjetaPrincipal.Location = new System.Drawing.Point(105, 44);
            this.cmbTarjetaPrincipal.Name = "cmbTarjetaPrincipal";
            this.cmbTarjetaPrincipal.Size = new System.Drawing.Size(443, 21);
            this.cmbTarjetaPrincipal.TabIndex = 5;
            this.cmbTarjetaPrincipal.SelectedIndexChanged += new System.EventHandler(this.cmbTarjetaPrincipal_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(455, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Tasa:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Fecha:";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.Location = new System.Drawing.Point(495, 71);
            this.txtTasaCambio.MaxLength = 7;
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Size = new System.Drawing.Size(53, 20);
            this.txtTasaCambio.TabIndex = 33;
            this.txtTasaCambio.Text = "30.0000";
            this.txtTasaCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbFechaMantValor
            // 
            this.cmbFechaMantValor.Location = new System.Drawing.Point(105, 71);
            this.cmbFechaMantValor.Name = "cmbFechaMantValor";
            this.cmbFechaMantValor.Size = new System.Drawing.Size(344, 20);
            this.cmbFechaMantValor.TabIndex = 32;
            this.cmbFechaMantValor.ValueChanged += new System.EventHandler(this.cmbFechaMantValor_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Mover saldos de trajetas:";
            // 
            // lstTarjetas
            // 
            this.lstTarjetas.CheckBoxes = true;
            this.lstTarjetas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCuenta,
            this.colDesc,
            this.colSaldoCS,
            this.colSaldoUS,
            this.colPerdCamb});
            this.lstTarjetas.FullRowSelect = true;
            this.lstTarjetas.GridLines = true;
            this.lstTarjetas.HideSelection = false;
            this.lstTarjetas.Location = new System.Drawing.Point(12, 127);
            this.lstTarjetas.Name = "lstTarjetas";
            this.lstTarjetas.Size = new System.Drawing.Size(536, 84);
            this.lstTarjetas.TabIndex = 37;
            this.lstTarjetas.UseCompatibleStateImageBehavior = false;
            this.lstTarjetas.View = System.Windows.Forms.View.Details;
            // 
            // colCuenta
            // 
            this.colCuenta.Text = "Tarjeta";
            this.colCuenta.Width = 50;
            // 
            // colDesc
            // 
            this.colDesc.Text = "Descripcion";
            this.colDesc.Width = 240;
            // 
            // colSaldoCS
            // 
            this.colSaldoCS.Text = "C$";
            this.colSaldoCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSaldoCS.Width = 80;
            // 
            // colSaldoUS
            // 
            this.colSaldoUS.Text = "U$";
            this.colSaldoUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSaldoUS.Width = 80;
            // 
            // colPerdCamb
            // 
            this.colPerdCamb.Text = "C$ PerdCamb";
            this.colPerdCamb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colPerdCamb.Width = 80;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(438, 280);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 29);
            this.btnOK.TabIndex = 38;
            this.btnOK.Text = "Aplicar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(231, 280);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 29);
            this.btnCancel.TabIndex = 39;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTasaBCN
            // 
            this.btnTasaBCN.Location = new System.Drawing.Point(495, 90);
            this.btnTasaBCN.Name = "btnTasaBCN";
            this.btnTasaBCN.Size = new System.Drawing.Size(53, 23);
            this.btnTasaBCN.TabIndex = 40;
            this.btnTasaBCN.Text = "BCN";
            this.btnTasaBCN.UseVisualStyleBackColor = true;
            this.btnTasaBCN.Click += new System.EventHandler(this.btnTasaBCN_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Perdida cambiaria:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Utilidad cambiaria:";
            // 
            // cmbCuentaCambPerdida
            // 
            this.cmbCuentaCambPerdida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCuentaCambPerdida.FormattingEnabled = true;
            this.cmbCuentaCambPerdida.Location = new System.Drawing.Point(105, 223);
            this.cmbCuentaCambPerdida.Name = "cmbCuentaCambPerdida";
            this.cmbCuentaCambPerdida.Size = new System.Drawing.Size(443, 21);
            this.cmbCuentaCambPerdida.TabIndex = 43;
            // 
            // cmbCuentaCambUtilidad
            // 
            this.cmbCuentaCambUtilidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCuentaCambUtilidad.FormattingEnabled = true;
            this.cmbCuentaCambUtilidad.Location = new System.Drawing.Point(105, 250);
            this.cmbCuentaCambUtilidad.Name = "cmbCuentaCambUtilidad";
            this.cmbCuentaCambUtilidad.Size = new System.Drawing.Size(443, 21);
            this.cmbCuentaCambUtilidad.TabIndex = 44;
            // 
            // btnValidar
            // 
            this.btnValidar.Location = new System.Drawing.Point(322, 280);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(110, 29);
            this.btnValidar.TabIndex = 45;
            this.btnValidar.Text = "Validar";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // FrmTarjetaCreditoCierreHaciaPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 317);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.cmbCuentaCambUtilidad);
            this.Controls.Add(this.cmbCuentaCambPerdida);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnTasaBCN);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstTarjetas);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTasaCambio);
            this.Controls.Add(this.cmbFechaMantValor);
            this.Controls.Add(this.cmbTarjetaPrincipal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTarjetaCreditoCierreHaciaPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mover saldos de tarjetas hacia principal";
            this.Load += new System.EventHandler(this.FrmTarjetaCreditoCierreHaciaPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbTarjetaPrincipal;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtTasaCambio;
		private System.Windows.Forms.DateTimePicker cmbFechaMantValor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView lstTarjetas;
		private System.Windows.Forms.ColumnHeader colCuenta;
		private System.Windows.Forms.ColumnHeader colDesc;
		private System.Windows.Forms.ColumnHeader colSaldoCS;
		private System.Windows.Forms.ColumnHeader colSaldoUS;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnTasaBCN;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cmbCuentaCambPerdida;
		private System.Windows.Forms.ComboBox cmbCuentaCambUtilidad;
		private System.Windows.Forms.ColumnHeader colPerdCamb;
		private System.Windows.Forms.Button btnValidar;
	}
}
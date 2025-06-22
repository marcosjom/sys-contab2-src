namespace Contab {
	partial class FrmTarjetaCredito {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTarjetaCredito));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTarjetaDetalle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTarjetaPrincipal = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMantValor = new System.Windows.Forms.TextBox();
            this.cmbFechaMantValor = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtIntCS = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTasaIntereses = new System.Windows.Forms.TextBox();
            this.txtIntUS = new System.Windows.Forms.TextBox();
            this.txtBonificableUS = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBonificableCS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnTasaBCN = new System.Windows.Forms.Button();
            this.txtIntMoraUS = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtIntMoraCS = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCargoAdminMora = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Estado de cuenta de tarjeta de credito.";
            // 
            // cmbTarjetaDetalle
            // 
            this.cmbTarjetaDetalle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarjetaDetalle.FormattingEnabled = true;
            this.cmbTarjetaDetalle.Location = new System.Drawing.Point(101, 48);
            this.cmbTarjetaDetalle.Name = "cmbTarjetaDetalle";
            this.cmbTarjetaDetalle.Size = new System.Drawing.Size(299, 21);
            this.cmbTarjetaDetalle.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tarjeta detalle:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tarjeta principal:";
            // 
            // cmbTarjetaPrincipal
            // 
            this.cmbTarjetaPrincipal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarjetaPrincipal.FormattingEnabled = true;
            this.cmbTarjetaPrincipal.Location = new System.Drawing.Point(101, 77);
            this.cmbTarjetaPrincipal.Name = "cmbTarjetaPrincipal";
            this.cmbTarjetaPrincipal.Size = new System.Drawing.Size(299, 21);
            this.cmbTarjetaPrincipal.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Mantenimiento al valor C$:";
            // 
            // txtMantValor
            // 
            this.txtMantValor.Location = new System.Drawing.Point(322, 161);
            this.txtMantValor.MaxLength = 8;
            this.txtMantValor.Name = "txtMantValor";
            this.txtMantValor.Size = new System.Drawing.Size(78, 20);
            this.txtMantValor.TabIndex = 6;
            this.txtMantValor.Text = "0.00";
            this.txtMantValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbFechaMantValor
            // 
            this.cmbFechaMantValor.Location = new System.Drawing.Point(101, 111);
            this.cmbFechaMantValor.Name = "cmbFechaMantValor";
            this.cmbFechaMantValor.Size = new System.Drawing.Size(200, 20);
            this.cmbFechaMantValor.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(89, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Intereses:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(55, 268);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Corrientes saldo local:";
            // 
            // txtIntCS
            // 
            this.txtIntCS.Location = new System.Drawing.Point(174, 264);
            this.txtIntCS.MaxLength = 8;
            this.txtIntCS.Name = "txtIntCS";
            this.txtIntCS.Size = new System.Drawing.Size(78, 20);
            this.txtIntCS.TabIndex = 18;
            this.txtIntCS.Text = "0.00";
            this.txtIntCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(270, 268);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Dolares:";
            // 
            // txtTasaIntereses
            // 
            this.txtTasaIntereses.Location = new System.Drawing.Point(347, 111);
            this.txtTasaIntereses.MaxLength = 7;
            this.txtTasaIntereses.Name = "txtTasaIntereses";
            this.txtTasaIntereses.Size = new System.Drawing.Size(53, 20);
            this.txtTasaIntereses.TabIndex = 20;
            this.txtTasaIntereses.Text = "30.0000";
            this.txtTasaIntereses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIntUS
            // 
            this.txtIntUS.Location = new System.Drawing.Point(322, 265);
            this.txtIntUS.MaxLength = 8;
            this.txtIntUS.Name = "txtIntUS";
            this.txtIntUS.Size = new System.Drawing.Size(78, 20);
            this.txtIntUS.TabIndex = 21;
            this.txtIntUS.Text = "0.00";
            this.txtIntUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBonificableUS
            // 
            this.txtBonificableUS.Location = new System.Drawing.Point(322, 290);
            this.txtBonificableUS.MaxLength = 8;
            this.txtBonificableUS.Name = "txtBonificableUS";
            this.txtBonificableUS.Size = new System.Drawing.Size(78, 20);
            this.txtBonificableUS.TabIndex = 25;
            this.txtBonificableUS.Text = "0.00";
            this.txtBonificableUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 308);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 24;
            // 
            // txtBonificableCS
            // 
            this.txtBonificableCS.Location = new System.Drawing.Point(174, 290);
            this.txtBonificableCS.MaxLength = 8;
            this.txtBonificableCS.Name = "txtBonificableCS";
            this.txtBonificableCS.Size = new System.Drawing.Size(78, 20);
            this.txtBonificableCS.TabIndex = 23;
            this.txtBonificableCS.Text = "0.00";
            this.txtBonificableCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 289);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(132, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Corrientes bonificables C$:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(270, 293);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Dolares:";
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(254, 328);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(148, 46);
            this.btnProcesar.TabIndex = 29;
            this.btnProcesar.Text = "Validar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Fecha:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(307, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Tasa:";
            // 
            // btnTasaBCN
            // 
            this.btnTasaBCN.Location = new System.Drawing.Point(347, 133);
            this.btnTasaBCN.Name = "btnTasaBCN";
            this.btnTasaBCN.Size = new System.Drawing.Size(53, 23);
            this.btnTasaBCN.TabIndex = 41;
            this.btnTasaBCN.Text = "BCN";
            this.btnTasaBCN.UseVisualStyleBackColor = true;
            this.btnTasaBCN.Click += new System.EventHandler(this.btnTasaBCN_Click);
            // 
            // txtIntMoraUS
            // 
            this.txtIntMoraUS.Location = new System.Drawing.Point(322, 237);
            this.txtIntMoraUS.MaxLength = 8;
            this.txtIntMoraUS.Name = "txtIntMoraUS";
            this.txtIntMoraUS.Size = new System.Drawing.Size(78, 20);
            this.txtIntMoraUS.TabIndex = 45;
            this.txtIntMoraUS.Text = "0.00";
            this.txtIntMoraUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(270, 240);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 44;
            this.label12.Text = "Dolares:";
            // 
            // txtIntMoraCS
            // 
            this.txtIntMoraCS.Location = new System.Drawing.Point(174, 236);
            this.txtIntMoraCS.MaxLength = 8;
            this.txtIntMoraCS.Name = "txtIntMoraCS";
            this.txtIntMoraCS.Size = new System.Drawing.Size(78, 20);
            this.txtIntMoraCS.TabIndex = 43;
            this.txtIntMoraCS.Text = "0.00";
            this.txtIntMoraCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(88, 240);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 42;
            this.label14.Text = "Por mora local:";
            // 
            // txtCargoAdminMora
            // 
            this.txtCargoAdminMora.Location = new System.Drawing.Point(322, 187);
            this.txtCargoAdminMora.MaxLength = 8;
            this.txtCargoAdminMora.Name = "txtCargoAdminMora";
            this.txtCargoAdminMora.Size = new System.Drawing.Size(78, 20);
            this.txtCargoAdminMora.TabIndex = 47;
            this.txtCargoAdminMora.Text = "0.00";
            this.txtCargoAdminMora.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(151, 190);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(165, 13);
            this.label15.TabIndex = 46;
            this.label15.Text = "Cargo administrativo por mora C$:";
            // 
            // FrmTarjetaCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 396);
            this.Controls.Add(this.txtCargoAdminMora);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtIntMoraUS);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtIntMoraCS);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.btnTasaBCN);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtBonificableUS);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBonificableCS);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtIntUS);
            this.Controls.Add(this.txtTasaIntereses);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtIntCS);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbFechaMantValor);
            this.Controls.Add(this.txtMantValor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbTarjetaPrincipal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbTarjetaDetalle);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTarjetaCredito";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar estado de cuenta de tarjeta de credito";
            this.Load += new System.EventHandler(this.FrmTarjetaCredito_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbTarjetaDetalle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbTarjetaPrincipal;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtMantValor;
		private System.Windows.Forms.DateTimePicker cmbFechaMantValor;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtIntCS;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtTasaIntereses;
		private System.Windows.Forms.TextBox txtIntUS;
		private System.Windows.Forms.TextBox txtBonificableUS;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtBonificableCS;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnProcesar;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnTasaBCN;
		private System.Windows.Forms.TextBox txtIntMoraUS;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox txtIntMoraCS;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txtCargoAdminMora;
		private System.Windows.Forms.Label label15;
	}
}
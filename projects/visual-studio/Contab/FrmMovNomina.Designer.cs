namespace Contab
{
    partial class FrmMovNomina
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMovNomina));
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSalario = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtINSSLaboral = new System.Windows.Forms.TextBox();
            this.txtIRLaboral = new System.Windows.Forms.TextBox();
            this.txtINSSPatronal = new System.Windows.Forms.TextBox();
            this.txtINATEC = new System.Windows.Forms.TextBox();
            this.txtVacaciones = new System.Windows.Forms.TextBox();
            this.txtAguinaldo = new System.Windows.Forms.TextBox();
            this.txtIndemnizacion = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbTablaRetencion = new System.Windows.Forms.ComboBox();
            this.btnManual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Descripcion:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(105, 15);
            this.txtDescripcion.MaxLength = 100;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(269, 20);
            this.txtDescripcion.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Salario C$:";
            // 
            // txtSalario
            // 
            this.txtSalario.Location = new System.Drawing.Point(105, 41);
            this.txtSalario.MaxLength = 9;
            this.txtSalario.Name = "txtSalario";
            this.txtSalario.Size = new System.Drawing.Size(75, 20);
            this.txtSalario.TabIndex = 3;
            this.txtSalario.Text = "0.00";
            this.txtSalario.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSalario.TextChanged += new System.EventHandler(this.txtSalario_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "INSS laboral:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "IR laboral:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "INSS patronal:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "INATEC:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 186);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Vacaciones:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Aguinaldo:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Indemnizacion:";
            // 
            // txtINSSLaboral
            // 
            this.txtINSSLaboral.Enabled = false;
            this.txtINSSLaboral.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtINSSLaboral.Location = new System.Drawing.Point(105, 71);
            this.txtINSSLaboral.MaxLength = 9;
            this.txtINSSLaboral.Name = "txtINSSLaboral";
            this.txtINSSLaboral.Size = new System.Drawing.Size(75, 20);
            this.txtINSSLaboral.TabIndex = 11;
            this.txtINSSLaboral.Text = "0.00";
            this.txtINSSLaboral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIRLaboral
            // 
            this.txtIRLaboral.Enabled = false;
            this.txtIRLaboral.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtIRLaboral.Location = new System.Drawing.Point(105, 98);
            this.txtIRLaboral.MaxLength = 9;
            this.txtIRLaboral.Name = "txtIRLaboral";
            this.txtIRLaboral.Size = new System.Drawing.Size(75, 20);
            this.txtIRLaboral.TabIndex = 12;
            this.txtIRLaboral.Text = "0.00";
            this.txtIRLaboral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtINSSPatronal
            // 
            this.txtINSSPatronal.Enabled = false;
            this.txtINSSPatronal.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtINSSPatronal.Location = new System.Drawing.Point(105, 127);
            this.txtINSSPatronal.MaxLength = 9;
            this.txtINSSPatronal.Name = "txtINSSPatronal";
            this.txtINSSPatronal.Size = new System.Drawing.Size(75, 20);
            this.txtINSSPatronal.TabIndex = 13;
            this.txtINSSPatronal.Text = "0.00";
            this.txtINSSPatronal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtINATEC
            // 
            this.txtINATEC.Enabled = false;
            this.txtINATEC.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtINATEC.Location = new System.Drawing.Point(105, 155);
            this.txtINATEC.MaxLength = 9;
            this.txtINATEC.Name = "txtINATEC";
            this.txtINATEC.Size = new System.Drawing.Size(75, 20);
            this.txtINATEC.TabIndex = 14;
            this.txtINATEC.Text = "0.00";
            this.txtINATEC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtVacaciones
            // 
            this.txtVacaciones.Enabled = false;
            this.txtVacaciones.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtVacaciones.Location = new System.Drawing.Point(105, 185);
            this.txtVacaciones.MaxLength = 9;
            this.txtVacaciones.Name = "txtVacaciones";
            this.txtVacaciones.Size = new System.Drawing.Size(75, 20);
            this.txtVacaciones.TabIndex = 15;
            this.txtVacaciones.Text = "0.00";
            this.txtVacaciones.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAguinaldo
            // 
            this.txtAguinaldo.Enabled = false;
            this.txtAguinaldo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtAguinaldo.Location = new System.Drawing.Point(105, 213);
            this.txtAguinaldo.MaxLength = 9;
            this.txtAguinaldo.Name = "txtAguinaldo";
            this.txtAguinaldo.Size = new System.Drawing.Size(75, 20);
            this.txtAguinaldo.TabIndex = 16;
            this.txtAguinaldo.Text = "0.00";
            this.txtAguinaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIndemnizacion
            // 
            this.txtIndemnizacion.Enabled = false;
            this.txtIndemnizacion.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtIndemnizacion.Location = new System.Drawing.Point(105, 240);
            this.txtIndemnizacion.MaxLength = 9;
            this.txtIndemnizacion.Name = "txtIndemnizacion";
            this.txtIndemnizacion.Size = new System.Drawing.Size(75, 20);
            this.txtIndemnizacion.TabIndex = 17;
            this.txtIndemnizacion.Text = "0.00";
            this.txtIndemnizacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(303, 269);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(71, 22);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "Aplicar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(226, 269);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(71, 22);
            this.btnCancelar.TabIndex = 19;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(188, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "(6.25% del salario base)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(188, 128);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(109, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "(16% del salario base)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(188, 157);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "(2% del salario base)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(187, 186);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(190, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "(1/12 del salario base / 1 mes por año)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(188, 214);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(190, 13);
            this.label15.TabIndex = 25;
            this.label15.Text = "(1/12 del salario base / 1 mes por año)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(188, 241);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(190, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "(1/12 del salario base / 1 mes por año)";
            // 
            // cmbTablaRetencion
            // 
            this.cmbTablaRetencion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTablaRetencion.FormattingEnabled = true;
            this.cmbTablaRetencion.Items.AddRange(new object[] {
            "Tabla Progrresiva 2013",
            "Tabla Progrresiva pre-2013"});
            this.cmbTablaRetencion.Location = new System.Drawing.Point(190, 96);
            this.cmbTablaRetencion.Name = "cmbTablaRetencion";
            this.cmbTablaRetencion.Size = new System.Drawing.Size(184, 21);
            this.cmbTablaRetencion.TabIndex = 27;
            this.cmbTablaRetencion.SelectedIndexChanged += new System.EventHandler(this.cmbTablaRetencion_SelectedIndexChanged);
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(86, 263);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(92, 23);
            this.btnManual.TabIndex = 28;
            this.btnManual.Text = "Calc. manual";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // FrmMovNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 302);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.cmbTablaRetencion);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtIndemnizacion);
            this.Controls.Add(this.txtAguinaldo);
            this.Controls.Add(this.txtVacaciones);
            this.Controls.Add(this.txtINATEC);
            this.Controls.Add(this.txtINSSPatronal);
            this.Controls.Add(this.txtIRLaboral);
            this.Controls.Add(this.txtINSSLaboral);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSalario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMovNomina";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nomina";
            this.Load += new System.EventHandler(this.FrmMovNomina_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSalario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtINSSLaboral;
        private System.Windows.Forms.TextBox txtIRLaboral;
        private System.Windows.Forms.TextBox txtINSSPatronal;
        private System.Windows.Forms.TextBox txtINATEC;
        private System.Windows.Forms.TextBox txtVacaciones;
        private System.Windows.Forms.TextBox txtAguinaldo;
        private System.Windows.Forms.TextBox txtIndemnizacion;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmbTablaRetencion;
		private System.Windows.Forms.Button btnManual;
    }
}
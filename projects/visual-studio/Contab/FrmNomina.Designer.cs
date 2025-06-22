namespace Contab {
	partial class FrmNomina {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNomina));
            this.lstImagenes = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnNuevoEmpleado = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colIdEmpleado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNombresEmpleado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colApellidosEmpleado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSalarioMensual = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIdCuentaSueldo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSueldo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCuentaVacaciones = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVacaciones = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCuentaAguinaldo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAguinaldo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCuentaIndemnizacion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIndemnizacion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstImagenes
            // 
            this.lstImagenes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lstImagenes.ImageStream")));
            this.lstImagenes.TransparentColor = System.Drawing.Color.Transparent;
            this.lstImagenes.Images.SetKeyName(0, "paquete16.gif");
            this.lstImagenes.Images.SetKeyName(1, "folder.gif");
            this.lstImagenes.Images.SetKeyName(2, "folderdatos.gif");
            this.lstImagenes.Images.SetKeyName(3, "word_copiar.gif");
            this.lstImagenes.Images.SetKeyName(4, "FlechaINOUT16(2).gif");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnActualizar);
            this.splitContainer1.Panel1.Controls.Add(this.btnNuevoEmpleado);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(1208, 642);
            this.splitContainer1.SplitterDistance = 33;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(152, 7);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(79, 21);
            this.btnActualizar.TabIndex = 2;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnNuevoEmpleado
            // 
            this.btnNuevoEmpleado.Location = new System.Drawing.Point(67, 7);
            this.btnNuevoEmpleado.Name = "btnNuevoEmpleado";
            this.btnNuevoEmpleado.Size = new System.Drawing.Size(79, 21);
            this.btnNuevoEmpleado.TabIndex = 1;
            this.btnNuevoEmpleado.Text = "Nuevo";
            this.btnNuevoEmpleado.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nomina";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIdEmpleado,
            this.colNombresEmpleado,
            this.colApellidosEmpleado,
            this.colSalarioMensual,
            this.colIdCuentaSueldo,
            this.colSueldo,
            this.colCuentaVacaciones,
            this.colVacaciones,
            this.colCuentaAguinaldo,
            this.colAguinaldo,
            this.colCuentaIndemnizacion,
            this.colIndemnizacion});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.lstImagenes;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1208, 605);
            this.listView1.SmallImageList = this.lstImagenes;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // colIdEmpleado
            // 
            this.colIdEmpleado.Tag = "";
            this.colIdEmpleado.Text = "ID";
            // 
            // colNombresEmpleado
            // 
            this.colNombresEmpleado.Text = "Nombres";
            this.colNombresEmpleado.Width = 125;
            // 
            // colApellidosEmpleado
            // 
            this.colApellidosEmpleado.Text = "Apellidos";
            this.colApellidosEmpleado.Width = 125;
            // 
            // colSalarioMensual
            // 
            this.colSalarioMensual.Text = "Salario mensual C$";
            this.colSalarioMensual.Width = 67;
            // 
            // colIdCuentaSueldo
            // 
            this.colIdCuentaSueldo.Text = "Cuenta sueldo";
            this.colIdCuentaSueldo.Width = 80;
            // 
            // colSueldo
            // 
            this.colSueldo.Text = "Sueldo acum C$";
            // 
            // colCuentaVacaciones
            // 
            this.colCuentaVacaciones.Text = "Cuenta vacaciones";
            this.colCuentaVacaciones.Width = 80;
            // 
            // colVacaciones
            // 
            this.colVacaciones.Text = "Vacaciones acum C$";
            // 
            // colCuentaAguinaldo
            // 
            this.colCuentaAguinaldo.Text = "Cuenta aguinaldo";
            this.colCuentaAguinaldo.Width = 80;
            // 
            // colAguinaldo
            // 
            this.colAguinaldo.Text = "Aguinaldo acum. C$";
            // 
            // colCuentaIndemnizacion
            // 
            this.colCuentaIndemnizacion.Text = "Cuenta indemnizacion";
            this.colCuentaIndemnizacion.Width = 80;
            // 
            // colIndemnizacion
            // 
            this.colIndemnizacion.Text = "Indemnizacion acum. C$";
            // 
            // FrmNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 642);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmNomina";
            this.Text = "Nomina";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ImageList lstImagenes;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader colIdEmpleado;
		private System.Windows.Forms.ColumnHeader colNombresEmpleado;
		private System.Windows.Forms.ColumnHeader colApellidosEmpleado;
		private System.Windows.Forms.ColumnHeader colIdCuentaSueldo;
		private System.Windows.Forms.ColumnHeader colSueldo;
		private System.Windows.Forms.ColumnHeader colCuentaVacaciones;
		private System.Windows.Forms.ColumnHeader colVacaciones;
		private System.Windows.Forms.ColumnHeader colCuentaAguinaldo;
		private System.Windows.Forms.ColumnHeader colCuentaIndemnizacion;
		private System.Windows.Forms.ColumnHeader colAguinaldo;
		private System.Windows.Forms.ColumnHeader colSalarioMensual;
		private System.Windows.Forms.ColumnHeader colIndemnizacion;
		private System.Windows.Forms.Button btnActualizar;
		private System.Windows.Forms.Button btnNuevoEmpleado;
	}
}
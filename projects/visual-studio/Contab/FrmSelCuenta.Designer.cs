namespace Contab {
	partial class FrmSelCuenta {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelCuenta));
            this.arbCuentas = new System.Windows.Forms.TreeView();
            this.lstImagenes = new System.Windows.Forms.ImageList(this.components);
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtIdCuenta = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // arbCuentas
            // 
            this.arbCuentas.FullRowSelect = true;
            this.arbCuentas.HideSelection = false;
            this.arbCuentas.ImageIndex = 0;
            this.arbCuentas.ImageList = this.lstImagenes;
            this.arbCuentas.Location = new System.Drawing.Point(8, 8);
            this.arbCuentas.Name = "arbCuentas";
            this.arbCuentas.SelectedImageIndex = 0;
            this.arbCuentas.Size = new System.Drawing.Size(491, 416);
            this.arbCuentas.TabIndex = 5;
            this.arbCuentas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.arbCuentas_AfterSelect);
            this.arbCuentas.DoubleClick += new System.EventHandler(this.arbCuentas_DoubleClick);
            this.arbCuentas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.arbCuentas_KeyPress);
            // 
            // lstImagenes
            // 
            this.lstImagenes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lstImagenes.ImageStream")));
            this.lstImagenes.TransparentColor = System.Drawing.Color.Transparent;
            this.lstImagenes.Images.SetKeyName(0, "paquete16.gif");
            this.lstImagenes.Images.SetKeyName(1, "folder.gif");
            this.lstImagenes.Images.SetKeyName(2, "folderdatos.gif");
            this.lstImagenes.Images.SetKeyName(3, "word_copiar.gif");
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Location = new System.Drawing.Point(399, 430);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(100, 27);
            this.btnSeleccionar.TabIndex = 9;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(290, 430);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 27);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtIdCuenta
            // 
            this.txtIdCuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdCuenta.Location = new System.Drawing.Point(8, 430);
            this.txtIdCuenta.MaxLength = 12;
            this.txtIdCuenta.Name = "txtIdCuenta";
            this.txtIdCuenta.Size = new System.Drawing.Size(200, 24);
            this.txtIdCuenta.TabIndex = 11;
            this.txtIdCuenta.TextChanged += new System.EventHandler(this.txtIdCuenta_TextChanged);
            // 
            // FrmSelCuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 466);
            this.Controls.Add(this.txtIdCuenta);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSeleccionar);
            this.Controls.Add(this.arbCuentas);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSelCuenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccione una cuenta detalle";
            this.Load += new System.EventHandler(this.FrmSelCuenta_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView arbCuentas;
		private System.Windows.Forms.ImageList lstImagenes;
		private System.Windows.Forms.Button btnSeleccionar;
		private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtIdCuenta;
	}
}
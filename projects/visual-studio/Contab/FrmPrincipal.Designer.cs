namespace Contab
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contabilidadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recalcularBalanceDeCuentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recalcularBalanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarMovimientosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contabilidadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.contabilidadToolStripMenuItem,
            this.herramientasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(719, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cerrarSesionToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 22);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // cerrarSesionToolStripMenuItem
            // 
            this.cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            this.cerrarSesionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cerrarSesionToolStripMenuItem.Text = "Cerrar sesion";
            this.cerrarSesionToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesionToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // contabilidadToolStripMenuItem
            // 
            this.contabilidadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recalcularBalanceDeCuentasToolStripMenuItem,
            this.importarMovimientosToolStripMenuItem});
            this.contabilidadToolStripMenuItem.Name = "contabilidadToolStripMenuItem";
            this.contabilidadToolStripMenuItem.Size = new System.Drawing.Size(87, 22);
            this.contabilidadToolStripMenuItem.Text = "Contabilidad";
            this.contabilidadToolStripMenuItem.Click += new System.EventHandler(this.contabilidadToolStripMenuItem_Click);
            // 
            // recalcularBalanceDeCuentasToolStripMenuItem
            // 
            this.recalcularBalanceDeCuentasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recalcularBalanceToolStripMenuItem});
            this.recalcularBalanceDeCuentasToolStripMenuItem.Name = "recalcularBalanceDeCuentasToolStripMenuItem";
            this.recalcularBalanceDeCuentasToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.recalcularBalanceDeCuentasToolStripMenuItem.Text = "Cuentas";
            // 
            // recalcularBalanceToolStripMenuItem
            // 
            this.recalcularBalanceToolStripMenuItem.Name = "recalcularBalanceToolStripMenuItem";
            this.recalcularBalanceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.recalcularBalanceToolStripMenuItem.Text = "Recalcular balance";
            this.recalcularBalanceToolStripMenuItem.Click += new System.EventHandler(this.recalcularBalanceToolStripMenuItem_Click);
            // 
            // importarMovimientosToolStripMenuItem
            // 
            this.importarMovimientosToolStripMenuItem.Name = "importarMovimientosToolStripMenuItem";
            this.importarMovimientosToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.importarMovimientosToolStripMenuItem.Text = "Importar Movimientos";
            this.importarMovimientosToolStripMenuItem.Click += new System.EventHandler(this.importarMovimientosToolStripMenuItem_Click);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contabilidadToolStripMenuItem1,
            this.actualizadorToolStripMenuItem});
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(66, 22);
            this.herramientasToolStripMenuItem.Text = "Ventanas";
            // 
            // actualizadorToolStripMenuItem
            // 
            this.actualizadorToolStripMenuItem.Name = "actualizadorToolStripMenuItem";
            this.actualizadorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.actualizadorToolStripMenuItem.Text = "Actualizador";
            this.actualizadorToolStripMenuItem.Click += new System.EventHandler(this.actualizadorToolStripMenuItem_Click);
            // 
            // contabilidadToolStripMenuItem1
            // 
            this.contabilidadToolStripMenuItem1.Name = "contabilidadToolStripMenuItem1";
            this.contabilidadToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.contabilidadToolStripMenuItem1.Text = "Contabilidad";
            this.contabilidadToolStripMenuItem1.Click += new System.EventHandler(this.contabilidadToolStripMenuItem1_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 404);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Contabilidad";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrincipal_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem contabilidadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalcularBalanceDeCuentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalcularBalanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarMovimientosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualizadorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contabilidadToolStripMenuItem1;
        //ToDo: implementar parametrizando cuentas origen/destino.
        //private System.Windows.Forms.ToolStripMenuItem moverSaldosHaciaPrincipalToolStripMenuItem;
        //ToDo: remove or re-enable
        //private System.Windows.Forms.ToolStripMenuItem normalizarConversionesDolaresA2DigitosToolStripMenuItem;
        //ToDo: remove or re-enable
        //private System.Windows.Forms.ToolStripMenuItem conversionesDolaresA2DigitosToolStripMenuItem;
    }
}


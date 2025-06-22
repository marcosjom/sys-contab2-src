namespace Contab
{
    partial class FrmContab
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmContab));
            this.lstImagenes = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.arbCuentas = new System.Windows.Forms.TreeView();
            this.mnuArbolCuentas = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.arbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarBalancesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.noMostrarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarCordobizadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarEnMonedasSeparadasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cuentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renombrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crearHijaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tabCuentas = new System.Windows.Forms.TabControl();
            this.tabDiario = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnExportarComprobantes = new System.Windows.Forms.Button();
            this.btnOKDiario = new System.Windows.Forms.Button();
            this.fechaFinDiario = new System.Windows.Forms.DateTimePicker();
            this.fechaIniDiario = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lstComprobantes = new System.Windows.Forms.ListView();
            this.colIdCuentaDetalle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNomCuentaDetalle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDebeCS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHaberCS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTasacambio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDebeUS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHaberUS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colReferencias = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuComprobantes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cambiarCuentaDeMovimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarMovimientoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tnSrvBasics = new System.Windows.Forms.Button();
            this.btnDepreciacion = new System.Windows.Forms.Button();
            this.btnAmortizaciones = new System.Windows.Forms.Button();
            this.btnNomina = new System.Windows.Forms.Button();
            this.btnValidaMov = new System.Windows.Forms.Button();
            this.btnFinalizarNuevoMov = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.fechaNuevoMov = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.gridMovimientos = new System.Windows.Forms.DataGridView();
            this.colIdCuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNomCuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDebe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHaber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEsUS = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReferencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSumaHaber = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSumaDebe = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabAuxiliar = new System.Windows.Forms.TabPage();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkOcultarDolares = new System.Windows.Forms.CheckBox();
            this.btnExportarAuxiliar = new System.Windows.Forms.Button();
            this.btnOKAuxiliar = new System.Windows.Forms.Button();
            this.fechaFinAuxiliar = new System.Windows.Forms.DateTimePicker();
            this.fechaIniAuxiliar = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.lstAuxiliar = new System.Windows.Forms.ListView();
            this.colIdMovimiento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuAuxiliarCtas = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.eliminarMovimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabBalanzaC = new System.Windows.Forms.TabPage();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.chkSubtotales = new System.Windows.Forms.CheckBox();
            this.btnExportarBalanza = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbNivelBalanza = new System.Windows.Forms.ComboBox();
            this.btnOKBalanza = new System.Windows.Forms.Button();
            this.fechaFinBalanza = new System.Windows.Forms.DateTimePicker();
            this.fechaIniBalanza = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.lstBalanza = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabEstadoR = new System.Windows.Forms.TabPage();
            this.tabEstadoFinanciero = new System.Windows.Forms.TabPage();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnExportarFinanciero = new System.Windows.Forms.Button();
            this.btnRecalcularBalance = new System.Windows.Forms.Button();
            this.lblFinHaberUS = new System.Windows.Forms.Label();
            this.lblFinHaberCS = new System.Windows.Forms.Label();
            this.lblFinDebeUS = new System.Windows.Forms.Label();
            this.lblFinDebeCS = new System.Windows.Forms.Label();
            this.chkFinOmitirCeros = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbNivelEstadoFinanciero = new System.Windows.Forms.ComboBox();
            this.btnActualizarEstadoFin = new System.Windows.Forms.Button();
            this.txtTasaEstadoFin = new System.Windows.Forms.TextBox();
            this.chkEstadoFinCordibizado = new System.Windows.Forms.CheckBox();
            this.lstEstadoFin = new System.Windows.Forms.ListView();
            this.colFinIdCuenta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinCuenta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinDebeCS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinHaberCS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinDebeUS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFinHaberUS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tmrAsync = new System.Windows.Forms.Timer(this.components);
            this.btnLimpiarMovs = new System.Windows.Forms.Button();
            this.lblValidationResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mnuArbolCuentas.SuspendLayout();
            this.tabCuentas.SuspendLayout();
            this.tabDiario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.mnuComprobantes.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMovimientos)).BeginInit();
            this.tabAuxiliar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.mnuAuxiliarCtas.SuspendLayout();
            this.tabBalanzaC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.tabEstadoFinanciero.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.panel4.SuspendLayout();
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.arbCuentas);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabCuentas);
            this.splitContainer1.Size = new System.Drawing.Size(1026, 505);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 3;
            // 
            // arbCuentas
            // 
            this.arbCuentas.ContextMenuStrip = this.mnuArbolCuentas;
            this.arbCuentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arbCuentas.HideSelection = false;
            this.arbCuentas.ImageIndex = 0;
            this.arbCuentas.ImageList = this.lstImagenes;
            this.arbCuentas.Location = new System.Drawing.Point(0, 13);
            this.arbCuentas.Name = "arbCuentas";
            this.arbCuentas.SelectedImageIndex = 0;
            this.arbCuentas.Size = new System.Drawing.Size(300, 492);
            this.arbCuentas.TabIndex = 4;
            this.arbCuentas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.arbCuentas_MouseDoubleClick);
            // 
            // mnuArbolCuentas
            // 
            this.mnuArbolCuentas.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.mnuArbolCuentas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arbolToolStripMenuItem,
            this.cuentaToolStripMenuItem});
            this.mnuArbolCuentas.Name = "mnuArbolCuentas";
            this.mnuArbolCuentas.Size = new System.Drawing.Size(113, 48);
            // 
            // arbolToolStripMenuItem
            // 
            this.arbolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actualizarToolStripMenuItem,
            this.mostrarBalancesToolStripMenuItem1});
            this.arbolToolStripMenuItem.Name = "arbolToolStripMenuItem";
            this.arbolToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.arbolToolStripMenuItem.Text = "Arbol";
            // 
            // actualizarToolStripMenuItem
            // 
            this.actualizarToolStripMenuItem.Name = "actualizarToolStripMenuItem";
            this.actualizarToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.actualizarToolStripMenuItem.Text = "Actualizar";
            this.actualizarToolStripMenuItem.Click += new System.EventHandler(this.actualizarToolStripMenuItem_Click);
            // 
            // mostrarBalancesToolStripMenuItem1
            // 
            this.mostrarBalancesToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noMostrarToolStripMenuItem1,
            this.mostrarCordobizadosToolStripMenuItem,
            this.mostrarEnMonedasSeparadasToolStripMenuItem1});
            this.mostrarBalancesToolStripMenuItem1.Name = "mostrarBalancesToolStripMenuItem1";
            this.mostrarBalancesToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.mostrarBalancesToolStripMenuItem1.Text = "Mostrar balances";
            // 
            // noMostrarToolStripMenuItem1
            // 
            this.noMostrarToolStripMenuItem1.Name = "noMostrarToolStripMenuItem1";
            this.noMostrarToolStripMenuItem1.Size = new System.Drawing.Size(238, 22);
            this.noMostrarToolStripMenuItem1.Text = "No mostrar";
            this.noMostrarToolStripMenuItem1.Click += new System.EventHandler(this.noMostrarToolStripMenuItem1_Click);
            // 
            // mostrarCordobizadosToolStripMenuItem
            // 
            this.mostrarCordobizadosToolStripMenuItem.Name = "mostrarCordobizadosToolStripMenuItem";
            this.mostrarCordobizadosToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.mostrarCordobizadosToolStripMenuItem.Text = "Mostrar cordobizados";
            this.mostrarCordobizadosToolStripMenuItem.Click += new System.EventHandler(this.mostrarCordobizadosToolStripMenuItem_Click);
            // 
            // mostrarEnMonedasSeparadasToolStripMenuItem1
            // 
            this.mostrarEnMonedasSeparadasToolStripMenuItem1.Name = "mostrarEnMonedasSeparadasToolStripMenuItem1";
            this.mostrarEnMonedasSeparadasToolStripMenuItem1.Size = new System.Drawing.Size(238, 22);
            this.mostrarEnMonedasSeparadasToolStripMenuItem1.Text = "Mostrar en monedas separadas";
            this.mostrarEnMonedasSeparadasToolStripMenuItem1.Click += new System.EventHandler(this.mostrarEnMonedasSeparadasToolStripMenuItem1_Click);
            // 
            // cuentaToolStripMenuItem
            // 
            this.cuentaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renombrarToolStripMenuItem,
            this.crearHijaToolStripMenuItem,
            this.eliminarToolStripMenuItem});
            this.cuentaToolStripMenuItem.Name = "cuentaToolStripMenuItem";
            this.cuentaToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.cuentaToolStripMenuItem.Text = "Cuenta";
            // 
            // renombrarToolStripMenuItem
            // 
            this.renombrarToolStripMenuItem.Name = "renombrarToolStripMenuItem";
            this.renombrarToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.renombrarToolStripMenuItem.Text = "Renombrar";
            this.renombrarToolStripMenuItem.Click += new System.EventHandler(this.renombrarToolStripMenuItem_Click);
            // 
            // crearHijaToolStripMenuItem
            // 
            this.crearHijaToolStripMenuItem.Name = "crearHijaToolStripMenuItem";
            this.crearHijaToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.crearHijaToolStripMenuItem.Text = "Crear hija";
            this.crearHijaToolStripMenuItem.Click += new System.EventHandler(this.crearHijaToolStripMenuItem_Click);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.eliminarToolStripMenuItem.Text = "Eliminar";
            this.eliminarToolStripMenuItem.Click += new System.EventHandler(this.eliminarToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cuentas Contables";
            // 
            // tabCuentas
            // 
            this.tabCuentas.Controls.Add(this.tabDiario);
            this.tabCuentas.Controls.Add(this.tabAuxiliar);
            this.tabCuentas.Controls.Add(this.tabBalanzaC);
            this.tabCuentas.Controls.Add(this.tabEstadoR);
            this.tabCuentas.Controls.Add(this.tabEstadoFinanciero);
            this.tabCuentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCuentas.Location = new System.Drawing.Point(0, 0);
            this.tabCuentas.Name = "tabCuentas";
            this.tabCuentas.SelectedIndex = 0;
            this.tabCuentas.Size = new System.Drawing.Size(722, 505);
            this.tabCuentas.TabIndex = 0;
            // 
            // tabDiario
            // 
            this.tabDiario.Controls.Add(this.splitContainer2);
            this.tabDiario.Location = new System.Drawing.Point(4, 22);
            this.tabDiario.Name = "tabDiario";
            this.tabDiario.Padding = new System.Windows.Forms.Padding(3);
            this.tabDiario.Size = new System.Drawing.Size(714, 479);
            this.tabDiario.TabIndex = 0;
            this.tabDiario.Text = "Comrpobantes de Diario";
            this.tabDiario.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(708, 473);
            this.splitContainer2.SplitterDistance = 28;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnExportarComprobantes);
            this.panel1.Controls.Add(this.btnOKDiario);
            this.panel1.Controls.Add(this.fechaFinDiario);
            this.panel1.Controls.Add(this.fechaIniDiario);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 28);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(625, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(188, 22);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cierre anual hacia CAPITAL";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExportarComprobantes
            // 
            this.btnExportarComprobantes.Location = new System.Drawing.Point(552, 6);
            this.btnExportarComprobantes.Name = "btnExportarComprobantes";
            this.btnExportarComprobantes.Size = new System.Drawing.Size(67, 22);
            this.btnExportarComprobantes.TabIndex = 11;
            this.btnExportarComprobantes.Text = "Exportar";
            this.btnExportarComprobantes.UseVisualStyleBackColor = true;
            this.btnExportarComprobantes.Click += new System.EventHandler(this.btnExportarComprobantes_Click);
            // 
            // btnOKDiario
            // 
            this.btnOKDiario.Location = new System.Drawing.Point(504, 6);
            this.btnOKDiario.Name = "btnOKDiario";
            this.btnOKDiario.Size = new System.Drawing.Size(42, 22);
            this.btnOKDiario.TabIndex = 3;
            this.btnOKDiario.Text = "OK";
            this.btnOKDiario.UseVisualStyleBackColor = true;
            this.btnOKDiario.Click += new System.EventHandler(this.btnOKDiario_Click);
            // 
            // fechaFinDiario
            // 
            this.fechaFinDiario.Location = new System.Drawing.Point(298, 6);
            this.fechaFinDiario.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaFinDiario.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaFinDiario.Name = "fechaFinDiario";
            this.fechaFinDiario.Size = new System.Drawing.Size(198, 20);
            this.fechaFinDiario.TabIndex = 2;
            // 
            // fechaIniDiario
            // 
            this.fechaIniDiario.Location = new System.Drawing.Point(94, 6);
            this.fechaIniDiario.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaIniDiario.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaIniDiario.Name = "fechaIniDiario";
            this.fechaIniDiario.Size = new System.Drawing.Size(198, 20);
            this.fechaIniDiario.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Rango fechas:";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lstComprobantes);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel2);
            this.splitContainer3.Size = new System.Drawing.Size(708, 441);
            this.splitContainer3.SplitterDistance = 159;
            this.splitContainer3.TabIndex = 0;
            // 
            // lstComprobantes
            // 
            this.lstComprobantes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIdCuentaDetalle,
            this.colNomCuentaDetalle,
            this.colDebeCS,
            this.colHaberCS,
            this.colTasacambio,
            this.colDebeUS,
            this.colHaberUS,
            this.colReferencias});
            this.lstComprobantes.ContextMenuStrip = this.mnuComprobantes;
            this.lstComprobantes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstComprobantes.FullRowSelect = true;
            this.lstComprobantes.GridLines = true;
            this.lstComprobantes.HideSelection = false;
            this.lstComprobantes.LargeImageList = this.lstImagenes;
            this.lstComprobantes.Location = new System.Drawing.Point(0, 0);
            this.lstComprobantes.Name = "lstComprobantes";
            this.lstComprobantes.ShowItemToolTips = true;
            this.lstComprobantes.Size = new System.Drawing.Size(708, 159);
            this.lstComprobantes.SmallImageList = this.lstImagenes;
            this.lstComprobantes.TabIndex = 0;
            this.lstComprobantes.UseCompatibleStateImageBehavior = false;
            this.lstComprobantes.View = System.Windows.Forms.View.Details;
            this.lstComprobantes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstComprobantes_MouseDoubleClick);
            // 
            // colIdCuentaDetalle
            // 
            this.colIdCuentaDetalle.Text = "Cuenta";
            this.colIdCuentaDetalle.Width = 100;
            // 
            // colNomCuentaDetalle
            // 
            this.colNomCuentaDetalle.Text = "Cuenta";
            this.colNomCuentaDetalle.Width = 150;
            // 
            // colDebeCS
            // 
            this.colDebeCS.Text = "DebeC$";
            this.colDebeCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colDebeCS.Width = 100;
            // 
            // colHaberCS
            // 
            this.colHaberCS.Text = "HaberC$";
            this.colHaberCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colHaberCS.Width = 100;
            // 
            // colTasacambio
            // 
            this.colTasacambio.Text = "refTasa";
            this.colTasacambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colTasacambio.Width = 75;
            // 
            // colDebeUS
            // 
            this.colDebeUS.Text = "refDebeU$";
            this.colDebeUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colDebeUS.Width = 100;
            // 
            // colHaberUS
            // 
            this.colHaberUS.Text = "refHaberU$";
            this.colHaberUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colHaberUS.Width = 100;
            // 
            // colReferencias
            // 
            this.colReferencias.Text = "Referencia";
            this.colReferencias.Width = 150;
            // 
            // mnuComprobantes
            // 
            this.mnuComprobantes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarCuentaDeMovimientoToolStripMenuItem,
            this.eliminarMovimientoToolStripMenuItem1});
            this.mnuComprobantes.Name = "mnuComprobantes";
            this.mnuComprobantes.Size = new System.Drawing.Size(241, 48);
            // 
            // cambiarCuentaDeMovimientoToolStripMenuItem
            // 
            this.cambiarCuentaDeMovimientoToolStripMenuItem.Name = "cambiarCuentaDeMovimientoToolStripMenuItem";
            this.cambiarCuentaDeMovimientoToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.cambiarCuentaDeMovimientoToolStripMenuItem.Text = "cambiar cuenta de movimiento";
            this.cambiarCuentaDeMovimientoToolStripMenuItem.Click += new System.EventHandler(this.cambiarCuentaDeMovimientoToolStripMenuItem_Click);
            // 
            // eliminarMovimientoToolStripMenuItem1
            // 
            this.eliminarMovimientoToolStripMenuItem1.Name = "eliminarMovimientoToolStripMenuItem1";
            this.eliminarMovimientoToolStripMenuItem1.Size = new System.Drawing.Size(240, 22);
            this.eliminarMovimientoToolStripMenuItem1.Text = "eliminar movimiento";
            this.eliminarMovimientoToolStripMenuItem1.Click += new System.EventHandler(this.eliminarMovimientoToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 278);
            this.panel2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.btnLimpiarMovs);
            this.splitContainer4.Panel1.Controls.Add(this.tnSrvBasics);
            this.splitContainer4.Panel1.Controls.Add(this.btnDepreciacion);
            this.splitContainer4.Panel1.Controls.Add(this.btnAmortizaciones);
            this.splitContainer4.Panel1.Controls.Add(this.btnNomina);
            this.splitContainer4.Panel1.Controls.Add(this.btnValidaMov);
            this.splitContainer4.Panel1.Controls.Add(this.btnFinalizarNuevoMov);
            this.splitContainer4.Panel1.Controls.Add(this.label5);
            this.splitContainer4.Panel1.Controls.Add(this.fechaNuevoMov);
            this.splitContainer4.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(708, 278);
            this.splitContainer4.SplitterDistance = 33;
            this.splitContainer4.TabIndex = 8;
            // 
            // tnSrvBasics
            // 
            this.tnSrvBasics.Location = new System.Drawing.Point(922, 5);
            this.tnSrvBasics.Name = "tnSrvBasics";
            this.tnSrvBasics.Size = new System.Drawing.Size(67, 22);
            this.tnSrvBasics.TabIndex = 20;
            this.tnSrvBasics.Text = "Serv Basic";
            this.tnSrvBasics.UseVisualStyleBackColor = true;
            this.tnSrvBasics.Visible = false;
            this.tnSrvBasics.Click += new System.EventHandler(this.tnSrvBasics_Click);
            // 
            // btnDepreciacion
            // 
            this.btnDepreciacion.Location = new System.Drawing.Point(803, 5);
            this.btnDepreciacion.Name = "btnDepreciacion";
            this.btnDepreciacion.Size = new System.Drawing.Size(54, 22);
            this.btnDepreciacion.TabIndex = 19;
            this.btnDepreciacion.Text = "Deprec.";
            this.btnDepreciacion.UseVisualStyleBackColor = true;
            this.btnDepreciacion.Visible = false;
            this.btnDepreciacion.Click += new System.EventHandler(this.btnDepreciacion_Click);
            // 
            // btnAmortizaciones
            // 
            this.btnAmortizaciones.Location = new System.Drawing.Point(863, 5);
            this.btnAmortizaciones.Name = "btnAmortizaciones";
            this.btnAmortizaciones.Size = new System.Drawing.Size(53, 22);
            this.btnAmortizaciones.TabIndex = 18;
            this.btnAmortizaciones.Text = "Amort.";
            this.btnAmortizaciones.UseVisualStyleBackColor = true;
            this.btnAmortizaciones.Visible = false;
            this.btnAmortizaciones.Click += new System.EventHandler(this.btnAmortizaciones_Click);
            // 
            // btnNomina
            // 
            this.btnNomina.Location = new System.Drawing.Point(732, 5);
            this.btnNomina.Name = "btnNomina";
            this.btnNomina.Size = new System.Drawing.Size(65, 22);
            this.btnNomina.TabIndex = 17;
            this.btnNomina.Text = "Nomina";
            this.btnNomina.UseVisualStyleBackColor = true;
            this.btnNomina.Visible = false;
            this.btnNomina.Click += new System.EventHandler(this.btnNomina_Click);
            // 
            // btnValidaMov
            // 
            this.btnValidaMov.Location = new System.Drawing.Point(412, 4);
            this.btnValidaMov.Name = "btnValidaMov";
            this.btnValidaMov.Size = new System.Drawing.Size(65, 22);
            this.btnValidaMov.TabIndex = 16;
            this.btnValidaMov.Text = "Validar";
            this.btnValidaMov.UseVisualStyleBackColor = true;
            this.btnValidaMov.Click += new System.EventHandler(this.btnValidaMov_Click);
            // 
            // btnFinalizarNuevoMov
            // 
            this.btnFinalizarNuevoMov.Location = new System.Drawing.Point(483, 4);
            this.btnFinalizarNuevoMov.Name = "btnFinalizarNuevoMov";
            this.btnFinalizarNuevoMov.Size = new System.Drawing.Size(80, 22);
            this.btnFinalizarNuevoMov.TabIndex = 15;
            this.btnFinalizarNuevoMov.Text = "Crear";
            this.btnFinalizarNuevoMov.UseVisualStyleBackColor = true;
            this.btnFinalizarNuevoMov.Click += new System.EventHandler(this.btnFinalizarNuevoMov_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Nuevo comporbante:";
            // 
            // fechaNuevoMov
            // 
            this.fechaNuevoMov.Location = new System.Drawing.Point(116, 6);
            this.fechaNuevoMov.Name = "fechaNuevoMov";
            this.fechaNuevoMov.Size = new System.Drawing.Size(209, 20);
            this.fechaNuevoMov.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-162, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nuevo movimiento contable:";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.gridMovimientos);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.lblValidationResult);
            this.splitContainer5.Panel2.Controls.Add(this.lblSumaHaber);
            this.splitContainer5.Panel2.Controls.Add(this.label8);
            this.splitContainer5.Panel2.Controls.Add(this.lblSumaDebe);
            this.splitContainer5.Panel2.Controls.Add(this.label6);
            this.splitContainer5.Size = new System.Drawing.Size(708, 241);
            this.splitContainer5.SplitterDistance = 173;
            this.splitContainer5.TabIndex = 9;
            // 
            // gridMovimientos
            // 
            this.gridMovimientos.AllowUserToResizeRows = false;
            this.gridMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMovimientos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdCuenta,
            this.colNomCuenta,
            this.colDebe,
            this.colHaber,
            this.colEsUS,
            this.colTC,
            this.colReferencia});
            this.gridMovimientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMovimientos.Location = new System.Drawing.Point(0, 0);
            this.gridMovimientos.Name = "gridMovimientos";
            this.gridMovimientos.RowHeadersWidth = 82;
            this.gridMovimientos.Size = new System.Drawing.Size(708, 173);
            this.gridMovimientos.TabIndex = 9;
            this.gridMovimientos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMovimientos_CellClick);
            this.gridMovimientos.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMovimientos_CellValueChanged);
            // 
            // colIdCuenta
            // 
            this.colIdCuenta.HeaderText = "Cuenta";
            this.colIdCuenta.MaxInputLength = 12;
            this.colIdCuenta.MinimumWidth = 10;
            this.colIdCuenta.Name = "colIdCuenta";
            this.colIdCuenta.Width = 150;
            // 
            // colNomCuenta
            // 
            this.colNomCuenta.HeaderText = "NomCuenta";
            this.colNomCuenta.MaxInputLength = 255;
            this.colNomCuenta.MinimumWidth = 10;
            this.colNomCuenta.Name = "colNomCuenta";
            this.colNomCuenta.ReadOnly = true;
            this.colNomCuenta.Width = 200;
            // 
            // colDebe
            // 
            this.colDebe.HeaderText = "Debe";
            this.colDebe.MaxInputLength = 12;
            this.colDebe.MinimumWidth = 10;
            this.colDebe.Name = "colDebe";
            this.colDebe.Width = 200;
            // 
            // colHaber
            // 
            this.colHaber.HeaderText = "Haber";
            this.colHaber.MaxInputLength = 12;
            this.colHaber.MinimumWidth = 10;
            this.colHaber.Name = "colHaber";
            this.colHaber.Width = 200;
            // 
            // colEsUS
            // 
            this.colEsUS.HeaderText = "U$";
            this.colEsUS.MinimumWidth = 10;
            this.colEsUS.Name = "colEsUS";
            this.colEsUS.Width = 40;
            // 
            // colTC
            // 
            this.colTC.HeaderText = "Tasa/C";
            this.colTC.MaxInputLength = 9;
            this.colTC.MinimumWidth = 10;
            this.colTC.Name = "colTC";
            this.colTC.Width = 75;
            // 
            // colReferencia
            // 
            this.colReferencia.HeaderText = "Referencia";
            this.colReferencia.MaxInputLength = 255;
            this.colReferencia.MinimumWidth = 10;
            this.colReferencia.Name = "colReferencia";
            this.colReferencia.Width = 250;
            // 
            // lblSumaHaber
            // 
            this.lblSumaHaber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumaHaber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSumaHaber.Location = new System.Drawing.Point(262, 3);
            this.lblSumaHaber.Name = "lblSumaHaber";
            this.lblSumaHaber.Size = new System.Drawing.Size(97, 13);
            this.lblSumaHaber.TabIndex = 3;
            this.lblSumaHaber.Text = "000,000.00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(174, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Suma Haber C$:";
            // 
            // lblSumaDebe
            // 
            this.lblSumaDebe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumaDebe.ForeColor = System.Drawing.Color.Green;
            this.lblSumaDebe.Location = new System.Drawing.Point(86, 3);
            this.lblSumaDebe.Name = "lblSumaDebe";
            this.lblSumaDebe.Size = new System.Drawing.Size(104, 13);
            this.lblSumaDebe.TabIndex = 1;
            this.lblSumaDebe.Text = "000,000.00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Suma Debe C$:";
            // 
            // tabAuxiliar
            // 
            this.tabAuxiliar.Controls.Add(this.splitContainer6);
            this.tabAuxiliar.Location = new System.Drawing.Point(4, 22);
            this.tabAuxiliar.Name = "tabAuxiliar";
            this.tabAuxiliar.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuxiliar.Size = new System.Drawing.Size(714, 479);
            this.tabAuxiliar.TabIndex = 1;
            this.tabAuxiliar.Text = "Auxiliar de Cuentas";
            this.tabAuxiliar.UseVisualStyleBackColor = true;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.lstAuxiliar);
            this.splitContainer6.Size = new System.Drawing.Size(708, 473);
            this.splitContainer6.SplitterDistance = 35;
            this.splitContainer6.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkOcultarDolares);
            this.panel3.Controls.Add(this.btnExportarAuxiliar);
            this.panel3.Controls.Add(this.btnOKAuxiliar);
            this.panel3.Controls.Add(this.fechaFinAuxiliar);
            this.panel3.Controls.Add(this.fechaIniAuxiliar);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(708, 35);
            this.panel3.TabIndex = 1;
            // 
            // chkOcultarDolares
            // 
            this.chkOcultarDolares.AutoSize = true;
            this.chkOcultarDolares.Location = new System.Drawing.Point(628, 11);
            this.chkOcultarDolares.Name = "chkOcultarDolares";
            this.chkOcultarDolares.Size = new System.Drawing.Size(96, 17);
            this.chkOcultarDolares.TabIndex = 13;
            this.chkOcultarDolares.Text = "separar C$/U$";
            this.chkOcultarDolares.UseVisualStyleBackColor = true;
            // 
            // btnExportarAuxiliar
            // 
            this.btnExportarAuxiliar.Location = new System.Drawing.Point(552, 6);
            this.btnExportarAuxiliar.Name = "btnExportarAuxiliar";
            this.btnExportarAuxiliar.Size = new System.Drawing.Size(67, 22);
            this.btnExportarAuxiliar.TabIndex = 12;
            this.btnExportarAuxiliar.Text = "Exportar";
            this.btnExportarAuxiliar.UseVisualStyleBackColor = true;
            this.btnExportarAuxiliar.Click += new System.EventHandler(this.btnExportarAuxiliar_Click);
            // 
            // btnOKAuxiliar
            // 
            this.btnOKAuxiliar.Location = new System.Drawing.Point(504, 6);
            this.btnOKAuxiliar.Name = "btnOKAuxiliar";
            this.btnOKAuxiliar.Size = new System.Drawing.Size(42, 22);
            this.btnOKAuxiliar.TabIndex = 3;
            this.btnOKAuxiliar.Text = "OK";
            this.btnOKAuxiliar.UseVisualStyleBackColor = true;
            this.btnOKAuxiliar.Click += new System.EventHandler(this.btnOKAuxiliar_Click);
            // 
            // fechaFinAuxiliar
            // 
            this.fechaFinAuxiliar.Location = new System.Drawing.Point(298, 6);
            this.fechaFinAuxiliar.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaFinAuxiliar.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaFinAuxiliar.Name = "fechaFinAuxiliar";
            this.fechaFinAuxiliar.Size = new System.Drawing.Size(198, 20);
            this.fechaFinAuxiliar.TabIndex = 2;
            // 
            // fechaIniAuxiliar
            // 
            this.fechaIniAuxiliar.Location = new System.Drawing.Point(94, 6);
            this.fechaIniAuxiliar.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaIniAuxiliar.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaIniAuxiliar.Name = "fechaIniAuxiliar";
            this.fechaIniAuxiliar.Size = new System.Drawing.Size(198, 20);
            this.fechaIniAuxiliar.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Rango fechas:";
            // 
            // lstAuxiliar
            // 
            this.lstAuxiliar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIdMovimiento,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader12,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader13,
            this.columnHeader8,
            this.columnHeader16});
            this.lstAuxiliar.ContextMenuStrip = this.mnuAuxiliarCtas;
            this.lstAuxiliar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAuxiliar.FullRowSelect = true;
            this.lstAuxiliar.GridLines = true;
            this.lstAuxiliar.HideSelection = false;
            this.lstAuxiliar.LargeImageList = this.lstImagenes;
            this.lstAuxiliar.Location = new System.Drawing.Point(0, 0);
            this.lstAuxiliar.Name = "lstAuxiliar";
            this.lstAuxiliar.Size = new System.Drawing.Size(708, 434);
            this.lstAuxiliar.SmallImageList = this.lstImagenes;
            this.lstAuxiliar.TabIndex = 1;
            this.lstAuxiliar.UseCompatibleStateImageBehavior = false;
            this.lstAuxiliar.View = System.Windows.Forms.View.Details;
            this.lstAuxiliar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstAuxiliar_MouseDoubleClick);
            // 
            // colIdMovimiento
            // 
            this.colIdMovimiento.Text = "Movimiento";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Comprobante";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "DebeC$";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "HaberC$";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 100;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "SaldoC$";
            this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader12.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Tasa";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 75;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "DebeU$";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "HaberU$";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader7.Width = 100;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "SaldoU$";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader13.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Referencia";
            this.columnHeader8.Width = 150;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Nota";
            this.columnHeader16.Width = 125;
            // 
            // mnuAuxiliarCtas
            // 
            this.mnuAuxiliarCtas.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.mnuAuxiliarCtas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.eliminarMovimientoToolStripMenuItem});
            this.mnuAuxiliarCtas.Name = "mnuComprobantes";
            this.mnuAuxiliarCtas.Size = new System.Drawing.Size(241, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(240, 22);
            this.toolStripMenuItem1.Text = "cambiar cuenta de movimiento";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // eliminarMovimientoToolStripMenuItem
            // 
            this.eliminarMovimientoToolStripMenuItem.Name = "eliminarMovimientoToolStripMenuItem";
            this.eliminarMovimientoToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.eliminarMovimientoToolStripMenuItem.Text = "eliminar movimiento";
            this.eliminarMovimientoToolStripMenuItem.Click += new System.EventHandler(this.eliminarMovimientoToolStripMenuItem_Click);
            // 
            // tabBalanzaC
            // 
            this.tabBalanzaC.Controls.Add(this.splitContainer8);
            this.tabBalanzaC.Location = new System.Drawing.Point(4, 22);
            this.tabBalanzaC.Name = "tabBalanzaC";
            this.tabBalanzaC.Size = new System.Drawing.Size(714, 479);
            this.tabBalanzaC.TabIndex = 2;
            this.tabBalanzaC.Text = "Balanza de Comprobacion";
            this.tabBalanzaC.UseVisualStyleBackColor = true;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.chkSubtotales);
            this.splitContainer8.Panel1.Controls.Add(this.btnExportarBalanza);
            this.splitContainer8.Panel1.Controls.Add(this.label10);
            this.splitContainer8.Panel1.Controls.Add(this.cmbNivelBalanza);
            this.splitContainer8.Panel1.Controls.Add(this.btnOKBalanza);
            this.splitContainer8.Panel1.Controls.Add(this.fechaFinBalanza);
            this.splitContainer8.Panel1.Controls.Add(this.fechaIniBalanza);
            this.splitContainer8.Panel1.Controls.Add(this.label9);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.lstBalanza);
            this.splitContainer8.Size = new System.Drawing.Size(714, 479);
            this.splitContainer8.SplitterDistance = 39;
            this.splitContainer8.TabIndex = 0;
            this.splitContainer8.Visible = false;
            // 
            // chkSubtotales
            // 
            this.chkSubtotales.AutoSize = true;
            this.chkSubtotales.Checked = true;
            this.chkSubtotales.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubtotales.Location = new System.Drawing.Point(600, 14);
            this.chkSubtotales.Margin = new System.Windows.Forms.Padding(2);
            this.chkSubtotales.Name = "chkSubtotales";
            this.chkSubtotales.Size = new System.Drawing.Size(74, 17);
            this.chkSubtotales.TabIndex = 11;
            this.chkSubtotales.Text = "subtotales";
            this.chkSubtotales.UseVisualStyleBackColor = true;
            // 
            // btnExportarBalanza
            // 
            this.btnExportarBalanza.Location = new System.Drawing.Point(725, 9);
            this.btnExportarBalanza.Name = "btnExportarBalanza";
            this.btnExportarBalanza.Size = new System.Drawing.Size(67, 22);
            this.btnExportarBalanza.TabIndex = 10;
            this.btnExportarBalanza.Text = "Exportar";
            this.btnExportarBalanza.UseVisualStyleBackColor = true;
            this.btnExportarBalanza.Click += new System.EventHandler(this.btnExportarBalanza_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(507, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Niveles:";
            // 
            // cmbNivelBalanza
            // 
            this.cmbNivelBalanza.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNivelBalanza.FormattingEnabled = true;
            this.cmbNivelBalanza.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbNivelBalanza.Location = new System.Drawing.Point(553, 10);
            this.cmbNivelBalanza.Name = "cmbNivelBalanza";
            this.cmbNivelBalanza.Size = new System.Drawing.Size(42, 21);
            this.cmbNivelBalanza.TabIndex = 8;
            // 
            // btnOKBalanza
            // 
            this.btnOKBalanza.Location = new System.Drawing.Point(677, 9);
            this.btnOKBalanza.Name = "btnOKBalanza";
            this.btnOKBalanza.Size = new System.Drawing.Size(42, 22);
            this.btnOKBalanza.TabIndex = 7;
            this.btnOKBalanza.Text = "OK";
            this.btnOKBalanza.UseVisualStyleBackColor = true;
            this.btnOKBalanza.Click += new System.EventHandler(this.btnOKBalanza_Click);
            // 
            // fechaFinBalanza
            // 
            this.fechaFinBalanza.Location = new System.Drawing.Point(301, 10);
            this.fechaFinBalanza.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaFinBalanza.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaFinBalanza.Name = "fechaFinBalanza";
            this.fechaFinBalanza.Size = new System.Drawing.Size(198, 20);
            this.fechaFinBalanza.TabIndex = 6;
            this.fechaFinBalanza.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // fechaIniBalanza
            // 
            this.fechaIniBalanza.Location = new System.Drawing.Point(97, 10);
            this.fechaIniBalanza.MaxDate = new System.DateTime(3000, 12, 31, 0, 0, 0, 0);
            this.fechaIniBalanza.MinDate = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.fechaIniBalanza.Name = "fechaIniBalanza";
            this.fechaIniBalanza.Size = new System.Drawing.Size(198, 20);
            this.fechaIniBalanza.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Rango fechas:";
            // 
            // lstBalanza
            // 
            this.lstBalanza.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader22,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader23,
            this.columnHeader18,
            this.columnHeader19});
            this.lstBalanza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBalanza.FullRowSelect = true;
            this.lstBalanza.GridLines = true;
            this.lstBalanza.HideSelection = false;
            this.lstBalanza.LargeImageList = this.lstImagenes;
            this.lstBalanza.Location = new System.Drawing.Point(0, 0);
            this.lstBalanza.Name = "lstBalanza";
            this.lstBalanza.Size = new System.Drawing.Size(714, 436);
            this.lstBalanza.SmallImageList = this.lstImagenes;
            this.lstBalanza.TabIndex = 1;
            this.lstBalanza.UseCompatibleStateImageBehavior = false;
            this.lstBalanza.View = System.Windows.Forms.View.Details;
            this.lstBalanza.Visible = false;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IDCuenta";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Cuenta";
            this.columnHeader9.Width = 350;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Debe C$";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Haber C$";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader11.Width = 100;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "-";
            this.columnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader22.Width = 30;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Debe C$";
            this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader14.Width = 100;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Haber C$";
            this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader15.Width = 100;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "-";
            this.columnHeader23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader23.Width = 30;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Debe C$";
            this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader18.Width = 100;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Haber C$";
            this.columnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader19.Width = 100;
            // 
            // tabEstadoR
            // 
            this.tabEstadoR.Location = new System.Drawing.Point(4, 22);
            this.tabEstadoR.Name = "tabEstadoR";
            this.tabEstadoR.Size = new System.Drawing.Size(714, 479);
            this.tabEstadoR.TabIndex = 4;
            this.tabEstadoR.Text = "Estado de Resultados";
            this.tabEstadoR.UseVisualStyleBackColor = true;
            // 
            // tabEstadoFinanciero
            // 
            this.tabEstadoFinanciero.Controls.Add(this.splitContainer7);
            this.tabEstadoFinanciero.Location = new System.Drawing.Point(4, 22);
            this.tabEstadoFinanciero.Name = "tabEstadoFinanciero";
            this.tabEstadoFinanciero.Padding = new System.Windows.Forms.Padding(3);
            this.tabEstadoFinanciero.Size = new System.Drawing.Size(714, 479);
            this.tabEstadoFinanciero.TabIndex = 5;
            this.tabEstadoFinanciero.Text = "Estado Financiero";
            this.tabEstadoFinanciero.UseVisualStyleBackColor = true;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer7.Location = new System.Drawing.Point(3, 3);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.panel4);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.lstEstadoFin);
            this.splitContainer7.Size = new System.Drawing.Size(708, 473);
            this.splitContainer7.SplitterDistance = 64;
            this.splitContainer7.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnExportarFinanciero);
            this.panel4.Controls.Add(this.btnRecalcularBalance);
            this.panel4.Controls.Add(this.lblFinHaberUS);
            this.panel4.Controls.Add(this.lblFinHaberCS);
            this.panel4.Controls.Add(this.lblFinDebeUS);
            this.panel4.Controls.Add(this.lblFinDebeCS);
            this.panel4.Controls.Add(this.chkFinOmitirCeros);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.cmbNivelEstadoFinanciero);
            this.panel4.Controls.Add(this.btnActualizarEstadoFin);
            this.panel4.Controls.Add(this.txtTasaEstadoFin);
            this.panel4.Controls.Add(this.chkEstadoFinCordibizado);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(708, 64);
            this.panel4.TabIndex = 0;
            this.panel4.Visible = false;
            // 
            // btnExportarFinanciero
            // 
            this.btnExportarFinanciero.Location = new System.Drawing.Point(513, 5);
            this.btnExportarFinanciero.Name = "btnExportarFinanciero";
            this.btnExportarFinanciero.Size = new System.Drawing.Size(67, 22);
            this.btnExportarFinanciero.TabIndex = 11;
            this.btnExportarFinanciero.Text = "Exportar";
            this.btnExportarFinanciero.UseVisualStyleBackColor = true;
            this.btnExportarFinanciero.Click += new System.EventHandler(this.btnExportarFinanciero_Click);
            // 
            // btnRecalcularBalance
            // 
            this.btnRecalcularBalance.Location = new System.Drawing.Point(513, 34);
            this.btnRecalcularBalance.Name = "btnRecalcularBalance";
            this.btnRecalcularBalance.Size = new System.Drawing.Size(179, 22);
            this.btnRecalcularBalance.TabIndex = 10;
            this.btnRecalcularBalance.Text = "Recalcular balance de cuentas";
            this.btnRecalcularBalance.UseVisualStyleBackColor = true;
            this.btnRecalcularBalance.Click += new System.EventHandler(this.btnRecalcularBalance_Click);
            // 
            // lblFinHaberUS
            // 
            this.lblFinHaberUS.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblFinHaberUS.ForeColor = System.Drawing.Color.Maroon;
            this.lblFinHaberUS.Location = new System.Drawing.Point(379, 39);
            this.lblFinHaberUS.Name = "lblFinHaberUS";
            this.lblFinHaberUS.Size = new System.Drawing.Size(123, 16);
            this.lblFinHaberUS.TabIndex = 9;
            this.lblFinHaberUS.Text = "Haber: U$0,000.00";
            this.lblFinHaberUS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFinHaberCS
            // 
            this.lblFinHaberCS.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblFinHaberCS.ForeColor = System.Drawing.Color.Maroon;
            this.lblFinHaberCS.Location = new System.Drawing.Point(140, 39);
            this.lblFinHaberCS.Name = "lblFinHaberCS";
            this.lblFinHaberCS.Size = new System.Drawing.Size(123, 16);
            this.lblFinHaberCS.TabIndex = 8;
            this.lblFinHaberCS.Text = "Haber: C$0,000.00";
            this.lblFinHaberCS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFinDebeUS
            // 
            this.lblFinDebeUS.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblFinDebeUS.ForeColor = System.Drawing.Color.Green;
            this.lblFinDebeUS.Location = new System.Drawing.Point(258, 39);
            this.lblFinDebeUS.Name = "lblFinDebeUS";
            this.lblFinDebeUS.Size = new System.Drawing.Size(123, 16);
            this.lblFinDebeUS.TabIndex = 7;
            this.lblFinDebeUS.Text = "Debe: U$0,000.00";
            this.lblFinDebeUS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFinDebeCS
            // 
            this.lblFinDebeCS.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblFinDebeCS.ForeColor = System.Drawing.Color.Green;
            this.lblFinDebeCS.Location = new System.Drawing.Point(3, 39);
            this.lblFinDebeCS.Name = "lblFinDebeCS";
            this.lblFinDebeCS.Size = new System.Drawing.Size(123, 16);
            this.lblFinDebeCS.TabIndex = 6;
            this.lblFinDebeCS.Text = "Debe: C$0,000.00";
            this.lblFinDebeCS.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkFinOmitirCeros
            // 
            this.chkFinOmitirCeros.AutoSize = true;
            this.chkFinOmitirCeros.Checked = true;
            this.chkFinOmitirCeros.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFinOmitirCeros.Location = new System.Drawing.Point(99, 9);
            this.chkFinOmitirCeros.Name = "chkFinOmitirCeros";
            this.chkFinOmitirCeros.Size = new System.Drawing.Size(130, 17);
            this.chkFinOmitirCeros.TabIndex = 5;
            this.chkFinOmitirCeros.Text = "omitir cuentas en cero";
            this.chkFinOmitirCeros.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Niveles:";
            // 
            // cmbNivelEstadoFinanciero
            // 
            this.cmbNivelEstadoFinanciero.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNivelEstadoFinanciero.FormattingEnabled = true;
            this.cmbNivelEstadoFinanciero.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbNivelEstadoFinanciero.Location = new System.Drawing.Point(49, 6);
            this.cmbNivelEstadoFinanciero.Name = "cmbNivelEstadoFinanciero";
            this.cmbNivelEstadoFinanciero.Size = new System.Drawing.Size(42, 21);
            this.cmbNivelEstadoFinanciero.TabIndex = 3;
            // 
            // btnActualizarEstadoFin
            // 
            this.btnActualizarEstadoFin.Location = new System.Drawing.Point(410, 5);
            this.btnActualizarEstadoFin.Name = "btnActualizarEstadoFin";
            this.btnActualizarEstadoFin.Size = new System.Drawing.Size(97, 22);
            this.btnActualizarEstadoFin.TabIndex = 2;
            this.btnActualizarEstadoFin.Text = "Actualizar reporte";
            this.btnActualizarEstadoFin.UseVisualStyleBackColor = true;
            this.btnActualizarEstadoFin.Click += new System.EventHandler(this.btnActualizarEstadoFin_Click);
            // 
            // txtTasaEstadoFin
            // 
            this.txtTasaEstadoFin.Enabled = false;
            this.txtTasaEstadoFin.Location = new System.Drawing.Point(349, 6);
            this.txtTasaEstadoFin.MaxLength = 7;
            this.txtTasaEstadoFin.Name = "txtTasaEstadoFin";
            this.txtTasaEstadoFin.Size = new System.Drawing.Size(54, 20);
            this.txtTasaEstadoFin.TabIndex = 1;
            // 
            // chkEstadoFinCordibizado
            // 
            this.chkEstadoFinCordibizado.AutoSize = true;
            this.chkEstadoFinCordibizado.Location = new System.Drawing.Point(237, 8);
            this.chkEstadoFinCordibizado.Name = "chkEstadoFinCordibizado";
            this.chkEstadoFinCordibizado.Size = new System.Drawing.Size(110, 17);
            this.chkEstadoFinCordibizado.TabIndex = 0;
            this.chkEstadoFinCordibizado.Text = "cordobizar a tasa:";
            this.chkEstadoFinCordibizado.UseVisualStyleBackColor = true;
            this.chkEstadoFinCordibizado.CheckedChanged += new System.EventHandler(this.chkEstadoFinCordibizado_CheckedChanged);
            // 
            // lstEstadoFin
            // 
            this.lstEstadoFin.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFinIdCuenta,
            this.colFinCuenta,
            this.colFinDebeCS,
            this.colFinHaberCS,
            this.colFinDebeUS,
            this.colFinHaberUS});
            this.lstEstadoFin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEstadoFin.FullRowSelect = true;
            this.lstEstadoFin.GridLines = true;
            this.lstEstadoFin.HideSelection = false;
            this.lstEstadoFin.LargeImageList = this.lstImagenes;
            this.lstEstadoFin.Location = new System.Drawing.Point(0, 0);
            this.lstEstadoFin.Name = "lstEstadoFin";
            this.lstEstadoFin.Size = new System.Drawing.Size(708, 405);
            this.lstEstadoFin.SmallImageList = this.lstImagenes;
            this.lstEstadoFin.TabIndex = 0;
            this.lstEstadoFin.UseCompatibleStateImageBehavior = false;
            this.lstEstadoFin.View = System.Windows.Forms.View.Details;
            this.lstEstadoFin.Visible = false;
            // 
            // colFinIdCuenta
            // 
            this.colFinIdCuenta.Text = "IDCuenta";
            this.colFinIdCuenta.Width = 80;
            // 
            // colFinCuenta
            // 
            this.colFinCuenta.Text = "Cuenta";
            this.colFinCuenta.Width = 350;
            // 
            // colFinDebeCS
            // 
            this.colFinDebeCS.Text = "Debe C$";
            this.colFinDebeCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFinDebeCS.Width = 100;
            // 
            // colFinHaberCS
            // 
            this.colFinHaberCS.Text = "Haber C$";
            this.colFinHaberCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFinHaberCS.Width = 100;
            // 
            // colFinDebeUS
            // 
            this.colFinDebeUS.Text = "Debe U$";
            this.colFinDebeUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFinDebeUS.Width = 100;
            // 
            // colFinHaberUS
            // 
            this.colFinHaberUS.Text = "Haber U$";
            this.colFinHaberUS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFinHaberUS.Width = 100;
            // 
            // tmrAsync
            // 
            this.tmrAsync.Enabled = true;
            this.tmrAsync.Interval = 250;
            this.tmrAsync.Tick += new System.EventHandler(this.tmrAsync_Tick);
            // 
            // btnLimpiarMovs
            // 
            this.btnLimpiarMovs.Location = new System.Drawing.Point(331, 4);
            this.btnLimpiarMovs.Name = "btnLimpiarMovs";
            this.btnLimpiarMovs.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiarMovs.TabIndex = 21;
            this.btnLimpiarMovs.Text = "Limpiar";
            this.btnLimpiarMovs.UseVisualStyleBackColor = true;
            this.btnLimpiarMovs.Click += new System.EventHandler(this.btnLimpiarMovs_Click);
            // 
            // lblValidationResult
            // 
            this.lblValidationResult.Location = new System.Drawing.Point(367, 3);
            this.lblValidationResult.Name = "lblValidationResult";
            this.lblValidationResult.Size = new System.Drawing.Size(500, 13);
            this.lblValidationResult.TabIndex = 4;
            this.lblValidationResult.Text = "...";
            // 
            // FrmContab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 505);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmContab";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contabilidad";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmContab_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mnuArbolCuentas.ResumeLayout(false);
            this.tabCuentas.ResumeLayout(false);
            this.tabDiario.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.mnuComprobantes.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMovimientos)).EndInit();
            this.tabAuxiliar.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.mnuAuxiliarCtas.ResumeLayout(false);
            this.tabBalanzaC.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel1.PerformLayout();
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.tabEstadoFinanciero.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList lstImagenes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView arbCuentas;
        private System.Windows.Forms.TabControl tabCuentas;
        private System.Windows.Forms.TabPage tabDiario;
        private System.Windows.Forms.TabPage tabAuxiliar;
        private System.Windows.Forms.TabPage tabBalanzaC;
        private System.Windows.Forms.TabPage tabEstadoR;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker fechaIniDiario;
        private System.Windows.Forms.DateTimePicker fechaFinDiario;
        private System.Windows.Forms.Button btnOKDiario;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView lstComprobantes;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker fechaNuevoMov;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.DataGridView gridMovimientos;
        private System.Windows.Forms.Button btnFinalizarNuevoMov;
        private System.Windows.Forms.Label lblSumaHaber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSumaDebe;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnValidaMov;
        private System.Windows.Forms.ColumnHeader colIdCuentaDetalle;
        private System.Windows.Forms.ColumnHeader colNomCuentaDetalle;
        private System.Windows.Forms.ColumnHeader colDebeCS;
        private System.Windows.Forms.ColumnHeader colHaberCS;
        private System.Windows.Forms.ColumnHeader colTasacambio;
        private System.Windows.Forms.ColumnHeader colDebeUS;
        private System.Windows.Forms.ColumnHeader colHaberUS;
        private System.Windows.Forms.ColumnHeader colReferencias;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnOKAuxiliar;
        private System.Windows.Forms.DateTimePicker fechaFinAuxiliar;
        private System.Windows.Forms.DateTimePicker fechaIniAuxiliar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView lstAuxiliar;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader colIdMovimiento;
        private System.Windows.Forms.TabPage tabEstadoFinanciero;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkEstadoFinCordibizado;
        private System.Windows.Forms.TextBox txtTasaEstadoFin;
        private System.Windows.Forms.Button btnActualizarEstadoFin;
        private System.Windows.Forms.ListView lstEstadoFin;
        private System.Windows.Forms.ComboBox cmbNivelEstadoFinanciero;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader colFinCuenta;
        private System.Windows.Forms.ColumnHeader colFinDebeCS;
        private System.Windows.Forms.ColumnHeader colFinHaberCS;
        private System.Windows.Forms.ColumnHeader colFinDebeUS;
        private System.Windows.Forms.ColumnHeader colFinHaberUS;
        private System.Windows.Forms.CheckBox chkFinOmitirCeros;
        private System.Windows.Forms.Label lblFinDebeCS;
        private System.Windows.Forms.Label lblFinDebeUS;
        private System.Windows.Forms.Label lblFinHaberUS;
        private System.Windows.Forms.Label lblFinHaberCS;
        private System.Windows.Forms.Button btnNomina;
        private System.Windows.Forms.Button btnRecalcularBalance;
        private System.Windows.Forms.ColumnHeader colFinIdCuenta;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.Button btnOKBalanza;
        private System.Windows.Forms.DateTimePicker fechaFinBalanza;
        private System.Windows.Forms.DateTimePicker fechaIniBalanza;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView lstBalanza;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbNivelBalanza;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.Button btnExportarBalanza;
        private System.Windows.Forms.Button btnExportarComprobantes;
        private System.Windows.Forms.Button btnExportarAuxiliar;
        private System.Windows.Forms.Button btnExportarFinanciero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdCuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNomCuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDebe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHaber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEsUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReferencia;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ContextMenuStrip mnuComprobantes;
        private System.Windows.Forms.ToolStripMenuItem cambiarCuentaDeMovimientoToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuArbolCuentas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip mnuAuxiliarCtas;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btnAmortizaciones;
        private System.Windows.Forms.Button btnDepreciacion;
        private System.Windows.Forms.CheckBox chkOcultarDolares;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ToolStripMenuItem eliminarMovimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eliminarMovimientoToolStripMenuItem1;
        private System.Windows.Forms.CheckBox chkSubtotales;
        private System.Windows.Forms.Button tnSrvBasics;
        private System.Windows.Forms.Timer tmrAsync;
        private System.Windows.Forms.ToolStripMenuItem cuentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renombrarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crearHijaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualizarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarBalancesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem noMostrarToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mostrarCordobizadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mostrarEnMonedasSeparadasToolStripMenuItem1;
        private System.Windows.Forms.Button btnLimpiarMovs;
        private System.Windows.Forms.Label lblValidationResult;
    }
}

namespace Contab {
    partial class FrmManager {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManager));
            this.label1 = new System.Windows.Forms.Label();
            this.sourceViewInfo = new System.Windows.Forms.Panel();
            this.sourceInfoEditBtn = new System.Windows.Forms.PictureBox();
            this.sourceInfoLbl = new System.Windows.Forms.Label();
            this.sourceViewEdit = new System.Windows.Forms.Panel();
            this.sourceEditApply = new System.Windows.Forms.Button();
            this.sourceEditChannelCmb = new System.Windows.Forms.ComboBox();
            this.sourceEditPortTxt = new System.Windows.Forms.TextBox();
            this.sourceEditServerTxt = new System.Windows.Forms.TextBox();
            this.sourceEditProtocolCmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.versionCurLbl = new System.Windows.Forms.Label();
            this.removeBtn = new System.Windows.Forms.Button();
            this.installBtn = new System.Windows.Forms.Button();
            this.versionLatestLbl = new System.Windows.Forms.Label();
            this.actionCurLbl = new System.Windows.Forms.Label();
            this.btnOpenAsDriverGUI = new System.Windows.Forms.Button();
            this.lblEmbedded = new System.Windows.Forms.Label();
            this.sourceViewInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceInfoEditBtn)).BeginInit();
            this.sourceViewEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Origen:";
            // 
            // sourceViewInfo
            // 
            this.sourceViewInfo.Controls.Add(this.sourceInfoEditBtn);
            this.sourceViewInfo.Controls.Add(this.sourceInfoLbl);
            this.sourceViewInfo.Location = new System.Drawing.Point(72, 12);
            this.sourceViewInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceViewInfo.Name = "sourceViewInfo";
            this.sourceViewInfo.Size = new System.Drawing.Size(472, 43);
            this.sourceViewInfo.TabIndex = 1;
            // 
            // sourceInfoEditBtn
            // 
            this.sourceInfoEditBtn.Image = global::Contab.Properties.Resources.estado_proceso;
            this.sourceInfoEditBtn.Location = new System.Drawing.Point(439, 14);
            this.sourceInfoEditBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceInfoEditBtn.Name = "sourceInfoEditBtn";
            this.sourceInfoEditBtn.Size = new System.Drawing.Size(16, 16);
            this.sourceInfoEditBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.sourceInfoEditBtn.TabIndex = 1;
            this.sourceInfoEditBtn.TabStop = false;
            this.sourceInfoEditBtn.Click += new System.EventHandler(this.sourceInfoEditBtn_Click);
            // 
            // sourceInfoLbl
            // 
            this.sourceInfoLbl.Location = new System.Drawing.Point(3, 5);
            this.sourceInfoLbl.Name = "sourceInfoLbl";
            this.sourceInfoLbl.Size = new System.Drawing.Size(433, 31);
            this.sourceInfoLbl.TabIndex = 0;
            this.sourceInfoLbl.Text = "source (channel)";
            this.sourceInfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sourceViewEdit
            // 
            this.sourceViewEdit.Controls.Add(this.sourceEditApply);
            this.sourceViewEdit.Controls.Add(this.sourceEditChannelCmb);
            this.sourceViewEdit.Controls.Add(this.sourceEditPortTxt);
            this.sourceViewEdit.Controls.Add(this.sourceEditServerTxt);
            this.sourceViewEdit.Controls.Add(this.sourceEditProtocolCmb);
            this.sourceViewEdit.Controls.Add(this.label2);
            this.sourceViewEdit.Location = new System.Drawing.Point(72, 14);
            this.sourceViewEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceViewEdit.Name = "sourceViewEdit";
            this.sourceViewEdit.Size = new System.Drawing.Size(472, 43);
            this.sourceViewEdit.TabIndex = 2;
            this.sourceViewEdit.Visible = false;
            // 
            // sourceEditApply
            // 
            this.sourceEditApply.Location = new System.Drawing.Point(395, 7);
            this.sourceEditApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceEditApply.Name = "sourceEditApply";
            this.sourceEditApply.Size = new System.Drawing.Size(67, 30);
            this.sourceEditApply.TabIndex = 4;
            this.sourceEditApply.Text = "Aplicar";
            this.sourceEditApply.UseVisualStyleBackColor = true;
            this.sourceEditApply.Click += new System.EventHandler(this.sourceEditApply_Click);
            // 
            // sourceEditChannelCmb
            // 
            this.sourceEditChannelCmb.FormattingEnabled = true;
            this.sourceEditChannelCmb.Items.AddRange(new object[] {
            "release",
            "debug"});
            this.sourceEditChannelCmb.Location = new System.Drawing.Point(296, 11);
            this.sourceEditChannelCmb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceEditChannelCmb.Name = "sourceEditChannelCmb";
            this.sourceEditChannelCmb.Size = new System.Drawing.Size(92, 24);
            this.sourceEditChannelCmb.TabIndex = 3;
            this.sourceEditChannelCmb.Text = "release";
            // 
            // sourceEditPortTxt
            // 
            this.sourceEditPortTxt.Location = new System.Drawing.Point(252, 11);
            this.sourceEditPortTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceEditPortTxt.Name = "sourceEditPortTxt";
            this.sourceEditPortTxt.Size = new System.Drawing.Size(39, 22);
            this.sourceEditPortTxt.TabIndex = 2;
            this.sourceEditPortTxt.Text = "80";
            // 
            // sourceEditServerTxt
            // 
            this.sourceEditServerTxt.Location = new System.Drawing.Point(88, 11);
            this.sourceEditServerTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceEditServerTxt.Name = "sourceEditServerTxt";
            this.sourceEditServerTxt.Size = new System.Drawing.Size(153, 22);
            this.sourceEditServerTxt.TabIndex = 1;
            this.sourceEditServerTxt.Text = "mortegam.com";
            // 
            // sourceEditProtocolCmb
            // 
            this.sourceEditProtocolCmb.FormattingEnabled = true;
            this.sourceEditProtocolCmb.Items.AddRange(new object[] {
            "http://",
            "https://"});
            this.sourceEditProtocolCmb.Location = new System.Drawing.Point(7, 10);
            this.sourceEditProtocolCmb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceEditProtocolCmb.Name = "sourceEditProtocolCmb";
            this.sourceEditProtocolCmb.Size = new System.Drawing.Size(75, 24);
            this.sourceEditProtocolCmb.TabIndex = 0;
            this.sourceEditProtocolCmb.Text = "http://";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = ":";
            // 
            // versionCurLbl
            // 
            this.versionCurLbl.Location = new System.Drawing.Point(17, 143);
            this.versionCurLbl.Name = "versionCurLbl";
            this.versionCurLbl.Size = new System.Drawing.Size(283, 28);
            this.versionCurLbl.TabIndex = 3;
            this.versionCurLbl.Text = "Current version: 1.1.1";
            this.versionCurLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(307, 143);
            this.removeBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(237, 30);
            this.removeBtn.TabIndex = 5;
            this.removeBtn.Text = "Desinstalar";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // installBtn
            // 
            this.installBtn.Location = new System.Drawing.Point(307, 103);
            this.installBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(237, 30);
            this.installBtn.TabIndex = 7;
            this.installBtn.Text = "Instalar";
            this.installBtn.UseVisualStyleBackColor = true;
            this.installBtn.Click += new System.EventHandler(this.installBtn_Click);
            // 
            // versionLatestLbl
            // 
            this.versionLatestLbl.Location = new System.Drawing.Point(17, 103);
            this.versionLatestLbl.Name = "versionLatestLbl";
            this.versionLatestLbl.Size = new System.Drawing.Size(283, 28);
            this.versionLatestLbl.TabIndex = 6;
            this.versionLatestLbl.Text = "Ultima versión version: 1.1.1";
            this.versionLatestLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // actionCurLbl
            // 
            this.actionCurLbl.Location = new System.Drawing.Point(13, 180);
            this.actionCurLbl.Name = "actionCurLbl";
            this.actionCurLbl.Size = new System.Drawing.Size(531, 48);
            this.actionCurLbl.TabIndex = 8;
            this.actionCurLbl.Text = "Analizando...";
            this.actionCurLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOpenAsDriverGUI
            // 
            this.btnOpenAsDriverGUI.Location = new System.Drawing.Point(307, 66);
            this.btnOpenAsDriverGUI.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenAsDriverGUI.Name = "btnOpenAsDriverGUI";
            this.btnOpenAsDriverGUI.Size = new System.Drawing.Size(237, 30);
            this.btnOpenAsDriverGUI.TabIndex = 9;
            this.btnOpenAsDriverGUI.Text = "Abrir...";
            this.btnOpenAsDriverGUI.UseVisualStyleBackColor = true;
            this.btnOpenAsDriverGUI.Click += new System.EventHandler(this.btnOpenAsDriverGUI_Click);
            // 
            // lblEmbedded
            // 
            this.lblEmbedded.Location = new System.Drawing.Point(17, 68);
            this.lblEmbedded.Name = "lblEmbedded";
            this.lblEmbedded.Size = new System.Drawing.Size(283, 28);
            this.lblEmbedded.TabIndex = 10;
            this.lblEmbedded.Text = "Esta versión: 1.1.1";
            this.lblEmbedded.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 241);
            this.Controls.Add(this.lblEmbedded);
            this.Controls.Add(this.btnOpenAsDriverGUI);
            this.Controls.Add(this.actionCurLbl);
            this.Controls.Add(this.installBtn);
            this.Controls.Add(this.versionLatestLbl);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.versionCurLbl);
            this.Controls.Add(this.sourceViewEdit);
            this.Controls.Add(this.sourceViewInfo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Contab - Instalador";
            this.sourceViewInfo.ResumeLayout(false);
            this.sourceViewInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceInfoEditBtn)).EndInit();
            this.sourceViewEdit.ResumeLayout(false);
            this.sourceViewEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel sourceViewInfo;
        private System.Windows.Forms.PictureBox sourceInfoEditBtn;
        private System.Windows.Forms.Label sourceInfoLbl;
        private System.Windows.Forms.Panel sourceViewEdit;
        private System.Windows.Forms.Button sourceEditApply;
        private System.Windows.Forms.ComboBox sourceEditChannelCmb;
        private System.Windows.Forms.TextBox sourceEditPortTxt;
        private System.Windows.Forms.TextBox sourceEditServerTxt;
        private System.Windows.Forms.ComboBox sourceEditProtocolCmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label versionCurLbl;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.Label versionLatestLbl;
        private System.Windows.Forms.Label actionCurLbl;
        private System.Windows.Forms.Button btnOpenAsDriverGUI;
        private System.Windows.Forms.Label lblEmbedded;
    }
}
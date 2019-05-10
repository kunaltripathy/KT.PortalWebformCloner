namespace KT.PortalWebformCloner
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadWebforms = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCloneWebform = new System.Windows.Forms.ToolStripButton();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbWebformToClone = new System.Windows.Forms.ComboBox();
            this.lblWebformToClone = new System.Windows.Forms.Label();
            this.lblWebFormName = new System.Windows.Forms.Label();
            this.txWebformName = new System.Windows.Forms.TextBox();
            this.cbReadonly = new System.Windows.Forms.CheckBox();
            this.toolStripMenu.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.tsbLoadWebforms,
            this.toolStripSeparator1,
            this.tsbCloneWebform});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(897, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            this.toolStripMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenu_ItemClicked);
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(64, 28);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbLoadWebforms
            // 
            this.tsbLoadWebforms.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadWebforms.Image")));
            this.tsbLoadWebforms.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadWebforms.Name = "tsbLoadWebforms";
            this.tsbLoadWebforms.Size = new System.Drawing.Size(117, 28);
            this.tsbLoadWebforms.Text = "Load webforms";
            this.tsbLoadWebforms.Click += new System.EventHandler(this.tsbLoadWebforms_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // tsbCloneWebform
            // 
            this.tsbCloneWebform.Enabled = false;
            this.tsbCloneWebform.Image = ((System.Drawing.Image)(resources.GetObject("tsbCloneWebform.Image")));
            this.tsbCloneWebform.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCloneWebform.Name = "tsbCloneWebform";
            this.tsbCloneWebform.Size = new System.Drawing.Size(119, 28);
            this.tsbCloneWebform.Text = "Clone Webform";
            this.tsbCloneWebform.Click += new System.EventHandler(this.tsbCloneWebform_Click);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbWebformToClone);
            this.gbOptions.Controls.Add(this.lblWebformToClone);
            this.gbOptions.Controls.Add(this.lblWebFormName);
            this.gbOptions.Controls.Add(this.txWebformName);
            this.gbOptions.Controls.Add(this.cbReadonly);
            this.gbOptions.Location = new System.Drawing.Point(3, 56);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(765, 292);
            this.gbOptions.TabIndex = 5;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // cbWebformToClone
            // 
            this.cbWebformToClone.Enabled = false;
            this.cbWebformToClone.FormattingEnabled = true;
            this.cbWebformToClone.Location = new System.Drawing.Point(145, 39);
            this.cbWebformToClone.Name = "cbWebformToClone";
            this.cbWebformToClone.Size = new System.Drawing.Size(354, 21);
            this.cbWebformToClone.TabIndex = 4;
            this.cbWebformToClone.SelectedIndexChanged += new System.EventHandler(this.cbWebformToClone_SelectedIndexChanged);
            // 
            // lblWebformToClone
            // 
            this.lblWebformToClone.AutoSize = true;
            this.lblWebformToClone.Location = new System.Drawing.Point(6, 39);
            this.lblWebformToClone.Name = "lblWebformToClone";
            this.lblWebformToClone.Size = new System.Drawing.Size(99, 13);
            this.lblWebformToClone.TabIndex = 3;
            this.lblWebformToClone.Text = "WebForm To Clone";
            // 
            // lblWebFormName
            // 
            this.lblWebFormName.AutoSize = true;
            this.lblWebFormName.Location = new System.Drawing.Point(6, 105);
            this.lblWebFormName.Name = "lblWebFormName";
            this.lblWebFormName.Size = new System.Drawing.Size(117, 13);
            this.lblWebFormName.TabIndex = 2;
            this.lblWebFormName.Text = "Cloned Webform Name";
            // 
            // txWebformName
            // 
            this.txWebformName.Enabled = false;
            this.txWebformName.Location = new System.Drawing.Point(145, 98);
            this.txWebformName.Name = "txWebformName";
            this.txWebformName.Size = new System.Drawing.Size(354, 20);
            this.txWebformName.TabIndex = 1;
            this.txWebformName.TextChanged += new System.EventHandler(this.txWebformName_TextChanged);
            // 
            // cbReadonly
            // 
            this.cbReadonly.AutoSize = true;
            this.cbReadonly.Enabled = false;
            this.cbReadonly.Location = new System.Drawing.Point(9, 141);
            this.cbReadonly.Name = "cbReadonly";
            this.cbReadonly.Size = new System.Drawing.Size(402, 17);
            this.cbReadonly.TabIndex = 0;
            this.cbReadonly.Text = "Clone WebForm as Readonly (This will create webform steps in Readonly mode)";
            this.cbReadonly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbReadonly.UseVisualStyleBackColor = true;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(897, 609);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbLoadWebforms;
        private System.Windows.Forms.ToolStripButton tsbCloneWebform;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.ComboBox cbWebformToClone;
        private System.Windows.Forms.Label lblWebformToClone;
        private System.Windows.Forms.Label lblWebFormName;
        private System.Windows.Forms.TextBox txWebformName;
        private System.Windows.Forms.CheckBox cbReadonly;
    }
}

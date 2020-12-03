namespace jollyview
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuItemActions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHide = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayout
            // 
            this.flowLayout.AutoScroll = true;
            this.flowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout.Location = new System.Drawing.Point(0, 24);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Size = new System.Drawing.Size(484, 537);
            this.flowLayout.TabIndex = 0;
            this.flowLayout.Layout += new System.Windows.Forms.LayoutEventHandler(this.flowLayout_Layout);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemActions,
            this.menuItemHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(484, 24);
            this.menuStrip.TabIndex = 1;
            // 
            // menuItemActions
            // 
            this.menuItemActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShow,
            this.menuItemHide,
            this.menuItemSeparator1,
            this.menuItemRemove,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuItemActions.Name = "menuItemActions";
            this.menuItemActions.Size = new System.Drawing.Size(59, 20);
            this.menuItemActions.Text = "Actions";
            // 
            // menuItemShow
            // 
            this.menuItemShow.Name = "menuItemShow";
            this.menuItemShow.Size = new System.Drawing.Size(134, 22);
            this.menuItemShow.Text = "Show All";
            // 
            // menuItemHide
            // 
            this.menuItemHide.Name = "menuItemHide";
            this.menuItemHide.Size = new System.Drawing.Size(134, 22);
            this.menuItemHide.Text = "Hide All";
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // menuItemRemove
            // 
            this.menuItemRemove.Name = "menuItemRemove";
            this.menuItemRemove.Size = new System.Drawing.Size(134, 22);
            this.menuItemRemove.Text = "Remove All";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(131, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(134, 22);
            this.menuItemExit.Text = "Exit";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(107, 22);
            this.menuItemAbout.Text = "About";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(484, 561);
            this.Controls.Add(this.flowLayout);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "JollyView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayout;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemActions;
        private System.Windows.Forms.ToolStripMenuItem menuItemShow;
        private System.Windows.Forms.ToolStripMenuItem menuItemHide;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemRemove;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
    }
}


namespace SDataClientApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tabResourceProperties = new System.Windows.Forms.TabPage();
            this.resourceProperties1 = new SDataClientApp.ResourceProperties();
            this.tabSchema = new System.Windows.Forms.TabPage();
            this.resourceSchema1 = new SDataClientApp.ResourceSchema();
            this.tabTemplate = new System.Windows.Forms.TabPage();
            this.resourceTemplate1 = new SDataClientApp.ResourceTemplate();
            this.tabSingle = new System.Windows.Forms.TabPage();
            this.singleResource1 = new SDataClientApp.SingleResource();
            this.tabCollection = new System.Windows.Forms.TabPage();
            this.resourceCollection1 = new SDataClientApp.ResourceCollection();
            this.tabService = new System.Windows.Forms.TabPage();
            this.serviceConfig1 = new SDataClientApp.ServiceConfig();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.statusStrip.SuspendLayout();
            this.tabResourceProperties.SuspendLayout();
            this.tabSchema.SuspendLayout();
            this.tabTemplate.SuspendLayout();
            this.tabSingle.SuspendLayout();
            this.tabCollection.SuspendLayout();
            this.tabService.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // tabResourceProperties
            // 
            this.tabResourceProperties.Controls.Add(this.resourceProperties1);
            resources.ApplyResources(this.tabResourceProperties, "tabResourceProperties");
            this.tabResourceProperties.Name = "tabResourceProperties";
            this.tabResourceProperties.UseVisualStyleBackColor = true;
            // 
            // resourceProperties1
            // 
            resources.ApplyResources(this.resourceProperties1, "resourceProperties1");
            this.resourceProperties1.Name = "resourceProperties1";
            this.resourceProperties1.Service = null;
            this.resourceProperties1.StatusLabel = this.statusLabel;
            // 
            // tabSchema
            // 
            this.tabSchema.Controls.Add(this.resourceSchema1);
            resources.ApplyResources(this.tabSchema, "tabSchema");
            this.tabSchema.Name = "tabSchema";
            this.tabSchema.UseVisualStyleBackColor = true;
            // 
            // resourceSchema1
            // 
            resources.ApplyResources(this.resourceSchema1, "resourceSchema1");
            this.resourceSchema1.Name = "resourceSchema1";
            this.resourceSchema1.Service = null;
            this.resourceSchema1.StatusLabel = this.statusLabel;
            // 
            // tabTemplate
            // 
            this.tabTemplate.Controls.Add(this.resourceTemplate1);
            resources.ApplyResources(this.tabTemplate, "tabTemplate");
            this.tabTemplate.Name = "tabTemplate";
            this.tabTemplate.UseVisualStyleBackColor = true;
            // 
            // resourceTemplate1
            // 
            resources.ApplyResources(this.resourceTemplate1, "resourceTemplate1");
            this.resourceTemplate1.Name = "resourceTemplate1";
            this.resourceTemplate1.Service = null;
            this.resourceTemplate1.StatusLabel = this.statusLabel;
            // 
            // tabSingle
            // 
            this.tabSingle.Controls.Add(this.singleResource1);
            resources.ApplyResources(this.tabSingle, "tabSingle");
            this.tabSingle.Name = "tabSingle";
            this.tabSingle.UseVisualStyleBackColor = true;
            // 
            // singleResource1
            // 
            resources.ApplyResources(this.singleResource1, "singleResource1");
            this.singleResource1.Name = "singleResource1";
            this.singleResource1.Service = null;
            this.singleResource1.StatusLabel = this.statusLabel;
            // 
            // tabCollection
            // 
            this.tabCollection.Controls.Add(this.resourceCollection1);
            resources.ApplyResources(this.tabCollection, "tabCollection");
            this.tabCollection.Name = "tabCollection";
            this.tabCollection.UseVisualStyleBackColor = true;
            // 
            // resourceCollection1
            // 
            resources.ApplyResources(this.resourceCollection1, "resourceCollection1");
            this.resourceCollection1.Name = "resourceCollection1";
            this.resourceCollection1.Service = null;
            this.resourceCollection1.StatusLabel = this.statusLabel;
            // 
            // tabService
            // 
            this.tabService.Controls.Add(this.serviceConfig1);
            resources.ApplyResources(this.tabService, "tabService");
            this.tabService.Name = "tabService";
            this.tabService.UseVisualStyleBackColor = true;
            // 
            // serviceConfig1
            // 
            resources.ApplyResources(this.serviceConfig1, "serviceConfig1");
            this.serviceConfig1.Name = "serviceConfig1";
            this.serviceConfig1.Service = null;
            this.serviceConfig1.StatusLabel = this.statusLabel;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabService);
            this.tabControl1.Controls.Add(this.tabCollection);
            this.tabControl1.Controls.Add(this.tabSingle);
            this.tabControl1.Controls.Add(this.tabTemplate);
            this.tabControl1.Controls.Add(this.tabSchema);
            this.tabControl1.Controls.Add(this.tabResourceProperties);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabResourceProperties.ResumeLayout(false);
            this.tabSchema.ResumeLayout(false);
            this.tabTemplate.ResumeLayout(false);
            this.tabSingle.ResumeLayout(false);
            this.tabCollection.ResumeLayout(false);
            this.tabService.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabPage tabResourceProperties;
        private System.Windows.Forms.TabPage tabSchema;
        private System.Windows.Forms.TabPage tabTemplate;
        private System.Windows.Forms.TabPage tabSingle;
        private System.Windows.Forms.TabPage tabCollection;
        private System.Windows.Forms.TabPage tabService;
        private System.Windows.Forms.TabControl tabControl1;
        private ResourceProperties resourceProperties1;
        private ResourceSchema resourceSchema1;
        private ResourceTemplate resourceTemplate1;
        private SingleResource singleResource1;
        private ResourceCollection resourceCollection1;
        private ServiceConfig serviceConfig1;
    }
}

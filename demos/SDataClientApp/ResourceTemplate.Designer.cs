namespace SDataClientApp
{
    partial class ResourceTemplate
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label15;
            System.Windows.Forms.Button btnTemplateRead;
            System.Windows.Forms.Label label16;
            System.Windows.Forms.Label label17;
            this.templatePayloadGrid = new System.Windows.Forms.PropertyGrid();
            this.tbTemplateURL = new System.Windows.Forms.TextBox();
            this.tbTemplateResourceKind = new System.Windows.Forms.TextBox();
            label15 = new System.Windows.Forms.Label();
            btnTemplateRead = new System.Windows.Forms.Button();
            label16 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label15.Location = new System.Drawing.Point(8, 128);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(45, 13);
            label15.TabIndex = 5;
            label15.Text = "Payload";
            // 
            // btnTemplateRead
            // 
            btnTemplateRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnTemplateRead.Location = new System.Drawing.Point(8, 96);
            btnTemplateRead.Name = "btnTemplateRead";
            btnTemplateRead.Size = new System.Drawing.Size(75, 23);
            btnTemplateRead.TabIndex = 4;
            btnTemplateRead.Text = "Read";
            btnTemplateRead.UseVisualStyleBackColor = true;
            btnTemplateRead.Click += new System.EventHandler(this.btnTemplateRead_Click);
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label16.Location = new System.Drawing.Point(8, 56);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(128, 13);
            label16.TabIndex = 2;
            label16.Text = "Resource Template URL:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label17.Location = new System.Drawing.Point(8, 8);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(80, 13);
            label17.TabIndex = 0;
            label17.Text = "Resource Kind:";
            // 
            // templatePayloadGrid
            // 
            this.templatePayloadGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.templatePayloadGrid.CommandsVisibleIfAvailable = false;
            this.templatePayloadGrid.HelpVisible = false;
            this.templatePayloadGrid.Location = new System.Drawing.Point(8, 144);
            this.templatePayloadGrid.Name = "templatePayloadGrid";
            this.templatePayloadGrid.Size = new System.Drawing.Size(784, 448);
            this.templatePayloadGrid.TabIndex = 6;
            this.templatePayloadGrid.ToolbarVisible = false;
            // 
            // tbTemplateURL
            // 
            this.tbTemplateURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTemplateURL.Location = new System.Drawing.Point(8, 72);
            this.tbTemplateURL.Name = "tbTemplateURL";
            this.tbTemplateURL.Size = new System.Drawing.Size(784, 20);
            this.tbTemplateURL.TabIndex = 3;
            // 
            // tbTemplateResourceKind
            // 
            this.tbTemplateResourceKind.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "TemplateResourceKind", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbTemplateResourceKind.Location = new System.Drawing.Point(8, 24);
            this.tbTemplateResourceKind.Name = "tbTemplateResourceKind";
            this.tbTemplateResourceKind.Size = new System.Drawing.Size(128, 20);
            this.tbTemplateResourceKind.TabIndex = 1;
            this.tbTemplateResourceKind.Text = global::SDataClientApp.Properties.Settings.Default.TemplateResourceKind;
            this.tbTemplateResourceKind.TextChanged += new System.EventHandler(this.tbTemplateResourceKind_TextChanged);
            // 
            // ResourceTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.templatePayloadGrid);
            this.Controls.Add(label15);
            this.Controls.Add(btnTemplateRead);
            this.Controls.Add(label16);
            this.Controls.Add(this.tbTemplateURL);
            this.Controls.Add(this.tbTemplateResourceKind);
            this.Controls.Add(label17);
            this.Name = "ResourceTemplate";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid templatePayloadGrid;
        private System.Windows.Forms.TextBox tbTemplateURL;
        private System.Windows.Forms.TextBox tbTemplateResourceKind;
    }
}

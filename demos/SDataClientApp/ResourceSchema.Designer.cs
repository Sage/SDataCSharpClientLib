namespace SDataClientApp
{
    partial class ResourceSchema
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
            System.Windows.Forms.Button btnSchemaRead;
            System.Windows.Forms.Label label18;
            System.Windows.Forms.Label label19;
            this.lbSchemaFileName = new System.Windows.Forms.Label();
            this.tbSchemaFileName = new System.Windows.Forms.TextBox();
            this.btnSchemaSave = new System.Windows.Forms.Button();
            this.tbSchemaURL = new System.Windows.Forms.TextBox();
            this.tbSchemaResourceKind = new System.Windows.Forms.TextBox();
            btnSchemaRead = new System.Windows.Forms.Button();
            label18 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSchemaRead
            // 
            btnSchemaRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnSchemaRead.Location = new System.Drawing.Point(8, 96);
            btnSchemaRead.Name = "btnSchemaRead";
            btnSchemaRead.Size = new System.Drawing.Size(75, 23);
            btnSchemaRead.TabIndex = 4;
            btnSchemaRead.Text = "Read";
            btnSchemaRead.UseVisualStyleBackColor = true;
            btnSchemaRead.Click += new System.EventHandler(this.btnSchemaRead_Click);
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label18.Location = new System.Drawing.Point(8, 56);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(123, 13);
            label18.TabIndex = 2;
            label18.Text = "Resource Schema URL:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label19.Location = new System.Drawing.Point(8, 8);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(80, 13);
            label19.TabIndex = 0;
            label19.Text = "Resource Kind:";
            // 
            // lbSchemaFileName
            // 
            this.lbSchemaFileName.AutoSize = true;
            this.lbSchemaFileName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbSchemaFileName.Location = new System.Drawing.Point(8, 128);
            this.lbSchemaFileName.Name = "lbSchemaFileName";
            this.lbSchemaFileName.Size = new System.Drawing.Size(57, 13);
            this.lbSchemaFileName.TabIndex = 5;
            this.lbSchemaFileName.Text = "File Name:";
            this.lbSchemaFileName.Visible = false;
            // 
            // tbSchemaFileName
            // 
            this.tbSchemaFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSchemaFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "SchemaFileName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSchemaFileName.Location = new System.Drawing.Point(8, 144);
            this.tbSchemaFileName.Name = "tbSchemaFileName";
            this.tbSchemaFileName.Size = new System.Drawing.Size(784, 20);
            this.tbSchemaFileName.TabIndex = 6;
            this.tbSchemaFileName.Text = global::SDataClientApp.Properties.Settings.Default.SchemaFileName;
            this.tbSchemaFileName.Visible = false;
            // 
            // btnSchemaSave
            // 
            this.btnSchemaSave.Enabled = false;
            this.btnSchemaSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSchemaSave.Location = new System.Drawing.Point(8, 168);
            this.btnSchemaSave.Name = "btnSchemaSave";
            this.btnSchemaSave.Size = new System.Drawing.Size(75, 23);
            this.btnSchemaSave.TabIndex = 7;
            this.btnSchemaSave.Text = "Save";
            this.btnSchemaSave.UseVisualStyleBackColor = true;
            this.btnSchemaSave.Visible = false;
            this.btnSchemaSave.Click += new System.EventHandler(this.btnSchemaSave_Click);
            // 
            // tbSchemaURL
            // 
            this.tbSchemaURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSchemaURL.Location = new System.Drawing.Point(8, 72);
            this.tbSchemaURL.Name = "tbSchemaURL";
            this.tbSchemaURL.Size = new System.Drawing.Size(784, 20);
            this.tbSchemaURL.TabIndex = 3;
            // 
            // tbSchemaResourceKind
            // 
            this.tbSchemaResourceKind.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "SchemaResourceKind", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSchemaResourceKind.Location = new System.Drawing.Point(8, 24);
            this.tbSchemaResourceKind.Name = "tbSchemaResourceKind";
            this.tbSchemaResourceKind.Size = new System.Drawing.Size(128, 20);
            this.tbSchemaResourceKind.TabIndex = 1;
            this.tbSchemaResourceKind.Text = global::SDataClientApp.Properties.Settings.Default.SchemaResourceKind;
            this.tbSchemaResourceKind.TextChanged += new System.EventHandler(this.tbSchemaResourceKind_TextChanged);
            // 
            // ResourceSchema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSchemaFileName);
            this.Controls.Add(this.tbSchemaFileName);
            this.Controls.Add(this.btnSchemaSave);
            this.Controls.Add(btnSchemaRead);
            this.Controls.Add(label18);
            this.Controls.Add(this.tbSchemaURL);
            this.Controls.Add(label19);
            this.Controls.Add(this.tbSchemaResourceKind);
            this.Name = "ResourceSchema";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSchemaFileName;
        private System.Windows.Forms.TextBox tbSchemaURL;
        private System.Windows.Forms.TextBox tbSchemaResourceKind;
        private System.Windows.Forms.Label lbSchemaFileName;
        private System.Windows.Forms.Button btnSchemaSave;
    }
}

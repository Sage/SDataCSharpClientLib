namespace SDataClientApp
{
    partial class SingleResource
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
            System.Windows.Forms.Label label30;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Button btnSingleRead;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label13;
            this.tbSingleResourceInclude = new System.Windows.Forms.TextBox();
            this.btnSingleDelete = new System.Windows.Forms.Button();
            this.btnSingleCreate = new System.Windows.Forms.Button();
            this.btnSingleUpdate = new System.Windows.Forms.Button();
            this.tbSingleResourceSelector = new System.Windows.Forms.TextBox();
            this.singlePayloadGrid = new System.Windows.Forms.PropertyGrid();
            this.tbSingleURL = new System.Windows.Forms.TextBox();
            this.tbSingleResourceKind = new System.Windows.Forms.TextBox();
            label30 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            btnSingleRead = new System.Windows.Forms.Button();
            label12 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label30.Location = new System.Drawing.Point(284, 8);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(45, 13);
            label30.TabIndex = 4;
            label30.Text = "Include:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label14.Location = new System.Drawing.Point(148, 8);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(98, 13);
            label14.TabIndex = 2;
            label14.Text = "Resource Selector:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label11.Location = new System.Drawing.Point(8, 128);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(45, 13);
            label11.TabIndex = 12;
            label11.Text = "Payload";
            // 
            // btnSingleRead
            // 
            btnSingleRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnSingleRead.Location = new System.Drawing.Point(8, 96);
            btnSingleRead.Name = "btnSingleRead";
            btnSingleRead.Size = new System.Drawing.Size(75, 23);
            btnSingleRead.TabIndex = 8;
            btnSingleRead.Text = "Read";
            btnSingleRead.UseVisualStyleBackColor = true;
            btnSingleRead.Click += new System.EventHandler(this.btnSingleRead_Click);
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label12.Location = new System.Drawing.Point(8, 56);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(113, 13);
            label12.TabIndex = 6;
            label12.Text = "Single Resource URL:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label13.Location = new System.Drawing.Point(8, 8);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(80, 13);
            label13.TabIndex = 0;
            label13.Text = "Resource Kind:";
            // 
            // tbSingleResourceInclude
            // 
            this.tbSingleResourceInclude.Location = new System.Drawing.Point(284, 24);
            this.tbSingleResourceInclude.Name = "tbSingleResourceInclude";
            this.tbSingleResourceInclude.Size = new System.Drawing.Size(133, 20);
            this.tbSingleResourceInclude.TabIndex = 5;
            this.tbSingleResourceInclude.TextChanged += new System.EventHandler(this.tbSingleResourceInclude_TextChanged);
            // 
            // btnSingleDelete
            // 
            this.btnSingleDelete.Enabled = false;
            this.btnSingleDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSingleDelete.Location = new System.Drawing.Point(248, 96);
            this.btnSingleDelete.Name = "btnSingleDelete";
            this.btnSingleDelete.Size = new System.Drawing.Size(75, 23);
            this.btnSingleDelete.TabIndex = 11;
            this.btnSingleDelete.Text = "Delete";
            this.btnSingleDelete.UseVisualStyleBackColor = true;
            this.btnSingleDelete.Click += new System.EventHandler(this.btnSingleDelete_Click);
            // 
            // btnSingleCreate
            // 
            this.btnSingleCreate.Enabled = false;
            this.btnSingleCreate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSingleCreate.Location = new System.Drawing.Point(88, 96);
            this.btnSingleCreate.Name = "btnSingleCreate";
            this.btnSingleCreate.Size = new System.Drawing.Size(75, 23);
            this.btnSingleCreate.TabIndex = 9;
            this.btnSingleCreate.Text = "Create";
            this.btnSingleCreate.UseVisualStyleBackColor = true;
            this.btnSingleCreate.Click += new System.EventHandler(this.btnSingleCreate_Click);
            // 
            // btnSingleUpdate
            // 
            this.btnSingleUpdate.Enabled = false;
            this.btnSingleUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSingleUpdate.Location = new System.Drawing.Point(168, 96);
            this.btnSingleUpdate.Name = "btnSingleUpdate";
            this.btnSingleUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnSingleUpdate.TabIndex = 10;
            this.btnSingleUpdate.Text = "Update";
            this.btnSingleUpdate.UseVisualStyleBackColor = true;
            this.btnSingleUpdate.Click += new System.EventHandler(this.btnSingleUpdate_Click);
            // 
            // tbSingleResourceSelector
            // 
            this.tbSingleResourceSelector.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "SingleResourceSelector", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSingleResourceSelector.Location = new System.Drawing.Point(148, 24);
            this.tbSingleResourceSelector.Name = "tbSingleResourceSelector";
            this.tbSingleResourceSelector.Size = new System.Drawing.Size(128, 20);
            this.tbSingleResourceSelector.TabIndex = 3;
            this.tbSingleResourceSelector.Text = global::SDataClientApp.Properties.Settings.Default.SingleResourceSelector;
            this.tbSingleResourceSelector.TextChanged += new System.EventHandler(this.tbSingleResourceSelector_TextChanged);
            // 
            // singlePayloadGrid
            // 
            this.singlePayloadGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.singlePayloadGrid.CommandsVisibleIfAvailable = false;
            this.singlePayloadGrid.HelpVisible = false;
            this.singlePayloadGrid.Location = new System.Drawing.Point(8, 144);
            this.singlePayloadGrid.Name = "singlePayloadGrid";
            this.singlePayloadGrid.Size = new System.Drawing.Size(784, 448);
            this.singlePayloadGrid.TabIndex = 13;
            this.singlePayloadGrid.ToolbarVisible = false;
            // 
            // tbSingleURL
            // 
            this.tbSingleURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSingleURL.Location = new System.Drawing.Point(8, 72);
            this.tbSingleURL.Name = "tbSingleURL";
            this.tbSingleURL.Size = new System.Drawing.Size(784, 20);
            this.tbSingleURL.TabIndex = 7;
            // 
            // tbSingleResourceKind
            // 
            this.tbSingleResourceKind.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "SingleResourceKind", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSingleResourceKind.Location = new System.Drawing.Point(8, 24);
            this.tbSingleResourceKind.Name = "tbSingleResourceKind";
            this.tbSingleResourceKind.Size = new System.Drawing.Size(128, 20);
            this.tbSingleResourceKind.TabIndex = 1;
            this.tbSingleResourceKind.Text = global::SDataClientApp.Properties.Settings.Default.SingleResourceKind;
            this.tbSingleResourceKind.TextChanged += new System.EventHandler(this.tbSingleResourceKind_TextChanged);
            // 
            // SingleResource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(label30);
            this.Controls.Add(this.tbSingleResourceInclude);
            this.Controls.Add(this.btnSingleDelete);
            this.Controls.Add(this.btnSingleCreate);
            this.Controls.Add(this.btnSingleUpdate);
            this.Controls.Add(label14);
            this.Controls.Add(this.tbSingleResourceSelector);
            this.Controls.Add(this.singlePayloadGrid);
            this.Controls.Add(label11);
            this.Controls.Add(btnSingleRead);
            this.Controls.Add(label12);
            this.Controls.Add(this.tbSingleURL);
            this.Controls.Add(label13);
            this.Controls.Add(this.tbSingleResourceKind);
            this.Name = "SingleResource";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSingleResourceInclude;
        private System.Windows.Forms.TextBox tbSingleResourceSelector;
        private System.Windows.Forms.PropertyGrid singlePayloadGrid;
        private System.Windows.Forms.TextBox tbSingleURL;
        private System.Windows.Forms.TextBox tbSingleResourceKind;
        private System.Windows.Forms.Button btnSingleDelete;
        private System.Windows.Forms.Button btnSingleCreate;
        private System.Windows.Forms.Button btnSingleUpdate;

    }
}

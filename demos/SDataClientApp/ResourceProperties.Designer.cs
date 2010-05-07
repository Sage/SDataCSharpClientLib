namespace SDataClientApp
{
    partial class ResourceProperties
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
            System.Windows.Forms.Button btnClearProperties;
            System.Windows.Forms.Button btnAddProperty;
            System.Windows.Forms.Label label28;
            System.Windows.Forms.Label lableProperties;
            System.Windows.Forms.Label label26;
            System.Windows.Forms.Label label24;
            System.Windows.Forms.Button btnPropertiesRead;
            System.Windows.Forms.Label label25;
            this.rpGridEntries = new System.Windows.Forms.DataGridView();
            this.cbIsFeed = new System.Windows.Forms.CheckBox();
            this.tbResourceProperty = new System.Windows.Forms.TextBox();
            this.tbRPResourceSelector = new System.Windows.Forms.TextBox();
            this.tbResourcePropertiesURL = new System.Windows.Forms.TextBox();
            this.tbRPResourceKind = new System.Windows.Forms.TextBox();
            this.lbProperties = new System.Windows.Forms.ListBox();
            this.gridRPPayloads = new System.Windows.Forms.PropertyGrid();
            btnClearProperties = new System.Windows.Forms.Button();
            btnAddProperty = new System.Windows.Forms.Button();
            label28 = new System.Windows.Forms.Label();
            lableProperties = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            label24 = new System.Windows.Forms.Label();
            btnPropertiesRead = new System.Windows.Forms.Button();
            label25 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClearProperties
            // 
            btnClearProperties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnClearProperties.Location = new System.Drawing.Point(416, 52);
            btnClearProperties.Name = "btnClearProperties";
            btnClearProperties.Size = new System.Drawing.Size(95, 23);
            btnClearProperties.TabIndex = 7;
            btnClearProperties.Text = "Clear Properties";
            btnClearProperties.UseVisualStyleBackColor = true;
            btnClearProperties.Click += new System.EventHandler(this.btnClearProperties_Click);
            // 
            // btnAddProperty
            // 
            btnAddProperty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnAddProperty.Location = new System.Drawing.Point(416, 24);
            btnAddProperty.Name = "btnAddProperty";
            btnAddProperty.Size = new System.Drawing.Size(95, 23);
            btnAddProperty.TabIndex = 6;
            btnAddProperty.Text = "Add Property";
            btnAddProperty.UseVisualStyleBackColor = true;
            btnAddProperty.Click += new System.EventHandler(this.btnAddProperty_Click);
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label28.Location = new System.Drawing.Point(284, 8);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(98, 13);
            label28.TabIndex = 4;
            label28.Text = "Resource Property:";
            // 
            // lableProperties
            // 
            lableProperties.AutoSize = true;
            lableProperties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lableProperties.Location = new System.Drawing.Point(516, 8);
            lableProperties.Name = "lableProperties";
            lableProperties.Size = new System.Drawing.Size(57, 13);
            lableProperties.TabIndex = 8;
            lableProperties.Text = "Properties:";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label26.Location = new System.Drawing.Point(8, 104);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(131, 13);
            label26.TabIndex = 11;
            label26.Text = "Resource Properties URL:";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label24.Location = new System.Drawing.Point(148, 8);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(98, 13);
            label24.TabIndex = 2;
            label24.Text = "Resource Selector:";
            // 
            // btnPropertiesRead
            // 
            btnPropertiesRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnPropertiesRead.Location = new System.Drawing.Point(8, 144);
            btnPropertiesRead.Name = "btnPropertiesRead";
            btnPropertiesRead.Size = new System.Drawing.Size(75, 23);
            btnPropertiesRead.TabIndex = 13;
            btnPropertiesRead.Text = "Read";
            btnPropertiesRead.UseVisualStyleBackColor = true;
            btnPropertiesRead.Click += new System.EventHandler(this.btnPropertiesRead_Click);
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label25.Location = new System.Drawing.Point(8, 8);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(80, 13);
            label25.TabIndex = 0;
            label25.Text = "Resource Kind:";
            // 
            // rpGridEntries
            // 
            this.rpGridEntries.AllowUserToAddRows = false;
            this.rpGridEntries.AllowUserToDeleteRows = false;
            this.rpGridEntries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rpGridEntries.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.rpGridEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rpGridEntries.Location = new System.Drawing.Point(8, 176);
            this.rpGridEntries.MultiSelect = false;
            this.rpGridEntries.Name = "rpGridEntries";
            this.rpGridEntries.ReadOnly = true;
            this.rpGridEntries.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.rpGridEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.rpGridEntries.Size = new System.Drawing.Size(784, 157);
            this.rpGridEntries.TabIndex = 14;
            // 
            // cbIsFeed
            // 
            this.cbIsFeed.AutoSize = true;
            this.cbIsFeed.Checked = true;
            this.cbIsFeed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsFeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbIsFeed.Location = new System.Drawing.Point(8, 56);
            this.cbIsFeed.Name = "cbIsFeed";
            this.cbIsFeed.Size = new System.Drawing.Size(61, 17);
            this.cbIsFeed.TabIndex = 10;
            this.cbIsFeed.Text = "Is Feed";
            this.cbIsFeed.UseVisualStyleBackColor = true;
            // 
            // tbResourceProperty
            // 
            this.tbResourceProperty.Location = new System.Drawing.Point(284, 24);
            this.tbResourceProperty.Name = "tbResourceProperty";
            this.tbResourceProperty.Size = new System.Drawing.Size(128, 20);
            this.tbResourceProperty.TabIndex = 5;
            // 
            // tbRPResourceSelector
            // 
            this.tbRPResourceSelector.Location = new System.Drawing.Point(148, 24);
            this.tbRPResourceSelector.Name = "tbRPResourceSelector";
            this.tbRPResourceSelector.Size = new System.Drawing.Size(128, 20);
            this.tbRPResourceSelector.TabIndex = 3;
            this.tbRPResourceSelector.TextChanged += new System.EventHandler(this.tbRPResourceSelector_TextChanged);
            // 
            // tbResourcePropertiesURL
            // 
            this.tbResourcePropertiesURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResourcePropertiesURL.Location = new System.Drawing.Point(8, 120);
            this.tbResourcePropertiesURL.Name = "tbResourcePropertiesURL";
            this.tbResourcePropertiesURL.Size = new System.Drawing.Size(784, 20);
            this.tbResourcePropertiesURL.TabIndex = 12;
            // 
            // tbRPResourceKind
            // 
            this.tbRPResourceKind.Location = new System.Drawing.Point(8, 24);
            this.tbRPResourceKind.Name = "tbRPResourceKind";
            this.tbRPResourceKind.Size = new System.Drawing.Size(128, 20);
            this.tbRPResourceKind.TabIndex = 1;
            this.tbRPResourceKind.Text = global::SDataClientApp.Properties.Settings.Default.SingleResourceKind;
            this.tbRPResourceKind.TextChanged += new System.EventHandler(this.tbRPResourceKind_TextChanged);
            // 
            // lbProperties
            // 
            this.lbProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProperties.FormattingEnabled = true;
            this.lbProperties.Location = new System.Drawing.Point(520, 24);
            this.lbProperties.Name = "lbProperties";
            this.lbProperties.Size = new System.Drawing.Size(272, 82);
            this.lbProperties.TabIndex = 9;
            // 
            // gridRPPayloads
            // 
            this.gridRPPayloads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRPPayloads.CommandsVisibleIfAvailable = false;
            this.gridRPPayloads.HelpVisible = false;
            this.gridRPPayloads.Location = new System.Drawing.Point(8, 360);
            this.gridRPPayloads.Name = "gridRPPayloads";
            this.gridRPPayloads.Size = new System.Drawing.Size(784, 231);
            this.gridRPPayloads.TabIndex = 15;
            this.gridRPPayloads.ToolbarVisible = false;
            // 
            // ResourceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rpGridEntries);
            this.Controls.Add(this.cbIsFeed);
            this.Controls.Add(btnClearProperties);
            this.Controls.Add(btnAddProperty);
            this.Controls.Add(label28);
            this.Controls.Add(this.tbResourceProperty);
            this.Controls.Add(this.tbRPResourceSelector);
            this.Controls.Add(this.tbResourcePropertiesURL);
            this.Controls.Add(this.tbRPResourceKind);
            this.Controls.Add(lableProperties);
            this.Controls.Add(this.lbProperties);
            this.Controls.Add(label26);
            this.Controls.Add(label24);
            this.Controls.Add(this.gridRPPayloads);
            this.Controls.Add(btnPropertiesRead);
            this.Controls.Add(label25);
            this.Name = "ResourceProperties";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView rpGridEntries;
        private System.Windows.Forms.TextBox tbResourceProperty;
        private System.Windows.Forms.TextBox tbRPResourceSelector;
        private System.Windows.Forms.TextBox tbResourcePropertiesURL;
        private System.Windows.Forms.TextBox tbRPResourceKind;
        private System.Windows.Forms.ListBox lbProperties;
        private System.Windows.Forms.PropertyGrid gridRPPayloads;
        private System.Windows.Forms.CheckBox cbIsFeed;
    }
}

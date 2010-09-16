namespace SDataClientApp
{
    partial class ServiceConfig
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
            System.Windows.Forms.Button btnInitialize;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label lblContract;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label lblDataSet;
            System.Windows.Forms.GroupBox groupBox2;
            this.tbDataSet = new System.Windows.Forms.TextBox();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbContract = new System.Windows.Forms.TextBox();
            this.tbApplication = new System.Windows.Forms.TextBox();
            this.tbVirtualDirectory = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.cbProtocol = new System.Windows.Forms.ComboBox();
            btnInitialize = new System.Windows.Forms.Button();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            lblContract = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lblDataSet = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // btnInitialize
            // 
            btnInitialize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnInitialize.Location = new System.Drawing.Point(392, 332);
            btnInitialize.Name = "btnInitialize";
            btnInitialize.Size = new System.Drawing.Size(75, 23);
            btnInitialize.TabIndex = 20;
            btnInitialize.Text = "Initialize";
            btnInitialize.UseVisualStyleBackColor = true;
            btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label7.Location = new System.Drawing.Point(12, 288);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(32, 13);
            label7.TabIndex = 18;
            label7.Text = "URL:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label6.Location = new System.Drawing.Point(288, 32);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(56, 13);
            label6.TabIndex = 3;
            label6.Text = "Password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(20, 32);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(63, 13);
            label5.TabIndex = 1;
            label5.Text = "User Name:";
            // 
            // lblContract
            // 
            lblContract.AutoSize = true;
            lblContract.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lblContract.Location = new System.Drawing.Point(20, 228);
            lblContract.Name = "lblContract";
            lblContract.Size = new System.Drawing.Size(50, 13);
            lblContract.TabIndex = 10;
            lblContract.Text = "Contract:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(288, 184);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(62, 13);
            label4.TabIndex = 14;
            label4.Text = "Application:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(20, 184);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(84, 13);
            label3.TabIndex = 8;
            label3.Text = "Virtual Directory:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(288, 140);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(41, 13);
            label2.TabIndex = 12;
            label2.Text = "Server:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(20, 140);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 13);
            label1.TabIndex = 6;
            label1.Text = "Protocol:";
            // 
            // groupBox1
            // 
            groupBox1.Location = new System.Drawing.Point(8, 116);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(473, 157);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "URL Settings";
            // 
            // tbDataSet
            // 
            this.tbDataSet.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "DataSetName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbDataSet.Location = new System.Drawing.Point(288, 244);
            this.tbDataSet.Name = "tbDataSet";
            this.tbDataSet.Size = new System.Drawing.Size(178, 20);
            this.tbDataSet.TabIndex = 17;
            this.tbDataSet.Text = global::SDataClientApp.Properties.Settings.Default.DataSetName;
            this.tbDataSet.TextChanged += new System.EventHandler(this.tbDataSet_TextChanged);
            // 
            // lblDataSet
            // 
            lblDataSet.AutoSize = true;
            lblDataSet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            lblDataSet.Location = new System.Drawing.Point(288, 228);
            lblDataSet.Name = "lblDataSet";
            lblDataSet.Size = new System.Drawing.Size(49, 13);
            lblDataSet.TabIndex = 16;
            lblDataSet.Text = "DataSet:";
            // 
            // groupBox2
            // 
            groupBox2.Location = new System.Drawing.Point(8, 8);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(473, 91);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Authentication";
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(12, 304);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(456, 20);
            this.tbURL.TabIndex = 19;
            // 
            // tbPassword
            // 
            this.tbPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbPassword.Location = new System.Drawing.Point(288, 48);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(178, 20);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.Text = global::SDataClientApp.Properties.Settings.Default.Password;
            // 
            // tbUserName
            // 
            this.tbUserName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "UserName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbUserName.Location = new System.Drawing.Point(20, 48);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(178, 20);
            this.tbUserName.TabIndex = 2;
            this.tbUserName.Text = global::SDataClientApp.Properties.Settings.Default.UserName;
            // 
            // tbContract
            // 
            this.tbContract.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "ContractName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbContract.Location = new System.Drawing.Point(20, 244);
            this.tbContract.Name = "tbContract";
            this.tbContract.Size = new System.Drawing.Size(178, 20);
            this.tbContract.TabIndex = 11;
            this.tbContract.Text = global::SDataClientApp.Properties.Settings.Default.ContractName;
            this.tbContract.TextChanged += new System.EventHandler(this.tbContract_TextChanged);
            // 
            // tbApplication
            // 
            this.tbApplication.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "ApplicationName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbApplication.Location = new System.Drawing.Point(288, 200);
            this.tbApplication.Name = "tbApplication";
            this.tbApplication.Size = new System.Drawing.Size(178, 20);
            this.tbApplication.TabIndex = 15;
            this.tbApplication.Text = global::SDataClientApp.Properties.Settings.Default.ApplicationName;
            this.tbApplication.TextChanged += new System.EventHandler(this.tbApplication_TextChanged);
            // 
            // tbVirtualDirectory
            // 
            this.tbVirtualDirectory.Location = new System.Drawing.Point(20, 200);
            this.tbVirtualDirectory.Name = "tbVirtualDirectory";
            this.tbVirtualDirectory.ReadOnly = true;
            this.tbVirtualDirectory.Size = new System.Drawing.Size(178, 20);
            this.tbVirtualDirectory.TabIndex = 9;
            this.tbVirtualDirectory.Text = "sdata";
            // 
            // tbServer
            // 
            this.tbServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "ServerName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbServer.Location = new System.Drawing.Point(288, 156);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(178, 20);
            this.tbServer.TabIndex = 13;
            this.tbServer.Text = global::SDataClientApp.Properties.Settings.Default.ServerName;
            this.tbServer.TextChanged += new System.EventHandler(this.tbServer_TextChanged);
            // 
            // cbProtocol
            // 
            this.cbProtocol.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "Protocol", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocol.FormattingEnabled = true;
            this.cbProtocol.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cbProtocol.Location = new System.Drawing.Point(20, 156);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(178, 21);
            this.cbProtocol.TabIndex = 7;
            this.cbProtocol.Text = global::SDataClientApp.Properties.Settings.Default.Protocol;
            this.cbProtocol.SelectedIndexChanged += new System.EventHandler(this.cbProtocol_SelectedIndexChanged);
            // 
            // ServiceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(btnInitialize);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(label7);
            this.Controls.Add(label6);
            this.Controls.Add(label5);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.tbContract);
            this.Controls.Add(lblContract);
            this.Controls.Add(this.tbApplication);
            this.Controls.Add(label4);
            this.Controls.Add(this.tbVirtualDirectory);
            this.Controls.Add(label3);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.cbProtocol);
            this.Controls.Add(this.tbDataSet);
            this.Controls.Add(lblDataSet);
            this.Controls.Add(groupBox1);
            this.Controls.Add(groupBox2);
            this.Name = "ServiceConfig";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbContract;
        private System.Windows.Forms.TextBox tbApplication;
        private System.Windows.Forms.TextBox tbVirtualDirectory;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.ComboBox cbProtocol;
        private System.Windows.Forms.TextBox tbDataSet;

    }
}

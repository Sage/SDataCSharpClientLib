using Sage.SData.Client.Core;

namespace SDataClientApp
{
    partial class ResourceCollection
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
            System.Windows.Forms.Label label23;
            System.Windows.Forms.Label label22;
            System.Windows.Forms.Label label29;
            System.Windows.Forms.Label label27;
            System.Windows.Forms.Button btnReaderRead;
            System.Windows.Forms.Button btnCollectionRead;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.Label label10;
            this.atomEntryGrid = new System.Windows.Forms.DataGridView();
            this.collectionPayloadGrid = new System.Windows.Forms.PropertyGrid();
            this.tbCurrentItem = new System.Windows.Forms.TextBox();
            this.tbReaderCount = new System.Windows.Forms.TextBox();
            this.numStartIndex = new System.Windows.Forms.NumericUpDown();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.tbCollectionURL = new System.Windows.Forms.TextBox();
            this.tbCollectionResourceKind = new System.Windows.Forms.TextBox();
            label23 = new System.Windows.Forms.Label();
            label22 = new System.Windows.Forms.Label();
            label29 = new System.Windows.Forms.Label();
            label27 = new System.Windows.Forms.Label();
            btnReaderRead = new System.Windows.Forms.Button();
            btnCollectionRead = new System.Windows.Forms.Button();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            label10 = new System.Windows.Forms.Label();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label23.Location = new System.Drawing.Point(208, 8);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(61, 13);
            label23.TabIndex = 2;
            label23.Text = "Start Index:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label22.Location = new System.Drawing.Point(296, 8);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(38, 13);
            label22.TabIndex = 4;
            label22.Text = "Count:";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label29.Location = new System.Drawing.Point(488, 8);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(67, 13);
            label29.TabIndex = 8;
            label29.Text = "Current Item:";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label27.Location = new System.Drawing.Point(380, 8);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(59, 13);
            label27.TabIndex = 6;
            label27.Text = "Total Items";
            // 
            // btnReaderRead
            // 
            btnReaderRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnReaderRead.Location = new System.Drawing.Point(112, 100);
            btnReaderRead.Name = "btnReaderRead";
            btnReaderRead.Size = new System.Drawing.Size(100, 23);
            btnReaderRead.TabIndex = 13;
            btnReaderRead.Text = "Read Reader";
            btnReaderRead.UseVisualStyleBackColor = true;
            btnReaderRead.Click += new System.EventHandler(this.btnReaderRead_Click);
            // 
            // btnCollectionRead
            // 
            btnCollectionRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            btnCollectionRead.Location = new System.Drawing.Point(8, 100);
            btnCollectionRead.Name = "btnCollectionRead";
            btnCollectionRead.Size = new System.Drawing.Size(100, 23);
            btnCollectionRead.TabIndex = 12;
            btnCollectionRead.Text = "Read Normal";
            btnCollectionRead.UseVisualStyleBackColor = true;
            btnCollectionRead.Click += new System.EventHandler(this.btnCollectionRead_Click);
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label9.Location = new System.Drawing.Point(8, 56);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(130, 13);
            label9.TabIndex = 10;
            label9.Text = "Resource Collection URL:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label8.Location = new System.Drawing.Point(8, 8);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(80, 13);
            label8.TabIndex = 0;
            label8.Text = "Resource Kind:";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            splitContainer1.Location = new System.Drawing.Point(8, 132);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.atomEntryGrid);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.collectionPayloadGrid);
            splitContainer1.Panel2.Controls.Add(label10);
            splitContainer1.Size = new System.Drawing.Size(785, 461);
            splitContainer1.SplitterDistance = 227;
            splitContainer1.TabIndex = 18;
            // 
            // atomEntryGrid
            // 
            this.atomEntryGrid.AllowUserToAddRows = false;
            this.atomEntryGrid.AllowUserToDeleteRows = false;
            this.atomEntryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.atomEntryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.atomEntryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atomEntryGrid.Location = new System.Drawing.Point(0, 0);
            this.atomEntryGrid.MultiSelect = false;
            this.atomEntryGrid.Name = "atomEntryGrid";
            this.atomEntryGrid.ReadOnly = true;
            this.atomEntryGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.atomEntryGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.atomEntryGrid.Size = new System.Drawing.Size(785, 227);
            this.atomEntryGrid.TabIndex = 0;
            this.atomEntryGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.atomEntryGrid_CellContentClick);
            // 
            // collectionPayloadGrid
            // 
            this.collectionPayloadGrid.CommandsVisibleIfAvailable = false;
            this.collectionPayloadGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collectionPayloadGrid.HelpVisible = false;
            this.collectionPayloadGrid.Location = new System.Drawing.Point(0, 16);
            this.collectionPayloadGrid.Name = "collectionPayloadGrid";
            this.collectionPayloadGrid.Size = new System.Drawing.Size(785, 214);
            this.collectionPayloadGrid.TabIndex = 1;
            this.collectionPayloadGrid.ToolbarVisible = false;
            // 
            // label10
            // 
            label10.Dock = System.Windows.Forms.DockStyle.Top;
            label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label10.Location = new System.Drawing.Point(0, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(785, 16);
            label10.TabIndex = 0;
            label10.Text = "Payload";
            // 
            // tbCurrentItem
            // 
            this.tbCurrentItem.Enabled = false;
            this.tbCurrentItem.Location = new System.Drawing.Point(488, 24);
            this.tbCurrentItem.Name = "tbCurrentItem";
            this.tbCurrentItem.Size = new System.Drawing.Size(100, 20);
            this.tbCurrentItem.TabIndex = 9;
            // 
            // tbReaderCount
            // 
            this.tbReaderCount.Location = new System.Drawing.Point(380, 24);
            this.tbReaderCount.Name = "tbReaderCount";
            this.tbReaderCount.Size = new System.Drawing.Size(100, 20);
            this.tbReaderCount.TabIndex = 7;
            // 
            // numStartIndex
            // 
            this.numStartIndex.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::SDataClientApp.Properties.Settings.Default, "CollectionStartIndex", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numStartIndex.Location = new System.Drawing.Point(208, 24);
            this.numStartIndex.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numStartIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartIndex.Name = "numStartIndex";
            this.numStartIndex.Size = new System.Drawing.Size(80, 20);
            this.numStartIndex.TabIndex = 3;
            this.numStartIndex.Value = global::SDataClientApp.Properties.Settings.Default.CollectionStartIndex;
            // 
            // numCount
            // 
            this.numCount.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::SDataClientApp.Properties.Settings.Default, "CollectionCount", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numCount.Location = new System.Drawing.Point(296, 24);
            this.numCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(80, 20);
            this.numCount.TabIndex = 5;
            this.numCount.Value = global::SDataClientApp.Properties.Settings.Default.CollectionCount;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLast.Location = new System.Drawing.Point(716, 100);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(75, 23);
            this.btnLast.TabIndex = 17;
            this.btnLast.Text = "Last >>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext.Location = new System.Drawing.Point(640, 100);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 16;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrevious.Location = new System.Drawing.Point(564, 100);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 15;
            this.btnPrevious.Text = "< Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirst.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFirst.Location = new System.Drawing.Point(488, 100);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(75, 23);
            this.btnFirst.TabIndex = 14;
            this.btnFirst.Text = "<< First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // tbCollectionURL
            // 
            this.tbCollectionURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCollectionURL.Location = new System.Drawing.Point(8, 72);
            this.tbCollectionURL.Name = "tbCollectionURL";
            this.tbCollectionURL.Size = new System.Drawing.Size(784, 20);
            this.tbCollectionURL.TabIndex = 11;
            // 
            // tbCollectionResourceKind
            // 
            this.tbCollectionResourceKind.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SDataClientApp.Properties.Settings.Default, "CollectionResourceKind", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbCollectionResourceKind.Location = new System.Drawing.Point(8, 24);
            this.tbCollectionResourceKind.Name = "tbCollectionResourceKind";
            this.tbCollectionResourceKind.Size = new System.Drawing.Size(128, 20);
            this.tbCollectionResourceKind.TabIndex = 1;
            this.tbCollectionResourceKind.Text = global::SDataClientApp.Properties.Settings.Default.CollectionResourceKind;
            this.tbCollectionResourceKind.TextChanged += new System.EventHandler(this.tbCollectionResourceKind_TextChanged);
            // 
            // ResourceCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(label29);
            this.Controls.Add(this.tbCurrentItem);
            this.Controls.Add(label27);
            this.Controls.Add(this.tbReaderCount);
            this.Controls.Add(btnReaderRead);
            this.Controls.Add(this.numStartIndex);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(btnCollectionRead);
            this.Controls.Add(label9);
            this.Controls.Add(this.tbCollectionURL);
            this.Controls.Add(label23);
            this.Controls.Add(label22);
            this.Controls.Add(label8);
            this.Controls.Add(this.tbCollectionResourceKind);
            this.Controls.Add(splitContainer1);
            this.Name = "ResourceCollection";
            this.Size = new System.Drawing.Size(800, 600);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.DataGridView atomEntryGrid;
        private System.Windows.Forms.PropertyGrid collectionPayloadGrid;
        private System.Windows.Forms.TextBox tbCurrentItem;
        private System.Windows.Forms.TextBox tbReaderCount;
        private System.Windows.Forms.NumericUpDown numStartIndex;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.TextBox tbCollectionURL;
        private System.Windows.Forms.TextBox tbCollectionResourceKind;

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using SDataClientApp.Properties;

namespace SDataClientApp
{
    public partial class Form1 : Form
    {
        private SDataService _sdataService;
        private SDataResourceCollectionRequest _sdataResourceCollectionRequest;
        private SDataSingleResourceRequest _sdataSingleResourceRequest;
        private SDataTemplateResourceRequest _sdataTemplateResourceRequest;
        private SDataResourceSchemaRequest _sdataResourceSchemaRequest;
        private SDataResourcePropertyRequest _sdataResourcePropertyRequest;

        private XmlSchema _schema;
        private string _url;
        private AtomFeed _feed;

        public Form1()
        {
            InitializeComponent();
            FormatURL();
        }

        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormatURL();
        }

        private void tbServer_TextChanged(object sender, EventArgs e)
        {
            FormatURL();
        }

        private void FormatURL()
        {
            try
            {
                var server = tbServer.Text;
                var pos = server.IndexOf(':');
                UrlBuilder builder;
                int port;

                if (pos >= 0 && int.TryParse(server.Substring(pos + 1), out port))
                {
                    server = server.Substring(0, pos);
                    builder = new UrlBuilder(cbProtocol.Text, server, port);
                }
                else
                {
                    builder = new UrlBuilder(cbProtocol.Text, server);
                }

                builder.PathSegments.Add(tbVirtualDirectory.Text);
                builder.PathSegments.Add(tbApplication.Text);
                builder.PathSegments.Add(tbContract.Text);
                builder.PathSegments.Add(tbDataSet.Text);

                _url = builder.ToString();
                tbURL.Text = _url;
            }
            catch (UriFormatException)
            {
            }
        }

        private void tbApplication_TextChanged(object sender, EventArgs e)
        {
            FormatURL();
        }

        private void tbContract_TextChanged(object sender, EventArgs e)
        {
            FormatURL();
        }

        private void tbDataSet_TextChanged(object sender, EventArgs e)
        {
            FormatURL();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            // create the SDataService with url, user name, and password
            _sdataService = new SDataService(tbURL.Text, tbUserName.Text, tbPassword.Text);
            // initialize it
            _sdataService.Initialize();
            statusLabel.Text = Resources.statusInitializationComplete;
        }

        //======================================================

        private void tabCollection_Enter(object sender, EventArgs e)
        {
            tbCollectionURL.Text = string.Format("{0}/{1}", _url, tbCollectionResourceKind.Text);
        }

        private void tbCollectionResourceKind_TextChanged(object sender, EventArgs e)
        {
            tbCollectionURL.Text = string.Format("{0}/{1}", _url, tbCollectionResourceKind.Text);
        }

        private void btnCollectionRead_Click(object sender, EventArgs e)
        {
            btnCollectionRead.Enabled = false;
            btnReaderRead.Enabled = true;
            collectionPayloadGrid.SelectedObject = null;
            UpdateCollection();
        }

        private void btnReaderRead_Click(object sender, EventArgs e)
        {
            try
            {
                // create the sSDataResourceCollectionRequest
                _sdataResourceCollectionRequest = new SDataResourceCollectionRequest(_sdataService);
                // set the resource kind
                _sdataResourceCollectionRequest.ResourceKind = tbCollectionResourceKind.Text;
                _sdataResourceCollectionRequest.StartIndex = (int) numStartIndex.Value;
                _sdataResourceCollectionRequest.Count = (int) numCount.Value;
                // read the feed
                _feed = _sdataResourceCollectionRequest.Read();

                lblFeedReader.Text = "Reader Ready";
                btnCollectionRead.Enabled = true;
                btnReaderRead.Enabled = false;

                if (_sdataResourceCollectionRequest.Reader == null)
                {
                    _sdataResourceCollectionRequest.Read(_feed);
                }

                tbReaderCount.Text = Convert.ToString(_sdataResourceCollectionRequest.Reader.Count);

                UpdateReaderGrid();
            }
            catch (SDataClientException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void atomEntryGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = atomEntryGrid.SelectedRows[0].Index;

            // get the atomentry for the row selected from the ffed
            var entry = ((IList<AtomEntry>) _feed.Entries)[index];

            // show it in the grid
            collectionPayloadGrid.SelectedObject = entry.GetSDataPayload();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (_sdataResourceCollectionRequest.Reader != null)
            {
                _sdataResourceCollectionRequest.Reader.First();
                UpdateReaderGrid();
            }
            else
            {
                NavigateCollection("first");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_sdataResourceCollectionRequest.Reader != null)
            {
                _sdataResourceCollectionRequest.Reader.Previous();
                UpdateReaderGrid();
            }
            else
            {
                NavigateCollection("previous");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_sdataResourceCollectionRequest.Reader != null)
            {
                if (_sdataResourceCollectionRequest.Reader.MoveNext())
                {
                    UpdateReaderGrid();
                }
                else
                {
                    MessageBox.Show("No entries available");
                }
            }
            else
            {
                NavigateCollection("next");
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (_sdataResourceCollectionRequest.Reader != null)
            {
                if (_sdataResourceCollectionRequest.Reader.Last())
                {
                    UpdateReaderGrid();
                }
                else
                {
                    MessageBox.Show("No entries available");
                }
            }
            else
            {
                NavigateCollection("last");
            }
        }

        private void tbCurrentItem_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Convert.ToInt32(tbCurrentItem.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("You must enter a number");
                return;
            }

            if (Convert.ToInt32(tbCurrentItem.Text) >= Convert.ToInt32(tbReaderCount.Text))
            {
                tbCurrentItem.Text = Convert.ToString((Convert.ToInt32(tbReaderCount) - 1));
            }

            if (Convert.ToInt32(tbCurrentItem.Text) < 0)
            {
                tbCurrentItem.Text = "0";
            }

            if (e.KeyCode == Keys.Enter)
            {
                _sdataResourceCollectionRequest.Reader.EntryIndex = Convert.ToInt32(tbCurrentItem.Text);
                UpdateReaderGrid();
            }
        }

        private void NavigateCollection(string relation)
        {
            var link = _feed.Links.First(item => item.Relation == relation);
            var query = link.Uri.Query;
            var pos1 = query.IndexOf("startIndex=") + 11;
            var pos2 = query.IndexOf("&", pos1);

            if (pos2 < 0)
            {
                pos2 = query.Length;
            }

            numStartIndex.Value = int.Parse(query.Substring(pos1, pos2 - pos1));
            UpdateCollection();
        }

        private void UpdateCollection()
        {
            try
            {
                // create the sSDataResourceCollectionRequest
                _sdataResourceCollectionRequest = new SDataResourceCollectionRequest(_sdataService);
                // set the resource kind
                _sdataResourceCollectionRequest.ResourceKind = tbCollectionResourceKind.Text;
                _sdataResourceCollectionRequest.StartIndex = (int) numStartIndex.Value;
                _sdataResourceCollectionRequest.Count = (int) numCount.Value;
                // read the feed
                _feed = _sdataResourceCollectionRequest.Read();

                var lookup = _feed.Links.ToLookup(link => link.Relation);
                btnFirst.Enabled = lookup["first"].Any();
                btnPrevious.Enabled = lookup["previous"].Any();
                btnNext.Enabled = lookup["next"].Any();
                btnLast.Enabled = lookup["last"].Any();

                var table = new DataTable();
                table.Columns.Add("Author");
                table.Columns.Add("Id");
                table.Columns.Add("Title");

                // iterate through the list of entries in the feed
                foreach (var atomentry in _feed.Entries)
                {
                    var dr = table.NewRow();
                    dr[0] = atomentry.Authors[0].Name;
                    dr[1] = atomentry.Id.Uri.AbsoluteUri;
                    dr[2] = atomentry.Title.Content;

                    table.Rows.Add(dr);
                }

                // show it in the grid
                atomEntryGrid.DataSource = table;
                atomEntryGrid.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateReaderGrid()
        {
            var table = new DataTable();
            table.Columns.Add("Author");
            table.Columns.Add("Id");
            table.Columns.Add("Title");

            var dr = table.NewRow();
            var atomentry = _sdataResourceCollectionRequest.Reader.Current;
            dr[0] = atomentry.Authors[0].Name;
            dr[1] = atomentry.Id.Uri.AbsoluteUri;
            dr[2] = atomentry.Title.Content;

            table.Rows.Add(dr);

            // show it in the grid
            atomEntryGrid.DataSource = table;
            atomEntryGrid.Refresh();

            var payload = atomentry.GetSDataPayload();

            // show it in the grid
            collectionPayloadGrid.SelectedObject = payload;
            tbCurrentItem.Text = Convert.ToString(_sdataResourceCollectionRequest.Reader.EntryIndex);

            btnPrevious.Enabled = _sdataResourceCollectionRequest.Reader.EntryIndex > 1;
            btnNext.Enabled = _sdataResourceCollectionRequest.Reader.EntryIndex < _sdataResourceCollectionRequest.Reader.Count;
        }

        //======================================================

        private void tabSingle_Enter(object sender, EventArgs e)
        {
            try
            {
                _sdataSingleResourceRequest = new SDataSingleResourceRequest(_sdataService);
                _sdataSingleResourceRequest.ResourceKind = tbSingleResourceKind.Text;
                _sdataSingleResourceRequest.ResourceSelector = tbSingleResourceSelector.Text;
                tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
            }
            catch (SDataClientException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbSingleResourceKind_TextChanged(object sender, EventArgs e)
        {
            if (_sdataSingleResourceRequest != null)
            {
                _sdataSingleResourceRequest.ResourceKind = tbSingleResourceKind.Text;
                tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
            }
        }

        private void tbSingleResourceSelector_TextChanged(object sender, EventArgs e)
        {
            if (_sdataSingleResourceRequest != null)
            {
                _sdataSingleResourceRequest.ResourceSelector = tbSingleResourceSelector.Text;
                tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
            }
        }

        private void btnSingleRead_Click(object sender, EventArgs e)
        {
            try
            {
                var entry = _sdataSingleResourceRequest.Read();
                DisplayEntry(entry);
                statusLabel.Text = Resources.statusReadComplete;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSingleCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var newEntry = _sdataSingleResourceRequest.Create();
                DisplayEntry(newEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSingleUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var newEntry = _sdataSingleResourceRequest.Update();
                DisplayEntry(newEntry);
                statusLabel.Text = Resources.statusUpdateComplete;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSingleDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _sdataSingleResourceRequest.Delete();
                DisplayEntry(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayEntry(AtomEntry entry)
        {
            _sdataSingleResourceRequest.Entry = entry;
            var exists = entry != null;

            btnSingleCreate.Enabled = exists;
            btnSingleUpdate.Enabled = exists;
            btnSingleDelete.Enabled = exists;

            if (exists)
            {
                var uri = entry.Id.Uri.OriginalString;
                var start = uri.IndexOf("(");
                var end = uri.IndexOf(")", start);
                tbSingleResourceSelector.Text = uri.Substring(start, end + 1 - start);

                // show it in the grid
                singlePayloadGrid.SelectedObject = entry.GetSDataPayload();
            }
            else
            {
                tbSingleResourceSelector.Text = "()";
                singlePayloadGrid.SelectedObject = null;
            }
        }

        //======================================================

        private void tabTemplate_Enter(object sender, EventArgs e)
        {
            try
            {
                _sdataTemplateResourceRequest = new SDataTemplateResourceRequest(_sdataService);
                _sdataTemplateResourceRequest.ResourceKind = tbTemplateResourceKind.Text;
                tbTemplateURL.Text = _sdataTemplateResourceRequest.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbTemplateResourceKind_TextChanged(object sender, EventArgs e)
        {
            if (_sdataTemplateResourceRequest != null)
            {
                _sdataTemplateResourceRequest.ResourceKind = tbTemplateResourceKind.Text;
                tbTemplateURL.Text = _sdataTemplateResourceRequest.ToString();
            }
        }

        private void btnTemplateRead_Click(object sender, EventArgs e)
        {
            try
            {
                var entry = _sdataTemplateResourceRequest.Read();

                // show it in the grid
                templatePayloadGrid.SelectedObject = entry.GetSDataPayload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //======================================================

        private void tabSchema_Enter(object sender, EventArgs e)
        {
            _sdataResourceSchemaRequest = new SDataResourceSchemaRequest(_sdataService);
            _sdataResourceSchemaRequest.ResourceKind = tbSchemaResourceKind.Text;
            tbSchemaURL.Text = _sdataResourceSchemaRequest.ToString();
        }

        private void tbSchemaResourceKind_TextChanged(object sender, EventArgs e)
        {
            if (_sdataResourceSchemaRequest != null)
            {
                _sdataResourceSchemaRequest.ResourceKind = tbSchemaResourceKind.Text;
                tbSchemaURL.Text = _sdataResourceSchemaRequest.ToString();
            }
        }

        private void btnSchemaRead_Click(object sender, EventArgs e)
        {
            try
            {
                _schema = _sdataResourceSchemaRequest.Read();
                if (_schema != null)
                {
                    MessageBox.Show(Resources.statusSchemaReadComplete);
                    btnSchemaSave.Enabled = true;
                    btnSchemaSave.Visible = true;
                    label20.Visible = true;
                    tbSchemaFileName.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSchemaSave_Click(object sender, EventArgs e)
        {
            var stream = new FileStream(tbSchemaFileName.Text, FileMode.Create);
            //Write the file
            _schema.Write(stream);
            stream.Close();

            MessageBox.Show(Resources.statusSchemaSaveComplete);
        }

        //======================================================

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusLabel.Text = string.Empty;
        }

        private void tabResourceProperties_Enter(object sender, EventArgs e)
        {
            try
            {
                _sdataResourcePropertyRequest = new SDataResourcePropertyRequest(_sdataService);
                _sdataResourcePropertyRequest.ResourceKind = tbRPResourceKind.Text;
                _sdataResourcePropertyRequest.ResourceSelector = tbRPResourceSelector.Text;
                tbResourcePropertiesURL.Text = _sdataResourcePropertyRequest.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbResourceProperty.Text != string.Empty)
                {
                    _sdataResourcePropertyRequest.ResourceProperties.Add(lbProperties.Items.Count,
                                                                         tbResourceProperty.Text);
                    lbProperties.Items.Add(tbResourceProperty.Text);
                    tbResourcePropertiesURL.Text = _sdataResourcePropertyRequest.ToString();
                    tbResourceProperty.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbRPResourceKind_TextChanged(object sender, EventArgs e)
        {
            if (_sdataService == null)
            {
                return;
            }

            try
            {
                _sdataResourcePropertyRequest.ResourceKind = tbRPResourceKind.Text;
                tbResourcePropertiesURL.Text = _sdataResourcePropertyRequest.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbRPResourceSelector_TextChanged(object sender, EventArgs e)
        {
            if (_sdataService == null)
            {
                return;
            }

            try
            {
                _sdataResourcePropertyRequest.ResourceSelector = tbRPResourceSelector.Text;
                tbResourcePropertiesURL.Text = _sdataResourcePropertyRequest.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearProperties_Click(object sender, EventArgs e)
        {
            lbProperties.Items.Clear();
            _sdataResourcePropertyRequest.ResourceProperties.Clear();
            tbResourcePropertiesURL.Text = "";
        }

        private void btnPropertiesRead_Click(object sender, EventArgs e)
        {
            if (cbIsFeed.Checked)
            {
                _feed = _sdataResourcePropertyRequest.ReadFeed();
                var table = new DataTable();
                table.Columns.Add("Author");
                table.Columns.Add("Id");
                table.Columns.Add("Title");

                // iterate through the list of entries in the feed
                foreach (var atomentry in _feed.Entries)
                {
                    var dr = table.NewRow();
                    dr[0] = atomentry.Authors[0].Name;
                    dr[1] = atomentry.Id.Uri.AbsoluteUri;
                    dr[2] = atomentry.Title.Content;

                    table.Rows.Add(dr);
                }

                // show it in the grid
                rpGridEntries.DataSource = table;

                rpGridEntries.Refresh();
            }
            else
            {
                var entry = _sdataResourcePropertyRequest.Read();
                var payload = entry.GetSDataPayload();

                // show it in the grid
                gridRPPayloads.SelectedObject = payload;
                singlePayloadGrid.SelectedObject = payload;
            }
        }

        private void tbSingleResourceInclude_TextChanged(object sender, EventArgs e)
        {
            _sdataSingleResourceRequest.Include = tbSingleResourceInclude.Text;
            tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
        }
    }
}
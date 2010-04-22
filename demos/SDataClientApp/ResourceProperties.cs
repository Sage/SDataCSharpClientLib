using System;
using System.Data;
using System.Windows.Forms;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

namespace SDataClientApp
{
    public partial class ResourceProperties : BaseControl
    {
        private SDataResourcePropertyRequest _sdataResourcePropertyRequest;

        public ResourceProperties()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            try
            {
                _sdataResourcePropertyRequest = new SDataResourcePropertyRequest(Service)
                                                {
                                                    ResourceKind = tbRPResourceKind.Text,
                                                    ResourceSelector = tbRPResourceSelector.Text
                                                };
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
                    _sdataResourcePropertyRequest.ResourceProperties.Add(tbResourceProperty.Text);
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
                var feed = _sdataResourcePropertyRequest.ReadFeed();
                var table = new DataTable();
                table.Columns.Add("Author");
                table.Columns.Add("Id");
                table.Columns.Add("Title");

                // iterate through the list of entries in the feed
                foreach (var atomentry in feed.Entries)
                {
                    var dr = table.NewRow();
                    dr[0] = atomentry.Authors[0].Name;
                    dr[1] = atomentry.Id.Uri.AbsoluteUri;
                    dr[2] = atomentry.Title.Content;

                    table.Rows.Add(dr);
                }

                gridRPPayloads.SelectedObject = null;

                // show it in the grid
                rpGridEntries.DataSource = table;

                rpGridEntries.Refresh();
                rpGridEntries.AutoResizeColumns();
            }
            else
            {
                var entry = _sdataResourcePropertyRequest.Read();
                var payload = entry.GetSDataPayload();

                rpGridEntries.DataSource = null;

                // show it in the grid
                gridRPPayloads.SelectedObject = payload;
            }
        }
    }
}
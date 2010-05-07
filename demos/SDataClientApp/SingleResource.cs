using System;
using System.Windows.Forms;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using SDataClientApp.Properties;

namespace SDataClientApp
{
    public partial class SingleResource : BaseControl
    {
        private SDataSingleResourceRequest _sdataSingleResourceRequest;

        public SingleResource()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            try
            {
                _sdataSingleResourceRequest = new SDataSingleResourceRequest(Service)
                                              {
                                                  ResourceKind = tbSingleResourceKind.Text,
                                                  ResourceSelector = tbSingleResourceSelector.Text
                                              };
                tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
            }
            catch (SDataClientException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbSingleResourceKind_TextChanged(object sender, EventArgs e)
        {
            _sdataSingleResourceRequest.ResourceKind = tbSingleResourceKind.Text;
            tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
        }

        private void tbSingleResourceSelector_TextChanged(object sender, EventArgs e)
        {
            _sdataSingleResourceRequest.ResourceSelector = tbSingleResourceSelector.Text;
            tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
        }

        private void btnSingleRead_Click(object sender, EventArgs e)
        {
            try
            {
                var entry = _sdataSingleResourceRequest.Read();
                DisplayEntry(entry);
                StatusLabel.Text = Resources.statusReadComplete;
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
                StatusLabel.Text = Resources.statusUpdateComplete;
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
                tbSingleResourceSelector.Text = null;
                singlePayloadGrid.SelectedObject = null;
            }
        }

        private void tbSingleResourceInclude_TextChanged(object sender, EventArgs e)
        {
            _sdataSingleResourceRequest.Include = !string.IsNullOrEmpty(tbSingleResourceInclude.Text) ? tbSingleResourceInclude.Text : null;
            tbSingleURL.Text = _sdataSingleResourceRequest.ToString();
        }
    }
}
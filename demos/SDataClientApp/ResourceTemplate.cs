using System;
using System.Windows.Forms;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

namespace SDataClientApp
{
    public partial class ResourceTemplate : BaseControl
    {
        private SDataTemplateResourceRequest _sdataTemplateResourceRequest;

        public ResourceTemplate()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            try
            {
                _sdataTemplateResourceRequest = new SDataTemplateResourceRequest(Service) {ResourceKind = tbTemplateResourceKind.Text};
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

                if (entry == null)
                {
                    templatePayloadGrid.SelectedObject = null;
                    MessageBox.Show("$template not supported");
                }
                else
                {
                    // show it in the grid
                    templatePayloadGrid.SelectedObject = entry.GetSDataPayload();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
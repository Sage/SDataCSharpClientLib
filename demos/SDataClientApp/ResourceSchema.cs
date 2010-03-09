using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;
using Sage.SData.Client.Core;
using SDataClientApp.Properties;

// ReSharper disable InconsistentNaming

namespace SDataClientApp
{
    public partial class ResourceSchema : BaseControl
    {
        private SDataResourceSchemaRequest _sdataResourceSchemaRequest;
        private XmlSchema _schema;

        public ResourceSchema()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            _sdataResourceSchemaRequest = new SDataResourceSchemaRequest(Service) {ResourceKind = tbSchemaResourceKind.Text};
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
                    lbSchemaFileName.Visible = true;
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
            using (var stream = new FileStream(tbSchemaFileName.Text, FileMode.Create))
            {
                _schema.Write(stream);
            }

            MessageBox.Show(Resources.statusSchemaSaveComplete);
        }
    }
}
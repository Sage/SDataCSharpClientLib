using System;
using Sage.SData.Client.Framework;
using SDataClientApp.Properties;

// ReSharper disable InconsistentNaming

namespace SDataClientApp
{
    public partial class ServiceConfig : BaseControl
    {
        public ServiceConfig()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
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

        private void FormatURL()
        {
            try
            {
                var server = tbServer.Text;
                var pos = server.IndexOf(':');
                var uri = new SDataUri();
                int port;

                if (pos >= 0 && int.TryParse(server.Substring(pos + 1), out port))
                {
                    server = server.Substring(0, pos);
                    uri.Port = port;
                }

                uri.Scheme = cbProtocol.Text;
                uri.Host = server;
                uri.Server = tbVirtualDirectory.Text;
                uri.Product = tbApplication.Text;
                uri.Contract = tbContract.Text;
                uri.CompanyDataset = tbDataSet.Text;

                tbURL.Text = uri.ToString();
            }
            catch (UriFormatException)
            {
            }
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            var server = tbServer.Text;
            var pos = server.IndexOf(':');
            int port;

            if (pos >= 0 && int.TryParse(server.Substring(pos + 1), out port))
            {
                server = server.Substring(0, pos);
            }
            else
            {
                port = 80;
            }

            Service.Protocol = cbProtocol.Text;
            Service.ServerName = server;
            Service.Port = port;
            Service.VirtualDirectory = tbVirtualDirectory.Text;
            Service.ApplicationName = tbApplication.Text;
            Service.ContractName = tbContract.Text;
            Service.DataSet = tbDataSet.Text;
            Service.UserName = tbUserName.Text;
            Service.Password = tbPassword.Text;

            StatusLabel.Text = Resources.statusInitializationComplete;
        }
    }
}
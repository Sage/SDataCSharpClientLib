using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;

namespace SDataClientApp
{
    public partial class ResourceCollection : BaseControl
    {
        private SDataResourceCollectionRequest _sdataResourceCollectionRequest;
        private AtomFeed _feed;
        private AtomFeedReader _reader;

        public ResourceCollection()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            tbCollectionURL.Text = new SDataUri(Service.Url).AppendPath(tbCollectionResourceKind.Text).ToString();
        }

        private void tbCollectionResourceKind_TextChanged(object sender, EventArgs e)
        {
            tbCollectionURL.Text = new SDataUri(Service.Url).AppendPath(tbCollectionResourceKind.Text).ToString();
        }

        private void btnCollectionRead_Click(object sender, EventArgs e)
        {
            collectionPayloadGrid.SelectedObject = null;
            UpdateCollection();
        }

        private void btnReaderRead_Click(object sender, EventArgs e)
        {
            try
            {
                _sdataResourceCollectionRequest = new SDataResourceCollectionRequest(Service)
                                                  {
                                                      ResourceKind = tbCollectionResourceKind.Text,
                                                      StartIndex = (int) numStartIndex.Value,
                                                      Count = (int) numCount.Value
                                                  };

                _feed = null;
                _reader = _sdataResourceCollectionRequest.ExecuteReader();

                tbReaderCount.Text = _reader.Count.ToString();

                UpdateReaderGrid();
            }
            catch (SDataClientException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void atomEntryGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_feed == null)
            {
                return;
            }

            var index = atomEntryGrid.SelectedRows[0].Index;

            // get the atomentry for the row selected from the feed
            var entry = _feed.Entries.Skip(index).First();

            // show it in the grid
            collectionPayloadGrid.SelectedObject = entry.GetSDataPayload();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.MoveFirst();
                UpdateReaderGrid();
            }
            else
            {
                NavigateCollection("first");
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.MovePrevious();
                UpdateReaderGrid();
            }
            else
            {
                NavigateCollection("previous");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                if (_reader.MoveNext())
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
            if (_reader != null)
            {
                if (_reader.MoveLast())
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
                _sdataResourceCollectionRequest = new SDataResourceCollectionRequest(Service)
                                                  {
                                                      ResourceKind = tbCollectionResourceKind.Text,
                                                      StartIndex = (int) numStartIndex.Value,
                                                      Count = (int) numCount.Value
                                                  };
                _feed = _sdataResourceCollectionRequest.Read();
                _reader = null;

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
                atomEntryGrid.AutoResizeColumns();

                atomEntryGrid_CellContentClick(null, null);
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
            var atomentry = _reader.Current;
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
            tbCurrentItem.Text = _reader.CurrentIndex.ToString();

            btnPrevious.Enabled = _reader.CurrentIndex > 1;
            btnNext.Enabled = _reader.CurrentIndex < _reader.Count;
        }
    }
}
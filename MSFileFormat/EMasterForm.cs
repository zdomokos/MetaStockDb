/*
 *  Copyright (C) 2006-2014 Robert Iwancz
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using MetaStockDb;


namespace MSFileFormat
{
	public class EMasterForm : MSFileFormat.BaseMaster
	{
		private System.ComponentModel.IContainer components = null;

        public EMasterForm(string dir) {  
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            string DataFileName = Path.Combine(dir, "EMASTER");
            if (File.Exists(DataFileName)) {
                this.Text = "EMASTER (" + DataFileName + ")";
                LoadEMasterFile(DataFileName);
            }
            else {
                MessageBox.Show("Unable to find an EMASTER file in directory " + dir, 
                                "File Does Not Exist",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		private EMasterForm()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // 
            // RecordsText
            // 
            this.RecordsText.Name = "RecordsText";
            // 
            // MaxText
            // 
            this.MaxText.Name = "MaxText";
            // 
            // RemainText
            // 
            this.RemainText.Name = "RemainText";
            // 
            // StockView
            // 
            this.StockView.Name = "StockView";
            // 
            // EMasterForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 701);
            this.Name = "EMasterForm";
            this.Text = "EMASTER";

        }
		#endregion


		public void LoadEMasterFile(string filename) {
            this.SuspendLayout();
            StockView.Columns.Clear();
            StockView.Columns.Add("ASC", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Num", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill1", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("NumFields", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Del", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("FB1", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Space", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("FB2", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Symbol", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill2", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Name", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill3", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("TimeFrame", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill4", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("FirstDate", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("BegTrade", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("LastDate", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("EndTrade", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("StartTime", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("EndTime", -1, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill5", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("MysteryData", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Fill6", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("ExtName", -2, HorizontalAlignment.Left);
            StockView.Columns.Add("Remainder", -2, HorizontalAlignment.Left);

            try 
            {
	            var masterFile = new EMasterFile();
	            masterFile.Load(filename);

	            for (int i = 0; i < masterFile.Records.Count; i++)
	            {
		            var lvi = new ListViewItem(masterFile.Records[i].ToStringArray());
		            StockView.Items.Add(lvi);
	            }
            }
            catch (IOException e) 
            {
                MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException e) 
            {
                MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

			this.ResumeLayout();
        }

        protected override void ExportData(string filename, bool filter) {
            using (StreamWriter sw = new StreamWriter(filename)) {
                const int iFileNumber = 1;
                const int iSymbol = 8;
                const int iName = 10;
                const int iFirstDate = 14;
                const int iLastDate = 16;
                const int iExtName = 23;

                string name;
                
                foreach (ListViewItem lvi in StockView.Items){
                    if (filter && lvi.SubItems[iSymbol].Text.Length > 3)
                        continue;
                    sw.Write(lvi.SubItems[iFileNumber].Text);
                    sw.Write(',');
                    sw.Write(lvi.SubItems[iSymbol].Text);
                    sw.Write(',');

                    if (lvi.SubItems[iExtName].Text.Length > 0)
                        name = lvi.SubItems[iExtName].Text;
                    else
                        name = lvi.SubItems[iName].Text;

                    if (name.IndexOf(',') >= 0) {
                        sw.Write("\"");
                        sw.Write(name);
                        sw.Write("\"");
                    }
                    else {
                        sw.Write(name);
                    }
                    sw.Write(',');

                    sw.Write(lvi.SubItems[iFirstDate].Text);
                    sw.Write(',');
                    sw.Write(lvi.SubItems[iLastDate].Text);

                    sw.WriteLine();
                }                
            }
        }

    }
}

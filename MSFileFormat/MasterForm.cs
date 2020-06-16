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

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using MetaStockDb;

namespace MSFileFormat
{
	public class MasterForm : MSFileFormat.BaseMaster
	{
		private System.ComponentModel.IContainer components = null;

		public MasterForm(string dir)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			string DataFileName = Path.Combine(dir, "MASTER");
			if (File.Exists(DataFileName))
			{
				this.Text = "MASTER (" + DataFileName + ")";
				LoadMasterFile(DataFileName);
			}
			else
			{
				MessageBox.Show("Unable to find a MASTER file in directory " + dir,
								"File Does Not Exist",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}


		private MasterForm()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
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
			// MasterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(784, 701);
			this.Name = "MasterForm";
			this.Text = "MASTER File";
			this.Load += new System.EventHandler(this.MasterForm_Load);

		}
		#endregion

		private void MasterForm_Load(object sender, System.EventArgs e)
		{

		}

		public void LoadMasterFile(string filename)
		{
			this.SuspendLayout();
			StockView.Columns.Clear();
			StockView.Columns.Add("Num", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("FileType", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("RecLen", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("NumFields", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("Res1", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("CI", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("Name", -1, HorizontalAlignment.Left);
			StockView.Columns.Add("Res2", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("CT", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("FirstDate", -1, HorizontalAlignment.Left);
			StockView.Columns.Add("LastDate", -1, HorizontalAlignment.Left);
			StockView.Columns.Add("TimeInt", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("IDTB", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("Symbol", -1, HorizontalAlignment.Left);
			StockView.Columns.Add("Res3", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("Flag", -2, HorizontalAlignment.Left);
			StockView.Columns.Add("Res4", -2, HorizontalAlignment.Left);

			try
			{
				var masterFile = new MasterFile();
				masterFile.Load(filename);

				for (int i = 0; i < masterFile.Records.Count; i++)
				{
					var lvi = new ListViewItem(masterFile.Records[i].ToStringArray());
					StockView.Items.Add(lvi);
				}
			}
			catch (IOException e)
			{
				MessageBox.Show(e.Message, "Error reading MASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			this.ResumeLayout();
		}

		protected override void ExportData(string filename, bool filter)
		{
			using (StreamWriter sw = new StreamWriter(filename))
			{
				const int iFileNumber = 0;
				const int iSymbol = 13;
				const int iName = 6;
				const int iFirstDate = 9;
				const int iLastDate = 10;

				foreach (ListViewItem lvi in StockView.Items)
				{
					if (filter && lvi.SubItems[iSymbol].Text.Length > 3)
						continue;
					sw.Write(lvi.SubItems[iFileNumber].Text);
					sw.Write(',');
					sw.Write(lvi.SubItems[iSymbol].Text);
					sw.Write(',');
					if (lvi.SubItems[iName].Text.IndexOf(',') >= 0)
					{
						sw.Write("\"");
						sw.Write(lvi.SubItems[iName].Text);
						sw.Write("\"");
					}
					else
					{
						sw.Write(lvi.SubItems[iName].Text);
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

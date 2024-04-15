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
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MetaStockDb;


namespace MSFileFormat
{
	public class XMasterForm : MSFileFormat.BaseMaster
	{
		private System.ComponentModel.IContainer components = null;

		public XMasterForm(string dir)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			string DataFileName = Path.Combine(dir, "XMASTER");
			if (File.Exists(DataFileName))
			{
				this.Text = "XMASTER (" + DataFileName + ")";
				Cursor.Current = Cursors.WaitCursor;
				LoadXMasterFile(DataFileName);
				Cursor.Current = Cursors.Default;
			}
			else
			{
				MessageBox.Show("Unable to find an XMASTER file in directory " + dir,
								"File Does Not Exist",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private XMasterForm()
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
			// XMasterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(840, 701);
			this.Name = "XMasterForm";
			this.Text = "XMaster";

		}
		#endregion

		public void LoadXMasterFile(string filename)
		{
			this.SuspendLayout();
			StockView.Columns.Clear();

			Type t = typeof(XMasterRec);
			FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fi in fields)
			{
				StockView.Columns.Add(fi.Name, (fi.FieldType == typeof(string)) ? -2 : -1, HorizontalAlignment.Left);
			}

			try
			{
				var masterFile = new XMasterFile();
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

			this.ResumeLayout();
		}


		protected override void ExportData(string filename, bool filter)
		{
			using (StreamWriter sw = new StreamWriter(filename))
			{
				const int iFileNumber = 5;
				const int iSymbol = 1;
				const int iName = 2;
				const int iFirstDate = 12;
				const int iLastDate = 15;

				foreach (ListViewItem lvi in StockView.Items)
				{
					if (filter && lvi.SubItems[1].Text.Length > 3)
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


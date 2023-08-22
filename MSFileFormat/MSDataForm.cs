using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaStockDb;

namespace MSFileFormat
{
	public partial class MSDataForm : Form
	{
		public MSDataForm()
		{
			InitializeComponent();
		}

		public string DbPath { get; set; }

		private void btnRead_Click(object sender, EventArgs e)
		{
			int fileNum = Convert.ToInt32(txtFileName.Text);
			string extension = fileNum > 255 ? "mwd" : "dat";
			string fileName = Path.Combine(DbPath, $"F{txtFileName.Text}.{extension}");

			var stock = new PriceDateFile(DbPath, new StockDataHeader(){FileNumber = fileNum});
			stock.Load(fileName);

			var sb = new StringBuilder();
			foreach (var stockRecord in stock.Records)
			{
				sb.AppendLine(stockRecord.ToString());
			}
			txtResult.Text = sb.ToString();
		}
	}
}

// Decompiled with JetBrains decompiler
// Type: Nis.Utils.TextFile
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using Norgate.Utils;
using System.Text;

namespace Nis.Utils
{
	public class TextFile : List<string>
	{
		private string ticker = "";
		private TextFileTranslateParams mp;

		public TextFile(TextFileTranslateParams myParams)
		{
			mp = myParams;
		}

		public string Translate(MsMkt msMkt)
		{
			return Translate(msMkt, false);
		}

		public string Translate(MsMkt msMkt, bool reverseDateOrder)
		{
			string str = "";
			int startLine = 0;
			if (mp.UseHeaders)
			{
				Add(header(mp.UseQuotes));
				if (Count > 0)
					startLine = 1;
			}

			ticker = msMkt.Ticker;
			if (msMkt.Reset())
			{
				do
				{
					DateTime.FromOADate((double) msMkt.CurrentDay.dtd.d);
					if (!reverseDateOrder)
						addLine(msMkt.CurrentDay.dtd);
					else
						insertLine(msMkt.CurrentDay.dtd, startLine);
				} while (msMkt.Next());
			}

			string path =
				$"{mp.TxtPath}\\{NgUtils.CheckTickerName(ticker)}.{mp.Extension}";
			try
			{
				File.WriteAllLines(path, ToArray(), Encoding.ASCII);
			}
			catch (IOException ex)
			{
				str = $"Exception: File {path} could not be accessed as it was open in another process";
			}
			catch (Exception ex)
			{
				str = $"Exception: File {path} experienced an unknown exception: {ex.Message}";
			}

			return str;
		}

		public void TestTranslate(MsBaseStructs.RecDay day, string ticker)
		{
			if (mp.UseHeaders)
				Add(header(mp.UseQuotes));
			this.ticker = ticker;
			addLine(day);
		}

		private string header(bool useQuotes)
		{
			string str1 = "";
			foreach (ColumnInfo colInfo in (List<ColumnInfo>) mp.ColInfoList)
			{
				string str2 = colInfo.outputName;
				if (str2.ToLower() == "dummy time")
					str2 = "Time";
				int totalWidth = mp.FieldWidth;
				if (useQuotes)
					totalWidth -= 2;
				if (totalWidth < 0)
					totalWidth = 0;
				string str3 = str2.PadRight(totalWidth);
				if (useQuotes)
					str3 = $"\"{str3}\"";
				str1 = str1 + str3 + mp.FieldDelim;
			}

			string str4 = str1.Trim();
			if (mp.FieldDelim.Length > 0)
				str4 = NgUtils.RemoveTrailingChar(str4.Trim(), mp.FieldDelim[0]);
			return str4.Trim();
		}

		private string addLine(MsBaseStructs.RecDay day)
		{
			string line = "";
			foreach (ColumnInfo colInfo in (List<ColumnInfo>) mp.ColInfoList)
				addField(day, colInfo, ref line);
			if (mp.FieldDelim.Length > 0)
				line = NgUtils.RemoveTrailingChar(line, mp.FieldDelim[0]);
			Add(line);
			return line;
		}

		private string insertLine(MsBaseStructs.RecDay day)
		{
			return insertLine(day, 0);
		}

		private string insertLine(MsBaseStructs.RecDay day, int startLine)
		{
			string line = "";
			foreach (ColumnInfo colInfo in (List<ColumnInfo>) mp.ColInfoList)
				addField(day, colInfo, ref line);
			if (mp.FieldDelim.Length > 0)
				line = NgUtils.RemoveTrailingChar(line, mp.FieldDelim[0]);
			Insert(startLine, line);
			return line;
		}

		private void addField(MsBaseStructs.RecDay day, ColumnInfo ci, ref string line)
		{
			string str1 = "";
			mp.NFI.NumberDecimalDigits = ci.DP;
			switch (ci.Name.ToLower())
			{
				case "close":
					str1 = formatPrice((double) day.c);
					break;
				case "date":
					str1 = DateTime.FromOADate((double) day.d).ToString(mp.DateFormat);
					break;
				case "dummy time":
					str1 = mp.DummyTime;
					break;
				case "high":
					str1 = formatPrice((double) day.h);
					break;
				case "low":
					str1 = formatPrice((double) day.l);
					break;
				case "open":
					str1 = formatPrice((double) day.o);
					break;
				case "open interest":
					str1 = formatPrice((double) day.i);
					break;
				case "openint":
					str1 = formatPrice((double) day.i);
					break;
				case "openinterest":
					str1 = formatPrice((double) day.i);
					break;
				case "ticker":
					str1 = ticker;
					break;
				case "vol":
					str1 = formatPrice((double) day.v);
					break;
				case "volume":
					str1 = formatPrice((double) day.v);
					break;
			}

			if (str1.Length <= 0)
				return;
			int totalWidth = mp.FieldWidth;
			if (mp.UseQuotes)
				totalWidth -= 2;
			if (totalWidth < 0)
				totalWidth = 0;
			string str2 = str1.PadLeft(totalWidth);
			if (mp.UseQuotes)
				str2 = $"\"{str2}\"";
			line = line + str2 + mp.FieldDelim;
		}

		private string formatPrice(double price)
		{
			double num = Math.Pow(10.0, (double) mp.NFI.NumberDecimalDigits);
			return (Math.Truncate(price * num + 0.5) / num).ToString("F", (IFormatProvider) mp.NFI);
		}
	}
}

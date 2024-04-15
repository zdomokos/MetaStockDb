// Decompiled with JetBrains decompiler
// Type: Norgate.Utils.NgUtils
// Assembly: norgate.general.4, Version=4.0.2.1, Culture=neutral, PublicKeyToken=d5eccc6216068f48
// MVID: A122D709-041A-426F-9B6C-C1457ADC069B
// Assembly location: C:\Program Files (x86)\Premium Data Converter\norgate.general.4.dll

//using ICSharpCode.SharpZipLib.BZip2;
//using ICSharpCode.SharpZipLib.Zip;

//using SevenZip;

using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Norgate.Utils
{
	public static class NgUtils
	{
		public static string AppendTrailingChar(string s, char c)
		{
			int index = s.Length - 1;
			if ((int) s[index] != (int) c)
				s += c.ToString();
			if (c == '/')
				s += c.ToString();
			return s;
		}

		public static string RemoveTrailingChar(string s, char c)
		{
			if (s.Length == 0)
				return s;
			StringBuilder stringBuilder1 = new StringBuilder(s);
			while (true)
			{
				StringBuilder stringBuilder2 = stringBuilder1;
				if ((int) stringBuilder2[stringBuilder2.Length - 1] == (int) c)
					--stringBuilder1.Length;
				else
					break;
			}

			return stringBuilder1.ToString();
		}

		public static string UrlEncode(string url)
		{
			StringBuilder stringBuilder = new StringBuilder(url);
			for (int index = stringBuilder.Length - 1; index >= 0; --index)
			{
				byte num = (byte) stringBuilder[index];
				if (num == (byte) 32)
					stringBuilder[index] = '+';
				else if (num < (byte) 48 || num > (byte) 57 && num < (byte) 65 ||
				         (num > (byte) 90 && num < (byte) 97 || num > (byte) 122))
				{
					stringBuilder.Remove(index, 1);
					stringBuilder.Insert(index, $"%{(object) num:X2}");
				}
			}

			return stringBuilder.ToString();
		}

		public static string UrlEncode2(string s)
		{
			StringBuilder stringBuilder = new StringBuilder(s);
			for (int index = stringBuilder.Length - 1; index >= 0; --index)
			{
				byte num = (byte) stringBuilder[index];
				stringBuilder.Remove(index, 1);
				stringBuilder.Insert(index, $"%{(object) num:X2}");
			}

			return stringBuilder.ToString();
		}

		public static void ChkDir(string dn)
		{
			if (Directory.Exists(dn))
				return;
			Directory.CreateDirectory(dn);
		}

		public static bool IsWeekend(DateTime dt)
		{
			if (dt.DayOfWeek != DayOfWeek.Saturday)
				return dt.DayOfWeek == DayOfWeek.Sunday;
			return true;
		}

		public static bool IsWeekday(DateTime dt)
		{
			return !IsWeekend(dt);
		}

		public static string GetEnumDesc(Enum e)
		{
			DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) e.GetType().GetField(e.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (customAttributes.Length != 0)
				return customAttributes[0].Description;
			return e.ToString();
		}

		public static bool MyDeleteFile(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			if (fileInfo.Exists)
			{
				fileInfo.Attributes = (FileAttributes) 0;
				fileInfo.Delete();
			}

			return fileInfo.Exists;
		}

		//public static object readIniValue(string fn, string section, string key, object defVal)
		//{
		//  object obj = defVal;
		//  Type type = defVal.GetType();
		//  string iniValue = new IniFileReader(fn).GetIniValue(section, key);
		//  if (iniValue != null && iniValue != "")
		//    obj = (object) iniValue;
		//  return Convert.ChangeType(obj, type);
		//}

		//public static bool writeIniValue(string fn, string section, string item, object value)
		//{
		//  IniFileReader iniFileReader = new IniFileReader(fn);
		//  string str = section;
		//  iniFileReader.SetIniSection(str, str);
		//  bool flag = iniFileReader.SetIniValue(section, item, (string) value);
		//  iniFileReader.OutputFilename = fn;
		//  iniFileReader.Save();
		//  return flag;
		//}

		public static byte[] HexToByteArray(string HexString)
		{
			int length = HexString.Length;
			byte[] numArray = new byte[length / 2];
			int startIndex = 0;
			while (startIndex < length)
			{
				numArray[startIndex / 2] = Convert.ToByte(HexString.Substring(startIndex, 2), 16);
				startIndex += 2;
			}

			return numArray;
		}

		public static string TimerString(TimeSpan ts)
		{
			return
				$"{(object) Math.Truncate(ts.TotalHours):00}:{(object) Math.Truncate((double) ts.Minutes):00}:{(object) ts.Seconds:00}";
		}

		public static void EraseFiles(string path, string mask)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (directoryInfo.Name.Length == 0 || directoryInfo.Name.ToLower().StartsWith("windows") ||
			    directoryInfo.Name.ToLower().StartsWith("program files"))
				return;
			foreach (FileInfo file in directoryInfo.GetFiles(mask))
			{
				try
				{
					file.Delete();
				}
				catch
				{
				}
			}
		}

		public static string EnDeCode(string s)
		{
			if (s == null || s.Length > 1024)
				return "";
			string str =
				"4104301140440410131141240231224241034241011312024111433132010332321222212123212440241023103403132204311014141440121123203404003132340344344222304443034240241123343344344313144101320201042434331302323112144222142241100044421404143203224114214042242024222221332143042312343304422230221001440110121124343122012420343222304332014133134334344430201200133134432424411202344043040341144342302443212032002111433411214401013224323444244220002132414043320143003114334224411311242241220320431234123141020312114214114431141410244022243001433100303404230101301301300101333000030422442241224200032210313032411044414120100400310334010042113403221002312301211010411311114004011143320041211430432440112201114132342003413434134322401432422323213300420020332422414411243023432420303024330011124024021420321242433122410243320232404010013233342020312102010220300123112233343033110144300101311330442011130114124401124043113124214422021103044143223110002214033044442100442404441114344421214012310334011431424034333310202043040424114024111311310131";
			StringBuilder stringBuilder = new StringBuilder(s);
			for (int index = 0; index <= s.Length - 1; ++index)
			{
				int num = (int) stringBuilder[index] ^ (int) str[index] - 48;
				stringBuilder[index] = (char) num;
			}

			return stringBuilder.ToString();
		}

		public static string IncludeTrailingPathDelimiter(string s)
		{
			if (s == null || s.Length == 0)
				return "";
			string str = s;
			if (!s.EndsWith("\\"))
				str = s + "\\";
			return str;
		}

		public static bool RawGetFile(string url, string pn)
		{
			bool flag = false;
			try
			{
				new WebClient().DownloadFile(url, pn);
				flag = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return flag;
		}

		//public static void LzmaDecode(Stream inStream, Stream outStream, SevenZipPbCallbackHandler pp)
		//{
		//  NgUtils.LzmaProgressInfo lzmaProgressInfo = (NgUtils.LzmaProgressInfo) null;
		//  if (pp != null)
		//    lzmaProgressInfo = new NgUtils.LzmaProgressInfo(pp);
		//  byte[] numArray = new byte[5];
		//  inStream.Read(numArray, 0, 5);
		//  long outSize = 0;
		//  for (int index = 0; index < 8; ++index)
		//  {
		//    int num = inStream.ReadByte();
		//    if (num < 0)
		//      throw new Exception("Can't Read 1");
		//    outSize |= (long) (byte) num << 8 * index;
		//  }
		//  long inSize = inStream.Length - inStream.Position;
		//  SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
		//  decoder.SetDecoderProperties(numArray);
		//  decoder.Code(inStream, outStream, inSize, outSize, (ICodeProgress) lzmaProgressInfo);
		//}

		public static SortedList ReadIniSection(string filePath, string section)
		{
			section = "[" + section + "]";
			SortedList sortedList = new SortedList();
			StreamReader streamReader = new StreamReader(filePath);
			bool flag1 = streamReader.EndOfStream;
			bool flag2 = false;
			while (!flag1)
			{
				string str1 = streamReader.ReadLine();
				if (!str1.StartsWith(";") && !str1.StartsWith("'"))
				{
					if (flag2)
					{
						if (str1.StartsWith("["))
						{
							flag1 = true;
						}
						else
						{
							int length = str1.IndexOf('=');
							if (length > 0)
							{
								string str2 = str1.Substring(0, length);
								string str3 = str1.Substring(length + 1, str1.Length - length - 1);
								if (!sortedList.ContainsKey((object) str2))
									sortedList.Add((object) str2, (object) str3);
							}
						}
					}
					else if (str1.Contains(section))
						flag2 = true;
				}

				if (streamReader.EndOfStream)
					flag1 = true;
			}

			streamReader.Close();
			return sortedList;
		}

		public static string ReadIniItem(string filePath, string section, string key, string defaultStr)
		{
			string str = defaultStr;
			SortedList sortedList = ReadIniSection(filePath, section);
			int index = sortedList.IndexOfKey((object) key);
			if (index >= 0)
				str = (string) sortedList.GetByIndex(index);
			return str;
		}

		public static string GetDbNameFromYearlyDbName(string ydbName)
		{
			string str = ydbName;
			string[] strArray = ydbName.Split('.');
			if (strArray.Length != 0)
				str = strArray[0];
			return str;
		}

		//public static string NorgateFolder
		//{
		//  get
		//  {
		//    return (string) NgRegUtils.readRegValue(Registry.CurrentUser, "software\\norgate", "norgatefolder", (object) "c:\\");
		//  }
		//}

		//public static string GetNorgateIniItem(string section, string key)
		//{
		//  return NgUtils.GetNorgateIniItem(section, key, "");
		//}

		//public static string GetNorgateIniItem(string section, string key, string defaultStr)
		//{
		//  return NgUtils.ReadIniItem(NgUtils.NorgateFolder + "norgate.ini", section, key, defaultStr);
		//}

		//public static string bz2Compress(string sBuffer)
		//{
		//  MemoryStream memoryStream = (MemoryStream) null;
		//  BZip2OutputStream bzip2OutputStream = (BZip2OutputStream) null;
		//  string base64String;
		//  try
		//  {
		//    memoryStream = new MemoryStream();
		//    int length = sBuffer.Length;
		//    using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream, Encoding.ASCII))
		//    {
		//      binaryWriter.Write(length);
		//      bzip2OutputStream = new BZip2OutputStream((Stream) memoryStream);
		//      bzip2OutputStream.Write(Encoding.ASCII.GetBytes(sBuffer), 0, sBuffer.Length);
		//      bzip2OutputStream.Close();
		//      base64String = Convert.ToBase64String(memoryStream.ToArray());
		//      memoryStream.Close();
		//      binaryWriter.Close();
		//    }
		//  }
		//  finally
		//  {
		//    bzip2OutputStream?.Dispose();
		//    memoryStream?.Dispose();
		//  }
		//  return base64String;
		//}

		//public static string bz2Expand(string compbytes)
		//{
		//  MemoryStream memoryStream = (MemoryStream) null;
		//  BZip2InputStream bzip2InputStream = (BZip2InputStream) null;
		//  string str;
		//  try
		//  {
		//    memoryStream = new MemoryStream(Convert.FromBase64String(compbytes));
		//    using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream, Encoding.ASCII))
		//    {
		//      int length = binaryReader.ReadInt32();
		//      bzip2InputStream = new BZip2InputStream((Stream) memoryStream);
		//      byte[] numArray = new byte[length];
		//      bzip2InputStream.Read(numArray, 0, numArray.Length);
		//      bzip2InputStream.Close();
		//      memoryStream.Close();
		//      str = Encoding.ASCII.GetString(numArray);
		//      binaryReader.Close();
		//    }
		//  }
		//  finally
		//  {
		//    bzip2InputStream?.Dispose();
		//    memoryStream?.Dispose();
		//  }
		//  return str;
		//}

		//public static void UnZip(string fileName, string destinationFolder)
		//{
		//  Directory.CreateDirectory(destinationFolder);
		//  destinationFolder = NgUtils.AppendTrailingChar(destinationFolder, '\\');
		//  ZipInputStream zipInputStream = new ZipInputStream((Stream) System.IO.File.OpenRead(fileName));
		//  ZipEntry nextEntry;
		//  while ((nextEntry = zipInputStream.GetNextEntry()) != null)
		//  {
		//    FileStream fileStream = System.IO.File.Create(destinationFolder + nextEntry.Name);
		//    byte[] buffer = new byte[nextEntry.Size];
		//    while (true)
		//    {
		//      long num = (long) zipInputStream.Read(buffer, 0, buffer.Length);
		//      if (num > 0L)
		//        fileStream.Write(buffer, 0, (int) num);
		//      else
		//        break;
		//    }
		//    fileStream.Close();
		//  }
		//  zipInputStream.Close();
		//  Console.WriteLine("Done!!");
		//}

		public static string CheckTickerName(string fn)
		{
			if (fn.Length != 3)
				return fn;
			bool flag = false;
			if (fn.Equals("CON", StringComparison.InvariantCultureIgnoreCase))
				flag = true;
			else if (fn.Equals("PRN", StringComparison.InvariantCultureIgnoreCase))
				flag = true;
			else if (fn.Equals("NUL", StringComparison.InvariantCultureIgnoreCase))
				flag = true;
			else if (fn.Equals("AUX", StringComparison.InvariantCultureIgnoreCase))
				flag = true;
			if (flag)
				fn += "_";
			return fn;
		}

		public static bool IsValidFilename(string testName)
		{
			return !new Regex("[" + Regex.Escape(Path.InvalidPathChars.ToString()) + "]").IsMatch(testName);
		}

		public static bool IsValidPath(string path)
		{
			return new Regex("^(([a-zA-Z]\\:)|(\\\\))(\\\\{1}|((\\\\{1})[^\\\\]([^/:*?<>\"|]*))+)$").IsMatch(path);
		}

		public static string ConvertCharArray2String(char[] ca)
		{
			string str = new string(ca);
			if (str.Contains("\0"))
				str = str.Substring(0, str.IndexOf(char.MinValue));
			return str;
		}

		public static byte[] CalcMd5Digest(string fn)
		{
			if (!File.Exists(fn))
				return (byte[]) null;
			FileStream fileStream = new FileStream(fn, FileMode.Open, FileAccess.Read);
			byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream) fileStream);
			fileStream.Close();
			return hash;
		}

		public static string CalcMd5DigestAsString(string fn)
		{
			byte[] numArray = CalcMd5Digest(fn);
			string str1 = "";
			foreach (byte num in numArray)
			{
				string str2 = num.ToString("X");
				switch (str2.Length)
				{
					case 0:
						str2 = "00";
						break;
					case 1:
						str2 = "0" + str2;
						break;
				}

				str1 += str2;
			}

			return str1;
		}

		public static bool Md5Compare(byte[] md51, byte[] md52)
		{
			if (md51 == null || md52 == null || (md51.Length != 16 || md52.Length != 16))
				return false;
			for (int index = 0; index < 16; ++index)
			{
				if ((int) md51[index] != (int) md52[index])
					return false;
			}

			return true;
		}

		//private class LzmaProgressInfo : ICodeProgress
		//{
		//  private SevenZipPbCallbackHandler updatePB;

		//  public LzmaProgressInfo(SevenZipPbCallbackHandler pp)
		//  {
		//    this.updatePB = pp;
		//  }

		//  public void SetProgress(long percentin, long percentout)
		//  {
		//    this.updatePB(percentin, percentout);
		//  }
		//}
	}
}

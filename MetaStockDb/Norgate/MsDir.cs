// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsDir
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using Norgate.Utils;

namespace Nis.Utils
{
	public class MsDir : MsBaseProcs
	{
		public string DirPath = "";
		public bool OpenOK;
		public bool Amended;

		private int maxEntries = 6000;
		private MsNewDirList sex = new MsNewDirList();
		private DirHdr hdr;
		private MsGlobals msg;
		private bool[] fnAvailable;

		public int Count => sex.Count;
		internal string datFileName
		{
			get
			{
				int fileNo = (int) sex.Get.FileNo;
				string str = $"F{(object) fileNo}.";
				return fileNo <= (int) byte.MaxValue ? str + "dat" : str + "mwd";
			}
		}
		internal string datFilePath => DirPath + datFileName;
		public DirEntry Get => sex.Get;
		public List<string> TickerList => sex.TickerList;

		public MsDir(string folder, bool isMono)
		{
			char ch = '\\';
			if (isMono)
				ch = '/';
			DirPath = folder;
			if ((int) DirPath[^1] != (int) ch)
				DirPath += ch.ToString();
			bool flag = false;
			msg = new MsGlobals(DirPath);
			hdr = new DirHdr(msg);
			resetFnAvailable();
			if (Directory.Exists(DirPath))
			{
				initFolder();
				if (File.Exists(msg.Fs1Pth))
				{
					try
					{
						clearROAttribute(msg.Fs1Pth);
						clearROAttribute(msg.Fs2Pth);
						clearROAttribute(msg.Fs3Pth);
					}
					catch
					{
					}

					try
					{
						msg.Fs1 = new FileStream(msg.Fs1Pth, FileMode.Open, FileAccess.Read, FileShare.Read);
						msg.Fs2 = new FileStream(msg.Fs2Pth, FileMode.Open, FileAccess.Read, FileShare.Read);
						if (File.Exists(msg.Fs3Pth))
							msg.Fs3 = new FileStream(msg.Fs3Pth, FileMode.Open, FileAccess.Read, FileShare.None);
						int num1 = (int) (msg.Fs1.Length / (long) RecMasterSize);
						int num2 = (int) (msg.Fs2.Length / (long) RecEmasterSize);
						if (num1 != num2)
						{
							if (num1 < num2)
							{
								string path = DirPath + "master.bak";
								if (File.Exists(path))
								{
									msg.Fs1.Close();
									msg.Fs1 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
								}
							}
							else if (num2 < num1)
							{
								string path = DirPath + "emaster.bak";
								if (File.Exists(path))
								{
									msg.Fs1.Close();
									msg.Fs2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
								}
							}
						}

						hdr = new DirHdr(msg);
						int num3 = (int) (msg.Fs1.Length / (long) RecMasterSize - 1L);
						for (int index = 0; index < num3; ++index)
						{
							DirEntry e = new DirEntry(msg);
							e.Read();
							if (e.Interval.CompareTo('d') == 0 || e.Interval.CompareTo('D') == 0)
							{
								e.AssetID = -1;
								sex.Add(ref e);
								fnAvailable[(int) e.FileNo] = false;
							}
						}

						if (msg.Fs3 != null)
						{
							try
							{
								if (msg.Fs3.Length > 150L)
								{
									int num4 = (int) (msg.Fs3.Length / (long) RecXmasterSize - 1L);
									for (int index = 0; index < num4; ++index)
									{
										DirEntry e = new DirEntry(msg, true);
										e.Read();
										sex.Add(ref e);
										fnAvailable[(int) e.FileNo] = false;
									}
								}
							}
							catch (Exception ex)
							{
								flag = true;
							}

							msg.Fs3.Close();
						}
					}
					catch (Exception ex)
					{
						flag = true;
					}

					msg.Fs1.Close();
					msg.Fs2.Close();
				}

				if (flag)
					return;
				OpenOK = true;
			}
			else
			{
				initFolder();
				hdr = new DirHdr(msg);
				OpenOK = true;
			}
		}

		public void Clear()
		{
			sex.Clear();
			DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
			foreach (FileSystemInfo file in directoryInfo.GetFiles("F*.*"))
				file.Delete();
			foreach (FileSystemInfo file in directoryInfo.GetFiles("*master.*"))
				file.Delete();
			string path = $"{(object) DirPath}MsSmart";
			if (Directory.Exists(path))
				Directory.Delete(path);
			resetFnAvailable();
		}

		public bool FindSymbol(string symbol)
		{
			return sex.ContainsBySymbol(symbol);
		}

		public bool FindFileNr(string fnStr)
		{
			try
			{
				return FindFileNr(Convert.ToInt32(fnStr));
			}
			catch
			{
				return false;
			}
		}

		public bool FindFileNr(int fnr)
		{
			return sex.ContainsByFnr(fnr);
		}

		public void Add(string symbol, string name)
		{
			ushort fNr = 0;
			for (ushort index = 1; (int) index <= maxEntries; ++index)
			{
				if (fnAvailable[(int) index])
				{
					fNr = index;
					fnAvailable[(int) index] = false;
					NgUtils.MyDeleteFile($"{(object) DirPath}MsSmart\\C{(object) index}.mws");
					break;
				}
			}

			Add(symbol, name, fNr);
		}

		public void Add(string symbol, string name, ushort fNr)
		{
			if (sex.Count >= maxEntries || FindSymbol(symbol))
				return;
			DirEntry e = new DirEntry(msg);
			e.Init();
			e.FileNo = fNr;
			e.Sym01 = symbol;
			e.MsName = name;
			e.Emstr.units = (byte) 0;
			sex.Add(ref e);
			Amended = true;
		}

		public void Remove()
		{
			fnAvailable[(int) sex.CurrentEntry.FileNo] = true;
			sex.DeleteCurrentEntry();
		}

		public void Save()
		{
			Directory.CreateDirectory(DirPath);
			if (sex.Count == 0)
			{
				File.Delete(msg.Fs1Pth);
				File.Delete(msg.Fs2Pth);
				File.Delete(msg.Fs3Pth);
			}
			else
			{
				if (File.Exists(msg.Fs1Pth))
					File.Copy(msg.Fs1Pth, msg.Fs1Pth + ".bak", true);
				if (File.Exists(msg.Fs2Pth))
					File.Copy(msg.Fs2Pth, msg.Fs2Pth + ".bak", true);
				if (File.Exists(msg.Fs3Pth))
					File.Copy(msg.Fs3Pth, msg.Fs3Pth + ".bak", true);
				if (!Amended)
					return;
				bool flag = sex.Count > (int) byte.MaxValue;
				msg.Fs1 = new FileStream(msg.Fs1Pth, FileMode.Create, FileAccess.Write, FileShare.None);
				msg.Fs2 = new FileStream(msg.Fs2Pth, FileMode.Create, FileAccess.Write, FileShare.None);
				if (flag)
					msg.Fs3 = new FileStream(msg.Fs3Pth, FileMode.Create, FileAccess.Write, FileShare.None);
				hdr.Save(sex.Count);
				sex.Reset();
				while (sex.Next())
					sex.Get.Write();
				msg.Fs1.Close();
				msg.Fs2.Close();
				if (flag)
					msg.Fs3.Close();
				if (sex.Count >= 256)
					return;
				File.Delete(msg.Fs3Pth);
			}
		}

		public void Reset()
		{
			sex.Reset();
		}

		public bool Next()
		{
			return sex.Next();
		}

		public bool ContainsBySymbol(string symbol)
		{
			return sex.ContainsBySymbol(symbol);
		}

		private void resetFnAvailable()
		{
			fnAvailable = new bool[maxEntries + 1];
			fnAvailable[0] = false;
			for (int index = 1; index <= maxEntries; ++index)
				fnAvailable[index] = true;
		}

		private void initFolder()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
			if (directoryInfo.Exists)
			{
				foreach (FileInfo file in directoryInfo.GetFiles("~*.*"))
				{
					file.Attributes = (FileAttributes) 0;
					file.Delete();
				}

				foreach (FileInfo file in directoryInfo.GetFiles("*.tmp"))
				{
					file.Attributes = (FileAttributes) 0;
					file.Delete();
				}
			}
			else
				directoryInfo.Create();
		}

		private void deleteTempFile(string fn)
		{
			fn = DirPath + fn;
			try
			{
				File.SetAttributes(fn, (FileAttributes) 0);
				File.Delete(fn);
			}
			catch
			{
			}
		}

	}
}

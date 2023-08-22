// Decompiled with JetBrains decompiler
// Type: Nis.Utils.DirEntry
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using Norgate.Utils;
using System.Runtime.InteropServices;

namespace Nis.Utils
{
	public class DirEntry : MsBaseProcs
	{
		public char Interval = 'D';
		internal int dp = 4;
		public string Sym01;
		public string Sym02;
		public string MsName;
		public int AssetID;
		public int Fjd;
		public int Ljd;
		public ushort FileNo;
		public bool Composite;
		public bool XmRec;
		internal RecMaster Mstr;
		internal RecEmaster Emstr;
		internal RecXmaster Xmstr;
		private MsGlobals msg;

		public DirEntry(MsGlobals msglobal)
		{
			msg = msglobal;
		}

		public DirEntry(MsGlobals msglobal, bool xmrec)
		{
			msg = msglobal;
			XmRec = xmrec;
		}

		public void Get()
		{
			if (XmRec)
			{
				Sym01 = NgUtils.ConvertCharArray2String(Xmstr.fi);
				MsName = NgUtils.ConvertCharArray2String(Xmstr.fn);
				try
				{
					Fjd = CalFunc.Ymd2jd(Xmstr.fd01);
				}
				catch
				{
					try
					{
						Fjd = CalFunc.Ymd2jd(Xmstr.fd02);
					}
					catch
					{
						Fjd = 0;
					}
				}

				try
				{
					Ljd = CalFunc.Ymd2jd(Xmstr.ld);
				}
				catch
				{
					Ljd = 0;
				}

				FileNo = Xmstr.rn;
				Interval = Xmstr.interval;
				Composite = Xmstr.Comp > (byte) 0;
			}
			else
			{
				Sym01 = new string(Emstr.sym01);
				string str1 = cleanCharArrayToString(Emstr.xFn);
				string str2 = cleanCharArrayToString(Emstr.fn);
				MsName = str1.Length <= 0 ? str2 : str1;
				try
				{
					Fjd = CalFunc.Ymd2jd(Emstr.fd);
				}
				catch
				{
					try
					{
						Fjd = (int) ms2ieee(Mstr.d1);
					}
					catch
					{
						Fjd = 0;
					}
				}

				try
				{
					Ljd = CalFunc.MsDate2Jd((double) (int) ms2ieee(Mstr.d2));
				}
				catch
				{
					Ljd = 0;
				}

				FileNo = Emstr.rn;
				Interval = Mstr.interval;
				Composite = Emstr.compFlag1 == (byte) 1;
			}

			Sym01.IndexOf(char.MinValue);
			Sym01 = Sym01.Replace("\0", "");
			MsName = MsName.Replace("\0", "");
			if (!Sym01.ToLower().StartsWith("mp_"))
				return;
			dp = 7;
		}

		private string cleanCharArrayToString(char[] p)
		{
			bool flag = false;
			for (int index = 0; index < p.Length; ++index)
			{
				if (p[index] == char.MinValue)
					flag = true;
				if (flag)
					p[index] = char.MinValue;
			}

			return new string(p).Replace("\0", "");
		}

		public void Put()
		{
			if (FileNo > (ushort) byte.MaxValue)
			{
				XmRec = true;
				Xmstr = new RecXmaster();
				Xmstr.fi = Sym01.PadRight(15, char.MinValue).ToCharArray();
				Xmstr.fn = MsName.PadRight(46, char.MinValue).ToCharArray();
				Xmstr.fd01 = CalFunc.Jd2cymd(Fjd);
				Xmstr.fd02 = Xmstr.fd01;
				Xmstr.ld = CalFunc.Jd2cymd(Ljd);
				Xmstr.rn = FileNo;
			}
			else
			{
				Mstr = new RecMaster();
				Emstr = new RecEmaster();
				int num = MsName.Length > 16 ? 1 : 0;
				string str1 = Sym01.PadRight(16, char.MinValue);
				string str2 = MsName.PadRight(17, char.MinValue);
				str1.CopyTo(0, Mstr.fi, 0, 16);
				str2.CopyTo(0, Mstr.fn, 0, 16);
				str1.CopyTo(0, Emstr.sym01, 0, 15);
				str2.CopyTo(0, Emstr.fn, 0, 17);
				if (num != 0)
					MsName.PadRight(45, char.MinValue).CopyTo(0, Emstr.xFn, 0, 45);
				else
					string.Empty.PadRight(45, char.MinValue).CopyTo(0, Emstr.xFn, 0, 45);
				Mstr.entNo = (byte) FileNo;
				Emstr.rn = (ushort) (byte) FileNo;
				Mstr.d1 = ieee2ms((float) CalFunc.Jd2MsDate(Fjd));
				Mstr.d2 = ieee2ms((float) CalFunc.Jd2MsDate(Ljd));
				Emstr.fd = CalFunc.Jd2cymd(Fjd);
				Emstr.d1 = CalFunc.Jd2MsDate(Fjd);
				Emstr.d2 = CalFunc.Jd2MsDate(Ljd);
			}
		}

		public void Init()
		{
			Mstr = new RecMaster();
			Emstr = new RecEmaster();
			Xmstr = new RecXmaster();
		}

		public void Read()
		{
			if (XmRec)
			{
				readFromFileStream(ref Xmstr);
			}
			else
			{
				readFromFileStream(ref Mstr);
				readFromFileStream(ref Emstr);
			}

			Get();
		}

		public void Write()
		{
			Put();
			if (XmRec)
			{
				writeToFileStream(Xmstr);
			}
			else
			{
				writeToFileStream(Mstr);
				writeToFileStream(Emstr);
			}
		}

		private void readFromFileStream(ref RecMaster rec)
		{
			byte[] buffer = new byte[RecMasterSize];
			int offset = 0;
			while (offset < buffer.Length)
				offset += msg.Fs1.Read(buffer, offset, buffer.Length - offset);
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			rec = (RecMaster) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
				typeof(RecMaster));
			gcHandle.Free();
		}

		private void readFromFileStream(ref RecEmaster rec)
		{
			byte[] buffer = new byte[RecEmasterSize];
			int offset = 0;
			while (offset < buffer.Length)
				offset += msg.Fs2.Read(buffer, offset, buffer.Length - offset);
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			rec = (RecEmaster) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
				typeof(RecEmaster));
			gcHandle.Free();
		}

		private void readFromFileStream(ref RecXmaster rec)
		{
			byte[] buffer = new byte[RecXmasterSize];
			int offset = 0;
			while (offset < buffer.Length)
				offset += msg.Fs3.Read(buffer, offset, buffer.Length - offset);
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			rec = (RecXmaster) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
				typeof(RecXmaster));
			gcHandle.Free();
		}

		private void writeToFileStream(RecMaster rec)
		{
			byte[] buffer = new byte[RecMasterSize];
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			Marshal.StructureToPtr((object) rec, gcHandle.AddrOfPinnedObject(), true);
			msg.Fs1.Write(buffer, 0, RecMasterSize);
			gcHandle.Free();
		}

		private void writeToFileStream(RecEmaster rec)
		{
			byte[] buffer = new byte[RecEmasterSize];
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			Marshal.StructureToPtr((object) rec, gcHandle.AddrOfPinnedObject(), true);
			msg.Fs2.Write(buffer, 0, RecEmasterSize);
			gcHandle.Free();
		}

		private void writeToFileStream(RecXmaster rec)
		{
			byte[] buffer = new byte[RecXmasterSize];
			GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
			Marshal.StructureToPtr((object) rec, gcHandle.AddrOfPinnedObject(), true);
			msg.Fs3.Write(buffer, 0, RecXmasterSize);
			gcHandle.Free();
		}
	}
}

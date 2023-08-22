// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsBaseStructs
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nis.Utils
{
	[Serializable]
	public class MsBaseStructs
	{
		public int RecMasterSize = 53;
		public int RecEmasterSize = 192;
		public int RecXmasterSize = 150;

		private static byte[] convertCharArray(char[] ca, int size)
		{
			int num = Math.Min(ca.Length, size);
			byte[] numArray = new byte[size];
			for (int index = 0; index < num; ++index)
				numArray[index] = (byte) ca[index];
			return numArray;
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 53, Pack = 1)]
		public class RecMasterHdr
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 49)]
			private byte[] fill = new byte[49];

			public ushort Count1;
			public ushort Count2;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(53);
					memoryStream.Write(BitConverter.GetBytes(Count1), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(Count2), 0, 2);
					memoryStream.Write(fill, 0, 49);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 53, Pack = 1)]
		public class RecMaster
		{
			public byte fileType = 101;
			public byte recLen = 28;
			public byte noFields = 7;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public char[] fn = new char[16];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] d1 = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] d2 = new byte[4];

			public char interval = 'D';

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public char[] fi = new char[16];

			public byte entNo;
			public byte xx;
			public ushort n1;
			public ushort n2;
			public ushort n3;
			public byte x8;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(53);
					memoryStream.WriteByte(entNo);
					memoryStream.WriteByte(fileType);
					memoryStream.WriteByte(xx);
					memoryStream.WriteByte(recLen);
					memoryStream.WriteByte(noFields);
					memoryStream.Write(BitConverter.GetBytes(n1), 0, 2);
					memoryStream.Write(convertCharArray(fn, 16), 0, 16);
					memoryStream.Write(BitConverter.GetBytes(n2), 0, 2);
					memoryStream.Write(d1, 0, 4);
					memoryStream.Write(d2, 0, 4);
					memoryStream.WriteByte((byte) interval);
					memoryStream.Write(BitConverter.GetBytes(n3), 0, 2);
					memoryStream.Write(convertCharArray(fi, 16), 0, 16);
					memoryStream.WriteByte(x8);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 192, Pack = 1)]
		public class RecEmasterHdr
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 188)]
			private byte[] fill = new byte[188];

			public ushort Count1;
			public ushort Count2;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(192);
					memoryStream.Write(BitConverter.GetBytes(Count1), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(Count2), 0, 2);
					memoryStream.Write(fill, 0, 188);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 192, Pack = 1)]
		public class RecEmaster
		{
			public byte noFields = 7;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			private byte[] s2 = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
			public char[] sym01 = new char[15];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			private byte[] aa = new byte[6];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public char[] fn = new char[17];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
			public char[] fn2 = new char[11];

			private char interval = 'D';

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public byte[] w3 = new byte[2];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] fill01 = new byte[8];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
			public char[] sym02 = new char[15];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			private byte[] fill03 = new byte[6];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			private byte[] fill04 = new byte[2];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] fill05 = new byte[8];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
			public char[] xFn = new char[45];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private byte[] fill06 = new byte[8];

			private ushort b1;
			public ushort rn;
			private ushort n1;
			private byte xx;
			public int d1;
			private int x1;
			public int d2;
			private int x2;
			public byte compFlag1;
			private byte fill02;
			public char compOp;
			public float wFactor1;
			public float wFactor2;
			private int int01;
			public int fd;
			public byte units;

			public RecEmaster()
			{
				s2[0] = (byte) 127;
				s2[1] = (byte) 0;
				s2[2] = (byte) 32;
				s2[3] = (byte) 0;
			}

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(192);
					memoryStream.Write(BitConverter.GetBytes(b1), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(rn), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(n1), 0, 2);
					memoryStream.WriteByte(noFields);
					memoryStream.Write(s2, 0, 4);
					memoryStream.Write(convertCharArray(sym01, 15), 0, 15);
					memoryStream.Write(aa, 0, 6);
					memoryStream.Write(convertCharArray(fn, 17), 0, 17);
					memoryStream.Write(convertCharArray(fn2, 11), 0, 11);
					memoryStream.WriteByte((byte) interval);
					memoryStream.Write(w3, 0, 2);
					memoryStream.WriteByte(xx);
					memoryStream.Write(BitConverter.GetBytes(d1), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(x1), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(d2), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(x2), 0, 4);
					memoryStream.Write(fill01, 0, 8);
					memoryStream.WriteByte(compFlag1);
					memoryStream.WriteByte(fill02);
					memoryStream.Write(convertCharArray(sym02, 15), 0, 15);
					memoryStream.Write(fill03, 0, 6);
					memoryStream.WriteByte((byte) compOp);
					memoryStream.Write(fill04, 0, 2);
					memoryStream.Write(BitConverter.GetBytes(wFactor1), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(wFactor2), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(int01), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(fd), 0, 4);
					memoryStream.WriteByte(units);
					memoryStream.Write(fill05, 0, 8);
					memoryStream.Write(convertCharArray(xFn, 45), 0, 45);
					memoryStream.Write(fill06, 0, 8);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 150, Pack = 1)]
		public class RecXmasterHdr
		{
			public int i1 = 1297677917;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] a1 = new byte[6];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
			public byte[] a2 = new byte[128];

			public int Count1;
			public int Count2;
			public int NextFno;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(150);
					memoryStream.Write(BitConverter.GetBytes(i1), 0, 4);
					memoryStream.Write(a1, 0, 6);
					memoryStream.Write(BitConverter.GetBytes(Count1), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(Count2), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(NextFno), 0, 4);
					memoryStream.Write(a2, 0, 128);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 150, Pack = 1)]
		public class RecXmaster
		{
			private byte leader = 1;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
			public char[] fi = new char[15];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
			public char[] fn = new char[46];

			public char interval = 'D';
			private uint nil02 = 2130706432;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			private byte[] nil04 = new byte[20];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			private byte[] nil07 = new byte[20];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
			private byte[] nil08 = new byte[9];

			private ushort nil01;
			public ushort rn;
			private byte byte01;
			private int ms01;
			private int nil03;
			private int ms02;
			public int fd01;
			public int fd02;
			private int nil05;
			public int ld;
			public byte Comp;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(150);
					memoryStream.WriteByte(leader);
					memoryStream.Write(convertCharArray(fi, 15), 0, 15);
					memoryStream.Write(convertCharArray(fn, 46), 0, 46);
					memoryStream.WriteByte((byte) interval);
					memoryStream.Write(BitConverter.GetBytes(nil01), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(rn), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(nil02), 0, 4);
					memoryStream.WriteByte(byte01);
					memoryStream.Write(BitConverter.GetBytes(ms01), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(nil03), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(ms02), 0, 4);
					memoryStream.Write(nil04, 0, 20);
					memoryStream.Write(BitConverter.GetBytes(fd01), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(fd02), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(nil05), 0, 4);
					memoryStream.Write(BitConverter.GetBytes(ld), 0, 4);
					memoryStream.Write(nil07, 0, 20);
					memoryStream.WriteByte(Comp);
					memoryStream.Write(nil08, 0, 9);
					return memoryStream;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 28, Pack = 1)]
		public class RecMsDay
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] d = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] o = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] h = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] l = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] c = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] v = new byte[4];

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public byte[] i = new byte[4];

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(28);
					memoryStream.Write(d, 0, 4);
					memoryStream.Write(o, 0, 4);
					memoryStream.Write(h, 0, 4);
					memoryStream.Write(l, 0, 4);
					memoryStream.Write(c, 0, 4);
					memoryStream.Write(v, 0, 4);
					memoryStream.Write(i, 0, 4);
					return memoryStream;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 28, Pack = 1)]
		public class RecMsDayHeader
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
			public byte[] Fill = new byte[24];

			public short I;
			public short Count;

			public MemoryStream ToMemoryStream
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream(28);
					memoryStream.Write(BitConverter.GetBytes(I), 0, 2);
					memoryStream.Write(BitConverter.GetBytes(Count), 0, 2);
					memoryStream.Write(Fill, 0, 24);
					return memoryStream;
				}
			}
		}

		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 52, Pack = 1)]
		public class RecDay
		{
			public int d;
			public float o;
			public float h;
			public float l;
			public float c;
			public float v;
			public float i;

			public RecDay()
			{
			}

			public RecDay(int d, float o, float h, float l, float c, float v, float i)
			{
				this.d = d;
				this.o = o;
				this.h = h;
				this.l = l;
				this.c = c;
				this.v = v;
				this.i = i;
			}
		}
	}
}

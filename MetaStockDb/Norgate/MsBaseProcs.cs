// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsBaseProcs
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System;
using System.IO;

namespace Nis.Utils
{
	[Serializable]
	public class MsBaseProcs : MsBaseStructs
	{
		private byte[] msbinZero = new byte[4];
		internal byte[] ieee = new byte[4];

		internal float ms2ieee(byte[] msbin)
		{
			byte[] numArray = new byte[4];
			byte num1 = (byte) ((uint) msbin[2] & 128U);
			for (int index = 0; index < 4; ++index)
				numArray[index] = (byte) 0;
			if (msbin[3] == (byte) 0)
				return 0.0f;
			numArray[3] |= num1;
			byte num2 = (byte) ((uint) msbin[3] - 2U);
			numArray[3] |= (byte) ((uint) num2 >> 1);
			numArray[2] |= (byte) ((uint) num2 << 7);
			numArray[2] |= (byte) ((uint) msbin[2] & (uint) sbyte.MaxValue);
			numArray[1] = msbin[1];
			numArray[0] = msbin[0];
			return BitConverter.ToSingle(numArray, 0);
		}

		internal byte[] ieee2ms(double sn)
		{
			return ieee2ms((float) sn);
		}

		internal byte[] ieee2ms(float sn)
		{
			byte[] numArray = new byte[4];
			ieee = BitConverter.GetBytes(sn);
			byte num1 = (byte) ((uint) ieee[3] & 128U);
			ieee[3] &= (byte) 127;
			byte num2 = (byte) ((uint) (byte) ((uint) ieee[3] << 1) | (uint) (byte) ((uint) ieee[2] >> 7));
			if (num2 == (byte) 254)
				return msbinZero;
			byte num3 = (byte) ((uint) num2 + 2U);
			numArray[3] = num3;
			numArray[2] = num1;
			numArray[2] |= (byte) ((uint) ieee[2] & (uint) sbyte.MaxValue);
			numArray[1] = ieee[1];
			numArray[0] = ieee[0];
			return numArray;
		}

		internal int calcMsDate(int jd)
		{
			DateTime dateTime = DateTime.FromOADate((double) jd);
			int year = dateTime.Year;
			int num1;
			int num2;
			int num3;
			int num4;
			if (year > 2004)
			{
				num1 = 1233137576;
				num2 = 8;
				num3 = (year - 2005) / 10;
				num4 = (year - 2005) % 10;
			}
			else if (year > 1952)
			{
				num1 = 1224829776;
				num2 = 16;
				num3 = (year - 1953) / 10;
				num4 = (year - 1953) % 10;
			}
			else if (year > 1926)
			{
				num1 = 1216602784;
				num2 = 32;
				num3 = (year - 1927) / 10;
				num4 = (year - 1927) % 10;
			}
			else if (dateTime.Year > 1912)
			{
				num1 = 1207835264;
				num2 = 64;
				num3 = (year - 1913) / 10;
				num4 = (year - 1913) % 10;
			}
			else
			{
				num1 = 0;
				num2 = 0;
				num3 = 0;
				num4 = 0;
			}

			if (num2 > 0)
			{
				int num5 = dateTime.Month - 1;
				int num6 = dateTime.Day - 1;
				num1 = num1 + num3 * num2 * 100000 + num4 * num2 * 10000 + num5 * num2 * 100 + num6 * num2;
			}

			return num1;
		}

		internal int msd2jd(float msdate)
		{
			int num1 = (int) Math.Round((double) msdate, 0);
			int day;
			int month;
			int year;
			if (num1 < 0)
			{
				day = num1 % 100 + 100;
				month = num1 / 100 % 100 + 99;
				year = num1 / 10000 + 1899;
			}
			else
			{
				int num2 = num1 / 10000;
				int num3 = num1 % 10000;
				month = num3 / 100;
				day = num3 % 100;
				year = num2 + 1900;
			}

			return Convert.ToInt32(new DateTime(year, month, day).ToOADate());
		}

		internal int msjd2ymd(int msjd)
		{
			DateTime dateTime = DateTime.FromOADate((double) msjd);
			return (int) Math.Truncate(10000.0 * (double) (dateTime.Year - 1900) + (double) (dateTime.Month * 100) +
			                           (double) dateTime.Day);
		}

		internal void clearROAttribute(string fn)
		{
			if (!File.Exists(fn))
				return;
			int num = 1;
			int attributes = (int) File.GetAttributes(fn);
			if ((attributes & num) <= 0)
				return;
			File.SetAttributes(fn, (FileAttributes) (attributes ^ num));
		}
	}
}

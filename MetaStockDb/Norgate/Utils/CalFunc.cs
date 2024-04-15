// Decompiled with JetBrains decompiler
// Type: Norgate.Utils.CalFunc
// Assembly: norgate.general.4, Version=4.0.2.1, Culture=neutral, PublicKeyToken=d5eccc6216068f48
// MVID: A122D709-041A-426F-9B6C-C1457ADC069B
// Assembly location: C:\Program Files (x86)\Premium Data Converter\norgate.general.4.dll

namespace Norgate.Utils
{
	public class CalFunc
	{
		public static string[] mths = new string[12]
		{
			"Jan",
			"Feb",
			"Mar",
			"Apr",
			"May",
			"Jun",
			"Jul",
			"Aug",
			"Sep",
			"Oct",
			"Nov",
			"Dec"
		};

		public static string[] dayname = new string[7]
		{
			"Sunday   ",
			"Monday   ",
			"Tuesday  ",
			"Wednesday",
			"Thursday ",
			"Friday   ",
			"Saturday "
		};

		public static string[] shortdn = new string[7]
		{
			"Sun",
			"Mon",
			"Tue",
			"Wed",
			"Thu",
			"Fri",
			"Sat"
		};

		public static byte[] maxDays = new byte[12]
		{
			(byte) 31,
			(byte) 28,
			(byte) 31,
			(byte) 30,
			(byte) 31,
			(byte) 30,
			(byte) 31,
			(byte) 31,
			(byte) 30,
			(byte) 31,
			(byte) 30,
			(byte) 31
		};

		public static char[] delmth = new char[13]
		{
			'A',
			'F',
			'G',
			'H',
			'J',
			'K',
			'M',
			'N',
			'Q',
			'U',
			'V',
			'X',
			'Z'
		};

		private static int day = 0;
		private static int month = 0;
		private static int year = 0;

		private static void setYMD(int ymd)
		{
			if (ymd == 0)
            {
                day = 1;
                month = 1;
                year = 1900;
            }
			else if (ymd < 0)
			{
                day = ymd % 100 + 100;
                month = ymd / 100 % 100 + 99;
                year = ymd / 10000 + 1899;
			}
			else
			{
                year = ymd / 10000;
				ymd %= 10000;
                month = ymd / 100;
				ymd %= 100;
                day = ymd;
				if (year >= 1800)
					return;
                year += 1900;
			}
		}

		public static int Ymd2jd(int ymd)
		{
            setYMD(ymd);
			return (int) Math.Truncate(new DateTime(year, month, day).ToOADate());
		}

		public static int Jd2cymd(int jd)
		{
			DateTime dateTime = DateTime.FromOADate((double) jd);
            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;
			return 10000 * year + 100 * month + day;
		}

		public static string Jd2cymds(int jd)
		{
			return Jd2cymd(jd).ToString();
		}

		public static int Jd2ymd(int jd)
		{
			return Jd2cymd(jd) % 1000000;
		}

		public static int MsDate2Jd(double msdate)
		{
			int num1 = (int) Math.Round(msdate);
			if (num1 < 0)
			{
                day = num1 % 100 + 100;
                month = num1 / 100 % 100 + 99;
                year = num1 / 10000 + 1899;
			}
			else
			{
                year = num1 / 10000;
				int num2 = num1 % 10000;
                month = num2 / 100;
                day = num2 % 100;
                year += 1900;
			}

			return (int) Math.Truncate(new DateTime(year, month, day).ToOADate());
		}

		public static int Jd2MsDateOld(int jd)
		{
			int num1 = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			DateTime dateTime1 = new DateTime();
			DateTime dateTime2 = DateTime.FromOADate((double) jd);
			int year = dateTime2.Year;
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
				num4 = (year - 1927) / 10;
			}
			else if (year > 1912)
			{
				num1 = 1207835264;
				num2 = 64;
				num3 = (year - 1913) / 10;
				num4 = (year - 1913) % 10;
			}

			if (num2 > 0)
			{
				int num5 = dateTime2.Month - 1;
				int num6 = dateTime2.Day - 1;
				num1 = num1 + num3 * num2 * 100000 + num4 * num2 * 10000 + num5 * num2 * 100 + num6 * num2;
			}

			return num1;
		}

		public static int Jd2MsDate(int jd)
		{
			DateTime dateTime = DateTime.FromOADate((double) jd);
			return 10000 * (dateTime.Year - 1900) + dateTime.Month * 100 + dateTime.Day;
		}

		public static int YmdS2jd(string s)
		{
			try
			{
				return Ymd2jd(Convert.ToInt32(s));
			}
			catch
			{
				return 0;
			}
		}

		public static int PrevBusJD(int jd)
		{
			DateTime dateTime = DateTime.FromOADate((double) (jd - 1));
			while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
				dateTime.AddDays(-1.0);
			return (int) Math.Truncate(dateTime.ToOADate());
		}

		public static void IncYM(ref int ym, int i)
		{
			int num1 = ym / 100;
			int num2 = ym % 100 + i;
			while (num2 > 12)
			{
				++num1;
				num2 -= 12;
			}

			ym = num1 * 100 + num2;
		}

		public static void DecYM(ref int ym, int i)
		{
			int num1 = ym / 100;
			int num2 = ym % 100 - i;
			while (num2 < 1)
			{
				--num1;
				num2 += 12;
			}

			ym = num1 * 100 + num2;
		}

		public static int GetMonthNumber(string ms)
		{
			bool flag = false;
			int index;
			for (index = 0; index < 12; ++index)
			{
				if (ms.ToLower().Equals(mths[index].ToLower()))
				{
					flag = true;
					break;
				}
			}

			if (flag)
				return index + 1;
			return 0;
		}

		public static string aa(string s)
		{
			return "";
		}
	}
}

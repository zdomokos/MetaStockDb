using System.Runtime.InteropServices;
using System.Text;

namespace MetaStockDb
{
	public static class MsFileIO
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct msbin2ieeVariant
		{
			[FieldOffset(0)] public float a;
			[FieldOffset(0)] public UInt32 b;
		}

		public static string ConvertToFillString(byte[] ba, int size)
		{
			var sb = new StringBuilder();
			bool allnull = true;
			char ch;

			for (int i = 0; i < size && allnull; i++)
			{
				if (ba[i] != 0)
					allnull = false;
			}

			if (allnull)
			{
				sb.AppendFormat("[NULL:{0}]", size);
				return sb.ToString();
			}

			for (int i = 0; i < size; i++)
			{
				ch = (char)ba[i];
				if (ch < 127 && (ch == ' ' || Char.IsLetterOrDigit(ch) || Char.IsPunctuation(ch)))
					sb.AppendFormat(" {0}", ch);
				else
					sb.AppendFormat("{0:X2}", ba[i]);
			}

			return sb.ToString();
		}

		public static string ReadFillString(BinaryReader br, byte[] ba, int count)
		{
			ReadIntoArray(br, ba, count);
			return ConvertToFillString(ba, count);
		}

		public static string ReadHexString(BinaryReader br, byte[] ba, int count)
		{
			ReadIntoArray(br, ba, count);
			return ConvertToHexString(ba, count);
		}

		public static string ReadStringField(BinaryReader br, byte[] ba, int count)
		{
			ReadIntoArray(br, ba, count);
			StringBuilder sb = new StringBuilder(count + 1);

			for (int i = 0; i < count; i++)
			{
				if (ba[i] == 0)
					break;
				sb.Append((char)ba[i]);
			}
			return sb.ToString();
		}

		public static void ReadIntoArray(BinaryReader br, byte[] data, int count)
		{
			int offset = 0;
			int remaining = count;
			while (remaining > 0)
			{
				int read = br.Read(data, offset, remaining);
				if (read <= 0)
					throw new EndOfStreamException
						($"End of stream reached with {remaining} bytes left to read");
				remaining -= read;
				offset += read;
			}
		}

		public static string ConvertToHexString(byte[] ba, int size)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < size; i++)
				sb.AppendFormat("{0:X} ", ba[i]);

			return sb.ToString();
		}

		public static string ConvertToHex(byte[] ba)
		{
			StringBuilder sb = new StringBuilder();

			foreach (byte b in ba)
				sb.AppendFormat("{0:X} ", b);

			return sb.ToString();
		}

		public static string ConvertToHex(byte[] ba, int row_len)
		{
			StringBuilder sb = new StringBuilder();
			int col = 0;
			char ch;

			foreach (byte b in ba)
			{
				ch = (char)b;
				if (ch < 127 && (ch == ' ' || Char.IsLetterOrDigit(ch) || Char.IsPunctuation(ch)))
					sb.AppendFormat(" {0} ", ch);
				else
					sb.AppendFormat("{0:X2} ", b);
				col++;
				if (col >= row_len)
				{
					col = 0;
					sb.Append("\r\n");
				}
				else if (col % 8 == 0)
				{
					sb.Append("  ");
				}
			}

			return sb.ToString();
		}

		public static string ConvertDateToString(uint msbin_date)
		{
			float ieee_date = 0;
			ConvertToIeeeFloat(msbin_date, ref ieee_date);
			uint date = ((uint)ieee_date) + 19000000;
			return date.ToString();
		}

		public static int ConvertToIeeeFloat(uint src, ref float dest)
		{
			msbin2ieeVariant c;
			UInt16 man;
			UInt16 exp;

			c.a = 0; // to eliminate compiler warnings
			c.b = src;

			if (c.b > 0)
			{
				man = (UInt16)(c.b >> 16);
				exp = (UInt16)((man & 0xff00u) - 0x0200u);
				//if (exp & 0x8000 != man & 0x8000)
				//    return 1;   // exponent overflow 
				man = (UInt16)(man & 0x7fu | (man << 8) & 0x8000u);   // move sign 
				man |= (UInt16)(exp >> 1);
				c.b = (c.b & 0xffffu);
				c.b |= (UInt32)(man << 16);
			}

			dest = c.a;

			return 0;
		}

		public static string ConvertDateToString(float ieee_date) {
			uint date = 0;
			if (ieee_date > 0) 
				date = ((uint) ieee_date) + 19000000;
			return date.ToString();
		}
		
		public static DateTime ConvertDateTime(float ieee_date) {
			int date = (int) ieee_date;
			if (date <= 0) return DateTime.MinValue;
			if (date < 10000000)
				date += 19000000;
			var y = date/10000;
			var m = date % 10000 / 100;
			var d = date % 100;
			return new DateTime(y,m,d);
		}
		
		public static DateTime ConvertDateTime(uint ieee_date) {
			int date = (int) ieee_date;
			if (date <= 0) return DateTime.MinValue;
			if (date < 10000000)
				date += 19000000;
			var y = date/10000;
			var m = date % 10000 / 100;
			var d = date % 100;
			return new DateTime(y,m,d);
		}
	}
}

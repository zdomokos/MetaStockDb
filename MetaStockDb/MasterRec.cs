namespace MetaStockDb
{
    public struct MSHeader
    {
        public int    num;
        public int    max;
        public string RemainText;
    }

    public struct MasterRec
    {
        public byte   FileNumber;
        public byte   FileType1, FileType2; // CT file type = 0'e' (5 or 7 flds)
        public byte   RecLen;               //record length in bytes (4 x NumFields)
        public byte   NumFields;
        public byte   Reserved1;
        public byte   CenturyIndicator; // (Or reserved???)
        public string Name;             // 16 bytes
        public byte   Reserved2;
        public byte   CTFlag;       // ????? if CT ver. 2.8, 'Y'; o.w., anything else
        public uint   FirstDate;    // 4 bytes MBF
        public uint   LastDate;     // 4 bytes MBF
        public byte   TimeInterval; // (D,W,M) 1 Char
        public ushort IDATimeBase;  // Intraday time base (Or reserved??)
        public string Symbol;       // 14 bytes
        public byte   Reserved3;    // Must be a space for MS???
        public byte   Flag;         // ' ' or '*' for autorun
        public byte   Reserved4;

        public string[] ToStringArray()
        {
            string file_type = $"{FileType1:X2} {FileType2:X2}";
            return new string[]
                   {
                       FileNumber.ToString("D4"),
                       file_type,
                       RecLen.ToString(),
                       NumFields.ToString(),
                       Reserved1.ToString("X2"),
                       CenturyIndicator.ToString("X2"),
                       Name,
                       Reserved2.ToString("X2"),
                       CTFlag.ToString("X2"), MsFileIO.ConvertDateToString(FirstDate),
                       MsFileIO.ConvertDateToString(LastDate),
                       new string((char)TimeInterval, 1),
                       IDATimeBase.ToString(),
                       Symbol,
                       Reserved3.ToString("X2"),
                       Flag.ToString("X2"),
                       Reserved4.ToString("X2")
                   };
        }
        /* Microsoft Basic floating point format to IEEE floating point format */
    }

    public struct EMasterRec
    {
        // EMASTER file
        public byte   asc1, asc2; // 2 digits ascii number???
        public byte   FileNumber;
        public string Fill1; // 3 bytes
        public byte   NumFields;
        public byte   Del;
        public byte   FB1;
        public byte   Space;
        public byte   FB2;
        public string Symbol;         // 14 bytes
        public string Fill2;          // 7 bytes
        public string Name;           // 16 bytes
        public string Fill3;          // 12 bytes
        public byte   TimeFrame;      // Timeframe: I=Intraday, D=Daily, W=Weekly, M=Monthly 
        public string Fill4;          // 3 bytes (& Time Interval)
        public float  FirstDate;      // 4 bytes MBF
        public float  BeginTradeTime; // 4 bytes MBF
        public float  LastDate;       // 4 bytes MBF
        public float  EndTradeTime;   // 4 bytes MBF
        public float  StartTimeRange; // 4 bytes MBF
        public float  EndTimeRange;   // 4 bytes MBF
        public string Fill5;          // 38 bytes
        public string MysteryData;    // 4 bytes
        public string Fill6;          // 9 bytes
        public string ExtName;
        public string Remainder; // 53 bytes minus whatever ExtName used

        public string[] ToStringArray()
        {
            string asc_str = $"{(char)asc1}{(char)asc2}";

            return new string[]
                   {
                       asc_str.ToString(),
                       FileNumber.ToString("D4"),
                       Fill1,
                       NumFields.ToString(),
                       Del.ToString("X2"),
                       FB1.ToString("X"),
                       Space.ToString("X2"),
                       FB2.ToString("X"),
                       Symbol,
                       Fill2,
                       Name,
                       Fill3,
                       new string((char)TimeFrame, 1),
                       Fill4, MsFileIO.ConvertDateToString(FirstDate), MsFileIO.ConvertDateToString(BeginTradeTime),
                       MsFileIO.ConvertDateToString(LastDate), MsFileIO.ConvertDateToString(EndTradeTime),
                       MsFileIO.ConvertDateToString(StartTimeRange), MsFileIO.ConvertDateToString(EndTimeRange),
                       Fill5,
                       MysteryData,
                       Fill6,
                       ExtName,
                       Remainder
                   };
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct XMasterRec
    {
        // XMASTER file
        public byte   StartByte; // 0x01
        public string Symbol;    // 15 bytes
        public string Name;      // 46 bytes
        public byte   TimeFrame; // Timeframe: I=Intraday, D=Daily, W=Weekly, M=Monthly 
        public string Fill1;     // 2 bytes
        public UInt16 FileNumber;
        public string Fill2; // 3 bytes
        public byte   Del;   // ASCII 127
        public string Fill3; // 9 bytes
        public UInt32 Date1;
        public UInt32 Mystery1;
        public string Fill4; // 16 bytes
        public UInt32 FirstDate1;
        public UInt32 FirstDate2;
        public string Fill5; // 4 bytes
        public UInt32 LastDate;
        public string Fill6; // 30 bytes

        public string[] ToStringArray()
        {
            return new string[]
                   {
                       StartByte.ToString("X2"),
                       Symbol,
                       Name,
                       new string((char)TimeFrame, 1),
                       Fill1,
                       FileNumber.ToString("D4"),
                       Fill2,
                       Del.ToString("X2"),
                       Fill3,
                       Date1.ToString(),
                       Mystery1.ToString(),
                       Fill4,
                       FirstDate1.ToString(),
                       FirstDate2.ToString(),
                       Fill5,
                       LastDate.ToString(),
                       Fill6,
                   };
        }
    }
}
using System.Text;

namespace MetaStockDb;

public abstract class MetaStockFile<T>
{
    public MetaStockFile()
    {
        Records = new List<T>();
    }

    public string   FileName { get; set; }
    public MSHeader Header;
    public List<T>  Records { get; set; }

    public abstract void Load(string fileName);
}

public class MasterFile : MetaStockFile<MasterRec>
{
    public override void Load(string filename)
    {
        FileName = filename;

        FileStream   fs = null;
        BinaryReader br = null;

        try
        {
            fs = new FileStream(filename, FileMode.Open);
            br = new BinaryReader(fs);

            Header.num = br.ReadInt16();
            Header.max = br.ReadInt16();
            byte[] buffer = br.ReadBytes(49);
            Header.RemainText = MsFileIO.ConvertToHex(buffer, 16);
            byte[] tmpbuf = new byte[60];

            for (int i = 0; i < Header.num; i++)
            {
                MasterRec rec;
                rec.FileNumber       = br.ReadByte();
                rec.FileType1        = br.ReadByte();
                rec.FileType2        = br.ReadByte();
                rec.RecLen           = br.ReadByte();
                rec.NumFields        = br.ReadByte();
                rec.Reserved1        = br.ReadByte();
                rec.CenturyIndicator = br.ReadByte();

                rec.Name = MsFileIO.ReadStringField(br, tmpbuf, 16).Trim();

                rec.Reserved2    = br.ReadByte();
                rec.CTFlag       = br.ReadByte();
                rec.FirstDate    = br.ReadUInt32();
                rec.LastDate     = br.ReadUInt32();
                rec.TimeInterval = br.ReadByte();
                rec.IDATimeBase  = br.ReadUInt16();

                rec.Symbol = MsFileIO.ReadStringField(br, tmpbuf, 14).Trim();

                rec.Reserved3 = br.ReadByte();
                rec.Flag      = br.ReadByte();
                rec.Reserved4 = br.ReadByte();

                Records.Add(rec);
            }
        }
        catch (Exception)
        {
            // log
            throw;
        }
        finally
        {
            br?.Close();
            fs?.Close();
        }
    }
}

public class EMasterFile : MetaStockFile<EMasterRec>
{
    public override void Load(string filename)
    {
        FileName = filename;

        FileStream   fs = null;
        BinaryReader br = null;

        try
        {
            byte[] tmpbuf = new byte[200];
            byte[] rembuf = new byte[60];

            fs = new FileStream(filename, FileMode.Open);
            br = new BinaryReader(fs);

            Header.num = br.ReadInt16();
            Header.max = br.ReadInt16();

            var sb = new StringBuilder();
            br.Read(tmpbuf, 0, 45);
            sb.Append(MsFileIO.ConvertToFillString(tmpbuf, 45));
            sb.Append("\r\n");
            br.Read(tmpbuf, 0, 4);
            sb.Append(MsFileIO.ConvertToFillString(tmpbuf, 4));
            sb.Append("\r\n");
            byte[] buffer = br.ReadBytes(139);
            sb.Append(MsFileIO.ConvertToHex(buffer, 16));
            Header.RemainText = sb.ToString();

            for (int i = 0; i < Header.num; i++)
            {
                EMasterRec rec;

                rec.asc1           = br.ReadByte();
                rec.asc2           = br.ReadByte();
                rec.FileNumber     = br.ReadByte();
                rec.Fill1          = MsFileIO.ReadHexString(br, tmpbuf, 3);
                rec.NumFields      = br.ReadByte();
                rec.Del            = br.ReadByte();
                rec.FB1            = br.ReadByte();
                rec.Space          = br.ReadByte();
                rec.FB2            = br.ReadByte();
                rec.Symbol         = MsFileIO.ReadStringField(br, tmpbuf, 14);
                rec.Fill2          = MsFileIO.ReadHexString(br, tmpbuf, 7);
                rec.Name           = MsFileIO.ReadStringField(br, tmpbuf, 16);
                rec.Fill3          = MsFileIO.ReadHexString(br, tmpbuf, 12);
                rec.TimeFrame      = br.ReadByte();
                rec.Fill4          = MsFileIO.ReadHexString(br, tmpbuf, 3);
                rec.FirstDate      = br.ReadSingle();
                rec.BeginTradeTime = br.ReadSingle();
                rec.LastDate       = br.ReadSingle();
                rec.EndTradeTime   = br.ReadSingle();
                rec.StartTimeRange = br.ReadSingle();
                rec.EndTimeRange   = br.ReadSingle();
                rec.Fill5          = MsFileIO.ReadFillString(br, tmpbuf, 38);
                rec.MysteryData    = MsFileIO.ReadHexString(br, tmpbuf, 4);
                rec.Fill6          = MsFileIO.ReadFillString(br, tmpbuf, 9);
                rec.ExtName        = MsFileIO.ReadStringField(br, tmpbuf, 53);
                Array.Copy(tmpbuf, rec.ExtName.Length, rembuf, 0, 53 - rec.ExtName.Length);
                rec.Remainder = MsFileIO.ConvertToFillString(rembuf, 53 - rec.ExtName.Length);

                Records.Add(rec);
            }
        }
        catch (Exception)
        {
            // log
            throw;
        }
        finally
        {
            br?.Close();
            fs?.Close();
        }
    }
}

public class XMasterFile : MetaStockFile<XMasterRec>
{
    public override void Load(string filename)
    {
        FileName = filename;
        FileStream   fs = null;
        BinaryReader br = null;

        try
        {
            byte[] tmpbuf = new byte[150];

            fs = new FileStream(filename, FileMode.Open);
            br = new BinaryReader(fs);

            var sb = new StringBuilder();
            br.Read(tmpbuf, 0, 10);
            sb.Append(MsFileIO.ConvertToFillString(tmpbuf, 10));
            sb.Append("\r\n");

            Header.num = br.ReadUInt16();
            UInt16 f1 = br.ReadUInt16();
            Header.max = br.ReadUInt16();
            UInt16 f2   = br.ReadUInt16();
            UInt32 next = br.ReadUInt16();
            UInt16 f3   = br.ReadUInt16();
            sb.AppendFormat("Next: {0}\r\n", next);

            byte[] buffer = br.ReadBytes(128);
            sb.Append(MsFileIO.ConvertToHex(buffer, 16));
            Header.RemainText = sb.ToString();

            for (UInt32 i = 0; i < Header.num; i++)
            {
                XMasterRec rec;

                rec.StartByte  = br.ReadByte();
                rec.Symbol     = MsFileIO.ReadStringField(br, tmpbuf, 15);
                rec.Name       = MsFileIO.ReadStringField(br, tmpbuf, 46);
                rec.TimeFrame  = br.ReadByte();
                rec.Fill1      = MsFileIO.ReadHexString(br, tmpbuf, 2);
                rec.FileNumber = br.ReadUInt16();
                rec.Fill2      = MsFileIO.ReadHexString(br, tmpbuf, 3);
                rec.Del        = br.ReadByte();
                rec.Fill3      = MsFileIO.ReadFillString(br, tmpbuf, 9);
                rec.Date1      = br.ReadUInt32();
                rec.Mystery1   = br.ReadUInt32();
                rec.Fill4      = MsFileIO.ReadFillString(br, tmpbuf, 16);
                rec.FirstDate1 = br.ReadUInt32();
                rec.FirstDate2 = br.ReadUInt32();
                rec.Fill5      = MsFileIO.ReadHexString(br, tmpbuf, 4);
                rec.LastDate   = br.ReadUInt32();
                rec.Fill6      = MsFileIO.ReadFillString(br, tmpbuf, 30);

                Records.Add(rec);
            }
        }
        catch (Exception)
        {
            // log
            throw;
        }
        finally
        {
            br?.Close();
            fs?.Close();
        }
    }
}

public class PriceDateFile : MetaStockFile<PriceRec>
{
    public PriceDateFile()
    {
    }

    public PriceDateFile(string dbRoot, StockDataHeader hdr)
    {
        string extension = hdr.FileNumber > 255 ? "mwd" : "dat";
        FileName        = @$"{dbRoot}\{hdr.Classifier}\F{hdr.FileNumber}.{extension}";
        StockDataHeader = hdr;
    }

    public StockDataHeader StockDataHeader { get; }

    public void Load()
    {
        Load(FileName);
    }

    public override void Load(string fileName)
    {
        FileName = fileName;
        FileStream   fs = null;
        BinaryReader br = null;

        try
        {
            fs = new FileStream(fileName, FileMode.Open);
            br = new BinaryReader(fs);

            fs.Seek(2, SeekOrigin.Begin);
            Header.num = br.ReadUInt16();

            fs.Seek(28, SeekOrigin.Begin);

            for (UInt32 i = 0; i < Header.num - 1; i++)
            {
                var   rec = new PriceRec();
                float f   = 0;
                uint  tmp;

                tmp = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Datef = f;
                tmp       = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Open = f;
                tmp      = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.High = f;
                tmp      = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Low = f;
                tmp     = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Close = f;
                tmp       = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Volume = f;
                tmp        = br.ReadUInt32();
                MsFileIO.ConvertToIeeeFloat(tmp, ref f);
                rec.Unadj = f;

                int date = Convert.ToInt32(Math.Floor(rec.Datef));
                int y    = date / 10000;

                rec.Date = new DateTime();

                Records.Add(rec);
            }
        }
        catch (Exception)
        {
            // log
            throw;
        }
        finally
        {
            br?.Close();
            fs?.Close();
        }
    }
}
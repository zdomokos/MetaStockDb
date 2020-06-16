using System;

namespace MetaStockDb
{
    public interface IBar
    {
        DateTime OpenTime { get; }

        double Open { get;  }
        double High { get; }
        double Low { get;  }
        double Close { get;  }

        long Volume { get; }
    }

    public class PriceRec
    {
        public DateTime Date;
        public float Datef;
        public float Open;
        public float High;
        public float Low;
        public float Close;
        public float Volume;
        public float Unadj;

        public string[] ToStringArray()
        {
            return new string[]
            {
                MsFileIO.ConvertDateToString(Datef),
                Open.ToString(),
                High.ToString(),
                Low.ToString(),
                Close.ToString(),
                Volume.ToString(),
                Unadj.ToString(),
            };
        }

        public override string ToString()
        {
            return $"{MsFileIO.ConvertDateToString(Datef)}, {Open}, {High}, {Low}, {Close}, {Volume}, {Unadj}";
        }
    }
}
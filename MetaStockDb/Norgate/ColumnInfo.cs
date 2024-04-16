// Decompiled with JetBrains decompiler
// Type: Nis.Utils.ColumnInfo
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

namespace Nis.Utils
{
    public class ColumnInfo
    {
        public string Name;
        public int    DP;
        public string outputName;

        public ColumnInfo(string name, int decPlaces, string outputName)
        {
            Name            = name;
            DP              = decPlaces;
            this.outputName = outputName;
        }
    }
}
// Decompiled with JetBrains decompiler
// Type: Nis.Utils.TextFileTranslateParams
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System.Globalization;

namespace Nis.Utils;

public class TextFileTranslateParams
{
    public bool             UseHeaders = true;
    public bool             UseQuotes  = true;
    public string           Extension  = "txt";
    public string           DateFormat = "yyyyMMdd";
    public string           DummyTime  = "16:00:00";
    public string           FieldDelim = "";
    public string           TxtPath    = "";
    public int              FieldWidth;
    public ColumnInfoList   ColInfoList;
    public CallbackHandler  cbh;
    public NumberFormatInfo NFI;

    public TextFileTranslateParams(string txtPath, bool useHeaders, bool useQuotes, string extension,
                                   string dateFormat, string fieldDelim, int fieldWidth, NumberFormatInfo nfi,
                                   ColumnInfoList ciList,
                                   string culture, CallbackHandler cbh, string dummyTime)
    {
        TxtPath     = txtPath;
        UseHeaders  = useHeaders;
        UseQuotes   = useQuotes;
        Extension   = extension;
        DateFormat  = dateFormat;
        FieldDelim  = fieldDelim;
        FieldWidth  = fieldWidth;
        DummyTime   = dummyTime;
        NFI         = nfi;
        ColInfoList = ciList;
        this.cbh    = cbh;
    }
}
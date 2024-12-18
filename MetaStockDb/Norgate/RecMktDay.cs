// Decompiled with JetBrains decompiler
// Type: Nis.Utils.RecMktDay
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using Norgate.Utils;

namespace Nis.Utils;

public class RecMktDay : MsBaseProcs
{
    public RecMsDay msd;
    public RecDay   dtd;

    public void MakeDtd()
    {
        dtd.d = CalFunc.Ymd2jd((int)ms2ieee(msd.d));
        dtd.o = ms2ieee(msd.o);
        dtd.h = ms2ieee(msd.h);
        dtd.l = ms2ieee(msd.l);
        dtd.c = ms2ieee(msd.c);
        dtd.v = ms2ieee(msd.v);
        dtd.i = ms2ieee(msd.i);
    }

    public void MakeDtdJd()
    {
        dtd.d = CalFunc.Ymd2jd((int)ms2ieee(msd.d));
    }

    public RecMktDay(RecDay day)
    {
        dtd = new RecDay(day.d, day.o, day.h, day.l, day.c, day.v, day.i);
        msd = new RecMsDay();
    }

    public RecMktDay()
    {
        dtd = new RecDay();
        msd = new RecMsDay();
    }

    public void makeMsd()
    {
        msd.d = ieee2ms((float)CalFunc.Jd2MsDate(dtd.d));
        msd.o = ieee2ms(dtd.o);
        msd.h = ieee2ms(dtd.h);
        msd.l = ieee2ms(dtd.l);
        msd.c = ieee2ms(dtd.c);
        msd.v = ieee2ms(dtd.v);
        msd.i = ieee2ms(dtd.i);
    }
}
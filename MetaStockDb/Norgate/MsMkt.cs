using Norgate.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Nis.Utils
{
    public class MsMkt : MsBaseProcs
    {
        private string _datFilePath = "";
        private byte[] _msDayBuff = new byte[28];
        private EnumPeriodicity _periodicity;
        private FileStream _fs;
        private MsMktList _dlyList;
        private MsMktList _result;
        private DirEntry _entry;
        private MsDir _msd;
        private RecMsDay _baseRec;
        private bool _loaded;

        public MsMkt(int periodicityIndex, string dataFilePath, string ticker)
        {
            _periodicity = (EnumPeriodicity)periodicityIndex;
            _datFilePath = dataFilePath;
            clearROAttribute(_datFilePath);
            _baseRec = new RecMsDay();
            Ticker = ticker;
        }

        public int Count => _result.Count;
        public string Ticker { get; }
        public RecMktDay CurrentDay => _result.Get;

        public List<RecDay> PriceList
        {
            get
            {
                List<RecDay> recDayList = new List<RecDay>();
                if (!_loaded)
                    Load();
                if (Reset())
                {
                    do
                    {
                        recDayList.Add(CurrentDay.dtd);
                    } while (Next());
                }

                return recDayList;
            }
        }

        public bool EOD { get; set; }

        public void Clear()
        {
            _result.Clear();
        }

        public bool Reset()
        {
            return _result.Reset();
        }

        public bool Next()
        {
            return _result.Next();
        }

        public void Load()
        {
            Load(new int?(), new int?());
        }

        public void Load(DateTime from)
        {
            Load(new int?((int)from.ToOADate()), new int?());
        }

        public void Load(DateTime from, DateTime to)
        {
            Load(new int?((int)from.ToOADate()), new int?((int)to.ToOADate()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fjd">OLE date from</param>
        /// <param name="ljd">OLE date to</param>
        public void Load(int? fjd, int? ljd)
        {
            LoadDailyData(fjd, ljd);
            switch (_periodicity)
            {
                case EnumPeriodicity.daily:
                    _result = _dlyList;
                    break;
                case EnumPeriodicity.weekly:
                    ProcessTableWeekly();
                    break;
                case EnumPeriodicity.monthly:
                    ProcessTableMonthly();
                    break;
            }
        }

        private void LoadDailyData(int? fjd, int? ljd)
        {
            _dlyList = new MsMktList();
            if (File.Exists(_datFilePath))
            {
                bool flag = false;
                if (!fjd.HasValue && !ljd.HasValue)
                {
                    flag = true;
                }
                else
                {
                    if (!fjd.HasValue)
                        fjd = new int?(-99999);
                    if (!ljd.HasValue)
                        ljd = new int?(99999);
                }

                _fs = new FileStream(_datFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                if (_fs.Length > 0L)
                {
                    _baseRec = new RecMsDay();
                    int num = (int)(_fs.Length / 28L) - 1;
                    ReadFromFileStream(_fs, ref _baseRec);
                    for (int index = 0; index < num; ++index)
                    {
                        RecMktDay day = new RecMktDay();
                        ReadFromFileStream(_fs, ref day.msd);
                        day.MakeDtd();
                        if (!flag)
                        {
                            int d1 = day.dtd.d;
                            int? nullable = fjd;
                            int valueOrDefault1 = nullable.GetValueOrDefault();
                            if ((d1 >= valueOrDefault1 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                            {
                                int d2 = day.dtd.d;
                                nullable = ljd;
                                int valueOrDefault2 = nullable.GetValueOrDefault();
                                if ((d2 <= valueOrDefault2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                                    continue;
                            }
                            else
                                continue;
                        }

                        _dlyList.Add(ref day);
                    }
                }

                _fs.Close();
                _dlyList.Reset();
            }

            _loaded = true;
        }

        private void ProcessTableWeekly()
        {
            RecMktDay day = (RecMktDay)null;
            _result = new MsMktList();
            int num1 = -1;
            double num2 = 0.0;
            if (_dlyList.Reset())
            {
                do
                {
                    RecMktDay get = _dlyList.Get;
                    RecDay dtd = get.dtd;
                    DateTime dateTime = DateTime.FromOADate((double)get.dtd.d);
                    if (day == null)
                    {
                        day = new RecMktDay(dtd);
                        day.dtd.v = 0.0f;
                        day.dtd.i = 0.0f;
                        num1 = -1;
                        num2 = (double)get.dtd.d;
                    }

                    if (dateTime.DayOfWeek <= (DayOfWeek)num1 || (double)get.dtd.d - num2 > 6.0)
                    {
                        _result.Add(ref day);
                        day = new RecMktDay(dtd);
                        day.dtd.v = 0.0f;
                        day.dtd.i = 0.0f;
                    }

                    if ((double)dtd.h > (double)day.dtd.h)
                        day.dtd.h = dtd.h;
                    if ((double)dtd.l < (double)day.dtd.l)
                        day.dtd.l = dtd.l;
                    day.dtd.c = dtd.c;
                    day.dtd.d = dtd.d;
                    day.dtd.v += dtd.v;
                    day.dtd.i = dtd.i;
                    num1 = (int)dateTime.DayOfWeek;
                    num2 = (double)dtd.d;
                } while (_dlyList.Next());
            }

            if (day == null)
                return;
            _result.Add(ref day);
        }

        private void ProcessTableMonthly()
        {
            RecMktDay day = (RecMktDay)null;
            int num1 = 0;
            int num2 = 0;
            DateTime minValue = DateTime.MinValue;
            _result = new MsMktList();
            if (!_dlyList.Reset())
                return;
            do
            {
                RecMktDay get = _dlyList.Get;
                RecDay dtd = get.dtd;
                DateTime dateTime = DateTime.FromOADate((double)get.dtd.d);
                DateTime date1;
                if (day == null)
                {
                    day = new RecMktDay(dtd);
                    day.dtd.v = 0.0f;
                    day.dtd.i = 0.0f;
                    date1 = dateTime.Date;
                    num1 = date1.Month;
                    date1 = dateTime.Date;
                    num2 = date1.Year;
                }

                date1 = dateTime.Date;
                if (date1.Month == num1)
                {
                    date1 = dateTime.Date;
                    if (date1.Year == num2)
                        goto label_6;
                }

                date1 = dateTime.Date;
                num1 = date1.Month;
                date1 = dateTime.Date;
                num2 = date1.Year;
                _result.Add(ref day);
                day = new RecMktDay(dtd);
                day.dtd.v = 0.0f;
                day.dtd.i = 0.0f;
            label_6:
                if ((double)dtd.h > (double)day.dtd.h)
                    day.dtd.h = dtd.h;
                if ((double)dtd.l < (double)day.dtd.l)
                    day.dtd.l = dtd.l;
                day.dtd.c = dtd.c;
                day.dtd.d = dtd.d;
                day.dtd.v += dtd.v;
                day.dtd.i = dtd.i;
                DateTime date2 = dateTime.Date;
            } while (_dlyList.Next());

            if (day == null)
                return;
            _result.Add(ref day);
        }

        public void Save()
        {
            if (_periodicity != EnumPeriodicity.daily)
                return;
            clearROAttribute(_datFilePath);
            _fs = new FileStream(_datFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            WriteToFileStream(_fs, new RecMsDayHeader()
            {
                Count = Convert.ToInt16(_dlyList.Count + 1)
            });
            BinaryWriter binaryWriter = new BinaryWriter((Stream)_fs);
            binaryWriter.Seek(8, SeekOrigin.Begin);
            binaryWriter.Write(Count + 1);
            binaryWriter.Seek(0, SeekOrigin.End);
            if (_dlyList.Reset())
            {
                _entry.Fjd = _dlyList.Get.dtd.d;
                do
                {
                    WriteToFileStream(_fs, _dlyList.Get.msd);
                } while (_dlyList.Next());

                _entry.Ljd = _dlyList.Get.dtd.d;
            }

            _fs.Close();
            if (_msd == null)
                return;
            _msd.Amended = true;
        }

        private string FormatPrice(double n)
        {
            return Math.Round(n, _entry.dp).ToString();
        }

        private string FormatVoi(double n)
        {
            return ((int)Math.Round(n, 0)).ToString();
        }

        public void Translate2Txt(TextFileTranslateParams myParams)
        {
            Translate2Txt(myParams, false);
        }

        public string Translate2Txt(TextFileTranslateParams myParams, bool reverseDateOrder)
        {
            return new TextFile(myParams).Translate(this, reverseDateOrder);
        }

        public void SaveToTxtFile(string fn)
        {
            StreamWriter streamWriter = new StreamWriter(fn);
            if (_result.Reset())
            {
                do
                {
                    RecDay dtd = _result.Get.dtd;
                    string str =
                        $"{(object)CalFunc.Jd2cymds(dtd.d)},{(object)FormatPrice((double)dtd.o)},{(object)FormatPrice((double)dtd.h)},{(object)FormatPrice((double)dtd.l)},{(object)FormatPrice((double)dtd.c)},{(object)FormatVoi((double)dtd.v)},{(object)FormatVoi((double)dtd.i)}";
                    streamWriter.WriteLine(str);
                } while (_result.Next());
            }

            streamWriter.Close();
        }

        public void BulkLoad(MsMktList list)
        {
            _dlyList = list;
            _result = list;
        }

        public void Add(RecMktDay dtdDay)
        {
            _dlyList.Add(ref dtdDay);
        }

        private void ReadFromFileStream(FileStream fs, ref RecMsDay rec)
        {
            int offset = 0;
            while (offset < _msDayBuff.Length)
                offset += fs.Read(_msDayBuff, offset, _msDayBuff.Length - offset);
            GCHandle gcHandle = GCHandle.Alloc((object)_msDayBuff, GCHandleType.Pinned);
            rec = (RecMsDay)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
                typeof(RecMsDay));
            gcHandle.Free();
        }

        private void WriteToFileStream(FileStream fs, RecMsDay rec)
        {
            GCHandle gcHandle = GCHandle.Alloc((object)_msDayBuff, GCHandleType.Pinned);
            Marshal.StructureToPtr((object)rec, gcHandle.AddrOfPinnedObject(), true);
            fs.Write(_msDayBuff, 0, 28);
            gcHandle.Free();
        }

        private void WriteToFileStream(FileStream fs, RecMsDayHeader rec)
        {
            GCHandle gcHandle = GCHandle.Alloc((object)_msDayBuff, GCHandleType.Pinned);
            Marshal.StructureToPtr((object)rec, gcHandle.AddrOfPinnedObject(), true);
            fs.Write(_msDayBuff, 0, 28);
            gcHandle.Free();
        }
    }
}

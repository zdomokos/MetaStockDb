// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsMktList
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System.Collections.Generic;

namespace Nis.Utils
{
	public class MsMktList
	{
		private SortedList<int, RecMktDay> _days = new SortedList<int, RecMktDay>();
		private int _currentIndex = -1;

		public int Count => _days.Count;
		public int FirstJD => _days.Keys[0];
		public int LastJD => _days.Keys[_days.Count - 1];
		public RecMktDay Get => _days.Values[_currentIndex];

		public void Add(ref RecMktDay day)
		{
			if (_days.ContainsKey(day.dtd.d))
				_days[day.dtd.d] = day;
			else
				_days.Add(day.dtd.d, day);
		}

		public RecMktDay Add(int jd)
		{
			RecMktDay recMktDay = new RecMktDay();
			_days.Add(jd, recMktDay);
			return recMktDay;
		}

		public void Append(ref RecMktDay day)
		{
			_days.Add(day.dtd.d, day);
		}

		public void Clear()
		{
			_days.Clear();
		}

		public bool Contains(int jd)
		{
			if (_days.ContainsKey(jd))
			{
				_currentIndex = _days.Keys.IndexOf(jd);
				return true;
			}

			_currentIndex = -1;
			return false;
		}

		public bool Remove(int jd)
		{
			if (_days[jd] == null)
				return false;
			_days.Remove(jd);
			return true;
		}

		public bool Reset()
		{
			_currentIndex = -1;
			return Next();
		}

		public bool Next()
		{
			if (_currentIndex >= _days.Count - 1)
				return false;
			++_currentIndex;
			return true;
		}
	}
}

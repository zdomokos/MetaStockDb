// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsNewDirList
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

namespace Nis.Utils
{
    public class MsNewDirList
    {
        private List<DirEntry>          sex          = new List<DirEntry>();
        private SortedList<string, int> symbols      = new SortedList<string, int>();
        private SortedList<int, int>    aids         = new SortedList<int, int>();
        private SortedList<int, int>    fileNrs      = new SortedList<int, int>();
        private int                     currentIndex = -1;
        private int                     maxEntries   = 6000;

        public void Add(ref DirEntry e)
        {
            currentIndex = -1;
            if (e.FileNo > (ushort)0 && fileNrs.ContainsKey((int)e.FileNo))
                currentIndex = fileNrs.IndexOfKey((int)e.FileNo);
            else if (e.AssetID > 0 && aids.ContainsKey(e.AssetID))
                currentIndex = aids.IndexOfKey(e.AssetID);
            else if (e.Sym01.Length > 0 && symbols.ContainsKey(e.Sym01))
                currentIndex = symbols.IndexOfKey(e.Sym01);
            if (currentIndex >= 0)
            {
                sex[currentIndex].Sym01   = e.Sym01;
                sex[currentIndex].MsName  = e.MsName;
                sex[currentIndex].AssetID = e.AssetID;
            }
            else
            {
                if (e.FileNo < (ushort)1)
                {
                    for (ushort index = 1; (int)index < maxEntries; ++index)
                    {
                        if (!fileNrs.ContainsKey((int)index))
                        {
                            e.FileNo = index;
                            break;
                        }
                    }
                }

                sex.Add(e);
                currentIndex = sex.Count - 1;
                symbols.Add(e.Sym01, currentIndex);
                fileNrs.Add((int)e.FileNo, currentIndex);
                if (e.AssetID <= 0)
                    return;
                aids.Add(e.AssetID, currentIndex);
            }
        }

        public int Count => sex.Count;

        public void DeleteCurrentEntry()
        {
            fileNrs.Remove((int)sex[currentIndex].FileNo);
            sex.RemoveAt(currentIndex);
        }

        public void Clear()
        {
            sex.Clear();
            symbols.Clear();
            fileNrs.Clear();
            aids.Clear();
        }

        public bool ContainsByAssetID(int assetid)
        {
            currentIndex = aids.IndexOfKey(assetid);
            return currentIndex >= 0;
        }

        public bool ContainsBySymbol(string symbol)
        {
            currentIndex = symbols[symbol];
            return currentIndex >= 0;
        }

        public bool ContainsByFnr(int fnr)
        {
            currentIndex = -1;
            if (fileNrs.ContainsKey(fnr))
                currentIndex = fileNrs[fnr];
            return currentIndex >= 0;
        }

        public DirEntry Get => sex[currentIndex];

        public void Reset()
        {
            currentIndex = -1;
        }

        public bool Next()
        {
            if (currentIndex >= sex.Count - 1)
                return false;
            ++currentIndex;
            return true;
        }

        public void FillLower255(string folderPath)
        {
            for (ushort index1 = 1; index1 < (ushort)256; ++index1)
            {
                if ((int)index1 < fileNrs.Count && !fileNrs.ContainsKey((int)index1))
                {
                    int key    = fileNrs.Keys[fileNrs.Count - 1];
                    int index2 = fileNrs.IndexOfKey(key);
                    sex[index2].FileNo = index1;
                    string   fileName     = $"{(object)folderPath}F{(object)key}.mwd";
                    string   destFileName = $"{(object)folderPath}F{(object)index1}.dat";
                    FileInfo fileInfo     = new FileInfo(fileName);
                    if (fileInfo.Exists)
                        fileInfo.MoveTo(destFileName);
                    fileNrs.Remove(key);
                    fileNrs.Add((int)index1, index2);
                }
            }
        }

        public int FJD
        {
            get => sex[currentIndex].Fjd;
            set => sex[currentIndex].Fjd = value;
        }

        public int LJD
        {
            get => sex[currentIndex].Ljd;
            set => sex[currentIndex].Ljd = value;
        }

        public DirEntry CurrentEntry
        {
            get => sex[currentIndex];
            set => sex[currentIndex] = value;
        }

        public List<string> TickerList
        {
            get
            {
                List<string> stringList = new List<string>();
                foreach (string key in (IEnumerable<string>)symbols.Keys)
                    stringList.Add(key);
                return stringList;
            }
        }
    }
}
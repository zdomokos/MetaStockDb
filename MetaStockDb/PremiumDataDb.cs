#region ZGT

// (c) Copyright 2020 ZTG LLC. All Rights Reserved.
// NOTICE: This file contains source code, ideas, techniques, and information (the Information) which are
// Proprietary and Confidential Information of ZTG LLC. This Information may not be used by or disclosed to any
// third party except under written license, and shall be subject to the limitations prescribed under license.

#endregion

namespace MetaStockDb;

public class PremiumDataDb
{
    public PremiumDataDb(string dbRootPath)
    {
        _dbRootPath  = dbRootPath;
        _symbolTable = new Dictionary<string, StockDataHeader>();
    }

    public int                                 Count       => _symbolTable.Count;
    public Dictionary<string, StockDataHeader> SymbolTable => _symbolTable;


    public void LoadSymbolTable()
    {
        // walk through all directories and store content of master files in memory
        var dirs = Directory.EnumerateDirectories(_dbRootPath, "*.*", SearchOption.AllDirectories);
        foreach (string dir in dirs)
        {
            if (!IsMsFolder(dir)) continue;

            string path = dir.Replace(_dbRootPath, "");

            // read master files
            string emaster = Path.Combine(dir, "emaster");
            string master  = Path.Combine(dir, "master");
            string xmaster = Path.Combine(dir, "xmaster");
            if (File.Exists(emaster))
            {
                try
                {
                    var masterFile = new EMasterFile();
                    masterFile.Load(emaster);
                    foreach (var hdr in masterFile.Records.Select(StockDataHeader.FromEMaster))
                        LoadToDb(hdr, path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e);
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (File.Exists(Path.Combine(dir, "master")))
            {
                try
                {
                    var masterFile = new MasterFile();
                    masterFile.Load(master);
                    foreach (var hdr in masterFile.Records.Select(StockDataHeader.FromMaster))
                        LoadToDb(hdr, path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.ToString());
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (File.Exists(Path.Combine(dir, "xmaster")))
            {
                try
                {
                    var masterFile = new XMasterFile();
                    masterFile.Load(xmaster);
                    foreach (var hdr in masterFile.Records.Select(StockDataHeader.FromXMaster))
                        LoadToDb(hdr, path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.ToString());
                    //MessageBox.Show(e.Message, "Error reading EMASTER file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public StockDataHeader GetHeader(string symbol)
    {
        return _symbolTable.GetValueOrDefault(symbol);
    }

    public PriceDateFile LoadBars(string symbol)
    {
        if (!_symbolTable.TryGetValue(symbol, out var hdr)) return null;

        var stock = new PriceDateFile(_dbRootPath, hdr);
        stock.Load();
        return stock;
    }

    private bool IsMsFolder(string dir)
    {
        return File.Exists($@"{dir}\master");
    }

    private void LoadToDb(StockDataHeader hdr, string path)
    {
        hdr.Classifier = path;
        if (!_symbolTable.ContainsKey(hdr.Symbol))
            _symbolTable.Add(hdr.Symbol, hdr);
        else
            _symbolTable.Add($"{hdr.Symbol}-{path}", hdr);
    }

    private readonly Dictionary<string, StockDataHeader> _symbolTable;
    private readonly string                              _dbRootPath;
}

public class StockDataHeader
{
    public enum TimeFrame
    {
        ID,
        D1,
        W1,
        M1
    }

    public TimeFrame Frequency { get; set; }

    public string Symbol     { get; set; }
    public string Name       { get; set; }
    public string Classifier { get; set; }
    public int    FileNumber { get; set; }

    public DateTime FirstDate { get; set; }
    public DateTime LastDate  { get; set; }

    public static StockDataHeader FromEMaster(EMasterRec rec)
    {
        var r = new StockDataHeader();
        switch (rec.TimeFrame)
        {
            case (byte)'I':
                r.Frequency = TimeFrame.ID;
                break;
            case (byte)'D':
                r.Frequency = TimeFrame.D1;
                break;
            case (byte)'W':
                r.Frequency = TimeFrame.W1;
                break;
            case (byte)'M':
                r.Frequency = TimeFrame.M1;
                break;
        }

        r.Symbol     = rec.Symbol;
        r.Name       = string.IsNullOrEmpty(rec.ExtName) ? rec.Name : rec.ExtName;
        r.FileNumber = rec.FileNumber;
        r.FirstDate  = MsFileIO.ConvertDateTime(rec.FirstDate);
        r.LastDate   = MsFileIO.ConvertDateTime(rec.LastDate);
        return r;
    }

    public static StockDataHeader FromMaster(MasterRec rec)
    {
        var r = new StockDataHeader();
        switch (rec.TimeInterval)
        {
            case (byte)'I':
                r.Frequency = TimeFrame.ID;
                break;
            case (byte)'D':
                r.Frequency = TimeFrame.D1;
                break;
            case (byte)'W':
                r.Frequency = TimeFrame.W1;
                break;
            case (byte)'M':
                r.Frequency = TimeFrame.M1;
                break;
        }

        r.Symbol     = rec.Symbol;
        r.Name       = rec.Name;
        r.FileNumber = rec.FileNumber;
        r.FirstDate  = MsFileIO.ConvertDateTime(rec.FirstDate);
        r.LastDate   = MsFileIO.ConvertDateTime(rec.LastDate);
        return r;
    }

    public static StockDataHeader FromXMaster(XMasterRec rec)
    {
        var r = new StockDataHeader();
        switch (rec.TimeFrame)
        {
            case (byte)'I':
                r.Frequency = TimeFrame.ID;
                break;
            case (byte)'D':
                r.Frequency = TimeFrame.D1;
                break;
            case (byte)'W':
                r.Frequency = TimeFrame.W1;
                break;
            case (byte)'M':
                r.Frequency = TimeFrame.M1;
                break;
        }

        r.Symbol     = rec.Symbol;
        r.Name       = rec.Name;
        r.FileNumber = rec.FileNumber;
        r.FirstDate  = MsFileIO.ConvertDateTime(rec.FirstDate1);
        r.LastDate   = MsFileIO.ConvertDateTime(rec.LastDate);
        return r;
    }
}
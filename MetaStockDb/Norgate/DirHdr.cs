// Decompiled with JetBrains decompiler
// Type: Nis.Utils.DirHdr
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System.Runtime.InteropServices;

namespace Nis.Utils
{
    public class DirHdr : MsBaseProcs
    {
        internal RecMasterHdr  masterHeader;
        internal RecEmasterHdr emasterHeader;
        internal RecXmasterHdr xmasterHeader;
        private  MsGlobals     metastockGlobals;

        public DirHdr(MsGlobals msglobal)
        {
            metastockGlobals = msglobal;
            masterHeader     = new RecMasterHdr();
            emasterHeader    = new RecEmasterHdr();
            xmasterHeader    = new RecXmasterHdr();
            if (File.Exists(metastockGlobals.Fs1Pth) && metastockGlobals.Fs1 != null)
                readFromFileStream(ref masterHeader);
            if (File.Exists(metastockGlobals.Fs2Pth) && metastockGlobals.Fs2 != null)
                readFromFileStream(ref emasterHeader);
            if (!File.Exists(metastockGlobals.Fs3Pth) || metastockGlobals.Fs3 == null)
                return;
            readFromFileStream(ref xmasterHeader);
        }

        public void Save(int count)
        {
            masterHeader.Count1  = (ushort)Math.Min(count, (int)byte.MaxValue);
            masterHeader.Count2  = masterHeader.Count1;
            emasterHeader.Count1 = masterHeader.Count1;
            emasterHeader.Count2 = masterHeader.Count1;
            writeToFileStream(masterHeader);
            writeToFileStream(emasterHeader);
            if (count <= (int)byte.MaxValue)
                return;
            xmasterHeader.Count1  = count - (int)byte.MaxValue;
            xmasterHeader.Count2  = count - (int)byte.MaxValue;
            xmasterHeader.NextFno = count + 1;
            writeToFileStream(xmasterHeader);
        }

        private void readFromFileStream(ref RecMasterHdr rec)
        {
            byte[] buffer = new byte[RecMasterSize];
            int    offset = 0;
            while (offset < buffer.Length)
                offset += metastockGlobals.Fs1.Read(buffer, offset, buffer.Length - offset);
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            rec = (RecMasterHdr)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
                                                       typeof(RecMasterHdr));
            gcHandle.Free();
        }

        private void readFromFileStream(ref RecEmasterHdr rec)
        {
            byte[] buffer = new byte[RecEmasterSize];
            int    offset = 0;
            while (offset < buffer.Length)
                offset += metastockGlobals.Fs2.Read(buffer, offset, buffer.Length - offset);
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            rec = (RecEmasterHdr)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
                                                        typeof(RecEmasterHdr));
            gcHandle.Free();
        }

        private void readFromFileStream(ref RecXmasterHdr rec)
        {
            byte[] buffer = new byte[RecXmasterSize];
            int    offset = 0;
            while (offset < buffer.Length)
                offset += metastockGlobals.Fs3.Read(buffer, offset, buffer.Length - offset);
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            rec = (RecXmasterHdr)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(),
                                                        typeof(RecXmasterHdr));
            gcHandle.Free();
        }

        private void writeToFileStream(RecMasterHdr rec)
        {
            byte[]   buffer   = new byte[RecMasterSize];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(rec, gcHandle.AddrOfPinnedObject(), true);
            metastockGlobals.Fs1.Write(buffer, 0, RecMasterSize);
            gcHandle.Free();
        }

        private void writeToFileStream(RecEmasterHdr rec)
        {
            byte[]   buffer   = new byte[RecEmasterSize];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(rec, gcHandle.AddrOfPinnedObject(), true);
            metastockGlobals.Fs2.Write(buffer, 0, RecEmasterSize);
            gcHandle.Free();
        }

        private void writeToFileStream(RecXmasterHdr rec)
        {
            byte[]   buffer   = new byte[RecXmasterSize];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(rec, gcHandle.AddrOfPinnedObject(), true);
            metastockGlobals.Fs3.Write(buffer, 0, RecXmasterSize);
            gcHandle.Free();
        }
    }
}
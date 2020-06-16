// Decompiled with JetBrains decompiler
// Type: Nis.Utils.MsGlobals
// Assembly: Premium Data Converter, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 363859BC-FAE9-47BF-B5F0-C936F45E98DD
// Assembly location: C:\Program Files (x86)\Premium Data Converter\Premium Data Converter.exe

using System.IO;

namespace Nis.Utils
{
	public class MsGlobals
	{
		private string FolderPath;
		public string Fs1Pth { get; }
		public string Fs2Pth { get; }
		public string Fs3Pth { get; }
		public FileStream Fs1;
		public FileStream Fs2;
		public FileStream Fs3;
		public FileStream FsMkt;

		public MsGlobals(string folder)
		{
			FolderPath = folder;
			Fs1Pth = folder + "master";
			Fs2Pth = folder + "emaster";
			Fs3Pth = folder + "xmaster";
		}
	}
}
// Decompiled with JetBrains decompiler
// Type: Norgate.Utils.Volid
// Assembly: norgate.general.4, Version=4.0.2.1, Culture=neutral, PublicKeyToken=d5eccc6216068f48
// MVID: A122D709-041A-426F-9B6C-C1457ADC069B
// Assembly location: C:\Program Files (x86)\Premium Data Converter\norgate.general.4.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Norgate.Utils
{
	public class Volid
	{
		private static readonly string[] SizeSuffixes = new string[9]
		{
			"bytes",
			"KB",
			"MB",
			"GB",
			"TB",
			"PB",
			"EB",
			"ZB",
			"YB"
		};

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GlobalMemoryStatusEx([In, Out] Volid.MEMORYSTATUSEX lpBuffer);

		[DllImport("kernel32.dll")]
		private static extern uint GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer,
			uint VolumeNameSize, ref uint VolumeSerialNumber, ref uint MaximumComponentLength, ref uint FileSystemFlags,
			StringBuilder FileSystemNameBuffer, uint FileSystemNameSize);

		public static string WinSysFolder => NgUtils.AppendTrailingChar(Environment.SystemDirectory, '\\');

		public static uint GetVolID
		{
			get
			{
				uint num1 = 0;
				uint num2 = 0;
				StringBuilder stringBuilder1 = new StringBuilder(256);
				uint num3 = 0;
				StringBuilder stringBuilder2 = new StringBuilder(256);
				string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
				StringBuilder VolumeNameBuffer = stringBuilder1;
				int capacity1 = VolumeNameBuffer.Capacity;
				ref uint local1 = ref num1;
				ref uint local2 = ref num2;
				ref uint local3 = ref num3;
				StringBuilder FileSystemNameBuffer = stringBuilder2;
				int capacity2 = FileSystemNameBuffer.Capacity;
				int volumeInformation = (int)GetVolumeInformation(pathRoot, VolumeNameBuffer, (uint) capacity1,
					ref local1, ref local2, ref local3, FileSystemNameBuffer, (uint) capacity2);
				return num1;
			}
		}

		public static string ComputerName => Environment.MachineName;

		private static string SizeSuffix(ulong value)
		{
			if (value == 0UL)
				return "0.0 bytes";
			int index = (int) Math.Log((double) value, 1024.0);
			return
				$"{(object) ((Decimal) value / (Decimal) (1L << index * 10)):n1} {(object)SizeSuffixes[index]}";
		}

		public static string GetTotalRam()
		{
			try
			{
				Volid.MEMORYSTATUSEX lpBuffer = new Volid.MEMORYSTATUSEX();
                GlobalMemoryStatusEx(lpBuffer);
				return SizeSuffix(lpBuffer.ullTotalPhys);
			}
			catch
			{
				return "Unknown";
			}
		}

		public static string GetFreeRam()
		{
			try
			{
				Volid.MEMORYSTATUSEX lpBuffer = new Volid.MEMORYSTATUSEX();
                GlobalMemoryStatusEx(lpBuffer);
				return SizeSuffix(lpBuffer.ullAvailPhys);
			}
			catch
			{
				return "Unknown";
			}
		}

		public static string VerStrLong(string fn)
		{
			try
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fn);
				return
					$"{(object) versionInfo.FileMajorPart}.{(object) versionInfo.FileMinorPart}.{(object) versionInfo.FileBuildPart}.{(object) versionInfo.FilePrivatePart}";
			}
			catch
			{
				return "Not Found";
			}
		}

		public static string VerStrShort(string fn)
		{
			try
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fn);
				return
					$"{(object) versionInfo.FileMajorPart}.{(object) versionInfo.FileMinorPart}.{(object) versionInfo.FileBuildPart}";
			}
			catch
			{
				return "Not Found";
			}
		}

		public static string GetInternalName(string fn)
		{
			try
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fn);
				return
					$"{(object) versionInfo.FileMajorPart}.{(object) versionInfo.FileMinorPart}.{(object) versionInfo.FileBuildPart}.{(object) versionInfo.FilePrivatePart}";
			}
			catch
			{
				return "Not Found";
			}
		}

		public static string WinTempFolder
		{
			get
			{
				try
				{
					return NgUtils.AppendTrailingChar(Path.GetTempPath(), '\\');
				}
				catch
				{
					return "";
				}
			}
		}

		public static string getOSversion => Environment.OSVersion.ToString();

		public static string getLocalIP
		{
			get
			{
				string hostName = Dns.GetHostName();
				IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
				string str = hostName + "   ";
				int num = 0;
				foreach (IPAddress address in hostEntry.AddressList)
					str += $"IP#{(object) ++num}:{(object) address.ToString()} ";
				return str;
			}
		}

		public static string QuickLaunchFolder
		{
			get
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if (folderPath != null && folderPath != "")
					return folderPath + "\\Microsoft\\Internet Explorer\\Quick Launch\\";
				return (string) null;
			}
		}

		//public static string MACAddress
		//{
		//  get
		//  {
		//    ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
		//    string empty = string.Empty;
		//    foreach (ManagementObject managementObject in instances)
		//    {
		//      if (empty == string.Empty && (bool) managementObject["IPEnabled"])
		//        empty = managementObject["MacAddress"].ToString();
		//      managementObject.Dispose();
		//    }
		//    return empty.Replace(":", "");
		//  }
		//}

		//public static string CpuID
		//{
		//  get
		//  {
		//    string empty = string.Empty;
		//    try
		//    {
		//      foreach (ManagementObject instance in new ManagementClass("Win32_Processor").GetInstances())
		//      {
		//        if (empty == string.Empty)
		//          empty = instance.Properties["ProcessorId"].Value.ToString();
		//      }
		//    }
		//    catch
		//    {
		//    }
		//    return empty;
		//  }
		//}

		//public static string CpuInfo
		//{
		//  get
		//  {
		//    string str1 = (string) null;
		//    string key = "Hardware\\Description\\System\\CentralProcessor\\0";
		//    ModifyRegistry modifyRegistry = new ModifyRegistry();
		//    modifyRegistry.SetRoot(2);
		//    if (modifyRegistry.KeyExists(key))
		//    {
		//      modifyRegistry.SubKey = key;
		//      string str2 = string.Format("{0}: {1} {2} ", (object) (string) modifyRegistry.Read("VendorIdentifier"), (object) ((string) modifyRegistry.Read("ProcessorNameString")).Trim(), (object) (string) modifyRegistry.Read("Identifier"));
		//      string str3 = "";
		//      try
		//      {
		//        if (modifyRegistry.Read("~MHZ").GetType() == Type.GetType("System.Int32"))
		//          str3 = Convert.ToString((int) modifyRegistry.Read("~MHZ"));
		//        else if (modifyRegistry.Read("~MHZ").GetType() == Type.GetType("System.String"))
		//          str3 = (string) modifyRegistry.Read("~MHZ");
		//      }
		//      catch
		//      {
		//        str3 = "Exception";
		//      }
		//      str1 = string.Format("{0} ({1} MHZ)", (object) str2, (object) str3);
		//    }
		//    return str1;
		//  }
		//}

		//public static string ScreenInfo
		//{
		//  get
		//  {
		//    string str1 = (string) null;
		//    foreach (Screen allScreen in Screen.AllScreens)
		//    {
		//      string str2 = str1;
		//      string format = "{0} X {1}  ";
		//      Rectangle bounds = allScreen.Bounds;
		//      __Boxed<int> width = (ValueType) bounds.Width;
		//      bounds = allScreen.Bounds;
		//      __Boxed<int> height = (ValueType) bounds.Height;
		//      string str3 = string.Format(format, (object) width, (object) height);
		//      str1 = str2 + str3;
		//    }
		//    return str1;
		//  }
		//}

		public static bool IsNT => Environment.OSVersion.Platform == PlatformID.Win32NT;

		//public static void DiskSize(string path, out string drv, out double size, out double free)
		//{
		//  StringBuilder stringBuilder = new StringBuilder(Path.GetPathRoot(path));
		//  if (stringBuilder.Length == 3)
		//    stringBuilder.Length = 2;
		//  drv = stringBuilder.ToString().ToUpper();
		//  ManagementObject managementObject = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}\"", (object) stringBuilder.ToString()));
		//  managementObject.Get();
		//  ulong uint64_1 = Convert.ToUInt64(managementObject["Size"]);
		//  size = (double) (uint64_1 / 1024UL / 1024UL);
		//  ulong uint64_2 = Convert.ToUInt64(managementObject["FreeSpace"]);
		//  free = (double) (uint64_2 / 1024UL / 1024UL);
		//}

		//public static string DiskDetails(string path)
		//{
		//  string drv = (string) null;
		//  double size = 0.0;
		//  double free = 0.0;
		//  Volid.DiskSize(path, out drv, out size, out free);
		//  return string.Format("{0} {1,7:###,##0}  {2,7:###,##0}", (object) drv, (object) size, (object) free);
		//}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MEMORYSTATUSEX
		{
			public uint dwLength;
			public uint dwMemoryLoad;
			public ulong ullTotalPhys;
			public ulong ullAvailPhys;
			public ulong ullTotalPageFile;
			public ulong ullAvailPageFile;
			public ulong ullTotalVirtual;
			public ulong ullAvailVirtual;
			public ulong ullAvailExtendedVirtual;

			public MEMORYSTATUSEX()
			{
				this.dwLength = (uint) Marshal.SizeOf(typeof(Volid.MEMORYSTATUSEX));
			}
		}
	}
}

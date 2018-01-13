using System;
using System.IO;

namespace RocketJump.Modification
{
	// Token: 0x020000B0 RID: 176
	public class Log
	{
		// Token: 0x0600051D RID: 1309 RVA: 0x00031684 File Offset: 0x0002F884
		static Log()
		{
			try
			{
				Log.fs = File.Open(Log.logFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".log", FileMode.Create);
				Log.sw = new StreamWriter(Log.fs);
				Log.sw.AutoFlush = true;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000054D5 File Offset: 0x000036D5
		public static void WriteLine(string s)
		{
			if (Log.sw != null)
			{
				Log.sw.WriteLine(s);
			}
		}

		// Token: 0x04000653 RID: 1619
		private static string logFile = "dg_mod";

		// Token: 0x04000654 RID: 1620
		private static StreamWriter sw;

		// Token: 0x04000655 RID: 1621
		private static FileStream fs;
	}
}

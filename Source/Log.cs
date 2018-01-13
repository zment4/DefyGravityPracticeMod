using System;
using System.IO;

namespace RocketJump.Modification
{
	// Token: 0x020000B0 RID: 176
	public class Log
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x000054D5 File Offset: 0x000036D5
		public static Log Instance
		{
			get
			{
				if (Log.instance == null)
				{
					Log.instance = new Log();
				}
				return Log.instance;
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00031680 File Offset: 0x0002F880
		public Log()
		{
			this.logFile = "dg_mod.log";
			try
			{
				this.fs = File.Open(this.logFile, FileMode.Create);
				this.sw = new StreamWriter(this.fs);
				this.sw.AutoFlush = true;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000054ED File Offset: 0x000036ED
		public void WriteLine(string s)
		{
			if (this.sw != null)
			{
				this.sw.WriteLine(s);
			}
		}

		// Token: 0x04000653 RID: 1619
		private static Log instance = new Log();

		// Token: 0x04000654 RID: 1620
		private string logFile;

		// Token: 0x04000655 RID: 1621
		private StreamWriter sw;

		// Token: 0x04000656 RID: 1622
		private FileStream fs;
	}
}

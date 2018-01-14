using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using SimpleJSON;

namespace RocketJump.Modification
{
	// Token: 0x020000CA RID: 202
	public class AutoUpdater
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x00005EF9 File Offset: 0x000040F9
		public AutoUpdater()
		{
			this.OpenURI("https://api.github.com/repos/jkarkkainen/DefyGravityPracticeMod/releases/latest", new Action<Stream>(this.OpenURICompleted));
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00033674 File Offset: 0x00031874
		private void OpenURI(string address, Action<Stream> callback)
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("user-agent", Assembly.GetExecutingAssembly().FullName);
			this.openReadCompletedEventHandler = delegate(object o, OpenReadCompletedEventArgs e)
			{
				if (e.Error != null)
				{
					throw e.Error;
				}
				Action<Stream> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(e.Result);
			};
			webClient.OpenReadCompleted += this.openReadCompletedEventHandler;
			webClient.OpenReadAsync(new Uri(address));
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000336D8 File Offset: 0x000318D8
		private void DownloadURI(string address, string filename, Action completedCallback = null, Action<long, long, int> progressCallback = null)
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("user-agent", Assembly.GetExecutingAssembly().FullName);
			this.asyncCompletedEventHandler = delegate(object o, AsyncCompletedEventArgs e)
			{
				if (e.Error != null)
				{
					throw e.Error;
				}
				Action completedCallback2 = completedCallback;
				if (completedCallback2 == null)
				{
					return;
				}
				completedCallback2();
			};
			webClient.DownloadFileCompleted += this.asyncCompletedEventHandler;
			this.downloadProgressChangedEventHandler = delegate(object o, DownloadProgressChangedEventArgs e)
			{
				Action<long, long, int> progressCallback2 = progressCallback;
				if (progressCallback2 == null)
				{
					return;
				}
				progressCallback2(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
			};
			webClient.DownloadProgressChanged += this.downloadProgressChangedEventHandler;
			webClient.DownloadFileAsync(new Uri(address), filename);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00033764 File Offset: 0x00031964
		private void OpenURICompleted(Stream stream)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					char[] array = new char[32768];
					int count;
					while ((count = streamReader.Read(array, 0, array.Length)) > 0)
					{
						stringWriter.Write(array, 0, count);
					}
				}
				this.json = JSON.Parse(stringWriter.ToString());
			}
			if (this.json != null)
			{
				this.version = this.json["tag_name"].ToString().Trim(new char[]
				{
					'"'
				}).Substring(1);
				string address = this.json["assets"][0]["browser_download_url"].ToString().Trim(new char[]
				{
					'"'
				});
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				string text = version.Minor + "." + version.Build;
				if (MessageBox.Show(string.Concat(new string[]
				{
					"Latest version: ",
					this.version,
					"\nYour version: ",
					text,
					"\n\nDo you want to update Defy Gravity PracticeMod?"
				}), "Update DefyGravity PracticeMod", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
				{
					if (MessageBox.Show("Do you want disable updates completely? You can re-enable them from My Documents\\My Games\\Defy Gravity\\PracticeMod\\PracticeModConfig.xml", "Disable updates", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
					{
						this.CheckFinished(false);
					}
					return;
				}
				if (this.version != text)
				{
					new Thread(delegate
					{
						this.downloadProgressForm = new AutoUpdater.ProgressBarForm();
						this.downloadProgressForm.ShowDialog();
					}).Start();
					this.DownloadURI(address, "DefyGravity.exe.new", new Action(this.DownloadCompleted), new Action<long, long, int>(this.DownloadProgressUpdated));
				}
			}
			this.CheckFinished(true);
			this.CheckComplete = true;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00005F18 File Offset: 0x00004118
		public void DownloadProgressUpdated(long bytesReceived, long bytesTotal, int progressPercent)
		{
			this.downloadProgressForm.DownloadProgress.Value = progressPercent;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00033958 File Offset: 0x00031B58
		public void DownloadCompleted()
		{
			this.downloadProgressForm.Close();
			this.DownloadComplete = true;
			string text = "DefyGravity.exe";
			string text2 = text + ".old";
			string sourceFileName = text + ".new";
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			File.Move(text, text2);
			File.Move(sourceFileName, text);
			Application.Restart();
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00005F2B File Offset: 0x0000412B
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00005F33 File Offset: 0x00004133
		public bool CheckComplete { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00005F3C File Offset: 0x0000413C
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x00005F44 File Offset: 0x00004144
		public bool DownloadComplete { get; private set; }

		// Token: 0x06000637 RID: 1591 RVA: 0x00005F66 File Offset: 0x00004166
		public AutoUpdater(Action<bool> callBack)
		{
			this.CheckFinished = callBack;
			this..ctor();
		}

		// Token: 0x04000697 RID: 1687
		private JSONNode json;

		// Token: 0x04000698 RID: 1688
		private string version;

		// Token: 0x04000699 RID: 1689
		private AutoUpdater.ProgressBarForm downloadProgressForm;

		// Token: 0x0400069C RID: 1692
		private OpenReadCompletedEventHandler openReadCompletedEventHandler;

		// Token: 0x0400069D RID: 1693
		private DownloadProgressChangedEventHandler downloadProgressChangedEventHandler;

		// Token: 0x0400069E RID: 1694
		private AsyncCompletedEventHandler asyncCompletedEventHandler;

		// Token: 0x0400069F RID: 1695
		private Action<bool> CheckFinished;

		// Token: 0x020000CB RID: 203
		public class ProgressBarForm : Form
		{
			// Token: 0x06000638 RID: 1592 RVA: 0x000339B4 File Offset: 0x00031BB4
			public ProgressBarForm()
			{
				base.ClientSize = new Size(600, 25);
				this.DownloadProgress = new ProgressBar();
				this.DownloadProgress.Location = new Point(0, 0);
				this.DownloadProgress.Name = "Download Progress Bar";
				this.DownloadProgress.Size = base.ClientSize;
				this.DownloadProgress.Step = 1;
				this.DownloadProgress.TabIndex = 0;
				base.Controls.Add(this.DownloadProgress);
				base.Name = "Download Progress";
				this.Text = "Downloading the latest version";
				base.StartPosition = FormStartPosition.CenterScreen;
				base.ResumeLayout(false);
			}

			// Token: 0x040006A0 RID: 1696
			public ProgressBar DownloadProgress;
		}
	}
}

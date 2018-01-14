using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RocketJump.Modification
{
	// Token: 0x020000A8 RID: 168
	public class PracticeMode
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x000052F9 File Offset: 0x000034F9
		private PracticeMode.PracticeModeData CurrentPracticeModeData
		{
			get
			{
				return (from x in this.PracticeModeDatas
				where x.Mode == this.CurrentMode
				select x).FirstOrDefault<PracticeMode.PracticeModeData>();
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0003168C File Offset: 0x0002F88C
		public void Update()
		{
			if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F10))
			{
				this.ToggleMode();
			}
			if (this.CurrentMode != PracticeMode.PracticeModes.None)
			{
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp))
				{
					this.LoadNextLevel();
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
				{
					this.LoadPreviousLevel();
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Tab))
				{
					this.ToggleHardMode();
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F1))
				{
					this.ToggleFixedTimestep();
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F2))
				{
					this.practiceModeSettings.UseExperimentalSlowdownFix = !this.practiceModeSettings.UseExperimentalSlowdownFix;
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F3))
				{
					this.ToggleVSync();
				}
			}
			this.levelTimerRunning = (this.oldLevelTimer != this._game.levelTime);
			this.oldLevelTimer = this._game.levelTime;
			this.oldState = this._game.keyboardState;
			if (this.oldLastUpdateTicks != this._game.lastUpdateTicks)
			{
				this.lastUpdateIntervalTicks = this._game.lastUpdateTicks - this.oldLastUpdateTicks;
				if (this.levelTimerRunning)
				{
					this.frameTimeTicks.Enqueue(this.lastUpdateIntervalTicks);
					while (this.frameTimeTicks.Count > 60)
					{
						this.frameTimeTicks.Dequeue();
					}
				}
			}
			this.oldLastUpdateTicks = this._game.lastUpdateTicks;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000317D4 File Offset: 0x0002F9D4
		public void Draw(SpriteBatch spriteBatch)
		{
			double item = (this.frameTimeTicks.Count < 60) ? 166666.0 : this.frameTimeTicks.Average();
			this.averageFrameTimes.Enqueue(item);
			while (this.averageFrameTimes.Count > 300)
			{
				this.averageFrameTimes.Dequeue();
			}
			if (this._game.otherResolution)
			{
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, this._game.SpriteScale);
			}
			else
			{
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
			}
			float num = 0f;
			if (this.CurrentMode != PracticeMode.PracticeModes.None)
			{
				spriteBatch.DrawString(this._game.hudFont, this.CurrentPracticeModeData.OnScreenText, new Vector2(0f, num), Color.Red);
				num += 25f;
				spriteBatch.DrawString(this._game.hudFont, string.Format("Level {0} {1}", this._game.levelIndex.ToString(), this._game.hardMode ? "HARD" : ""), new Vector2(0f, num), Color.Red);
				num += 25f;
				if (!this.practiceModeSettings.IsFixedTimeStep)
				{
					spriteBatch.DrawString(this._game.hudFont, "Fixed Timestep disabled", new Vector2(0f, num), Color.Red);
					num += 25f;
				}
				if (this.practiceModeSettings.UseExperimentalSlowdownFix)
				{
					spriteBatch.DrawString(this._game.hudFont, "Experimental slowdown fix active", new Vector2(0f, num), Color.Red);
					num += 25f;
				}
				if (!this.practiceModeSettings.SyncToVerticalRefresh)
				{
					spriteBatch.DrawString(this._game.hudFont, "VSync off", new Vector2(0f, num), Color.Red);
					num += 25f;
				}
			}
			TexturePackSettings.Instance.ApplyPointClampSetting(spriteBatch.GraphicsDevice);
			double upperLimit = 168333.3;
			double lowerLimit = 165000.0;
			if ((this.averageFrameTimes.All((double x) => x > upperLimit) || this.averageFrameTimes.All((double x) => x < lowerLimit)) && !this.practiceModeSettings.IsFixedTimeStep && !this.practiceModeSettings.UseExperimentalSlowdownFix)
			{
				int width = this._game.notificationTexture.Width;
				int height = this._game.notificationTexture.Height;
				int x2 = this._game.graphics.PreferredBackBufferWidth - 10 - width;
				int y = 10;
				spriteBatch.Draw(this._game.notificationTexture, new Rectangle(x2, y, this._game.notificationTexture.Width, this._game.notificationTexture.Height), Color.Red);
			}
			spriteBatch.End();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00031ABC File Offset: 0x0002FCBC
		public PracticeMode()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			try
			{
				this._game = PlatformerGame.Instance;
				this.configFileName = "PracticeModConfig.xml";
				this.PracticeModeDatas = new List<PracticeMode.PracticeModeData>
				{
					new PracticeMode.PracticeModeData
					{
						Mode = PracticeMode.PracticeModes.None
					},
					new PracticeMode.PracticeModeData
					{
						Mode = PracticeMode.PracticeModes.LevelWithCheckpoints,
						OnScreenText = "Level Practice - checkpoints active",
						OnExit = delegate
						{
							this._game.levelIndex = this._game.levelIndex - 1;
						},
						OnDeath = delegate
						{
							if (this._game.savePoint == 0)
							{
								this._game.levelTime = 0f;
							}
						}
					},
					new PracticeMode.PracticeModeData
					{
						Mode = PracticeMode.PracticeModes.LevelWithoutCheckpoints,
						OnScreenText = "Level Practice - checkpoints inactive",
						OnExit = delegate
						{
							this._game.levelIndex = this._game.levelIndex - 1;
						},
						OnDeath = delegate
						{
							this._game.savePoint = 0;
							this._game.levelTime = 0f;
						}
					}
				};
				this.oldState = Keyboard.GetState();
				this.LoadConfig();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			this.oldLastUpdateTicks = this._game.lastUpdateTicks;
			this.frameTimeTicks = new Queue<long>();
			this.averageFrameTimes = new Queue<double>();
			if (File.Exists("DefyGravity.exe.old"))
			{
				File.Delete("DefyGravity.exe.old");
			}
			if (this.practiceModeSettings.CheckForUpdates)
			{
				new AutoUpdater(delegate(bool b)
				{
					this.practiceModeSettings.CheckForUpdates = b;
				});
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00031C20 File Offset: 0x0002FE20
		public void OnDeath()
		{
			Action onDeath = this.CurrentPracticeModeData.OnDeath;
			if (onDeath == null)
			{
				return;
			}
			onDeath();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00031C44 File Offset: 0x0002FE44
		public void OnExit()
		{
			if (this.allowNextLoad)
			{
				this.allowNextLoad = false;
				return;
			}
			Action onExit = this.CurrentPracticeModeData.OnExit;
			if (onExit == null)
			{
				return;
			}
			onExit();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00005327 File Offset: 0x00003527
		public bool KeyPressed(Microsoft.Xna.Framework.Input.Keys key)
		{
			return this._game != null && !this.oldState.IsKeyDown(key) && this._game.keyboardState.IsKeyDown(key);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000538B File Offset: 0x0000358B
		public void ToggleMode()
		{
			this.CurrentMode = (this.CurrentMode + 1) % (PracticeMode.PracticeModes)3;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00031C78 File Offset: 0x0002FE78
		public void LoadNextLevel()
		{
			if (this._game.levelIndex >= 24)
			{
				this._game.levelIndex = 0;
				this._game.hardMode = !this._game.hardMode;
			}
			this._game.savePoint = 0;
			this._game.showingPlot = true;
			this._game.levelTime = 0f;
			this._game.GetType().GetMethod("LoadNextLevel", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this._game, null);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00031D08 File Offset: 0x0002FF08
		public void LoadPreviousLevel()
		{
			this._game.levelIndex -= 2;
			if (this._game.levelIndex < 0)
			{
				this._game.levelIndex = 23;
				this._game.hardMode = !this._game.hardMode;
			}
			this._game.savePoint = 0;
			this._game.showingPlot = true;
			this._game.levelTime = 0f;
			this._game.GetType().GetMethod("LoadNextLevel", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this._game, null);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00031DA8 File Offset: 0x0002FFA8
		public void ToggleHardMode()
		{
			this._game.hardMode = !this._game.hardMode;
			this._game.levelIndex--;
			this._game.loadNextLevel = true;
			this.allowNextLoad = true;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00031DF4 File Offset: 0x0002FFF4
		public void ToggleFixedTimestep()
		{
			this.practiceModeSettings.IsFixedTimeStep = (this._game.IsFixedTimeStep = !this._game.IsFixedTimeStep);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00031E28 File Offset: 0x00030028
		public void SaveConfig()
		{
			try
			{
				Directory.CreateDirectory(this.configFilePathBase);
				using (FileStream fileStream = new FileStream(this.configFilePath, FileMode.Create))
				{
					new XmlSerializer(typeof(PracticeMode.PracticeModeSettings)).Serialize(fileStream, this.practiceModeSettings);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00031E98 File Offset: 0x00030098
		public void LoadConfig()
		{
			try
			{
				using (FileStream fileStream = new FileStream(this.configFilePath, FileMode.Open))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(PracticeMode.PracticeModeSettings));
					this.practiceModeSettings = (PracticeMode.PracticeModeSettings)xmlSerializer.Deserialize(fileStream);
				}
			}
			catch (Exception)
			{
			}
			if (this.practiceModeSettings == null)
			{
				this.practiceModeSettings = new PracticeMode.PracticeModeSettings();
			}
			this.ApplyConfig();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00031F1C File Offset: 0x0003011C
		public void ApplyConfig()
		{
			this._game.IsFixedTimeStep = this.practiceModeSettings.IsFixedTimeStep;
			this._game.graphics.SynchronizeWithVerticalRetrace = this.practiceModeSettings.SyncToVerticalRefresh;
			this._game.graphics.ApplyChanges();
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000053BB File Offset: 0x000035BB
		private string configFilePathBase
		{
			get
			{
				return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games"), Path.Combine("Defy Gravity", "PracticeMod"));
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000053E1 File Offset: 0x000035E1
		private string configFilePath
		{
			get
			{
				return Path.Combine(this.configFilePathBase, this.configFileName);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000053F4 File Offset: 0x000035F4
		protected override void Finalize()
		{
			this.SaveConfig();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00031F6C File Offset: 0x0003016C
		public void ToggleVSync()
		{
			this.practiceModeSettings.SyncToVerticalRefresh = !this.practiceModeSettings.SyncToVerticalRefresh;
			this._game.graphics.SynchronizeWithVerticalRetrace = this.practiceModeSettings.SyncToVerticalRefresh;
			this._game.graphics.ApplyChanges();
		}

		// Token: 0x0400062C RID: 1580
		private PlatformerGame _game;

		// Token: 0x0400062D RID: 1581
		private PracticeMode.PracticeModes CurrentMode;

		// Token: 0x0400062E RID: 1582
		private List<PracticeMode.PracticeModeData> PracticeModeDatas;

		// Token: 0x0400062F RID: 1583
		private KeyboardState oldState;

		// Token: 0x04000630 RID: 1584
		private bool allowNextLoad;

		// Token: 0x04000631 RID: 1585
		public PracticeMode.PracticeModeSettings practiceModeSettings;

		// Token: 0x04000632 RID: 1586
		private string configFileName;

		// Token: 0x04000633 RID: 1587
		public long lastUpdateIntervalTicks;

		// Token: 0x04000634 RID: 1588
		public long oldLastUpdateTicks;

		// Token: 0x04000635 RID: 1589
		public Queue<long> frameTimeTicks;

		// Token: 0x04000636 RID: 1590
		public float oldLevelTimer;

		// Token: 0x04000637 RID: 1591
		public bool levelTimerRunning;

		// Token: 0x04000638 RID: 1592
		public Queue<double> averageFrameTimes;

		// Token: 0x04000639 RID: 1593
		public Texture2D notificationTexture;

		// Token: 0x020000A9 RID: 169
		public enum PracticeModes
		{
			// Token: 0x0400063B RID: 1595
			None,
			// Token: 0x0400063C RID: 1596
			LevelWithCheckpoints,
			// Token: 0x0400063D RID: 1597
			LevelWithoutCheckpoints
		}

		// Token: 0x020000AA RID: 170
		public class PracticeModeData
		{
			// Token: 0x0400063E RID: 1598
			public PracticeMode.PracticeModes Mode;

			// Token: 0x0400063F RID: 1599
			public string OnScreenText = "";

			// Token: 0x04000640 RID: 1600
			public Action OnExit;

			// Token: 0x04000641 RID: 1601
			public Action OnDeath;
		}

		// Token: 0x020000AB RID: 171
		public class PracticeModeSettings
		{
			// Token: 0x0600050B RID: 1291 RVA: 0x0000541D File Offset: 0x0000361D
			public PracticeModeSettings()
			{
				this.IsFixedTimeStep = true;
				this.SyncToVerticalRefresh = true;
				this.CheckForUpdates = true;
			}

			// Token: 0x04000642 RID: 1602
			public bool IsFixedTimeStep;

			// Token: 0x04000643 RID: 1603
			public bool UseExperimentalSlowdownFix;

			// Token: 0x04000644 RID: 1604
			public bool SyncToVerticalRefresh;

			// Token: 0x04000645 RID: 1605
			public bool CheckForUpdates;
		}
	}
}

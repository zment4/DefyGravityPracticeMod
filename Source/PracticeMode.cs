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
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000052D9 File Offset: 0x000034D9
		private PracticeMode.PracticeModeData CurrentPracticeModeData
		{
			get
			{
				return (from x in this.PracticeModeDatas
				where x.Mode == this.CurrentMode
				select x).FirstOrDefault<PracticeMode.PracticeModeData>();
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00030C8C File Offset: 0x0002EE8C
		public void Update()
		{
			if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.F10))
			{
				this.ToggleMode();
			}
			if (this.CurrentMode != PracticeMode.PracticeModes.None)
			{
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
				{
					this.LoadNextLevel();
				}
				if (this.KeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp))
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

		// Token: 0x060004EA RID: 1258 RVA: 0x00030DD4 File Offset: 0x0002EFD4
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

		// Token: 0x060004EB RID: 1259 RVA: 0x000310BC File Offset: 0x0002F2BC
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
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000311EC File Offset: 0x0002F3EC
		public void OnDeath()
		{
			Action onDeath = this.CurrentPracticeModeData.OnDeath;
			if (onDeath == null)
			{
				return;
			}
			onDeath();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00031210 File Offset: 0x0002F410
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

		// Token: 0x060004EF RID: 1263 RVA: 0x00005307 File Offset: 0x00003507
		public bool KeyPressed(Microsoft.Xna.Framework.Input.Keys key)
		{
			return this._game != null && !this.oldState.IsKeyDown(key) && this._game.keyboardState.IsKeyDown(key);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000536B File Offset: 0x0000356B
		public void ToggleMode()
		{
			this.CurrentMode = (this.CurrentMode + 1) % (PracticeMode.PracticeModes)3;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00031244 File Offset: 0x0002F444
		public void LoadNextLevel()
		{
			this._game.levelIndex -= 2;
			if (this._game.levelIndex < 0)
			{
				this._game.levelIndex = 23;
				this._game.hardMode = !this._game.hardMode;
			}
			this._game.savePoint = 0;
			this._game.loadNextLevel = true;
			this.allowNextLoad = true;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000312B8 File Offset: 0x0002F4B8
		public void LoadPreviousLevel()
		{
			if (this._game.levelIndex >= 24)
			{
				this._game.levelIndex = 0;
				this._game.hardMode = !this._game.hardMode;
			}
			this._game.savePoint = 0;
			this._game.loadNextLevel = true;
			this.allowNextLoad = true;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00031318 File Offset: 0x0002F518
		public void ToggleHardMode()
		{
			this._game.hardMode = !this._game.hardMode;
			this._game.levelIndex--;
			this._game.loadNextLevel = true;
			this.allowNextLoad = true;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00031364 File Offset: 0x0002F564
		public void ToggleFixedTimestep()
		{
			this.practiceModeSettings.IsFixedTimeStep = !(bool)this.GetNamedMember(this._game, "isFixedTimeStep");
			this.SetNamedMember(this._game, "isFixedTimeStep", this.practiceModeSettings.IsFixedTimeStep);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000313B8 File Offset: 0x0002F5B8
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

		// Token: 0x060004F9 RID: 1273 RVA: 0x00031428 File Offset: 0x0002F628
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
				this.practiceModeSettings = new PracticeMode.PracticeModeSettings
				{
					IsFixedTimeStep = true,
					SyncToVerticalRefresh = true
				};
			}
			this.ApplyConfig();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x000314B8 File Offset: 0x0002F6B8
		public void ApplyConfig()
		{
			try
			{
				this.SetNamedMember(this._game, "isFixedTimeStep", this.practiceModeSettings.IsFixedTimeStep);
				this._game.graphics.SynchronizeWithVerticalRetrace = this.practiceModeSettings.SyncToVerticalRefresh;
				this._game.graphics.ApplyChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0000539B File Offset: 0x0000359B
		private string configFilePathBase
		{
			get
			{
				return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games"), Path.Combine("Defy Gravity", "PracticeMod"));
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x000053C1 File Offset: 0x000035C1
		private string configFilePath
		{
			get
			{
				return Path.Combine(this.configFilePathBase, this.configFileName);
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000053D4 File Offset: 0x000035D4
		protected override void Finalize()
		{
			this.SaveConfig();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000053DC File Offset: 0x000035DC
		public object GetNamedMember(object obj, string name)
		{
			return this.GetMemberInfo(obj, name).GetValue(obj);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000053EC File Offset: 0x000035EC
		public void SetNamedMember(object obj, string name, object value)
		{
			this.GetMemberInfo(obj, name).SetValue(obj, value);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000053FD File Offset: 0x000035FD
		public MemberInfo GetMemberInfo(object obj, string name)
		{
			return this.GetMemberInfo(obj.GetType(), name);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00031530 File Offset: 0x0002F730
		public MemberInfo GetMemberInfo(Type type, string name)
		{
			PropertyInfo propertyInfo = null;
			if (propertyInfo == null)
			{
				propertyInfo = type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic);
			}
			if (propertyInfo == null)
			{
				propertyInfo = type.GetProperty(name, BindingFlags.Static | BindingFlags.NonPublic);
			}
			if (propertyInfo != null)
			{
				return propertyInfo;
			}
			FieldInfo fieldInfo = null;
			if (fieldInfo == null)
			{
				fieldInfo = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
			}
			if (fieldInfo == null)
			{
				fieldInfo = type.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
			}
			if (fieldInfo == null && type.BaseType != null)
			{
				return this.GetMemberInfo(type.BaseType, name);
			}
			return fieldInfo;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00031594 File Offset: 0x0002F794
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
			// Token: 0x04000642 RID: 1602
			public bool IsFixedTimeStep;

			// Token: 0x04000643 RID: 1603
			public bool UseExperimentalSlowdownFix;

			// Token: 0x04000644 RID: 1604
			public bool SyncToVerticalRefresh;
		}
	}
}

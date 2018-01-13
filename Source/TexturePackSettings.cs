using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace RocketJump.Modification
{
	// Token: 0x020000B4 RID: 180
	public class TexturePackSettings
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x000055EA File Offset: 0x000037EA
		public TexturePackSettings()
		{
			if (PlatformerGame.Instance.TextureContent.RootDirectory != "Content")
			{
				this.LoadSettings();
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00005613 File Offset: 0x00003813
		private string TextureSettingsPath
		{
			get
			{
				return Path.Combine(PlatformerGame.Instance.TextureContent.RootDirectory, "Settings.xml");
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00031924 File Offset: 0x0002FB24
		public void LoadSettings()
		{
			try
			{
				using (FileStream fileStream = File.Open(this.TextureSettingsPath, FileMode.Open, FileAccess.Read))
				{
					TexturePackSettings texturePackSettings = (TexturePackSettings)new XmlSerializer(base.GetType()).Deserialize(fileStream);
					this.UsePointClamp = texturePackSettings.UsePointClamp;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00031990 File Offset: 0x0002FB90
		public void SaveSettings()
		{
			try
			{
				using (FileStream fileStream = File.Open(this.TextureSettingsPath, FileMode.Create, FileAccess.Write))
				{
					new XmlSerializer(base.GetType()).Serialize(fileStream, this);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000563A File Offset: 0x0000383A
		public static TexturePackSettings Instance
		{
			get
			{
				return TexturePackSettings.instance;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00005641 File Offset: 0x00003841
		public void ApplyPointClampSetting(GraphicsDevice graphicsDevice)
		{
			if (this.UsePointClamp)
			{
				SamplerState samplerState = graphicsDevice.SamplerStates[0];
				samplerState.MipFilter = TextureFilter.Point;
				samplerState.MinFilter = TextureFilter.Point;
				samplerState.MagFilter = TextureFilter.Point;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000566B File Offset: 0x0000386B
		protected override void Finalize()
		{
			this.SaveSettings();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00005673 File Offset: 0x00003873
		public void UnApplyPointClampSetting(GraphicsDevice graphicsDevice)
		{
			if (this.UsePointClamp)
			{
				SamplerState samplerState = graphicsDevice.SamplerStates[0];
				samplerState.MipFilter = TextureFilter.Linear;
				samplerState.MinFilter = TextureFilter.Linear;
				samplerState.MagFilter = TextureFilter.Linear;
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000569D File Offset: 0x0000389D
		public void ApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
				this.ApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000056C2 File Offset: 0x000038C2
		public void UnApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
				this.UnApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0400065C RID: 1628
		public bool UsePointClamp;

		// Token: 0x0400065D RID: 1629
		private static TexturePackSettings instance = new TexturePackSettings();
	}
}

using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace RocketJump.Modification
{
	// Token: 0x020000B4 RID: 180
	public class TexturePackSettings
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x00005610 File Offset: 0x00003810
		public TexturePackSettings()
		{
			if (PlatformerGame.Instance.TextureContent.RootDirectory != "Content")
			{
				this.LoadSettings();
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00005639 File Offset: 0x00003839
		private string TextureSettingsPath
		{
			get
			{
				return Path.Combine(PlatformerGame.Instance.TextureContent.RootDirectory, "Settings.xml");
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0003190C File Offset: 0x0002FB0C
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

		// Token: 0x06000537 RID: 1335 RVA: 0x00031978 File Offset: 0x0002FB78
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00005660 File Offset: 0x00003860
		public static TexturePackSettings Instance
		{
			get
			{
				return TexturePackSettings.instance;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00005667 File Offset: 0x00003867
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

		// Token: 0x0600053B RID: 1339 RVA: 0x00005691 File Offset: 0x00003891
		protected override void Finalize()
		{
			this.SaveSettings();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00005699 File Offset: 0x00003899
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

		// Token: 0x0600053D RID: 1341 RVA: 0x000056C3 File Offset: 0x000038C3
		public void ApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
				this.ApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000056E8 File Offset: 0x000038E8
		public void UnApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
				this.UnApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0400065D RID: 1629
		public bool UsePointClamp;

		// Token: 0x0400065E RID: 1630
		private static TexturePackSettings instance = new TexturePackSettings();
	}
}

using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RocketJump.Modification
{
	// Token: 0x020000B4 RID: 180
	public class TexturePackSettings
	{
		// Token: 0x06000533 RID: 1331 RVA: 0x000055BF File Offset: 0x000037BF
		public TexturePackSettings()
		{
			if (PlatformerGame.Instance.TextureContent.RootDirectory != "Content")
			{
				this.LoadSettings();
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x000055E8 File Offset: 0x000037E8
		private string TextureSettingsPath
		{
			get
			{
				return Path.Combine(PlatformerGame.Instance.TextureContent.RootDirectory, "Settings.xml");
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000318B4 File Offset: 0x0002FAB4
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

		// Token: 0x06000536 RID: 1334 RVA: 0x00031920 File Offset: 0x0002FB20
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
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0000560F File Offset: 0x0000380F
		public static TexturePackSettings Instance
		{
			get
			{
				return TexturePackSettings.instance;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00005616 File Offset: 0x00003816
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

		// Token: 0x0600053A RID: 1338 RVA: 0x00005640 File Offset: 0x00003840
		protected override void Finalize()
		{
			this.SaveSettings();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00005648 File Offset: 0x00003848
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

		// Token: 0x0600053C RID: 1340 RVA: 0x0003197C File Offset: 0x0002FB7C
		public void ApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				Matrix namedMember = spriteBatch.GetNamedMember("spriteTransformMatrix");
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, namedMember);
				this.ApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000319BC File Offset: 0x0002FBBC
		public void UnApplyPointClampSetting(SpriteBatch spriteBatch)
		{
			if (this.UsePointClamp)
			{
				Matrix namedMember = spriteBatch.GetNamedMember("spriteTransformMatrix");
				spriteBatch.End();
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, namedMember);
				this.UnApplyPointClampSetting(spriteBatch.GraphicsDevice);
			}
		}

		// Token: 0x0400065C RID: 1628
		public bool UsePointClamp;

		// Token: 0x0400065D RID: 1629
		private static TexturePackSettings instance = new TexturePackSettings();
	}
}

using System;
using Microsoft.Xna.Framework;

namespace RocketJump.Modification
{
	// Token: 0x020000B1 RID: 177
	public class FakeGameTime : GameTime
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x000316FC File Offset: 0x0002F8FC
		public new TimeSpan ElapsedGameTime
		{
			get
			{
				return TimeSpan.FromTicks((this.currentGameTime.ElapsedGameTime.Ticks > 0L) ? 166666L : 0L);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x000054E9 File Offset: 0x000036E9
		public new TimeSpan TotalGameTime
		{
			get
			{
				return this.totalGameTime;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00005504 File Offset: 0x00003704
		public new TimeSpan ElapsedRealTime
		{
			get
			{
				return this.currentGameTime.ElapsedRealTime;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00005511 File Offset: 0x00003711
		public new TimeSpan TotalRealTime
		{
			get
			{
				return this.currentGameTime.TotalRealTime;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00031730 File Offset: 0x0002F930
		public void Update()
		{
			if (this.currentGameTime.TotalGameTime.Ticks > 0L)
			{
				this.totalGameTime += this.ElapsedGameTime;
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000551E File Offset: 0x0000371E
		public void SetGameTime(GameTime gameTime)
		{
			this.currentGameTime = gameTime;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00005527 File Offset: 0x00003727
		public new bool IsRunningSlowly
		{
			get
			{
				return this.currentGameTime.IsRunningSlowly;
			}
		}

		// Token: 0x04000656 RID: 1622
		private TimeSpan totalGameTime;

		// Token: 0x04000657 RID: 1623
		public GameTime currentGameTime = new GameTime();
	}
}

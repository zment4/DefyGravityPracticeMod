using System;
using Microsoft.Xna.Framework;

namespace RocketJump.Modification
{
	// Token: 0x020000B1 RID: 177
	public class FakeGameTime : GameTime
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x000316E4 File Offset: 0x0002F8E4
		public new TimeSpan ElapsedGameTime
		{
			get
			{
				return TimeSpan.FromTicks((this.currentGameTime.ElapsedGameTime.Ticks > 0L) ? 166666L : 0L);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000550F File Offset: 0x0000370F
		public new TimeSpan TotalGameTime
		{
			get
			{
				return this.totalGameTime;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000552A File Offset: 0x0000372A
		public new TimeSpan ElapsedRealTime
		{
			get
			{
				return this.currentGameTime.ElapsedRealTime;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00005537 File Offset: 0x00003737
		public new TimeSpan TotalRealTime
		{
			get
			{
				return this.currentGameTime.TotalRealTime;
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00031718 File Offset: 0x0002F918
		public void Update()
		{
			if (this.currentGameTime.TotalGameTime.Ticks > 0L)
			{
				this.totalGameTime += this.ElapsedGameTime;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00005544 File Offset: 0x00003744
		public void SetGameTime(GameTime gameTime)
		{
			this.currentGameTime = gameTime;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0000554D File Offset: 0x0000374D
		public new bool IsRunningSlowly
		{
			get
			{
				return this.currentGameTime.IsRunningSlowly;
			}
		}

		// Token: 0x04000657 RID: 1623
		private TimeSpan totalGameTime;

		// Token: 0x04000658 RID: 1624
		public GameTime currentGameTime = new GameTime();
	}
}

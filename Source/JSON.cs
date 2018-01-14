using System;

namespace SimpleJSON
{
	// Token: 0x020000C9 RID: 201
	public static class JSON
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x00005EF1 File Offset: 0x000040F1
		public static JSONNode Parse(string aJSON)
		{
			return JSONNode.Parse(aJSON);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000BF RID: 191
	public class JSONArray : JSONNode
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00005A4D File Offset: 0x00003C4D
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00005A55 File Offset: 0x00003C55
		public override bool Inline
		{
			get
			{
				return this.inline;
			}
			set
			{
				this.inline = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000208F File Offset: 0x0000028F
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Array;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00005A5E File Offset: 0x00003C5E
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_List.GetEnumerator());
		}

		// Token: 0x1700009F RID: 159
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				this.m_List.Add(value);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00005AFB File Offset: 0x00003CFB
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00005ADD File Offset: 0x00003CDD
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			this.m_List.Add(aItem);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00005B08 File Offset: 0x00003D08
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00005B36 File Offset: 0x00003D36
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00005B46 File Offset: 0x00003D46
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00032DD4 File Offset: 0x00030FD4
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('[');
			int count = this.m_List.Count;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
				{
					aSB.Append(',');
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				this.m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append(']');
		}

		// Token: 0x04000681 RID: 1665
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x04000682 RID: 1666
		private bool inline;
	}
}

using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C6 RID: 198
	public class JSONBool : JSONNode
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00005DA1 File Offset: 0x00003FA1
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Boolean;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsBoolean
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00033378 File Offset: 0x00031578
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00005DA4 File Offset: 0x00003FA4
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x000334C8 File Offset: 0x000316C8
		public override string Value
		{
			get
			{
				return this.m_Data.ToString();
			}
			set
			{
				bool data;
				if (bool.TryParse(value, out data))
				{
					this.m_Data = data;
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00005DB1 File Offset: 0x00003FB1
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00005DB9 File Offset: 0x00003FB9
		public override bool AsBool
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00005DC2 File Offset: 0x00003FC2
		public JSONBool(bool aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00005D76 File Offset: 0x00003F76
		public JSONBool(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00005DD1 File Offset: 0x00003FD1
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.m_Data ? "true" : "false");
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00005DEE File Offset: 0x00003FEE
		public override bool Equals(object obj)
		{
			return obj != null && obj is bool && this.m_Data == (bool)obj;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00005E0D File Offset: 0x0000400D
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000692 RID: 1682
		private bool m_Data;
	}
}

using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C8 RID: 200
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x00005E71 File Offset: 0x00004071
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.None;
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00033378 File Offset: 0x00031578
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00005E74 File Offset: 0x00004074
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00005E8A File Offset: 0x0000408A
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00005EA0 File Offset: 0x000040A0
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x170000BE RID: 190
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				JSONArray jsonarray = new JSONArray();
				jsonarray.Add(value);
				this.Set(jsonarray);
			}
		}

		// Token: 0x170000BF RID: 191
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				JSONObject jsonobject = new JSONObject();
				jsonobject.Add(aKey, value);
				this.Set(jsonobject);
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00033530 File Offset: 0x00031730
		public override void Add(JSONNode aItem)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.Add(aItem);
			this.Set(jsonarray);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0003350C File Offset: 0x0003170C
		public override void Add(string aKey, JSONNode aItem)
		{
			JSONObject jsonobject = new JSONObject();
			jsonobject.Add(aKey, aItem);
			this.Set(jsonobject);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00005EDA File Offset: 0x000040DA
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00005EE5 File Offset: 0x000040E5
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00005EDA File Offset: 0x000040DA
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0000208C File Offset: 0x0000028C
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00033554 File Offset: 0x00031754
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x00033578 File Offset: 0x00031778
		public override int AsInt
		{
			get
			{
				JSONNumber aVal = new JSONNumber(0.0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONNumber aVal = new JSONNumber((double)value);
				this.Set(aVal);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00033594 File Offset: 0x00031794
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00033578 File Offset: 0x00031778
		public override float AsFloat
		{
			get
			{
				JSONNumber aVal = new JSONNumber(0.0);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONNumber aVal = new JSONNumber((double)value);
				this.Set(aVal);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x000335BC File Offset: 0x000317BC
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x000335E8 File Offset: 0x000317E8
		public override double AsDouble
		{
			get
			{
				JSONNumber aVal = new JSONNumber(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONNumber aVal = new JSONNumber(value);
				this.Set(aVal);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00033604 File Offset: 0x00031804
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00033620 File Offset: 0x00031820
		public override bool AsBool
		{
			get
			{
				JSONBool aVal = new JSONBool(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONBool aVal = new JSONBool(value);
				this.Set(aVal);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0003363C File Offset: 0x0003183C
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00033658 File Offset: 0x00031858
		public override JSONObject AsObject
		{
			get
			{
				JSONObject jsonobject = new JSONObject();
				this.Set(jsonobject);
				return jsonobject;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00005E51 File Offset: 0x00004051
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000695 RID: 1685
		private JSONNode m_Node;

		// Token: 0x04000696 RID: 1686
		private string m_Key;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000B7 RID: 183
	public abstract class JSONNode
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000544 RID: 1348
		public abstract JSONNodeType Tag { get; }

		// Token: 0x1700007B RID: 123
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x1700007C RID: 124
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x000056BD File Offset: 0x000038BD
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0000208A File Offset: 0x0000028A
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0000208C File Offset: 0x0000028C
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0000208A File Offset: 0x0000028A
		public virtual bool Inline
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000208A File Offset: 0x0000028A
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000056C4 File Offset: 0x000038C4
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000021F6 File Offset: 0x000003F6
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000021F6 File Offset: 0x000003F6
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000056D2 File Offset: 0x000038D2
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000056D5 File Offset: 0x000038D5
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000056DE File Offset: 0x000038DE
		public IEnumerable<JSONNode> DeepChildren
		{
			get
			{
				foreach (JSONNode jsonnode in this.Children)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChildren)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000324F0 File Offset: 0x000306F0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00032514 File Offset: 0x00030714
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x0600055D RID: 1373
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x0600055E RID: 1374
		public abstract JSONNode.Enumerator GetEnumerator();

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x000056EE File Offset: 0x000038EE
		public IEnumerable<KeyValuePair<string, JSONNode>> Linq
		{
			get
			{
				return new JSONNode.LinqEnumerator(this);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000056F6 File Offset: 0x000038F6
		public JSONNode.KeyEnumerator Keys
		{
			get
			{
				return new JSONNode.KeyEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00005703 File Offset: 0x00003903
		public JSONNode.ValueEnumerator Values
		{
			get
			{
				return new JSONNode.ValueEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00032538 File Offset: 0x00030738
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00005710 File Offset: 0x00003910
		public virtual double AsDouble
		{
			get
			{
				double result = 0.0;
				if (double.TryParse(this.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
				{
					return result;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00005724 File Offset: 0x00003924
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0000572D File Offset: 0x0000392D
		public virtual int AsInt
		{
			get
			{
				return (int)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00005737 File Offset: 0x00003937
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x0000572D File Offset: 0x0000392D
		public virtual float AsFloat
		{
			get
			{
				return (float)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00032574 File Offset: 0x00030774
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00005740 File Offset: 0x00003940
		public virtual bool AsBool
		{
			get
			{
				bool result = false;
				if (bool.TryParse(this.Value, out result))
				{
					return result;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00005757 File Offset: 0x00003957
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000575F File Offset: 0x0000395F
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00005767 File Offset: 0x00003967
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0000576F File Offset: 0x0000396F
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00005782 File Offset: 0x00003982
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000578A File Offset: 0x0000398A
		public static implicit operator double(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsDouble;
			}
			return 0.0;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000057A5 File Offset: 0x000039A5
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000057AE File Offset: 0x000039AE
		public static implicit operator float(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsFloat;
			}
			return 0f;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000057A5 File Offset: 0x000039A5
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000057C5 File Offset: 0x000039C5
		public static implicit operator int(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsInt;
			}
			return 0;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000057D8 File Offset: 0x000039D8
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000057E0 File Offset: 0x000039E0
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000057F3 File Offset: 0x000039F3
		public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
		{
			return aKeyValue.Value;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000325A4 File Offset: 0x000307A4
		public static bool operator ==(JSONNode a, object b)
		{
			if (a == b)
			{
				return true;
			}
			bool flag = a is JSONNull || a == null || a is JSONLazyCreator;
			bool flag2 = b is JSONNull || b == null || b is JSONLazyCreator;
			return (flag && flag2) || (!flag && a.Equals(b));
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x000057FC File Offset: 0x000039FC
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00005808 File Offset: 0x00003A08
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000580E File Offset: 0x00003A0E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00005816 File Offset: 0x00003A16
		internal static StringBuilder EscapeBuilder
		{
			get
			{
				if (JSONNode.m_EscapeBuilder == null)
				{
					JSONNode.m_EscapeBuilder = new StringBuilder();
				}
				return JSONNode.m_EscapeBuilder;
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000325FC File Offset: 0x000307FC
		internal static string Escape(string aText)
		{
			StringBuilder escapeBuilder = JSONNode.EscapeBuilder;
			escapeBuilder.Length = 0;
			if (escapeBuilder.Capacity < aText.Length + aText.Length / 10)
			{
				escapeBuilder.Capacity = aText.Length + aText.Length / 10;
			}
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					escapeBuilder.Append("\\b");
					break;
				case '\t':
					escapeBuilder.Append("\\t");
					break;
				case '\n':
					escapeBuilder.Append("\\n");
					break;
				case '\v':
					goto IL_E2;
				case '\f':
					escapeBuilder.Append("\\f");
					break;
				case '\r':
					escapeBuilder.Append("\\r");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_E2;
						}
						escapeBuilder.Append("\\\\");
					}
					else
					{
						escapeBuilder.Append("\\\"");
					}
					break;
				}
				IL_121:
				i++;
				continue;
				IL_E2:
				if (c < ' ' || (JSONNode.forceASCII && c > '\u007f'))
				{
					ushort num = (ushort)c;
					escapeBuilder.Append("\\u").Append(num.ToString("X4"));
					goto IL_121;
				}
				escapeBuilder.Append(c);
				goto IL_121;
			}
			string result = escapeBuilder.ToString();
			escapeBuilder.Length = 0;
			return result;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0003274C File Offset: 0x0003094C
		private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
		{
			if (quoted)
			{
				ctx.Add(tokenName, token);
				return;
			}
			string a = token.ToLower();
			if (a == "false" || a == "true")
			{
				ctx.Add(tokenName, a == "true");
				return;
			}
			if (a == "null")
			{
				ctx.Add(tokenName, null);
				return;
			}
			double n;
			if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out n))
			{
				ctx.Add(tokenName, n);
				return;
			}
			ctx.Add(tokenName, token);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000327E8 File Offset: 0x000309E8
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			bool flag = false;
			bool flag2 = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_33E;
						case '\v':
						case '\f':
							goto IL_330;
						default:
							if (c != ' ')
							{
								goto IL_330;
							}
							break;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_330;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
						else
						{
							if (stringBuilder.Length > 0 || flag2)
							{
								JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							}
							text = "";
							stringBuilder.Length = 0;
							flag2 = false;
						}
					}
					else
					{
						flag = !flag;
						flag2 = (flag2 || flag);
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									stringBuilder.Append(aJSON[i]);
									goto IL_33E;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									jsonnode.Add(text, stack.Peek());
								}
								text = "";
								stringBuilder.Length = 0;
								jsonnode = stack.Peek();
								goto IL_33E;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											stringBuilder.Append('\b');
											goto IL_33E;
										}
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
											goto IL_33E;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											stringBuilder.Append('\n');
											goto IL_33E;
										}
										switch (c2)
										{
										case 'r':
											stringBuilder.Append('\r');
											goto IL_33E;
										case 't':
											stringBuilder.Append('\t');
											goto IL_33E;
										case 'u':
										{
											string s = aJSON.Substring(i + 1, 4);
											stringBuilder.Append((char)int.Parse(s, NumberStyles.AllowHexSpecifier));
											i += 4;
											goto IL_33E;
										}
										}
									}
									stringBuilder.Append(c2);
									goto IL_33E;
								}
								goto IL_33E;
							case ']':
								break;
							default:
								goto IL_330;
							}
						}
						else
						{
							if (flag)
							{
								stringBuilder.Append(aJSON[i]);
								goto IL_33E;
							}
							text = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag2 = false;
							goto IL_33E;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_330;
						}
					}
					else
					{
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_33E;
						}
						stack.Push(new JSONObject());
						if (jsonnode != null)
						{
							jsonnode.Add(text, stack.Peek());
						}
						text = "";
						stringBuilder.Length = 0;
						jsonnode = stack.Peek();
						goto IL_33E;
					}
					if (flag)
					{
						stringBuilder.Append(aJSON[i]);
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (stringBuilder.Length > 0 || flag2)
						{
							JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							flag2 = false;
						}
						text = "";
						stringBuilder.Length = 0;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_33E:
				i++;
				continue;
				IL_330:
				stringBuilder.Append(aJSON[i]);
				goto IL_33E;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x0400066B RID: 1643
		public static bool forceASCII;

		// Token: 0x0400066C RID: 1644
		[ThreadStatic]
		private static StringBuilder m_EscapeBuilder;

		// Token: 0x020000B8 RID: 184
		public struct Enumerator
		{
			// Token: 0x17000092 RID: 146
			// (get) Token: 0x06000581 RID: 1409 RVA: 0x0000582E File Offset: 0x00003A2E
			public bool IsValid
			{
				get
				{
					return this.type > JSONNode.Enumerator.Type.None;
				}
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x00005839 File Offset: 0x00003A39
			public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this.type = JSONNode.Enumerator.Type.Array;
				this.m_Object = default(Dictionary<string, JSONNode>.Enumerator);
				this.m_Array = aArrayEnum;
			}

			// Token: 0x06000583 RID: 1411 RVA: 0x00005855 File Offset: 0x00003A55
			public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this.type = JSONNode.Enumerator.Type.Object;
				this.m_Object = aDictEnum;
				this.m_Array = default(List<JSONNode>.Enumerator);
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x06000584 RID: 1412 RVA: 0x00032B54 File Offset: 0x00030D54
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					if (this.type == JSONNode.Enumerator.Type.Array)
					{
						return new KeyValuePair<string, JSONNode>(string.Empty, this.m_Array.Current);
					}
					if (this.type == JSONNode.Enumerator.Type.Object)
					{
						return this.m_Object.Current;
					}
					return new KeyValuePair<string, JSONNode>(string.Empty, null);
				}
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x00005871 File Offset: 0x00003A71
			public bool MoveNext()
			{
				if (this.type == JSONNode.Enumerator.Type.Array)
				{
					return this.m_Array.MoveNext();
				}
				return this.type == JSONNode.Enumerator.Type.Object && this.m_Object.MoveNext();
			}

			// Token: 0x0400066D RID: 1645
			private JSONNode.Enumerator.Type type;

			// Token: 0x0400066E RID: 1646
			private Dictionary<string, JSONNode>.Enumerator m_Object;

			// Token: 0x0400066F RID: 1647
			private List<JSONNode>.Enumerator m_Array;

			// Token: 0x020000B9 RID: 185
			private enum Type
			{
				// Token: 0x04000671 RID: 1649
				None,
				// Token: 0x04000672 RID: 1650
				Array,
				// Token: 0x04000673 RID: 1651
				Object
			}
		}

		// Token: 0x020000BA RID: 186
		public struct ValueEnumerator
		{
			// Token: 0x06000586 RID: 1414 RVA: 0x0000589E File Offset: 0x00003A9E
			public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000587 RID: 1415 RVA: 0x000058AC File Offset: 0x00003AAC
			public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x000058BA File Offset: 0x00003ABA
			public ValueEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x06000589 RID: 1417 RVA: 0x00032BA0 File Offset: 0x00030DA0
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x000058C3 File Offset: 0x00003AC3
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x0600058B RID: 1419 RVA: 0x000058D0 File Offset: 0x00003AD0
			public JSONNode.ValueEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000674 RID: 1652
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x020000BB RID: 187
		public struct KeyEnumerator
		{
			// Token: 0x0600058C RID: 1420 RVA: 0x000058D8 File Offset: 0x00003AD8
			public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x0600058D RID: 1421 RVA: 0x000058E6 File Offset: 0x00003AE6
			public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x0600058E RID: 1422 RVA: 0x000058F4 File Offset: 0x00003AF4
			public KeyEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x0600058F RID: 1423 RVA: 0x00032BC0 File Offset: 0x00030DC0
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x000058FD File Offset: 0x00003AFD
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000591 RID: 1425 RVA: 0x0000590A File Offset: 0x00003B0A
			public JSONNode.KeyEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000675 RID: 1653
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x020000BC RID: 188
		public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IEnumerable<KeyValuePair<string, JSONNode>>, IDisposable, IEnumerator, IEnumerable
		{
			// Token: 0x06000592 RID: 1426 RVA: 0x00005912 File Offset: 0x00003B12
			internal LinqEnumerator(JSONNode aNode)
			{
				this.m_Node = aNode;
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000593 RID: 1427 RVA: 0x00005940 File Offset: 0x00003B40
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000594D File Offset: 0x00003B4D
			object IEnumerator.Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x06000595 RID: 1429 RVA: 0x0000595F File Offset: 0x00003B5F
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x0000596C File Offset: 0x00003B6C
			public void Dispose()
			{
				this.m_Node = null;
				this.m_Enumerator = default(JSONNode.Enumerator);
			}

			// Token: 0x06000597 RID: 1431 RVA: 0x00005981 File Offset: 0x00003B81
			public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x0000598E File Offset: 0x00003B8E
			public void Reset()
			{
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x06000599 RID: 1433 RVA: 0x00005981 File Offset: 0x00003B81
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x04000676 RID: 1654
			private JSONNode m_Node;

			// Token: 0x04000677 RID: 1655
			private JSONNode.Enumerator m_Enumerator;
		}
	}
}

using System;

namespace SimpleJSON
{
	// Token: 0x020000B5 RID: 181
	public enum JSONNodeType
	{
		// Token: 0x04000660 RID: 1632
		Array = 1,
		// Token: 0x04000661 RID: 1633
		Object,
		// Token: 0x04000662 RID: 1634
		String,
		// Token: 0x04000663 RID: 1635
		Number,
		// Token: 0x04000664 RID: 1636
		NullValue,
		// Token: 0x04000665 RID: 1637
		Boolean,
		// Token: 0x04000666 RID: 1638
		None,
		// Token: 0x04000667 RID: 1639
		Custom = 255
	}
}

using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C7 RID: 199
	public class JSONNull : JSONNode
	{
		// Token: 0x06000603 RID: 1539 RVA: 0x00005E1A File Offset: 0x0000401A
		public static JSONNull CreateOrGet()
		{
			if (JSONNull.reuseSameInstance)
			{
				return JSONNull.m_StaticInstance;
			}
			return new JSONNull();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00005E2E File Offset: 0x0000402E
		private JSONNull()
		{
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00005E36 File Offset: 0x00004036
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.NullValue;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsNull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00033378 File Offset: 0x00031578
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00005E39 File Offset: 0x00004039
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0000208A File Offset: 0x0000028A
		public override string Value
		{
			get
			{
				return "null";
			}
			set
			{
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x0000208A File Offset: 0x0000028A
		public override bool AsBool
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00005E40 File Offset: 0x00004040
		public override bool Equals(object obj)
		{
			return this == obj || obj is JSONNull;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0000208C File Offset: 0x0000028C
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00005E51 File Offset: 0x00004051
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append("null");
		}

		// Token: 0x04000693 RID: 1683
		private static JSONNull m_StaticInstance = new JSONNull();

		// Token: 0x04000694 RID: 1684
		public static bool reuseSameInstance = true;
	}
}

using System;
using System.Globalization;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C5 RID: 197
	public class JSONNumber : JSONNode
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00005D41 File Offset: 0x00003F41
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Number;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsNumber
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00033378 File Offset: 0x00031578
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00005D44 File Offset: 0x00003F44
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x000333E4 File Offset: 0x000315E4
		public override string Value
		{
			get
			{
				return this.m_Data.ToString(CultureInfo.InvariantCulture);
			}
			set
			{
				double data;
				if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out data))
				{
					this.m_Data = data;
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00005D56 File Offset: 0x00003F56
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00005D5E File Offset: 0x00003F5E
		public override double AsDouble
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00005D67 File Offset: 0x00003F67
		public JSONNumber(double aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00005D76 File Offset: 0x00003F76
		public JSONNumber(string aData)
		{
			this.Value = aData;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00005D85 File Offset: 0x00003F85
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append(this.Value);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0003340C File Offset: 0x0003160C
		private static bool IsNumeric(object value)
		{
			return value is int || value is uint || value is float || value is double || value is decimal || value is long || value is ulong || value is short || value is ushort || value is sbyte || value is byte;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00033474 File Offset: 0x00031674
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (base.Equals(obj))
			{
				return true;
			}
			JSONNumber jsonnumber = obj as JSONNumber;
			if (jsonnumber != null)
			{
				return this.m_Data == jsonnumber.m_Data;
			}
			return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00005D94 File Offset: 0x00003F94
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000691 RID: 1681
		private double m_Data;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C1 RID: 193
	public class JSONObject : JSONNode
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00005BB2 File Offset: 0x00003DB2
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x00005BBA File Offset: 0x00003DBA
		public override bool Inline
		{
			get
			{
				return this.inline;
			}
			set
			{
				this.inline = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00005BC3 File Offset: 0x00003DC3
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.Object;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00005BC6 File Offset: 0x00003DC6
		public override JSONNode.Enumerator GetEnumerator()
		{
			return new JSONNode.Enumerator(this.m_Dict.GetEnumerator());
		}

		// Token: 0x170000A8 RID: 168
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
					return;
				}
				this.m_Dict.Add(aKey, value);
			}
		}

		// Token: 0x170000A9 RID: 169
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (value == null)
				{
					value = JSONNull.CreateOrGet();
				}
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00005C37 File Offset: 0x00003E37
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00033024 File Offset: 0x00031224
		public override void Add(string aKey, JSONNode aItem)
		{
			if (aItem == null)
			{
				aItem = JSONNull.CreateOrGet();
			}
			if (string.IsNullOrEmpty(aKey))
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
				return;
			}
			if (this.m_Dict.ContainsKey(aKey))
			{
				this.m_Dict[aKey] = aItem;
				return;
			}
			this.m_Dict.Add(aKey, aItem);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00005C44 File Offset: 0x00003E44
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return result;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00033094 File Offset: 0x00031294
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000330DC File Offset: 0x000312DC
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = (from k in this.m_Dict
				where k.Value == aNode
				select k).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				result = aNode;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00005C6F File Offset: 0x00003E6F
		public override IEnumerable<JSONNode> Children
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
				{
					yield return keyValuePair.Value;
				}
				Dictionary<string, JSONNode>.Enumerator enumerator = default(Dictionary<string, JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00033148 File Offset: 0x00031348
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('{');
			bool flag = true;
			if (this.inline)
			{
				aMode = JSONTextMode.Compact;
			}
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (!flag)
				{
					aSB.Append(',');
				}
				flag = false;
				if (aMode == JSONTextMode.Indent)
				{
					aSB.AppendLine();
				}
				if (aMode == JSONTextMode.Indent)
				{
					aSB.Append(' ', aIndent + aIndentInc);
				}
				aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
				if (aMode == JSONTextMode.Compact)
				{
					aSB.Append(':');
				}
				else
				{
					aSB.Append(" : ");
				}
				keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
			}
			if (aMode == JSONTextMode.Indent)
			{
				aSB.AppendLine().Append(' ', aIndent);
			}
			aSB.Append('}');
		}

		// Token: 0x04000688 RID: 1672
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x04000689 RID: 1673
		private bool inline;
	}
}

using System;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x020000C4 RID: 196
	public class JSONString : JSONNode
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00005CEF File Offset: 0x00003EEF
		public override JSONNodeType Tag
		{
			get
			{
				return JSONNodeType.String;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0000208F File Offset: 0x0000028F
		public override bool IsString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00033378 File Offset: 0x00031578
		public override JSONNode.Enumerator GetEnumerator()
		{
			return default(JSONNode.Enumerator);
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00005CF2 File Offset: 0x00003EF2
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00005CFA File Offset: 0x00003EFA
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00005D03 File Offset: 0x00003F03
		public JSONString(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00005D12 File Offset: 0x00003F12
		internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
		{
			aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00033390 File Offset: 0x00031590
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				return true;
			}
			string text = obj as string;
			if (text != null)
			{
				return this.m_Data == text;
			}
			JSONString jsonstring = obj as JSONString;
			return jsonstring != null && this.m_Data == jsonstring.m_Data;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00005D34 File Offset: 0x00003F34
		public override int GetHashCode()
		{
			return this.m_Data.GetHashCode();
		}

		// Token: 0x04000690 RID: 1680
		private string m_Data;
	}
}

using System;

namespace SimpleJSON
{
	// Token: 0x020000B6 RID: 182
	public enum JSONTextMode
	{
		// Token: 0x04000669 RID: 1641
		Compact,
		// Token: 0x0400066A RID: 1642
		Indent
	}
}

using System;
using System.Reflection;

namespace RocketJump.Modification
{
	// Token: 0x020000AF RID: 175
	public static class MemberInfoExtension
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x00031644 File Offset: 0x0002F844
		public static object GetValue(this MemberInfo mi, object obj)
		{
			PropertyInfo propertyInfo = mi as PropertyInfo;
			object result;
			if ((result = ((propertyInfo != null) ? propertyInfo.GetValue(obj, null) : null)) == null)
			{
				FieldInfo fieldInfo = mi as FieldInfo;
				if (fieldInfo == null)
				{
					return null;
				}
				result = fieldInfo.GetValue(obj);
			}
			return result;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000054AD File Offset: 0x000036AD
		public static void SetValue(this MemberInfo mi, object obj, object val)
		{
			PropertyInfo propertyInfo = mi as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj, val, null);
			}
			FieldInfo fieldInfo = mi as FieldInfo;
			if (fieldInfo == null)
			{
				return;
			}
			fieldInfo.SetValue(obj, val);
		}
	}
}

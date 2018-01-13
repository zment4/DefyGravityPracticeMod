using System;
using System.Reflection;

namespace RocketJump.Modification
{
	// Token: 0x020000AF RID: 175
	public static class MemberInfoExtension
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x00031528 File Offset: 0x0002F728
		public static MemberInfo GetMemberInfo(Type type, string name)
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
				return MemberInfoExtension.GetMemberInfo(type.BaseType, name);
			}
			return fieldInfo;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000547D File Offset: 0x0000367D
		public static MemberInfo GetMemberInfo(this object obj, string name)
		{
			return MemberInfoExtension.GetMemberInfo(obj.GetType(), name);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0003158C File Offset: 0x0002F78C
		public static T GetValue<T>(this MemberInfo mi, object obj)
		{
			PropertyInfo propertyInfo = mi as PropertyInfo;
			object obj2;
			if ((obj2 = ((propertyInfo != null) ? propertyInfo.GetValue(obj, null) : null)) == null)
			{
				FieldInfo fieldInfo = mi as FieldInfo;
				if (fieldInfo == null)
				{
					return default(T);
				}
				obj2 = fieldInfo.GetValue(obj);
			}
			return (T)((object)obj2);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000315D4 File Offset: 0x0002F7D4
		public static void SetValue<T>(this MemberInfo mi, object obj, T val)
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

		// Token: 0x0600051A RID: 1306 RVA: 0x0000548B File Offset: 0x0000368B
		public static void SetNamedMember<T>(this object obj, string name, T value)
		{
			obj.GetMemberInfo(name).SetValue(obj, value);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000549B File Offset: 0x0000369B
		public static T GetNamedMember<T>(this object obj, string name)
		{
			return obj.GetMemberInfo(name).GetValue(obj);
		}
	}
}

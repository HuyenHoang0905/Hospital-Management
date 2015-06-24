using System;

namespace DevComponents.DotNetBar
{
	[AttributeUsage(AttributeTargets.Property|AttributeTargets.Method|AttributeTargets.Field)]
	public class DevCoBrowsable : Attribute
	{
		public static readonly DevCoBrowsable Yes=new DevCoBrowsable(true);
		public static readonly DevCoBrowsable No=new DevCoBrowsable(false);
		public bool Browsable;
		public DevCoBrowsable(bool browsable)
		{
			this.Browsable = browsable;
		}
	}
}

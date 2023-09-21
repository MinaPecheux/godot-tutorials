using Godot;
using System;

namespace MonoCustomResourceRegistry
{
	[AttributeUsage(System.AttributeTargets.Class)]
	public partial class RegisteredTypeAttribute : System.Attribute
	{
		public string name;
		public string iconPath;
		public string baseType;

		public RegisteredTypeAttribute(string name, string iconPath = "", string baseType = "")
		{
			this.name = name;
			this.iconPath = iconPath;
			this.baseType = baseType;
		}
	}
}
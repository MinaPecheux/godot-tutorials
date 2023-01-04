using Godot;
using Godot.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonoCustomResourceRegistry
{
	public static class Settings
	{
		public enum ResourceSearchType
		{
			Recursive = 0,
			Namespace = 1,
		}

		public static string ClassPrefix => GetSettings(nameof(ClassPrefix)).ToString();
		public static ResourceSearchType SearchType => (ResourceSearchType) GetSettings(nameof(SearchType));
		public static ReadOnlyCollection<string> ResourceScriptDirectories => new ReadOnlyCollection<string>(((Array) GetSettings(nameof(ResourceScriptDirectories))).Cast<string>().ToList());

		public static void Init()
		{
			AddSetting(nameof(ClassPrefix), Variant.Type.String, "");
			AddSetting(nameof(SearchType), Variant.Type.Int, ResourceSearchType.Recursive, PropertyHint.Enum, "Recursive,Namespace");
			AddSetting(nameof(ResourceScriptDirectories), Variant.Type.StringArray, new  Array<string> (new string[]{ "res://"}));
		}

		private static object GetSettings(string title)
		{
			return ProjectSettings.GetSetting($"{nameof(MonoCustomResourceRegistry)}/{title}");
		}

		private static void AddSetting(string title, Variant.Type type, object value, PropertyHint hint = PropertyHint.None, string hintString = "")
		{
			title = SettingPath(title);
			if (!ProjectSettings.HasSetting(title))
				ProjectSettings.SetSetting(title, value);
			var info = new Dictionary
			{
				["name"] = title,
				["type"] = type,
				["hint"] = hint,
				["hint_string"] = hintString,
			};
			ProjectSettings.AddPropertyInfo(info);
		}

		private static string SettingPath(string title) => $"{nameof(MonoCustomResourceRegistry)}/{title}";
	}
}
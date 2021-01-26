// TODO: implement average game time??
namespace CardGames
{
	#region Namespaces
	using System;
	#endregion

	/// <summary>
	/// This class is used to store the preferences
	/// </summary>
	public class Preferences
	{
		#region Private Fields
		private static bool _sound;									// Sound on or off
		private static bool _taskbar;								// Show in taskbar on or off
		#endregion
		
		#region Properties
		/// <summary>
		/// Whether sound is on or off for invalid moves
		/// </summary>
		public static bool SoundEnabled
		{
			get 
			{
				return _sound;
			}
			set
			{
				_sound = value;
			}
		}

		/// <summary>
		/// Whether the program is shown in the taskbar or not
		/// </summary>
		public static bool ShowInTaskbar
		{
			get
			{
				return _taskbar;
			}
			set
			{
				_taskbar = value;
			}
		}
		#endregion

		#region Public Methods
		public static void GetPreferences()
		{
			try
			{
				Microsoft.Win32.RegistryKey key;
				key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Junkosoft\\CardGames", false);
				_sound = bool.Parse(key.GetValue("Sound").ToString().ToLower());
				_taskbar = bool.Parse(key.GetValue("Taskbar").ToString().ToLower());
			}
			catch
			{
				DoPreferencesReset();
			}
		}

		public static void SetPreferences()
		{
			Microsoft.Win32.RegistryKey key;
			key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Junkosoft\\CardGames", true);
			key.SetValue("Sound", _sound);
			key.SetValue("Taskbar", _taskbar);			
		}
		#endregion

		#region Private Methods
		private static void DoPreferencesReset()
		{
			_sound = true;
			_taskbar = true;
			Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Junkosoft\\CardGames");
			SetPreferences();
		}
		#endregion
	}
}
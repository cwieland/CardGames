namespace CardGames.Towers
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
		private static bool _autoMove;								// Auto move from reserve
		#endregion
		
		#region Properties
		/// <summary>
		/// Whether the program is shown in the taskbar or not
		/// </summary>
		public static bool AutoMove
		{
			get
			{
				return _autoMove;
			}
			set
			{
				_autoMove = value;
			}
		}
		#endregion

		#region Public Methods
		public static void GetPreferences()
		{
			try
			{
				Microsoft.Win32.RegistryKey key;
				key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Junkosoft\\CardGames\\Towers", false);
				_autoMove = bool.Parse(key.GetValue("AutoMove").ToString().ToLower());
			}
			catch
			{
				DoPreferencesReset();
			}
		}

		public static void SetPreferences()
		{
			Microsoft.Win32.RegistryKey key;
			key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Junkosoft\\CardGames\\Towers", true);
			key.SetValue("AutoMove", _autoMove);			
		}
		#endregion

		#region Private Methods
		private static void DoPreferencesReset()
		{
			_autoMove = false;
			Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Junkosoft\\CardGames\\Towers");
			SetPreferences();
		}
		#endregion
	}
}
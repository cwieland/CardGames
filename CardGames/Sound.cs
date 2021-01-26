namespace CardGames
{
	#region Namespaces
	using System;
	using System.Runtime.InteropServices;
	#endregion

	#region Sound Class
	public class Sound
	{
		#region External Methods
		// static extern methods for making sound through interop
		[DllImport( "User32.dll", SetLastError = true )]
		public static extern Boolean MessageBeep(Int32 beepType);		
		#endregion
	}
	#endregion

	#region BeepTypes Enumeration
	// Enum for message beep types
	enum BeepTypes
	{ 
		Simple = -1,
		Ok                = 0x00000000,
		IconHand          = 0x00000010,
		IconQuestion      = 0x00000020,
		IconExclamation   = 0x00000030,
		IconAsterisk      = 0x00000040
	}
	#endregion
}
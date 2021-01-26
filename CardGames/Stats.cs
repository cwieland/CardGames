namespace CardGames
{
	#region Namespaces
	using System;
	using System.Text;
	using System.Windows.Forms;
	#endregion

	/// <summary>
	/// This is a class to handle game statistics
	/// </summary>
	public class Stats
	{
		#region Private Fields
		private int _wins;											// The number of wins
		private int _losses;										// The number of losses
		private int _numberofdiscards;								// The number of total discards
		private string _gameName;									// The name of the current game
		#endregion
		
		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameName"></param>
		public Stats(string gameName)
		{
			_gameName = gameName;
			ReadStats();
		}
		#endregion

		#region Properties
		/// <summary>
		/// The number of wins
		/// </summary>
		public int Wins
		{
			get 
			{
				return _wins;
			}
			set 
			{
				_wins = value;
			}
		}

		/// <summary>
		/// The number of losses 
		/// </summary>
		public int Losses
		{
			get 
			{
				return _losses;
			}
			set 
			{
				_losses = value;
			}
		}

		/// <summary>
		/// The total number of cards discared since stats were started
		/// </summary>
		public int NumberOfDiscards
		{
			get
			{
				return _numberofdiscards;
			}
			set
			{
				_numberofdiscards = value;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This reads the statistics from the registry
		/// If no exist, it creates them
		/// </summary>
		public void ReadStats()
		{
			try 
			{
				Microsoft.Win32.RegistryKey key;
				key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Junkosoft\\CardGames\\" + _gameName, false);
				_wins = (int)key.GetValue("Wins");
				_losses = (int)key.GetValue("Losses");
				_numberofdiscards = (int)key.GetValue("NumberOfDiscards");
			}
			catch
			{
				DoStatsReset();
			}
		}

		/// <summary>
		/// This writes the statistics to the registry
		/// </summary>
		public void WriteStats()
		{
			Microsoft.Win32.RegistryKey key;
			key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Junkosoft\\CardGames\\" + _gameName);
			key.SetValue("Wins", _wins);
			key.SetValue("Losses", _losses);
			key.SetValue("NumberOfDiscards", _numberofdiscards);
		}

		/// <summary>
		/// This resets the statistics
		/// </summary>
		public void ResetStats()
		{
			DialogResult result = MessageBox.Show("Are you sure?", "Reset Statstics", MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			if (result == DialogResult.Yes)
				DoStatsReset();
		}

		/// <summary>
		/// This brings up a dialog box showing the Statistics
		/// </summary>
		public void ViewStats() 
		{
			StringBuilder statsMessage = new StringBuilder();
			statsMessage.Append("Wins:\t\t");
			statsMessage.Append(_wins);
			statsMessage.Append("\nLosses:\t\t");
			statsMessage.Append(_losses);

			statsMessage.Append("\n\nWin Percentage:\t");
			float winPercentage = 0;
			if (_wins + _losses > 0)
				winPercentage = (float)(_wins * 100) / (float)(_wins + _losses);
			statsMessage.Append(winPercentage.ToString("N2") + "%");
			
			statsMessage.Append("\n\n An average of ");
			float averageDiscards = 0;
			if (_wins + _losses > 0)
				averageDiscards = (float)_numberofdiscards / (float)(_wins + _losses);
			statsMessage.Append(averageDiscards.ToString("N2"));
			statsMessage.Append(" cards played per hand.");

			MessageBox.Show(statsMessage.ToString(), _gameName + " Statistics", MessageBoxButtons.OK);
		}

		/// <summary>
		/// Show Resign Game confirm dialog
		/// </summary>
		/// <returns></returns>
		public DialogResult ResignGame()
		{
			return MessageBox.Show("Resign this game?", _gameName, MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}

		/// <summary>
		/// Show a Win hand / another game confirm dialog
		/// </summary>
		/// <returns></returns>
		public DialogResult AnotherHand()
		{
			return MessageBox.Show("Congratulations!\nYou have won this hand!\n\nDo you want to play another hand?",
				_gameName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}
		#endregion
		
		#region Private Methods
		/// <summary>
		/// Does the statistics reset without a dialog
		/// </summary>
		private void DoStatsReset()
		{
			_wins = 0;
			_losses = 0;
			_numberofdiscards = 0;
			WriteStats();
		}
		#endregion
	}
}

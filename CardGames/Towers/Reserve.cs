namespace CardGames.Towers
{
	#region Namespaces
	using System;
	#endregion

	/// <summary>
	/// This class represents the reserve cells (the four free card spots).
	/// </summary>
	public class Reserve
	{
		#region Private Fields
		private int[] reserve;
		#endregion

		#region Properties
		public int this[int index]
		{
			get 
			{
				return reserve[index];
			}
			set
			{
				reserve[index] = value;
			}
		}
		#endregion

		#region Constructor
		public Reserve()
		{
			// Initialize the data structures
			reserve = new int[4];
			for (int count = 0; count < 4; count++)
			{
				reserve[count] = -1;
			}
		}
		#endregion

		#region Public Methods
		public bool Empty(int index)
		{
			return reserve[index] == -1;
		}

		public void RemoveCard(int index)
		{
			reserve[index] = -1;
		}
		#endregion
	}
}

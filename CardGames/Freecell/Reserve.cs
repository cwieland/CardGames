namespace CardGames.Freecell
{
	#region Namespaces
	using System;
	#endregion

	/// <summary>
	/// This class represents the freecell reserve (the four free card spots).
	/// </summary>
	public class FreecellReserve
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
		public FreecellReserve()
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

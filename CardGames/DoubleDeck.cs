using System;

namespace CardGames
{
	/// <summary>
	/// This represents two decks of cards (standard 52)
	/// </summary>
	public class DoubleDeck
	{
		#region Private Fields
		private int[] CardArray = new int[104];
		#endregion

		#region Properties
		public int this[int index]
		{
			get
			{
				if (index >= 0 && index <= 103)
					return CardArray[index] % 52;
				else
					throw (new System.ArgumentOutOfRangeException("index", index,
						"Value must be between 0 and 103."));
			}
			set
			{
				CardArray[index] = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes deck with the 52 integers.
		/// </summary>
		public DoubleDeck()
		{
			// Deck uses RankCollated cards 0 - 104
			for( int i = 0; i < 104; i++ )
			{
				CardArray[i] = i;
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Randomly rearrange integers
		/// </summary>
		public void Shuffle()
		{
			int[] newArray = new int[104];
			bool[] used = new bool[104];

			for( int j = 0; j < 104; j++ )
			{
				used[j] = false;
			}
    
			Random rnd = new Random();
			int iCount = 0;
			int iNum;

			while( iCount < 104 )
			{
				iNum = rnd.Next( 0, 104 ); // between 0 and 103

				if( used[iNum] == false )
				{
					newArray[iCount] = iNum;
					used[iNum] = true;
					iCount++;
				}
			}

			// Load original array with shuffled array
			CardArray = newArray;
		}    
		#endregion
	}
}

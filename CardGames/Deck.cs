namespace CardGames
{
	#region Namespaces
	using System;
	#endregion

	/// <summary>
	/// This represents the deck of cards (standard 52)
	/// </summary>
	public class Deck
	{
		#region Private Fields
		private int[] CardArray = new int[52];
		#endregion

		#region Properties
		public int this[int index]
		{
			get
			{
				if (index >= 0 && index <= 51)
					return CardArray[index];
				else
					throw (new System.ArgumentOutOfRangeException("index", index,
						"Value must be between 0 and 51."));
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
		public Deck()
		{
			// Deck uses RankCollated cards 0 - 51
			for( int i = 0; i < 52; i++ )
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
			int[] newArray = new int[52];
			bool[] used = new bool[52];

			for( int j = 0; j < 52; j++ )
			{
				used[j] = false;
			}
    
			Random rnd = new Random();
			int iCount = 0;
			int iNum;

			while( iCount < 52 )
			{
				iNum = rnd.Next( 0, 52 ); // between 0 and 51

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

namespace CardGames
{
	#region Namespaces
	using System;
	using System.Collections.Specialized;
	#endregion

	/// <summary>
	/// This class represents the foundation for several solitare games.
	/// </summary>
	public class Foundation
	{	
		#region Private Fields
		private CardCollection[] FoundationHands;
		#endregion

		#region Properties
		public CardCollection this[int column]
		{
			get 
			{
				return FoundationHands[column];
			}
		}
		#endregion

		#region Constructors
		public Foundation()
		{
			FoundationHands = new CardCollection[4];
			for (int count = 0; count < 4; count++)
			{
				FoundationHands[count] = new CardCollection();
			}
		}
		#endregion

		#region Public Methods
		public int Count(int column) 
		{
			return FoundationHands[column].Count;
		}
		#endregion
	}
}

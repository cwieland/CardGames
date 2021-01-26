namespace CardGames.Freecell
{
	#region Namespaces
	using System;
	using System.Collections;
	#endregion

	/// <summary>
	/// This class represents the Tableau for Freecell
	/// </summary>
	public class FreecellTableau
	{	
		#region Private Fields
		private CardCollection[] TableauRow;
		#endregion

		#region Properties
		public CardCollection this[int index] 
		{
			get
			{
				return TableauRow[index];
			}
		}
		#endregion

		#region Constructors
		public FreecellTableau()
		{
			TableauRow = new CardCollection[8];
			for (int count = 0; count < 8; count++)
				TableauRow[count] = new CardCollection();
		}
		#endregion
	}
}

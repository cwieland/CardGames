namespace CardGames
{
	#region Namespaces
	using System;
	using System.Collections;
	#endregion

	/// <summary>
	/// This is a collection of cards that represents the player's hand
	/// </summary>
	public class CardCollection : CollectionBase
	{
		#region Constructors
		public CardCollection()
		{
			this.List.Clear();
		}
		#endregion
		
		#region Properties
		public int this[int index]
		{
			get
			{
				return (int)this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}
		#endregion

		#region Public Methods
		public int Add(int theCard) 
		{
			return this.List.Add(theCard); 
		}

		public void Remove(int index)
		{
			this.List.Remove(index);
		}

		public void RemoveTopCard()
		{
			this.List.RemoveAt(this.List.Count-1);
		}

		public int GetTopCard()
		{
			return this[this.Count-1];
		}
		#endregion
	}
}

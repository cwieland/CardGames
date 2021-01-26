// Osmosis task list:
//  Fix highlighted card bug (on refresh)

namespace CardGames.Osmosis
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Text;
	using System.Windows.Forms;
	#endregion

	/// <summary>
	/// This is a simple implementation of the solitare game Osmosis.
	/// </summary>
	public class Osmosis : System.Windows.Forms.Form
	{
		#region Constants
		private const int TopY = 20;								// The top most position of Reserve and Foundation piles
		private const int ReserveX = 20;							// The left most position of the Reserve piles
		private const int ReserveWidth = 5;							// The width of the cards displayed in the Reserve piles
		private const int FoundationX = 125;						// The left most position of the Reserve piles
		private const int FoundationWidth = 15;						// The width of the cards displayed in the Foundation piles
		private const int PileHeight = 105;							// The height of the cards displayed in the Foundation and Reserve
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		private const int StockX = 420;								// The left most position of the Stock pile
		private const int StockY = 20;								// The top most position of the Stock pile
		private const int WasteX = 500;								// The left most position of the Waste pile
		private const int WasteWidth = 15;							// The width of the cards in the waste pile
		#endregion

		#region Private Fields
		private Foundation theFoundation;							// The data structure for the foundation
		private CardCollection[] theReserve;						// The data structure for the reserve
		private CardCollection theStock;							// The data structure for the stock pile
		private CardCollection theWaste;							// The data structure for the waste pile
		private Deck theDeck;										// The data structure for the deck of cards
		private int selectedCard;									// The currently selected card
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private CardBack currentCardBack;							// The selected card back
		private CardRank foundationBaseRank;						// The rank of the foundation base cards
		private int cardsPlayed;									// The number of cards played this hand
		#endregion

		#region Controls
		private System.ComponentModel.Container components = null;	// Required designer variable
		private System.Windows.Forms.MainMenu UserMenu;				// The form's menu
		private System.Windows.Forms.MenuItem File;					// The File menu
		private System.Windows.Forms.MenuItem New;					// The New Game item
		private System.Windows.Forms.MenuItem FileSeperator;		// The file seperator
		private System.Windows.Forms.MenuItem Exit;					// The Exit menu item
		private System.Windows.Forms.MenuItem ResetStats;			// The Reset stats menu item
		private System.Windows.Forms.MenuItem ViewStats;			// The View stats menu item
		private System.Windows.Forms.MenuItem OptionsSeperator;		// The Options Seperator
		private System.Windows.Forms.MenuItem ChangeCardBack;			// The Preferences menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public Osmosis()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			theStock = new CardCollection();
			theWaste = new CardCollection();
			theReserve = new CardCollection[4];
			theFoundation = new Foundation();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			currentCardBack = CardBack.Weave1;
			for (int count = 0; count < 4; count++)
				theReserve[count] = new CardCollection();

			NewHand();
		}
		#endregion
	
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.UserMenu = new System.Windows.Forms.MainMenu();
			this.File = new System.Windows.Forms.MenuItem();
			this.New = new System.Windows.Forms.MenuItem();
			this.FileSeperator = new System.Windows.Forms.MenuItem();
			this.Exit = new System.Windows.Forms.MenuItem();
			this.Options = new System.Windows.Forms.MenuItem();
			this.ViewStats = new System.Windows.Forms.MenuItem();
			this.ResetStats = new System.Windows.Forms.MenuItem();
			this.OptionsSeperator = new System.Windows.Forms.MenuItem();
			this.ChangeCardBack = new System.Windows.Forms.MenuItem();
			// 
			// UserMenu
			// 
			this.UserMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.File,
																					 this.Options});
			// 
			// File
			// 
			this.File.Index = 0;
			this.File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.New,
																				 this.FileSeperator,
																				 this.Exit});
			this.File.Text = "&File";
			// 
			// New
			// 
			this.New.Index = 0;
			this.New.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.New.Text = "&New Game";
			this.New.Click += new System.EventHandler(this.New_Click);
			// 
			// FileSeperator
			// 
			this.FileSeperator.Index = 1;
			this.FileSeperator.Text = "-";
			// 
			// Exit
			// 
			this.Exit.Index = 2;
			this.Exit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.Exit.Text = "E&xit";
			this.Exit.Click += new System.EventHandler(this.Exit_Click);
			// 
			// Options
			// 
			this.Options.Index = 1;
			this.Options.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.ViewStats,
																					this.ResetStats,
																					this.OptionsSeperator,
																					this.ChangeCardBack});
			this.Options.Text = "&Options";
			// 
			// ViewStats
			// 
			this.ViewStats.Index = 0;
			this.ViewStats.Text = "&View Statstics";
			this.ViewStats.Click += new System.EventHandler(this.ViewStats_Click);
			// 
			// ResetStats
			// 
			this.ResetStats.Index = 1;
			this.ResetStats.Text = "&Reset Statstics";
			this.ResetStats.Click += new System.EventHandler(this.ResetStats_Click);
			// 
			// OptionsSeperator
			// 
			this.OptionsSeperator.Index = 2;
			this.OptionsSeperator.Text = "-";
			// 
			// ChangeCardBack
			// 
			this.ChangeCardBack.Index = 3;
			this.ChangeCardBack.Text = "&Change Card Back";
			this.ChangeCardBack.Click += new System.EventHandler(this.ChangeCardBack_Click);
			// 
			// Osmosis
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(615, 452);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Osmosis";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Osmosis";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Osmosis_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Osmosis_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Osmosis_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles any mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Osmosis_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;
			
			for (int column = 0; column < 10; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;
				int right = 0;
			
				if (column < 4)
				{
					left = ReserveX;							
					top = TopY + PileHeight * column;
					bottom = top + CardHeight;
					right = left + CardWidth;

					if (theReserve[column].Count > 0)
						left += ReserveWidth * (theReserve[column].Count - 1);
				}
				else if (column < 8)
				{
					left = FoundationX;
					top = TopY + PileHeight * (column - 4);
					bottom = top + CardHeight;
					right = left + CardWidth + FoundationWidth * (theFoundation[column - 4].Count - 2);
				}
				else if (column == 8)
				{
					left = StockX;
					top = StockY;
					bottom = top + CardHeight;
					right = left + CardWidth;
				}
				else
				{
					left = WasteX;
					top = StockY;

					int stacksize = 3;
					if (theWaste.Count < stacksize)
						stacksize = theWaste.Count;
					
					if (stacksize > 0)
						left += (stacksize - 1) * WasteWidth;

					bottom = top + CardHeight;
					right = left + CardWidth;
				}				
							
				if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
				{
					if (oldSelection == column)
					{
						if (e.Button == MouseButtons.Left) 
						{
							selectedCard = -1;
							ToggleCardSelect(column, false);
						}
					}
					else if (oldSelection == -1)
					{
						if (e.Button == MouseButtons.Left) 
						{
							if (column == 8)
							{
								DealStock();
							}
							else
							{
								bool selectFromReserve = (column < 4 && theReserve[column].Count > 0);
								bool selectFromWaste = (column == 9 && theWaste.Count > 0);
								if (selectFromReserve || selectFromWaste)
								{
									selectedCard = column;
									ToggleCardSelect(column, true);
								}
							}
						} 
						else if (e.Button == MouseButtons.Right)
						{
							// TODO: implement...Move the card to the foundation
						}
					}
					else
					{
						if (e.Button == MouseButtons.Left)
							AttemptMove(oldSelection, column);
					}
				}
			}
		}

		/// <summary>
		/// Draw the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Osmosis_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Osmosis_Closing(object sender, CancelEventArgs e)
		{
			if (!finished)
			{
				if (theStats.ResignGame() == DialogResult.Yes)
				{
					theStats.Losses++;
					theStats.NumberOfDiscards += cardsPlayed;
					theStats.WriteStats();
				}
				else
				{
					e.Cancel = true;
				}
			}
		}
		#endregion

		#region Control Events
		/// <summary>
		/// Starts a new hand
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void New_Click(object sender, EventArgs e)
		{
			if (!finished)
			{
				if (theStats.ResignGame() == DialogResult.Yes)
				{
					theStats.Losses++;
					theStats.NumberOfDiscards += cardsPlayed;
					theStats.WriteStats();
					NewHand();
				}
			}
			else
			{
				NewHand();
			}
		}

		/// <summary>
		/// Exits the game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Exit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Change the selected Card back
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeCardBack_Click(object sender, System.EventArgs e)
		{
			currentCardBack++;
			if (currentCardBack == CardBack.Unused)
				currentCardBack = CardBack.Crosshatch;

			Graphics theGraphics = this.CreateGraphics();
			drawCard.Begin(theGraphics);

			// Draw the Reserve piles
			for (int column = 0; column < 4; column++)
			{
				if (theReserve[column].Count > 0)
				{
					for (int count = 0; count < theReserve[column].Count-1; count++)
						drawCard.DrawCardBack(new Point(ReserveX + ReserveWidth * count, TopY + PileHeight * column),
							currentCardBack);
					drawCard.DrawCard(new Point(ReserveX + ReserveWidth * (theReserve[column].Count - 1),
						TopY + PileHeight * column), theReserve[column].GetTopCard());
				}
			}
			
			// Draw the stock pile
			if (theStock.Count > 0)
			{
				int stacksize = 3;
				if (theStock.Count < stacksize)
					stacksize = theStock.Count;
				for (int count = 0; count < stacksize; count++) 
					drawCard.DrawCardBack(new Point(StockX+count, StockY+count), currentCardBack);
			}

			drawCard.End();
		}

		/// <summary>
		/// Resets the Win / Loss stats
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ResetStats_Click(object sender, System.EventArgs e)
		{
			theStats.ResetStats();
		}

		/// <summary>
		/// This shows a dialog with wins, losses and win percentage
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ViewStats_Click(object sender, System.EventArgs e)
		{
			theStats.ViewStats();
		}
		#endregion

		#region Protected Methods
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Private Methods
		private void NewHand()
		{
			// Shuffle the deck
			theDeck.Shuffle();
			selectedCard = -1;
			cardsPlayed = 0;
			finished = true;

			// Clear the stock and waste piles
			theStock.Clear();
			theWaste.Clear();

			for (int row = 0; row < 4; row++)
			{
				// Clear the Foundation and Reserve
				theFoundation[row].Clear();
				theReserve[row].Clear();

				// Get the cards for the reserve piles
				for (int cardCount = 0; cardCount < 4; cardCount++)
					theReserve[row].Add(theDeck[cardCount + row * 4]);
			}

			// Get the first card of the foundation
			theFoundation[0].Add(theDeck[16]);
			foundationBaseRank = Card.RankFromCardIndex(theDeck[16]);
			cardsPlayed++;

			// Fill the stock pile
			for (int count = 51; count > 16; count--)
				theStock.Add(theDeck[count]);

			// Draw the screen
			this.Refresh();
		}

		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			if (newPosition > 3 && newPosition < 8) 
			{
				if (newPosition == 4)
				{
					// Adding another card of the same suit
					CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[0].GetTopCard());
					CardSuit newSuit;
					if (oldPosition < 4)
						newSuit = Card.SuitFromCardIndex(theReserve[oldPosition].GetTopCard());
					else
						newSuit = Card.SuitFromCardIndex(theWaste.GetTopCard());

					if (baseSuit == newSuit)
					{
						MoveCard(oldPosition, newPosition);
						CheckGameOver();
					}
					else
						InvalidMove(oldPosition);
				}
				else
				{
					// Adding a new foundation base card
					if (theFoundation[newPosition - 4].Count == 0)
					{
						CardRank newRank;
						if (oldPosition < 4)
							newRank = Card.RankFromCardIndex(theReserve[oldPosition].GetTopCard());
						else
							newRank = Card.RankFromCardIndex(theWaste.GetTopCard());

						if (newRank == foundationBaseRank)
							MoveCard(oldPosition, newPosition);
						else
							InvalidMove(oldPosition);
					}
					else
					{
						// Adding another card of the same suit
						CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
						CardSuit newSuit;
						CardRank newRank;
						if (oldPosition < 4)
						{
							newSuit = Card.SuitFromCardIndex(theReserve[oldPosition].GetTopCard());
							newRank = Card.RankFromCardIndex(theReserve[oldPosition].GetTopCard());
						}
						else
						{
							newSuit = Card.SuitFromCardIndex(theWaste.GetTopCard());
							newRank = Card.RankFromCardIndex(theWaste.GetTopCard());
						}

						bool RankInPreviousRow = false;
						for (int count = 0; (count < theFoundation[newPosition - 5].Count && !RankInPreviousRow); count++)
						{
							CardRank baseRank = Card.RankFromCardIndex(theFoundation[newPosition - 5][count]);
                            RankInPreviousRow = baseRank == newRank;
						}

						if (baseSuit == newSuit && RankInPreviousRow)
						{
							MoveCard(oldPosition, newPosition);
							CheckGameOver();
						}
						else
							InvalidMove(oldPosition);						// Check card suit and see if rank is in above row
					}
				}
			}
			else
			{
				InvalidMove(oldPosition);
			}

			selectedCard = -1;
			CheckGameOver();

		}

		/// <summary>
		/// Draws the Cards
		/// </summary>
		private void DrawCards()
		{
			drawCard.Begin(this.CreateGraphics());
			for (int column = 0; column < 4; column++)
			{
				if (theFoundation[column].Count > 0)
				{
					for (int count = 0; count < theFoundation[column].Count; count++)
					{
						drawCard.DrawCard(new Point(FoundationX + FoundationWidth * count, TopY +
							PileHeight * column), theFoundation[column][count]);
					}
				}
				else
				{
					drawCard.DrawCardBack(new Point(FoundationX, TopY + PileHeight * column), CardBack.O);
				}

				if (theReserve[column].Count > 0)
				{
					for (int count = 0; count < theReserve[column].Count-1; count++)
					{
						drawCard.DrawCardBack(new Point(ReserveX + ReserveWidth * count, TopY + PileHeight * column),
							currentCardBack);
					}

					drawCard.DrawCard(new Point(ReserveX + ReserveWidth * (theReserve[column].Count - 1),
						TopY + PileHeight * column), theReserve[column].GetTopCard());

				}
				else
				{
					drawCard.DrawCardBack(new Point(ReserveX, TopY + PileHeight * column), CardBack.Unused);
				}
			}

			DrawStockAndWaste();
			drawCard.End();
		}

		/// <summary>
		/// Draws the stock and waste piles
		/// </summary>
		private void DrawStockAndWaste()
		{
			// Erase the card areas
			Rectangle oldCardArea = new Rectangle(StockX, StockY, CardWidth+3, CardHeight+3);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			oldCardArea = new Rectangle(WasteX, StockY, CardWidth+WasteWidth*3, CardHeight);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			
			// Draw the stock pile
			if (theStock.Count > 0)
			{
				int stacksize = 3;
				if (theStock.Count < stacksize)
					stacksize = theStock.Count;
				for (int count = 0; count < stacksize; count++) 
					drawCard.DrawCardBack(new Point(StockX+count, StockY+count), currentCardBack);
			}
			else
				drawCard.DrawCardBack(new Point(StockX, StockY), CardBack.X);

			// Draw the waste pile
			if (theWaste.Count > 0)
			{
				int stacksize = 3;
				if (theWaste.Count < stacksize)
					stacksize = theWaste.Count;

				for (int count = 0; count < stacksize; count++)
				{
					drawCard.DrawCard(new Point(WasteX + WasteWidth * count, StockY),
						theWaste[theWaste.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(WasteX, StockY), CardBack.Unused);
			}
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="column">Column of the card (0-4 wastepile, 5 stock pile)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int column, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();

			if (column < 4)
			{
				cardLocation.X = ReserveX;
				cardLocation.Y = TopY + PileHeight * column;

				if (theReserve[column].Count > 0)
					 cardLocation.X += ReserveWidth * (theReserve[column].Count - 1);
							
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theReserve[column].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theReserve[column].GetTopCard());
			}
			else if (column == 9)
			{
				int stacksize = 3;
				if (theWaste.Count < stacksize)
					stacksize = theWaste.Count;

				cardLocation.X = WasteX;
				cardLocation.Y = StockY;

				if (stacksize > 0)
					 cardLocation.X += WasteWidth * (stacksize - 1);

				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theWaste.GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theWaste.GetTopCard());
			}

			drawCard.End();
		}

		/// <summary>
		/// Does a card move
		/// </summary>
		/// <param name="oldPosition">Old position of card</param>
		/// <param name="newPosition">New position of card</param>		
		private void MoveCard(int oldPosition, int newPosition)
		{
			int movingCard = 52;

			if (oldPosition < 4)
				movingCard = theReserve[oldPosition].GetTopCard();
			else if (oldPosition == 9)
				movingCard = theWaste.GetTopCard();

			// add it to new location if it is valid
			if (movingCard < 52)
			{
				if (newPosition > 3 && newPosition < 8)
				{
					theFoundation[newPosition - 4].Add(movingCard);
					drawCard.Begin(this.CreateGraphics());
					drawCard.DrawCard(new Point(FoundationX + FoundationWidth * (theFoundation[newPosition - 4].Count - 1),
						TopY + PileHeight * (newPosition - 4)), theFoundation[newPosition - 4].GetTopCard());
					drawCard.End();
					cardsPlayed++;
				}

				// remove it from old location
				RemoveCard(oldPosition);
			}
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column"></param>
		private void RemoveCard(int column)
		{
			if (column < 4) 
			{
				int x = ReserveX;
				if (theReserve[column].Count > 0)
					x += (theReserve[column].Count - 1) * ReserveWidth;
				int y = TopY + column * PileHeight;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			
				theReserve[column].RemoveTopCard();
				drawCard.Begin(this.CreateGraphics());
			
				if (theReserve[column].Count == 0)
					drawCard.DrawCardBack(new Point(ReserveX, TopY + column * PileHeight), CardBack.Unused);
				else
					drawCard.DrawCard(new Point(ReserveX + (theReserve[column].Count - 1) * ReserveWidth,
						TopY + column * PileHeight), theReserve[column].GetTopCard());
				drawCard.End();
			}
			else if (column == 9)
			{
				theWaste.RemoveTopCard();
				drawCard.Begin(this.CreateGraphics());
				DrawStockAndWaste();
				drawCard.End();			
			}
		}

		/// <summary>
		/// Deals cards from the stock pile to the waste pile
		/// </summary>
		private void DealStock()
		{
			if (theStock.Count > 0)
			{
				// determine the number of cards to deal
				int numbertodeal = 3;
				if (theStock.Count < numbertodeal)
					numbertodeal = theStock.Count;

				// add the cards to the waste pile and remove them from the stock
				for (int count = 0; count < numbertodeal; count++)
				{
					theWaste.Add(theStock.GetTopCard());
					theStock.RemoveTopCard();
				}
			}
			else
			{
				// Move the cards back to the stock pile
				for (int count = theWaste.Count - 1; count >= 0; count--)
				{
					theStock.Add(theWaste.GetTopCard());
					theWaste.RemoveTopCard();
				}
			}

			drawCard.Begin(this.CreateGraphics());
			DrawStockAndWaste();
			drawCard.End();
		}

		/// <summary>
		/// Determines whether the game is won or not
		/// </summary>
		private void CheckGameOver()
		{
			bool gameover = true;
			for (int count = 0; count < 4 && gameover; count++)
			{
				gameover = theFoundation[count].Count == 13;
			}
			
			if (gameover)
			{
				theStats.Wins++;
				theStats.NumberOfDiscards += cardsPlayed;
				theStats.WriteStats();
				finished = true;
				if (theStats.AnotherHand() == DialogResult.Yes)
					NewHand();
			}
		}

		/// <summary>
		/// Code to handle a invalid move
		/// </summary>
		/// <param name="position">position of the card</param>
		private void InvalidMove(int position)
		{
			if (CardGames.Preferences.SoundEnabled)
				Sound.MessageBeep((Int32)BeepTypes.Ok);
			ToggleCardSelect(position, false);
		}
		#endregion

		#region Debug Methods
		private void _DEBUG_FillFoundation(int column)
		{
			theFoundation[column].Clear();
			for (int count = 0; count < 13; count++)
				theFoundation[column].Add(Card.ToCardIndex(CardSuit.Hearts, (CardRank)count));
		}
		#endregion
	}
}
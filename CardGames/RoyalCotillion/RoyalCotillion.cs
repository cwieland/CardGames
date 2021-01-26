// Royal Cotillion task list:
//  Implement card selection on the left tableau (ladies' side)
//  Implement card moving logic (all to foundation, waste/stock to right tableau)
//  Implement automatic moving of cards from waste/stock piles to right tableau
//  Fix highlighted card bug in waste pile (on refresh)

namespace CardGames.RoyalCotillion
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
	/// This is a simple implementation of the solitare game Royal Cotillion.
	/// </summary>
	public class RoyalCotillion : System.Windows.Forms.Form
	{
		#region Constants
		private const int TopY = 10;								// The top most position of Tableau and Foundation piles
		private const int FoundationX = 321;						// The left most position of the Reserve piles
		private const int DrawingHeight = 110;						// The height of the cards displayed in the Foundation piles
		private const int TableauLeftX = 15;						// The left most position of the left tableau
		private const int TableauRightX = 484;						// The left most position of the right tableau
		private const int TableauWidth = 74;						// The width of the cards in the left and right tableaux
		private const int StockX = 84;								// The left most position of the stock pile
		private const int StockY = 340;								// The top most position of the stock pile
		private const int WasteX = 168;								// The top most position of the waste pile
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private Foundation[] theFoundation;							// The data structure for the foundation
		private int[] theLeftTableau;								// The data structure for the left tableau, "ladies' side"
		private int[] theRightTableau;								// The data structure for the right tableau, "lord's side"
		private DoubleDeck theDeck;									// The data structure for the deck of cards
		private CardCollection theStock;							// The data structure for the stock pile
		private CardCollection theWaste;							// The data structure for the waste pile
		private int selectedCard;									// The currently selected card
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
		private CardBack currentCardBack;							// The selected card back
		private Stats theStats;										// The game statistics
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
		private System.Windows.Forms.MenuItem ViewStats;
		private System.Windows.Forms.MenuItem OptionsSeperator;
		private System.Windows.Forms.MenuItem ChangeCardBack;			// The Preferences menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public RoyalCotillion()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new DoubleDeck();
			theLeftTableau = new int[12];
			theRightTableau = new int[16];
			theFoundation = new Foundation[2];
			theStock = new CardCollection();
			theWaste = new CardCollection();
			drawCard = new Card();
			currentCardBack = CardBack.Weave1;
			theStats = new Stats(this.Name);
			for (int index = 0; index < 2; index++)
			{
				theFoundation[index] = new Foundation();
			}

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
			// RoyalCotillion
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(794, 457);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "RoyalCotillion";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Royal Cotillion";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RoyalCotillion_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RoyalCotillion_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.RoyalCotillion_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles any mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RoyalCotillion_MouseDown(object sender, MouseEventArgs e)
		{
			int oldSelection = selectedCard;
			
			for (int column = 0; column < 38; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;
				int right = 0;

				if (column < 12)
				{
					// Check cards on the "ladies' side" (left tableau)
					left = TableauLeftX + TableauWidth * (column % 4);
					top = TopY + DrawingHeight * (column / 4);
				}
				else if (column < 28)
				{
					// Check cards on the "lord's side" (right tableau)
					left = TableauRightX + TableauWidth * ((column - 12) % 4);
					top = TopY + DrawingHeight * ((column - 12) / 4);
				}
				else if (column < 32)
				{
					// Check pile in first foundation column (ace -> king)
					left = FoundationX;
					top = TopY + DrawingHeight * (column - 28);
				}
				else if (column < 36)
				{
					// Check pile in second foundation column (two -> queen)
					left = FoundationX + CardWidth;
					top = TopY + DrawingHeight * (column - 32);
				}
				else if (column == 36)
				{
					// Check the stock pile
					int stacksize = 3;
					if (theStock.Count < stacksize)
						stacksize = theStock.Count;
					left = StockX + stacksize;
					top = StockY + stacksize;
				}
				else
				{
					// Check the waste pile			
					int stacksize = 3;
					if (theWaste.Count < stacksize)
						stacksize = theWaste.Count;
					left = WasteX + stacksize;
					top = StockY + stacksize;
				}

				bottom = top + CardHeight;
				right = left + CardWidth;

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
							if (column == 36)
							{
								if (theStock.Count > 0)
								{ 
									DealStock();
								}
								else
								{
									InvalidMove(column);
								}
							}
							else
							{
								int[] bottomCard = {-1,-1,-1,-1};
								for (int x = 11; x >= 0; x--)
								{
									if (bottomCard[x%4] == -1 && theLeftTableau[x] > -1)
									{
										bottomCard[x%4] = x;
									}
								}
								bool selectFromLeftTableau = (column == bottomCard[0] || column == bottomCard[1] ||
									column == bottomCard[2] || column == bottomCard[3]);
								bool selectFromRightTableau = (column > 11 && column < 28 &&
									theRightTableau[column - 12] > - 1);
								bool selectFromWaste = (column == 37 && theWaste.Count > 0);
								if (selectFromLeftTableau || selectFromRightTableau || selectFromWaste)
								{
									selectedCard = column;
									ToggleCardSelect(column, true);
								}
								else
								{
									InvalidMove(column);
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
						{
							AttemptMove(oldSelection, column);
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RoyalCotillion_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RoyalCotillion_Closing(object sender, CancelEventArgs e)
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
		/// Exits the game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Exit_Click(object sender, EventArgs e)
		{
			this.Close();
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
			theStock.Clear();
			theWaste.Clear();
			selectedCard = -1;
			cardsPlayed = 0;
			finished = true;

			// Clear the Foundation and Reserve
			for (int index = 0; index < 2; index++)
			{
				for (int row = 0; row < 4; row++)
				{
					theFoundation[index][row].Clear();
				}
			}
			
			// Deal the first twelve cards onto the ladies' side (left)
			for (int index = 0; index < 12; index++)
			{
				theLeftTableau[index] = theDeck[index];
			}

			// Deal the next sixteen cards onto the lord's side (right)
			for (int index = 0; index < 16; index++)
			{
				theRightTableau[index] = theDeck[index + 12];
			}

			// Deal the rest into the stock pile (in original order)
			for (int x = 103; x > 27; x--)
			{
				theStock.Add(theDeck[x]);
			}

			// Draw the screen
			this.Refresh();
		}

		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			// Moving card to left foundation piles
			if (newPosition >= 28 && newPosition <= 31)
			{
				// TODO: move to left foundation if valid
				if (oldPosition < 12)
				{
					// TODO: move from left tableau
					if (theFoundation[0][newPosition-28].Count > 0)
					{
						CardRank sourceRank = Card.RankFromCardIndex(theLeftTableau[oldPosition]);
						CardRank targetRank = Card.RankFromCardIndex(theFoundation[0][newPosition-28].GetTopCard());

						switch (targetRank)
						{
							case CardRank.Queen:
								targetRank = CardRank.Ace;
								break;

							case CardRank.King:
								targetRank = CardRank.Two;
								break;

							default:
								targetRank = targetRank += 2;
								break;
						}

						if (sourceRank == targetRank)
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}

					}
					else if (Card.RankFromCardIndex(theLeftTableau[oldPosition]) == CardRank.Ace)
					{
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
				else if (oldPosition >= 12 && oldPosition <= 27)
				{
					// TODO: move from right tableau
				}
				else if (oldPosition == 37)
				{
					// TODO: move from waste pile
				}
			}
			else if (newPosition >= 32 && newPosition <= 35)
			{
				// TODO: move to right foundation if valid
				if (oldPosition < 12)
				{
					// TODO: move from left tableau
				}
				else if (oldPosition >= 12 && oldPosition <= 27)
				{
					// TODO: move from right tableau
				}
				else if (oldPosition == 37)
				{
					// TODO: move from waste pile
				}
			}
			else
			{
				InvalidMove(oldPosition);
			}

			selectedCard = -1;
			CheckGameOver();

//			if (newPosition < 4)
//			{
//				if (oldPosition < 4)
//				{
//					// Moving from the left tableau
//					bool validMove = false;
//					if (theTableau[newPosition].Count > 0)
//					{
//						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
//						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition].GetTopCard());
//						validMove = (baseRank + 1 == newRank);
//					}
//
//					if (validMove || theTableau[newPosition].Count == 0)
//						MoveCard(oldPosition, newPosition);
//					else
//						InvalidMove(oldPosition);
//				}
//				else if (oldPosition > 7)
//				{
//					// Moving from the right tableau
//					bool validMove = false;
//					if (theTableau[newPosition].Count > 0)
//					{
//						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
//						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition].GetTopCard());
//						validMove = (baseRank + 1 == newRank);
//					}
//
//					if (validMove || theTableau[newPosition].Count == 0)
//						MoveCard(oldPosition, newPosition);
//					else
//						InvalidMove(oldPosition);
//				}
//			}
//			else if (newPosition < 8)
//			{
//				// Move to the foundation
//				if (oldPosition < 4)
//				{
//					// Moving from the left tableau
//					CardRank baseRank = Card.RankFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
//					CardRank newRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
//					CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
//					CardSuit newSuit = Card.SuitFromCardIndex(theTableau[oldPosition].GetTopCard());
//
//					if (baseRank + 1 == newRank && baseSuit == newSuit)
//						MoveCard(oldPosition, newPosition);
//					else
//						InvalidMove(oldPosition);
//				}
//				else
//				{
//					// Moving from the right tableau
//					CardRank baseRank = Card.RankFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
//					CardRank newRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
//					CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
//					CardSuit newSuit = Card.SuitFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
//
//					if (baseRank + 1 == newRank && baseSuit == newSuit)
//						MoveCard(oldPosition, newPosition);
//					else
//					InvalidMove(oldPosition);
//				}
//			}
//			else
//			{
//				// Move to the right tableau
//				if (oldPosition < 4)
//				{
//					// Moving from the left tableau
//					bool validMove = false;
//					if (theTableau[newPosition - 4].Count > 0)
//					{
//						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
//						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition - 4].GetTopCard());
//						validMove = (baseRank + 1 == newRank);
//					}
//
//					if (validMove || theTableau[newPosition - 4].Count == 0)
//						MoveCard(oldPosition, newPosition);
//					else
//						InvalidMove(oldPosition);
//				}
//				else if (oldPosition > 7)
//				{
//					// Moving from the right tableau
//					bool validMove = false;
//					if (theTableau[newPosition - 4].Count > 0)
//					{
//						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
//						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition - 4].GetTopCard());
//						validMove = (baseRank + 1 == newRank);
//					}
//
//					if (validMove || theTableau[newPosition - 4].Count == 0)
//						MoveCard(oldPosition, newPosition);
//					else
//						InvalidMove(oldPosition);
//				}			
//			}
		}

		/// <summary>
		/// Draws the Cards
		/// </summary>
		private void DrawCards()
		{
			drawCard.Begin(this.CreateGraphics());
			
			// Draw the "Ladies' side" (left tableau)
			for (int index = 0; index < 12; index++)
			{
				if (theLeftTableau[index] > -1)
				{
					if (selectedCard == index)
					{
						drawCard.DrawHighlightedCard(new Point(TableauLeftX + TableauWidth * (index % 4),
							TopY + DrawingHeight * (index / 4)), theLeftTableau[index]);
					}
					else
					{
						drawCard.DrawCard(new Point(TableauLeftX + TableauWidth * (index % 4),
							TopY + DrawingHeight * (index / 4)), theLeftTableau[index]);
					}
				}
				else
				{
					drawCard.DrawCardBack(new Point(TableauLeftX + TableauWidth * (index % 4),
						TopY + DrawingHeight * (index / 4)), CardBack.O);
				}
			}

			// Draw the "Ladies' side" (right tableau)
			for (int index = 0; index < 16; index++)
			{
				if (theRightTableau[index] > -1)
				{
					if (selectedCard == index + 12)
					{
						drawCard.DrawHighlightedCard(new Point(TableauRightX + TableauWidth * (index % 4),
							TopY + DrawingHeight * (index / 4)), theRightTableau[index]);
					}
					else
					{
						drawCard.DrawCard(new Point(TableauRightX + TableauWidth * (index % 4),
							TopY + DrawingHeight * (index / 4)), theRightTableau[index]);
					}
				}
				else
				{
					drawCard.DrawCardBack(new Point(TableauRightX + TableauWidth * (index % 4),
						TopY + DrawingHeight * (index / 4)), CardBack.O);
				}
			}

			// Draw the foundation piles in the center
			for (int index = 0; index < 8; index++)
			{
				if (theFoundation[index / 4][index % 4].Count > 0)
				{
					drawCard.DrawCard(new Point(FoundationX + CardWidth * (index / 4), 
						TopY + DrawingHeight * (index % 4)), theFoundation[index / 4][index % 4].GetTopCard());
				}
				else
				{
					drawCard.DrawCardBack(new Point(FoundationX + CardWidth * (index / 4), 
						TopY + DrawingHeight * (index % 4)), CardBack.O);
				}
			}
			
			// Draw the stock and waste piles
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
			oldCardArea = new Rectangle(WasteX, StockY, CardWidth+3, CardHeight+3);
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
					drawCard.DrawCard(new Point(WasteX+count, StockY+count),
						theWaste[theWaste.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(WasteX, StockY), CardBack.O);
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

			if (column < 12)
			{
				cardLocation.X = TableauLeftX + TableauWidth * ((column) % 4);
				cardLocation.Y = TopY + DrawingHeight * ((column) / 4);
			
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theLeftTableau[column]);
				else
					drawCard.DrawCard(cardLocation, theLeftTableau[column]);
			}
			else if (column < 28)
			{
				cardLocation.X = TableauRightX + TableauWidth * ((column - 12) % 4);
				cardLocation.Y = TopY + DrawingHeight * ((column - 12) / 4);
			
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theRightTableau[column - 12]);
				else
					drawCard.DrawCard(cardLocation, theRightTableau[column - 12]);
			}
			else if (column == 37)
			{
				int stacksize = 3;
				if (theWaste.Count < stacksize)
				{
					stacksize = theWaste.Count - 1;
				}

				if (selected)
				{
					drawCard.DrawHighlightedCard(new Point(WasteX+stacksize, StockY+stacksize),
						theWaste.GetTopCard());
				}
				else
				{
					drawCard.DrawCard(new Point(WasteX+stacksize, StockY+stacksize),
						theWaste.GetTopCard());
				}
			}

//			if (column < 4)
//			{
//				int space = theTableau[column].Count - originalCount[column];
//
//				cardLocation.X = TableauLeftX + space;
//				cardLocation.Y = TopY + DrawingHeight * column + space;
//
//				if (originalCount[column] > 0)
//					 cardLocation.X += TableauWidth * (originalCount[column] - 1);
//							
//				if (selected)
//					drawCard.DrawHighlightedCard(cardLocation, theTableau[column].GetTopCard());
//				else
//					drawCard.DrawCard(cardLocation, theTableau[column].GetTopCard());
//			}
//			else if (column > 7)
//			{
//				int space = theTableau[column - 4].Count - originalCount[column - 4];
//
//				cardLocation.X = TableauRightX + space;
//				cardLocation.Y = TopY + DrawingHeight * (column - 8) + space;
//
//				if (originalCount[column - 4] > 0)
//					cardLocation.X += TableauWidth * (originalCount[column - 4] - 1);
//							
//				if (selected)
//					drawCard.DrawHighlightedCard(cardLocation, theTableau[column - 4].GetTopCard());
//				else
//					drawCard.DrawCard(cardLocation, theTableau[column - 4].GetTopCard());	
//			}

			drawCard.End();
		}

		/// <summary>
		/// Deals cards from the stock pile to the waste pile
		/// </summary>
		private void DealStock()
		{
			finished = false;

			if (theStock.Count > 0)
			{
				// determine the number of cards to deal
				int numbertodeal = 1;
				if (theStock.Count < numbertodeal)
					numbertodeal = theStock.Count;

				// add the cards to the waste pile and remove them from the stock
				for (int count = 0; count < numbertodeal; count++)
				{
					theWaste.Add(theStock.GetTopCard());
					theStock.RemoveTopCard();
				}
			}

			drawCard.Begin(this.CreateGraphics());
			DrawStockAndWaste();
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

//			if (oldPosition < 4)
//				movingCard = theTableau[oldPosition].GetTopCard();
//			else if (oldPosition > 7)
//				movingCard = theTableau[oldPosition - 4].GetTopCard();

			// add it to new location if it is valid
			if (movingCard < 52)
			{
//				if (newPosition < 4)
//				{
//					theTableau[newPosition].Add(movingCard);
//					int space = theTableau[newPosition].Count - originalCount[newPosition];
//					
//					drawCard.Begin(this.CreateGraphics());
//					Point cardLocation = new Point();
//					cardLocation.X = TableauLeftX + space;
//					cardLocation.Y = TopY + DrawingHeight * (newPosition) + space;
//
//					if (originalCount[newPosition] > 0)
//						 cardLocation.X += TableauWidth * (originalCount[newPosition] - 1);
//
//					drawCard.DrawCard(cardLocation, theTableau[newPosition].GetTopCard());
//					drawCard.End();
//				}
//				else if (newPosition < 8)
//				{
//					theFoundation[newPosition - 4].Add(movingCard);
//					drawCard.Begin(this.CreateGraphics());
//					drawCard.DrawCard(new Point(FoundationX, TopY + DrawingHeight * (newPosition - 4)),
//						theFoundation[newPosition - 4].GetTopCard());
//					drawCard.End();
//					cardsPlayed++;
//				}
//				else
//				{
//					theTableau[newPosition - 4].Add(movingCard);
//					int space = theTableau[newPosition - 4].Count - originalCount[newPosition - 4];
//
//					drawCard.Begin(this.CreateGraphics());
//					Point cardLocation = new Point();
//					cardLocation.X = TableauRightX + space;
//					cardLocation.Y = TopY + DrawingHeight * (newPosition - 8) + space;
//
//					if (originalCount[newPosition - 4] > 0)
//						cardLocation.X += TableauWidth * (originalCount[newPosition - 4] - 1);
//
//					drawCard.DrawCard(cardLocation, theTableau[newPosition - 4].GetTopCard());
//					drawCard.End();
//				}

				// remove it from old location
				RemoveCard(oldPosition);
			}
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column">Column of the card to remove</param>
		private void RemoveCard(int column)
		{
//			if (column < 4) 
//			{
//				int space = theTableau[column].Count - originalCount[column];
//				int x = TableauLeftX + space;
//				if (originalCount[column] > 0)
//					x += (originalCount[column] - 1) * TableauWidth;
//				int y = TopY + column * DrawingHeight + space;
//				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
//				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
//
//				theTableau[column].RemoveTopCard();
//				if (originalCount[column] > theTableau[column].Count)
//					originalCount[column]--;
//				space--;
//
//				drawCard.Begin(this.CreateGraphics());
//							
//				if (theTableau[column].Count == 0)
//				{
//					drawCard.DrawCardBack(new Point(TableauLeftX, y), CardBack.O);
//				}
//				else
//				{
//					if (space < 0)
//						space = 0;
//
//					if (originalCount[column] > 0)
//					{
//						drawCard.DrawCard(new Point(TableauLeftX + (originalCount[column] - 1) * TableauWidth + space,
//							TopY + column * DrawingHeight + space),theTableau[column].GetTopCard());
//					}
//					else
//					{
//						drawCard.DrawCard(new Point(TableauLeftX + space, TopY + column *
//							DrawingHeight + space), theTableau[column].GetTopCard());
//					}
//				}
//
//				drawCard.End();
//			}
//			else if (column > 7)
//			{
//				int space = theTableau[column - 4].Count - originalCount[column - 4];
//				int x = TableauRightX + space;
//				if (originalCount[column - 4] > 0)
//					x += (originalCount[column - 4] - 1) * TableauWidth;
//				int y = TopY + (column - 8) * DrawingHeight + space;
//				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
//				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
//				
//				theTableau[column - 4].RemoveTopCard();
//				if (originalCount[column - 4] > theTableau[column - 4].Count)
//					originalCount[column - 4]--;
//				space--;
//
//				drawCard.Begin(this.CreateGraphics());
//							
//				if (theTableau[column - 4].Count == 0)
//				{
//					drawCard.DrawCardBack(new Point(TableauRightX, y), CardBack.O);
//				}
//				else
//				{
//					if (space < 0)
//						space = 0;
//
//					if (originalCount[column - 4] > 0)
//					{
//						drawCard.DrawCard(new Point(TableauRightX + (originalCount[column - 4] - 1) * TableauWidth + space,
//							TopY + (column - 8) * DrawingHeight + space), theTableau[column - 4].GetTopCard());
//					}
//					else
//					{
//						drawCard.DrawCard(new Point(TableauRightX + space, TopY + (column - 8) *
//							DrawingHeight + space),	theTableau[column - 4].GetTopCard());
//					}
//				}
//
//				drawCard.End();
//			}
		}

		/// <summary>
		/// Determines whether the game is won or not
		/// </summary>
		private void CheckGameOver()
		{
			bool gameover = true;
			// TODO: Test this logic!
			for (int count = 0; count < 8 && gameover; count++)
			{
				gameover = theFoundation[count / 4][count % 4].Count == 13;
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
	}
}
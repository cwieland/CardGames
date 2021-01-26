// Corners tasks:
//  Fix highlighted card bug (on refresh)

namespace CardGames.Corners
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
	/// This is a simple implementation of the solitare game Corners.
	/// </summary>
	public class Corners : System.Windows.Forms.Form
	{
		#region Constants
		private const int StockX = 25;								// The left most position of the stock pile
		private const int TableX = 245;								// The left most position of the table
		private const int TableY = 15;								// The top most position of the table and stock pile
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 107;							// The height of a card
		private const int WasteX = 115;								// The left most position of the waste pile
		private const int WasteWidth = 15;							// The width of the waste cards
		#endregion

		#region Private Fields
		private Deck theDeck;										// The data structure for the deck of cards
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private Foundation theFoundation;							// The data structure for the foundation
		private CardBack currentCardBack;							// The selected card back
		private CardCollection[] theTableau;						// The data structure for the tableau
		private CardCollection theStock;							// The data structure for the stock pile
		private CardCollection theWaste;							// The data structure for the waste pile
		private int selectedCard;									// The currently selected card
		private int cardsPlayed;									// The number of cards played to the foundation
		private bool finished;										// Whether the hand is finished or not
		private CardRank foundationStartRank;						// The starting rank of the foundation
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
		private System.Windows.Forms.MenuItem OptionsSeperator;		// The options seperator
		private System.Windows.Forms.MenuItem ChangeCardBack;		// The Change Card Back menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public Corners()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			theWaste = new CardCollection();
			theStock = new CardCollection();
			theTableau = new CardCollection[5];
			theFoundation = new Foundation();
			currentCardBack = CardBack.Weave1;			
			for (int count = 0; count < 5; count++)
				theTableau[count] = new CardCollection();
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
			// Corners
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(498, 355);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Corners";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Corners";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Corners_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Corners_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Corners_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles a mouse click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Corners_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			for (int column = 0; column < 11; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;

				if (column < 9)
				{
					// Clicked on the tableau and foundation
					int numberOfCards = 0;
					switch (column)
					{
						case 0:
							numberOfCards = theFoundation[0].Count;
							break;
						case 1:
							numberOfCards = theTableau[0].Count;
							break;
						case 2:
							numberOfCards = theFoundation[1].Count;
							break;
						case 3:
							numberOfCards = theTableau[1].Count;
							break;
						case 4:
							numberOfCards = theTableau[2].Count;
							break;
						case 5:
							numberOfCards = theTableau[3].Count;
							break;
						case 6:
							numberOfCards = theFoundation[2].Count;
							break;
						case 7:
							numberOfCards = theTableau[4].Count;
							break;
						case 8:
							numberOfCards = theFoundation[3].Count;
							break;
					}

					left = (column % 3) * CardWidth + TableX;
					top = (column / 3) * CardHeight + TableY;
					bottom = top + CardHeight;
				}
				else if (column == 9)
				{
					// Clicked on the waste pile
					int stacksize = 3;
					if (theWaste.Count < stacksize)
						stacksize = theWaste.Count;
					
					if (stacksize == 0)
					{
						left = WasteX;
						top = TableY;
					}
					else
					{
						left = WasteX + (stacksize - 1) * WasteWidth;
						top = TableY;
					}

					bottom = top + CardHeight;
				}
				else if (column == 10)
				{
					// Clicked on the stock pile
					int stacksize = 3;
					if (theStock.Count < stacksize)
						stacksize = theStock.Count;
					left = StockX + stacksize;
					top = TableY + stacksize;
					bottom = top + CardHeight;
				}

				int right = left + CardWidth;
				
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
							if (column == 10)
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
								bool selectFromTableau1 = (column == 1 && theTableau[0].Count > 0);
								bool selectFromTableau2 = (column == 3 && theTableau[1].Count > 0);
								bool selectFromTableau3 = (column == 4 && theTableau[2].Count > 0);
								bool selectFromTableau4 = (column == 5 && theTableau[3].Count > 0); 
								bool selectFromTableau5 = (column == 7 && theTableau[4].Count > 0);
								bool selectFromWaste = (column == 9 && theWaste.Count > 0);
								if (selectFromTableau1 || selectFromTableau2 || selectFromTableau3
									|| selectFromTableau4 || selectFromTableau5 || selectFromWaste)
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
		/// Refreshes the screen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Corners_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Corners_Closing(object sender, CancelEventArgs e)
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
		/// Changes the Card Back
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
			
			// Redraw the Stock pile
			if (theStock.Count > 0)
			{
				int stacksize = 3;
				if (theStock.Count < stacksize)
					stacksize = theStock.Count;
				for (int count = 0; count < stacksize; count++) 
					drawCard.DrawCardBack(new Point(StockX+count, TableY+count), currentCardBack);
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
		/// <summary>
		/// Deals a new hand
		/// </summary>
		private void NewHand()
		{
			theDeck.Shuffle();
			selectedCard = -1;
			cardsPlayed = 0;
			finished = true;

			// Reset all card piles
			theStock.Clear();
			theWaste.Clear();

			// Reset the foundation and tableau
			for (int count = 0; count < 5; count++)
			{
				if (count < 4)
				{
					theFoundation[count].Clear();
				}
				theTableau[count].Clear();
			}

			// Start the tableau with the first five cards
			for (int count = 0; count < 5; count++)
			{
				theTableau[count].Add(theDeck[count]);
			}
			
			// Add the next card to the first pile of the foundation
			theFoundation[0].Add(theDeck[5]);
			foundationStartRank = Card.RankFromCardIndex(theDeck[5]);
			cardsPlayed++;

			// Fill the stock pile with the rest of the deck
			for (int count = 51; count > 5; count--)
			{
				theStock.Add(theDeck[count]);
			}

			this.Refresh();
		}

		/// <summary>
		/// Draws / refreshes the entrie screen
		/// </summary>
		private void DrawCards()
		{
			drawCard.Begin(this.CreateGraphics());

			// Draw the stock and waste piles
			DrawStockAndWaste();

			for (int pilenumber = 0; pilenumber < 9; pilenumber++)
			{
				DrawPile(pilenumber);
			}

			drawCard.End();
		}

		/// <summary>
		/// Draws the stock and waste piles
		/// </summary>
		private void DrawStockAndWaste()
		{
			// Erase the card areas
			Rectangle oldCardArea = new Rectangle(StockX, TableY, CardWidth+3, CardHeight+3);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			oldCardArea = new Rectangle(WasteX, TableY, CardWidth+WasteWidth*3, CardHeight);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			
			// Draw the stock pile
			if (theStock.Count > 0)
			{
				int stacksize = 3;
				if (theStock.Count < stacksize)
					stacksize = theStock.Count;
				for (int count = 0; count < stacksize; count++) 
					drawCard.DrawCardBack(new Point(StockX+count, TableY+count), currentCardBack);
			}
			else
				drawCard.DrawCardBack(new Point(StockX, TableY), CardBack.X);

			// Draw the waste pile
			if (theWaste.Count > 0)
			{
				int stacksize = 3;
				if (theWaste.Count < stacksize)
					stacksize = theWaste.Count;

				for (int count = 0; count < stacksize; count++)
				{
					drawCard.DrawCard(new Point(WasteX + WasteWidth * count, TableY),
						theWaste[theWaste.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(WasteX, TableY), CardBack.Unused);
			}
		}

		/// <summary>
		/// Draws a selected pile
		/// </summary>
		/// <param name="pilenumber">Pile number (0-8)</param>
		private void DrawPile(int pilenumber)
		{
			CardCollection currentPile;
			switch (pilenumber)
			{
				case 0:
					currentPile = theFoundation[0];
					break;
				case 1:
					currentPile = theTableau[0];
					break;
				case 2:
					currentPile = theFoundation[1];
					break;
				case 3:
					currentPile = theTableau[1];
					break;
				case 4:
					currentPile = theTableau[2];
					break;
				case 5:
					currentPile = theTableau[3];
					break;
				case 6:
					currentPile = theFoundation[2];
					break;
				case 7:
					currentPile = theTableau[4];
					break;
				default:
					currentPile = theFoundation[3];
					break;
			}

			// Draw the waste pile
			if (currentPile.Count > 0)
			{
				int stacksize = 3;
				if (currentPile.Count < stacksize)
					stacksize = currentPile.Count;

				for (int count = 0; count < stacksize; count++)
				{
					drawCard.DrawCard(new Point(TableX + (pilenumber % 3) * CardWidth + count, 
						TableY + (pilenumber / 3) * CardHeight + count),
						currentPile[currentPile.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(TableX + (pilenumber % 3) * CardWidth, 
					TableY + (pilenumber / 3) * CardHeight), CardBack.O);
			}
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="column">Column of the card (0-3 tableau, 8 reserve, 9 waste)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int column, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();

			if (column == 1)
			{
				int stacksize = 3;
				if (theTableau[0].Count - 1 < stacksize)
					stacksize = theTableau[0].Count - 1;

				cardLocation.X = CardWidth + TableX + stacksize;
				cardLocation.Y = TableY + stacksize;
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[0].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[0].GetTopCard());
			}
			if (column > 2 && column < 6)
			{
				int stacksize = 3;
				if (theTableau[(column % 3) + 1].Count - 1 < stacksize)
					stacksize = theTableau[(column % 3) + 1].Count - 1;

				cardLocation.X = column % 3 * CardWidth + TableX + stacksize;
				cardLocation.Y = CardHeight + TableY + stacksize;
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[(column % 3) + 1].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[(column % 3) + 1].GetTopCard());
			}
			else if (column == 7)
			{
				int stacksize = 3;
				if (theTableau[4].Count - 1 < stacksize)
					stacksize = theTableau[4].Count - 1;

				cardLocation.X = CardWidth + TableX + stacksize;
				cardLocation.Y = 2 * CardHeight + TableY + stacksize;
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[4].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[4].GetTopCard());
			}
			else if (column == 9)
			{
				cardLocation.X = WasteX;
				cardLocation.Y = TableY;

				if (theWaste.Count > 0)
				{
					int stacksize = 3;
					if (theWaste.Count < stacksize)
						stacksize = theWaste.Count;
					cardLocation.X = WasteX + WasteWidth * (stacksize - 1);

					if (selected)
						drawCard.DrawHighlightedCard(cardLocation, theWaste.GetTopCard());
					else
						drawCard.DrawCard(cardLocation, theWaste.GetTopCard());
				}
				else
					drawCard.DrawCardBack(cardLocation, CardBack.Unused);
			}

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
		/// Attempt a move
		/// </summary>
		/// <param name="oldPosition">Original position of card</param>
		/// <param name="newPosition">New position of card</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			if (newPosition == 1 || newPosition == 3 || newPosition == 4 || newPosition == 5 || newPosition == 7)
			{
				// Moving card to tableau
				if (oldPosition == 1 || oldPosition == 3 || oldPosition == 4 || oldPosition == 5 || oldPosition == 7)
				{
					// Moving from tableau
					bool valid = false;
					bool sourcenotempty  = theTableau[GetTableauIndex(oldPosition)].Count > 0;
					bool targetnotempty = theTableau[GetTableauIndex(newPosition)].Count > 0;
					if (sourcenotempty && targetnotempty)
					{
						CardRank sourceRank = Card.RankFromCardIndex(theTableau[GetTableauIndex(oldPosition)].GetTopCard());
						CardRank targetRank = Card.RankFromCardIndex(theTableau[GetTableauIndex(newPosition)].GetTopCard());
						valid = targetRank - 1 == sourceRank || (targetRank == CardRank.Ace && sourceRank == CardRank.King);
					}
					
					if (valid || !targetnotempty)
					{
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
				else if (oldPosition == 9)
				{
					// Moving card from the waste pile
					if (theTableau[GetTableauIndex(newPosition)].Count == 0)
					{
						// Empty tableau pile
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						// Tableau pile with cards
						CardRank sourceRank = Card.RankFromCardIndex(theWaste.GetTopCard());
						CardRank targetRank = Card.RankFromCardIndex(theTableau[GetTableauIndex(newPosition)].GetTopCard());

						if (targetRank - 1 == sourceRank || (targetRank == CardRank.Ace && sourceRank == CardRank.King))
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
				else
				{
					InvalidMove(oldPosition);
				}
			}
			else if (newPosition == 0 || newPosition == 2 || newPosition == 6 || newPosition == 8)
			{
				// Moving card to foundation
				if (oldPosition == 1 || oldPosition == 3 || oldPosition == 4 || oldPosition == 5 || oldPosition == 7)
				{
					// Moving from the tableau
					if (theFoundation[GetFoundationIndex(newPosition)].Count == 0)
					{
						// Empty foundation pile
						CardRank sourceRank = Card.RankFromCardIndex(theTableau[GetTableauIndex(oldPosition)].GetTopCard());
						if (sourceRank == foundationStartRank)
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
					else
					{
						// Foundation pile with cards
						CardRank sourceRank = Card.RankFromCardIndex(theTableau[GetTableauIndex(oldPosition)].GetTopCard());
						CardSuit sourceSuit = Card.SuitFromCardIndex(theTableau[GetTableauIndex(oldPosition)].GetTopCard());
						CardRank targetRank = Card.RankFromCardIndex(theFoundation[GetFoundationIndex(newPosition)].GetTopCard());
						CardSuit targetSuit = Card.SuitFromCardIndex(theFoundation[GetFoundationIndex(newPosition)].GetTopCard());

						if ((targetRank + 1 == sourceRank || targetRank == CardRank.King && sourceRank == CardRank.Ace)
							&& targetSuit == sourceSuit)
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
				else if (oldPosition == 9)
				{
					// Moving card from the waste pile
					if (theFoundation[GetFoundationIndex(newPosition)].Count == 0)
					{
						// Empty foundation pile
						CardRank sourceRank = Card.RankFromCardIndex(theWaste.GetTopCard());
						if (sourceRank == foundationStartRank)
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
					else
					{
						// Foundation pile with cards
						CardRank sourceRank = Card.RankFromCardIndex(theWaste.GetTopCard());
						CardSuit sourceSuit = Card.SuitFromCardIndex(theWaste.GetTopCard());
						CardRank targetRank = Card.RankFromCardIndex(theFoundation[GetFoundationIndex(newPosition)].GetTopCard());
						CardSuit targetSuit = Card.SuitFromCardIndex(theFoundation[GetFoundationIndex(newPosition)].GetTopCard());

						if ((targetRank + 1 == sourceRank || targetRank == CardRank.King && sourceRank == CardRank.Ace)
							&& targetSuit == sourceSuit)						
						{
							MoveCard(oldPosition, newPosition);
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
				else
				{
					InvalidMove(oldPosition);
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
		/// Does a card move
		/// </summary>
		/// <param name="oldPosition">Old position of card</param>
		/// <param name="newPosition">New position of card</param>		
		private void MoveCard(int oldPosition, int newPosition)
		{
			// Get the value of the card
			int movingCard = 52;
			if (oldPosition == 1 || oldPosition == 3 || oldPosition == 4 || oldPosition == 5 || oldPosition == 7)
			{
				movingCard = theTableau[GetTableauIndex(oldPosition)].GetTopCard();
			}
			else if (oldPosition == 0 || oldPosition == 2 || oldPosition == 6 || oldPosition == 8)
			{
				movingCard = theFoundation[GetFoundationIndex(oldPosition)].GetTopCard();
			}
			else if (oldPosition == 9)
			{
				movingCard = theWaste.GetTopCard();
			}

			// add it to new location if it is valid
			if (movingCard < 52)
			{
				if (newPosition < 9)
				{
					if (newPosition == 1 || newPosition == 3 || newPosition == 4 || newPosition == 5 || newPosition == 7)
					{
						theTableau[GetTableauIndex(newPosition)].Add(movingCard);
					}
					else if (newPosition == 0 || newPosition == 2 || newPosition == 6 || newPosition == 8)
					{
						theFoundation[GetFoundationIndex(newPosition)].Add(movingCard);
						cardsPlayed++;
					}

					int x = (newPosition % 3)*CardWidth+TableX;
					int y = (newPosition / 3)*CardHeight+TableY;
					Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
					Graphics windowGraphics = this.CreateGraphics();
					windowGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
					drawCard.Begin(this.CreateGraphics());
					DrawPile(newPosition);
					drawCard.End();
				}
				else
				{
					drawCard.Begin(this.CreateGraphics());
					DrawStockAndWaste();
					drawCard.End();
				}

				// remove it from old location
				RemoveCard(oldPosition);	
			}
		}

		/// <summary>
		/// Removes a card and erases it from the screen if necessary.
		/// </summary>
		/// <param name="column"></param>
		private void RemoveCard(int column)
		{
			if (column == 1 || column == 3 || column == 4 || column == 5 || column == 7) 
			{
				theTableau[GetTableauIndex(column)].RemoveTopCard();
				int x = (column%3)*CardWidth+TableX;
				int y = (column/3)*CardHeight+TableY;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				Graphics windowGraphics = this.CreateGraphics();
				windowGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				drawCard.Begin(windowGraphics);
				DrawPile(column);
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
		/// Gets the index of the tableau pile from a column number
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		private int GetTableauIndex(int column)
		{
			int index = -1;
			switch (column)
			{
				case 1:
					index = 0;
					break;
				case 3:
					index = 1;
					break;
				case 4:
					index = 2;
					break;
				case 5:
					index = 3;
					break;
				case 7:
					index = 4;
					break;
			}
			return index;
		}

		/// <summary>
		/// Gets the index of the foundation pile from a column number
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		private int GetFoundationIndex(int column)
		{
			int index = -1;
			switch (column)
			{
				case 0:
					index = 0;
					break;
				case 2:
					index = 1;
					break;
				case 6:
					index = 2;
					break;
				case 8:
					index = 3;
					break;
			}
			return index;
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
	}
}
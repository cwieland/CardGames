// Klondike tasks:
//  Fix highlighted card bug (on refresh)
//  implement an undo option

namespace CardGames.Klondike
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
	/// This is a simple implementation of the solitare game Klondike.
	/// </summary>
	public class Klondike : System.Windows.Forms.Form
	{
		#region Constants
		private const int StockX = 25;								// The left most position of the stock pile
		private const int FoundationX = 245;						// The left most position of the Foundation and waste piles
		private const int FoundationY = 15;							// The top most position of the Foundation
		private const int TableauX = 25;							// The left most position of the tableau
		private const int TableauY = 130;							// The top most position of the tableau
		private const int TableauWidth = 80;						// The width of the tableau piles
		private const int TableauHeight = 17;						// The height of the tableau pile cards
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
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
		private int[] hiddenCount;									// Number of cards face down on the tableau pile
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
		#endregion`
	
		#region Constructors
		public Klondike()
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
			theTableau = new CardCollection[7];
			hiddenCount = new int[7];
			theFoundation = new Foundation();
			currentCardBack = CardBack.Weave1;			
			for (int count = 0; count < 7; count++)
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
			// Klondike
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(600, 547);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Klondike";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Klondike";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Calculation_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Klondike_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Calculation_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles a mouse click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Calculation_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			for (int column = 0; column < 13; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;

				if (column < 7)
				{
					// Clicked on the tableau
					left = column * TableauWidth + TableauX;
					top = TableauY;
					bottom = top + CardHeight;

					if (theTableau[column].Count > 0)
						bottom += (theTableau[column].Count - 1) * TableauHeight;
				}
				else if (column < 11)
				{
					// Clicked on the foundation
					left = (column - 7) * CardWidth + FoundationX;
					top = FoundationY;
					bottom = top + CardHeight;
				}
				else if (column == 11)
				{
					// Clicked on the stock pile
					int stacksize = 3;
					if (theStock.Count < stacksize)
						stacksize = theStock.Count;
					left = StockX + stacksize;
					top = FoundationY + stacksize;
					bottom = top + CardHeight;
				}
				else if (column == 12)
				{
					// Clicked on the waste pile
					int stacksize = 3;
					if (theWaste.Count < stacksize)
						stacksize = theWaste.Count;
					
					if (stacksize == 0)
					{
						left = WasteX;
						top = FoundationY;
					}
					else
					{
						left = WasteX + (stacksize - 1) * WasteWidth;
						top = FoundationY;
					}
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
							if (column == 11)
							{
								DealStock();
							}
							else
							{
								bool selectFromTableau = (column < 7 && theTableau[column].Count > 0);
								bool selectFromWaste = (column == 12 && theWaste.Count > 0);
								if (selectFromTableau || selectFromWaste)
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
		private void Calculation_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Klondike_Closing(object sender, CancelEventArgs e)
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

		#region Control Events`
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
					drawCard.DrawCardBack(new Point(StockX+count, FoundationY+count), currentCardBack);
			}
			
			// Redraw any tableau columns that have hidden cards
			for (int column = 0; column < 7; column++)
			{
				if (hiddenCount[column] > 0)
				{
					for (int count = 0; count < theTableau[column].Count; count++)
					{
						if (count < hiddenCount[column])
							drawCard.DrawCardBack(new Point(TableauWidth * column + TableauX, TableauHeight * count + TableauY),
								currentCardBack);
						else
							drawCard.DrawCard(new Point(TableauWidth * column + TableauX, TableauHeight * count + TableauY),
								theTableau[column][count]);
					}
				}
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

			for (int count = 0; count < 4; count++)
				theFoundation[count].Clear();

			for (int count = 0; count < 7; count++)
				theTableau[count].Clear();

			int deckLocation = 0;

			for (int column = 0; column < 7; column++)
			{
			// Set the number of hidden cards
				hiddenCount[column] = column;

				// Deal a row of card across
				for (int addcolumn = column; addcolumn < 7; addcolumn++)
				{
					theTableau[addcolumn].Add(theDeck[deckLocation]);
					deckLocation++;
				}
			}
			
			// Fill the stock pile with the rest of the deck
			for (int count = 51; count >= deckLocation; count--)
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
			
			// Draw the stock, waste, and reserve piles
			DrawStockAndWaste();

			for (int column = 0; column < 4; column++)
			{
				// Draw the foundation
				if (theFoundation[column].Count > 0)
					drawCard.DrawCard(new Point(CardWidth * column + FoundationX, FoundationY),
						theFoundation[column].GetTopCard());
				else
					drawCard.DrawCardBack(new Point(CardWidth * column + FoundationX, FoundationY), CardBack.O);
			}

			for (int column = 0; column < 7; column++)
			{
				// Draw the tableau
				if (theTableau[column].Count > 0)
					for (int count = 0; count < theTableau[column].Count; count++)
						if (count < hiddenCount[column])
							drawCard.DrawCardBack(new Point(TableauWidth * column + TableauX, TableauHeight * count + TableauY),
								currentCardBack);
						else
							drawCard.DrawCard(new Point(TableauWidth * column + TableauX, TableauHeight * count + TableauY),
								theTableau[column][count]);
				else
					drawCard.DrawCardBack(new Point(TableauWidth * column + TableauX, TableauY), CardBack.O);
			}

			drawCard.End();
		}

		/// <summary>
		/// Draws the stock and waste piles
		/// </summary>
		private void DrawStockAndWaste()
		{
			// Erase the card areas
			Rectangle oldCardArea = new Rectangle(StockX, FoundationY, CardWidth+3, CardHeight+3);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			oldCardArea = new Rectangle(WasteX, FoundationY, CardWidth+WasteWidth*3, CardHeight);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			
			// Draw the stock pile
			if (theStock.Count > 0)
			{
				int stacksize = 3;
				if (theStock.Count < stacksize)
					stacksize = theStock.Count;
				for (int count = 0; count < stacksize; count++) 
					drawCard.DrawCardBack(new Point(StockX+count, FoundationY+count), currentCardBack);
			}
			else
				drawCard.DrawCardBack(new Point(StockX, FoundationY), CardBack.X);

			// Draw the waste pile
			if (theWaste.Count > 0)
			{
				int stacksize = 3;
				if (theWaste.Count < stacksize)
					stacksize = theWaste.Count;

				for (int count = 0; count < stacksize; count++)
				{
					drawCard.DrawCard(new Point(WasteX + WasteWidth * count, FoundationY),
						theWaste[theWaste.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(WasteX, FoundationY), CardBack.Unused);
			}
		}
		
		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="column">Column of the card (0-6 tableau, 7-10 foundation, 11 stock, 12 waste)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int column, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();

			if (column < 7)
			{
				cardLocation.X = column * TableauWidth + TableauX;
				cardLocation.Y = (theTableau[column].Count - 1) * TableauHeight + TableauY;
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[column].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[column].GetTopCard());
			}
			else if (column == 12)
			{
				cardLocation.X = WasteX;
				cardLocation.Y = FoundationY;

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
		/// Attempt a move
		/// </summary>
		/// <param name="oldPosition">Original position of card</param>
		/// <param name="newPosition">New position of card</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			if (newPosition < 7)
			{
				// Moving card to tableau
				if (oldPosition < 7)
				{
					// Moving a column to another column
					if (theTableau[newPosition].Count > 0)
					{
						if (IsValidTableauMove(theTableau[newPosition].GetTopCard(), theTableau[oldPosition][hiddenCount[oldPosition]]))
							MoveColumn(oldPosition, newPosition);
						else
							InvalidMove(oldPosition);
					}
					else
					{
						CardRank topRank = Card.RankFromCardIndex(
							theTableau[oldPosition][hiddenCount[oldPosition]]);
						if (topRank == CardRank.King)
							MoveColumn(oldPosition, newPosition);
						else
							InvalidMove(oldPosition);
					}
				}
				else if (oldPosition == 12)
				{
					// Moving card from the waste pile
					if (theTableau[newPosition].Count > 0)
					{
						if (IsValidTableauMove(theTableau[newPosition].GetTopCard(), theWaste.GetTopCard()))
							MoveCard(oldPosition, newPosition);
						else
						{
							InvalidMove(oldPosition);
						}
					}
					else
					{
						CardRank topRank = Card.RankFromCardIndex(theWaste.GetTopCard());
						if (topRank == CardRank.King)
						{
							theTableau[newPosition].Add(theWaste.GetTopCard());
							RemoveCard(oldPosition);
							drawCard.Begin(this.CreateGraphics());
							drawCard.DrawCard(new Point(newPosition * TableauWidth +
								TableauX, TableauY), theTableau[newPosition].GetTopCard());
							drawCard.End();
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
			else if (newPosition < 11)
			{
				// Moving card to foundation
				if (oldPosition < 7)
				{
					// Moving from the tableau
					if (IsValidFoundationMove(newPosition, theTableau[oldPosition].GetTopCard()))
					{
						MoveCard(oldPosition, newPosition);
						cardsPlayed++;
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
				else if (oldPosition == 12)
				{
					// Moving card from the waste pile
					if (IsValidFoundationMove(newPosition, theWaste.GetTopCard()))
					{
						MoveCard(oldPosition, newPosition);
						cardsPlayed++;
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
			}
			else
			{
				InvalidMove(oldPosition);
			}

			selectedCard = -1;
			CheckGameOver();
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
		/// Moves a tableau column
		/// </summary>
		/// <param name="oldPosition">Old position of card</param>
		/// <param name="newPosition">New position of card</param>		
		private void MoveColumn(int oldPosition, int newPosition)
		{
			Graphics windowGraphics = this.CreateGraphics();
			drawCard.Begin(windowGraphics);

			int numToRemove = 0;
			for (int count = hiddenCount[oldPosition]; count < theTableau[oldPosition].Count; count++)
			{
				numToRemove++;
				theTableau[newPosition].Add(theTableau[oldPosition][count]);
				drawCard.DrawCard(new Point(newPosition * TableauWidth + TableauX, TableauY + TableauHeight *
					(theTableau[newPosition].Count - 1)), theTableau[newPosition].GetTopCard());
			}

			drawCard.End();
						
			// erase the column from the screen
			int x = oldPosition * TableauWidth + TableauX;
			int y = TableauY + TableauHeight * (theTableau[oldPosition].Count - 1);
			Rectangle oldCardArea = new Rectangle(x, TableauY, CardWidth, y);
			windowGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);	
			
			drawCard.Begin(windowGraphics);
			if (hiddenCount[oldPosition] > 0)
				hiddenCount[oldPosition]--;
			for (int count = 0; count < numToRemove; count++)
				theTableau[oldPosition].RemoveTopCard();			

			if (theTableau[oldPosition].Count > 0)
				for (int count = 0; count < theTableau[oldPosition].Count; count++)
					if (count < hiddenCount[oldPosition])
						drawCard.DrawCardBack(new Point(TableauWidth * oldPosition + TableauX, TableauHeight * count + TableauY),
							currentCardBack);
					else
						drawCard.DrawCard(new Point(TableauWidth * oldPosition + TableauX, TableauHeight * count + TableauY),
							theTableau[oldPosition][count]);
			else
				drawCard.DrawCardBack(new Point(TableauWidth * oldPosition + TableauX, TableauY), CardBack.O);

			drawCard.End();
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
			if (oldPosition < 7)
			{
				movingCard = theTableau[oldPosition].GetTopCard();
				if (theTableau[oldPosition].Count == hiddenCount[oldPosition] + 1 && hiddenCount[oldPosition] > 0)
				{
					hiddenCount[oldPosition]--;
				}
			}
			else if (oldPosition == 12)
			{
				movingCard = theWaste.GetTopCard();
			}

			// add it to new location if it is valid
			if (movingCard < 52)
			{
				if (newPosition < 7)
				{
					theTableau[newPosition].Add(movingCard);
					drawCard.Begin(this.CreateGraphics());
					drawCard.DrawCard(new Point(TableauWidth * newPosition + TableauX, TableauHeight * 
						(theTableau[newPosition].Count-1) + TableauY), theTableau[newPosition].GetTopCard());
					drawCard.End();
				}
				else if (newPosition < 11)
				{
					theFoundation[newPosition - 7].Add(movingCard);
					drawCard.Begin(this.CreateGraphics());
					drawCard.DrawCard(new Point(CardWidth * (newPosition - 7) + FoundationX, FoundationY),
						theFoundation[newPosition - 7].GetTopCard());
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
			if (column < 7) 
			{
				theTableau[column].RemoveTopCard();
				int x = column * TableauWidth + TableauX;
				int y = (theTableau[column].Count) * TableauHeight + TableauY;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				Graphics windowGraphics = this.CreateGraphics();
				windowGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				drawCard.Begin(windowGraphics);
				if (theTableau[column].Count > 0)
					drawCard.DrawCard(new Point(TableauWidth * column + TableauX,
						TableauHeight *	(theTableau[column].Count - 1) + TableauY), theTableau[column].GetTopCard());
				else
				{
					drawCard.DrawCardBack(new Point(TableauWidth * column + TableauX, TableauY),
						CardBack.O);
				}
				drawCard.End();
			}
			else if (column == 12)
			{
				theWaste.RemoveTopCard();
				drawCard.Begin(this.CreateGraphics());
				DrawStockAndWaste();
				drawCard.End();
			}
		}

		/// <summary>
		/// Checks to see if the moving card is one rank less and a different colored suit than
		/// the stationary card
		/// </summary>
		/// <param name="stationaryCard">Card being played on</param>
		/// <param name="movingCard">Card that is moving</param>
		/// <returns></returns>
		private bool IsValidTableauMove(int stationaryCard, int movingCard)
		{
			bool valid = false;
			CardRank stationaryRank = Card.RankFromCardIndex(stationaryCard);
			CardRank movingRank = Card.RankFromCardIndex(movingCard);
			CardSuit stationarySuit = Card.SuitFromCardIndex(stationaryCard);
			CardSuit movingSuit = Card.SuitFromCardIndex(movingCard);

			// Check the suit
			switch (stationarySuit)
			{
				case CardSuit.Clubs:
				case CardSuit.Spades:
					valid = (movingSuit == CardSuit.Hearts || movingSuit == CardSuit.Diamond);
					break;

				case CardSuit.Diamond:
				case CardSuit.Hearts:
					valid = (movingSuit == CardSuit.Clubs || movingSuit == CardSuit.Spades);
					break;
			}

			// Check the rank
			if (stationaryRank == CardRank.Ace)
				valid = (valid && movingRank == CardRank.King);
			else
				valid = (valid && ((int)stationaryRank - 1 == (int)movingRank));
			return valid;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="movingCard"></param>
		/// <returns></returns>
		private bool IsValidFoundationMove(int column, int movingCard)
		{
			bool validMove = false;
			if (theFoundation[column - 7].Count > 0)
			{
				CardRank movingRank = Card.RankFromCardIndex(movingCard);
				CardSuit movingSuit = Card.SuitFromCardIndex(movingCard);
				CardRank stationaryRank = Card.RankFromCardIndex(theFoundation[column - 7].GetTopCard());
				CardSuit stationarySuit = Card.SuitFromCardIndex(theFoundation[column - 7].GetTopCard());
				if (stationaryRank == CardRank.King)
					validMove = (movingSuit == stationarySuit) && movingRank == CardRank.Ace;
				else
					validMove = (movingSuit == stationarySuit) && (int)movingRank - 1 == (int)stationaryRank;
			}
			else if (theFoundation[column - 7].Count < 13)
			{
				CardRank currentRank = Card.RankFromCardIndex(movingCard);
				validMove = (currentRank == CardRank.Ace);
			}
			
			return validMove;
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
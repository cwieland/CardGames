// Monte Carlo task list:

namespace CardGames.MonteCarlo
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Text;
	using System.Windows.Forms;
	#endregion

	#region MonteCarloCard struct
	public struct MonteCarloCard
	{
		public bool Active;
		public int Card;
	}
	#endregion

	/// <summary>
	/// This is a simple implementation of the solitare game Monte Carlo.
	/// </summary>
	public class MonteCarlo : System.Windows.Forms.Form
	{
		#region Constants
		private const int TopY = 20;								// The top most position of Reserve and Foundation piles
		private const int LeftX = 20;								// The left most position of the Reserve piles
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		private const int StockX = 420;								// The left most position of the Stock pile
		private const int StockY = 20;								// The top most position of the Stock pile
		#endregion

		#region Private Fields
		private CardCollection theStock;							// The data structure for the stock pile
		private Deck theDeck;										// The data structure for the deck of cards
		private MonteCarloCard[] theTableau;						// The data structure for the tableau
		private int selectedCard;									// The currently selected card
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private CardBack currentCardBack;							// The selected card back
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
		public MonteCarlo()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			
			theStock = new CardCollection();
			theTableau = new MonteCarloCard[25];
			drawCard = new Card();
			theStats = new Stats(this.Name);
			currentCardBack = CardBack.Weave1;

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
			// MonteCarlo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(510, 520);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "MonteCarlo";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Monte Carlo";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MonteCarlo_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MonteCarlo_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MonteCarlo_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles any mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MonteCarlo_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			// Check to see if the tableau is clicked
			for (int column = 0; column < 5; column++)
			{
				for (int row = 0; row < 5; row++)
				{
					int left = LeftX + CardWidth * column;
					int top = TopY + CardHeight * row;
					int bottom = top + CardHeight;
					int right = left + CardWidth;
					int index = row * 5 + column;
							
					if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
					{
						if (oldSelection == index)
						{
							if (e.Button == MouseButtons.Left) 
							{
								if (theTableau[index].Active)
								{
									selectedCard = -1;
									ToggleCardSelect(index, false);
								}
								else
								{
									InvalidSelect();
								}
							}
						}
						else if (oldSelection == -1)
						{
							if (e.Button == MouseButtons.Left)
							{
								if (theTableau[index].Active)
								{
									selectedCard = index;
									ToggleCardSelect(index, true);
								}
								else
								{
									InvalidSelect();
								}
							} 
						}
						else
						{
							if (e.Button == MouseButtons.Left)
								AttemptMove(oldSelection, index);
						}
					}
				}
			}

			// Check to see if the the stock pile was clicked
			if ((e.X > StockX && e.X < StockX + CardWidth) && (e.Y > StockY && e.Y < StockY + CardHeight))
			{
				if (oldSelection == -1)
				{
					DealStock();
				}
				else
				{
					InvalidSelect();
				}
			}
		}

		/// <summary>
		/// Draw the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MonteCarlo_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MonteCarlo_Closing(object sender, CancelEventArgs e)
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
			
			// Fill the stock pile
			for (int count = 51; count >= 0; count--)
				theStock.Add(theDeck[count]);

			// Setup the tableau
			for (int count = 0; count < 25; count++)
			{
				theTableau[count] = new MonteCarloCard();
				theTableau[count].Active = true;
				theTableau[count].Card = theStock.GetTopCard();
				theStock.RemoveTopCard();
			}

			// Draw the screen
			this.Refresh();
		}

		/// <summary>
		/// Attempts to make the move specified
		/// </summary>
		/// <param name="oldPosition">Position of first card</param>
		/// <param name="newPosition">Position of second card</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;
			if (IsAdjacent(oldPosition, newPosition) && Card.RankFromCardIndex(theTableau[oldPosition].Card) ==
				Card.RankFromCardIndex(theTableau[newPosition].Card))
			{
				RemoveCard(oldPosition);
				RemoveCard(newPosition);
				cardsPlayed += 2;
			}
			else
			{
				InvalidMove(oldPosition);
			}

			selectedCard = -1;
			CheckGameOver();

		}

		/// <summary>
		/// Determines if the two cards are adjacent to each other
		/// </summary>
		/// <param name="oldPosition">Position of first card</param>
		/// <param name="newPosition">Position of second card</param>
		/// <returns>Whether or not the two cards are adjacent</returns>
		private bool IsAdjacent(int oldPosition, int newPosition)
		{	
			int oldRow = oldPosition / 5;
			int oldColumn = oldPosition % 5;
			int newRow = newPosition / 5;
			int newColumn = newPosition % 5;

			return (oldRow == newRow - 1 || oldRow == newRow || oldRow == newRow + 1) &&
				(oldColumn == newColumn - 1 || oldColumn == newColumn || oldColumn == newColumn + 1);
		}

		/// <summary>
		/// Draws the Cards
		/// </summary>
		private void DrawCards()
		{
			drawCard.Begin(this.CreateGraphics());
			for (int column = 0; column < 5; column++)
			{
				for (int row = 0; row < 5; row ++)
				{
					int index = row * 5 + column;
					if (theTableau[index].Active)
					{
						if (selectedCard == index)
						{
							drawCard.DrawHighlightedCard(new Point(LeftX + CardWidth * column, TopY +
								CardHeight * row), theTableau[index].Card);
						}
						else
						{
							drawCard.DrawCard(new Point(LeftX + CardWidth * column, TopY +
								CardHeight * row), theTableau[index].Card);
						}
					}
					else
					{
						drawCard.DrawCardBack(new Point(LeftX + CardWidth * column, 
							TopY + CardHeight * row), CardBack.O);
					}
				}
			}

			DrawStock();
			drawCard.End();
		}

		/// <summary>
		/// Draws the stock pile
		/// </summary>
		private void DrawStock()
		{
			// Erase the card areas
			Rectangle oldCardArea = new Rectangle(StockX, StockY, CardWidth+3, CardHeight+3);
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
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="index">Index of the card (0-24)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int index, bool selected)
		{
			int row = index / 5;
			int column = index % 5;

			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();
			cardLocation.X = LeftX + CardWidth * column;
			cardLocation.Y = TopY + CardHeight * row;

			if (selected)
				drawCard.DrawHighlightedCard(cardLocation, theTableau[index].Card);
			else
				drawCard.DrawCard(cardLocation, theTableau[index].Card);
		
			drawCard.End();
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="index"></param>
		private void RemoveCard(int index)
		{
			int row = index / 5;
			int column = index % 5;
			theTableau[index].Active = false;
		
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();
			cardLocation.X = LeftX + CardWidth * column;
			cardLocation.Y = TopY + CardHeight * row;
			drawCard.DrawCardBack(cardLocation, CardBack.O);	
			drawCard.End();			
		}

		/// <summary>
		/// Deals cards from the stock pile to the waste pile
		/// </summary>
		private void DealStock()
		{
			MonteCarloCard[] oldTableau = theTableau;
			int newindex = 0;

			// Move all of the active cards up
			for (int index = 0; index < 25; index++)
			{
				if (oldTableau[index].Active)
				{
					theTableau[newindex] = oldTableau[index];
					newindex++;
				}
			}

			// Draw new cards to fill the rest of the tableau
			for (int count = newindex; count < 25; count++)
			{
				if (theStock.Count > 0)
				{
					theTableau[count].Active = true;
					theTableau[count].Card = theStock.GetTopCard();
					theStock.RemoveTopCard();
				}
				else
				{
					theTableau[count].Active = false;
				}
			}

			// Redraw the screen
			DrawCards();
		}

		/// <summary>
		/// Determines whether the game is won or not
		/// </summary>
		private void CheckGameOver()
		{
			bool gameover = (theStock.Count == 0);
			for (int count = 0; count < 25 && gameover; count++)
			{
				gameover = !theTableau[count].Active;
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

		/// <summary>
		/// Code to handle an invalid select
		/// </summary>
		private void InvalidSelect()
		{
			if (CardGames.Preferences.SoundEnabled)
				Sound.MessageBeep((Int32)BeepTypes.Ok);
		}
		#endregion

		#region Debug Methods
		private void DEBUG_EmptyTableauAndStock()
		{
			theStock.Clear();
			for (int count = 0; count < 25; count++)
			{
				theTableau[count].Active = false;
			}
			CheckGameOver();
		}
		#endregion
	}
}
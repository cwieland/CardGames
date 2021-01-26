namespace CardGames.Golf
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
	/// This is a simple implementation of the solitare game Golf.
	/// </summary>
	public class Golf : System.Windows.Forms.Form
	{
		#region Constants
		private const int StockX = 25;								// The left most position of the stock pile
		private const int StockY = 275;								// The top most position of the stock pile
		private const int TableauX = 25;							// The left most position of the tableau
		private const int TableauY = 20;							// The top most position of the tableau
		private const int TableauHeight = 35;						// The height of the tableau pile cards
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		private const int WasteX = 115;								// The left most position of the waste pile
		private const int WasteWidth = 15;							// The width of the waste cards
		#endregion

		#region Private Fields
		private Deck theDeck;										// The data structure for the deck of cards
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private CardBack currentCardBack;							// The selected card back
		private CardCollection[] theTableau;						// The data structure for the tableau
		private CardCollection theStock;							// The data structure for the stock pile
		private CardCollection theFoundation;						// The data structure for the foundation pile
		private int cardsPlayed;									// The number of cards played
		private bool finished;										// Whether the hand is finished or not
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
		public Golf()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			theStock = new CardCollection();
			theFoundation = new CardCollection();
			theTableau = new CardCollection[7];
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
			// Golf
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(600, 392);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Golf";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Golf";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Calculation_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Golf_Closing);
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
			for (int column = 0; column < 9; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;

				if (column < 7)
				{
					// Clicked on the tableau
					left = column * CardWidth + TableauX;
					top = TableauY;
					bottom = top + CardHeight;

					if (theTableau[column].Count > 0)
						bottom += (theTableau[column].Count - 1) * TableauHeight;
				}
				else if (column == 7)
				{
					// Clicked on the stock pile
					int stacksize = 3;
					if (theStock.Count < stacksize)
						stacksize = theStock.Count;
					left = StockX + stacksize;
					top = StockY + stacksize;
					bottom = top + CardHeight;
				}
				else if (column == 8)
				{
					// Clicked on the waste pile
					int stacksize = 3;
					if (theFoundation.Count < stacksize)
						stacksize = theFoundation.Count;
					
					if (stacksize == 0)
					{
						left = WasteX;
						top = StockY;
					}
					else
					{
						left = WasteX + (stacksize - 1) * WasteWidth;
						top = StockY;
					}

					bottom = top + CardHeight;
				}

				int right = left + CardWidth;
				
				if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
				{
					if (column < 7)
					{
						AttemptMove(column);
					}
					else if (column == 7)
					{
						DealStock();
					}
					else if (column == 8)
					{
						InvalidMove();
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
		private void Golf_Closing(object sender, CancelEventArgs e)
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
		/// <summary>
		/// Deals a new hand
		/// </summary>
		private void NewHand()
		{
			theDeck.Shuffle();
			cardsPlayed = 0;
			finished = true;

			// Reset all card piles
			theStock.Clear();
			theFoundation.Clear();

			for (int count = 0; count < 7; count++)
			{
				theTableau[count].Clear();
			}
					
			// Start the tableau with the next four cards
			for (int count = 0; count < 35; count++)
			{
				theTableau[count % 7].Add(theDeck[count]);
			}
	
			// Play the first card of the from the stock
			theFoundation.Add(theDeck[35]);
			cardsPlayed += 1;

			// Fill the stock pile with the rest of the deck
			for (int count = 51; count > 35; count--)
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
			DrawStock();
			DrawFoundation();

			for (int column = 0; column < 7; column++)
			{
				// Draw the tableau
				if (theTableau[column].Count > 0)
					for (int count = 0; count < theTableau[column].Count; count++)
						drawCard.DrawCard(new Point(CardWidth * column + TableauX, TableauHeight * count + TableauY),
							theTableau[column][count]);
			}
			
			drawCard.End();
		}

		/// <summary>
		/// Draws the stock pile
		/// </summary>
		private void DrawStock()
		{
			// Erase the stock pile area
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
		/// Draws the foundation pile area
		/// </summary>
		private void DrawFoundation()
		{
			// Erase the waste pile area
			Rectangle oldCardArea = new Rectangle(WasteX, StockY, CardWidth+WasteWidth*3, CardHeight);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			
			// Draw the waste pile
			if (theFoundation.Count > 0)
			{
				int stacksize = 3;
				if (theFoundation.Count < stacksize)
					stacksize = theFoundation.Count;

				for (int count = 0; count < stacksize; count++)
				{
					drawCard.DrawCard(new Point(WasteX + WasteWidth * count, StockY),
						theFoundation[theFoundation.Count-(stacksize-count)]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(WasteX, StockY), CardBack.Unused);
			}
		}
		
		/// <summary>
		/// Deals the next card from the stock pile
		/// </summary>
		private void DealStock()
		{
			if (theStock.Count > 0)
			{
				// Deal the next card from stock to waste
				theFoundation.Add(theStock.GetTopCard());
				theStock.RemoveTopCard();
				cardsPlayed += 1;
						
				// Redraw the stock and waste
				drawCard.Begin(this.CreateGraphics());
				DrawStock();
				DrawFoundation();
				drawCard.End();
			}
			else
			{
				InvalidMove();
			}
		}

		/// <summary>
		/// Attempt a move
		/// </summary>
		/// <param name="Position">Position of card to play (0-6)</param>
		private void AttemptMove(int Position)
		{
			finished = false;
			
			if (theTableau[Position].Count > 0)
			{
				CardRank selectedCardRank = Card.RankFromCardIndex(theTableau[Position].GetTopCard());
				CardRank wasteCardRank = Card.RankFromCardIndex(theFoundation.GetTopCard());

				if (selectedCardRank == wasteCardRank + 1 || selectedCardRank == wasteCardRank - 1)
				{
					RemoveCard(Position);
				}
				else
				{
					InvalidMove();
				}
			}
			else
			{
				InvalidMove();
			}

			CheckGameOver();
		}

		/// <summary>
		/// Determines whether the game is won or not
		/// </summary>
		private void CheckGameOver()
		{
			bool gameover = true;

			// See if the tableau and stock piles are empty
			for (int count = 0; count < 8 && gameover; count++)
			{
				if (count < 7)
				{
					// Check each row of the tableau
					gameover = theTableau[count].Count == 0;
				}
				else
				{
					// Check the stock pile
					gameover = theStock.Count == 0;
				}
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

		private void RemoveCard(int column)
		{
			theFoundation.Add(theTableau[column].GetTopCard());
			theTableau[column].RemoveTopCard();
			cardsPlayed += 1;
			int x = column*CardWidth+TableauX;
			int y = (theTableau[column].Count)*TableauHeight+TableauY;
			Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
			Graphics windowGraphics = this.CreateGraphics();
			windowGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
			drawCard.Begin(windowGraphics);
			if (theTableau[column].Count > 0)
			{
				drawCard.DrawCard(new Point(CardWidth * column + TableauX,
					TableauHeight *	(theTableau[column].Count - 1) + TableauY), theTableau[column].GetTopCard());
			}
			DrawFoundation();
			drawCard.End();
		}

		/// <summary>
		/// Code to handle a invalid move
		/// </summary>
		private void InvalidMove()
		{
			if (CardGames.Preferences.SoundEnabled)
				Sound.MessageBeep((Int32)BeepTypes.Ok);
		}
		#endregion

	}
}
// Beleguered Castle task list:
//  Fix highlighted card bug (on refresh)

namespace CardGames.BelegueredCastle
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
	/// This is a simple implementation of the solitare game Beleguered Castle.
	/// </summary>
	public class BelegueredCastle : System.Windows.Forms.Form
	{
		#region Constants
		private const int TopY = 20;								// The top most position of Reserve and Foundation piles
		private const int FoundationX = 200;						// The left most position of the Reserve piles
		private const int DrawingHeight = 110;						// The height of the cards displayed in the Foundation piles
		private const int TableauLeftX = 25;						// The left most position of the left tableau
		private const int TableauRightX = 300;						// The left most position of the right tableau
		private const int TableauWidth = 15;						// The width of the dealt cards in the tableau
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private Foundation theFoundation;							// The data structure for the foundation
		private CardCollection[] theTableau;						// The data structure for the tableau
		private int[] originalCount;								// The number of cards that were dealt in pile
		private Deck theDeck;										// The data structure for the deck of cards
		private int selectedCard;									// The currently selected card
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
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
		private System.Windows.Forms.MenuItem ViewStats;			// The Preferences menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public BelegueredCastle()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			originalCount = new int[8];
			theTableau = new CardCollection[8];
			theFoundation = new Foundation();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			for (int count = 0; count < 8; count++)
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
																					this.ResetStats});
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
			// BelegueredCastle
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(469, 472);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "BelegueredCastle";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Beleguered Castle";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BelegueredCastle_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BelegueredCastle_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BelegueredCastle_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// Handles any mouse click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BelegueredCastle_MouseDown(object sender, MouseEventArgs e)
		{
			int oldSelection = selectedCard;
			
			for (int column = 0; column < 12; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;
				int right = 0;

				if (column < 4)
				{
					int space = theTableau[column].Count - originalCount[column];

					left = TableauLeftX + space;
					top = TopY + DrawingHeight * column + space;
					bottom = top + CardHeight;
					right = left + CardWidth + TableauWidth * (originalCount[column] - 1);					

					if (originalCount[column] == 0)
						right = left + CardWidth;
				}
				else if (column < 8)
				{
					left = FoundationX;
					top = TopY + DrawingHeight * (column - 4);
					bottom = top + CardHeight;
					right = left + CardWidth;
				}
				else
				{
					int space = theTableau[column - 4].Count - originalCount[column - 4];

					left = TableauRightX + space;
					top = TopY + DrawingHeight * (column - 8) + space;
					bottom = top + CardHeight;
					right = left + CardWidth + TableauWidth * (originalCount[column - 4] - 1);

					if (originalCount[column - 4] == 0)
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
							bool selectFromLeftTableau = (column < 4 && theTableau[column].Count > 0);
							bool selectFromRightTableau = (column > 7 && theTableau[column - 4].Count > 0);
							if (selectFromLeftTableau || selectFromRightTableau)
							{
								selectedCard = column;
								ToggleCardSelect(column, true);
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
		private void BelegueredCastle_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BelegueredCastle_Closing(object sender, CancelEventArgs e)
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
			cardsPlayed = 4;
			finished = true;

			// Clear the Foundation and Reserve
			for (int row = 0; row < 4; row++)
				theFoundation[row].Clear();
			for (int row = 0; row < 8; row++)
			{
				theTableau[row].Clear();
				originalCount[row] = 6;
			}

			// Setup the variables to keep track of current pile
			int currentFoundationPile = 0;
			int currentTableauPile = 0;
			
			// Deal the cards
			for (int count = 0; count < 52; count++)
			{
				if (Card.RankFromCardIndex(theDeck[count]) == CardRank.Ace)
				{
					theFoundation[currentFoundationPile].Add(theDeck[count]);
					currentFoundationPile++;
				}
				else
				{
					theTableau[currentTableauPile].Add(theDeck[count]);
					if (theTableau[currentTableauPile].Count == 6)
						currentTableauPile++;
				}
			}

			// Draw the screen
			this.Refresh();
		}

		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			// Moving card to left tableau
			if (newPosition < 4)
			{
				if (oldPosition < 4)
				{
					// Moving from the left tableau
					bool validMove = false;
					if (theTableau[newPosition].Count > 0)
					{
						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition].GetTopCard());
						validMove = (baseRank + 1 == newRank);
					}

					if (validMove || theTableau[newPosition].Count == 0)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}
				else if (oldPosition > 7)
				{
					// Moving from the right tableau
					bool validMove = false;
					if (theTableau[newPosition].Count > 0)
					{
						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition].GetTopCard());
						validMove = (baseRank + 1 == newRank);
					}

					if (validMove || theTableau[newPosition].Count == 0)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}
			}
			else if (newPosition < 8)
			{
				// Move to the foundation
				if (oldPosition < 4)
				{
					// Moving from the left tableau
					CardRank baseRank = Card.RankFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
					CardRank newRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
					CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
					CardSuit newSuit = Card.SuitFromCardIndex(theTableau[oldPosition].GetTopCard());

					if (baseRank + 1 == newRank && baseSuit == newSuit)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}
				else
				{
					// Moving from the right tableau
					CardRank baseRank = Card.RankFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
					CardRank newRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
					CardSuit baseSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 4].GetTopCard());
					CardSuit newSuit = Card.SuitFromCardIndex(theTableau[oldPosition - 4].GetTopCard());

					if (baseRank + 1 == newRank && baseSuit == newSuit)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}
			}
			else
			{
				// Move to the right tableau
				if (oldPosition < 4)
				{
					// Moving from the left tableau
					bool validMove = false;
					if (theTableau[newPosition - 4].Count > 0)
					{
						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition - 4].GetTopCard());
						validMove = (baseRank + 1 == newRank);
					}

					if (validMove || theTableau[newPosition - 4].Count == 0)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}
				else if (oldPosition > 7)
				{
					// Moving from the right tableau
					bool validMove = false;
					if (theTableau[newPosition - 4].Count > 0)
					{
						CardRank baseRank = Card.RankFromCardIndex(theTableau[oldPosition - 4].GetTopCard());
						CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition - 4].GetTopCard());
						validMove = (baseRank + 1 == newRank);
					}

					if (validMove || theTableau[newPosition - 4].Count == 0)
						MoveCard(oldPosition, newPosition);
					else
						InvalidMove(oldPosition);
				}			
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
				if (theTableau[column].Count > 0)
				{
					for (int count = 0; count < originalCount[column]; count++)
					{
						drawCard.DrawCard(new Point(TableauLeftX + TableauWidth * count,
							TopY + DrawingHeight * column), theTableau[column][count]);
					}
					
					for (int count = originalCount[column]; count < theTableau[column].Count; count++)
					{
						int space = count - originalCount[column] + 1;
						int x = TableauLeftX;
						
						if (originalCount[column] > 0)
							x += TableauWidth * (originalCount[column] - 1);

						drawCard.DrawCard(new Point(x + space, TopY + DrawingHeight * column + space),
							theTableau[column].GetTopCard());
					}
				}
				else
				{
					drawCard.DrawCardBack(new Point(TableauLeftX, TopY + DrawingHeight * column), CardBack.O);
				}

				drawCard.DrawCard(new Point(FoundationX, TopY + DrawingHeight * column),
					theFoundation[column].GetTopCard());

				if (theTableau[column + 4].Count > 0)
				{
					for (int count = 0; count < originalCount[column + 4]; count++)
					{
						drawCard.DrawCard(new Point(TableauRightX + TableauWidth * count,
							TopY + DrawingHeight * column), theTableau[column + 4][count]);
					}
					for (int count = originalCount[column + 4]; count < theTableau[column + 4].Count; count++)
					{
						int space = count - originalCount[column + 4] + 1;
						int x = TableauRightX;
						
						if (originalCount[column + 4] > 0)
							x += TableauWidth * (originalCount[column + 4] - 1);

						drawCard.DrawCard(new Point(x + space, TopY + DrawingHeight * column + space),
							theTableau[column + 4].GetTopCard());
					}
				}
				else
				{
					drawCard.DrawCardBack(new Point(TableauRightX, TopY + DrawingHeight * column), CardBack.O);
				}
			}

			drawCard.End();
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
				int space = theTableau[column].Count - originalCount[column];

				cardLocation.X = TableauLeftX + space;
				cardLocation.Y = TopY + DrawingHeight * column + space;

				if (originalCount[column] > 0)
					 cardLocation.X += TableauWidth * (originalCount[column] - 1);
							
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[column].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[column].GetTopCard());
			}
			else if (column > 7)
			{
				int space = theTableau[column - 4].Count - originalCount[column - 4];

				cardLocation.X = TableauRightX + space;
				cardLocation.Y = TopY + DrawingHeight * (column - 8) + space;

				if (originalCount[column - 4] > 0)
					cardLocation.X += TableauWidth * (originalCount[column - 4] - 1);
							
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[column - 4].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[column - 4].GetTopCard());	
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
				movingCard = theTableau[oldPosition].GetTopCard();
			else if (oldPosition > 7)
				movingCard = theTableau[oldPosition - 4].GetTopCard();

			// add it to new location if it is valid
			if (movingCard < 52)
			{
				if (newPosition < 4)
				{
					theTableau[newPosition].Add(movingCard);
					int space = theTableau[newPosition].Count - originalCount[newPosition];
					
					drawCard.Begin(this.CreateGraphics());
					Point cardLocation = new Point();
					cardLocation.X = TableauLeftX + space;
					cardLocation.Y = TopY + DrawingHeight * (newPosition) + space;

					if (originalCount[newPosition] > 0)
						 cardLocation.X += TableauWidth * (originalCount[newPosition] - 1);

					drawCard.DrawCard(cardLocation, theTableau[newPosition].GetTopCard());
					drawCard.End();
				}
				else if (newPosition < 8)
				{
					theFoundation[newPosition - 4].Add(movingCard);
					drawCard.Begin(this.CreateGraphics());
					drawCard.DrawCard(new Point(FoundationX, TopY + DrawingHeight * (newPosition - 4)),
						theFoundation[newPosition - 4].GetTopCard());
					drawCard.End();
					cardsPlayed++;
				}
				else
				{
					theTableau[newPosition - 4].Add(movingCard);
					int space = theTableau[newPosition - 4].Count - originalCount[newPosition - 4];

					drawCard.Begin(this.CreateGraphics());
					Point cardLocation = new Point();
					cardLocation.X = TableauRightX + space;
					cardLocation.Y = TopY + DrawingHeight * (newPosition - 8) + space;

					if (originalCount[newPosition - 4] > 0)
						cardLocation.X += TableauWidth * (originalCount[newPosition - 4] - 1);

					drawCard.DrawCard(cardLocation, theTableau[newPosition - 4].GetTopCard());
					drawCard.End();
				}

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
			if (column < 4) 
			{
				int space = theTableau[column].Count - originalCount[column];
				int x = TableauLeftX + space;
				if (originalCount[column] > 0)
					x += (originalCount[column] - 1) * TableauWidth;
				int y = TopY + column * DrawingHeight + space;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);

				theTableau[column].RemoveTopCard();
				if (originalCount[column] > theTableau[column].Count)
					originalCount[column]--;
				space--;

				drawCard.Begin(this.CreateGraphics());
							
				if (theTableau[column].Count == 0)
				{
					drawCard.DrawCardBack(new Point(TableauLeftX, y), CardBack.O);
				}
				else
				{
					if (space < 0)
						space = 0;

					if (originalCount[column] > 0)
					{
						drawCard.DrawCard(new Point(TableauLeftX + (originalCount[column] - 1) * TableauWidth + space,
							TopY + column * DrawingHeight + space),theTableau[column].GetTopCard());
					}
					else
					{
						drawCard.DrawCard(new Point(TableauLeftX + space, TopY + column *
							DrawingHeight + space), theTableau[column].GetTopCard());
					}
				}

				drawCard.End();
			}
			else if (column > 7)
			{
				int space = theTableau[column - 4].Count - originalCount[column - 4];
				int x = TableauRightX + space;
				if (originalCount[column - 4] > 0)
					x += (originalCount[column - 4] - 1) * TableauWidth;
				int y = TopY + (column - 8) * DrawingHeight + space;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				
				theTableau[column - 4].RemoveTopCard();
				if (originalCount[column - 4] > theTableau[column - 4].Count)
					originalCount[column - 4]--;
				space--;

				drawCard.Begin(this.CreateGraphics());
							
				if (theTableau[column - 4].Count == 0)
				{
					drawCard.DrawCardBack(new Point(TableauRightX, y), CardBack.O);
				}
				else
				{
					if (space < 0)
						space = 0;

					if (originalCount[column - 4] > 0)
					{
						drawCard.DrawCard(new Point(TableauRightX + (originalCount[column - 4] - 1) * TableauWidth + space,
							TopY + (column - 8) * DrawingHeight + space), theTableau[column - 4].GetTopCard());
					}
					else
					{
						drawCard.DrawCard(new Point(TableauRightX + space, TopY + (column - 8) *
							DrawingHeight + space),	theTableau[column - 4].GetTopCard());
					}
				}

				drawCard.End();
			}
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
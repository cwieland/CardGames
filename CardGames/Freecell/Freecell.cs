// Freecell wish list:
//  Fix highlighted card bug, breaks app (on refresh)
//  auto play to foundation as option (card should move when both cards of opposite color and rank-1 are in foundation)
//  double click to move card to foundation (if it will play)
//  implement an Undo option (one card)
//  column move logic
//  game over logic (out of moves (lose))

namespace CardGames.Freecell
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
	/// This is a simple implementation of the solitare game Freecell.
	/// </summary>
	public class Freecell : System.Windows.Forms.Form
	{
		#region Constants
		private const int LeftX = 15;								// The left most position of the Tableau and Freecells
		private const int FoundationX = 392;						// The left most position of the Foundation
		private const int TableauY = 125;							// The top most position of the Tableau
		private const int TopY = 10;								// The top most position of the Freecells and the Foundation
		private const int TableauHeight = 17;						// The height of cards in the tableau
		private const int TableauWidth = 88;						// The width of cards in the tableau
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private FreecellReserve theFreecells;						// The data structure for the freecells
		private FreecellTableau theTableau;							// The data structure for the tableau
		private Foundation theFoundation;							// The data structure for the foundation
		private Deck theDeck;										// The data structure for the deck of cards
		private Stats theStats;										// The statistics for this game
		private int selectedCard;									// The selected card (0-7 tableau, 8-11 freecells)
		private bool finished;										// The hand is finished
		private bool[] refreshColumn;								// Holds the refreshColumn flags for each column
		private Card drawCard;										// Card object used to draw the cards
		private int cardsPlayed;									// Number of cards played in this hand
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
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public Freecell()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			refreshColumn = new bool[16];
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
			// Freecell
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(714, 449);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Freecell";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Freecell";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Freecell_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Freecell_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Freecell_Paint);

		}
		#endregion

		#region Form Events
		/// <summary>
		/// Redraws the screen.  Sets all of the refreshColumn flags and class the DrawCards method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Freecell_Paint(object sender, PaintEventArgs e)
		{
			// Set the refreshColumn flags
			for (int count = 0; count < 16; count++)
				refreshColumn[count] = true;
			DrawCards();
		}

		/// <summary>
		/// Fires when the user clicks the mouse button.  Used to select and move cards.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Freecell_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			// Check the tableau
			for (int column = 0; column < 16; column++)
			{
				int left = 0;
				int top = 0;

				if (column < 8)
				{
					left = column * TableauWidth + LeftX;
					top = (theTableau[column].Count - 1) * TableauHeight + TableauY;
				}
				else if (column < 12)
				{
					left = (column-8) * CardWidth + LeftX;
					top = TopY;
				}
				else
				{
					left = (column-12) * CardWidth + FoundationX;
					top = TopY;
				}

				int right = left + CardWidth;
				int bottom = top + CardHeight;

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
							bool selectFromTableau = (column < 8 && theTableau[column].Count > 0);
							bool selectFromFreecells = (column > 7 && column < 12 && !theFreecells.Empty(column-8));
							if (selectFromTableau || selectFromFreecells)
							{
								selectedCard = column;
								ToggleCardSelect(column, true);
							}
						} 
						else if (e.Button == MouseButtons.Right)
						{
							int count = 0;
							bool foundempty = false;
							while (count < 4 && !foundempty) 
							{
								if (theFreecells.Empty(count))
								{
									AttemptMove(column, count + 8);
									foundempty = true;
								}
								count++;
							}
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
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Freecell_Closing(object sender, CancelEventArgs e)
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
		/// This event is fired when the user selects Exit from the File Menu
		/// It closes the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Exit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// This event is fired when the user selects New Game from the File Menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void New_Click(object sender, System.EventArgs e)
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
			// Select no card and reset the finished flag
			cardsPlayed = 0;
			selectedCard = -1;
			finished = true;
			
			// Shuffle the deck and initialize the data structures
			theDeck.Shuffle();
			theFreecells = new FreecellReserve();
			theTableau = new FreecellTableau();
			theFoundation = new Foundation();

			// Fill the Tableau using the deck
			for (int count = 0; count < 52; count++)
			{
				theTableau[count % 8].Add(theDeck[count]);
			}
			this.Refresh();
		}

		/// <summary>
		/// Draws the cards.  Only draws areas that have refreshColumn flag set
		/// </summary>
		private void DrawCards()
		{
			// Draw the freecells and the foundation
			drawCard.Begin(this.CreateGraphics());
			
			for (int column = 0; column < 8; column++)
			{				
				if (column < 4) 
				{
					if (refreshColumn[column+8])
					{
						// Draw the freecells
						if (theFreecells.Empty(column))
							drawCard.DrawCardBack(new Point(column*CardWidth+LeftX, TopY), CardBack.O);
						else if (selectedCard == column + 8)
							drawCard.DrawHighlightedCard(new Point(column*CardWidth+LeftX, TopY), theFreecells[column]);
						else
							drawCard.DrawCard(new Point(column*CardWidth+LeftX, TopY), theFreecells[column]);
					}

					if (refreshColumn[column+12])
					{
						// Draw the foundation
						if (theFoundation[column].Count == 0)
							drawCard.DrawCardBack(new Point(column*CardWidth+FoundationX, TopY), CardBack.O);
						else
							drawCard.DrawCard(new Point(column*CardWidth+FoundationX, TopY), theFoundation[column].GetTopCard());					
					}
				}

				// Draw the tableau
				if (refreshColumn[column])
				{
					if (theTableau[column].Count == 0)
						drawCard.DrawCardBack(new Point(column*TableauWidth+LeftX, TableauY), CardBack.O);
					int startcount = theTableau[column].Count - 15;
					if (startcount < 0) startcount = 0;
					for (int count = startcount; count < theTableau[column].Count; count++)
						if (selectedCard == column && theTableau[column].Count == count + 1)
							drawCard.DrawHighlightedCard(new Point(column*TableauWidth+LeftX, count*TableauHeight/(int)(theTableau[column].Count / 12)+TableauY), theTableau[column][count]);
						else
							drawCard.DrawCard(new Point(column*TableauWidth+LeftX, count*TableauHeight+TableauY), theTableau[column][count]);
				}
			}

			// Finish drawing and reset the refreshColumn array
			drawCard.End();

			for (int count=0; count < 16; count++)
				refreshColumn[count] = false;
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column"></param>
		private void RemoveCard(int column)
		{
			if (column < 8) 
			{
				theTableau[column].RemoveTopCard();
				int x = column*TableauWidth+LeftX;
				int y = (theTableau[column].Count)*TableauHeight+TableauY;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				
			}
			else if (column < 12)
			{
				theFreecells.RemoveCard(column-8);
			}
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="column">Column of the card (0-7 tableau, 8-11 freecell)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int column, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();
			int cardValue = 0;

			if (column < 8)
			{
				cardLocation.X = column*TableauWidth+LeftX;
				cardLocation.Y = (theTableau[column].Count-1)*TableauHeight+TableauY;
				cardValue = theTableau[column].GetTopCard();
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, cardValue);
				else
					drawCard.DrawCard(cardLocation, cardValue);
			}
			else if (column < 12)
			{
				cardLocation.X = (column-8) * CardWidth + LeftX;
				cardLocation.Y = TopY;
				cardValue = theFreecells[column-8];

				// Draw the freecells
				if (theFreecells.Empty(column-8))
					drawCard.DrawCardBack(cardLocation, CardBack.O);
				else if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theFreecells[column-8]);
				else
					drawCard.DrawCard(cardLocation, theFreecells[column-8]);
			}

			drawCard.End();
		}

		/// <summary>
		/// Processes a move.
		/// </summary>
		/// <param name="oldPosition">Current position of card (0-7 tableau, 8-11 freecells)</param>
		/// <param name="newPosition">Attempted new position of card (0-7 tableau, 8-11 freecells, 12-15 foundation)</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			// attempting to move card to tableau
			if (newPosition < 8)
			{
				if (oldPosition < 8)								// from another row in tableau
				{
					if (theTableau[newPosition].Count == 0)
					{
						MoveCard(oldPosition, newPosition);
					}
					else 
					{
						int movingCard = theTableau[oldPosition].GetTopCard();
						int stationaryCard = theTableau[newPosition].GetTopCard();
						if (IsValidTableauMove(stationaryCard, movingCard))
							MoveCard(oldPosition, newPosition);
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
				else if (oldPosition < 12)							// from a freecell
				{
					if (theTableau[newPosition].Count == 0)
						MoveCard(oldPosition, newPosition);
					else 
					{
						int movingCard = theFreecells[oldPosition-8];
						int stationaryCard = theTableau[newPosition].GetTopCard();
						if (IsValidTableauMove(stationaryCard, movingCard))
							MoveCard(oldPosition, newPosition);
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
			}
			
			// attempting to move card to one of the freecells
			else if (newPosition < 12)
			{
				if (theFreecells.Empty(newPosition-8)) 
				{
					if (oldPosition < 12)								// from tableau or another freecell
						MoveCard(oldPosition, newPosition);
				}
				else
				{
					InvalidMove(oldPosition);
				}
			}
			
			// attempting to move card to the foundation
			else
			{
				int currentCard = 0;
				if (oldPosition < 8)								// from another row in tableau
					currentCard = theTableau[oldPosition].GetTopCard();
				else if (oldPosition < 12)							// from a freecell
					currentCard = theFreecells[oldPosition-8];

				int count = theFoundation[newPosition-12].Count;
				if (count == 0)	// pile is empty
				{
					if (Card.RankFromCardIndex(currentCard) == CardRank.Ace)
					{
						cardsPlayed++;
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
				else											// the pile has cards
				{
					// verify the card's rank and suit
					bool RankOK = (int)Card.RankFromCardIndex(currentCard) == count;
					bool SuitOK = Card.SuitFromCardIndex(currentCard) == 
						Card.SuitFromCardIndex(theFoundation[newPosition-12].GetTopCard());

					// execute the move if it is valid or unselect if it is not
					if (RankOK && SuitOK)
					{
						cardsPlayed++;
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
			}

			// Reset the selected card and draw the changes
			selectedCard = -1;
			DrawCards();
			CheckGameOver();
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
			valid = (valid && ((int)stationaryRank - 1 == (int)movingRank));
			return valid;
		}

		/// <summary>
		/// Does a card move
		/// </summary>
		/// <param name="oldPosition">Old position of card</param>
		/// <param name="newPosition">New position of card</param>		
		private void MoveCard(int oldPosition, int newPosition)
		{
			// Get the value of the card
			int movingCard;
			if (oldPosition < 8)
				movingCard = theTableau[oldPosition].GetTopCard();
			else
				movingCard = theFreecells[oldPosition-8];

			// add it to new location
			if (newPosition < 8)
				theTableau[newPosition].Add(movingCard);
			else if (newPosition < 12)
				theFreecells[newPosition-8] = movingCard;
			else
				theFoundation[newPosition-12].Add(movingCard);

			// remove it from old location and set refreshColumn flags
			RemoveCard(oldPosition);
			refreshColumn[newPosition] = true;
			refreshColumn[oldPosition] = true;
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
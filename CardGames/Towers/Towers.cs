// Towers wish list:
//  Fix highlighted card bug, breaks app (on refresh)

namespace CardGames.Towers
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
	/// This is a simple implementation of the solitare game Towers.
	/// </summary>
	public class Towers : System.Windows.Forms.Form
	{
		#region Constants
		private const int FoundationLeftX = 10;						// The left most position of the left-side Foundation
		private const int FoundationRightX = 642;					// The left most position of the right-side Foundation
		private const int ReserveX = 247;							// The left most position of the reserve cells
		private const int TableauX = 10;							// The left most position of the Tableau and cells
		private const int TableauY = 125;							// The top most position of the Tableau
		private const int TopY = 10;								// The top most position of the cells and the Foundation
		private const int TableauHeight = 23;						// The height of cards in the tableau
		private const int TableauWidth = 79;						// The width of cards in the tableau
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private Reserve theCells;									// The data structure for the cells
		private CardCollection[] theTableau;						// The data structure for the tableau
		private Foundation theFoundation;							// The data structure for the foundation
		private Deck theDeck;										// The data structure for the deck of cards
		private Stats theStats;										// The statistics for this game
		private int selectedCard;									// The selected card (0-9 tableau, 10-13 cells)
		private bool finished;										// The hand is finished
		private bool[] refreshColumn;								// Holds the refreshColumn flags for each column
		private Card drawCard;										// Card object used to draw the cards
		private int cardsPlayed;									// Number of cards played in this hand
		private bool autoplay;										// Whether or not cards should automatically move to foundation
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
		private System.Windows.Forms.MenuItem GameOptions;			// The View stats menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public Towers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			refreshColumn = new bool[18];
			GetPreferences();
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
			this.GameOptions = new System.Windows.Forms.MenuItem();
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
																					this.GameOptions});
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
			// GameOptions
			// 
			this.GameOptions.Index = 3;
			this.GameOptions.Text = "&Game Options";
			this.GameOptions.Click += new System.EventHandler(this.GameOptions_Click);
			// 
			// Towers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(800, 451);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Towers";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Towers";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Towers_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Towers_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Towers_Paint);

		}
		#endregion

		#region Form Events
		/// <summary>
		/// Redraws the screen.  Sets all of the refreshColumn flags and class the DrawCards method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Towers_Paint(object sender, PaintEventArgs e)
		{
			// Set the refreshColumn flags
			for (int count = 0; count < 18; count++)
				refreshColumn[count] = true;
			DrawCards();
		}

		/// <summary>
		/// Fires when the user clicks the mouse button.  Used to select and move cards.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Towers_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			// Check the tableau
			for (int column = 0; column < 18; column++)
			{
				int left = 0;
				int top = 0;

				if (column < 10)
				{
					left = column * TableauWidth + TableauX;
					top = (theTableau[column].Count - 1) * TableauHeight + TableauY;
				}
				else if (column < 14)
				{
					left = (column-10) * CardWidth + ReserveX;
					top = TopY;
				}
				else if (column < 16)
				{
					left = (column-14) * CardWidth + FoundationLeftX;
					top = TopY;
				}
				else
				{
					left = (column-16) * CardWidth + FoundationRightX;
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
							bool selectFromTableau = (column < 10 && theTableau[column].Count > 0);
							bool selectFromCells = (column > 9 && column < 14 && !theCells.Empty(column-10));
							if (selectFromTableau || selectFromCells)
							{
								selectedCard = column;
								ToggleCardSelect(column, true);
							}
						} 
						else if (e.Button == MouseButtons.Right)
						{
							// TODO: implement automatic send to reserve cell
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
		private void Towers_Closing(object sender, CancelEventArgs e)
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

		/// <summary>
		/// This shows a dialog with options specific to this game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameOptions_Click(object sender, System.EventArgs e)
		{
			(new Options()).ShowDialog();
			GetPreferences();
			AutoMoveToFoundation();
			DrawCards();
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
			theCells = new Reserve();
			theFoundation = new Foundation();
			theTableau = new CardCollection[10];
			for (int count = 0; count < 10; count++)
			{
				theTableau[count] = new CardCollection();
			}

			// Fill the Tableau using the deck
			for (int count = 0; count < 50; count++)
			{
				theTableau[count % 10].Add(theDeck[count]);
			}
			
			// Put the last two cards in the reserve cells
			theCells[1] = theDeck[50];
			theCells[2] = theDeck[51];
			AutoMoveToFoundation();
			this.Refresh();
		}

		/// <summary>
		/// Draws the cards.  Only draws areas that have refreshColumn flag set
		/// </summary>
		private void DrawCards()
		{
			// Draw the reserve cells and the foundation
			drawCard.Begin(this.CreateGraphics());
			
			for (int column = 0; column < 10; column++)
			{				
				if (column < 4) 
				{
					if (refreshColumn[column+10])
					{
						// Draw the reserve cells
						if (theCells.Empty(column))
							drawCard.DrawCardBack(new Point(column*CardWidth+ReserveX, TopY), CardBack.O);
						else if (selectedCard == column + 10)
							drawCard.DrawHighlightedCard(new Point(column*CardWidth+ReserveX, TopY), theCells[column]);
						else
							drawCard.DrawCard(new Point(column*CardWidth+ReserveX, TopY), theCells[column]);
					}

					if (refreshColumn[column+14])
					{
						// Draw the foundation
						if (column < 2)
						{
							if (theFoundation[column].Count == 0)
								drawCard.DrawCardBack(new Point(column*CardWidth+FoundationLeftX, TopY), CardBack.O);
							else
								drawCard.DrawCard(new Point(column*CardWidth+FoundationLeftX, TopY), theFoundation[column].GetTopCard());					
						}
						else
						{
							if (theFoundation[column].Count == 0)
								drawCard.DrawCardBack(new Point((column-2)*CardWidth+FoundationRightX, TopY), CardBack.O);
							else
								drawCard.DrawCard(new Point((column-2)*CardWidth+FoundationRightX, TopY), theFoundation[column].GetTopCard());
						}
					}
				}

				// Draw the tableau
				if (refreshColumn[column])
				{
					if (theTableau[column].Count == 0)
						drawCard.DrawCardBack(new Point(column*TableauWidth+TableauX, TableauY), CardBack.O);
					int startcount = theTableau[column].Count - 15;
					if (startcount < 0) startcount = 0;
					for (int count = startcount; count < theTableau[column].Count; count++)
						if (selectedCard == column && theTableau[column].Count == count + 1)
							drawCard.DrawHighlightedCard(new Point(column*TableauWidth+TableauX, count*TableauHeight/(int)(theTableau[column].Count / 12)+TableauY), theTableau[column][count]);
						else
							drawCard.DrawCard(new Point(column*TableauWidth+TableauX, count*TableauHeight+TableauY), theTableau[column][count]);
				}
			}

			// Finish drawing and reset the refreshColumn array
			drawCard.End();

			for (int count=0; count < 18; count++)
				refreshColumn[count] = false;
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column"></param>
		private void RemoveCard(int column)
		{
			if (column < 10) 
			{
				theTableau[column].RemoveTopCard();
				int x = column*TableauWidth+TableauX;
				int y = (theTableau[column].Count)*TableauHeight+TableauY;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				
			}
			else if (column < 14)
			{
				theCells.RemoveCard(column-10);
			}
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="column">Column of the card (0-9 tableau, 10-13 reserve cells)</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int column, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = new Point();
			int cardValue = 0;

			if (column < 10)
			{
				cardLocation.X = column*TableauWidth+TableauX;
				cardLocation.Y = (theTableau[column].Count-1)*TableauHeight+TableauY;
				cardValue = theTableau[column].GetTopCard();
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, cardValue);
				else
					drawCard.DrawCard(cardLocation, cardValue);
			}
			else if (column < 14)
			{
				cardLocation.X = (column-10) * CardWidth + ReserveX;
				cardLocation.Y = TopY;
				cardValue = theCells[column-10];

				// Draw the reserve cells
				if (theCells.Empty(column-10))
					drawCard.DrawCardBack(cardLocation, CardBack.O);
				else if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theCells[column-10]);
				else
					drawCard.DrawCard(cardLocation, theCells[column-10]);
			}

			drawCard.End();
		}

		/// <summary>
		/// Processes a move.
		/// </summary>
		/// <param name="oldPosition">Current position of card (0-9 tableau, 10-13 reserve cells)</param>
		/// <param name="newPosition">Attempted new position of card (0-9 tableau, 10-13 reserve cells, 14-17 foundation)</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			// attempting to move card to tableau
			if (newPosition < 10)
			{
				if (oldPosition < 10)								// from another row in tableau
				{
					if (theTableau[newPosition].Count == 0)
					{
						if (Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard()) == CardRank.King)
						{
							MoveCard(oldPosition, newPosition);
							AutoMoveToFoundation();
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
					else 
					{
						int movingCard = theTableau[oldPosition].GetTopCard();
						int stationaryCard = theTableau[newPosition].GetTopCard();
						if (IsValidTableauMove(stationaryCard, movingCard))
						{
							MoveCard(oldPosition, newPosition);
							AutoMoveToFoundation();
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
				else if (oldPosition < 14)							// from a reserve cell
				{
					if (theTableau[newPosition].Count == 0)
						if (Card.RankFromCardIndex(theCells[oldPosition-10]) == CardRank.King)
						{
							MoveCard(oldPosition, newPosition);
							AutoMoveToFoundation();
						}
						else
						{
							InvalidMove(oldPosition);
						}
					else 
					{
						int movingCard = theCells[oldPosition-10];
						int stationaryCard = theTableau[newPosition].GetTopCard();
						if (IsValidTableauMove(stationaryCard, movingCard))
						{
							MoveCard(oldPosition, newPosition);
							AutoMoveToFoundation();
						}
						else
						{
							InvalidMove(oldPosition);
						}
					}
				}
			}
			
			// attempting to move card to one of the reserve cells
			else if (newPosition < 14)
			{
				if (theCells.Empty(newPosition-10)) 
				{
					if (oldPosition < 14)								// from tableau or another reserve cell
					{
						MoveCard(oldPosition, newPosition);
						AutoMoveToFoundation();
					}
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
				if (oldPosition < 10)								// from another row in tableau
					currentCard = theTableau[oldPosition].GetTopCard();
				else if (oldPosition < 14)							// from a reserve cell
					currentCard = theCells[oldPosition-10];

				if (theFoundation[newPosition-14].Count == 0)	// pile is empty
				{
					if (Card.RankFromCardIndex(currentCard) == CardRank.Ace)
					{
						cardsPlayed++;
						MoveCard(oldPosition, newPosition);
						AutoMoveToFoundation();
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
				else											// the pile has cards
				{
					// verify the card's rank and suit
					bool RankOK = (int)Card.RankFromCardIndex(currentCard) - 1 == 
						(int)Card.RankFromCardIndex(theFoundation[newPosition-14].GetTopCard());
					bool SuitOK = Card.SuitFromCardIndex(currentCard) == 
						Card.SuitFromCardIndex(theFoundation[newPosition-14].GetTopCard());

					// execute the move if it is valid or unselect if it is not
					if (RankOK && SuitOK)
					{
						cardsPlayed++;
						MoveCard(oldPosition, newPosition);
						AutoMoveToFoundation();
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
			CardRank stationaryRank = Card.RankFromCardIndex(stationaryCard);
			CardRank movingRank = Card.RankFromCardIndex(movingCard);
			CardSuit stationarySuit = Card.SuitFromCardIndex(stationaryCard);
			CardSuit movingSuit = Card.SuitFromCardIndex(movingCard);

			// Move is valid if cards are some suit and moving card's rank is on less than other card
			return stationarySuit == movingSuit && ((int)stationaryRank - 1 == (int)movingRank);
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
			if (oldPosition < 10)
				movingCard = theTableau[oldPosition].GetTopCard();
			else
				movingCard = theCells[oldPosition-10];

			// add it to new location
			if (newPosition < 10)
				theTableau[newPosition].Add(movingCard);
			else if (newPosition < 14)
				theCells[newPosition-10] = movingCard;
			else
				theFoundation[newPosition-14].Add(movingCard);

			// remove it from old location and set refreshColumn flags
			RemoveCard(oldPosition);
			refreshColumn[newPosition] = true;
			refreshColumn[oldPosition] = true;
		}

		/// <summary>
		/// Automatically moves cards to the foundation
		/// </summary>
		private void AutoMoveToFoundation()
		{
			if (autoplay)
			{
				bool automovefinished = false;
				int[] ranks = {0,0,0,0};
				int[] pilenumber = {-1,-1,-1,-1};

				while (!automovefinished)
				{
					// See what card ranks are next for the foundations
					automovefinished = true;
					for (int count = 0; count < 4; count++)
					{
						if (theFoundation[count].Count > 0)
						{
							int foundationcard = theFoundation[count].GetTopCard();
							CardRank foundationRank = Card.RankFromCardIndex(foundationcard);
							CardSuit foundationSuit = Card.SuitFromCardIndex(foundationcard);

							ranks[(int)foundationSuit] = (int)foundationRank + 1;
							pilenumber[(int)foundationSuit] = count;
						}
					}

					// Check the tableau and cells for a move
					for (int count = 0; count < 14; count++)
					{
						int currentcard = -1;
						if (count < 10)
						{
							if (theTableau[count].Count > 0)
							{
								currentcard = theTableau[count].GetTopCard();
							}
						}
						else
						{
							if (!theCells.Empty(count - 10))
							{
								currentcard = theCells[count - 10];
							}
						}

						if (currentcard >= 0)
						{
							CardSuit currentSuit = Card.SuitFromCardIndex(currentcard);
							CardRank currentRank = Card.RankFromCardIndex(currentcard);

							if (ranks[(int)currentSuit] == (int)currentRank)
							{
								if (currentRank == CardRank.Ace)
								{
									// find first empty pile
									int firstemptypile = -1;
									for (int index = 0; index < 4 && firstemptypile < 0; index++)
									{
										if (theFoundation[index].Count == 0)
										{
											firstemptypile = index;
										}
									}
								
									// If an empty pile found move the card
									if (firstemptypile >= 0)
									{
										cardsPlayed++;
										MoveCard(count, firstemptypile + 14);
										automovefinished = false;
										finished = false;
									}
								}
								else
								{
									// move card to foundation
									cardsPlayed++;
									MoveCard(count, pilenumber[(int)currentSuit] + 14);
									automovefinished = false;
									finished = false;
								}
							}
						}					
					}
				}
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

		/// <summary>
		/// Gets the preference setting for the current game
		/// </summary>
		private void GetPreferences()
		{
			Preferences.GetPreferences();
			autoplay = Preferences.AutoMove;
		}
		#endregion
	}
}
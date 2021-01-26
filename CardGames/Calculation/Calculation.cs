// Calculation wish list:
//  Fix highlighted card bug (on refresh)
//  add a label to display number of cards played?
//  show stock pile as 5 cards when >= 5 and as total cards when less < 5.
//  determine when game is over (no more moves)
//  implement an undo option (one per hand)

namespace CardGames.Calculation
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
	/// This is a simple implementation of the solitare game Calculation.
	/// </summary>
	public class Calculation : System.Windows.Forms.Form
	{
		#region Constants
		private const int StockX = 20;								// The left most position of the stock pile
		private const int StockY = 65;								// The top most position of the stock pile
		private const int FoundationX = 125;						// The left most position of the Foundation and waste piles
		private const int FoundationY = 10;							// The top most position of the Foundation
		private const int WasteY = 125;								// The top most position of the Waste piles
		private const int WasteHeight = 20;							// The height of the waste pile cards
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private Foundation theFoundation;							// The data structure for the foundation
		private CardCollection[] theWaste;							// The data structure for the wastepiles
		private Deck theDeck;										// The data structure for the deck of cards
		private int currentCard;									// The current position in the deck
		private int selectedCard;									// The currently selected card
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private CardBack currentCardBack;							// The selected card back
		private int[] foundationBaseCards;							// The foundation base cards
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
		private System.Windows.Forms.MenuItem ChangeCardBack;			// The View stats menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public Calculation()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			theWaste = new CardCollection[4];
			theFoundation = new Foundation();
			drawCard = new Card();
			theStats = new Stats(this.Name);
			foundationBaseCards = new int[4];
			currentCardBack = CardBack.Weave1;
			for (int count = 0; count < 4; count++)
				theWaste[count] = new CardCollection();

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
			// Calculation
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(458, 459);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "Calculation";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Calculation";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Calculation_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Calculation_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Calculation_Paint);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Calculation_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			for (int column = 0; column < 9; column++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;

				if (column < 4)
				{
					left = column * CardWidth + FoundationX;
					top = WasteY;
					bottom = top + CardHeight;

					if (theWaste[column].Count > 0)
						bottom += (theWaste[column].Count - 1) * WasteHeight;
				}
				else if (column == 4)
				{
					left = StockX;
					top = StockY;
					bottom = top + CardHeight;
				}
				else
				{
					left = (column-5) * CardWidth + FoundationX;
					top = FoundationY;
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
							bool selectFromWaste = (column < 4 && theWaste[column].Count > 0);
							bool selectFromStock = (column == 4 && currentCard < 52);
							if (selectFromWaste || selectFromStock)
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
		private void Calculation_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Calculation_Closing(object sender, CancelEventArgs e)
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
			for (int count = 0; count < 4; count++)
				if (theFoundation[count].Count == 13)
					drawCard.DrawCardBack(new Point(count * CardWidth + FoundationX, FoundationY), currentCardBack);
			
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
			// Shuffle the deck and reset the currentCard
			theDeck.Shuffle();
			currentCard = -1;
			selectedCard = -1;
			cardsPlayed = 0;
			finished = true;

			// Get the initial cards for the Foundation
			CardRank target = CardRank.Ace;
			while (target < CardRank.Five)
			{
				bool cardfound = false;
				int count = 0;
				while (!cardfound)
				{
					if (Card.RankFromCardIndex(theDeck[count]) == target)
					{
						foundationBaseCards[(int)target] = count;                    
						cardfound = true;
					}
					count++;
				}
				target++;
			}

			// Reset the waste piles and start the foundations
			for (int count = 0; count < 4; count++)
			{
				theWaste[count].Clear();
				theFoundation[count].Clear();
				theFoundation[count].Add(theDeck[foundationBaseCards[count]]);
				cardsPlayed += 4;
			}

			// Get the first card from the reserve pile and redraw the screen
			GetNextCard();

			this.Refresh();
		}

		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			if (oldPosition < 4) 
			{
				if (newPosition > 4)
				{
					CardRank oldCardRank = Card.RankFromCardIndex(theWaste[oldPosition].GetTopCard());				
					int newRankNumber = (int)Card.RankFromCardIndex(theFoundation[newPosition-5].GetTopCard()) +
						newPosition - 4;
					CardRank newCardRank = (CardRank)(newRankNumber % 13);
	
					if (newCardRank == oldCardRank && theFoundation[newPosition-5].Count < 13)
					{
						cardsPlayed++;
						AddCard(oldPosition, newPosition);
						RemoveCard(oldPosition);
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
			else if (oldPosition == 4)
			{
				if (newPosition < 4)
				{
					if (currentCard < 52)
					{
						AddCard(oldPosition, newPosition);
						GetNextCard();
					}
				}
				else if (newPosition > 4)
				{
					CardRank oldCardRank = Card.RankFromCardIndex(theDeck[currentCard]);				
					int newRankNumber = (int)Card.RankFromCardIndex(theFoundation[newPosition-5].GetTopCard()) +
						newPosition - 4;
					CardRank newCardRank = (CardRank)(newRankNumber % 13);
					if (newCardRank == oldCardRank && theFoundation[newPosition-5].Count < 13)
					{
						cardsPlayed++;
						AddCard(oldPosition, newPosition);
						GetNextCard();
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
				if (theFoundation[column].Count < 13)
					drawCard.DrawCard(new Point(CardWidth * column + FoundationX, FoundationY),
						theFoundation[column].GetTopCard());
				else
					drawCard.DrawCardBack(new Point(CardWidth * column + FoundationX, FoundationY), currentCardBack);

				if (theWaste[column].Count > 0)
					for (int count = 0; count < theWaste[column].Count; count++)
						drawCard.DrawCard(new Point(CardWidth * column + FoundationX, WasteHeight * count + WasteY),
							theWaste[column][count]);
				else
					drawCard.DrawCardBack(new Point(CardWidth * column + FoundationX, WasteY), CardBack.O);
			}

			drawCard.DrawCard(new Point(StockX, StockY), theDeck[currentCard]);
			drawCard.End();
		}

		/// <summary>
		/// Gets the next card in the deck;
		/// </summary>
		private void GetNextCard()
		{
			drawCard.Begin(this.CreateGraphics());
			currentCard++;

			bool isBaseCard = true;
			while (isBaseCard)
			{
				isBaseCard = false;
				for (int count = 0; count < 4 && !isBaseCard; count++)
				{
					if (currentCard < 52)
						isBaseCard = (currentCard == foundationBaseCards[count]);
					else
						isBaseCard = false;
				}
				if (isBaseCard) currentCard++;
			}

			if (currentCard < 52)
				drawCard.DrawCard(new Point(StockX, StockY), theDeck[currentCard]);
			else
				drawCard.DrawCardBack(new Point(StockX, StockY), CardBack.X);
			
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
				cardLocation.X = column * CardWidth + FoundationX;
				cardLocation.Y = (theWaste[column].Count - 1) * WasteHeight + WasteY;
				
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theWaste[column].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theWaste[column].GetTopCard());
			}
			else
			{
				cardLocation.X = StockX;
				cardLocation.Y = StockY;
				if (currentCard < 52)
				{
					if (selected)
						drawCard.DrawHighlightedCard(cardLocation, theDeck[currentCard]);
					else
						drawCard.DrawCard(cardLocation, theDeck[currentCard]);
				}
				else
				{
					drawCard.DrawCardBack(cardLocation, CardBack.X);
				}
			}

			drawCard.End();
		}

		/// <summary>
		/// Adds a card to the waste pile
		/// </summary>
		private void AddCard(int oldPosition, int newPosition)
		{
			int card = 0;

			if (oldPosition < 4)
				card = theWaste[oldPosition].GetTopCard();
			else if (oldPosition == 4)
				card = theDeck[currentCard];

			drawCard.Begin(this.CreateGraphics());
			if (newPosition < 4)
			{
				theWaste[newPosition].Add(card);
				drawCard.DrawCard(new Point(newPosition * CardWidth + FoundationX, (theWaste[newPosition].Count - 1) * WasteHeight + WasteY), card);
			}
			else if (newPosition > 4)
			{
				theFoundation[newPosition-5].Add(card);
				if (theFoundation[newPosition-5].Count < 13)
				{
					drawCard.DrawCard(new Point((newPosition - 5) * CardWidth + FoundationX, FoundationY), card);
				}
				else
				{
					drawCard.DrawCardBack(new Point((newPosition - 5) * CardWidth + FoundationX, FoundationY), currentCardBack);
				}
			}

			drawCard.End();
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column"></param>
		private void RemoveCard(int column)
		{
			if (column < 4) 
			{
				int x = column*CardWidth+FoundationX;
				int y = (theWaste[column].Count-1)*WasteHeight+WasteY;
				Rectangle oldCardArea = new Rectangle(x, y, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);

				theWaste[column].RemoveTopCard();
				drawCard.Begin(this.CreateGraphics());

				if (theWaste[column].Count == 0)
					drawCard.DrawCardBack(new Point(column * CardWidth + FoundationX, WasteY), CardBack.O);
				else
					drawCard.DrawCard(new Point(column * CardWidth + FoundationX, 
						(theWaste[column].Count - 1) * WasteHeight + WasteY), theWaste[column].GetTopCard());
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

		#region Debug Methods
		/// <summary>
		/// This method is used only for debugging / testing....
		/// </summary>
		private void _DEBUG_FillCards()
		{
			// TODO: testing code
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Two));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Three));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Four));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Five));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Six));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Seven));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Eight));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Nine));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Ten));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Jack));
			theFoundation[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Queen));

			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Two));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Three));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Four));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Five));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Six));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Seven));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Eight));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Nine));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Jack));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Queen));
			theFoundation[1].Add(Card.ToCardIndex(CardSuit.Clubs, CardRank.Jack));

			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Two));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Three));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Four));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Five));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Six));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Seven));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Eight));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Nine));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Ten));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Jack));
			theFoundation[2].Add(Card.ToCardIndex(CardSuit.Clubs, CardRank.Ten));

			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Two));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Three));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Four));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Five));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Six));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Seven));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Eight));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Nine));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Jack));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.King));
			theFoundation[3].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.Nine));

			theWaste[0].Add(Card.ToCardIndex(CardSuit.Hearts, CardRank.King));
			theWaste[0].Add(Card.ToCardIndex(CardSuit.Clubs, CardRank.King));
			theWaste[0].Add(Card.ToCardIndex(CardSuit.Diamond, CardRank.King));
			theWaste[0].Add(Card.ToCardIndex(CardSuit.Spades, CardRank.King));
		}
		#endregion
	}
}
// AcesUp wish list:
//  add a label showing the number of cards played
//  game over logic - no more moves possible (reset flag and write stats)
//  label showing that moves are possible
//  freecell or waste pile option
//  add an undo option (one per game)
//  add a betting mode using discards
//  - 44 2X
//  - 45 3X
//  - 46 5X
//  - 47 10X
//  - 48 25X

namespace CardGames.AcesUp
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
	/// This is a simple implementation of the solitare game Aces Up.
	/// </summary>
	public class AcesUp : System.Windows.Forms.Form
	{
		#region Constants
		private const int LeftX = 15;								// The left most position of the Tableau
		private const int TopY = 10;								// The top most position
		private const int ReserveX = 350;							// The X position of the reserve
		private const int TableauHeight = 17;						// The height of the cards in the Tableau
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		#endregion

		#region Private Fields
		private Deck theDeck;										// The data structure for the deck of cards
		private Stats theStats;										// The stats for this game
		private bool finished;										// The hand is finished
		private Card drawCard;										// Card object used to draw the cards
		private int currentCard;									// The current card of the reserve
		private CardCollection[] theTableau;						// The tableau rows
		private CardBack selectedCardBack;							// The selected CardBack
		private int cardsPlayed;									// The number of cards played this hand
		#endregion

		#region Controls
		private System.ComponentModel.Container components = null;	// Required designer variable
		private System.Windows.Forms.MainMenu UserMenu;				// The form's menu
		private System.Windows.Forms.MenuItem File;					// The File menu
		private System.Windows.Forms.MenuItem New;					// The New Game item
		private System.Windows.Forms.MenuItem FileSeperator;		// The file seperator
		private System.Windows.Forms.MenuItem Exit;					// The Exit menu item
		private System.Windows.Forms.MenuItem ResetStats;			// The Reset Stats item
		private System.Windows.Forms.MenuItem ViewStats;			// The View Stats item
		private System.Windows.Forms.Label ReserveLeft;				// The cards left in the reserve
		private System.Windows.Forms.MenuItem ChangeCardBack;		// The Change card back item
		private System.Windows.Forms.MenuItem OptionSeperator;		// The Option menu seperator
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public AcesUp()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theTableau = new CardCollection[4];
			theStats = new Stats(this.Name);
			selectedCardBack = CardBack.Weave1;
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
			this.OptionSeperator = new System.Windows.Forms.MenuItem();
			this.ChangeCardBack = new System.Windows.Forms.MenuItem();
			this.ReserveLeft = new System.Windows.Forms.Label();
			this.SuspendLayout();
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
																					this.OptionSeperator,
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
			// OptionSeperator
			// 
			this.OptionSeperator.Index = 2;
			this.OptionSeperator.Text = "-";
			// 
			// ChangeCardBack
			// 
			this.ChangeCardBack.Index = 3;
			this.ChangeCardBack.Text = "&Change Card Back";
			this.ChangeCardBack.Click += new System.EventHandler(this.ChangeCardBack_Click);
			// 
			// ReserveLeft
			// 
			this.ReserveLeft.Location = new System.Drawing.Point(352, 112);
			this.ReserveLeft.Name = "ReserveLeft";
			this.ReserveLeft.Size = new System.Drawing.Size(80, 16);
			this.ReserveLeft.TabIndex = 0;
			// 
			// AcesUp
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(434, 320);
			this.ControlBox = false;
			this.Controls.Add(this.ReserveLeft);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "AcesUp";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Aces Up";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AcesUp_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AcesUp_Closing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.AcesUp_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		#region Form Events
		private void AcesUp_MouseDown(object sender, MouseEventArgs e)
		{
			for (int column = 0; column < 5; column++)
			{
				int left = 0;
				int top = 0;

				if (column < 4)
				{
					left = column * CardWidth + LeftX;
					top = (theTableau[column].Count - 1) * TableauHeight + TopY;
				}
				else
				{
					left = ReserveX;
					top = TopY;
				}

				int right = left + CardWidth;
				int bottom = top + CardHeight;

				if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
				{
					if (e.Button == MouseButtons.Left) 
					{
						if (column < 4)
						{
							if (theTableau[column].Count > 0)
								AttemptMove(column);
						}
						else
						{
							if (currentCard < 52)
								DealCards();
							else if (CardGames.Preferences.SoundEnabled)							
								Sound.MessageBeep((Int32)BeepTypes.Ok);
						}
					}
				}
			}
		}

		private void AcesUp_Paint(object sender, PaintEventArgs e)
		{
			// Determine the number of cards left and begin drawing
			int cardsLeft = 52 - currentCard;
			ReserveLeft.Text = cardsLeft.ToString() + " cards left";
			drawCard.Begin(e.Graphics);

			// Draw the reserve pile or an empty spot
			if (cardsLeft > 0)
				for (int count = 4; count >= 0; count--)
					drawCard.DrawCardBack(new Point(ReserveX + count, TopY + count), selectedCardBack);
			else
			{

				Rectangle oldCardArea = new Rectangle(ReserveX, TopY , + CardWidth + 4, CardHeight + 4);
				e.Graphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				drawCard.DrawCardBack(new Point(ReserveX, TopY), CardBack.X);
			}

			// Draw the tableau
			for (int count = 0; count < 4; count++)
				if (theTableau[count].Count > 0)
					for (int cardsInRow = 0; cardsInRow < theTableau[count].Count; cardsInRow++)
						drawCard.DrawCard(new Point(CardWidth * count + LeftX, cardsInRow * TableauHeight + TopY),
							theTableau[count][cardsInRow]);
				else
					drawCard.DrawCardBack(new Point(CardWidth * count + LeftX, TopY), CardBack.O);

			drawCard.End();
		}

		/// <summary>
		/// Handles the closing of the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AcesUp_Closing(object sender, CancelEventArgs e)
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
		/// Changes the Card Back
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeCardBack_Click(object sender, System.EventArgs e)
		{
			selectedCardBack++;
			if (selectedCardBack == CardBack.Unused)
				selectedCardBack = CardBack.Crosshatch;

			Graphics theGraphics = this.CreateGraphics();
			drawCard.Begin(theGraphics);
			if (currentCard < 52)
				for (int count = 4; count >= 0; count--)
					drawCard.DrawCardBack(new Point(ReserveX + count, TopY + count), selectedCardBack);
			else
			{
				Rectangle oldCardArea = new Rectangle(ReserveX, TopY , + CardWidth + 4, CardHeight + 4);
				theGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				drawCard.DrawCardBack(new Point(ReserveX, TopY), CardBack.X);
			}
			drawCard.End();
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
			// Reset the finished flag
			currentCard = 0;
			cardsPlayed = 0;
			finished = true;
			
			// Shuffle the deck and initialize the data structures
			theDeck.Shuffle();
			for (int count = 0; count < 4; count++)
			{
				theTableau[count] = new CardCollection();
			}
			this.Refresh();
		}

		/// <summary>
		/// Deals a row of cards to the tableau
		/// </summary>
		private void DealCards()
		{
			Graphics theGraphics = this.CreateGraphics();
			drawCard.Begin(theGraphics);
			for (int count = currentCard; count < currentCard + 4; count++)
				theTableau[count % 4].Add(theDeck[count]);
			currentCard += 4;

			if (currentCard == 52)
			{
				Rectangle oldCardArea = new Rectangle(ReserveX, TopY , + CardWidth + 4, CardHeight + 4);
				theGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				drawCard.DrawCardBack(new Point(ReserveX, TopY), CardBack.X);
			}

			// Determine the number of cards left and begin drawing
			int cardsLeft = 52 - currentCard;
			ReserveLeft.Text = cardsLeft.ToString() + " cards left";
			
			// Draw the tableau
			for (int count = 0; count < 4; count++)
				drawCard.DrawCard(new Point(CardWidth * count + LeftX, (theTableau[count].Count - 1)
					* TableauHeight + TopY), theTableau[count].GetTopCard());

			drawCard.End();
		}

		/// <summary>
		/// Attempts to move a card
		/// </summary>
		private void AttemptMove(int column)
		{
			finished = false;
			bool lowerCardFound = false;

			for (int count = 0; count < 4 && !lowerCardFound; count++)
			{
				if (count == column)
				{
					if (theTableau[column].Count > 1)
					{
						CardRank removeCardRank = Card.RankFromCardIndex(theTableau[column].GetTopCard());
						CardRank baseCardRank = Card.RankFromCardIndex(theTableau[column][theTableau[column].Count - 2]);
						CardSuit removeCardSuit = Card.SuitFromCardIndex(theTableau[column].GetTopCard());
						CardSuit baseCardSuit = Card.SuitFromCardIndex(theTableau[column][theTableau[column].Count - 2]);
						if (baseCardRank == CardRank.Ace) 
							lowerCardFound = (removeCardSuit == baseCardSuit);
						else if (removeCardRank == CardRank.Ace)
							lowerCardFound = false;
						else
							lowerCardFound = (removeCardRank < baseCardRank && removeCardSuit == baseCardSuit);
					}
					else 
						lowerCardFound = false;
				}
				else
				{
					if (theTableau[count].Count > 0) 
					{
						CardRank removeCardRank = Card.RankFromCardIndex(theTableau[column].GetTopCard());
						CardRank baseCardRank = Card.RankFromCardIndex(theTableau[count].GetTopCard());
						CardSuit removeCardSuit = Card.SuitFromCardIndex(theTableau[column].GetTopCard());
						CardSuit baseCardSuit = Card.SuitFromCardIndex(theTableau[count].GetTopCard());
						if (baseCardRank == CardRank.Ace) 
							lowerCardFound = (removeCardSuit == baseCardSuit);
						else if (removeCardRank == CardRank.Ace)
							lowerCardFound = false;
						else
							lowerCardFound = (removeCardRank < baseCardRank && removeCardSuit == baseCardSuit);
					}
					else
						lowerCardFound = false;
				}
			}

			if (lowerCardFound)
			{
				cardsPlayed++;
				RemoveCard(column);
			}
			else
			{
				bool moveComplete = false;
				for (int count = 0; count < 4 && !moveComplete; count++)
				{
					if (theTableau[count].Count == 0)
					{
						moveComplete = true;
						theTableau[count].Add(theTableau[column].GetTopCard());
						RemoveCard(column);

						// Get the graphics object from the form
						Graphics theGraphics = this.CreateGraphics();
						drawCard.Begin(theGraphics);

						// Draw the card
						drawCard.DrawCard(new Point(CardWidth * count + LeftX, TopY),
							theTableau[count].GetTopCard());	
						drawCard.End();
					}
				}

				if (!moveComplete && CardGames.Preferences.SoundEnabled) Sound.MessageBeep((Int32)BeepTypes.Ok);
			}

			CheckGameOver();
		}

		private void RemoveCard(int column)
		{
			// Get the graphics object from the form
			Graphics theGraphics = this.CreateGraphics();
			drawCard.Begin(theGraphics);

			// Erase the card that is removed
			int x = CardWidth * column + LeftX;
			int y = TableauHeight * (theTableau[column].Count - 1) + TopY;
			Rectangle oldCardArea = new Rectangle(x, y , CardWidth, CardHeight);
			theGraphics.FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				
			// Remove the card and draw the card above it
			theTableau[column].RemoveTopCard();
			if (theTableau[column].Count > 0)
				drawCard.DrawCard(new Point(x, y - TableauHeight), theTableau[column].GetTopCard());	
			else
				drawCard.DrawCardBack(new Point(x, TopY), CardBack.O);
			drawCard.End();
		}

		/// <summary>
		/// Checks to see if the game is won or not.
		/// </summary>
		private void CheckGameOver()
		{
			bool gameover = true;
			if (currentCard == 52)
			{
				for (int count = 0; count < 4 && gameover; count++)
					if (theTableau[count].Count > 1)
						gameover = false;
			}
			else         
				gameover = false;

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
		#endregion
	}
}
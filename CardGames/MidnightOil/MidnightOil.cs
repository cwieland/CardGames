// Midnight Oil tasks
//  Fix highlighted card bug (on refresh)
//  Implement code to force user to play drawn card or place it back if invalid move

namespace CardGames.MidnightOil
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
	/// This is a simple implementation of the solitare game Midnight Oil
	/// </summary>
	public class MidnightOil : System.Windows.Forms.Form
	{
		#region Constants
		private const int FoundationX = 25;							// The left most position of the Foundation and waste piles
		private const int FoundationY = 15;							// The top most position of the Foundation
		private const int FoundationWidth = 114;					// The width of the cards in Foundation
		private const int TableauX = 25;							// The left most position of the tableau
		private const int TableauY = 130;							// The top most position of the tableau
		private const int TableauWidth = 114;						// The width of the tableau piles
		private const int TableauHeight = 107;						// The height of the tableau pile cards
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		private const int OriginalCardWidth = 12;					// The width of a consealed card
		private const int PickedX = 615;							// The top most position of the picked card
		private const int PickedY = 15;								// The top most position of the picked card
		#endregion

		#region Private Fields
		private Deck theDeck;										// The data structure for the deck of cards
		private CardCollection[] theTableau;						// The data structure representing the tableau
		private int[] originalCount;								// The number of original cards in the pile
		private Foundation theFoundation;							// The data structure representing the foundation
		private CardBack currentCardBack;							// The selected card back
		private Card drawCard;										// Card object used to draw the cards
		private Stats theStats;										// The game statistics
		private int drawsLeft;										// Number of draws left
		private int shufflesLeft;									// Number of shuffles left
		private bool finished;										// Whether the current hand is finished or not
		private int cardsPlayed;									// The number of cards played to the foundation
		private int selectedCard;									// The currently selected card
		private int pickedCard;										// The card picked from a pile
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
		private System.Windows.Forms.Button Shuffle;
		private System.Windows.Forms.Button Pick;			// The View stats menu item
		private System.Windows.Forms.MenuItem Options;				// The Options menu
		#endregion
	
		#region Constructors
		public MidnightOil()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theDeck = new Deck();
			drawCard = new Card();
			theTableau = new CardCollection[18];
			originalCount = new int[18];
			theFoundation = new Foundation();	
			theStats = new Stats(this.Name);
			currentCardBack = CardBack.Weave1;
			
			for (int count = 0; count < 18; count++)
			{
				theTableau[count] = new CardCollection();
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
			this.Shuffle = new System.Windows.Forms.Button();
			this.Pick = new System.Windows.Forms.Button();
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
			// Shuffle
			// 
			this.Shuffle.BackColor = System.Drawing.Color.Yellow;
			this.Shuffle.Location = new System.Drawing.Point(485, 34);
			this.Shuffle.Name = "Shuffle";
			this.Shuffle.Size = new System.Drawing.Size(96, 23);
			this.Shuffle.TabIndex = 0;
			this.Shuffle.Text = "Shuffle Cards";
			this.Shuffle.Click += new System.EventHandler(this.Shuffle_Click);
			// 
			// Pick
			// 
			this.Pick.BackColor = System.Drawing.Color.Red;
			this.Pick.Location = new System.Drawing.Point(485, 69);
			this.Pick.Name = "Draw";
			this.Pick.Size = new System.Drawing.Size(96, 23);
			this.Pick.TabIndex = 1;
			this.Pick.Text = "Draw Card";
			this.Pick.Click += new System.EventHandler(this.Pick_Click);
			// 
			// MidnightOil
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(714, 459);
			this.ControlBox = false;
			this.Controls.Add(this.Pick);
			this.Controls.Add(this.Shuffle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.UserMenu;
			this.Name = "MidnightOil";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Midnight Oil";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MidnightOil_MouseDown);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MidnightOil_Paint);
			this.Closing += new CancelEventHandler(MidnightOil_Closing);
			this.ResumeLayout(false);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MidnightOil_MouseDown(object sender, MouseEventArgs e)
		{
			// Get the previous selection
			int oldSelection = selectedCard;

			for (int index = 0; index < 23; index++)
			{
				int left = 0;
				int top = 0;
				int bottom = 0;
				int right = 0;

				if (index < 18)
				{
					// Check to see if the tableau pile is clicked
					Point topleft = this.GetPointForCard(index);
					left = topleft.X;
					top = topleft.Y;
				}
				else if (index < 22)
				{
					left = FoundationX + FoundationWidth * (index - 18);
					top = FoundationY;
				}
				else
				{
					left = PickedX;
					top = PickedY;
				}
			
				right = left + CardWidth;
				bottom = top + CardHeight;

				if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
				{
					if (oldSelection == index)
					{
						if (e.Button == MouseButtons.Left)
						{
							if ((index < 18 && theTableau[index].Count > 0) ||
								(index == 22 && pickedCard > -1))
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
							if ((index < 18 && theTableau[index].Count > 0) ||
								(index == 22 && pickedCard > -1))
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
						if (index < 22)
						{
							if (e.Button == MouseButtons.Left)
								AttemptMove(oldSelection, index);
						}
						else
						{
							selectedCard = -1;
							InvalidMove(oldSelection);
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MidnightOil_Paint(object sender, PaintEventArgs e)
		{
			DrawCards();
		}

		/// <summary>
		/// Handles the closing of the form	
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MidnightOil_Closing(object sender, CancelEventArgs e)
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
		/// 
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

		/// <summary>
		/// This shuffles and redeals the tableau
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Shuffle_Click(object sender, System.EventArgs e)
		{
			finished = false;
			selectedCard = -1;

			if (shufflesLeft > 0)
			{
				// Put all of the cards in to one pile and clear the tableau
				CardCollection theShuffle = new CardCollection();
				for (int pile = 0; pile < 18; pile++)
				{
					for (int count = 0; count < theTableau[pile].Count; count++)
					{
						theShuffle.Add(theTableau[pile][count]);
					}
					
					theTableau[pile].Clear();
					originalCount[pile] = 0;
				}

				// Randomize the indexes of the new pile
				int[] newIndex = new int[theShuffle.Count];
				bool[] used = new bool[theShuffle.Count];

				for( int j = 0; j < theShuffle.Count; j++ )
				{
					used[j] = false;
				}
    
				Random rnd = new Random();
				int iCount = 0;
				int iNum;

				while( iCount < theShuffle.Count )
				{
					iNum = rnd.Next( 0, theShuffle.Count ); // between 0 and the number of cards

					if( used[iNum] == false )
					{
						newIndex[iCount] = iNum;
						used[iNum] = true;
						iCount++;
					}
				}

				// Deal the cards to the tableau using the new random indexes
				for (int cardCount = 0; cardCount < theShuffle.Count; cardCount++)
				{
					int pile = cardCount / 3;
					theTableau[pile].Add(theShuffle[newIndex[cardCount]]);
					originalCount[pile]++;
				}

				// Draw the cards and remove one shuffle
				Rectangle oldCardArea = new Rectangle(TableauX, TableauY, TableauWidth * 6, TableauHeight * 3);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				DrawCards();
				shufflesLeft--;
			}

			// Hide the button if we are out of shuffles
			Shuffle.Visible = (shufflesLeft > 0);
		}

		/// <summary>
		/// This allows the play to draw a card from a pile
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Pick_Click(object sender, System.EventArgs e)
		{
			finished = false;

			if (drawsLeft > 0)
			{
				PickCard dialog = new PickCard(theTableau);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					// Get the selected card
					int pile = dialog.SelectedPile;
					int index = dialog.SelectedIndex;
					pickedCard = theTableau[pile][index];
					
					// Erase the cards on the table
					int row = pile / 6;
					int column = pile % 6;
					int left = TableauX + column * TableauWidth;
					int top = TableauY + row * TableauHeight;					
					Point topleft = GetPointForCard(pile);
					int width = (topleft.X + CardWidth) - left;
					int height = (topleft.Y + CardHeight) - top;
					Rectangle oldCardArea = new Rectangle(left, top, width, height);
					this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);
				
					// Remove the card from the pile
					theTableau[pile].RemoveAt(index);
					if (index < originalCount[pile])
					{
						originalCount[pile]--;
					}

					// Redraw the pile
					drawCard.Begin(this.CreateGraphics());
					DrawPile(pile, drawCard);
					drawCard.End();

                    // Decrement the number of draws left and dispose of the dialog resources
					drawsLeft--;
					dialog.Dispose();
				}
			}

			Pick.Visible = (drawsLeft > 0);
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
			selectedCard = -1;
			pickedCard = -1;
			cardsPlayed = 0;
			finished = true;
			Pick.Visible = true;
			Shuffle.Visible = true;

			// Clear the foundation
			theDeck.Shuffle();
			for (int count = 0; count < 4; count++)
			{
				theFoundation[count].Clear();
			}

			// Reset all of the values;
			drawsLeft = 1;
			shufflesLeft = 2;
			for (int index = 0; index < 18; index++)
			{
				originalCount[index] = 0;
				theTableau[index].Clear();
			}

			// Deal out the first hand
			for (int count = 0; count < 52; count++)
			{
				int index = count / 3;
				theTableau[index].Add(theDeck[count]);
				originalCount[index]++;
			}

			DrawCards();
		}
		
		/// <summary>
		/// Gets the topleft corner for the card
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private Point GetPointForCard(int index)
		{
			int x = 0;
			int y = 0;

			if (index < 18)
			{
				int column = index % 6;
				int row = index / 6;
				x = TableauX + TableauWidth * column;
				y = TableauHeight * row + TableauY;

				if (originalCount[index] > 1)
				{
					x += (originalCount[index] - 1) * OriginalCardWidth;
				}

				if (theTableau[index].Count > originalCount[index] && originalCount[index] > 0)
				{
					x += theTableau[index].Count - originalCount[index];
					y += theTableau[index].Count - originalCount[index];
				}
			}
			else if (index == 22)
			{
				x = PickedX;
				y = PickedY;
			}

			return new Point(x,y);
		}

		/// <summary>
		/// Draws the tableau and the foundation
		/// </summary>
		private void DrawCards()
		{
			drawCard.Begin(this.CreateGraphics());
			
			// Draw the foundation
			for (int column = 0; column < 4; column++)
			{				
				if (theFoundation[column].Count > 0)
				{
					if (Card.RankFromCardIndex(theFoundation[column].GetTopCard()) == CardRank.King)
					{
						drawCard.DrawCardBack(new Point(FoundationWidth * column + FoundationX, FoundationY),
							currentCardBack);
					}
					else
					{
						drawCard.DrawCard(new Point(FoundationWidth * column + FoundationX, FoundationY),
							theFoundation[column].GetTopCard());
					}
				}
				else
					drawCard.DrawCardBack(new Point(FoundationWidth * column + FoundationX, FoundationY), CardBack.O);
			}

			// Draw the tableau
			for (int index = 0; index < 18; index++)
			{
				DrawPile(index, drawCard);
			}

			// Draw the card picked out
			if (pickedCard > -1)
			{
				if (selectedCard == 22)
				{
					drawCard.DrawHighlightedCard(new Point(PickedX, PickedY), pickedCard);
				}
				else
				{
					drawCard.DrawCard(new Point(PickedX, PickedY), pickedCard);
				}
			}
			else
			{
				Rectangle oldCardArea = new Rectangle(PickedX, PickedY, CardWidth, CardHeight);
				this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea); 
			}

			drawCard.End();
		}

		/// <summary>
		/// Draw the specified pile
		/// </summary>
		/// <param name="index">Number of the pile</param>
		/// <param name="drawCard">Card object used to draw</param>
		private void DrawPile(int index, Card drawCard)
		{
			int column = index % 6;
			int row = index / 6;

			if (theTableau[index].Count > 0)
			{
				for (int count = 0; count < theTableau[index].Count; count++)
				{
					int x = TableauX + TableauWidth * column;
					int y = TableauHeight * row + TableauY;

					if (count < originalCount[index])
					{
						x += count * OriginalCardWidth;
					}
					else
					{
						if (originalCount[index] > 0)
						{
							x += count - originalCount[index] + 1;
							y += count - originalCount[index] + 1;
						}

						if (originalCount[index] > 1)
						{
							x += (originalCount[index] - 1) * OriginalCardWidth;									
						}
					}

					drawCard.DrawCard(new Point(x, y), theTableau[index][count]);
				}
			}
			else
			{
				drawCard.DrawCardBack(new Point(TableauWidth * column + TableauX,
					TableauHeight * row + TableauY), CardBack.Unused);
			}
		}

		/// <summary>
		/// Toggles whether or not a card is selected
		/// </summary>
		/// <param name="index">Position of the card</param>
		/// <param name="selected">Whether or not card is highlighted</param>
		private void ToggleCardSelect(int index, bool selected)
		{
			drawCard.Begin(this.CreateGraphics());
			Point cardLocation = GetPointForCard(index);

			if (index < 18)
			{
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, theTableau[index].GetTopCard());
				else
					drawCard.DrawCard(cardLocation, theTableau[index].GetTopCard());
			}
			else if (index == 22)
			{
				if (selected)
					drawCard.DrawHighlightedCard(cardLocation, pickedCard);
				else
					drawCard.DrawCard(cardLocation, pickedCard);
			}
			
			drawCard.End();
		}

		/// <summary>
		/// Attempts to make the move specified
		/// </summary>
		/// <param name="oldPosition">Position of first card</param>
		/// <param name="newPosition">Position of second card</param>
		private void AttemptMove(int oldPosition, int newPosition)
		{
			finished = false;

			if (newPosition < 18)
			{
				if (theTableau[newPosition].Count > 0)
				{
					// Moving card around in the tableau
					CardRank oldRank;
					CardSuit oldSuit;
					if (oldPosition < 18)
					{
						oldRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
						oldSuit = Card.SuitFromCardIndex(theTableau[oldPosition].GetTopCard());
					}
					else
					{
						oldRank = Card.RankFromCardIndex(pickedCard);
						oldSuit = Card.SuitFromCardIndex(pickedCard);
					}
					CardRank newRank = Card.RankFromCardIndex(theTableau[newPosition].GetTopCard());
					CardSuit newSuit = Card.SuitFromCardIndex(theTableau[newPosition].GetTopCard());
					if (newSuit == oldSuit && oldRank + 1 == newRank)
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
					InvalidMove(oldPosition);
				}
			}
			else
			{
				// Moving card to the foundation
				if (theFoundation[newPosition - 18].Count > 0)
				{
					CardRank oldRank;
					CardSuit oldSuit;

					if (oldPosition < 18)
					{
						oldRank = Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
						oldSuit = Card.SuitFromCardIndex(theTableau[oldPosition].GetTopCard());
					}
					else
					{
						oldRank = Card.RankFromCardIndex(pickedCard);
						oldSuit = Card.SuitFromCardIndex(pickedCard);
					}
					CardRank newRank = Card.RankFromCardIndex(theFoundation[newPosition - 18].GetTopCard());
					CardSuit newSuit = Card.SuitFromCardIndex(theFoundation[newPosition - 18].GetTopCard());
					if (newSuit == oldSuit && oldRank == newRank + 1)
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
					// Moving card to empty pile
					CardRank newRank ;
					if (oldPosition < 18)
					{
						newRank	= Card.RankFromCardIndex(theTableau[oldPosition].GetTopCard());
					}
					else
					{
						newRank = Card.RankFromCardIndex(pickedCard);
					}

					if (newRank == CardRank.Ace)
					{
						MoveCard(oldPosition, newPosition);
					}
					else
					{
						InvalidMove(oldPosition);
					}
				}
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
			int movingCard = 0;
			
			if (oldPosition < 18)
			{
				movingCard = theTableau[oldPosition].GetTopCard();
			}
			else
			{
				movingCard = pickedCard;
			}
			
			if (newPosition < 18)
			{
				theTableau[newPosition].Add(movingCard);
				drawCard.Begin(this.CreateGraphics());
				drawCard.DrawCard(GetPointForCard(newPosition), theTableau[newPosition].GetTopCard());
				drawCard.End();
			}
			else if (newPosition < 22)
			{
				theFoundation[newPosition - 18].Add(movingCard);
				drawCard.Begin(this.CreateGraphics());
				if (theFoundation[newPosition - 18].Count < 13)
				{
					drawCard.DrawCard(new Point(FoundationX + (newPosition - 18) * FoundationWidth, FoundationY),
						theFoundation[newPosition - 18].GetTopCard());
				}
				else
				{
					drawCard.DrawCardBack(new Point(FoundationX + (newPosition - 18) * FoundationWidth, FoundationY),
						currentCardBack);
				}
				drawCard.End();
				cardsPlayed++;
			}

			// remove it from old location
			RemoveCard(oldPosition);
		}

		/// <summary>
		/// Removes a card.  If the card is in the tableau it erases it from the screen as well.
		/// </summary>
		/// <param name="column">Column of the card to remove</param>
		private void RemoveCard(int index)
		{
			Point topleft = GetPointForCard(index);
			Size cardsize = new Size(CardWidth, CardHeight);
			Rectangle oldCardArea = new Rectangle(topleft, cardsize);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);

			if (index < 18)
			{
				theTableau[index].RemoveTopCard();
				if (originalCount[index] > theTableau[index].Count)
					originalCount[index]--;
			}
			else
			{
				pickedCard = -1;
			}

			drawCard.Begin(this.CreateGraphics());
			topleft = GetPointForCard(index);

			if (index < 18)
			{
				if (theTableau[index].Count == 0)
				{
					drawCard.DrawCardBack(topleft, CardBack.Unused);
				}
				else
				{
					drawCard.DrawCard(topleft ,theTableau[index].GetTopCard());
				}
			}
			
			drawCard.End();
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
		/// Code to handle an invalid select
		/// </summary>
		private void InvalidSelect()
		{
			if (CardGames.Preferences.SoundEnabled)
				Sound.MessageBeep((Int32)BeepTypes.Ok);
		}
		#endregion
	}
}
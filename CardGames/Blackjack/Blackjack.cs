// Blackjack wish list:
//  trap Alt+F4 keystroke (OnClose event??)
//  Implement a custom stats class to for win / loss record and win percentage
//  Implement more options:
//  - Money / Betting
//  - Selectable rules (Casino modes)
//  - Splitting / Double down
//  add a question mark to the new hand dialog
//  create a custom stats class for this game and capture stats to registry

namespace CardGames.Blackjack
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Data;
	#endregion

	/// <summary>
	/// This is simple implementation of Blackjack
	/// The dealer (computer) will hit if he is over 16 and will continue to until he is over 16.
	/// </summary>
	public class Blackjack : System.Windows.Forms.Form
	{		
		#region Constants
		private const int DealerY = 75;
		private const int PlayerY = 235;
		private const int HandX = 15;
		private const int HitX = 120;
		private const int HitWidth = 80;
		private const int StockPileX = 530;
		private const int StockPileY = 145;
		private const int CardWidth = 79;
		private const int CardHeight = 97;
		#endregion

		#region Controls
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label DealersHandLabel;
		private System.Windows.Forms.Label PlayerHandLabel;
		private System.Windows.Forms.Label PlayerWinsMessage;
		private System.Windows.Forms.Label DealerWinsMessage;
		private System.Windows.Forms.Label DrawMessage;
		private System.Windows.Forms.Label DealerWinsDisplay;
		private System.Windows.Forms.Label PlayerWinsDisplay;
		private System.Windows.Forms.Label DealerWinsLabel;
		private System.Windows.Forms.Label PlayerWinsLabel;
		private System.Windows.Forms.Label PlayerScore;
		private System.Windows.Forms.Label DealerScore;
		private System.Windows.Forms.MainMenu MainMenu1;
		private System.Windows.Forms.MenuItem File;
		private System.Windows.Forms.MenuItem StartNewGame;
		private System.Windows.Forms.MenuItem FileSeperator;
		private System.Windows.Forms.MenuItem Options;
		private System.Windows.Forms.MenuItem ChangeCardBack;
		private System.Windows.Forms.Button Hit;
		private System.Windows.Forms.Button Stand;
		private System.Windows.Forms.Button StartNewHand;
		#endregion
	
		#region Private Fields
		private Deck theDeck;
		private CardCollection dealerHand;
		private CardCollection playerHand;
		private Card cardDrawing;
		private CardBack selectedCardBack;
		private bool handFinished;
		private int currentCard;
		private System.Windows.Forms.MenuItem Exit;
		private int dealerWins;
		private int playerWins;
		#endregion

		#region Constructors
		public Blackjack()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			theDeck = new Deck();
			cardDrawing = new Card();
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
			this.MainMenu1 = new System.Windows.Forms.MainMenu();
			this.File = new System.Windows.Forms.MenuItem();
			this.StartNewGame = new System.Windows.Forms.MenuItem();
			this.FileSeperator = new System.Windows.Forms.MenuItem();
			this.Exit = new System.Windows.Forms.MenuItem();
			this.Options = new System.Windows.Forms.MenuItem();
			this.ChangeCardBack = new System.Windows.Forms.MenuItem();
			this.DealersHandLabel = new System.Windows.Forms.Label();
			this.PlayerHandLabel = new System.Windows.Forms.Label();
			this.Stand = new System.Windows.Forms.Button();
			this.PlayerScore = new System.Windows.Forms.Label();
			this.DealerScore = new System.Windows.Forms.Label();
			this.PlayerWinsMessage = new System.Windows.Forms.Label();
			this.DealerWinsMessage = new System.Windows.Forms.Label();
			this.Hit = new System.Windows.Forms.Button();
			this.StartNewHand = new System.Windows.Forms.Button();
			this.DealerWinsDisplay = new System.Windows.Forms.Label();
			this.PlayerWinsDisplay = new System.Windows.Forms.Label();
			this.DealerWinsLabel = new System.Windows.Forms.Label();
			this.PlayerWinsLabel = new System.Windows.Forms.Label();
			this.DrawMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// MainMenu1
			// 
			this.MainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.File,
																					  this.Options});
			// 
			// File
			// 
			this.File.Index = 0;
			this.File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.StartNewGame,
																				 this.FileSeperator,
																				 this.Exit});
			this.File.Text = "&File";
			// 
			// StartNewGame
			// 
			this.StartNewGame.Index = 0;
			this.StartNewGame.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.StartNewGame.Text = "&New Game";
			this.StartNewGame.Click += new System.EventHandler(this.StartNewGame_Click);
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
																					this.ChangeCardBack});
			this.Options.Text = "&Options";
			// 
			// ChangeCardBack
			// 
			this.ChangeCardBack.Index = 0;
			this.ChangeCardBack.Text = "Change Card &Back";
			this.ChangeCardBack.Click += new System.EventHandler(this.ChangeCardBack_Click);
			// 
			// DealersHandLabel
			// 
			this.DealersHandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DealersHandLabel.Location = new System.Drawing.Point(8, 48);
			this.DealersHandLabel.Name = "DealersHandLabel";
			this.DealersHandLabel.Size = new System.Drawing.Size(120, 23);
			this.DealersHandLabel.TabIndex = 0;
			this.DealersHandLabel.Text = "Dealer\'s Hand";
			// 
			// PlayerHandLabel
			// 
			this.PlayerHandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PlayerHandLabel.Location = new System.Drawing.Point(8, 208);
			this.PlayerHandLabel.Name = "PlayerHandLabel";
			this.PlayerHandLabel.Size = new System.Drawing.Size(120, 23);
			this.PlayerHandLabel.TabIndex = 1;
			this.PlayerHandLabel.Text = "Player\'s Hand";
			// 
			// Stand
			// 
			this.Stand.BackColor = System.Drawing.Color.Red;
			this.Stand.Location = new System.Drawing.Point(144, 8);
			this.Stand.Name = "Stand";
			this.Stand.Size = new System.Drawing.Size(48, 23);
			this.Stand.TabIndex = 3;
			this.Stand.Text = "&Stand";
			this.Stand.Click += new System.EventHandler(this.Stand_Click);
			// 
			// PlayerScore
			// 
			this.PlayerScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PlayerScore.Location = new System.Drawing.Point(128, 208);
			this.PlayerScore.Name = "PlayerScore";
			this.PlayerScore.Size = new System.Drawing.Size(80, 23);
			this.PlayerScore.TabIndex = 4;
			// 
			// DealerScore
			// 
			this.DealerScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DealerScore.Location = new System.Drawing.Point(128, 48);
			this.DealerScore.Name = "DealerScore";
			this.DealerScore.Size = new System.Drawing.Size(80, 23);
			this.DealerScore.TabIndex = 5;
			// 
			// PlayerWinsMessage
			// 
			this.PlayerWinsMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PlayerWinsMessage.Location = new System.Drawing.Point(8, 360);
			this.PlayerWinsMessage.Name = "PlayerWinsMessage";
			this.PlayerWinsMessage.TabIndex = 8;
			this.PlayerWinsMessage.Text = "Player Wins";
			// 
			// DealerWinsMessage
			// 
			this.DealerWinsMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DealerWinsMessage.Location = new System.Drawing.Point(8, 360);
			this.DealerWinsMessage.Name = "DealerWinsMessage";
			this.DealerWinsMessage.Size = new System.Drawing.Size(104, 23);
			this.DealerWinsMessage.TabIndex = 9;
			this.DealerWinsMessage.Text = "Dealer Wins";
			// 
			// Hit
			// 
			this.Hit.BackColor = System.Drawing.Color.Yellow;
			this.Hit.Location = new System.Drawing.Point(88, 8);
			this.Hit.Name = "Hit";
			this.Hit.Size = new System.Drawing.Size(48, 23);
			this.Hit.TabIndex = 10;
			this.Hit.Text = "&Hit";
			this.Hit.Click += new System.EventHandler(this.Hit_Click);
			// 
			// StartNewHand
			// 
			this.StartNewHand.BackColor = System.Drawing.Color.DodgerBlue;
			this.StartNewHand.Location = new System.Drawing.Point(8, 8);
			this.StartNewHand.Name = "StartNewHand";
			this.StartNewHand.TabIndex = 11;
			this.StartNewHand.Text = "New &Hand";
			this.StartNewHand.Click += new System.EventHandler(this.StartNewHand_Click);
			// 
			// DealerWinsDisplay
			// 
			this.DealerWinsDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DealerWinsDisplay.Location = new System.Drawing.Point(584, 16);
			this.DealerWinsDisplay.Name = "DealerWinsDisplay";
			this.DealerWinsDisplay.Size = new System.Drawing.Size(32, 23);
			this.DealerWinsDisplay.TabIndex = 12;
			// 
			// PlayerWinsDisplay
			// 
			this.PlayerWinsDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PlayerWinsDisplay.Location = new System.Drawing.Point(584, 40);
			this.PlayerWinsDisplay.Name = "PlayerWinsDisplay";
			this.PlayerWinsDisplay.Size = new System.Drawing.Size(32, 23);
			this.PlayerWinsDisplay.TabIndex = 13;
			// 
			// DealerWinsLabel
			// 
			this.DealerWinsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DealerWinsLabel.Location = new System.Drawing.Point(520, 16);
			this.DealerWinsLabel.Name = "DealerWinsLabel";
			this.DealerWinsLabel.Size = new System.Drawing.Size(64, 24);
			this.DealerWinsLabel.TabIndex = 14;
			this.DealerWinsLabel.Text = "Dealer";
			// 
			// PlayerWinsLabel
			// 
			this.PlayerWinsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PlayerWinsLabel.Location = new System.Drawing.Point(520, 40);
			this.PlayerWinsLabel.Name = "PlayerWinsLabel";
			this.PlayerWinsLabel.Size = new System.Drawing.Size(64, 23);
			this.PlayerWinsLabel.TabIndex = 15;
			this.PlayerWinsLabel.Text = "Player";
			// 
			// DrawMessage
			// 
			this.DrawMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.DrawMessage.Location = new System.Drawing.Point(8, 360);
			this.DrawMessage.Name = "DrawMessage";
			this.DrawMessage.TabIndex = 16;
			this.DrawMessage.Text = "Draw";
			// 
			// Blackjack
			// 
			this.AcceptButton = this.StartNewHand;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(618, 362);
			this.ControlBox = false;
			this.Controls.Add(this.DrawMessage);
			this.Controls.Add(this.PlayerWinsLabel);
			this.Controls.Add(this.DealerWinsLabel);
			this.Controls.Add(this.PlayerWinsDisplay);
			this.Controls.Add(this.DealerWinsDisplay);
			this.Controls.Add(this.StartNewHand);
			this.Controls.Add(this.Hit);
			this.Controls.Add(this.DealerWinsMessage);
			this.Controls.Add(this.PlayerWinsMessage);
			this.Controls.Add(this.DealerScore);
			this.Controls.Add(this.PlayerScore);
			this.Controls.Add(this.Stand);
			this.Controls.Add(this.PlayerHandLabel);
			this.Controls.Add(this.DealersHandLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.MainMenu1;
			this.Name = "Blackjack";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Blackjack";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Blackjack_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion

		#region Form Events
		private void Blackjack_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.X > StockPileX && e.X < StockPileX + CardWidth) && (e.Y > StockPileY && e.Y < StockPileY + CardHeight))
			{
				if (handFinished)
					NewHand();
				else
					DoPlayerHit();
			}			
		}
		#endregion

		#region Control Events
		private void Exit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void StartNewGame_Click(object sender, System.EventArgs e)
		{
			DialogResult choice = MessageBox.Show("Start a New Game?", "Confirm New Game", MessageBoxButtons.YesNo);
			if (choice == DialogResult.Yes)
			{
				dealerWins = 0;
				playerWins = 0;
				NewHand();
			}
		}

		private void ChangeCardBack_Click(object sender, System.EventArgs e)
		{
			selectedCardBack++;
			if (selectedCardBack == CardBack.Unused)
				selectedCardBack = CardBack.Crosshatch;
			this.Refresh();
		}

		private void StartNewHand_Click(object sender, System.EventArgs e)
		{
			NewHand();
		}

		private void Hit_Click(object sender, System.EventArgs e)
		{
			DoPlayerHit();
		}

		private void Stand_Click(object sender, System.EventArgs e)
		{
			PlayerFinished();
			while (CalculateScore(dealerHand) < 17)
			{
				TakeHit(ref dealerHand);
			}

			if (CalculateScore(dealerHand) > 21)
			{
				this.Refresh();
				playerWins += 1;
				PlayerWinsMessage.Visible = true;
			}
			else if (CalculateScore(playerHand) > CalculateScore(dealerHand))
			{
				playerWins += 1;
				PlayerWinsMessage.Visible = true;
			}
			else if (CalculateScore(playerHand) < CalculateScore(dealerHand))
			{
				dealerWins += 1;
				DealerWinsMessage.Visible = true;
			}
			else
			{
				DrawMessage.Visible = true;
			}
			
			this.Refresh();
		}
		#endregion

		#region Overriden Methods
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// Initialize the drawing routines
			cardDrawing.Begin( e.Graphics );

			// Draw the dealer's hand
			if (handFinished)
				cardDrawing.DrawCard( new Point( HandX, DealerY ), dealerHand[0] );
			else
				cardDrawing.DrawCardBack( new Point( HandX, DealerY ), selectedCardBack );
			cardDrawing.DrawCard( new Point( HandX + 11, DealerY + 11 ), dealerHand[1] );

			// Draw the dealer's hits
			for (int hitCards = 2; hitCards < dealerHand.Count; hitCards++)
			{
				cardDrawing.DrawCard( new Point( (HitX + HitWidth*(hitCards-2)), DealerY+6 ), dealerHand[hitCards] );
			}

			// Draw the player's hand
			cardDrawing.DrawCard( new Point( HandX, PlayerY ), playerHand[0] );
			cardDrawing.DrawCard( new Point( HandX + 11, PlayerY + 11 ), playerHand[1] );

			// Draw the player's hits
			for (int hitCards = 2; hitCards < playerHand.Count; hitCards++)
			{
				cardDrawing.DrawCard( new Point( (HitX + HitWidth*(hitCards-2)), PlayerY+6 ), playerHand[hitCards] );
			}

			// Write the scores to the screen
			if (CalculateScore(playerHand) <= 21) 
			{
				PlayerScore.Text = CalculateScore(playerHand).ToString("N0");
			}
			else
			{
				PlayerScore.Text = "BUSTED";
			}
			if (handFinished)
			{
				if (CalculateScore(dealerHand) <= 21)
				{
					DealerScore.Text = CalculateScore(dealerHand).ToString("N0");
				}
				else
				{
					DealerScore.Text = "BUSTED";
				}
			}
			else
			{
				DealerScore.Text = string.Empty;
			}

			// Output the number of wins
			DealerWinsDisplay.Text = dealerWins.ToString("N0");
			PlayerWinsDisplay.Text = playerWins.ToString("N0");

			// Draw the stock pile
			for (int offset = 4; offset >= 0; offset--) 
				cardDrawing.DrawCardBack( new Point( StockPileX+offset, StockPileY-offset ), selectedCardBack );
						
			// Finish the drawing routines
			cardDrawing.End();

			base.OnPaint (e);
		}
		#endregion

		#region Private Methods
		private void NewHand()
		{
			// Enabled the Hit and Stay buttons again
			Hit.Enabled = true;
			Stand.Enabled = true;
			StartNewHand.Enabled = false;
			PlayerWinsMessage.Visible = false;
			DealerWinsMessage.Visible = false;
			DrawMessage.Visible = false;

			// Initialize the hand and shuffle the deck
			dealerHand = new CardCollection();
			playerHand = new CardCollection();
			theDeck.Shuffle();

			// Add the first four cards to the hands
			playerHand.Add(theDeck[0]);
			dealerHand.Add(theDeck[1]);
			playerHand.Add(theDeck[2]);
			dealerHand.Add(theDeck[3]);

			// Hide the dealer's hand and set the next card for hitting.
			handFinished = false;
			currentCard = 4;

			// Check for blackjack here
			if (IsBlackjack(playerHand) && IsBlackjack(dealerHand)) 
			{
				PlayerFinished();
				DrawMessage.Visible = true;
			}
			else if (IsBlackjack(playerHand))
			{
				PlayerFinished();
				PlayerWinsMessage.Visible = true;
				playerWins++;
			}
			else if (IsBlackjack(dealerHand))
			{
				PlayerFinished();
				DealerWinsMessage.Visible = true;
				dealerWins++;
			}

			// Draw the updated screen
			this.Refresh();
		}

		private bool IsBlackjack(CardCollection theHand)
		{
			bool faceCard = false;
			bool ace = false;
			
			// check the first card for an ace or face card
			switch (Card.RankFromCardIndex(theHand[0]))
			{
				case CardRank.Ace:
					ace = true;
					break;
				case CardRank.Jack:
				case CardRank.Queen:
				case CardRank.King:
					faceCard = true;
					break;
			}

			// check the second card for an ace or face card
			switch (Card.RankFromCardIndex(theHand[1]))
			{
				case CardRank.Ace:
					ace = true;
					break;
				case CardRank.Jack:
				case CardRank.Queen:
				case CardRank.King:
					faceCard = true;
					break;
			}
			return faceCard && ace;
		}

		private int CalculateScore(CardCollection theHand)
		{
			int score = 0;
			int numAce = 0;
			foreach (int currentCard in theHand)
			{
				switch (Card.RankFromCardIndex(currentCard))
				{
					case CardRank.Ace:
						numAce += 1;
						score += 11;
						break;
					case CardRank.Two:
						score += 2;
						break;
					case CardRank.Three:
						score += 3;
						break;
					case CardRank.Four:
						score += 4;
						break;
					case CardRank.Five:
						score += 5;
						break;
					case CardRank.Six:
						score += 6;
						break;
					case CardRank.Seven:
						score += 7;
						break;
					case CardRank.Eight:
						score += 8;
						break;
					case CardRank.Nine:
						score += 9;
						break;
					case CardRank.Ten:
					case CardRank.Jack:
					case CardRank.Queen:
					case CardRank.King:
						score += 10;
						break;
				}
			}

			// If the score is over 21 and the hand contains aces, lower each ace by 10
			// until the score is 21 or less or until all aces a have a value of 1.
			while (score > 21 && numAce > 0)
			{
				score -= 10;
				numAce--;
			}
			return score;
		}

		private void TakeHit(ref CardCollection theHand)
		{
			theHand.Add(theDeck[currentCard]);
			currentCard++;
		}

		private void PlayerFinished()
		{
			Hit.Enabled = false;
			Stand.Enabled = false;
			StartNewHand.Enabled = true;
			handFinished = true;
		}

		private void DoPlayerHit()
		{
			TakeHit(ref playerHand);
			if (CalculateScore(playerHand) > 21)
			{
				PlayerFinished();		
				this.Refresh();
				dealerWins += 1;
				DealerWinsMessage.Visible = true;
			}
			this.Refresh();
		}
		#endregion

	}
}

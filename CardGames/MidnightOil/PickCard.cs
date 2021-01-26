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
	/// This is used to select a card from a pile
	/// </summary>
	public class PickCard : System.Windows.Forms.Form
	{
		#region Constants
		private const int LeftX = 20;								// The left most position
		private const int TopY = 15;								// The top most position
		private const int CardWidth = 79;							// The width of a card
		private const int CardHeight = 97;							// The height of a card
		private const int DisplayWidth = 12;						// The display size of the card
		#endregion		

		#region Private Fields
		private CardCollection[] theTableau;						// The tableau passed into the class
		private Card drawCard;										// Card object used to draw the cards
		private int selectedIndex;									// The card that is selected in the current pile
		#endregion

		#region Properties
		/// <summary>
		/// The card that was selected to be picked
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
		}

		public int SelectedPile
		{
			get
			{
				return SelectedPileList.SelectedIndex;
			}
		}
		#endregion

		#region Controls
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button Accept;
		private System.Windows.Forms.Label PileLabel;
		private System.Windows.Forms.ComboBox SelectedPileList;
		private System.Windows.Forms.Button Cancel;
		#endregion
	
		#region Constructors
		public PickCard(CardCollection[] passedInTableau)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize all data structures here
			theTableau = passedInTableau;
			drawCard = new Card();
			selectedIndex = -1;
		}
		#endregion
	
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Accept = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.SelectedPileList = new System.Windows.Forms.ComboBox();
			this.PileLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Accept
			// 
			this.Accept.BackColor = System.Drawing.Color.Yellow;
			this.Accept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Accept.Enabled = false;
			this.Accept.Location = new System.Drawing.Point(112, 136);
			this.Accept.Name = "Accept";
			this.Accept.Size = new System.Drawing.Size(75, 24);
			this.Accept.TabIndex = 1;
			this.Accept.Text = "&Accept";
			this.Accept.Click += new System.EventHandler(this.Accept_Click);
			// 
			// Cancel
			// 
			this.Cancel.BackColor = System.Drawing.Color.Red;
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(200, 136);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 2;
			this.Cancel.Text = "&Cancel";
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// SelectedPileList
			// 
			this.SelectedPileList.BackColor = System.Drawing.Color.Green;
			this.SelectedPileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SelectedPileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.SelectedPileList.ForeColor = System.Drawing.Color.Black;
			this.SelectedPileList.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10",
																  "11",
																  "12",
																  "13",
																  "14",
																  "15",
																  "16",
																  "17",
																  "18"});
			this.SelectedPileList.Location = new System.Drawing.Point(40, 136);
			this.SelectedPileList.Name = "SelectedPileList";
			this.SelectedPileList.Size = new System.Drawing.Size(48, 21);
			this.SelectedPileList.TabIndex = 3;
			this.SelectedPileList.SelectedIndex = 0;
			this.SelectedPileList.SelectedIndexChanged += new System.EventHandler(this.SelectedPileList_SelectedIndexChanged);
			// 
			// PileLabel
			// 
			this.PileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PileLabel.ForeColor = System.Drawing.Color.Black;
			this.PileLabel.Location = new System.Drawing.Point(8, 135);
			this.PileLabel.Name = "PileLabel";
			this.PileLabel.Size = new System.Drawing.Size(32, 22);
			this.PileLabel.TabIndex = 4;
			this.PileLabel.Text = "Pile";
			this.PileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PickCard
			// 
			this.AcceptButton = this.Accept;
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(290, 176);
			this.ControlBox = false;
			this.Controls.Add(this.PileLabel);
			this.Controls.Add(this.SelectedPileList);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Accept);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PickCard";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select a Card";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PickCard_MouseDown);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PickCard_Paint);
			this.ResumeLayout(false);

		}
		#endregion
	
		#region Form Events
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PickCard_MouseDown(object sender, MouseEventArgs e)
		{
			int oldSelection = selectedIndex;
			int index = SelectedPile;

			for (int count = 0; count < theTableau[SelectedPile].Count; count++)
			{
				int left = LeftX + count * DisplayWidth;				
				int right = left + DisplayWidth;
				int top = TopY;
				int bottom = top + CardHeight;

				if (count == theTableau[SelectedPile].Count - 1)
				{
					right += CardWidth - DisplayWidth;
				}

				if ((e.X > left && e.X < right) && (e.Y > top && e.Y < bottom))
				{
					if (oldSelection == count)
					{
						selectedIndex = -1;
						Accept.Enabled = false;
					}
					else
					{
						selectedIndex = count;
						Accept.Enabled = true;
					}
					DrawPile();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PickCard_Paint(object sender, PaintEventArgs e)
		{
			DrawPile();
		}
		#endregion

		#region Control Events
		private void SelectedPileList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Accept.Enabled = false;
			selectedIndex = -1;
			DrawPile();
		}

		private void Accept_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
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
		private void DrawPile()
		{
			Rectangle oldCardArea = new Rectangle(LeftX, TopY, CardWidth + DisplayWidth * 14, CardHeight);
			this.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), oldCardArea);

			int index = SelectedPileList.SelectedIndex;
			for (int count = 0; count < theTableau[index].Count; count++)
			{
				drawCard.Begin(this.CreateGraphics());
				if (selectedIndex == count)
				{
					drawCard.DrawHighlightedCard(new Point(LeftX + DisplayWidth * count, TopY), theTableau[index][count]);
				}
				else
				{
					drawCard.DrawCard(new Point(LeftX + DisplayWidth * count, TopY), theTableau[index][count]);
				}

				drawCard.End();
			}
		}
		#endregion
	}
}
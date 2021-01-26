using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace CardGames.Test
{
	/// <summary>
	/// Summary description for CardControl.
	/// </summary>
	public class CardControl : System.Windows.Forms.UserControl
	{
		public event MouseEventHandler MoveControl;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Card drawCard;
		private int cardIndex;
		private bool mousedown;

		public CardControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			drawCard = new Card();
			cardIndex = 0;
		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// CardControl
			// 
			this.Name = "CardControl";
			this.Size = new System.Drawing.Size(75, 95);
			this.Paint += new PaintEventHandler(CardControl_Paint);
			this.MouseDown += new MouseEventHandler(CardControl_MouseDown);
			this.MouseUp += new MouseEventHandler(CardControl_MouseUp);
			this.MouseMove += new MouseEventHandler(CardControl_MouseMove);

		}
		#endregion

		private void CardControl_Paint(object sender, PaintEventArgs e)
		{
			drawCard.Begin(e.Graphics);
			drawCard.DrawCard(new Point(0,0), cardIndex);
			drawCard.End();
		}

		private void CardControl_MouseDown(object sender, MouseEventArgs e)
		{
			mousedown = true;
		}


		private void CardControl_MouseUp(object sender, MouseEventArgs e)
		{
			mousedown = false;

		}

		private void CardControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (MoveControl != null)
			{
				MoveControl(this, e);
			}
			if (mousedown)
			{
//				Point location = this.Location;
//				location.X = e.X;
//				location.Y = e.Y;
//				this.Location = location;
			}
		}
	}
}

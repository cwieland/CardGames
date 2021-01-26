namespace CardGames.Test
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Reflection;
	using System.Resources;
	using System.Windows.Forms;
	#endregion

	/// <summary>
	/// This is a test form that demonstrates how to move a card
	/// </summary>
	public class MoveCard : System.Windows.Forms.Form
	{		
		#region Constants
		private const int cardwidth = 79;
		private const int cardheight = 97;
		#endregion

		#region Controls
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label Event;
		private System.Windows.Forms.Label Position;
		private CardControl test;
		#endregion

		#region Private Fields
		#endregion

		#region Constructors
		public MoveCard()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.test = new CardControl();
			this.test.Location = new System.Drawing.Point(48, 104);
			this.test.MoveControl += new MouseEventHandler(test_MoveControl);
			this.Controls.Add(this.test);
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Position = new System.Windows.Forms.Label();
			this.Event = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Position
			// 
			this.Position.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Position.Location = new System.Drawing.Point(16, 8);
			this.Position.Name = "Position";
			this.Position.TabIndex = 0;
			// 
			// Event
			// 
			this.Event.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Event.Location = new System.Drawing.Point(16, 32);
			this.Event.Name = "Event";
			this.Event.TabIndex = 1;
			// 
			// DragTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(672, 390);
			this.Controls.Add(this.Event);
			this.Controls.Add(this.Position);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DragTest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DragTest";
			this.ResumeLayout(false);

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

		private void test_MoveControl(object sender, MouseEventArgs e)
		{
			Point location = this.test.Location;
			location.X = e.X;
			location.Y = e.Y;
			this.test.Location = location;
		}
	}
}

// CardGames wish list
//  Make user control for card object???
//  Edit small icon
//  Move common functionality to seperate classes
//  Document author tags on all methods and classes

namespace CardGames
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	using System.Data;
	using System.Text;
	using System.Reflection;
	#endregion

	/// <summary>
	/// This is the main form that allows the user to select a game.
	/// </summary>
	public class SelectGame : System.Windows.Forms.Form
	{
		#region Controls
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label SelectGameMessage;
		private System.Windows.Forms.LinkLabel BlackjackLink;
		private System.Windows.Forms.LinkLabel Freecell;
		private System.Windows.Forms.LinkLabel AcesUp;
		private System.Windows.Forms.LinkLabel Calculation;
		private System.Windows.Forms.LinkLabel Canfield;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem FileMenuItem;
		private System.Windows.Forms.MenuItem ExitMenuItem;
		private System.Windows.Forms.MenuItem OptionsMenuItem;
		private System.Windows.Forms.MenuItem PreferencesMenuItem;
		private System.Windows.Forms.LinkLabel Klondike;
		private System.Windows.Forms.LinkLabel MonteCarlo;
		private System.Windows.Forms.LinkLabel MidnightOil;
		private System.Windows.Forms.LinkLabel BeleagueredCastle;
		private System.Windows.Forms.LinkLabel Pyramid;
		private System.Windows.Forms.LinkLabel Golf;
		private System.Windows.Forms.LinkLabel Corners;
		private System.Windows.Forms.LinkLabel Reno;
		private System.Windows.Forms.LinkLabel RoyalCotillion;
		private System.Windows.Forms.MenuItem HelpMenuItem;
		private System.Windows.Forms.MenuItem About;
		private System.Windows.Forms.LinkLabel Towers;
		private System.Windows.Forms.LinkLabel Osmosis;
		#endregion

		#region Constructors
		public SelectGame()
		{
			//
			// Required for Windows Form Designer support
			//
			Preferences.GetPreferences();
			InitializeComponent();
			//(new Test.MoveCard()).ShowDialog();
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SelectGameMessage = new System.Windows.Forms.Label();
			this.BlackjackLink = new System.Windows.Forms.LinkLabel();
			this.Freecell = new System.Windows.Forms.LinkLabel();
			this.AcesUp = new System.Windows.Forms.LinkLabel();
			this.Calculation = new System.Windows.Forms.LinkLabel();
			this.Canfield = new System.Windows.Forms.LinkLabel();
			this.Osmosis = new System.Windows.Forms.LinkLabel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.FileMenuItem = new System.Windows.Forms.MenuItem();
			this.ExitMenuItem = new System.Windows.Forms.MenuItem();
			this.OptionsMenuItem = new System.Windows.Forms.MenuItem();
			this.PreferencesMenuItem = new System.Windows.Forms.MenuItem();
			this.HelpMenuItem = new System.Windows.Forms.MenuItem();
			this.About = new System.Windows.Forms.MenuItem();
			this.Klondike = new System.Windows.Forms.LinkLabel();
			this.MonteCarlo = new System.Windows.Forms.LinkLabel();
			this.MidnightOil = new System.Windows.Forms.LinkLabel();
			this.BeleagueredCastle = new System.Windows.Forms.LinkLabel();
			this.Pyramid = new System.Windows.Forms.LinkLabel();
			this.Golf = new System.Windows.Forms.LinkLabel();
			this.Corners = new System.Windows.Forms.LinkLabel();
			this.Reno = new System.Windows.Forms.LinkLabel();
			this.RoyalCotillion = new System.Windows.Forms.LinkLabel();
			this.Towers = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// SelectGameMessage
			// 
			this.SelectGameMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.SelectGameMessage.ForeColor = System.Drawing.Color.Red;
			this.SelectGameMessage.Location = new System.Drawing.Point(8, 8);
			this.SelectGameMessage.Name = "SelectGameMessage";
			this.SelectGameMessage.Size = new System.Drawing.Size(312, 16);
			this.SelectGameMessage.TabIndex = 0;
			this.SelectGameMessage.Text = "Please Select A Game";
			this.SelectGameMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BlackjackLink
			// 
			this.BlackjackLink.LinkColor = System.Drawing.Color.Yellow;
			this.BlackjackLink.Location = new System.Drawing.Point(216, 32);
			this.BlackjackLink.Name = "BlackjackLink";
			this.BlackjackLink.Size = new System.Drawing.Size(104, 16);
			this.BlackjackLink.TabIndex = 3;
			this.BlackjackLink.TabStop = true;
			this.BlackjackLink.Text = "Blackjack";
			this.BlackjackLink.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.BlackjackLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.BlackjackLink_LinkClicked);
			// 
			// Freecell
			// 
			this.Freecell.LinkColor = System.Drawing.Color.Yellow;
			this.Freecell.Location = new System.Drawing.Point(8, 64);
			this.Freecell.Name = "Freecell";
			this.Freecell.Size = new System.Drawing.Size(104, 16);
			this.Freecell.TabIndex = 6;
			this.Freecell.TabStop = true;
			this.Freecell.Text = "Freecell";
			this.Freecell.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Freecell.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Freecell_LinkClicked);
			// 
			// AcesUp
			// 
			this.AcesUp.LinkColor = System.Drawing.Color.Yellow;
			this.AcesUp.Location = new System.Drawing.Point(8, 32);
			this.AcesUp.Name = "AcesUp";
			this.AcesUp.Size = new System.Drawing.Size(104, 16);
			this.AcesUp.TabIndex = 1;
			this.AcesUp.TabStop = true;
			this.AcesUp.Text = "Aces Up";
			this.AcesUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.AcesUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AcesUp_LinkClicked);
			// 
			// Calculation
			// 
			this.Calculation.LinkColor = System.Drawing.Color.Yellow;
			this.Calculation.Location = new System.Drawing.Point(8, 48);
			this.Calculation.Name = "Calculation";
			this.Calculation.Size = new System.Drawing.Size(104, 16);
			this.Calculation.TabIndex = 4;
			this.Calculation.TabStop = true;
			this.Calculation.Text = "Calculation";
			this.Calculation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Calculation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Calculation_LinkClicked);
			// 
			// Canfield
			// 
			this.Canfield.LinkColor = System.Drawing.Color.Yellow;
			this.Canfield.Location = new System.Drawing.Point(112, 48);
			this.Canfield.Name = "Canfield";
			this.Canfield.Size = new System.Drawing.Size(104, 16);
			this.Canfield.TabIndex = 5;
			this.Canfield.TabStop = true;
			this.Canfield.Text = "Canfield";
			this.Canfield.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Canfield.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Canfield_LinkClicked);
			// 
			// Osmosis
			// 
			this.Osmosis.LinkColor = System.Drawing.Color.Yellow;
			this.Osmosis.Location = new System.Drawing.Point(216, 80);
			this.Osmosis.Name = "Osmosis";
			this.Osmosis.Size = new System.Drawing.Size(104, 16);
			this.Osmosis.TabIndex = 10;
			this.Osmosis.TabStop = true;
			this.Osmosis.Text = "Osmosis";
			this.Osmosis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Osmosis.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Osmosis_LinkClicked);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.FileMenuItem,
																					  this.OptionsMenuItem,
																					  this.HelpMenuItem});
			// 
			// FileMenuItem
			// 
			this.FileMenuItem.Index = 0;
			this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.ExitMenuItem});
			this.FileMenuItem.Text = "&File";
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Index = 0;
			this.ExitMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// OptionsMenuItem
			// 
			this.OptionsMenuItem.Index = 1;
			this.OptionsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.PreferencesMenuItem});
			this.OptionsMenuItem.Text = "&Options";
			// 
			// PreferencesMenuItem
			// 
			this.PreferencesMenuItem.Index = 0;
			this.PreferencesMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.PreferencesMenuItem.Text = "&Preferences";
			this.PreferencesMenuItem.Click += new System.EventHandler(this.PreferencesMenuItem_Click);
			// 
			// HelpMenuItem
			// 
			this.HelpMenuItem.Index = 2;
			this.HelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.About});
			this.HelpMenuItem.Text = "&Help";
			// 
			// About
			// 
			this.About.Index = 0;
			this.About.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.About.Text = "&About";
			this.About.Click += new System.EventHandler(this.About_Click);
			// 
			// Klondike
			// 
			this.Klondike.LinkColor = System.Drawing.Color.Yellow;
			this.Klondike.Location = new System.Drawing.Point(216, 64);
			this.Klondike.Name = "Klondike";
			this.Klondike.Size = new System.Drawing.Size(104, 16);
			this.Klondike.TabIndex = 7;
			this.Klondike.TabStop = true;
			this.Klondike.Text = "Klondike";
			this.Klondike.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Klondike.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Klondike_LinkClicked);
			// 
			// MonteCarlo
			// 
			this.MonteCarlo.LinkColor = System.Drawing.Color.Yellow;
			this.MonteCarlo.Location = new System.Drawing.Point(112, 80);
			this.MonteCarlo.Name = "MonteCarlo";
			this.MonteCarlo.Size = new System.Drawing.Size(104, 16);
			this.MonteCarlo.TabIndex = 9;
			this.MonteCarlo.TabStop = true;
			this.MonteCarlo.Text = "Monte Carlo";
			this.MonteCarlo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.MonteCarlo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MonteCarlo_LinkClicked);
			// 
			// MidnightOil
			// 
			this.MidnightOil.LinkColor = System.Drawing.Color.Yellow;
			this.MidnightOil.Location = new System.Drawing.Point(8, 80);
			this.MidnightOil.Name = "MidnightOil";
			this.MidnightOil.Size = new System.Drawing.Size(104, 16);
			this.MidnightOil.TabIndex = 8;
			this.MidnightOil.TabStop = true;
			this.MidnightOil.Text = "Midnight Oil";
			this.MidnightOil.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.MidnightOil.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MidnightOil_LinkClicked);
			// 
			// BeleagueredCastle
			// 
			this.BeleagueredCastle.LinkColor = System.Drawing.Color.Yellow;
			this.BeleagueredCastle.Location = new System.Drawing.Point(112, 32);
			this.BeleagueredCastle.Name = "BeleagueredCastle";
			this.BeleagueredCastle.Size = new System.Drawing.Size(104, 16);
			this.BeleagueredCastle.TabIndex = 2;
			this.BeleagueredCastle.TabStop = true;
			this.BeleagueredCastle.Text = "Beleaguered Castle";
			this.BeleagueredCastle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.BeleagueredCastle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.BeleagueredCastle_LinkClicked);
			// 
			// Pyramid
			// 
			this.Pyramid.LinkColor = System.Drawing.Color.Yellow;
			this.Pyramid.Location = new System.Drawing.Point(8, 96);
			this.Pyramid.Name = "Pyramid";
			this.Pyramid.Size = new System.Drawing.Size(104, 16);
			this.Pyramid.TabIndex = 11;
			this.Pyramid.TabStop = true;
			this.Pyramid.Text = "Pyramid";
			this.Pyramid.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Pyramid.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Pyramid_LinkClicked);
			// 
			// Golf
			// 
			this.Golf.LinkColor = System.Drawing.Color.Yellow;
			this.Golf.Location = new System.Drawing.Point(112, 64);
			this.Golf.Name = "Golf";
			this.Golf.Size = new System.Drawing.Size(104, 16);
			this.Golf.TabIndex = 12;
			this.Golf.TabStop = true;
			this.Golf.Text = "Golf";
			this.Golf.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Golf.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Golf_LinkClicked);
			// 
			// Corners
			// 
			this.Corners.LinkColor = System.Drawing.Color.Yellow;
			this.Corners.Location = new System.Drawing.Point(216, 48);
			this.Corners.Name = "Corners";
			this.Corners.Size = new System.Drawing.Size(104, 16);
			this.Corners.TabIndex = 13;
			this.Corners.TabStop = true;
			this.Corners.Text = "Corners";
			this.Corners.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Corners.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Corners_LinkClicked);
			// 
			// Reno
			// 
			this.Reno.LinkColor = System.Drawing.Color.Yellow;
			this.Reno.Location = new System.Drawing.Point(112, 96);
			this.Reno.Name = "Reno";
			this.Reno.Size = new System.Drawing.Size(104, 16);
			this.Reno.TabIndex = 14;
			this.Reno.TabStop = true;
			this.Reno.Text = "Reno";
			this.Reno.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Reno.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Reno_LinkClicked);
			// 
			// RoyalCotillion
			// 
			this.RoyalCotillion.LinkColor = System.Drawing.Color.Yellow;
			this.RoyalCotillion.Location = new System.Drawing.Point(216, 96);
			this.RoyalCotillion.Name = "RoyalCotillion";
			this.RoyalCotillion.Size = new System.Drawing.Size(104, 16);
			this.RoyalCotillion.TabIndex = 15;
			this.RoyalCotillion.TabStop = true;
			this.RoyalCotillion.Text = "Royal Cotillion";
			this.RoyalCotillion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.RoyalCotillion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RoyalCotillion_LinkClicked);
			// 
			// Towers
			// 
			this.Towers.LinkColor = System.Drawing.Color.Yellow;
			this.Towers.Location = new System.Drawing.Point(8, 112);
			this.Towers.Name = "Towers";
			this.Towers.Size = new System.Drawing.Size(104, 16);
			this.Towers.TabIndex = 16;
			this.Towers.TabStop = true;
			this.Towers.Text = "Towers";
			this.Towers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.Towers.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Towers_LinkClicked);
			// 
			// SelectGame
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(322, 139);
			this.ControlBox = false;
			this.Controls.Add(this.Towers);
			this.Controls.Add(this.RoyalCotillion);
			this.Controls.Add(this.Reno);
			this.Controls.Add(this.Corners);
			this.Controls.Add(this.Golf);
			this.Controls.Add(this.Pyramid);
			this.Controls.Add(this.BeleagueredCastle);
			this.Controls.Add(this.MidnightOil);
			this.Controls.Add(this.MonteCarlo);
			this.Controls.Add(this.Klondike);
			this.Controls.Add(this.Osmosis);
			this.Controls.Add(this.Canfield);
			this.Controls.Add(this.Calculation);
			this.Controls.Add(this.AcesUp);
			this.Controls.Add(this.Freecell);
			this.Controls.Add(this.BlackjackLink);
			this.Controls.Add(this.SelectGameMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Menu = this.mainMenu1;
			this.Name = "SelectGame";
			this.ShowInTaskbar = false;
			this.Text = "Select A Game";
			this.ResumeLayout(false);

		}
		#endregion

		#region Control Events
		private void AcesUp_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new AcesUp.AcesUp()).ShowDialog();
			this.Show();		
		}

		private void BeleagueredCastle_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new BelegueredCastle.BelegueredCastle()).ShowDialog();
			this.Show();
		}

		private void BlackjackLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Blackjack.Blackjack()).ShowDialog();
			this.Show();
		}

		private void Calculation_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Calculation.Calculation()).ShowDialog();
			this.Show();		
		}

		private void Canfield_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Canfield.Canfield()).ShowDialog();
			this.Show();
		}

		private void Corners_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Corners.Corners()).ShowDialog();
			this.Show();
		}

		private void Freecell_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Freecell.Freecell()).ShowDialog();
			this.Show();
		}

		private void Golf_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Golf.Golf()).ShowDialog();
			this.Show();
		}

		private void Klondike_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Klondike.Klondike()).ShowDialog();
			this.Show();
		}

		private void MidnightOil_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new MidnightOil.MidnightOil()).ShowDialog();
			this.Show();
		}

		private void MonteCarlo_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new MonteCarlo.MonteCarlo()).ShowDialog();
			this.Show();
		}
	
		private void Osmosis_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Osmosis.Osmosis()).ShowDialog();
			this.Show();
		}

		private void Pyramid_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show("Not implemented yet!");
		}

		private void Reno_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Reno.Reno()).ShowDialog();
			this.Show();
		}

		private void RoyalCotillion_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new RoyalCotillion.RoyalCotillion()).ShowDialog();
			this.Show();
		}

		private void PreferencesMenuItem_Click(object sender, System.EventArgs e)
		{
			(new Options()).ShowDialog();
			this.ShowInTaskbar = Preferences.ShowInTaskbar;		
		}
		
		private void About_Click(object sender, System.EventArgs e)
		{
			StringBuilder infoMessage = new StringBuilder();
			infoMessage.Append("Card Games\n\nWritten by Charles Wieland\nVersion ");
			infoMessage.Append(Assembly.GetExecutingAssembly().GetName().Version + "\n");
			infoMessage.Append("(c) 2005-2007 Junkosoft");
			MessageBox.Show(infoMessage.ToString(), "About Card Games", MessageBoxButtons.OK);
		}

		private void Towers_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			this.Hide();
			(new Towers.Towers()).ShowDialog();
			this.Show();
		}

		private void ExitMenuItem_Click(object sender, System.EventArgs e)
		{
			this.Close();
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new SelectGame());
		}
		#endregion
	}
}

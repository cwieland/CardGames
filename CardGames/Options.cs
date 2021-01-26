namespace CardGames
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	#endregion
	
	/// <summary>
	/// This is the global preferences dialog box.
	/// </summary>
	public class Options : System.Windows.Forms.Form
	{
		#region Controls
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.CheckBox PlaySound;
		private System.Windows.Forms.CheckBox Taskbar;
		private System.ComponentModel.Container components = null;	// Required designer variable.
		#endregion

		#region Constructor
		public Options()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Preferences.GetPreferences();
			PlaySound.Checked = Preferences.SoundEnabled;
			Taskbar.Checked = Preferences.ShowInTaskbar;
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.PlaySound = new System.Windows.Forms.CheckBox();
			this.Taskbar = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// OK
			// 
			this.OK.Location = new System.Drawing.Point(24, 72);
			this.OK.Name = "OK";
			this.OK.TabIndex = 0;
			this.OK.Text = "&OK";
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(160, 72);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "&Cancel";
			// 
			// PlaySound
			// 
			this.PlaySound.Location = new System.Drawing.Point(16, 16);
			this.PlaySound.Name = "PlaySound";
			this.PlaySound.Size = new System.Drawing.Size(240, 24);
			this.PlaySound.TabIndex = 2;
			this.PlaySound.Text = "Play sound when an invalid move is made";
			// 
			// Taskbar
			// 
			this.Taskbar.Location = new System.Drawing.Point(16, 40);
			this.Taskbar.Name = "Taskbar";
			this.Taskbar.Size = new System.Drawing.Size(240, 24);
			this.Taskbar.TabIndex = 3;
			this.Taskbar.Text = "Show In Taskbar";
			// 
			// Options
			// 
			this.AcceptButton = this.OK;
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(264, 112);
			this.ControlBox = false;
			this.Controls.Add(this.Taskbar);
			this.Controls.Add(this.PlaySound);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Global Preferences";
			this.ResumeLayout(false);

		}
		#endregion

		#region Control Events
		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				Preferences.SoundEnabled = PlaySound.Checked;
				Preferences.ShowInTaskbar = Taskbar.Checked;
				Preferences.SetPreferences();
			}
			catch 
			{
				MessageBox.Show("Error Saving Preferences", "Card Games", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally 
			{
				this.Close();
			}
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
	}
}

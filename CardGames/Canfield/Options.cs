namespace CardGames.Canfield
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Forms;
	#endregion
	
	/// <summary>
	/// This is the Canfield preferences dialog box.
	/// </summary>
	public class Options : System.Windows.Forms.Form
	{
		#region Controls
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.CheckBox AutoMove;
		private System.Windows.Forms.CheckBox ShowMoney;
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
			ShowMoney.Checked = Preferences.ShowMoney;
			AutoMove.Checked = Preferences.AutoMove;
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
			this.AutoMove = new System.Windows.Forms.CheckBox();
			this.ShowMoney = new System.Windows.Forms.CheckBox();
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
			// AutoMove
			// 
			this.AutoMove.Location = new System.Drawing.Point(16, 16);
			this.AutoMove.Name = "AutoMove";
			this.AutoMove.Size = new System.Drawing.Size(240, 24);
			this.AutoMove.TabIndex = 2;
			this.AutoMove.Text = "Automatically move cards from reserve pile";
			// 
			// ShowMoney
			// 
			this.ShowMoney.Location = new System.Drawing.Point(16, 40);
			this.ShowMoney.Name = "ShowMoney";
			this.ShowMoney.Size = new System.Drawing.Size(240, 24);
			this.ShowMoney.TabIndex = 3;
			this.ShowMoney.Text = "Show money meter";
			// 
			// Options
			// 
			this.AcceptButton = this.OK;
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(264, 112);
			this.ControlBox = false;
			this.Controls.Add(this.ShowMoney);
			this.Controls.Add(this.AutoMove);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Canfield Preferences";
			this.ResumeLayout(false);

		}
		#endregion

		#region Control Events
		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				Preferences.AutoMove = AutoMove.Checked;
				Preferences.ShowMoney = ShowMoney.Checked;
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

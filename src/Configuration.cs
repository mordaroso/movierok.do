// Configuration.cs created with MonoDevelop
// User: mordaroso at 11:05 PM 8/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Do.Addins;
using Gtk;

namespace Movierok
{
	public partial class Configuration : Gtk.Bin
	{
		static IPreferences prefs;
		
		public Configuration()
		{
			this.Build();
			TxtUsername.Text = Username;
			TxtPlayer.Text = Player;
		}
		
		static Configuration ()
		{
			prefs = Do.Addins.Util.GetPreferences ("movierok");
		}
		
		public static string Username {
			get { return prefs.Get<string> ("Username",""); }
			set { prefs.Set<string> ("Username", value); }
		}
		
		public static string Player {
			get { return prefs.Get<string> ("Player", "vlc"); }
			set { prefs.Set<string> ("Player", value); }
		}

		protected virtual void OnTxtUsernameChanged (object sender, System.EventArgs e)
		{
			Username = TxtUsername.Text;
		}
		
		protected virtual void OnTxtPlayerChanged (object sender, System.EventArgs e)
		{
			Player = TxtPlayer.Text;
		}
	}
}

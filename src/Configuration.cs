/*
 * Configuration.cs
 * 
 * GNOME Do is the legal property of its developers, whose names are too numerous
 * to list here.  Please refer to the COPYRIGHT file distributed with this
 * source distribution.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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

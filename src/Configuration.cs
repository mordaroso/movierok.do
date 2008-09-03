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
using System.Collections.Generic;
using Gtk;
using Do;
using Do.Addins;

namespace Movierok
{
	public partial class Configuration : Gtk.Bin
	{
		static IPreferences prefs;
		
		public Configuration()
		{
			this.Build();
			txtUsername.Text = Username;
			chsrPlayer.SetFilename(Player);
			FillCombo();
		}
		
		void FillCombo ()
		{
			ListStore store = new ListStore(typeof (string));
			cmbProfiles.Model = store;
			TreeIter iter = new TreeIter();
			
			List<FirefoxProfile> profiles = FirefoxProfile.All;
			foreach(FirefoxProfile profile in profiles){
				TreeIter tmpIter = store.AppendValues(profile.Name);
				if (Profile == ""  && profile.isDefault)
					Profile = profile.Name;
				if(Profile == profile.Name){
					iter = tmpIter;
				}
			}
			
			cmbProfiles.SetActiveIter(iter); 
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
			get { return prefs.Get<string> ("Player", "/usr/bin/vlc"); }
			set { prefs.Set<string> ("Player", value); }
		}
		
		public static string Profile {
			get {return prefs.Get<string> ("Profile", ""); }
			set { prefs.Set<string> ("Profile", value); }
		}

		protected virtual void OnTxtUsernameChanged (object sender, System.EventArgs e)
		{
			Username = txtUsername.Text;
		}
		

		protected virtual void OnChooserPlayerSelectionChanged (object sender, System.EventArgs e)
		{
			Player = chsrPlayer.Filename;
		}

		protected virtual void OnCmbProfilesChanged (object sender, System.EventArgs e)
		{
			TreeIter iter;

			if (cmbProfiles.GetActiveIter (out iter))
				Profile = (string) cmbProfiles.Model.GetValue (iter, 0);
		}

		protected virtual void OnBtnAccountClicked (object sender, System.EventArgs e)
		{
			Util.Environment.Open ("http://" + MovierokDo.remote + "/users/new");
		}
		
	}
}

/*
 * FirefoxProfile.cs
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
using System.IO;
using System.Collections.Generic;

namespace Movierok
{
	public class FirefoxProfile {
		const string BeginProfile = "[Profile";
		const string BeginProfileName = "Name=";
		const string BeginProfilePath = "Path=";
		const string BeginDefaultProfile = "Default=1";
		
		protected string name, path;
		protected bool is_default = false;
		
		public virtual string text {
			get{return Name;}
		}
		
		public virtual string Name { 
			get { return name; } 
			set { name = value; }
		}
		
		public virtual string Path { 
			get { return path; } 
			set { path = value; }
		}
		
		public virtual bool isDefault { 
			get { return is_default; } 
			set { is_default = value; }
		}
		
		public static FirefoxProfile getProfile(string name){
			List<FirefoxProfile> profiles = All;
			foreach(FirefoxProfile profile in profiles){
				if(profile.Name == name){
					return profile;
				}
			}
			return null;
		}
		
		public static List<FirefoxProfile> All {
			get{			
				string line, inipath, home;				
				List<FirefoxProfile> profiles =  new List<FirefoxProfile>();
				home =  Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				inipath = "~/.mozilla/firefox/profiles.ini".Replace ("~", home);
				FirefoxProfile profile = null;
				using (StreamReader r = File.OpenText (inipath)) {
					while (null != (line = r.ReadLine ())) {
						if (line.StartsWith (BeginProfile)){
							if(profile != null)
								profiles.Add(profile);
							profile = new FirefoxProfile();
						}
						else if (line.StartsWith (BeginProfileName)) {
							line = line.Trim ();
							line = line.Substring (BeginProfileName.Length);
							profile.Name = line;
						}else if (line.StartsWith (BeginProfilePath)) {
							line = line.Trim ();
							line = line.Substring (BeginProfilePath.Length);
							profile.Path = line;
						}else if (line.StartsWith (BeginDefaultProfile)) {
							profile.isDefault = true;
						}
					}
				}
				if(profile != null)
					profiles.Add(profile);
				return profiles;
			}
		}
	}
}
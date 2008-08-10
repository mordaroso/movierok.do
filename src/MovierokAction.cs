/*
 * MovierokAction.cs
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
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Collections; 
using Mono.Data.SqliteClient;
using System.Diagnostics;

using Do;
using Do.Universe;
using Mono.Unix;



namespace Movierok
{		
	public sealed class MovierokAction : AbstractAction
	{
		const string BeginProfileName = "Path=";
		const string BeginDefaultProfile = "Default=1";
		
		public override string Name {
			get { return Catalog.GetString ("Play"); }
		}
		
		public override string Description {
			get { return Catalog.GetString ("Play rip"); }
		}
		
		public override string Icon {
			get { return "video"; }
		}
		
		public override Type [] SupportedItemTypes {
            get {
                return new Type [] {
                    typeof (RipItem),
                };
            }
        }
		
		public override IItem[] Perform (IItem[] items, IItem[] modifierItems)
		{
			new Thread ((ThreadStart) delegate {
				foreach (RipItem item in items ) {
					
					Console.Out.WriteLine(item.Name + " start");
					List<string> paths = GetPathsByChecksums(item.Parts);					
					string args = "";					
					foreach(string path in paths)
						args += "'"+path+"' ";
					Process.Start(Configuration.Player, args);
				}
			}).Start ();
			return null;
		}
		
		/// <summary>
		/// Looks in the firefox profiles file (~/.mozilla/firefox/profiles.ini)
		/// for the name of the default profile, and returns the path to the
		/// default profile.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> containing the absolute path to the
		/// bookmarks.html file of the default firefox profile for the current
		/// user.
		/// </returns>
		static string profile_path;
		static string ProfilePath {
			get {
				string line, profile, path;
				
				if (null != profile_path) return profile_path;

				profile = null;
				path = Path.Combine (Paths.UserHome, ".mozilla/firefox/profiles.ini");
				using (StreamReader r = File.OpenText (path)) {
					while (null != (line = r.ReadLine ())) {
						if (line.StartsWith (BeginDefaultProfile)) break;
						if (line.StartsWith (BeginProfileName)) {
							line = line.Trim ();
							line = line.Substring (BeginProfileName.Length);
							profile = line;
						}
					}
				}
				return profile_path = Paths.Combine (
					Paths.UserHome, ".mozilla/firefox", profile);
			}
		}
		
		public static List<string> GetPathsByChecksums (List<string> checksums) {
			List<string> paths = new List<string> ();
			try{
				string connectionString = "URI=file:~/movierok.sqlite,version=3".Replace ("~", ProfilePath);
				IDbConnection dbcon;
				dbcon = (IDbConnection) new SqliteConnection(connectionString);
				dbcon.Open();
				IDbCommand dbcmd = dbcon.CreateCommand();		
				foreach(string checksum in checksums){
					string sql =
					"SELECT path FROM parts WHERE checksum  = '"+checksum+"'";
					dbcmd.CommandText = sql;
					IDataReader reader = dbcmd.ExecuteReader();
					while(reader.Read()) {
						string path = reader.GetString (0);    
						Console.Out.WriteLine(path);
						paths.Add (path);
					}
					// clean up
					reader.Close();
					reader = null;
				}
				dbcmd.Dispose();
				dbcmd = null;
				dbcon.Close();
				dbcon = null;
			} catch (Exception e) {
				// Something went horribly wrong, so we print the error message.
				Console.Error.WriteLine ("Could not read checksums: {0}, {1}", e.Message, e.StackTrace);
			}
			return paths;
		}
	}
}
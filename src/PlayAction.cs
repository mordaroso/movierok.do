/*
 * PlayAction.cs
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
	public sealed class PlayAction : AbstractAction
	{
		
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
					List<string> paths = GetPathsByMrokhashes(item.Parts);					
					string args = "";					
					foreach(string path in paths)
						args += "'"+path+"' ";
					Process.Start(Configuration.Player, args);
				}
			}).Start ();
			return null;
		}
		
		public static List<string> GetPathsByMrokhashes (List<string> mrokhashes) {
			List<string> paths = new List<string> ();
			string connectionString = "";
			try{
				FirefoxProfile profile = FirefoxProfile.getProfile(Configuration.Profile);
				connectionString = "URI=file:~/movierok.sqlite,version=3".Replace ("~", profile.Path);
				IDbConnection dbcon;
				dbcon = (IDbConnection) new SqliteConnection(connectionString);
				dbcon.Open();
				IDbCommand dbcmd = dbcon.CreateCommand();		
				foreach(string mrokhash in mrokhashes){
					string sql =
					"SELECT path FROM parts WHERE mrokhash  = '"+mrokhash+"'";
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
				Console.Error.WriteLine ("Could not read mrokhashes in {0}: {1}, {2}", connectionString ,e.Message, e.StackTrace);
			}
			return paths;
		}
	}
}

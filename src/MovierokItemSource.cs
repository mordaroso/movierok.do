/*
 * MovierokItemSource.cs
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

using Do.Addins;
using Do.Universe;

using Mono.Unix;

namespace Movierok {
  
	public class MovierokItemSource : IItemSource, IConfigurable {

    public MovierokItemSource ()
    {
	  
    }

    public string Name {
			get { return Catalog.GetString ("movierok movies"); }
		}
		
		public string Description {
			get { return Catalog.GetString ("Finds movierok movies to play."); }
		}
		
		public string Icon {
			get { return "movierok_logo_192x192_black.png@" + GetType ().Assembly.FullName; }
		}
		
		public ICollection<IItem> Items {
			get {
				List<IItem> items = new List<IItem>();
				//items.AddRange(MovierokDo.Movies);
				items.AddRange(MovierokDo.Rips);
				return items;
			}
		}
		
		public ICollection<IItem> ChildrenOfItem (IItem item)
		{
			return null;
		}
		
		public Type[] SupportedItemTypes {
			get {
				return new Type[] {
					typeof (RipItem),
					typeof (IURLItem)
						};
				}
			}
			
			public void UpdateItems ()
			{
				MovierokDo.UpdateRips();
			}
			
			public Gtk.Bin GetConfiguration () {
				return new Configuration ();
			}
			
		}
		
	}
	
	
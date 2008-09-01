/*
 * RemoteAction.cs
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

using Do;
using Do.Universe;
using Mono.Unix;
using Do.Addins;



namespace Movierok
{		
	public sealed class RemoteAction : AbstractAction
	{
		
		public override string Name {
			get { return Catalog.GetString ("Show at "+ MovierokDo.remote); }
		}
		
		public override string Description {
			get { return Catalog.GetString ("Show selected rip at " + MovierokDo.remote); }
		}
		
		public override string Icon {
			get { return "movierok_logo_192x192_black.png@" + GetType ().Assembly.FullName; }
		}
		
		public override Type [] SupportedItemTypes {
            get {
                return new Type [] {
                    typeof (RipItem)
                };
            }
        }
		
		public override IItem[] Perform (IItem[] items, IItem[] modifierItems)
		{
				foreach (RipItem item in items ) {
					Util.Environment.Open (item.URL);
				}
			return null;
		}
	}
}

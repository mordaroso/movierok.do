/*
 * RipItem.cs
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
using System.Net;

using Do.Addins;
using Do.Universe;
using Mono.Unix;

namespace Movierok
{

	public class RipItem : IItem, IURLItem
	{
		protected string releaser;
		protected int id;
		protected List<string> parts;
		protected MovieItem movie;

		public RipItem ()
		{
		}

		public virtual string Name { 
			get { return movie.Name; } 
		}
		
		public virtual string Description { 
			get { return "Rip by " + releaser; } 
		}
		
		public virtual string Icon { 
			get { return movie.Cover; } 
		}
		
		public virtual string URL {
			get { return "http://movierok.org/rips/"+Id;}
		}

		public virtual int Id { 
			get { return id; } 
			set { id = value; }
		}
		
		public virtual string Releaser { 
			get { return releaser; } 
			set { releaser = value; }
		}
		
		public virtual MovieItem Movie { 
			get { return movie; } 
			set { movie = value; }
		}
		
		public virtual List<string> Parts{
			get { return parts; } 
			set { parts = value; }
		}

	}
}

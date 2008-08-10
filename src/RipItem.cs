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

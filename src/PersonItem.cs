using System;

using Do.Universe;
using Mono.Unix;

namespace Movierok
{

	public class PersonItem : IItem
	{
		protected string name;
		protected int omdb;

		public PersonItem ()
		{
		}

		public virtual string Name { 
			get { return name; } 
			set { name = value; }
		}
		
		public virtual string Description { 
			get { return "Person from a movie"; } 
		}
		
		public virtual string Icon { 
			get { return "emblem-people"; } 
		}

		public virtual int Omdb { 
			get { return omdb; } 
			set { omdb = value; }
		}
	}
}

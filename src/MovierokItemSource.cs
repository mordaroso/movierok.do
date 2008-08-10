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
			get { return "movierok_logo64.png@" + GetType ().Assembly.FullName; }
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
	
	
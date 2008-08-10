using System;
using System.Net;

using Do.Universe;
using Mono.Unix;

namespace Movierok
{

	public class MovieItem : IItem
	{
		protected string title, description, cover;
		protected int omdb, image;

		public MovieItem ()
		{
		}
		
		private static void DownloadImage(int image)
		{
			Uri imageUri = new Uri("http://www.omdb.org/image/default/"+image+".jpeg");
			string movierok_folder = MovierokDo.MovierokDirectory();
			string image_folder = movierok_folder+"/images/";
			string location = image_folder + image + ".jpeg";
			WebClient client = new WebClient ();
			// create folders if not exist
			if (!System.IO.Directory.Exists (movierok_folder))
				System.IO.Directory.CreateDirectory (movierok_folder);
			if (!System.IO.Directory.Exists (image_folder))
				System.IO.Directory.CreateDirectory (image_folder);
			// download image
			try {
				client.DownloadFile (imageUri.AbsoluteUri, location);
			} catch {
				Console.Error.WriteLine ("{0} does not exist!",location);
			}
		}

		public virtual string Name { 
			get { return title; } 
		}
		
		public virtual string Description { 
			get { return description; } 
			set { description = value; }
		}
		
		public virtual string Icon { 
			get { return cover; } 
		}
		
		public virtual int Omdb { 
			get { return omdb; } 
			set { omdb = value; }
		}
		
		public virtual int Image { 
			get { return image; } 
			set { 
				image = value;
				if (!System.IO.File.Exists (MovierokDo.MovierokDirectory()+"/images/" + image+ ".jpeg")){
					DownloadImage(image);
				}
				cover = MovierokDo.MovierokDirectory()+"/images/" + image + ".jpeg";
			}
		}
		
		public virtual string Title { 
			get { return title; } 
			set { title = value; }
		}
		
		public virtual string Cover { 
			get { return cover; } 
		}

	}
}

/*
 * MovierokItem.cs
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

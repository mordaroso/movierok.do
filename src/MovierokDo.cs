/*
 * MovierokDo.cs
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
using System.IO;
using System.Collections.Generic;
using Mono.Unix;
using System.Xml;
using System.Threading;

using Do.Universe;

namespace Movierok
{
	public class MovierokDo
	{
		private static List<IItem> movies;
		private static List<IItem> rips;
		public const string remote = "movierok.org";
		
		static MovierokDo ()
		{
			movies = new List<IItem> ();
			rips = new List<IItem> ();
		} 
		
		public static string MovierokDirectory(){
			string home =  Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string mr_directory = "~/.local/share/gnome-do/movierok/".Replace ("~", home);
			return mr_directory;
		}
		
		public static List<IItem> Movies {
			get { return movies; }
		}
		
		public static List<IItem> Rips {
			get { return rips; }
		}
		
		public static void UpdateRips ()
		{
			if (String.IsNullOrEmpty (Configuration.Username))
				return;
			movies = new List<IItem> ();
			rips = new List<IItem> ();
			try {
				XmlDocument xmlDoc = GetXML(Configuration.Username);
				// Rips
				XmlNode ripsNode = xmlDoc.SelectSingleNode("rips");
				XmlNodeList ripList = ripsNode.ChildNodes;
				foreach (XmlNode ripNode in ripList){
					
					//Parse Rip Node
					RipItem rip = new RipItem();					
					// Rip Id
					XmlNode idNode = ripNode.SelectSingleNode("id");
					rip.Id = int.Parse(idNode.FirstChild.Value);
					
					// Rip Omdb Id
					XmlNode omdbNode = ripNode.SelectSingleNode("omdb");
					if(omdbNode.HasChildNodes)					
						rip.Omdb = int.Parse(omdbNode.FirstChild.Value);
					
					// Rip Title
					XmlNode titleNode = ripNode.SelectSingleNode("title");
					if(titleNode.HasChildNodes)					
						rip.Title = titleNode.FirstChild.Value;
					
					// Rip Image
					XmlNode imageNode = ripNode.SelectSingleNode("image");
					if(imageNode.HasChildNodes)					
						rip.Image = int.Parse(imageNode.FirstChild.Value);
					
					// Rip Releaser
					XmlNode releaserNode = ripNode.SelectSingleNode("releaser");
					if(releaserNode.HasChildNodes)					
						rip.Releaser = releaserNode.FirstChild.Value;
					
					
					// Parts
					XmlNode partsNode = ripNode.SelectSingleNode("parts");
					XmlNodeList partList = partsNode.ChildNodes;
					List<string> parts = new List<string>();
					foreach(XmlNode partNode in partList){
						//Part MRChecksum
						parts.Add(partNode.FirstChild.Value);
					}
					rip.Parts = parts;
					rips.Add(rip);
				}
			} catch (Exception e) {
				// Something went horribly wrong, so we print the error message.
				Console.Error.WriteLine ("Could not read movierok rips {0}: {1}",
				                         Configuration.Username, e.Message);
			}
		}
		
		protected static XmlDocument GetXML(string uname){
			if (!System.IO.Directory.Exists (MovierokDirectory()))
				System.IO.Directory.CreateDirectory (MovierokDirectory());
			XmlDocument xmlDoc = new XmlDocument();
			try{
				// refresh loop in hours
				int hours = 4;
				long diff = (DateTime.Now.ToFileTime()-System.IO.File.GetCreationTime(MovierokDirectory() + uname + ".xml").ToFileTime())/10000000-(3600*hours);
				if(diff > 0)					
					RefreshXML(uname);
				xmlDoc.Load(MovierokDirectory() + uname + ".xml");
			} catch {
				RefreshXML(uname);
				xmlDoc.Load(MovierokDirectory() + uname + ".xml");
			}
			return xmlDoc;
		}
		
		protected static void RefreshXML(string uname){
			if(System.IO.File.Exists (MovierokDirectory() + uname + ".xml")){
				System.IO.File.Delete(MovierokDirectory() + uname + ".xml");
			}
			MovierokDo.GetRipListFromOrg(uname);		
		}
		
		protected static void GetRipListFromOrg(string uname){
			// get all.xml from movierok.org
			Uri xmlUri = new Uri("http://"+remote+"/users/"+uname+"/rips/all.xml");
			string location = MovierokDirectory() + uname + ".xml";
			WebClient client = new WebClient ();
			
			// create folder if not exist
			if (!System.IO.Directory.Exists (MovierokDirectory()))
				System.IO.Directory.CreateDirectory (MovierokDirectory());
			
			// download all.xml
			try {
				client.DownloadFile (xmlUri.AbsoluteUri, location);
			} catch (Exception e){
				Console.Error.WriteLine ("Error while fetching {0}: {1}",xmlUri.AbsoluteUri, e.Message);
			}
		}
	}
}

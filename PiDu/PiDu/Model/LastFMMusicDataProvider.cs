using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PiDu.Model
{
    public class LastFMMusicDataProvider : MusicDataProviderBase
    {
        private const string API_KEY = "e56789fb972650fe4559b29463f76ec7";
        private const string API_URL = "http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key={0}&artist={1}&album={2}";

        public LastFMMusicDataProvider(string cacheDirectory)
            : base(cacheDirectory)
        {

        }

        protected override async Task<bool> GetExternalAlbumDataInternal(IAlbum album)
        {
            if (album == null)
                throw new ArgumentNullException("album");
            if (String.IsNullOrEmpty(album.Title) || String.IsNullOrEmpty(album.Artist))
                return false;

            string albumTitle = album.Title;
            string albumArtist = album.Artist;

            if (!Directory.Exists(CacheDirectory))
                Directory.CreateDirectory(CacheDirectory);

            string imageFile = String.Format("{0}\\{1}_{2}.png", CacheDirectory, album.Artist, album.Title);
            try
            {
                XDocument xml = XDocument.Load(String.Format(API_URL, API_KEY, albumArtist, albumTitle));
                if (xml.Root.Attribute("status").Value.Equals("ok"))
                {
                    var albumNode = xml.Root.Element("album");

                    if (albumNode.Equals(default(XNode)))
                        return false;

                    var elements = albumNode.Elements("image");
                    var imageElement = elements
                        .SingleOrDefault(node => node.Attribute("size").Value.Equals("extralarge"));

                    string imageURI;
                    if (imageElement != null)
                    {
                        imageURI = imageElement.Value;

                        WebClient client = new WebClient();
                        await client.DownloadFileTaskAsync(new Uri(imageURI), imageFile);
                        album.ImageUri = imageFile;
                    }
                    else
                    {
                        string blah = "Image null";
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;

        }

        protected override async Task<bool> GetCachedAlbumDataInternal(IAlbum album)
        {
            if (album == null)
                throw new ArgumentNullException("album");
            if (String.IsNullOrEmpty(album.Title) || String.IsNullOrEmpty(album.Artist))
                return false;

            if (!Directory.Exists(CacheDirectory))
                return false;

            string imageFile = String.Format("{0}\\{1}_{2}.png", CacheDirectory, album.Artist, album.Title);

            if (File.Exists(imageFile))
                album.ImageUri=imageFile;

            return false;
        }

        
    }
}

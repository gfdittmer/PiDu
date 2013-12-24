using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public class FreeCoverMusicDataProvider : IMusicDataProvider
    {
        private const string API_URL = "http://www.freecovers.net/api/search/{0}/Music+CD";

        public void GetAlbumData(IAlbum album)
        {
            if (album == null)
                throw new ArgumentNullException("title");

            string albumTitle = album.Title;


        }
    }
}

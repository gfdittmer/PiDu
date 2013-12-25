using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lastfm;
using Lastfm.Services;
namespace PiDu.Model
{
    public class MusicDataProvider 
    {
        private Session _session;

        public MusicDataProvider()
        {
            string apiKey = "e56789fb972650fe4559b29463f76ec7";
            string secret = "5cbaccbf3e0d7774dbe04defa8e2125a";

            this._session = new Session(apiKey, secret);
        }

        public void GetExternalAlbumData(IAlbum album)
        {
            Lastfm.Services.Album fmalbum = new Lastfm.Services.Album("", "", this._session);



        }
    }
}

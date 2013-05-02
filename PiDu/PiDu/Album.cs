using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu
{
    public class Album
    {
        public ObservableCollection<Song> Songs { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public Lastfm.Services.Album ExternalMeta { get; set; }
    }
}

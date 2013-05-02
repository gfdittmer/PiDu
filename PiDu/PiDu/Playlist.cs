using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu
{
    public class Playlist
    {
        public ObservableCollection<Song> Songs { get; set; }

        public string Name { get; set; }
    }
}

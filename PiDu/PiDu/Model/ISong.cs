using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface ISong : INotifyPropertyChanged
    {
        IAlbum Album { get;}

        string Name { get; set; }
        int TrackNumber { get; set; }
        string FileLocation { get; set; }
    }
}

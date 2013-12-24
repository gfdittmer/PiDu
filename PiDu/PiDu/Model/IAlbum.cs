using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface IAlbum : INotifyPropertyChanged
    {
        IEnumerable<ISong> Songs { get; set; }

        string Artist { get; set; }
        string Title { get; set; }

        IDictionary<string, object> ExternalMetaData { get; set; }
    }
}

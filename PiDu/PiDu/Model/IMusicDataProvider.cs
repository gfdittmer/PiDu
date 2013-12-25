using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface IMusicDataProvider : INotifyPropertyChanged
    {
        string CacheDirectory { get; set; }

        Task<bool> GetExternalAlbumData(IAlbum album);
        Task<bool> GetCachedAlbumData(IAlbum album);
    }
}

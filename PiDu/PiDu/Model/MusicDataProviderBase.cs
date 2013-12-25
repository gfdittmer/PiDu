using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public abstract class MusicDataProviderBase : IMusicDataProvider
    {
        #region CacheDirectory
        private string _cacheDirectory;
        public string CacheDirectory
        {
            get
            {
                return _cacheDirectory;
            }
            set
            {
                _cacheDirectory = value;
                //TODO: move all data to new folder
                RaisePropertyChanged();
            }
        }
        #endregion

        public MusicDataProviderBase(string cacheDirectory)
        {
            if (String.IsNullOrEmpty(cacheDirectory))
                throw new ArgumentNullException("cacheDirectory");

            CacheDirectory = cacheDirectory;

        }

        public async Task<bool> GetExternalAlbumData(IAlbum album)
        {
            return await GetExternalAlbumDataInternal(album);
        }

        protected abstract Task<bool> GetExternalAlbumDataInternal(IAlbum album);

        public async Task<bool> GetCachedAlbumData(IAlbum album)
        {
            return await GetCachedAlbumDataInternal(album);
        }

        protected abstract Task<bool> GetCachedAlbumDataInternal(IAlbum album);

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

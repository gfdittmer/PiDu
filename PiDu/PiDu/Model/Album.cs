using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public class Album : IAlbum
    {
        #region Songs
        private IEnumerable<ISong> _songs;
        public IEnumerable<ISong> Songs
        {
            get
            {
                return _songs;
            }
            set
            {
                _songs = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Artist
        private string _artist;
        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                _artist = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Title
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ImageUrl
        private string _imageUri;
        public string ImageUri
        {
            get
            {
                return _imageUri;
            }
            set
            {
                _imageUri = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ExternalMetaData
        private IDictionary<string, object> _externalMetaData;
        public IDictionary<string, object> ExternalMetaData
        {
            get
            {
                return _externalMetaData;
            }
            set
            {
                _externalMetaData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public Lastfm.Services.Album ExternalMeta { get; set; }

        public Album()
        {
            Title = "Unknown";
            Artist = "Unknown";
        }

        #region INotifyPropertyChanged
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion


        public override string ToString()
        {
            return String.Format(@"""{0}"" - {1}", this.Title, this.Artist);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Album))
                return false;

            Album other = obj as Album;

            return this.Title.ToLower().Equals(other.Title.ToLower()) &&
                this.Artist.ToLower().Equals(other.Artist.ToLower());
        }



        
    }
}

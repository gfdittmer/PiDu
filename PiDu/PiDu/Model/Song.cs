using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public class Song : ISong
    {
        #region Album
        private IAlbum _album;
        public IAlbum Album
        {
            get
            {
                return _album;
            }
            set
            {
                _album = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Name
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TrackNumber
        private int _trackNumber;
        public int TrackNumber
        {
            get
            {
                return _trackNumber;
            }
            set
            {
                _trackNumber = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region FileLocation
        private string _fileLocation;
        public string FileLocation
        {
            get
            {
                return _fileLocation;
            }
            set
            {
                _fileLocation = value;
            }
        }
        #endregion

        public Song(IAlbum album)
        {
            if (album == null)
                throw new ArgumentNullException("album");

            Album = album;

            Name = "Unknown track";
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

    }
}

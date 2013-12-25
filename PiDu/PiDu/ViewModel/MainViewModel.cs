using PiDu.Model;
using PiDu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PiDu
{
    public class MainViewModel: IMainViewModel
    {
        private ILibrary _library;
        private static IPlayer _player;

        public MainViewModel()
        {
            _library = new Library();
            _library.LibraryUpdated += _library_LibraryUpdated;
            _library.Load();

            Playlist = new Model.Playlist();
            Playlist.NewList += Playlist_NewList;
            Playlist.CurrentSongChanged += Playlist_CurrentSongChanged;

            _player = new Player();
            _player.PlayFinished += _player_PlayFinished;
            _player.CurrentPlayPosition += _player_CurrentPlayPosition;


            InitializeCommands();
        }

        private void InitializeProperties()
        {
            TotalSeconds = 10;
            CurrentTime = 0;
            IsLooped = false;
        }

        private void InitializeCommands()
        {
            SelectAlbum = new AlbumSelectCommand();
            PlayAlbum = new DelegateCommand<IAlbum>(this.Play);
            PlaySong = new DelegateCommand<ISong>(this.Play);
            PlayPauseSong = new DelegateCommand(this.PlayPause);
            NextSong = new DelegateCommand(this.Next);
            PreviousSong = new DelegateCommand(this.Previous);
            ToggleLoop = new DelegateCommand(this.Loop);
            ShowCurrentPlaylist = new DelegateCommand(this.ShowPlaylist);
            PrepareSeek = new DelegateCommand<double>(this.TempUpdateSeekPos);
            Seek = new DelegateCommand<double>(this.SeekToPos);
        }

        

        #region Private methods
        private void Play(IAlbum album)
        {
            Playlist.PlaySongs(album.Songs.ToList());
            ShowPlaylist();
        }

        private void Play(ISong song)
        {
            Playlist.PlaySongs(song.Album.Songs.ToList(), song);
            ShowPlaylist();
        }

        private void StartPlay(ISong song)
        {
            if (_player.Play(song, IsLooped) > 0)
            {
                CurrentSong = song;
                this.RaisePropertyChanged("CurrentSong");

                TotalSeconds = _player.Length;
                this.RaisePropertyChanged("TotalSeconds");
            }
        }

        private void PlayPause()
        {
            if (_player.IsPlaying)
            {
                _player.Pause();
            }
            else
            {
                _player.Resume(IsLooped);
            }
        }

        private void Next()
        {
            if (Playlist.HasNext)
            {
                Playlist.Next();
                ShowPlaylist();
            }
        }

        private void Previous()
        {
            if (Playlist.HasPrevious)
            {
                Playlist.Previous();
                ShowPlaylist();
            }
        }

        private void Loop()
        {
            IsLooped = !IsLooped;

            _player.IsLooped = IsLooped;
            this.RaisePropertyChanged("IsLooped");
        }

        private void ShowPlaylist()
        {
            CurrentPlaylistShowing = true;
            this.RaisePropertyChanged("CurrentPlaylistShowing");
        }

        private string MillisecondsToTime(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).ToString(@"mm\:ss");
        }

        private void TempUpdateSeekPos(double pos)
        {
            IsDragging = true;
            UpdateSeekPos((int)pos);
        }

        private bool IsDragging = false;
        private void UpdateSlider(int pos)
        {
            if (!IsDragging)
            {
                CurrentTime = pos;

                this.RaisePropertyChanged("CurrentTime");
            }
        }

        private void UpdateSeekPos(int pos)
        {
            //Console.WriteLine("Updating " + pos);
            RemainingSeconds = "-" + MillisecondsToTime(_player.Length - pos);
            SecondsElapsed = MillisecondsToTime(pos);

            this.RaisePropertyChanged("RemainingSeconds");
            this.RaisePropertyChanged("SecondsElapsed");
        }

        private void SeekToPos(double pos)
        {
            if (_player.IsPlaying)
            {
                _player.Seek((uint)pos);
            }
            IsDragging = false;
        }

        #endregion

        #region Properties
        public bool IsLooped { get; set; }

        #region FilteredAlbums
        private ICollectionView _filteredAlbums;
        public ICollectionView FilteredAlbums
        {
            get
            {
                return _filteredAlbums;
            }
            private set
            {
                _filteredAlbums = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public IAlbum SelectedAlbum { get; private set; }
        public ISong CurrentSong { get; private set; }
        public IPlaylist Playlist { get; private set; }
        public string RemainingSeconds { get; private set; }
        public string SecondsElapsed { get; private set; }
        public int TotalSeconds { get; private set; }
        public int CurrentTime { get; private set; }
        public bool CurrentPlaylistShowing { get; set; }
        #endregion

        #region Player events
        void _player_CurrentPlayPosition(object sender, int e)
        {
            UpdateSeekPos(e);

            UpdateSlider(e);
        }
        void _player_PlayFinished(object sender, EventArgs e)
        {
            if (Playlist.HasNext)
            {
                Playlist.Next();
            }
        }
        #endregion

        #region Playlist events
        void Playlist_CurrentSongChanged(object sender, EventArgs e)
        {
            this.StartPlay(Playlist.Current);
        }

        void Playlist_NewList(object sender, EventArgs e)
        {
            this.StartPlay(Playlist.Current);
        }

        void FilteredAlbums_CurrentChanged(object sender, EventArgs e)
        {
            SelectedAlbum = FilteredAlbums.CurrentItem as Album;
            if (SelectedAlbum != null)
            {
                Console.WriteLine("Selected: " + SelectedAlbum.Title);
            }
            this.RaisePropertyChanged("SelectedAlbum");
            CurrentPlaylistShowing = false;
            this.RaisePropertyChanged("CurrentPlaylistShowing");

        }

        void _library_LibraryUpdated(object sender, EventArgs e)
        {
            ILibrary library = sender as ILibrary;
            FilteredAlbums = new CollectionView(library.Albums);
            FilteredAlbums.CurrentChanged += FilteredAlbums_CurrentChanged;
        }
        #endregion

        #region Commands
        public ICommand SelectAlbum { get; set; }
        public ICommand PlayAlbum { get; set; }
        public ICommand PlaySong { get; set; }
        public ICommand PlayPauseSong { get; set; }
        public ICommand ToggleLoop { get; set; }
        public ICommand ShowCurrentPlaylist { get; set; }
        public ICommand NextSong { get; set; }
        public ICommand PreviousSong { get; set; }
        public ICommand PrepareSeek { get; set; }
        public ICommand Seek { get; set; }

        public ICommand SortByArtist { get; set; }
        public ICommand SortByAlbum { get; set; }
        public ICommand SortBySong { get; set; }
        public ICommand SoryByDate { get; set; }
        public ICommand SortByGenre { get; set; }
        public ICommand SoryByPlaylist { get; set; }
        #endregion

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



    public class AlbumSelectCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Console.WriteLine(((IAlbum)parameter).Title.ToString());
        }
    }

    public class DelegateCommand: ICommand
    {
        private readonly Action _execute;

        public DelegateCommand(Action execute)
        {
            this._execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._execute();
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public DelegateCommand(Action<T> execute)
        {
            this._execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._execute((T)parameter);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PiDu
{
    public class ViewModel:INotifyPropertyChanged
    {
        private Library _library;
        private static Player _player;
        private Dispatcher _dispatcher;

        private ObservableCollection<Album> _albums;

        public ViewModel(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;

            _albums = new ObservableCollection<Album>();
            
            FilteredAlbums = new ListCollectionView(_albums);
            FilteredAlbums.CurrentChanged += FilteredAlbums_CurrentChanged;

            _library = new Library();
            _library.LibraryUpdated += _library_LibraryUpdated;
            Task.Run(()=> _library.Load());

            _player = new Player();

            
            SelectAlbum = new AlbumSelectCommand();
            PlaySong = new DelegateCommand<Song>(this.Play);
            PlayPauseSong = new DelegateCommand(this.PlayPause);
        }

        void FilteredAlbums_CurrentChanged(object sender, EventArgs e)
        {
            CurrentAlbum = FilteredAlbums.CurrentItem as Album;
            if (CurrentAlbum != null)
            {
                Console.WriteLine("Selected: " + CurrentAlbum.Title);
            }
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentAlbum"));
            }
        }

        void _library_LibraryUpdated(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync(new Action(()=>
            {
                foreach (Album album in _library.Albums.OrderBy(x=>x.Title))
                {
                    if (!_albums.Contains(album))
                    {
                        _albums.Add(album);
                    }
                }
            }));
        }


        private void Play(Song song)
        {
            if(_player.Play(song, false) > 0){
                CurrentSong = song;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentSong"));
                }
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
                _player.Resume(false);
            }
        }

        public ICollectionView FilteredAlbums { get; private set; }
        public Album CurrentAlbum { get; private set; }
        public Song CurrentSong { get; private set; }

        public ICommand SelectAlbum { get; set; }
        public ICommand PlaySong { get; set; }
        public ICommand PlayPauseSong { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
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
            Console.WriteLine(((Album)parameter).Title.ToString());
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

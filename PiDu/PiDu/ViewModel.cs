﻿using System;
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

        public ViewModel()
        {
            _albums = new ObservableCollection<Album>();
            
            FilteredAlbums = new ListCollectionView(_albums);
            FilteredAlbums.CurrentChanged += FilteredAlbums_CurrentChanged;

            _library = new Library();
            _library.LibraryUpdated += _library_LibraryUpdated;
            Task.Run(()=> _library.Load());

            Playlist = new PiDu.Playlist();
            Playlist.NewList += Playlist_NewList;
            Playlist.CurrentSongChanged += Playlist_CurrentSongChanged;

            _player = new Player();
            _player.PlayFinished += _player_PlayFinished;

            
            SelectAlbum = new AlbumSelectCommand();
            PlayAlbum = new DelegateCommand<Album>(this.Play);
            PlaySong = new DelegateCommand<Song>(this.Play);
            PlayPauseSong = new DelegateCommand(this.PlayPause);
            ToggleLoop = new DelegateCommand(this.Loop);

            IsLooped = false;
        }



        void Playlist_CurrentSongChanged(object sender, EventArgs e)
        {
            this.StartPlay(Playlist.Current);
        }

        void _player_PlayFinished(object sender, EventArgs e)
        {
            if (Playlist.HasNext)
            {
                Playlist.Next();
            }
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

        private void Play(Album album)
        {
            Playlist.PlaySongs(album.Songs.ToList());
        }

        private void Play(Song song)
        {
            Playlist.PlaySongs(song.Album.Songs.ToList());
        }

        private void StartPlay(Song song)
        {
            if (_player.Play(song, IsLooped) > 0)
            {
                this.RaisePropertyChanged("CurrentSong");
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

        

        private void Loop()
        {
            IsLooped = !IsLooped;

            this.RaisePropertyChanged("IsLooped");
        }

        public bool IsLooped { get; set; }

        public ICollectionView FilteredAlbums { get; private set; }
        public Album SelectedAlbum { get; private set; }

        public Song CurrentSong { get; private set; }

        public Playlist Playlist { get; private set; }

        public ICommand SelectAlbum { get; set; }
        public ICommand PlayAlbum { get; set; }
        public ICommand PlaySong { get; set; }
        public ICommand PlayPauseSong { get; set; }
        public ICommand ToggleLoop { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
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

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
        private Player _player;
        private Dispatcher _dispatcher;

        private ObservableCollection<Album> _albums;

        public ViewModel(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;

            _albums = new ObservableCollection<Album>();
            
            FilteredAlbums = new ListCollectionView(_albums);

            _library = new Library();
            _library.LibraryUpdated += _library_LibraryUpdated;
            Task.Run(()=> _library.Load());

            _player = new Player();

            
            SelectAlbum = new AlbumSelectCommand();
        }

        void _library_LibraryUpdated(object sender, EventArgs e)
        {
            Console.WriteLine("Library updated. Updating viewmodel data.");

            Application.Current.Dispatcher.InvokeAsync(new Action(()=>
            {
                foreach (Album album in _library.Albums.OrderBy(x=>x.Title))
                {
                    if (!_albums.Contains(album))
                    {
                        _albums.Add(album);
                    }
                }
                Console.WriteLine("Updated viewmodel data.");
            }));
        }



        public ICollectionView FilteredAlbums { get; private set; }

        public ICommand SelectAlbum { get; set; }

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
}

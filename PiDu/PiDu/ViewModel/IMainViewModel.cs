using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.ViewModel
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        bool IsLooped { get; set; }
        System.ComponentModel.ICollectionView FilteredAlbums { get; }
        Model.IAlbum SelectedAlbum { get; }
        Model.ISong CurrentSong { get; }
        Model.IPlaylist Playlist { get; }
        string RemainingSeconds { get; }
        string SecondsElapsed { get; }
        int TotalSeconds { get; }
        int CurrentTime { get; }
        bool CurrentPlaylistShowing { get; set; }


        System.Windows.Input.ICommand SelectAlbum { get; set; }
        System.Windows.Input.ICommand PlayAlbum { get; set; }
        System.Windows.Input.ICommand PlaySong { get; set; }
        System.Windows.Input.ICommand PlayPauseSong { get; set; }
        System.Windows.Input.ICommand ToggleLoop { get; set; }
        System.Windows.Input.ICommand ShowCurrentPlaylist { get; set; }
        System.Windows.Input.ICommand NextSong { get; set; }
        System.Windows.Input.ICommand PreviousSong { get; set; }
        System.Windows.Input.ICommand PrepareSeek { get; set; }
        System.Windows.Input.ICommand Seek { get; set; }
        System.Windows.Input.ICommand SortByArtist { get; set; }
        System.Windows.Input.ICommand SortByAlbum { get; set; }
        System.Windows.Input.ICommand SortBySong { get; set; }
        System.Windows.Input.ICommand SoryByDate { get; set; }
        System.Windows.Input.ICommand SortByGenre { get; set; }
        System.Windows.Input.ICommand SoryByPlaylist { get; set; }
    }
}

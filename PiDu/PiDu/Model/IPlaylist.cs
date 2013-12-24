using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface IPlaylist
    {
        void PlaySongs(IEnumerable<ISong> songs);
        void PlaySongs(IEnumerable<ISong> songs, ISong currentSong);
        
        IEnumerable<ISong> Songs{get;}

        ISong Current { get; set; }
        void Previous();
        void Next();
        void ToggleShuffle();

        bool HasPrevious { get; }
        bool HasNext { get; }
        bool Shuffled { get; }

        string Name { get; set; }


        event EventHandler NewList;
        event EventHandler CurrentSongChanged;
    }
}

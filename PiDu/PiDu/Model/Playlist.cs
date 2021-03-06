﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public class Playlist : IPlaylist
    {
        public void PlaySongs(IEnumerable<ISong> songs)
        {
            if (songs == null)
            {
                throw new ArgumentNullException("Songs cannot be null");
            }

            index = 0;
            Songs = songs;

            if (this.NewList != null)
            {
                this.NewList(this, new EventArgs());
            }
        }

        public void PlaySongs(IEnumerable<ISong> songs, ISong currentSong)
        {
            if (songs == null)
            {
                throw new ArgumentNullException("Songs cannot be null");
            }

            index = 0;
            Songs = songs;

            Current = currentSong;

            if (this.NewList != null)
            {
                this.NewList(this, new EventArgs());
            }
        }

        public IEnumerable<ISong> Songs { get; private set; }

        private int index = 0;
        public ISong Current
        {
            get
            {
                if(Songs.Count() >0 )
                {
                    return Songs.ElementAt(index);
                }

                return null;
            }

            set
            {
                if (!Songs.Contains(value))
                {
                    throw new ArgumentOutOfRangeException("Not a valid song");
                }
                else
                {
                    index = Songs.ToList().IndexOf(value);
                }
            }
        }
        public void Previous()
        {
            index--;
            if (index < 0) { index = 0; }
            else
            {
                if (this.CurrentSongChanged != null)
                {
                    this.CurrentSongChanged(this, new EventArgs());
                }
            }
        }

        public bool HasPrevious
        {
            get
            {
                return index > 0;
            }
        }

        public bool HasNext
        {
            get
            {
                return index <= (Songs.Count() - 1);
            }
        }

        public void Next()
        {
            index++;
            if (index >= Songs.Count()) { index = Songs.Count() - 1; }
            else
            {
                if (this.CurrentSongChanged != null)
                {
                    this.CurrentSongChanged(this, new EventArgs());
                }
            }
        }

        public bool Shuffled { get; private set; }

        public void ToggleShuffle()
        {
            Shuffled = !Shuffled;

            if (Shuffled)
            {
                //TODO: randomize the list
            }
            else
            {
                Sort();
            }
        }

        private void Sort()
        {

        }

        public string Name { get; set; }


        public event EventHandler NewList;
        public event EventHandler CurrentSongChanged;
    }
}

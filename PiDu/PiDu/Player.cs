using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IrrKlang;
using System.Timers;

namespace PiDu
{
    public class Player
    {
        private Timer _playUpdater;

        private ISoundEngine _engine;
        private ISound _currentlyPlayingSound;

        public Player()
        {
            this._engine = new ISoundEngine();

            this._playUpdater = new Timer(100);
            this._playUpdater.Elapsed += _playUpdater_Elapsed;
        }

        public uint Play(Song song, bool loop)
        {
            try
            {
                this.Stop();
                
                this._currentlyPlayingSound = this._engine.Play2D(song.FileLocation, loop);

                System.Console.WriteLine("Playing: " + song.FileLocation);

                if (this._currentlyPlayingSound == null)
                {
                    throw new ArgumentException("Unable to play song");
                }

                this._playUpdater.Start();

                return this._currentlyPlayingSound.PlayLength;
            }
            catch (Exception e)
            {
                //TODO: do something with this
                System.Console.WriteLine(e.ToString());
                return 0;
            }
        }

        void _playUpdater_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.CurrentPlayPosition != null
                && this._currentlyPlayingSound != null)
            {
                this.CurrentPlayPosition(this, (int)this._currentlyPlayingSound.PlayPosition);
            }

            if (this._currentlyPlayingSound != null && this._currentlyPlayingSound.Finished && 
                this.PlayFinished != null)
            {
                this.PlayFinished(this, new EventArgs());
                this._playUpdater.Stop();
            }
        }

        public void Seek(uint pos)
        {
            if (HasSoundFile)
            {
                this._currentlyPlayingSound.PlayPosition = pos;
            }
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                this._currentlyPlayingSound.Paused = true;
            }
        }

        public void Resume(bool loop)
        {
            if (this._currentlyPlayingSound != null)
            {
                this._currentlyPlayingSound.Paused = false;
            }
        }

        public bool HasSoundFile
        {
            get
            {
                return this._currentlyPlayingSound != null && !this._currentlyPlayingSound.Finished;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return HasSoundFile && !this._currentlyPlayingSound.Paused;
            }
        }

        public void Stop()
        {
            if (HasSoundFile)
            {
                this._currentlyPlayingSound.Stop();
                this._playUpdater.Stop();
            }
        }

        public int Length
        {
            get
            {
                return (int)this._currentlyPlayingSound.PlayLength;
            }
        }

        public event EventHandler<int> CurrentPlayPosition;
        public event EventHandler PlayFinished;
    }
}

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

        public uint Play(Song song)
        {
            try
            {
                this._currentlyPlayingSound = this._engine.Play2D(song.FileLocation);

                this._playUpdater.Start();

                return this._currentlyPlayingSound.PlayLength;
            }
            catch (Exception)
            {
                //TODO: do something with this
            }
            return 0;
        }

        void _playUpdater_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.CurrentPlayPosition != null
                && this._currentlyPlayingSound != null)
            {
                this.CurrentPlayPosition(this, (int)this._currentlyPlayingSound.PlayPosition);
            }
        }

        public void PanTo(uint pos)
        {
            if (this._currentlyPlayingSound != null
                && !this._currentlyPlayingSound.Finished)
            {
                this._currentlyPlayingSound.PlayPosition = pos;
            }
        }

        public void Stop()
        {
            if (this._currentlyPlayingSound != null
                && this._currentlyPlayingSound.Finished)
            {
                this._currentlyPlayingSound.Stop();
                this._playUpdater.Stop();
            }
        }

        public event EventHandler<int> CurrentPlayPosition;
    }
}

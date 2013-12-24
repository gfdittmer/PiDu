using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface IPlayer
    {
        uint Play(ISong song, bool loop);
        void Seek(uint pos);
        void Pause();
        void Resume(bool loop);
        bool HasSoundFile {get;}
        bool IsPlaying { get;}
        bool IsLooped { get; set; }
        void Stop();
        int Length { get; }

        event EventHandler<int> CurrentPlayPosition;
        event EventHandler PlayFinished;
    }
}

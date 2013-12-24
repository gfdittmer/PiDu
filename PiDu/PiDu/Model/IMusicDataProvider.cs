using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface IMusicDataProvider
    {
        void GetAlbumData(IAlbum album);
    }
}

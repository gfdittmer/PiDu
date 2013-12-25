using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiDu.Model
{
    public interface ILibrary
    {
        ICollection<IAlbum> Albums { get; set; }
        ICollection<ISong> Songs { get; set; }

        Task Load();
        Task Load(string startingDirectory);

        event EventHandler LibraryUpdated;
    }
}

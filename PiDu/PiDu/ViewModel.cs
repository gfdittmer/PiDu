using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiDu
{
    public class ViewModel
    {
        public ViewModel()
        {
            Library = new Library();
            Player = new Player();
            Select = new AlbumSelectCommand();
        }

        public Library Library { get; set; }
        public Player Player { get; set; }

        public ICommand Select { get; set; }
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

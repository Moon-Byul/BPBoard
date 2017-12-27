using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Newtonsoft.Json.Linq;

namespace BPBoard
{
    public class BoardList : Notifier
    {
        private ObservableCollection<Board> boards = new ObservableCollection<Board>();

        public ObservableCollection<Board> Boards
        {
            get{ return boards; }
            set
            {
                boards = value;
                OnPropertyChanged("Boards");
            }
        }

        public void AddItem(Board board)
        {
            boards.Add(board);
            OnPropertyChanged("Boards");
        }

        public void Clear()
        {
            boards.Clear();
        }
    }
}

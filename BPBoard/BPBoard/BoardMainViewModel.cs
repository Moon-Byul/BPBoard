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
    class BoardMainViewModel : Notifier
    {
        private BoardList list;
        private PageArray pageArray = new PageArray();

        public ICommand ListClickCommand { get; set; }
        public ICommand TestButtonClickCommand { get; set; }

        public ICommand FirstCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand LastCommand { get; set; }

        public PageClickCommand pageClickCommand { get; protected set; }

        public bool IsAvailableNext
        {
            get
            {
                Console.WriteLine("Next : " + (pageArray.MaxPage > pageArray.CurrentPage));
                return pageArray.MaxPage > pageArray.CurrentPage;
            }
        }
        public bool IsAvailablePrevious
        {
            get
            {
                Console.WriteLine("Prev : " + (pageArray.CurrentPage > 0));
                return pageArray.CurrentPage > 1;
            }
        }

        private int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = -1;
                if(value >= 0)
                {
                    Console.WriteLine(value);
                }
                OnPropertyChanged("SelectedIndex");
            }
        }

        private Visibility isLoadingVisibility = Visibility.Collapsed;
        public Visibility IsLoadingVisibility
        {
            get { return isLoadingVisibility; }
            set
            {
                isLoadingVisibility = value;
                OnPropertyChanged("IsLoadingVisibility");
            }
        }

        public BoardMainViewModel()
        {
            TestButtonClickCommand = new RelayCommand(o => ButtonClickEvent("Test"));
            ListClickCommand = new RelayCommand(o => ButtonClickEvent("Show"));
            FirstCommand = new RelayCommand(o => ButtonClickEvent("First"));
            NextCommand = new RelayCommand(o => ButtonClickEvent("Next"));
            PreviousCommand = new RelayCommand(o => ButtonClickEvent("Previous"));
            LastCommand = new RelayCommand(o => ButtonClickEvent("Last"));
            pageClickCommand = new PageClickCommand(this);

            list = new BoardList();

            LoadingData();
        }

        public BoardList List
        {
            get{ return list; }
            set
            {
                list = value;
                OnPropertyChanged("List");
            }
        }

        public PageArray PageArray
        {
            get { return pageArray; }
            set
            {
                pageArray = value;
                OnPropertyChanged("PageArray");
            }
        }

        private void ButtonClickEvent(object sender)
        {
            String command = sender.ToString();

            if (command.Equals("Test"))
            {
                Board board = new Board
                {
                    PostNum = 0,
                    Title = "Test",
                    Content = "Test",
                    Author = "Test",
                    Email = "Test2",
                    Date = new DateTime()
                };
                list.AddItem(board);
            }
            else if (command.Equals("Show"))
            {
                if (SelectedIndex >= 0)
                    Console.WriteLine(SelectedIndex);
                SelectedIndex = -1;
                /*
                Application currentApplication = Application.Current;

                MainWindow window = (BPBoard.MainWindow)App.Current.MainWindow;
                window.MainFrame.Navigate(new Uri("BoardView.xaml", UriKind.Relative));
                */
            }
            else if (command.Equals("Next"))
            {
                NextPage();
            }
            else if (command.Equals("Previous"))
            {
                PrevPage();
            }
        }

        private async void LoadingData()
        {
            IsLoadingVisibility = Visibility.Visible;

            pageArray.RefreshPageList();

            JArray listData = await Task<JArray>.Run(() => getListData(pageArray.StartPage));

            AddItemToJson(listData);

            IsLoadingVisibility = Visibility.Collapsed;
        }

        private JArray getListData(int pageNum)
        {
            string jsonText = SendWeb.GetJsonToWeb("http://" + Application.Current.Resources["ServerIP"] + "/asp/board_pageList.asp", "pageNum= + " + pageNum  + "& perPage=10");

            return JArray.Parse(jsonText);
        }

        public class PageClickCommand : ICommand
        {
            private BoardMainViewModel viewModel;
            public event EventHandler CanExecuteChanged;

            public PageClickCommand(BoardMainViewModel model)
            {
                viewModel = model;
            }

            public bool CanExecute(object paramter)
            {
                return true;
            }

            public void Execute(object Parameter)
            {
                Console.WriteLine(Parameter);
            }
        }

        public async void PrevPage()
        {
            IsLoadingVisibility = Visibility.Visible;

            pageArray.CurrentPage -= 1;
            pageArray.RefreshPageList();

            JArray listData = await Task<JArray>.Run(() => getListData(pageArray.CurrentPage));

            AddItemToJson(listData);

            OnPropertyChanged("IsAvailableNext");
            OnPropertyChanged("IsAvailablePrevious");

            IsLoadingVisibility = Visibility.Collapsed;
        }

        public async void NextPage()
        {
            IsLoadingVisibility = Visibility.Visible;

            pageArray.CurrentPage += 1;
            pageArray.RefreshPageList();

            JArray listData = await Task<JArray>.Run(() => getListData(pageArray.CurrentPage));

            AddItemToJson(listData);

            OnPropertyChanged("IsAvailableNext");
            OnPropertyChanged("IsAvailablePrevious");

            IsLoadingVisibility = Visibility.Collapsed;
        }

        private void AddItemToJson(JArray listData)
        {
            list.Clear();

            foreach (JObject item in listData)
            {
                Board board = new Board
                {
                    PostNum = Convert.ToInt32(item.GetValue("postNum").ToString()),
                    Title = item.GetValue("Title").ToString(),
                    Content = item.GetValue("Contents").ToString(),
                    Author = item.GetValue("Author").ToString(),
                    Email = item.GetValue("Email").ToString(),
                    Date = Convert.ToDateTime(item.GetValue("Date").ToString())
                };
                list.AddItem(board);
            }
        }
    }
}

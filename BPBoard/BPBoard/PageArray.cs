using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace BPBoard
{
    class PageArray : Notifier
    {
        private int maxPage;
        public int MaxPage
        {
            get { return maxPage; }
        }

        private int startPage = 1;
        public int StartPage
        {
            get { return startPage; }
            set
            {
                startPage = value;
                OnPropertyChanged("StartPage");
            }
        }

        private int currentPage = 1;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private static int pageNum = 10; // 최대 페이지
        public int PageNum
        {
            get { return pageNum; }
        }

        PageItem[] pageList = new PageItem[pageNum];
        public PageItem[] PageList
        {
            get { return pageList; }
            set
            {
                pageList = value;
            }
        }

        public PageItem[] RefreshPageList()
        {
            Task task = new Task(() => LoadMaxPage());
            task.Start();
            task.Wait();

            Console.WriteLine(maxPage);

            for (int i = 0; i < pageNum; i++)
            {
                if(pageList[i] == null)
                {
                    pageList[i] = new PageItem()
                    {
                        Num = Convert.ToString(startPage + i),
                        Visible = (startPage + i <= maxPage ? Visibility.Visible : Visibility.Collapsed),
                        Bold = (startPage == startPage + i ? FontWeights.Bold : FontWeights.Normal)
                    };
                }
                else
                {
                    pageList[i].Num = Convert.ToString(startPage + i);
                    pageList[i].Visible = (startPage + i <= maxPage ? Visibility.Visible : Visibility.Collapsed);
                    pageList[i].Bold = (currentPage == startPage + i ? FontWeights.Bold : FontWeights.Normal);
                }
            }
            return pageList;
        }

        public void NextPage()
        {
            Task task = new Task(() => LoadMaxPage());
            task.Start();
            task.Wait();
        }

        public void PrevPage()
        {
            Task task = new Task(() => LoadMaxPage());
            task.Start();
            task.Wait();
        }

        public void LoadMaxPage()
        {
            string jsonText = SendWeb.GetJsonToWeb("http://" + Application.Current.Resources["ServerIP"] + "/asp/board_getRows.asp", "");

            JArray json = JArray.Parse(jsonText);

            maxPage = (Convert.ToInt32(json[0]["count"]) / pageNum) + 1;
        }
    }
}

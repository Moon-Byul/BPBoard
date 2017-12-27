using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BPBoard
{
    class PageItem : Notifier
    {
        private String num;
        public String Num
        {
            get { return num; }
            set
            {
                num = value;
                OnPropertyChanged("Num");
            }
        }

        private Visibility visible;
        public Visibility Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged("Visible");
            }
        }

        private FontWeight bold;
        public FontWeight Bold
        {
            get { return bold; }
            set
            {
                bold = value;
                OnPropertyChanged("Bold");
            }
        }
    }
}

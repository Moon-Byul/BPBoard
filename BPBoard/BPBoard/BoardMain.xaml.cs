using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BPBoard
{
    /// <summary>
    /// BoardMain.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BoardMain : Page
    {
        public BoardMain()
        {
            InitializeComponent();

            for (int i=0; i<10; i++)
            {
                Button tbTemp = new Button();
                tbTemp.Margin = new Thickness(10, 0, 10, 0);
                tbTemp.Background = Brushes.Transparent;
                tbTemp.BorderBrush = Brushes.Transparent;
                tbTemp.Style = Application.Current.FindResource("TransparentBtn") as Style;

                pageStackPanel.Children.Add(tbTemp);
                
                BindingOperations.SetBinding(tbTemp, Button.ContentProperty, new Binding("PageArray.PageList[" + i + "].Num"));
                BindingOperations.SetBinding(tbTemp, VisibilityProperty, new Binding("PageArray.PageList[" + i + "].Visible"));
                BindingOperations.SetBinding(tbTemp, Button.FontWeightProperty, new Binding("PageArray.PageList[" + i + "].Bold"));

                BindingOperations.SetBinding(tbTemp, Button.CommandProperty, new Binding("pageClickCommand"));
                BindingOperations.SetBinding(tbTemp, Button.CommandParameterProperty, new Binding("PageArray.PageList[" + i + "].Num"));
            }
        }
    }
}

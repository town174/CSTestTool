using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfStyles.Controls
{
    /// <summary>
    /// CardControl.xaml 的交互逻辑
    /// </summary>
    public partial class CardControl : UserControl, INotifyPropertyChanged
    {
        public CardControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _title = "一张小卡片";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                //激发事件
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        private Viewbox _ctsp = null;
        public Viewbox ContentPanel
        {
            get
            {
                return _ctsp;
            }
            set
            {
                _ctsp = value;
                Grid.SetRow(_ctsp, 2);
                bsGrid.Children.Add(_ctsp);
                //bsPanel.Children.Add(_ctsp);
            }
        }
    }
}

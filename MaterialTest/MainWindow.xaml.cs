using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace MaterialTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitListView();

        }

        DataTable dt = new DataTable();
        void InitListView()
        {
            var tecs2 = new ObservableCollection<Teacher>() {
                new Teacher(){ Name = "abby", Age = "20", Class="5"},
                new Teacher(){ Name = "john", Age = "35", Class="10"},
                new Teacher(){ Name = "lucy", Age = "40", Class="8"}
            };

            //绑定DataTable
            dt.Columns.Add("Age", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Class", typeof(string));
            foreach (var item in tecs2)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Age;
                dr[1] = item.Name;
                dr[2] = item.Class;
                dt.Rows.Add(dr);
            }
            //lsView3.DataContext = dt;
            //lsView3.SetBinding(ListView.ItemsSourceProperty, new Binding());
            //lsView3.Items.SortDescriptions.Add(new SortDescription("Class", ListSortDirection.Ascending));
        }
    }

    public class Teacher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //激发事件
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        private string _age;
        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                //激发事件
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }
        private string _class;
        public string Class
        {
            get { return _class; }
            set
            {
                _class = value;
                //激发事件
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Class"));
                }
            }
        }

        public override string ToString()
        {
            return $"name:{_name},age:{_age},class:{_class}";
        }
    }
}

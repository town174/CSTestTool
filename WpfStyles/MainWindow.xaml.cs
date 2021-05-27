using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Timers;

namespace WpfStyles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Member> memberData = new ObservableCollection<Member>();
        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
            memberData.Add(new Member()
            {
                Name = "Joe",
                Age = "23",
                Sex = SexOpt.Male,
                Pass = true,
                Email = new Uri("mailto:Joe@school.com")
            });
            memberData.Add(new Member()
            {
                Name = "Mike",
                Age = "20",
                Sex = SexOpt.Male,
                Pass = false,
                Email = new Uri("mailto:Mike@school.com")
            });
            memberData.Add(new Member()
            {
                Name = "Lucy",
                Age = "25",
                Sex = SexOpt.Female,
                Pass = true,
                Email = new Uri("mailto:Lucy@school.com")
            });
            dg1.DataContext = memberData;
        }

        void InitTimer()
        {
            Timer t = new Timer(3000);
            t.Elapsed += (a, b) => {
                //memberData[DateTime.Now.Second % 3].Count++;
                this.Dispatcher.Invoke(() => {
                    memberData.Add(new Member()
                    {
                        Name = "Town",
                        Age = "28",
                        Sex = SexOpt.Male,
                        Pass = true,
                        Email = new Uri("mailto:Town@school.com")
                    });
                });
            };
            //t.Start();
        }
    }

    public enum SexOpt { Male, Female };

    public class Member
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public SexOpt Sex { get; set; }
        public bool Pass { get; set; }
        public Uri Email { get; set; }
        public int Count { get; set; }
    }
}

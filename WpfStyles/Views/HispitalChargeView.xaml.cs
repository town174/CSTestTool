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
using System.Windows.Shapes;

namespace WpfStyles.Views
{
    /// <summary>
    /// HispitalChargeView.xaml 的交互逻辑
    /// </summary>
    public partial class HispitalChargeView : Window
    {
        public HispitalChargeView()
        {
            InitializeComponent();
            InitCharts();
            DataContext = this;
        }

        ObservableCollection<Charge> BloodRoomCharges = null;
        ObservableCollection<Charge> CheckCharges = null;
        ObservableCollection<Charge> FeatureCharges = null;
        ObservableCollection<Charge> PathologyCharges = null;
        ObservableCollection<Charge> NrmCharges = null;
        ObservableCollection<Charge> RadiateCharges = null;
        ObservableCollection<Charge> GastroscopeCharges = null;
        private void InitCharts()
        {
            //init blood
            BloodRoomCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 8213, LastMonth = 4866, LinkRelative = 66.71, LastYear = 3345, YOY = 145.53 },
                new Charge(){ Project = "检查收入",CurrentMonth = 7665, LastMonth = 0, LinkRelative = 100.00, LastYear = 3345, YOY = 129.18 },
                new Charge(){ Project = "手术收入",CurrentMonth = 435,  LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00},
                new Charge(){ Project = "治疗收入",CurrentMonth = 112,  LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00}
            };
            //datagrid
            //listview
            //BloodDg.ItemsSource = BloodRoomCharges;
            BloodDg.DataContext = BloodRoomCharges;

            //init check
            CheckCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 0, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 }
            };
            CheckDg.DataContext = CheckCharges;

            //init feature
            FeatureCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 7365, LastMonth = 7365, LinkRelative = 0, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "检查收入",CurrentMonth = 6223, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "体检收入",CurrentMonth = 1142, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00},
            };
            FeatureDg.DataContext = FeatureCharges;

            //init pathology
            PathologyCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 1465, LastMonth = 1465, LinkRelative = 0, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "检查收入",CurrentMonth = 790, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "体检收入",CurrentMonth = 675, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00},
            };
            PathologyDg.DataContext = PathologyCharges;

            //init nrm
            NrmCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 0, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 }
            };
            NrmDg.DataContext = NrmCharges;

            //init radiate
            RadiateCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 8357, LastMonth = 8357, LinkRelative = 0, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "DR收入",  CurrentMonth = 7767, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "检查收入",CurrentMonth = 345, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "体检收入",CurrentMonth = 245, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00},
            };
            RadiateDg.DataContext = RadiateCharges;

            //init gastrose
            GastroscopeCharges = new ObservableCollection<Charge>() {
                new Charge(){ Project = "总收入",  CurrentMonth = 6145, LastMonth = 6145, LinkRelative = 0, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "检查收入",CurrentMonth = 5789, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00 },
                new Charge(){ Project = "体检收入",CurrentMonth = 355, LastMonth = 0, LinkRelative = 100.00, LastYear = 0, YOY = 100.00},
            };
            GastroscopeDg.DataContext = GastroscopeCharges;
        }
    }

    public class Charge
    {
        public string Project { get; set; }
        public int CurrentMonth { get; set; }
        public int LastMonth { get; set; }
        public int LastYear { get; set; }
        public double LinkRelative { get; set; }
        public double YOY { get; set; }
    }
}

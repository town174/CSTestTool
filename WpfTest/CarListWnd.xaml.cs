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
using System.Windows.Shapes;
using WpfTest.Entity;

namespace WpfTest
{
    /// <summary>
    /// CarListWnd.xaml 的交互逻辑
    /// </summary>
    public partial class CarListWnd : Window
    {
        public CarListWnd()
        {
            InitializeComponent();
            InitCarList();
        }

        void InitCarList()
        {
            List<Car> carList = new List<Car>()
            {
                new Car(){ Automaker = "Audi",Name = "A4",Year = "2021",TopSpeed = "220"},
                new Car() { Automaker = "Audi", Name = "Q5", Year = "2019", TopSpeed = "180" },
                new Car() { Automaker = "Toyota", Name = "Camry", Year = "2018", TopSpeed = "160" }
            };
            this.carCtl.listBoxCars.ItemsSource = carList;
        }
    }
}

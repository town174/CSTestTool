using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.Entity
{
    public class Teacher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set {
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
    }
}

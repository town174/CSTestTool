using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSamples.LogicCore.Modules;

namespace WpfSamples.ViewModel
{
    [Module(ModuleType.KnowledgePoint, "XamlView", "1_Xaml")]
    public class XamlViewModel : ViewModelBase
    {

        private string title = "Xaml知识点xxx";

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}

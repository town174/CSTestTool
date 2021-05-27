using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSamples.LogicCore.Modules
{
    /// <summary>
    /// 模块管理入口
    /// </summary>
    public class ModuleManager : ViewModelBase
    {
        public ModuleManager()
        {
            InitModuleGroups();
        }

        private void InitModuleGroups()
        {
            Array array = System.Enum.GetValues(typeof(ModuleType));

            foreach (var m in array)
            {
                ModuleType t = (ModuleType)m;
                var attr = GetEnumAttrbute.GetDescription(t);
                if (attr != null)
                    _ModuleGroups.Add(new ModuleGroup() { ModuleType = t, GroupName = attr.Caption, GroupIcon = attr.Remark });
            }
        }

        private ObservableCollection<PageModule> _Modules = new ObservableCollection<PageModule>();
        private ObservableCollection<ModuleGroup> _ModuleGroups = new ObservableCollection<ModuleGroup>();

        /// <summary>
        /// 已加载模块<含分组>
        /// </summary>
        public ObservableCollection<ModuleGroup> ModuleGroups
        {
            get { return _ModuleGroups; }
            set { _ModuleGroups = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 已加载模块
        /// </summary>
        public ObservableCollection<PageModule> Modules
        {
            get { return _Modules; }
            set { _Modules = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 加载模块-根据权限
        public async Task LoadModules()
        {
            try
            {
                ModuleComponent loader = new ModuleComponent();
                var IModule = await loader.GetModules();
                IModule = IModule.OrderBy(x => x.Name).ToList();
                foreach (var i in IModule)
                {
                    if (!loader.ModuleVerify(i)) continue;

                    var m = ModuleGroups.FirstOrDefault(t => t.ModuleType.Equals(i.ModuleType));
                    if (m != null)
                    {
                        if (m.Modules == null) m.Modules = new ObservableCollection<PageModule>();
                        int value = int.MaxValue;
                        m.Modules.Add(new PageModule(i.Code, i.Name, value, i.ICON));
                    }
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载模块-根据权限
        public void LoadModulesSync()
        {
            try
            {
                ModuleComponent loader = new ModuleComponent();
                var IModule =  loader.GetModules().Result;
                foreach (var i in IModule)
                {
                    if (!loader.ModuleVerify(i)) continue;

                    var m = ModuleGroups.FirstOrDefault(t => t.ModuleType.Equals(i.ModuleType));
                    if (m != null)
                    {
                        if (m.Modules == null) m.Modules = new ObservableCollection<PageModule>();
                        //todo
                        //int value = Loginer.LoginerUser.IsAdmin == true ? int.MaxValue : loader.Authority.authorities;
                        int value = int.MaxValue;
                        m.Modules.Add(new PageModule(i.Code, i.Name, value, i.ICON));
                    }
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

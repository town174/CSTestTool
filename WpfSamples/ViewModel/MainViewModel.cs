using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfSamples.LogicCore.Common;
using WpfSamples.LogicCore.Enum;
using WpfSamples.LogicCore.Modules;
using WpfSamples.UiCore.Controls;

namespace WpfSamples.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            //_ModuleManager = new ModuleManager();
            //_ModuleManager.ModuleGroups = new System.Collections.ObjectModel.ObservableCollection<ModuleGroup>() {
            //    new ModuleGroup(){
            //        GroupName = "xxxx",
            //        ModuleType = ModuleType.BasicData,
            //        Modules = new System.Collections.ObjectModel.ObservableCollection<Module>(){
            //            new Module("code","xaml",int.MaxValue,"")
            //        }
            //    }
            //};

            Messenger.Default.Register<string>(this, "LoadData", a => {
                //_ModuleManager = new ModuleManager();
                //await _ModuleManager.LoadModules();
                InitDefaultView();
            });
        }

        #region 模块系统

        private ModuleManager _ModuleManager;

        /// <summary>
        /// 模块管理器
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return _ModuleManager; }
        }

        #endregion

        #region 命令(Binding Command)

        private RelayCommand<PageModule> _ExcuteCommand;
        private RelayCommand<PageInfo> _ExitCommand;

        /// <summary>
        /// 打开模块
        /// </summary>
        public RelayCommand<PageModule> ExcuteCommand
        {
            get
            {
                if (_ExcuteCommand == null)
                {
                    _ExcuteCommand = new RelayCommand<PageModule>(t => Excute(t));
                }
                return _ExcuteCommand;
            }
            set { _ExcuteCommand = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 关闭页
        /// </summary>
        public RelayCommand<PageInfo> ExitCommand
        {
            get
            {
                if (_ExitCommand == null)
                {
                    _ExitCommand = new RelayCommand<PageInfo>(t => ExitPage(t));
                }
                return _ExitCommand;
            }
            set { _ExitCommand = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 初始化/页面相关

        public ObservableCollection<PageInfo> OpenPageCollection { get; set; } = new ObservableCollection<PageInfo>();

        private object _CurrentPage;

        /// <summary>
        /// 当前选择页
        /// </summary>
        public object CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 初始化首页
        /// </summary>
        public async void InitDefaultView()
        {
            //初始化工具栏,通知窗口
            //_PopBoxView = new PopBoxViewModel();
            //_NoticeView = new NoticeViewModel();
            //加载窗体模块
            _ModuleManager = new ModuleManager();
            await _ModuleManager.LoadModules();
            //设置系统默认首页
            var page = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals("系统首页"));
            if (page == null)
            {
                //演示Demo加载默认首页,较消耗性能。 实际开发务移除患者更新开发部件。
                Home about = new Home();
                OpenPageCollection.Add(new PageInfo() { HeaderName = "系统首页", Body = about });
                CurrentPage = OpenPageCollection[OpenPageCollection.Count - 1];
            }
        }

        /// <summary>
        /// 执行模块
        /// </summary>
        /// <param name="module"></param>
        private async void Excute(PageModule module)
        {
            try
            {
                var page = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals(module.Name));
                if (page != null) { CurrentPage = page; return; }
                if (string.IsNullOrWhiteSpace(module.Code))
                {
                    //404页面
                    //DefaultViewPage defaultViewPage = new DefaultViewPage();
                    //OpenPageCollection.Add(new PageInfo() { HeaderName = module.Name, Body = defaultViewPage });
                    //CurrentPage = defaultViewPage;
                }
                else
                {
                    await Task.Factory.StartNew(() =>
                    {
                        //todo load page
                        //var dialog = ServiceProvider.Instance.Get<IModel>(module.Code);
                        //dialog.BindDefaultModel();
                        //OpenPageCollection.Add(new PageInfo() { HeaderName = module.Name, Body = dialog.GetView() });

                        //todo module=>view
                        var defaultViewPage = GetWindow(module.Code);
                        OpenPageCollection.Add(new PageInfo() { HeaderName = module.Name, Body = defaultViewPage });
                        CurrentPage = defaultViewPage;

                    }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                }
                CurrentPage = OpenPageCollection[OpenPageCollection.Count - 1];
            }
            catch (Exception ex)
            {
                //Msg.Error(ex.Message);
            }
            finally
            {
                Messenger.Default.Send(false, "PackUp");
                GC.Collect();
            }
        }

        /// <summary>
        /// 获取当前程序集下模块
        /// </summary>
        /// <returns></returns>
        public object GetWindow(string viewName)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                var types = asm.GetTypes();
                foreach (var t in types)
                {
                    //var wnd = (ModuleAttribute)t.GetCustomAttribute(typeof(Window), false);
                    //if (wnd.Name.Equals(viewName))
                    //return
                    //var tt = t.GetType().Name;
                    //if (t.GetType().FullName.Equals(typeof(Window).FullName) && t.Name.Equals(viewName))
                    if(t.Name.Equals(viewName))
                        return Activator.CreateInstance(t);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="module"></param>
        private void ExitPage(PageInfo module)
        {
            try
            {
                var tab = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals(module.HeaderName));
                if (tab.HeaderName != "系统首页") OpenPageCollection.Remove(tab);
            }
            catch (Exception ex)
            {
                //Msg.Error(ex.Message);
            }
        }

        public void ExitPage(MenuBehaviorType behaviorType, string pageName)
        {
            switch (behaviorType)
            {
                case MenuBehaviorType.ExitCurrentPage:
                    var page = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals(pageName));
                    if (page.HeaderName != "系统首页") OpenPageCollection.Remove(page);
                    break;
                case MenuBehaviorType.ExitAllPage:
                    var pageList = OpenPageCollection.Where(t => t.HeaderName != "系统首页").ToList();
                    if (pageList != null)
                    {
                        pageList.ForEach(t =>
                        {
                            OpenPageCollection.Remove(t);
                        });
                    }
                    break;
                case MenuBehaviorType.ExitAllExcept:
                    var pageListExcept = OpenPageCollection.Where(t => t.HeaderName != pageName && t.HeaderName != "系统首页").ToList();
                    if (pageListExcept != null)
                    {
                        pageListExcept.ForEach(t =>
                        {
                            OpenPageCollection.Remove(t);
                        });
                    }
                    break;
            }
        }

        #endregion
    }
}
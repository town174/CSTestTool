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

        #region ģ��ϵͳ

        private ModuleManager _ModuleManager;

        /// <summary>
        /// ģ�������
        /// </summary>
        public ModuleManager ModuleManager
        {
            get { return _ModuleManager; }
        }

        #endregion

        #region ����(Binding Command)

        private RelayCommand<PageModule> _ExcuteCommand;
        private RelayCommand<PageInfo> _ExitCommand;

        /// <summary>
        /// ��ģ��
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
        /// �ر�ҳ
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

        #region ��ʼ��/ҳ�����

        public ObservableCollection<PageInfo> OpenPageCollection { get; set; } = new ObservableCollection<PageInfo>();

        private object _CurrentPage;

        /// <summary>
        /// ��ǰѡ��ҳ
        /// </summary>
        public object CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// ��ʼ����ҳ
        /// </summary>
        public async void InitDefaultView()
        {
            //��ʼ��������,֪ͨ����
            //_PopBoxView = new PopBoxViewModel();
            //_NoticeView = new NoticeViewModel();
            //���ش���ģ��
            _ModuleManager = new ModuleManager();
            await _ModuleManager.LoadModules();
            //����ϵͳĬ����ҳ
            var page = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals("ϵͳ��ҳ"));
            if (page == null)
            {
                //��ʾDemo����Ĭ����ҳ,���������ܡ� ʵ�ʿ������Ƴ����߸��¿���������
                Home about = new Home();
                OpenPageCollection.Add(new PageInfo() { HeaderName = "ϵͳ��ҳ", Body = about });
                CurrentPage = OpenPageCollection[OpenPageCollection.Count - 1];
            }
        }

        /// <summary>
        /// ִ��ģ��
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
                    //404ҳ��
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
        /// ��ȡ��ǰ������ģ��
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
        /// �ر�ҳ��
        /// </summary>
        /// <param name="module"></param>
        private void ExitPage(PageInfo module)
        {
            try
            {
                var tab = OpenPageCollection.FirstOrDefault(t => t.HeaderName.Equals(module.HeaderName));
                if (tab.HeaderName != "ϵͳ��ҳ") OpenPageCollection.Remove(tab);
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
                    if (page.HeaderName != "ϵͳ��ҳ") OpenPageCollection.Remove(page);
                    break;
                case MenuBehaviorType.ExitAllPage:
                    var pageList = OpenPageCollection.Where(t => t.HeaderName != "ϵͳ��ҳ").ToList();
                    if (pageList != null)
                    {
                        pageList.ForEach(t =>
                        {
                            OpenPageCollection.Remove(t);
                        });
                    }
                    break;
                case MenuBehaviorType.ExitAllExcept:
                    var pageListExcept = OpenPageCollection.Where(t => t.HeaderName != pageName && t.HeaderName != "ϵͳ��ҳ").ToList();
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
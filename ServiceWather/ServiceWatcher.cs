using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWatcher
{
    partial class ServiceWatcher : ServiceBase
    {
        private static string currentExePath = string.Empty;
        public ServiceWatcher()
        {
            InitializeComponent();
            currentExePath = AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 服务运行方式
        /// </summary>
        private static readonly int _runModel = Convert.ToInt32(ConfigurationManager.AppSettings["runModel"]);
        /// <summary>
        /// 检查间隔
        /// </summary>
        private static readonly int _timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["timerInterval"]) * 1000;
        /// <summary>
        /// 要守护的服务名
        /// </summary>
        private static readonly string _fullName = ConfigurationManager.AppSettings["fullName"];
        /// <summary>
        /// 要守护的服务名
        /// </summary>
        private static readonly string _toWatchServiceName = ConfigurationManager.AppSettings["toWatchServiceName"];
        /// <summary>
        /// 检查连接
        /// </summary>
        private static readonly string _checkUrl = ConfigurationManager.AppSettings["checkurl"];
        private System.Timers.Timer _timer;

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            //服务启动时开启定时器
            _timer = new System.Timers.Timer();
            _timer.Interval = _timerInterval;
            _timer.Enabled = true;
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if(_runModel == 1)
            {
                //如果服务状态为停止，则重新启动服务
                if (!CheckSericeStart(_toWatchServiceName))
                {
                    StartService(_toWatchServiceName);
                }
            }
            else if (_runModel == 2)
            {
                //如果进程状态为停止或不存在，则重新启动进程
                if (!CheckProcessStart(_toWatchServiceName))
                {
                    StartProcess(_toWatchServiceName,_fullName);
                }
            }
            else if (_runModel == 3)
            {
                //如果cmd状态为停止或不存在，则重新启动进程
                if (!CheckCmdStart(_toWatchServiceName))
                {
                    StartCmd(_toWatchServiceName, _fullName);
                }
            }
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            //如果服务状态为停止，则重新启动服务
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        #region Cmd
        private bool CheckCmdStart(string processName)
        {
            bool result = true;
            try
            {
                var processes = Process.GetProcesses(); 
                if (!processes.Any(r => r.ProcessName.Equals(processName))) return false;
                else
                {
                    RestClient client = new RestClient(_checkUrl);
                    IRestRequest request = new RestRequest(Method.GET);
                    request.Timeout = _timerInterval / 4;
                    var respone = client.Get(request);//Get()(request);
                    if (respone.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        private bool StartCmd(string processName, string processFullName)
        {
            StopCmd(processName);
            //todo
            Process p = new Process();
            //设置要启动cmd
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //显示程序窗口
            p.StartInfo.CreateNoWindow = false;
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(processFullName + "&exit"); //+ "&exit");
            p.StandardInput.AutoFlush = true;

            //获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            return true;
        }
        private bool StopCmd(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes != null && processes.Count() > 0)
            {
                int c = processes.Count();
                for (int i = 0; i < c; i++)
                {
                    processes[i].Kill();
                }
            }
            return true;
        }
        #endregion

        #region Process
        private bool CheckProcessStart(string processName)
        {
            bool result = true;
            try
            {
                var processes = Process.GetProcesses(); //ServiceController.GetServices();
                if (!processes.Any(r => r.ProcessName.Equals(processName))) return false;
                else
                {
                    RestClient client = new RestClient(_checkUrl);
                    IRestRequest request = new RestRequest(Method.GET);
                    request.Timeout = _timerInterval / 4;
                    var respone = client.Get(request);//Get()(request);
                    if (respone.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        private bool StartProcess(string processName, string processFullName)
        {
            StopProcess(processName);
            Process.Start(processFullName);
            return true;
        }
        private bool StopProcess(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes != null && processes.Count() > 0)
            {
                int c = processes.Count();
                for (int i = 0; i < c; i++)
                {
                    processes[i].Kill();
                }
            }
            return true;
        }
        #endregion

        #region Service
        private bool CheckSericeStart(string serviceName)
        {
            bool result = true;
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
                        if ((service.Status == ServiceControllerStatus.Stopped)
                            || (service.Status == ServiceControllerStatus.StopPending))
                        {
                            result = false;
                            return result;
                        }
                    }
                    if (serviceName.Trim().Equals(_toWatchServiceName))
                    {
                        RestClient client = new RestClient(_checkUrl);
                        IRestRequest request = new RestRequest(Method.GET);
                        request.Timeout = _timerInterval / 4;
                        var respone = client.Get(request);//Get()(request);
                        if (respone.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            result = false;
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(currentExePath, ex);
            }
            return result;
        }
        private void StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            service.Stop();
                        }
                        service.Start();
                        //直到服务启动
                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                        //LogHelper.WriteLog(currentExePath, string.Format("启动服务:{0}", serviceName));
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(currentExePath, ex);
            }
        }
        private void StopService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
                        service.Stop();
                        //直到服务停止
                        service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                        //LogHelper.WriteLog(currentExePath, string.Format("启动服务:{0}", serviceName));
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(currentExePath, ex);
            }
        }
        #endregion
    }
}

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

namespace ServiceWather
{
    partial class ServiceWather : ServiceBase
    {
        private static string currentExePath = string.Empty;
        public ServiceWather()
        {
            InitializeComponent();
            currentExePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 检查间隔
        /// </summary>
        private static readonly int _timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["timerInterval"]) * 1000;
        /// <summary>
        /// 要守护的服务名
        /// </summary>
        private static readonly string toWatchServiceName = ConfigurationManager.AppSettings["toWatchServiceName"];
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
            //如果服务状态为停止，则重新启动服务
            if (!CheckSericeStart(toWatchServiceName))
            {
                StartService(toWatchServiceName);
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

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">要启动的服务名称</param>
        private void StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Trim() == serviceName.Trim())
                    {
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
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="serviceName"></param>
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
    }
}

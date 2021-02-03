using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftUpdateTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        RegisterHelper register = new RegisterHelper();
        private void Form1_Load(object sender, EventArgs e)
        {
            //const string _relativPath = @"Microsoft\NET Framework Setup\NDP\";
            //var vs = register.GetValues(RegisterHelper.KeyType.HKEY_LOCAL_MACHINE, _relativPath);
            //MessageBox.Show("net版本:" + string.Join(",",vs));
            var vs = GetNetVersionsOnPC();
            TbVersion.Text = string.Join(",", vs);
        }

        public void OpenDefaultBroswer(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        #region Functions.RegistryKey.DotNetVersion

        /// <summary>
        /// Get:判断.Net Framework的Release是否符合需要
        /// (.Net Framework 版本在4.0及以上)
        /// </summary>
        /// <param name="release">需要的版本 version = 4.5 release = 379893</param>
        /// <returns></returns>
        public bool GetNetRelease(int release)
        {
            const string _path = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (Microsoft.Win32.RegistryKey _key = Microsoft.Win32.RegistryKey.OpenBaseKey(
              Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32)
              .OpenSubKey(_path))
            {
                if (_key == null) { return false; }
                object _value = _key.GetValue("Release");
                if (_value == null) { return false; }
                if (_value is int _netValue)
                {
                    return _netValue >= release;
                }
                return false;
            }
        }




        /// <summary>
        /// Get:PC已安装的.Net Framework的所有版本号
        /// (.Net Framework 版本在2.0及以上)
        /// </summary>
        /// <returns></returns>
        public string[] GetNetVersionsOnPC()
        {
            //const string _path = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            const string _path = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";

            using (Microsoft.Win32.RegistryKey _key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(
              Microsoft.Win32.RegistryHive.LocalMachine, "")
              .OpenSubKey(_path))
            {
                if (_key == null) { return new string[0]; }

                string[] _keyNames = _key.GetSubKeyNames();
                List<string> _results = new List<string>();

                for (int i = 0; i < _keyNames.Length; i++)
                {
                    string _keyName = _keyNames[i];
                    if (!_keyName.StartsWith("v")) { continue; }

                    Microsoft.Win32.RegistryKey _versionKey = _key.OpenSubKey(_keyName);
                    string _value = this.FindRegistryKeyValueString(_versionKey, "Version", "");
                    _results.Add(_value);
                }
                return _results.ToArray();
            }
        }


        /// <summary>
        /// Get:PC已安装的.Net Framework的最大版本号
        /// (.Net Framework 版本在2.0及以上)
        /// </summary>
        /// <returns></returns>
        public string GetMaxNetVersionOnPC()
        {
            string[] _versions = this.GetNetVersionsOnPC();
            if (_versions == null) { return ""; }
            if (_versions.Length < 1) { return ""; }

            string _result = "";
            foreach (string _version in _versions)
            {
                if (string.Compare(_version, _result) > 0)
                {
                    _result = _version;
                }
            }
            return _result;
        }

        /// <summary>
        /// 判断.Net Framework的Version是否符合需要
        /// (.Net Framework 版本在2.0及以上)
        /// </summary>
        /// <param name="version">需要的版本 version = 4.5</param>
        /// <returns></returns>
        public bool CheckNetVersionOnPC(string version)
        {
            string _max = this.GetMaxNetVersionOnPC();
            if (string.IsNullOrWhiteSpace(_max)) { return false; }
            return string.Compare(_max, version) >= 0 ? true : false;
        }

        #endregion

        #region Functions.RegistryKey

        /// <summary>
        /// Get:查找注册表值
        /// </summary>
        /// <param name="key">注册节点</param>
        /// <param name="valueName">注册值名称</param>
        /// <returns>返回符合条件的第一个值</returns>
        public object FindRegistryKeyValue(Microsoft.Win32.RegistryKey key, string valueName)
        {
            if (key == null) { return null; }
            if (string.IsNullOrWhiteSpace(valueName)) { return null; }

            object _value = key.GetValue(valueName);
            if (_value != null) { return _value; }

            string[] _keyNames = key.GetSubKeyNames();
            foreach (string _keyName in _keyNames)
            {
                Microsoft.Win32.RegistryKey _subKey = key.OpenSubKey(_keyName);
                _value = this.FindRegistryKeyValue(_subKey, valueName);
                if (_value != null) { return _value; }
            }

            return null;
        }

        /// <summary>
        /// Get:查找注册表值
        /// </summary>
        /// <param name="key">注册节点</param>
        /// <param name="valueName">注册值名称</param>
        /// <param name="defaultValue">注册值的默认值</param>
        /// <returns>返回符合条件的第一个值</returns>
        public object FindRegistryKeyValue(Microsoft.Win32.RegistryKey key, string valueName, object defaultValue)
        {
            object _value = this.FindRegistryKeyValue(key, valueName);
            if (_value == null) { return defaultValue; }
            return _value.ToString();
        }

        /// <summary>
        /// Get:查找注册表值的文本值
        /// </summary>
        /// <param name="key">注册节点</param>
        /// <param name="valueName">注册值名称</param>
        /// <param name="defaultValue">注册值的默认值</param>
        /// <returns>返回符合条件的第一个值</returns>
        public string FindRegistryKeyValueString(Microsoft.Win32.RegistryKey key, string valueName, string defaultValue = "")
        {
            object _value = this.FindRegistryKeyValue(key, valueName);
            if (_value == null) { return defaultValue; }
            return _value.ToString();
        }

        #endregion

        private void BtnCheckVersion_Click(object sender, EventArgs e)
        {
            var miniVersion = TbSoftVersion.Text;
            var cds = CheckNetVersionOnPC(miniVersion);
            MessageBox.Show($"系统版本{(cds ? "满足" : "不满足")}软件需要");
            if (!cds) OpenDefaultBroswer("https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net48-web-installer");
        }
    }
}

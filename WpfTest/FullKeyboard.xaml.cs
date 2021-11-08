using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XDM_Controller
{
    /// <summary>
    /// FullKeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class FullKeyboard : Window
    {
        private String valueString;

        internal String ValueString
        {
            get { return valueString; }
        }

        public FullKeyboard() : this("虚拟键盘","")
        {

        }

        public  FullKeyboard(String inputTitle, String inputvalue)
        {
            InitializeComponent();
            FullKeyboardTitle.Text = inputTitle;
            TbValue.Text = inputvalue;
            valueString = inputvalue;
        }

        //通过判断按钮的content属性来做对应处理，以简化大量按钮的编程
        private void ButtonGrid_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)e.OriginalSource;    //获取click事件触发源，即按了的按钮
            if ((String)clickedButton.Content=="DEL")
            {
                if (TbValue.Text.Length>0)
                {
                    TbValue.Text = TbValue.Text.Substring(0, TbValue.Text.Length - 1);
                }
            }
            else if ((String)clickedButton.Content == "AC")
            {
                TbValue.Text = "";
            }
            else if ((String)clickedButton.Content == "确认")
            {
                valueString = TbValue.Text;
                this.Close();
            }
            else if ((String)clickedButton.Content == "A/a")
            {
                int count = ButtonGrid.Children.Count;
                for (int i = 10; i < count-4; i++)
                {
                    Button buttonTemp = ButtonGrid.Children[i] as Button;
                    String contentTemp = buttonTemp.Content as String;
                    buttonTemp.Content = contentTemp[0] > 90 ? contentTemp.ToUpper() : contentTemp.ToLower();
                }
            }
            else
            {
                TbValue.Text += (String)clickedButton.Content;
            }
        }

    }
}

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
using System.Drawing;

namespace ContentsConverter
{
    /// <summary>
    /// wndAddRule.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wndAddRule : Window
    {
        public wndAddRule()
        {
            InitializeComponent();
        }

        public delegate void OnCloseEventHandler (ConvertInfo cinfo);
        public event OnCloseEventHandler OnCloseEvent;

        string err;

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                ConvertInfo ci = new ConvertInfo();
                ci.origin = tb_origin.Text;
                ci.target = tb_target.Text; 

                if (OnCloseEvent != null) OnCloseEvent(ci);
            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }

            
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            if (OnCloseEvent != null) OnCloseEvent(null);
        }
    }
}

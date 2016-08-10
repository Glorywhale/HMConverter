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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContentsConverter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void OnCloseEventHandler();
        public event OnCloseEventHandler OnCloseEvent;

        wndAddRule wndAddRule = null;
        private void ShowWndAddRule()
        {
            if (wndAddRule == null)
            {
                wndAddRule = new wndAddRule();
                wndAddRule.OnCloseEvent += new ContentsConverter.wndAddRule.OnCloseEventHandler(wndAddRule_OnCloseEvent);                
            }
            wndAddRule.Show();
        }

        private void CloseWndAddRule()
        {
            if (wndAddRule != null)
            {
                wndAddRule.Close();
                wndAddRule.OnCloseEvent -=new ContentsConverter.wndAddRule.OnCloseEventHandler(wndAddRule_OnCloseEvent);
                wndAddRule = null;
            }
        }

        ConfigFile cf = new ConfigFile();
        void wndAddRule_OnCloseEvent(ConvertInfo cinfo)
        {
            if (cinfo != null)
            {                
                if (cf.Load() == false) return;

                ConvertInfoSet.DT_ConvertRow row = cf.ConvertSet.DT_Convert.NewDT_ConvertRow();
                row.origin = cinfo.origin;
                row.target = cinfo.target;
                cf.AddConvertInfo(row);
                cf.Save();
            }

            RefreshConvertInfo();

            CloseWndAddRule();
        }

        List<ConvertInfo> ciList = new List<ConvertInfo>();
        public void RefreshConvertInfo()
        {
            try
            {
                cf.Load();
                ciList.Clear();
                foreach (ConvertInfoSet.DT_ConvertRow row in cf.ConvertSet.DT_Convert)
                {
                    ConvertInfo ci = new ConvertInfo();
                    ci.origin = row.origin;
                    ci.target = row.target;

                    ciList.Add(ci);
                }

                dgRule.ItemsSource = null;
                dgRule.ItemsSource = ciList;

            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private string err = string.Empty;
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowWndAddRule();
            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private void btn_modify_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConvertInfo ci = dgRule.SelectedItem as ConvertInfo;
                for (int i = 0; i < ciList.Count; i++)
                {
                    ConvertInfoSet.DT_ConvertRow row = cf.ConvertSet.DT_Convert.Rows[i] as ConvertInfoSet.DT_ConvertRow;
                    if (row == null) continue;
                    if (row.origin.Equals(ci.origin) && row.target.Equals(ci.target))
                    {
                        cf.ConvertSet.DT_Convert.Rows.RemoveAt(i);
                        ciList.RemoveAt(dgRule.SelectedIndex);
                    }
                }

                cf.Save();

                dgRule.ItemsSource = null;
                dgRule.ItemsSource = ciList;
            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshConvertInfo();
            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            ConvertContents();
        }

        
        public void ConvertContents()
        {            
            string convertContents = tb_origin_contents.Text;
            for (int i = 0; i < ciList.Count; i++)
            {
                convertContents = convertContents.Replace(ciList[i].origin, ciList[i].target);                                
            }
            tb_target_contents.Text = convertContents;                
        }

        private void btn_copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(tb_target_contents.Text);
            }
            catch (Exception ex)
            {
                err = string.Format(ex.Message +" - " +ex.StackTrace);
                MessageBox.Show(err, "오류 발생", MessageBoxButton.OK);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (OnCloseEvent != null) OnCloseEvent();
        }

        
    }
}

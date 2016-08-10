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
using System.Diagnostics;
using MahApps.Metro.Controls;

namespace ContentsConverter
{
    /// <summary>
    /// wndParsing.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class wndParsing : MetroWindow
    {
        public wndParsing()
        {
            InitializeComponent();

            webBrowser1 = new vwBrowser();
            webBrowser1.OnCompleteEvent += new vwBrowser.OnCompleteEventHandler(webBrowser1_OnCompleteEvent);
            wf.Child = webBrowser1;
        }

        ConvertManager cm = new ConvertManager();
        bool isLogin = false;
        void webBrowser1_OnCompleteEvent(ParseInfo pitem)
        {
            if (!isLogin)
            {
                webBrowser1.Login(tb_id.Text, tb_password.Password);
                isLogin = true;
            }

            if (pitem != null)
            {
                tb_url.Text = pitem.url;
                tb_title.Text = pitem.title;
                tb_age.Text = pitem.age;
                tb_acttarget.Text = pitem.target;
                tb_nuri.Text = pitem.nuri;
                tb_actintro.Text = pitem.intro;
                tb_actMat.Text = pitem.material;
                tb_actMethod.Text = cm.ConvertContents(pitem.method);
            }
        }

        vwBrowser webBrowser1;
        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            BrowseUrl();
        }

        public void BrowseUrl()
        {
            try
            {
                string url = tb_url.Text;
                if (!tb_url.Text.Contains("http://"))
                {
                    url = "http://" + tb_url.Text;
                }
                tb_url.Text = url;

                if (url.Length == 0)
                {
                    url = "http://www.kidkids.net/eduinfo_new/eduplan_day.htm";
                }
                webBrowser1.Navigate(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }
                
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string url = "http://www.kidkids.net/";            
            webBrowser1.Navigate(url);
        }

        private void tb_copy_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null)
            {
                string type = btn.Name.Substring(0, btn.Name.IndexOf("copy") - 1);

                TextBox tb = sp_table.FindName(type) as TextBox;
                Clipboard.SetText(tb.Text);
            }
        }

        MainWindow main = null;
        public void ShowConvert()
        {
            if (main == null)
            {
                main = new MainWindow();
                main.OnCloseEvent += new MainWindow.OnCloseEventHandler(main_OnCloseEvent);                
            }
            main.Show();
        }

        void main_OnCloseEvent()
        {
            if (main != null)
            {
                main.Close();
                main.OnCloseEvent -=new MainWindow.OnCloseEventHandler(main_OnCloseEvent);
                main = null;
            }
            this.Show();
        }

        private void btn_showConvert_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ShowConvert();
        }

        private void btn_web_Click(object sender, RoutedEventArgs e)
        {
            cd_web.Width = new GridLength(3, GridUnitType.Star);
            cd_contents.Width = new GridLength(0, GridUnitType.Star);
        }

        private void btn_contents_Click(object sender, RoutedEventArgs e)
        {
            cd_web.Width = new GridLength(0, GridUnitType.Star);
            cd_contents.Width = new GridLength(3, GridUnitType.Star);
        }

        private void btn_half_Click(object sender, RoutedEventArgs e)
        {
            cd_web.Width = new GridLength(1, GridUnitType.Star);
            cd_contents.Width = new GridLength(1, GridUnitType.Star);
        }

        private void btn_prev_Click(object sender, RoutedEventArgs e)
        {
            webBrowser1.WebBrowser.Document.Window.History.Go(-1);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            webBrowser1.WebBrowser.Document.Window.History.Go(1);
        }

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://www.kidkids.net/";
            webBrowser1.Navigate(url);
        }

        private void tb_url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BrowseUrl();
            } 
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("♡완전 스릉흔드♡!!", "♡", MessageBoxButton.OK);
        }
    }
}

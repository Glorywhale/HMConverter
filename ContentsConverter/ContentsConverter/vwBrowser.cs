using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ContentsConverter
{
    public partial class vwBrowser : UserControl
    {
        public vwBrowser()
        {
            InitializeComponent();
                        
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        public WebBrowser WebBrowser
        {
            get { return webBrowser1; }
            set { webBrowser1 = value; }
        }

        private void vwBrowser_Load(object sender, EventArgs e)
        {
            
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {   
            webBrowser1.Document.Click += new HtmlElementEventHandler(Document_Click);

            GetText();
        }

        
        private void GetText()
        {
            //List<string> strList = new List<string>();
            string temp = string.Empty;

            ParseInfo pitem = new ParseInfo();

            pitem.url = webBrowser1.Url.ToString();

            // Title
            HtmlElementCollection h2List = webBrowser1.Document.GetElementsByTagName("h2");
            foreach (HtmlElement h2Item in h2List)
            {
                if (!h2Item.InnerHtml.Contains("href"))
                {
                    pitem.title = h2Item.InnerText;
                }
            }

            // Content Table
            HtmlElementCollection tables = webBrowser1.Document.GetElementsByTagName("table");
            foreach (HtmlElement heitem in tables)
            {
                if (heitem.GetAttribute("className").Equals("ep_info actdetail"))
                {
                    HtmlElementCollection thitems = heitem.GetElementsByTagName("th");
                    HtmlElementCollection tditems = heitem.GetElementsByTagName("td");



                    for (int i = 0; i < thitems.Count; i++)
                    {
                        //temp += thitems[i].InnerText + " " + tditems[i].InnerText + "\n";
                        if (thitems[i].InnerText.Contains("연령"))
                        {
                            pitem.age = tditems[i].InnerText;
                        } else if (thitems[i].InnerText.Contains("목표"))
                        {
                            pitem.target = tditems[i].InnerText;
                        }
                        else if (thitems[i].InnerText.Contains("누리"))
                        {
                            pitem.nuri = tditems[i].InnerText;
                        }
                        else if (thitems[i].InnerText.Contains("개요"))
                        {
                            pitem.intro = tditems[i].InnerText;
                        }
                        else if (thitems[i].InnerText.Contains("자료"))
                        {
                            pitem.material = tditems[i].InnerText;
                        }                        
                    }
                    //temp += "\n";
                }
                
                if (heitem.GetAttribute("className").Equals("actdetail1 mgtop_vlow") || heitem.GetAttribute("className").Equals("actdetail1 mgtop_low"))
                {
                    HtmlElementCollection thitems = heitem.GetElementsByTagName("th");
                    HtmlElementCollection tditems = heitem.GetElementsByTagName("td");

                    for (int i = 0; i < thitems.Count; i++)
                    {
                        temp += thitems[i].InnerText + "\n" + tditems[i].InnerText + "\n";
                    }
                    temp += "\n";

                    pitem.method = temp;
                }                
            }

            if (OnCompleteEvent != null) OnCompleteEvent(pitem);
        }

        public void Login(string id, string password)
        {
            try
            {
                webBrowser1.Document.GetElementById("member_id").SetAttribute("value", id);
                webBrowser1.Document.GetElementById("pw").SetAttribute("value", password);


                webBrowser1.Document.GetElementById("login_btn").InvokeMember("click");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        public void Navigate(string url)
        {
            webBrowser1.Navigate(url);            
        }

        void Document_Click(object sender, HtmlElementEventArgs e)
        {
            //HtmlElement ele = webBrowser1.Document.GetElementFromPoint(e.MousePosition);
                        
            //while (ele != null)
            //{
            //    if (ele.TagName.ToLower() == "a")
            //    {
            //        // METHOD-1
            //        // Use the url to open a new tab
            //        //string url = ele.GetAttribute("href");
            //        // TODO: open the new tab
            //        //e.ReturnValue = false;

            //        // METHOD-2
            //        // Use this to make it navigate to the new URL on the current browser/tab
            //        ele.SetAttribute("target", "_self");
            //    }
            //    ele = ele.Parent;
            //}

            
            //item = item.Parent;
            CheckTag();           
            
        }

        public delegate void OnCompleteEventHandler(ParseInfo pitem);
        public event OnCompleteEventHandler OnCompleteEvent;

        private void CheckTag()
        {
            HtmlElementCollection ele = webBrowser1.Document.GetElementsByTagName("a");
            
            if (ele != null)
            {
                for (int i = 0; i < ele.Count; i++)
                {
                    HtmlElement item = ele[i];
                    if (item.TagName.ToLower() == "a")
                    {
                        // METHOD-1
                        // Use the url to open a new tab
                        //string url = item.GetAttribute("href");
                        //// TODO: open the new tab
                        //e.ReturnValue = false;

                        // METHOD-2
                        // Use this to make it navigate to the new URL on the current browser/tab                        
                        if (item.GetAttribute("target").Equals("_blank"))
                        {
                            item.SetAttribute("target", "_self");
                        }                        
                    }
                }
            }
        }
    }
}

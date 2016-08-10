using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentsConverter
{
    public class ConvertManager
    {
        ConfigFile cf = new ConfigFile();

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ex.Message + " - " + ex.StackTrace));
            }
        }

        public string ConvertContents(string origin)
        {
            RefreshConvertInfo();
            if (origin == null) return string.Empty;
            string convertContents = origin;
            for (int i = 0; i < ciList.Count; i++)
            {
                convertContents = convertContents.Replace(ciList[i].origin, ciList[i].target);
            }
            return convertContents;
        }
    }

    public class ConvertInfo
    {
        //public int idx;
        public string origin { get; set; }
        public string target { get; set; }
    }

    public class ParseInfo
    {
        public string url { get; set; }
        public string title { get; set; }
        public string age { get; set; }
        public string target { get; set; }
        public string nuri { get; set; }
        public string intro { get; set; }
        public string material { get; set; }
        public string method { get; set; }
    }
}

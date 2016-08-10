using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentsConverter
{
    public class ConfigFile
    {
        const string _folder = "data";
        const string _filename = "config.dat";

        private string filename = "";

        public ConfigFile()
        {
            filename = System.IO.Path.Combine(System.IO.Path.Combine(AppConfig.APPStartPath, _folder), _filename);
        }

        ConvertInfoSet convertSet = new ConvertInfoSet();
        public ConvertInfoSet ConvertSet
        {
            get { return convertSet; }
        }

        public bool AddConvertInfo(ConvertInfoSet.DT_ConvertRow row)
        {
            try
            {
                convertSet.DT_Convert.AddDT_ConvertRow(row);                
                return true;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool ModifyConvertInfo(ConvertInfoSet.DT_ConvertRow row)
        {
            try
            {
                if (convertSet.DT_Convert.Rows.Count > 0)
                {
                    ConvertInfoSet.DT_ConvertRow drow = convertSet.DT_Convert.Rows[0] as ConvertInfoSet.DT_ConvertRow;                    
                    drow = row;
                }
                return true;
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                return false;
            }
        }

        /// 데이터저장
        /// </summary>
        public bool Save()
        {
            try
            {
                string saveFolder = System.IO.Path.GetDirectoryName(filename);
                if (!System.IO.Directory.Exists(saveFolder)) System.IO.Directory.CreateDirectory(saveFolder);

                convertSet.WriteXml(filename);

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }


        /// <summary>
        /// 데이터읽어오기
        /// </summary>
        public bool Load()
        {
            try
            {
                if (System.IO.File.Exists(filename))
                {
                    convertSet.Clear();
                    convertSet.ReadXml(filename);
                }

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}

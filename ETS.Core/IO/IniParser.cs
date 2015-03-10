using System;
using System.Collections.Generic;
using System.IO;

namespace ETS.IO
{
    public class IniParser
    {
         //一个Section
        public class IniSection 
        {
            public string SectionName ;
            public List<KeyValuePair<string, string>> KVList;

            public IniSection(string SectionName)
            {
                this.SectionName = SectionName.Trim();
                this.KVList = new List<KeyValuePair<string, string>>();
            }

            public void AddSetting(string settingName, string settingValue)
            {
                for (int i = 0; i < this.KVList.Count; i++)
                {
                    if (this.KVList[i].Key == settingName)
                    {
                        if (settingValue == null)
                            this.KVList.RemoveAt(i--);
                        else
                            this.KVList[i] = new KeyValuePair<string, string>(settingName, settingValue);

                        return;
                    }
                }

                if(settingValue != null)
                    this.KVList.Add(new KeyValuePair<string, string>(settingName, settingValue));
            }

            //获取一个设置
            public string GetSetting(string settingName)
            {
                for(int i = 0;i<this.KVList.Count;i++)
                    if (this.KVList[i].Key == settingName)
                        return this.KVList[i].Value;

                return null;
            }
        }


        //总的数据
        private List<IniSection> IniData; 

    
        /// <summary>
        /// ini文件路径
        /// </summary>
        private string IniFilePath = null;

        /// <summary>
        /// 空的Ini结构体
        /// </summary>
        public IniParser()
        {
            IniData = new List<IniSection>();
        }

        public void SetIniContent(string content)
        {
            string currentSectionName = string.Empty;
            string strLine = string.Empty;
            string line_end = "\r\n";
            IniData = new List<IniSection>();

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            try
            {
                string[] allLines = content.Split(new string[] {line_end}, StringSplitOptions.RemoveEmptyEntries);
                if (allLines.Length > 0)
                {
                    foreach (string line in allLines)
                    {
                        strLine = line.Trim();
                        if (strLine == "" || strLine.StartsWith("#")) //跳过注释
                            continue;
                        if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            currentSectionName = strLine.Substring(1, strLine.Length - 2);
                        else
                        {
                            string[] keyPair = strLine.Split(new char[] { '=' }, 2); //这里恐怕不能用 split.回头再说
                            if (keyPair.Length != 2)
                                continue;

                            if (currentSectionName == null)
                                continue; //丢弃

                            string Key = keyPair[0];
                            string Value = keyPair[1];

                            AddSetting(currentSectionName, Key, Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Opens the INI file at the given path and enumerates the values in the IniParser.
        /// </summary>
        /// <param name="iniPath">Full path to INI file.</param>
        public IniParser(String iniPath)
        {
            TextReader iniFile = null;
            String strLine = null;
            String currentSectionName = null;
            IniFilePath = iniPath; //保存的时候用的

            IniData = new List<IniSection>();

            if (!File.Exists(iniPath)) //不存在就为空，纯写入用的
                return;

            try
            {
                iniFile = new StreamReader(iniPath);

                while ((strLine = iniFile.ReadLine()) != null)
                {
                    strLine = strLine.Trim();

                    if (strLine == "" || strLine.StartsWith("#")) //跳过注释
                        continue;

                    if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                        currentSectionName = strLine.Substring(1, strLine.Length - 2);
                    else
                    {
                        string []keyPair = strLine.Split(new char[] { '=' }, 2); //这里恐怕不能用 split.回头再说
                        if (keyPair.Length != 2)
                            continue;

                        if (currentSectionName == null)
                            continue; //丢弃

                        string Key = keyPair[0];
                        string Value = keyPair[1];

                        AddSetting(currentSectionName, Key, Value);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (iniFile != null)
                    iniFile.Close();
            }
        }

        /// <summary>
        /// 获得当前所有Section的列表
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionList()
        {
            List<string> section_list = new List<string>();
            for (int i = 0; i < IniData.Count; i++)
                section_list.Add(IniData[i].SectionName);

            return section_list.ToArray();
        }

        private IniSection LocateSection(string section_name)
        {
            for (int i = 0; i < IniData.Count; i++)
                if (section_name == IniData[i].SectionName)
                    return IniData[i];

            return null;
        }

        /// <summary>
        /// 获得当前所有Section的列表
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionSettingList(string section_name)
        {
            List<string> setting_list = new List<string>();

            IniSection isect = LocateSection(section_name);
            if (isect != null)
            {
                for (int i = 0; i < isect.KVList.Count; i++)
                    setting_list.Add(isect.KVList[i].Key);
            }

            return setting_list.ToArray();
        }

        /// <summary>
        /// Returns the value for the given section, key pair.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="settingName">Key name.</param>
        public String GetSetting(String sectionName, String settingName)
        {
            IniSection isect = LocateSection(sectionName);
            if (isect == null)
                return null;

            return isect.GetSetting(settingName);
        }

        /// <summary>
        /// Adds or replaces a setting to the table to be saved.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        /// <param name="settingValue">Value of key.</param>
        public void AddSetting(String sectionName, String settingName, String settingValue)
        {
            IniSection isect = LocateSection(sectionName);
            if (isect == null)
            {
                isect = new IniSection(sectionName);
                IniData.Add(isect);
            }

            isect.AddSetting(settingName, settingValue);
        }

        /// <summary>
        /// 增加一个设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="setting_value_raw"></param>
        public void AddSettingRaw(string settingName, string setting_value_raw)
        {
            string[] setting_list = setting_value_raw.Split('\n');
            for (int i = 0; i < setting_list.Length; i++)
            {
                string this_setting_value = setting_list[i].Trim();

                int equal_pos = this_setting_value.IndexOf('=');

                if (equal_pos == -1 || equal_pos == 0 || equal_pos == this_setting_value.Length - 1)
                    continue;

                string setting_key = this_setting_value.Substring(0, equal_pos);
                string setting_value = this_setting_value.Substring(equal_pos + 1);

                AddSetting(settingName, setting_key.Trim(), setting_value.Trim());
            }
        }
        /// <summary>
        /// Remove a setting.
        /// </summary>
        /// <param name="sectionName">Section to add under.</param>
        /// <param name="settingName">Key name to add.</param>
        public void DeleteSetting(String sectionName, String settingName)
        {
            AddSetting(sectionName, settingName, null);
        }

        /// <summary>
        /// ToString方法，得到Ini的内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String iniContent = "";
            string line_end = "\r\n";


            for (int i = 0; i < IniData.Count; i++)
            {
                IniSection isect = IniData[i];

                iniContent += ("[" + isect.SectionName + "]" + line_end);

                for (int j = 0; j < isect.KVList.Count; j++)
                {
                    iniContent += isect.KVList[j].Key + "=" + isect.KVList[j].Value + line_end;
                }

                iniContent += line_end;
            }

            return iniContent;
        }

        /// <summary>
        /// Save settings to new file.
        /// </summary>
        /// <param name="newFilePath">New file path.</param>
        public void SaveSettings(String newFilePath)
        {
            if (newFilePath == null)
                throw new Exception("在保存Ini文件时，发现文件路径为空");

            string iniContent = this.ToString();

            try
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                
                TextWriter tw = new StreamWriter(newFilePath);
                tw.Write(iniContent);
                tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save settings back to ini file.
        /// </summary>
        public void SaveSettings()
        {
            SaveSettings(IniFilePath);
        }
    }
    
}

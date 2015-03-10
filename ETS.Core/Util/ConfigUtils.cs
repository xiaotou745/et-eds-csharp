using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Common.Logging;

namespace ETS.Util
{
	public class ConfigUtils
	{
		/// <summary>
		/// 日志对象
		/// </summary>
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public static T GetConfigValue<T>(string key, T defaule) where T : class
		{
			AssertUtils.StringNotNullOrEmpty(key, "key");

			T result = defaule;
			try
			{
				if (Contains(ConfigurationManager.AppSettings.AllKeys, key))
				{
					string obj = ConfigurationManager.AppSettings[key];
					if (obj != null && obj.Trim() != "")
					{
						if (typeof(T) == typeof(int))
							result = int.Parse(obj.Trim()).ToString(CultureInfo.InvariantCulture) as T;
						else if (typeof(T) == typeof(bool))
							result = bool.Parse(obj.Trim()).ToString(CultureInfo.InvariantCulture) as T;
						else if (typeof(T) == typeof(string))
							result = obj.Trim() as T;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Info(key + "没有读取到", ex);
				result = defaule;
			}
			return result;
		}

		private static bool Contains(IEnumerable<string> appSettings, string key)
		{
			AssertUtils.ArgumentNotNull(appSettings, "appSettings");
			AssertUtils.StringNotNullOrEmpty(key, "key");

			foreach (string appSetting in appSettings)
			{
				if (appSetting == key)
				{
					return true;
				}
			}
			return false;
		}
	}
}
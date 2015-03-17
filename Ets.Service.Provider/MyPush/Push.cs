﻿//using ifunction.JPush;
//using ifunction.JPush.V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cn.jpush.api;
using cn.jpush.api.push;
using cn.jpush.api.report;
using cn.jpush.api.common;
using cn.jpush.api.util;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;
using ETS.Util;
using Ets.Model.Common;
//using SuperManCommonModel.Common;
//using SuperManCore;

namespace Ets.Service.Provider.MyPush
{
    public class Push
    {
        /// <summary>
        /// 极光消息推送
        /// </summary>
        /// <param name="tagId">来源标识（B端/C端）</param>
        /// <param name="title">提示title</param>
        /// <param name="alert"></param>
        /// <param name="content"></param>
        /// <param name="RegistrationId">商户id  注册ID 数组。多个注册ID之间是 OR 关系，即取并集。 设备标识。一次推送最多 1000 个。 </param>
        /// <param name="city">城市 </param>
        public static void PushMessage(int tagId, string title, string alert, string content, string RegistrationId, string city)
        {
            try
            {
                string appKey = "";
                string masterSecret = "";
                if (tagId == 0) //C端
                {
                    appKey = "dce902893245e99461b9a5c8";// Your App Key from JPush
                    masterSecret = "fdc95d37d67c9472ad4e0e96";// Your Master Secret from JPush
                }
                else if (tagId == 1) //B端
                {
                    appKey = "d794d51f2ffaf5de42001c4b";// Your App Key from JPush
                    masterSecret = "03f956afaaeb086481aa3b7c";// Your Master Secret from JPush
                }
                JPushClient client = new JPushClient(appKey, masterSecret);
                Audience audience = null;
                if (tagId == 0)  //C端
                {
                    if (ConfigSettings.Instance.IsGroupPush)   //TODO 暂时有效
                        audience = Audience.s_tag_and(city.Trim(), "海底捞");
                    else
                        audience = Audience.s_tag_and(city.Trim());
                }
                else if (tagId == 1 && !string.IsNullOrEmpty(RegistrationId)) //B端
                    audience = Audience.s_tag_and(RegistrationId);

                PushPayload pushPayload = new PushPayload();
                pushPayload.platform = Platform.android_ios();
                pushPayload.audience = audience;
                Notification notification = new Notification().setAlert(alert);
                notification.AndroidNotification = new AndroidNotification().setTitle(title);
                notification.IosNotification = new IosNotification().setAlert(alert).setBadge(1).setSound("YourSound");
                pushPayload.notification = notification.Check();
                var response = client.SendPush(pushPayload);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// 极光推送方法
        /// </summary>
        /// <param name="model"></param>
        public static void PushMessage(JPushModel model)
        {
            string appKey = "";
            string masterSecret = "";
            if (model.TagId == 0) //C端
            {
                appKey = "dce902893245e99461b9a5c8";// Your App Key from JPush
                masterSecret = "fdc95d37d67c9472ad4e0e96";// Your Master Secret from JPush
            }
            else if (model.TagId == 1) //B端
            {
                appKey = "d794d51f2ffaf5de42001c4b";// Your App Key from JPush
                masterSecret = "03f956afaaeb086481aa3b7c";// Your Master Secret from JPush
            }
            JPushClient client = new JPushClient(appKey, masterSecret);
            Audience audience = null;
            if (model.TagId == 0)  //C端
                audience = Audience.s_tag_and(model.City.Trim());
            else if (model.TagId == 1 && !string.IsNullOrEmpty(model.RegistrationId)) //B端
                audience = Audience.s_tag_and(model.RegistrationId);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = audience;
            Notification notification = new Notification().setAlert(model.Alert);
            notification.AndroidNotification = new AndroidNotification().setTitle(model.Title);
            notification.IosNotification = new IosNotification().setAlert(model.Alert).setBadge(1).setSound("YourSound");
            pushPayload.notification = notification.Check();
            var response = client.SendPush(pushPayload);
        }
    }
}
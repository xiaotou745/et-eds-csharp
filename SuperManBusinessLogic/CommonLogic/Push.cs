using ifunction.JPush;
using ifunction.JPush.V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManBusinessLogic.CommonLogic
{
    public class Push
    {
        public static void PushMessage(int tagId, string title, string alert, string content, string RegistrationId)
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

            Dictionary<string, string> customizedValues = new Dictionary<string, string>();
            //customizedValues.Add("CK1", "CV1");
            //customizedValues.Add("CK2", "CV2");

            JPushClientV3 client = new JPushClientV3(appKey, masterSecret);
            Audience audience = new Audience();

            // In JPush V3, tag can be multiple added with different values.
            // In following code, it is to send push to those who are in ((Tag1 AND Tag2) AND (Tag3 OR Tag4))
            // If you want to send to all, please use: audience.Add(PushTypeV3.Broadcast, null);

            // audience.Add(PushTypeV3.ByTagWithinAnd, new List<string>(new string[] { "Tag1", "Tag2" }));
            //audience.Add(PushTypeV3.ByTagWithinOr, new List<string>(new string[] { tagId.ToString() }));
            if (tagId == 0)
            {
                audience.Add(PushTypeV3.Broadcast, new List<string>(new string[] { "all" }));
            }
            if (tagId == 1)
            {
                if (!string.IsNullOrEmpty(RegistrationId))
                {
                    audience.Add(PushTypeV3.ByTagWithinAnd, new List<string>(new string[] { RegistrationId }));
                }
            }           

            //// audience.Add(PushTypeV3.Broadcast,null);

            // In JPush V3, Notification would not be display on screen, it would be transferred to app instead.
            // And different platform can provide different notification data.
            Notification notification = new Notification
            {
                AndroidNotification = new AndroidNotificationParameters
                {
                    Title = title,
                    Alert = alert,
                    CustomizedValues = customizedValues
                },
                iOSNotification = new iOSNotificationParameters
                {
                    Badge = 1,
                    Alert = alert,
                    Sound = "YourSound",
                    CustomizedValues = customizedValues
                }
            };

            var response = client.SendPushMessage(new PushMessageRequestV3
            {
                Audience = audience,
                Platform = PushPlatform.Android,
                IsTestEnvironment = true,
                AppMessage = new AppMessage
                {
                    Content = content,
                    CustomizedValue = customizedValues
                },
                Notification = notification
            });
            string ResponseCode = response.ResponseCode.ToString();
            string ResponseMessage = response.ResponseMessage;

            ResponseCode = response.ResponseCode.ToString();
            ResponseMessage = response.ResponseMessage;
            // Console.WriteLine(response.ResponseCode.ToString() + ":" + response.ResponseMessage);
            //Console.WriteLine("Push sent.");
            // Console.WriteLine(response.ResponseCode.ToString() + ":" + response.ResponseMessage);


            List<string> idToCheck = new List<string>();
            idToCheck.Add(response.MessageId);

            //var statusList = client.QueryPushMessageStatus(idToCheck);

            //SuperManDataAccess.pushmessage pushMessage = new SuperManDataAccess.pushmessage();
            //pushMessage.Title = title;
            //pushMessage.Alert = alert;
            //pushMessage.Content = content;
            //pushMessage.PushDate = DateTime.Now;
            //pushMessage.AreaId = tagId;

            //using (var ctx = new SuperManDataAccess.supermanEntities())
            //{
            //    ctx.pushmessage.Add(pushMessage);
            //    ctx.SaveChanges();
            //}
            //// Console.WriteLine("Status track is completed.");
            //if (statusList != null)
            //{
            //    foreach (var one in statusList)
            //    {
            //        SuperManDataAccess.pushmessagestatus pms = new SuperManDataAccess.pushmessagestatus();
            //        pms.MessageId = one.MessageId.HasValue ? one.MessageId.Value.ToString() : "";
            //        pms.AndroidDeliveredCount = one.AndroidDeliveredCount;

            //        using (var ctx = new SuperManDataAccess.supermanEntities())
            //        {
            //            ctx.pushmessagestatus.Add(pms);
            //            ctx.SaveChanges();
            //        }
            //        // Console.WriteLine(string.Format("Id: {0}, Android: {1}, iOS: {2}", one.MessageId, one.AndroidDeliveredCount, one.ApplePushNotificationDeliveredCount));
            //    }
            //}
        }
    }
}

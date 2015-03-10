using CalculateCommon;
using SuperManBusinessLogic.C_Logic;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperManBusinessLogic.Group_Logic;

namespace SuperManBusinessLogic.Order_Logic
{
    public class OrderModelTranslator : TranslatorBase<order, OrderModel>
    {
        public static readonly OrderModelTranslator Instance = new OrderModelTranslator();
        public override OrderModel Translate(order from)
        {
            OrderModel to = new OrderModel();
            to.Id = from.Id;
            to.OrderNo = from.OrderNo;
            to.BusinessModel = ConventValue(from.business);
            if (from.clienterId != null)
            {
                to.ClienterName = ClienterLogic.clienterLogic().GetClienterById(from.clienterId.Value).TrueName;
                to.ClienterPhoneNo = ClienterLogic.clienterLogic().GetClienterById(from.clienterId.Value).PhoneNo;
            }                
            to.PickUpAddress = from.PickUpAddress;
            if (from.PubDate.HasValue)
            {
                to.PubDate = from.PubDate.Value.ToString();
            } 
            to.ReceviceName = from.ReceviceName;
            to.RecevicePhoneNo = from.RecevicePhoneNo;
            to.ReceviceAddress = from.ReceviceAddress;
            to.ActualDoneDate = from.ActualDoneDate;
            to.IsPay = from.IsPay;
            to.Amount = from.Amount;
            to.OrderCommission = from.OrderCommission;
            to.DistribSubsidy = from.DistribSubsidy;
            to.WebsiteSubsidy = from.WebsiteSubsidy;
            to.Remark = from.Remark;
            to.Status = from.Status;
            
            to.OriginalOrderNo = from.OriginalOrderNo;//原平台订单号
            to.OriginalOrderId = from.OriginalOrderId;//原平台订单id
            if (from.business != null) {
                to.GroupId = from.business.GroupId;
                if (to.GroupId != null) //当前商户有集团信息
                    to.GroupName = GroupLogic.groupLogic().GetGroupName(Convert.ToInt32(to.GroupId));
            }
            if (from.business != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
            {
                var d1 = new Degree(from.business.Longitude.Value, from.business.Latitude.Value);
                var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                to.distanceB2R = CoordDispose.GetDistanceGoogle(d1, d2);
            }
            else
                to.distanceB2R = 0;
            return to;
        }
        public override order Translate(OrderModel from)
        {
            //var to = new order();
            //to.Id = from.Id;
            //to.businessId = from.BusinessModel.Id;
            //to.clienterId = from.ClienterModel.Id;
            //to.PickUpAddress = from.PickUpAddress;
            //to.PubDate = from.PubDate;
            //to.ReceviceName = from.ReceviceName;
            //to.RecevicePhoneNo = from.RecevicePhoneNo;
            //to.ReceviceAddress = from.ReceviceAddress;
            //to.ActualDoneDate = from.ActualDoneDate;
            //to.IsPay = from.IsPay;
            //to.Amount = from.Amount;
            //to.OrderCommission = from.OrderCommission;
            //to.DistribSubsidy = from.DistribSubsidy;
            //to.WebsiteSubsidy = from.WebsiteSubsidy;
            //to.Remark = from.Remark;
            //to.Status = from.Status;
            //return to;
            return null;
        }

        private BusinessModel ConventValue(business t)
        {
            var item = new BusinessModel();
            item.Id = t.Id;
            item.Name = t.Name;
            item.City = t.City;
            item.PhoneNo = t.PhoneNo;
            item.Password = t.Password;
            item.CheckPicUrl = t.CheckPicUrl;
            item.Address = t.Address;
            item.Landline = t.Landline;
            item.Longitude = t.Longitude;
            item.Latitude = t.Latitude;
            return item;
        }
        private ClienterModel ConventValue(clienter t)
        {
            var item = new ClienterModel();
            item.Id = t.Id;
            item.PhoneNo = t.PhoneNo;
            item.LoginName = t.LoginName;
            item.Password = t.Password;
            item.TrueName = t.TrueName;
            item.IDCard = t.IDCard;
            item.PicWithHandUrl = t.PicWithHandUrl;
            item.PicUrl = t.PicUrl;
            item.Status = t.Status;
            return item;
        }
    }
}

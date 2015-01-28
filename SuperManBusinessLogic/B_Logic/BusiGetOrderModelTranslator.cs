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

namespace SuperManBusinessLogic.B_Logic
{
    public class BusiGetOrderModelTranslator : TranslatorBase<order, BusiGetOrderModel>
    {
        public static readonly BusiGetOrderModelTranslator Instance = new BusiGetOrderModelTranslator();
        public override BusiGetOrderModel Translate(order from)
        {
            var model = new BusiGetOrderModel();
            if(from.ActualDoneDate!=null)
            {
                model.ActualDoneDate = from.ActualDoneDate.Value.ToShortDateString() + " " + from.ActualDoneDate.Value.ToShortTimeString();
            }
            model.Amount = from.Amount;
            model.IsPay = from.IsPay;
            model.OrderNo = from.OrderNo;
            model.PickUpAddress = from.PickUpAddress;            
            if (from.PubDate != null)
            {
                model.PubDate = from.PubDate.Value.ToShortDateString() + " " + from.PubDate.Value.ToShortTimeString();
            }
            business _business = null;
            if (from.businessId != null)
            {
                _business = BusiLogic.busiLogic().GetBusinessById(from.businessId.Value);
            }
            if (_business != null)
            {
                model.PickUpName = _business.Name;
            }
            model.ReceviceAddress = from.ReceviceAddress;
            model.ReceviceName = from.ReceviceName;
            model.RecevicePhoneNo = from.RecevicePhoneNo;
            if (from.Remark == null)
            {
                model.Remark = "";
            }
            else
            {
                model.Remark = from.Remark;
            }
            model.Status = from.Status;
            if(from.clienterId!=null)
            {
                var clienter = ClienterLogic.clienterLogic().GetClienterById(from.clienterId.Value);
                model.superManName = clienter.TrueName == null ? string.Empty : clienter.TrueName;
                model.superManPhone = clienter.PhoneNo == null ? string.Empty : clienter.PhoneNo;
            }            
            if (_business != null && from.ReceviceLongitude != null && from.ReceviceLatitude != null)
            {
                var d1 = new Degree(_business.Longitude.Value, _business.Latitude.Value);
                var d2 = new Degree(from.ReceviceLongitude.Value, from.ReceviceLatitude.Value);
                model.distanceB2R = CoordDispose.GetDistanceGoogle(d1, d2);
            }
            else
                model.distanceB2R = 0;
            return model;
        }

        public override order Translate(BusiGetOrderModel from)
        {
            throw new NotImplementedException();
        }
    }
}

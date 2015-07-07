using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Clienter;
using Ets.Dao.DeliveryCompany;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DomainModel.Area;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Model.ParameterModel.DeliveryCompany;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.DeliveryCompany;
using Ets.Service.Provider.Common;

namespace Ets.Service.Provider.DeliveryCompany
{
    /// <summary>
    /// 物流公司业务逻辑
    /// </summary>
    public class DeliveryCompanyProvider : IDeliveryCompanyProvider
    {
        readonly DeliveryCompanyDao dao = new DeliveryCompanyDao();
        private ClienterDao clienterDao = new ClienterDao();
        /// <summary>
        /// 获取物流公司列表
        /// danny-20150706
        /// </summary>
        /// <returns></returns>
        public IList<DeliveryCompanyModel> GetDeliveryCompanyList()
        {
            return dao.GetDeliveryCompanyList();
        }

        /// <summary>
        /// 物流公司批量导入骑士  add by caoheyang 20150707
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public ResultModel<string> DoBatchImportClienter(DoBatchImportClienterPM model)
        {
            int insertCount = 0; //成功数量
            int updateCount = 0; //更新数量
            int errorCount = 0; //失败数量
             List<string> errorPhones = new List<string>(); //失败数量
             foreach (BatchImportClienterExcelDM temp in model.Datas)
            {
                ClienterModel clienterInfo= clienterDao.GetUserInfoByUserPhoneNo(temp.Phone);
                if (clienterInfo == null) //骑士尚未存在，做insert操作。
                {
                    
                    int id = clienterDao.DeliveryCompanyInsertClienter(new clienter()
                    {
                        PhoneNo = temp.Phone, //真实姓名
                        Password = temp.Phone, //手机号
                        TrueName = temp.Name, //真实姓名
                        IDCard = temp.IdCard, //身份证号
                        Status =(byte) ClienteStatus.Status2.GetHashCode(),
                        City = temp.City, //城市
                        CityId = temp.CityCode,
                        CityCode = temp.CityCode, //城市编码
                        DeliveryCompanyId = model.CompanyId //物流公司id
                    },model.OptId,model.OptName);
                   insertCount = id > 0 ? (insertCount + 1) : insertCount; //新增数量+1
                }
                else
                {
                    if (clienterInfo.TrueName == temp.Name && clienterInfo.IDCard == temp.IdCard) //做更新操作
                    {
                        updateCount = updateCount + clienterDao.BindDeliveryCompany(clienterInfo.Id, model.CompanyId, model.OptId, model.OptName); // 成功  更新数量+1
                    }
                    else
                    {
                        errorCount = errorCount+ 1; //失败数量+1
                        errorPhones.Add(temp.Phone);
                    }
                }
            }
            string message = string.Format(@"新增成功骑士:<font style='color:red'>
{0}</font>人，更新成功骑士<font style='color:red'>{1}</font>人，更新失败骑士<font style='color:red'>{2}</font>人。<br/>", insertCount, updateCount, errorCount);
            if (errorPhones.Count > 0)
            {
                message = message + "更新失败骑士的手机号码为:" + string.Join(",",errorPhones);
            }
            return ResultModel<string>.Conclude(SystemState.Success,message);
        }
    }
}

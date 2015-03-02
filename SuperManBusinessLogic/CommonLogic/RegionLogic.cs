using SuperManCommonModel;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManBusinessLogic.CommonLogic
{
    /// <summary>
    /// 省市级联业务逻辑类 add by caoheyang 20150302
    /// </summary>
    public class RegionLogic
    {
        private volatile static RegionLogic _instance = null;
        private static readonly object lockHelper = new object();
        private RegionLogic() { }

        public static RegionLogic regionLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new RegionLogic();
                }
            }
            return _instance;
        }


        /// <summary>
        /// 根据父级id获取城市信息，默认FID为0，代表获取所有省   add by caoheyang 20150302
        /// </summary>
        /// <param name="Fid">父级Id</param>
        /// <returns></returns>
        public IList<region> GetRegionsByFid(int Fid = ConstValues.Fid1)
        {
            using (var db = new supermanEntities())
            {
                List<region> regions = db.region.Where(p => p.Fid == Fid).ToList();
                return regions;
            }
        }

        /// <summary>
        /// 根据父级Code获取城市信息，默认Code为0，代表获取所有省   add by caoheyang 20150302
        /// </summary>
        /// <param name="Fid">父级Id</param>
        /// <returns></returns>
        public IList<region> GetRegionsByCode(string code = ConstValues.Code1)
        {
            using (var db = new supermanEntities())
            {
                region temp = db.region.First(item => item.Code == code);
                if (temp == null)
                    return new List<region>();
                else
                    return db.region.Where(p => p.Fid == temp.Id).ToList();
            }
        }
    }
}

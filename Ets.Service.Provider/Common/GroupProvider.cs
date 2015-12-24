﻿using System;
using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.DomainModel.Group;
using Ets.Model.ParameterModel.Group;
using Ets.Service.IProvider.Common;
using ETS.Util;
using ETS.Data.PageData;

namespace Ets.Service.Provider.Common
{
    /// <summary>
    /// 集团api配置业务层-平扬 2015.3.16
    /// </summary>
    public class GroupProvider:IGroupProvider
    {
        /// <summary>
        /// 数据访问类
        /// </summary>
        readonly GroupDao _dao=new GroupDao();

        /// <summary>
        /// 获取集团分页列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public PageInfo<GroupApiConfigModel> GetGroupList(GroupParaModel criteria)
        {
            PageInfo<GroupApiConfigModel> pageinfo = _dao.GetGroupList<GroupApiConfigModel>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 创建集团Api配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public ResultInfo<bool> CreateGroupApiConfig(GroupApiConfigModel config)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                config.AppSecret = Helper.Uuid().ToUpper();
                result.Data = _dao.CreateGroupApiConfig(config);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 修改集团api配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateGroupApiConfig(GroupApiConfigModel config)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = _dao.UpdateGroupApiConfig(config);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 修改集团名称信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateGroupName(GroupModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = _dao.UpdateGroupName(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 修改集团状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultInfo<bool> UpdateGroupStatus(GroupModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = _dao.UpdateGroupStatus(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }

        /// <summary>
        /// 根据appkey和版本查询配置信息
        /// </summary>
        /// <param name="appkey">appkey</param>
        /// <param name="version">版本号码</param>
        /// <returns></returns>
        public GroupApiConfigModel GetGroupApiConfigByAppKey(string appkey, string version)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            redis.Delete(appkey + version);
            GroupApiConfigModel model= redis.Get<GroupApiConfigModel>(appkey + version);
            if (model == null)
            {
                GroupApiConfigModel tempmodel = _dao.GetGroupApiConfigByAppKey(appkey, version);
                if (tempmodel == null)
                    return null;
                else
                {
                    redis.Set(appkey + version, tempmodel);
                    return redis.Get<GroupApiConfigModel>(appkey + version);
                }
            }
            else
                return model;
        }
        
 
        /// <summary>
        /// 判断集团是否已经存在
        /// </summary>
        /// <param name="config"></param>
        /// <returns>配置id</returns>
        public ResultInfo<bool> HasExistsGroup(GroupModel model)
        {
            var result = new ResultInfo<bool> { Message = "", Result = false, Data = false };
            try
            {
                result.Data = _dao.HasExistsGroup(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
        /// <summary>
        /// 获取集团名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetGroupName(int id)
        {
            return _dao.GetGroupName(id);
        }

        /// <summary>
        /// 创建集团
        /// </summary>
        /// <param name="config"></param>
        /// <returns>配置id</returns>
        public ResultInfo<int> AddGroup(GroupModel model)
        {
            var result = new ResultInfo<int> { Message = "", Result = false, Data = 0 };
            try
            {
                result.Data = _dao.AddGroup(model);
                result.Message = "执行成功";
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Result = false;
                LogHelper.LogWriterFromFilter(ex);
            }
            return result;
        }
    }
}

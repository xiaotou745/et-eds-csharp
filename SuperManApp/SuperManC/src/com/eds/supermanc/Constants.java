package com.eds.supermanc;

public class Constants {
    /**
     * 
     */
    public static final boolean MODE_DEBUG = true;// 是否为debug模式
    /**
     * 日志
     */
    public static final boolean LOAD_LOG_MSG = true;// 本地保存日志
    public static final String TIME_FROMAT_LOG = "yyyy-MM-dd HH:mm:ss";

    public static final String CODE_TYPE = "utf-8";

    // private static final String HOST = "http://wuliuapi.4008107107.com";// 海底捞线上
    private static final String HOST = "http://59.151.43.221:7178";// 海底捞线下
    public static final String GET_LATEST_MISSION_URL = HOST + "/ClienterAPI/GetJobListNoLoginLatest_C";// 最新任务
    public static final String GET_NEAR_MISSION_URL = HOST + "/ClienterAPI/GetJobList_C";// 最近任务
    public static final String LOGIN_URL = HOST + "/ClienterAPI/PostLogin_C";
    public static final String GET_MY_MISSION_URL = HOST + "/ClienterAPI/GetMyJobList_C";
    public static final String REGISTER_URL = HOST + "/ClienterAPI/PostRegisterInfo_C";
    public static final String UPDATE_PASSWORD_URL = HOST + "/ClienterAPI/PostModifyPwd_C";
    public static final String RUSH_ORDER_URL = HOST + "/ClienterAPI/RushOrder_C";
    public static final String FINISH_ORDER_URL = HOST + "/ClienterAPI/FinishOrder_C";
    public static final String GET_MY_BALANCE_URL = HOST + "/ClienterAPI/GetMyBalance";
    public static final String FORGET_PASSWORD_URL = HOST + "/ClienterAPI/PostForgetPwd_C";
    public static final String GET_MY_BALANCE_DYNAMIC = HOST + "/ClienterAPI/GetMyBalanceDynamic";
    public static final String UPLOAD_FILES = HOST + "/ClienterAPI/PostAudit_C";
    public static final String GET_CHECK_CODE = HOST + "/ClienterAPI/CheckCode";
    public static final String GET_MESS_MISSION_URL = HOST + "/ClienterAPI/GetJobListCanHeTask_C";// 获取餐盒订单
    public static final String GET_SEND_MISSION_URL = HOST + "/ClienterAPI/GetJobListSongCanTask_C";// 送餐任务

    public static final int GETLATESTMISSION = 0;// 最新任务
    public static final int GETNEARMISSION = 1;// 附近任务
    public static final int LOGIN = 2;
    public static final int GETMYMISSION = 3;
    public static final int REGISTER = 4;
    public static final int UPDATEPASSWORD = 5;
    public static final int RUSHORDER = 6;
    public static final int FINISHORDER = 7;
    public static final int GETMYBALANCE = 8;
    public static final int FORGET_PASSWORD = 9;
    public static final int GETMYBALANCEDYNAMIC = 10;
    public static final int GETMESSMISSION = 11;// 餐盒任务
    public static final int GETSENDMISSION = 12;// 送餐任务

    // 用户所属区域
    public static final String USER_CITY_NAME_TAG = "USER_CITY_NAME_TAG";// 城市名字
    public static final String USER_CITY_ID_TAG = "USER_CITY_ID_TAG";// 城市id
    public static final int DEFAULT_REQUESET_CODE = 10001;// 默认请求
    public static final int DEFAULT_RESULT_CODE = 10002;// 默认请求
    public static final int SET_JPUSH_TAG_RESULT_CODE = 10003;

    // 提现电话
    public static final String CASH_TEL = "";
    // 推动标签
    public static final String[] TAGS_ARR = { "海底捞" };

}

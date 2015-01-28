package com.supermanc;

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
	
	private static final String HOST = "http://api.edaisong.com.cn";
	public static final String GET_LATEST_MISSION_URL = HOST+"/ClienterAPI/GetJobListNoLoginLatest_C";
	public static final String GET_NEAR_MISSION_URL = HOST+"/ClienterAPI/GetJobList_C";
	public static final String LOGIN_URL = HOST+"/ClienterAPI/PostLogin_C";
	public static final String GET_MY_MISSION_URL = HOST+"/ClienterAPI/GetMyJobList_C";
	public static final String REGISTER_URL = HOST+"/ClienterAPI/PostRegisterInfo_C";
	public static final String UPDATE_PASSWORD_URL = HOST+"/ClienterAPI/PostModifyPwd_C";
	public static final String RUSH_ORDER_URL = HOST+"/ClienterAPI/RushOrder_C";
	public static final String FINISH_ORDER_URL = HOST+"/ClienterAPI/FinishOrder_C";
	public static final String GET_MY_BALANCE_URL = HOST+"/ClienterAPI/GetMyBalance";
	public static final String FORGET_PASSWORD_URL = HOST+"/ClienterAPI/PostForgetPwd_C";
	public static final String GET_MY_BALANCE_DYNAMIC = HOST+"/ClienterAPI/GetMyBalanceDynamic";
	public static final String UPLOAD_FILES = HOST+"/ClienterAPI/PostAudit_C";
	public static final String GET_CHECK_CODE = HOST+"/ClienterAPI/CheckCode";
	
	public static final int GETLATESTMISSION = 0;
	public static final int GETNEARMISSION = 1;
	public static final int LOGIN = 2;
	public static final int GETMYMISSION = 3;
	public static final int REGISTER = 4;
	public static final int	UPDATEPASSWORD = 5;
	public static final int RUSHORDER = 6;
	public static final int FINISHORDER = 7;
	public static final int GETMYBALANCE = 8;
	public static final int FORGET_PASSWORD = 9;
	public static final int GETMYBALANCEDYNAMIC = 10;
	
}

package com.eds.supermanb.constant;

public class Constants {
    /**
     * 模式
     */
    public static final boolean MODE_DEBUG = true;// 是否为debug模式
    /**
     * 日志
     */
    public static final boolean LOAD_LOG_MSG = true;// 本地保存日志
    public static final String TIME_FROMAT_LOG = "yyyy-MM-dd HH:mm:ss";

    /*
     * OrderStatus 1 已完成 0 待抢单 2 已接单 3 已取消
     */
    // public static final String host = "http://api.edaisong.com.cn/BusinessAPI/";// 线上服务器
    public static final String host = "http://10.8.7.40:7178/BusinessAPI/";// 开发服务器

    public static final String URL_PostRegisterInfo = "PostRegisterInfo_B";

    public static final String URL_PostLogin_B = "PostLogin_B";

    public static final String URL_PostGetVerifyCode_B = "CheckCode";
    public static final String URL_CHANGE_PASSWORD_GetVerifyCode_B = "CheckCodeFindPwd";
    public static final String URL_POST_CHANGE_PASSWORD = "PostForgetPwd_B";

    public static final String URL_ADD_ADDRESS = "PostManagerAddress_B";
    public static final String URL_CANCEL_ORDER = "CancelOrder_B";
    public static final String URL_REGIST = "PostRegisterInfo_B";
    public static final String URL_LOGIN = "PostLogin_B";
    public static final String URL_AUDIT = "PostAudit_B";
    public static final String URL_GET_ORDER_LIST = "GetOrderList_B";
    public static final String URL_POST_PUBLISH_ORDER = "PostPublishOrder_B";
    public static final String URL_GET_ORDER_SATISTICS = "OrderCount_B";
    public static int ORDER_FINISH = 1;
    public static int ORDER_NOT_RECIVED = 0;
    public static int ORDER_RECIVED = 2;
    public static final String CODE_TYPE = "utf-8";

    // 客服电话
    public static final String CUSTOM_TEL = "";

}

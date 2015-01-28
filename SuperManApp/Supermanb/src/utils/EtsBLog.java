package utils;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import android.util.Log;
import constant.Constants;

/**
 * 日志管理
 * 
 * @author kangzhen
 * 
 *         2014-4-11
 */
public class EtsBLog {

    public static final String S_LOGTAG = "ets_B";
    public static final boolean B_DEBUG = Constants.MODE_DEBUG; // 是否输出日志

    public static void v(String msg) {
        if (B_DEBUG) {
            android.util.Log.v(S_LOGTAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void v(String TAG, String msg) {
        if (B_DEBUG) {
            android.util.Log.v(TAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void e(String msg) {
        if (B_DEBUG) {
            android.util.Log.e(S_LOGTAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void e(String TAG, String msg) {
        if (B_DEBUG) {
            android.util.Log.e(TAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void i(String msg) {
        if (B_DEBUG) {
            Calendar calendar = Calendar.getInstance();
            Date date = calendar.getTime();
            SimpleDateFormat df = new SimpleDateFormat(Constants.TIME_FROMAT_LOG);
            String buf = df.format(date);
            msg = buf.concat("||" + msg);
            android.util.Log.i(S_LOGTAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void i(String TAG, String msg) {
        if (B_DEBUG) {
            msg = Thread.currentThread().getName().concat("::" + msg);
            Calendar calendar = Calendar.getInstance();
            Date date = calendar.getTime();
            SimpleDateFormat df = new SimpleDateFormat(Constants.TIME_FROMAT_LOG);
            String buf = df.format(date);
            msg = buf.concat("||" + msg);
            android.util.Log.i(TAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void d(String msg) {
        if (B_DEBUG) {
            android.util.Log.d(S_LOGTAG, msg);
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

    public static void d(String TAG, String msg) {
        if (B_DEBUG) {
            if (msg == null) {
                Log.d(TAG, "null");
            } else {
                Log.d(TAG, msg);
            }
            EtsBLogHelper.writeLogInfo(msg);
        }
    }

}

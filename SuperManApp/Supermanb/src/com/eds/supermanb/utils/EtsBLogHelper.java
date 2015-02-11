package com.eds.supermanb.utils;

import java.io.File;
import java.io.FileOutputStream;

import com.eds.supermanb.constant.Constants;

import android.os.Environment;

/**
 * 日志帮助类
 * 
 * @author kangzhen
 * 
 *         2014-4-11
 */
public class EtsBLogHelper {

    public String logName = "EtsBLog.txt";// 文件名字
    public String logPath = "EtsB";
    public String logLocation = "";// 文件路径

    /**
     * 获取sd卡路径
     */
    public String isHasLogFile() {
        return Environment.getExternalStorageDirectory().getAbsolutePath();
    }

    /**
     * 向sd卡写日志信息
     */
    public void writeLogInfoTOSDk(String str) {
        if (StringUtil.isSdcardExist() && !"".equals(str)) {
            try {
                // 有sd卡
                String strPath = isHasLogFile() + File.separator + logPath;
                File filePath = new File(strPath);
                if (!filePath.exists()) {
                    // 不存在目录
                    filePath.mkdirs();
                }
                File mFile = new File(strPath, logName);
                FileOutputStream mFilepStream;
                if (mFile.exists()) {
                    // 存在文件，已经追加的方式打开
                    mFilepStream = new FileOutputStream(mFile, true);
                } else {
                    // 文件不存在，直接打开
                    mFilepStream = new FileOutputStream(mFile);
                }
                byte[] mByte = str.getBytes();
                mFilepStream.write(mByte);
                mFilepStream.write("\r\n".getBytes());
                mFilepStream.flush();
                mFilepStream.close();
            } catch (Exception e) {
                e.getMessage();
            }

        }
    }

    public synchronized static void writeLogInfo(String str) {
        if (Constants.LOAD_LOG_MSG) {
            WriteTread mWriteThrad = new WriteTread(str);
            mWriteThrad.start();
        }
    }

    /**
     * 写数据线程
     * 
     * @author kangzhen
     * 
     *         2014-4-11
     */
    static class WriteTread extends Thread {
        String strLog;

        WriteTread(String str) {
            strLog = str;
        }

        @Override
        public void run() {
            EtsBLogHelper mHelp = new EtsBLogHelper();
            mHelp.writeLogInfoTOSDk(strLog);
        }

    }
}

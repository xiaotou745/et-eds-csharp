package com.eds.supermanc.utils;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class MD5 {
    private final static String KEY = "etsapi2012611bhl";
    // 十六进制下数字到字符的映射数组
    private final static String[] hexDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D",
            "E", "F" };

    /**
     * 把inputString加密
     */
    public static String md5(String inputStr) {
        return encodeByMD5(KEY + inputStr);
    }

    public static String md5NoKey(String inputStr) {
        return encodeByMD5(inputStr);
    }

    /**
     * 验证输入的密码是否正确
     * 
     * @param password
     *            真正的密码（加密后的真密码）
     * @param inputString
     *            输入的字符串
     * @return 验证结果，boolean类型
     */
    public static boolean authenticatePassword(String password, String inputString) {

        if (password.equals(encodeByMD5(inputString))) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * 对字符串进行MD5编码
     */
    private static String encodeByMD5(String originString) {
        if (originString != null) {
            try {
                // 创建具有指定算法名称的信息摘要
                MessageDigest md5 = MessageDigest.getInstance("MD5");
                // 使用指定的字节数组对摘要进行最后更新，然后完成摘要计算
                byte[] results = md5.digest(originString.getBytes("UTF-8"));
                // 将得到的字节数组变成字符串返回
                String result = byteArrayToHexString(results);
                return result;
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
        return null;
    }

    /**
     * 轮换字节数组为十六进制字符串
     * 
     * @param b
     *            字节数组
     * @return 十六进制字符串
     */
    private static String byteArrayToHexString(byte[] b) {
        StringBuffer resultSb = new StringBuffer();
        for (int i = 0; i < b.length; i++) {
            resultSb.append(byteToHexString(b[i]));
        }
        return resultSb.toString();
    }

    // 将一个字节转化成十六进制形式的字符串
    private static String byteToHexString(byte b) {
        int n = b;
        if (n < 0) {
            n = 256 + n;
        }
        int d1 = n / 16;
        int d2 = n % 16;
        return hexDigits[d1] + hexDigits[d2];
    }

    /**
     * MD5加密，32位
     */
    public static byte[] encode(byte[] data) {
        byte[] encoded = null;
        try {
            MessageDigest md = MessageDigest.getInstance("MD5");
            encoded = md.digest(data);
        } catch (NoSuchAlgorithmException e) {
            EtsCLog.e(e.toString());
        }
        return encoded;
    }

    public static String encodeToString(String str) {
        byte[] encoded = encode(str.getBytes());
        if (null == encoded) {
            return null;
        } else {
            StringBuffer md5StrBuff = new StringBuffer();
            for (int i = 0; i < encoded.length; i++) {
                String hex = Integer.toHexString(0xFF & encoded[i]);
                if (hex.length() == 1) {
                    md5StrBuff.append('0').append(hex);
                } else {
                    md5StrBuff.append(hex);
                }
            }
            return md5StrBuff.toString();
        }
    }

    public static byte[] encode(File f) {
        FileInputStream fis = null;
        byte[] encoded = null;
        try {
            fis = new FileInputStream(f);
            MessageDigest md = MessageDigest.getInstance("MD5");
            byte[] buffer = new byte[1024];
            int numRead = 0;
            while ((numRead = fis.read(buffer)) > 0) {
                md.update(buffer, 0, numRead);
            }
            encoded = md.digest();
        } catch (Exception e) {
            EtsCLog.e(e.toString());
        } finally {
            if (fis != null) {
                try {
                    fis.close();
                } catch (IOException e) {
                    EtsCLog.e(e.toString());
                }
                fis = null;
            }
        }
        return encoded;
    }

    public static String encodeToString(File f) {
        byte[] encoded = encode(f);
        if (null == encoded) {
            return null;
        } else {
            StringBuffer md5StrBuff = new StringBuffer();
            for (int i = 0; i < encoded.length; i++) {
                String hex = Integer.toHexString(0xFF & encoded[i]);
                if (hex.length() == 1) {
                    md5StrBuff.append('0').append(hex);
                } else {
                    md5StrBuff.append(hex);
                }
            }
            return md5StrBuff.toString();
        }
    }

}
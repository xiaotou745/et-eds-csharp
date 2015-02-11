package com.eds.supermanb.utils;

import java.util.ArrayList;
import java.util.HashMap;

import com.eds.supermanb.picker.CityMode;

import android.os.Environment;
import android.text.TextUtils;

public class StringUtil {
    public static boolean isMobileNO(String mobiles) {
        /*
         * 移动：134、135、136、137、138、139、150、151、157(TD)、158、159、187、188 联通：130、131、132、152、155、156、185、186
         * 电信：133、153、180、189、（1349卫通） 总结起来就是第一位必定为1，第二位必定为3或5或8，其他位置的可以为0-9
         */
        String telRegex = "[1][34578]\\d{9}";// "[1]"代表第1位为数字1，"[358]"代表第二位可以为3、5、8中的一个，"\\d{9}"代表后面是可以是0～9的数字，有9位。
        if (TextUtils.isEmpty(mobiles))
            return false;
        else
            return mobiles.matches(telRegex);
    }

    /**
     * 开放城市过滤
     * 
     * @param originList
     *            原城市
     * @param filterStr
     *            过滤条件
     * @return 符合条件
     */
    public static ArrayList<CityMode> filterCityByName(ArrayList<CityMode> originList, String[] filterStr) {
        ArrayList<CityMode> filterList = new ArrayList<CityMode>();
        if (filterStr == null || filterStr.length == 0) {
            return originList;
        } else {
            try {
                HashMap<String, CityMode> mapCity = new HashMap<String, CityMode>();
                int isize = originList.size();
                for (int i = 0; i < isize; i++) {
                    CityMode bean = originList.get(i);
                    byte[] bKey = bean.getName().toString().trim().getBytes("UTF-8");
                    String strkey = new String(bKey, "UTF-8");
                    mapCity.put(strkey, bean);
                }
                int strLeng = filterStr.length;
                for (int j = 0; j < strLeng; j++) {
                    byte[] bkey = filterStr[j].toString().trim().getBytes("UTF-8");
                    String skey = new String(bkey, "UTF-8");
                    if (mapCity.containsKey(skey)) {
                        CityMode fBean = mapCity.get(skey);
                        if (fBean != null) {
                            filterList.add(fBean);
                        }
                    }
                }
            } catch (Exception e) {

            }
            return filterList;
        }
    }

    /**
     * 判断是否存在SD卡
     * 
     * @return
     */
    public static boolean isSdcardExist() {
        return Environment.MEDIA_MOUNTED.equals(Environment.getExternalStorageState()) ? true : false;
    }

    // 字符串是否为空
    public static boolean isNullByString(String str) {
        if (str == null || "".equals(str) || "null".equals(str)) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * 将空字符串装换为默认值
     * 
     * @param str
     * @param defaultValue
     * @return
     */
    public static String nullToDefaultValue(String str, String defaultValue) {
        if (str == null || "".equals(str) || "null".equals(str)) {
            return defaultValue;
        } else {
            return str;
        }
    }

    /**
     * 获取指定的区域
     * 
     * @param data
     * @param key
     * @return
     */
    public static int getIndexOfListByKey(ArrayList<CityMode> data, String key) {
        int iIndex = 0;
        if (!"".equals(key) && data != null && data.size() > 0) {
            int isize = data.size();
            for (int i = 0; i < isize; i++) {
                String strName = data.get(i).getName();
                if (key.equals(strName)) {
                    iIndex = i;
                    break;
                }
            }
        }
        return iIndex;
    }
}

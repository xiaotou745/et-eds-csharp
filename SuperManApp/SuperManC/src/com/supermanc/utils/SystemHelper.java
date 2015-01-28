package com.supermanc.utils;

import android.content.Context;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager.NameNotFoundException;

public class SystemHelper {

	public static String getVersionName(Context context){
		try {  
	        PackageInfo pi = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);  
	        return pi.versionName;  
	    } catch (NameNotFoundException e) {  
	        e.printStackTrace();  
	        return "";  
	    } 
	}
	
	public static int getVersionCode(Context context){
		try {  
	        PackageInfo pi = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);  
	        return pi.versionCode;  
	    } catch (NameNotFoundException e) {  
	        e.printStackTrace();  
	        return 0;  
	    } 
	}
	
}

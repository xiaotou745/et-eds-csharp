package com.supermanc.utils;

import com.supermanc.beans.UserVo;
import com.supermanc.beans.UserVo.User;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;

public class UserTools {

	private static User user = null;

	private static final String USER_SHAREPERFERENCE_NAME = "user_shareperference";
	
	private static final String USER_ID = "user_id";
	private static final String USER_NAME = "user_name";
	private static final String PASSWORD = "password";
	private static final String STATUS = "status";
	private static final String AMOUNT = "amount";
	
	private static UserVo.User findUser(Context context) {
		if (user == null || "".equals(user.getUserId())) {
			SharedPreferences mShareUserData = context.getSharedPreferences(USER_SHAREPERFERENCE_NAME,
					Context.MODE_PRIVATE);
			UserVo userVo = new UserVo();
			user = userVo.new User();
			user.setUserId(mShareUserData.getString(USER_ID, ""));
			user.setUserName(mShareUserData.getString(USER_NAME, ""));
			user.setPassword(mShareUserData.getString(PASSWORD, ""));
			user.setStatus(mShareUserData.getInt(STATUS, 0));
			user.setAmount(mShareUserData.getString(AMOUNT, "0"));
		}
		return user;
	}

	public static void saveUser(Context context,UserVo muser) {
		SharedPreferences mShareUserData = context.getSharedPreferences(USER_SHAREPERFERENCE_NAME,
				Context.MODE_PRIVATE);
		Editor editor = mShareUserData.edit();
		editor.putString(USER_ID, muser.getResult().getUserId());
		editor.putString(USER_NAME, muser.getResult().getUserName());
		editor.putString(PASSWORD, muser.getResult().getPassword());
		editor.putInt(STATUS, muser.getResult().getStatus());
		editor.putString(AMOUNT, muser.getResult().getAmount());
		editor.commit();
	}
	
	public static UserVo.User getUser(Context context){
		User user = UserTools.findUser(context);
		if(user == null || "".equals(user.getUserId())){
			return null;
		}else{
			return user;
		}
	}
	
	public static void clear(Context context){
		SharedPreferences mShareUserData = context.getSharedPreferences(USER_SHAREPERFERENCE_NAME,
				Context.MODE_PRIVATE);
		if(user != null){
			user = null;
		}
		Editor editor = mShareUserData.edit();
		editor.clear();
		editor.commit();
	}

}

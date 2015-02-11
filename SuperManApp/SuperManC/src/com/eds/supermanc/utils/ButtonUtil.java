package com.eds.supermanc.utils;

import com.supermanc.R;

import android.app.Activity;
import android.widget.Button;

public class ButtonUtil {

	public static void setEnable(Button btn){
		btn.setEnabled(true);
		btn.setBackgroundResource(R.drawable.green_btn_selector);
	}
	
	public static void setDisabled(Button btn,Activity activity){
		btn.setEnabled(false);
		btn.setBackgroundColor(activity.getResources().getColor(R.color.gray_text_color));
	}
	
}

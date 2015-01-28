package com.supermanc.activity;

import android.content.Intent;
import android.content.SharedPreferences.Editor;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;
import cn.jpush.android.api.JPushInterface;

import com.supermanc.R;
import com.supermanc.utils.UserTools;
import com.umeng.update.UmengUpdateAgent;
import com.umeng.update.UmengUpdateListener;
import com.umeng.update.UpdateResponse;
import com.umeng.update.UpdateStatus;

public class SettingActivity extends BaseActivity implements OnClickListener{

	private LinearLayout aboutUs;
	private LinearLayout checkUpdate;
	private LinearLayout updatePassword;
	private Button loginout;
	private CheckBox isPush;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		setContentView(R.layout.setting_activity);
		initView();
		initListener();
	}

	private void initView(){
		initTitle();
		isPush = (CheckBox)findViewById(R.id.isPush);
		aboutUs = (LinearLayout)findViewById(R.id.aboutUs);
		checkUpdate = (LinearLayout)findViewById(R.id.checkNewVesion);
		updatePassword = (LinearLayout)findViewById(R.id.updatePassword);
		loginout = (Button)findViewById(R.id.loginout);
	}
	
	private void initTitle(){
		LinearLayout backLayout = (LinearLayout)this.findViewById(R.id.titleLeftLayout);
		backLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				SettingActivity.this.finish();
			}
		});
		((ImageView)this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
		((TextView)this.findViewById(R.id.titleContent)).setText("设置");
	}

	private void initListener(){
		aboutUs.setOnClickListener(this);
		checkUpdate.setOnClickListener(this);
		updatePassword.setOnClickListener(this);
		loginout.setOnClickListener(this);
		isPush.setOnCheckedChangeListener(new OnCheckedChangeListener() {
			
			@Override
			public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				if(!isChecked){
					isReceive = false;
					JPushInterface.stopPush(getApplicationContext());
				}else{
					isReceive = true;
					JPushInterface.resumePush(getApplicationContext());
				}
				Editor editor = preferences.edit();
				editor.putBoolean("isReceive",isReceive);
				editor.commit();
			}
		});
	}
	
	boolean isReceive = true;
	@Override
	public void onResume() {
		super.onResume();
		isReceive  = preferences.getBoolean("isReceive", true);
		isPush.setChecked(isReceive);       
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.aboutUs:
			
			break;
		case R.id.checkNewVesion:
			UmengUpdateAgent.update(this);
			UmengUpdateAgent.setUpdateAutoPopup(false);
			UmengUpdateAgent.setUpdateListener(new UmengUpdateListener() {
				
				@Override
				public void onUpdateReturned(int updateStatus, UpdateResponse updateInfo) {
					// TODO Auto-generated method stub
			        switch (updateStatus) {
			        case UpdateStatus.Yes: // has update
			            UmengUpdateAgent.showUpdateDialog(SettingActivity.this, updateInfo);
			            break;
			        case UpdateStatus.No: // has no update
			            Toast.makeText(SettingActivity.this, "当前已经是最新版本", Toast.LENGTH_SHORT).show();
			            break;
			        case UpdateStatus.NoneWifi: // none wifi
			            Toast.makeText(SettingActivity.this, "没有wifi连接， 只在wifi下更新", Toast.LENGTH_SHORT).show();
			            break;
			        case UpdateStatus.Timeout: // time out
			            Toast.makeText(SettingActivity.this, "连接超时", Toast.LENGTH_SHORT).show();
			            break;
			        }
				}
			});
			break;
		case R.id.updatePassword:
			goOtherActivity(R.id.updatePassword);
			break;
		case R.id.loginout:
			UserTools.clear(this);
			JPushInterface.stopPush(getApplicationContext());
			this.finish();
			break;
		default:
			break;
		}
	}
	
	private void goOtherActivity(int id){
		Intent intent = new Intent();
		if(id == R.id.updatePassword){
			intent.setClass(this, UpdatePasswordActivity.class);
		}
		startActivity(intent);
	}

}

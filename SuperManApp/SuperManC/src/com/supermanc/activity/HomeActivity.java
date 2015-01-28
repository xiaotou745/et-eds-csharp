package com.supermanc.activity;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Map;

import android.app.ActionBar;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.util.DisplayMetrics;
import android.util.TypedValue;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewConfiguration;
import android.view.Window;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;
import cn.jpush.android.api.JPushInterface;

import com.android.volley.VolleyError;
import com.supermanc.Constants;
import com.supermanc.R;
import com.supermanc.beans.UserVo;
import com.supermanc.beans.UserVo.User;
import com.supermanc.fragments.LatestMissionFragment;
import com.supermanc.fragments.NearMissionFragment;
import com.supermanc.utils.UserTools;
import com.supermanc.utils.VolleyTool;
import com.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.views.PagerSlidingTabStrip;
import com.umeng.analytics.MobclickAgent;
import com.umeng.update.UmengDialogButtonListener;
import com.umeng.update.UmengUpdateAgent;
import com.umeng.update.UpdateStatus;

public class HomeActivity extends FragmentActivity implements OnClickListener,HTTPListener{

	private NearMissionFragment nearMissionFragment;
	private LatestMissionFragment latestMissionFragment;
	private PagerSlidingTabStrip tabs;
	private LinearLayout unLoginLayout;
	private LinearLayout loginLayout;
	private Button mIdValidate;
	private TextView mCheckStatusInfo;
	ProgressDialog dialog = null;
	/**
	 * 获取当前屏幕的密度
	 */
	private DisplayMetrics dm;

	private Button mLogin;
	private Button mRegister;
	private Button mRefresh;

	User user = null;
	public SharedPreferences preferences;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.home_activity);
		preferences = this.getSharedPreferences("supermancpush", Context.MODE_PRIVATE);
		latestMissionFragment = new LatestMissionFragment();
		nearMissionFragment = new NearMissionFragment();
		initView();
		UmengUpdateAgent.update(this);
		UmengUpdateAgent.setDialogListener(new UmengDialogButtonListener() {
			@Override
			public void onClick(int arg0) {
				if(arg0==UpdateStatus.NotNow){	
					UmengUpdateAgent.update(HomeActivity.this);
				}
			}
		});
		initActionBar();
		initListener();
//		init();
		// 是否审核通过不知道 所以在首页重新登录获取最新的状态
		login();
	}

	// 初始化 JPush。如果已经初始化，但没有登录成功，则执行重新登录。
	private void init(){
			if(user!=null ){
				//已登录
				boolean	isReceive  = preferences.getBoolean("isReceive", true);	
				if(isReceive){
					JPushInterface.resumePush(getApplicationContext());
				}else{
					JPushInterface.stopPush(getApplicationContext());
				}
			}else{
				//没有登录
				JPushInterface.resumePush(getApplicationContext());
			}
	}

	private void initView(){
		mLogin = (Button)findViewById(R.id.btnLogin);
		mRegister = (Button)findViewById(R.id.btnRegister);
		mRefresh = (Button)findViewById(R.id.refresh);
		unLoginLayout= (LinearLayout)findViewById(R.id.unLoginLayout);
		loginLayout= (LinearLayout)findViewById(R.id.loginedLayout);
		mIdValidate = (Button)findViewById(R.id.idValidate);
		mCheckStatusInfo = (TextView)findViewById(R.id.checkStatusInfo);
	}

	private void initActionBar(){
		getOverflowMenu();
		dm = getResources().getDisplayMetrics();
		ViewPager pager = (ViewPager) findViewById(R.id.pager);
		tabs = (PagerSlidingTabStrip) findViewById(R.id.tabs);
		pager.setAdapter(new ViewPagerAdapter(getSupportFragmentManager()));
		tabs.setViewPager(pager);
		setTabsValue();
		ActionBar actionBar = this.getActionBar();
		Resources resources = this.getResources();
		Drawable drawable = resources.getDrawable(R.drawable.title_bg);
		actionBar.setBackgroundDrawable(drawable);
	}

	private void initListener(){
		mLogin.setOnClickListener(this);
		mRegister.setOnClickListener(this);
		mRefresh.setOnClickListener(this);
		mIdValidate.setOnClickListener(this);
	}

	private void login(){
		if(UserTools.getUser(this) != null && UserTools.getUser(this).getStatus() != 1){
			dialog = ProgressDialog.show(HomeActivity.this, "提示", "获取个人信息中...",false,false);
			Map<String,String> params = new HashMap<String,String>();
			params.put("phoneNo", UserTools.getUser(this).getUserName());
			params.put("passWord", UserTools.getUser(this).getPassword());
			VolleyTool.post(Constants.LOGIN_URL, params, this, Constants.LOGIN, UserVo.class);
		}
	}
	/**
	 * 展示用户信息
	 */
private void showUserInfo(){
	user = UserTools.getUser(this);
	mIdValidate.setVisibility(View.VISIBLE);
	if(user == null){
		unLoginLayout.setVisibility(View.VISIBLE);
		loginLayout.setVisibility(View.GONE);
	}else{
		if(user.getStatus() == 1){
			unLoginLayout.setVisibility(View.GONE);
			loginLayout.setVisibility(View.GONE);
		}else{
			if(user.getStatus() == 3){
				mCheckStatusInfo.setText("您已提交审核，请等待审核通过后再下单");
				mIdValidate.setVisibility(View.GONE);
			}else if(user.getStatus() == 0){
				mCheckStatusInfo.setText("您的审核信息尚有疑问，请致电客服或重新提交审核");
			}
			unLoginLayout.setVisibility(View.GONE);
			loginLayout.setVisibility(View.VISIBLE);
		}
	}
}
	@Override
	protected void onResume() {
		super.onResume();
		MobclickAgent.onResume(this);		
		JPushInterface.onResume(this);
		showUserInfo();	
		init();
	}

	@Override
	public boolean onPrepareOptionsMenu(Menu menu) {
		if(user == null){
			menu.findItem(R.id.id_user).setTitle("未登录");
			return super.onPrepareOptionsMenu(menu);
		}else{
			menu.findItem(R.id.id_user).setTitle(user.getUserName());
		}
		MenuItem checkItem = menu.findItem(R.id.id_check);
		if(user.getStatus() == 1){
			checkItem.setTitle("审核已通过");
		}else if(user.getStatus() == 0){
			checkItem.setTitle("审核被拒绝");
		}else if(user.getStatus() == 3){
			checkItem.setTitle("审核中");
		}else{
			checkItem.setTitle("身份验证");
		}
		return super.onPrepareOptionsMenu(menu);
	}

	@Override
	protected void onPause() {
		super.onPause();
		JPushInterface.onPause(this);
		MobclickAgent.onPause(this);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.btnRegister:
			goOtherActivity(R.id.btnRegister);
			break;
		case R.id.btnLogin:
			goOtherActivity(R.id.btnLogin);
			break;
		case R.id.refresh:
			nearMissionFragment.refresh();
			latestMissionFragment.refresh();
			break;
		case R.id.idValidate:
			goOtherActivity(R.id.idValidate);
			break;
		default:
			break;
		}
	}

	private void goOtherActivity(int id){
		Intent intent = new Intent();
		if(id == R.id.btnRegister){
			intent.setClass(this, RegisterActivity.class);
		}else if(id == R.id.idValidate){
			intent.setClass(this, VerificationActivity.class);
		}else{
			intent.setClass(this, LoginActivity.class);
		}
		startActivity(intent);
	}

	public class ViewPagerAdapter extends FragmentPagerAdapter {

		public ViewPagerAdapter(FragmentManager fm) {
			super(fm);
		}

		private final String[] titles = { "最新任务", "附近任务"};

		@Override
		public CharSequence getPageTitle(int position) {
			return titles[position];
		}

		@Override
		public int getCount() {
			return titles.length;
		}

		@Override
		public Fragment getItem(int position) {
			switch (position) {
			case 0:
				/*if (latestMissionFragment == null) {
					latestMissionFragment = new LatestMissionFragment();
				}*/
				return latestMissionFragment;
			case 1:
				/*if (nearMissionFragment == null) {
					nearMissionFragment = new NearMissionFragment();
				}*/
				return nearMissionFragment;
			default:
				return null;
			}
		}

	}

	/**
	 * 对PagerSlidingTabStrip的各项属性进行赋值。
	 */
	private void setTabsValue() {
		// 设置Tab是自动填充满屏幕的
		tabs.setShouldExpand(true);
		// 设置Tab的分割线是透明的
		tabs.setDividerColor(Color.TRANSPARENT);
		// 设置Tab底部线的高度
		tabs.setUnderlineHeight((int) TypedValue.applyDimension(
				TypedValue.COMPLEX_UNIT_DIP, 1, dm));
		// 设置Tab Indicator的高度
		tabs.setIndicatorHeight((int) TypedValue.applyDimension(
				TypedValue.COMPLEX_UNIT_DIP, 4, dm));
		// 设置Tab标题文字的大小
		tabs.setTextSize((int) TypedValue.applyDimension(
				TypedValue.COMPLEX_UNIT_SP, 16, dm));
		// 设置Tab Indicator的颜色
		tabs.setIndicatorColor(Color.parseColor("#00b383"));
		// 设置选中Tab文字的颜色 (这是我自定义的一个方法)
		tabs.setSelectedTextColor(Color.parseColor("#00b383"));
		// 取消点击Tab时的背景色
		tabs.setTabBackground(0);
	}

	// fix that "more" item does not show
	private void getOverflowMenu() {
		try {
			ViewConfiguration config = ViewConfiguration.get(this);
			Field menuKeyField = ViewConfiguration.class.getDeclaredField("sHasPermanentMenuKey");
			if(menuKeyField != null) {
				menuKeyField.setAccessible(true);
				menuKeyField.setBoolean(config, false);
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		goOthersActivity(item.getItemId());
		return super.onOptionsItemSelected(item);
	}

	private void goOthersActivity(int id){
		if(id == R.id.id_user){
			return;
		}
		Intent intent = new Intent();
		if(user == null){
			intent.setClass(this, LoginActivity.class);
		}else{
			if(id == R.id.action_settings){
				intent.setClass(this, SettingActivity.class);
			}else if(id == R.id.my_mission){
				intent.setClass(this, MyMissionActivity.class);
			}else if(id == R.id.my_money){
				intent.setClass(this, MyMoneyActivity.class);
			}else if(id == R.id.id_check){
				if(user.getStatus() == 2 || user.getStatus() == 0){
					intent.setClass(this, VerificationActivity.class);
				}else{
					String msg = "";
					if(user.getStatus() == 3){
						msg = "正在审核中，请等待";
					}else{
						msg = "审核已通过";
					}
					Toast.makeText(this, msg, Toast.LENGTH_SHORT).show();
					return;
				}
			}
		}
		startActivity(intent);
	}

	// fix that submenu does not show icon
	@Override
	public boolean onMenuOpened(int featureId, Menu menu) {
		if (featureId == Window.FEATURE_ACTION_BAR && menu != null) {
			if (menu.getClass().getSimpleName().equals("MenuBuilder")) {
				try {
					Method m = menu.getClass().getDeclaredMethod(
							"setOptionalIconsVisible", Boolean.TYPE);
					m.setAccessible(true);
					m.invoke(menu, true);
				} catch (Exception e) {
				}
			}
		}
		return super.onMenuOpened(featureId, menu);
	}
	// config actionbar menu
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		MenuInflater inflater = getMenuInflater();
		inflater.inflate(R.menu.home, menu);
		return super.onCreateOptionsMenu(menu);
	}

	@Override
	public <T> void onResponse(T t, int requestCode) {
		if(dialog != null){
			dialog.dismiss();
		}if(requestCode == Constants.LOGIN){
			UserVo userVo = (UserVo)t;
			if(userVo.getStatus() == 0){
				userVo.getResult().setUserName(user.getUserName());
				userVo.getResult().setPassword(user.getPassword());
				UserTools.saveUser(this, userVo);
				showUserInfo();
			}else{
				Toast.makeText(this, userVo.getMessage(), Toast.LENGTH_SHORT).show();
			}
		}
		
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		if(dialog != null){
			dialog.dismiss();
		}
	}

}

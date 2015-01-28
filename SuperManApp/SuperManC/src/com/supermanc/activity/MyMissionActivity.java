package com.supermanc.activity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.util.DisplayMetrics;
import android.util.TypedValue;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.LinearLayout;
import android.widget.TextView;

import cn.jpush.android.api.JPushInterface;

import com.supermanc.R;
import com.supermanc.fragments.DoneMissionFragment;
import com.supermanc.fragments.MyMissionFragment;
import com.supermanc.views.PagerSlidingTabStrip;
import com.umeng.analytics.MobclickAgent;

public class MyMissionActivity extends FragmentActivity implements OnClickListener{

	private MyMissionFragment myMissionFragment;
	private DoneMissionFragment doneMissionFragment;
	private PagerSlidingTabStrip tabs;
	/**
	 * 获取当前屏幕的密度
	 */
	private DisplayMetrics dm;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		myMissionFragment = new MyMissionFragment();
		doneMissionFragment = new DoneMissionFragment();
		setContentView(R.layout.my_mission_activity);
		initView();
		dm = getResources().getDisplayMetrics();
		ViewPager pager = (ViewPager) findViewById(R.id.pager);
		tabs = (PagerSlidingTabStrip) findViewById(R.id.tabs);
		pager.setAdapter(new ViewPagerAdapter(getSupportFragmentManager()));
		tabs.setViewPager(pager);
		setTabsValue();
		initListener();
	}
	
	private void initView(){
		initTitle();
	}
	
	private void initTitle(){
		LinearLayout backLayout = (LinearLayout)this.findViewById(R.id.titleLeftLayout);
		backLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				MyMissionActivity.this.finish();
			}
		});
		((TextView)this.findViewById(R.id.titleContent)).setText("我的任务");
	}
	
	private void initListener(){
	}
	
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.btnRegister:
			break;
		case R.id.btnLogin:
			break;
		default:
			break;
		}
	}
	
	private void goOtherActivity(int id){
		Intent intent = new Intent();
		if(id == R.id.btnRegister){
			intent.setClass(this, RegisterActivity.class);
		}else{
			intent.setClass(this, LoginActivity.class);
		}
		startActivity(intent);
	}
	
	public class ViewPagerAdapter extends FragmentPagerAdapter {

		public ViewPagerAdapter(FragmentManager fm) {
			super(fm);
		}

		private final String[] titles = { "我的任务", "已完成任务"};

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
				return myMissionFragment;
			case 1:
				return doneMissionFragment;
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

	@Override
	protected void onPause() {
		super.onPause();
		MobclickAgent.onPause(this);
		JPushInterface.onPause(this);
	}

	@Override
	protected void onResume() {
		super.onResume();
		MobclickAgent.onResume(this);
		JPushInterface.onResume(this);
	}
	
}

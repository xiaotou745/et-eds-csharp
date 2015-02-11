package com.eds.supermanc.activity;

import java.util.HashMap;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import com.android.volley.VolleyError;
import com.eds.supermanc.Constants;
import com.eds.supermanc.beans.UserVo;
import com.eds.supermanc.beans.MissionBean.Mission;
import com.eds.supermanc.beans.UserVo.User;
import com.eds.supermanc.utils.ButtonUtil;
import com.eds.supermanc.utils.UserTools;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

public class MissionDetailActivity extends BaseActivity implements HTTPListener,OnClickListener{
	
	private Mission mission = null;
	private LinearLayout getOrderLayout;
	private LinearLayout contactSellerLayout;
	private LinearLayout orderPrepareLayout;
	private TextView titleContent;
	private Button btnGetOrder;//抢单
	private Button btnDone;//完成
	private Button btnContactSeller;//联系收货人
	private Button contactSeller;// 联系卖家
	private LinearLayout fromAddressLayout;
	private LinearLayout toAddressLayout;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		setContentView(R.layout.mission_detail_activity);
		this.mission = (Mission)this.getIntent().getExtras().getSerializable("mission");
		initView();
		initListener();
		initData();
		changeUI();
	}
	
	private void initView(){
		titleContent = (TextView)findViewById(R.id.titleContent);
		getOrderLayout = (LinearLayout)findViewById(R.id.getOrderLayout);
		contactSellerLayout = (LinearLayout)findViewById(R.id.contactSellerLayout);
		orderPrepareLayout = (LinearLayout)findViewById(R.id.orderPrepareLayout);
		btnGetOrder = (Button)findViewById(R.id.btnGetOrder);
		btnDone = (Button)findViewById(R.id.btnDone);
		contactSeller = (Button)findViewById(R.id.contactSeller);
		btnContactSeller = (Button)findViewById(R.id.btnContactSeller);
		fromAddressLayout = (LinearLayout)findViewById(R.id.fromAddressIcon);
		toAddressLayout = (LinearLayout)findViewById(R.id.toAddressIcon);
		initTitle();
	}
	
	private void changeUI(){
		if(mission.getStatus() == 0){
			getOrderLayout.setVisibility(View.VISIBLE);
			contactSellerLayout.setVisibility(View.GONE);
			orderPrepareLayout.setVisibility(View.GONE);
			titleContent.setText("待抢单");
		}else if(mission.getStatus() == 2){
			getOrderLayout.setVisibility(View.GONE);
			contactSellerLayout.setVisibility(View.GONE);
			orderPrepareLayout.setVisibility(View.VISIBLE);
			titleContent.setText("已接单");
		}else if(mission.getStatus() == 1){
			getOrderLayout.setVisibility(View.GONE);
			contactSellerLayout.setVisibility(View.VISIBLE);
			orderPrepareLayout.setVisibility(View.GONE);
			titleContent.setText("订单已完成");
		}else if(mission.getStatus() == 3){
			getOrderLayout.setVisibility(View.GONE);
			contactSellerLayout.setVisibility(View.GONE);
			orderPrepareLayout.setVisibility(View.GONE);
			titleContent.setText("订单已取消");
		} 
	}
	
	private void initTitle(){
		LinearLayout backLayout = (LinearLayout)this.findViewById(R.id.titleLeftLayout);
		backLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				MissionDetailActivity.this.finish();
			}
		});
		((ImageView)this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
		titleContent.setText("任务详情");
	}
	
	private void initListener(){
		btnGetOrder.setOnClickListener(this);
		btnDone.setOnClickListener(this);
		contactSeller.setOnClickListener(this);
		btnContactSeller.setOnClickListener(this);
		fromAddressLayout.setOnClickListener(this);
		toAddressLayout.setOnClickListener(this);
	}

	private void initData(){
		((TextView)findViewById(R.id.incomeMoney)).setText(mission.getIncome()+"元");
		((TextView)findViewById(R.id.distance)).setText(mission.getDistance()+"公里");
		((TextView)findViewById(R.id.incomeTime)).setText(mission.getPubDate());
		((TextView)findViewById(R.id.shopName)).setText(mission.getBusinessName());
		((TextView)findViewById(R.id.shopAddress)).setText(mission.getPickUpCity()+mission.getPickUpAddress());
		((TextView)findViewById(R.id.distanceBet)).setText("相距"+mission.getDistanceB2R()+"公里");
		((TextView)findViewById(R.id.toUserName)).setText(mission.getReceviceName());
		((TextView)findViewById(R.id.toUserAddress)).setText(mission.getReceviceCity()+mission.getReceviceAddress());
		((TextView)findViewById(R.id.shopName)).setText(mission.getBusinessName());
		((TextView)findViewById(R.id.amount)).setText("订单总金额"+mission.getAmount()+"元");
		((TextView)findViewById(R.id.sellerPhone)).setText(mission.getBusinessPhone());
		((TextView)findViewById(R.id.receiverPhone)).setText(mission.getRecevicePhone());
		if(mission.isIsPay()){
			((TextView)findViewById(R.id.isPayText)).setText("卖家已付款");
		}else{
			((TextView)findViewById(R.id.isPayText)).setText("卖家未付款");
		}
		((TextView)findViewById(R.id.remark)).setText(mission.getRemark());
	}
	
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.btnGetOrder:
			request(R.id.btnGetOrder);
			break;
		case R.id.btnDone:
			request(R.id.btnDone);
			break;
		case R.id.contactSeller:
			call(mission.getBusinessPhone());
			break;
		case R.id.btnContactSeller:
			call(mission.getRecevicePhone());
			break;
		case R.id.fromAddressIcon:
		case R.id.toAddressIcon:
			goBaiduMap(v.getId());
			break;
		default:
			break;
		}
	}
	
	private void goBaiduMap(int id){
		Intent intent = new Intent();
		intent.setClass(this, BaiduMapActivity.class);
		String city = "";
		String address = "";
		if(id == R.id.fromAddressIcon){
			city = mission.getPickUpCity();
			address = mission.getPickUpAddress();
		}
		if(id == R.id.toAddressIcon){
			city = mission.getReceviceCity();
			address = mission.getReceviceAddress();
		}
		intent.putExtra("city", city);
		intent.putExtra("detailAddress", address);
		startActivity(intent);
	}

	private void request(int id){
		// 判断是否身份验证通过
		User user = UserTools.getUser(this);
		if(user.getStatus() == 2 || user.getStatus() == 0){
			Intent intent = new Intent();
			intent.setClass(this, VerificationActivity.class);
			startActivity(intent);
			return;
		}
		if(user.getStatus() == 3){
			Toast.makeText(this, "你的身份正在审核中... 暂时无法进行抢单等操作", Toast.LENGTH_SHORT).show();
			return;
		}
		
		Map<String,String> params = new HashMap<String,String>();
		params.put("userId", UserTools.getUser(this).getUserId());
		params.put("orderNo", mission.getOrderNo());
		if(id == R.id.btnGetOrder){
			ButtonUtil.setDisabled(btnGetOrder, this);
			VolleyTool.get(Constants.RUSH_ORDER_URL, params, this, Constants.RUSHORDER, null);
		}else if(id == R.id.btnDone){
			ButtonUtil.setDisabled(btnDone, this);
			VolleyTool.get(Constants.FINISH_ORDER_URL, params, this, Constants.FINISHORDER, null);
		}
	}
	
	private void call(String phoneNo){
		Uri telUri = Uri.parse("tel:"+phoneNo);
		Intent intent= new Intent(Intent.ACTION_DIAL, telUri);
		startActivity(intent);
	}
	
	@Override
	public <T> void onResponse(T t, int requestCode) {
		String response = (String)t;
		try {
			JSONObject obj = new JSONObject(response);
			int code = obj.getInt("Status");
			String message = obj.getString("Message");
			if(code == 0){
				if(requestCode == Constants.RUSHORDER){
					mission.setStatus(2);
					ButtonUtil.setEnable(btnGetOrder);
					Toast.makeText(this, "抢单成功", Toast.LENGTH_SHORT).show();
				}if(requestCode == Constants.FINISHORDER){
					JSONObject json = new JSONObject(obj.getString("Result"));
					String money = json.getString("balanceAmount");
					UserVo vo = new UserVo();
					User user = UserTools.getUser(this);
					user.setAmount(money);
					vo.setResult(user);
					UserTools.saveUser(this, vo);
					Toast.makeText(this, "完成订单成功", Toast.LENGTH_SHORT).show();
					mission.setStatus(1);
					ButtonUtil.setEnable(btnDone);
				}
				changeUI();
			}else{
				ButtonUtil.setEnable(btnDone);
				ButtonUtil.setEnable(btnGetOrder);
				Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		ButtonUtil.setEnable(btnDone);
		ButtonUtil.setEnable(btnGetOrder);
	}
	
}
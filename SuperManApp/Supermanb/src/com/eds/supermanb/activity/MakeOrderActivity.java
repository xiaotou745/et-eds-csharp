package com.eds.supermanb.activity;

import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioGroup;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.baidu.mapapi.search.core.SearchResult;
import com.baidu.mapapi.search.geocode.GeoCodeResult;
import com.baidu.mapapi.search.geocode.GeoCoder;
import com.baidu.mapapi.search.geocode.OnGetGeoCoderResultListener;
import com.baidu.mapapi.search.geocode.ReverseGeoCodeResult;
import com.eds.supermanb.constant.Constants;
import com.eds.supermanb.entitys.User;
import com.eds.supermanb.utils.StringUtil;
import com.eds.supermanb.utils.UserUtil;
import com.eds.supermanb.utils.VolleyUtil;
import com.eds.supermanb.utils.VolleyUtil.HTTPListener;
import com.supermanb.supermanb.R;


public class MakeOrderActivity extends BaseActionBarActivity implements
		OnGetGeoCoderResultListener, HTTPListener {
	private EditText reciverPhnoe;
	private EditText reciverAddress;
	private EditText reciverMoney;
	private EditText deliveryMsg;
	private EditText reciverName;
	private RadioGroup group;
	private Button btnMakeOrder;
	private GeoCoder mSearch;
	private String reciverAdd, reciverAmount, reviverPh, reciverNa;
	boolean isPay;
	private double longitude;
	private double laitude;
	private User user;
	private ProgressDialog dialog;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.make_order_activity);
		actionBar.setTitle("发起订单");
		initView();

	}

	private void initView() {
		// TODO Auto-generated method stub
		reciverAddress = (EditText) findViewById(R.id.et_reciverAddress);
		reciverMoney = (EditText) findViewById(R.id.et_money);
		reciverName = (EditText) findViewById(R.id.et_reciverName);
		reciverPhnoe = (EditText) findViewById(R.id.et_reciverPhone);
		deliveryMsg = (EditText) findViewById(R.id.et_msg);
		group = (RadioGroup) findViewById(R.id.rg_check);
		btnMakeOrder = (Button) findViewById(R.id.btn_putOrder);
		mSearch = GeoCoder.newInstance();
		mSearch.setOnGetGeoCodeResultListener(this);
		user = UserUtil.readUser(this);
		btnMakeOrder.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				reciverAdd = reciverAddress.getText().toString();
				reciverAmount = reciverMoney.getText().toString();
				reviverPh = reciverPhnoe.getText().toString();
				reciverNa = reciverName.getText().toString();

				if (group.getCheckedRadioButtonId() == R.id.rb_1) {
					isPay = false;
				} else {
					isPay = true;
				}
//				if (reciverNa.length() == 0) {
//					sendMsg("请输入收货人姓名");
//					return;
//				}
				if (!StringUtil.isMobileNO(reviverPh)) {
					sendMsg("请输入正确手机号");
					return;
				}
				if (reciverAmount .isEmpty()
						|| Double.parseDouble(reciverAmount) == 0) {
					sendMsg("请输入大于零的金额");
					return;
				}
				if (reciverAdd .isEmpty() || reciverAdd.length() == 0) {
					sendMsg("请输入收货人地址");
					return;
				}
				dialog = ProgressDialog.show(MakeOrderActivity.this, "提示",
						"订单正在发布", false, false);
				btnMakeOrder.setEnabled(false);
//				mSearch.geocode(new GeoCodeOption().address(reciverAdd).city(
//						user.address.city.getName()));
				submitOrderInfo();
			}
		});
	}
	/**
	 * 无地址经纬度的提交
	 */
public void submitOrderInfo(){
	VolleyUtil util = new VolleyUtil(this);
	HashMap<String, String> params = new HashMap<String, String>();
	params.put("userId", "" + user.id);
	params.put("laitude", "" + laitude);
	params.put("longitude", "" + longitude);
	params.put("receviceName", reciverNa);
	params.put("recevicePhone", "" + reviverPh);
	params.put("receviceAddress", reciverAdd);
	params.put("Amount", "" + reciverAmount);
	params.put("IsPay", "" + isPay);
	params.put("Remark", deliveryMsg.getText().toString());

	util.post(params, Constants.URL_POST_PUBLISH_ORDER, this, 500);
}
	@Override
	public void onGetGeoCodeResult(GeoCodeResult arg0) {
		// TODO Auto-generated method stub
		if (arg0 == null || arg0.error != SearchResult.ERRORNO.NO_ERROR) {
			Toast.makeText(this, "抱歉，您的地址定位失败", Toast.LENGTH_LONG).show();
			dialog.dismiss();
			btnMakeOrder.setEnabled(true);
			return;
		}
		String strInfo = String.format("纬度：%f 经度：%f",
				arg0.getLocation().latitude, arg0.getLocation().longitude);
		// Toast.makeText(this, strInfo, Toast.LENGTH_LONG).show();
		laitude = arg0.getLocation().latitude;
		longitude = arg0.getLocation().longitude;
		VolleyUtil util = new VolleyUtil(this);
		HashMap<String, String> params = new HashMap<String, String>();
		params.put("userId", "" + user.id);
		params.put("laitude", "" + laitude);
		params.put("longitude", "" + longitude);
		params.put("receviceName", reciverNa);
		params.put("recevicePhone", "" + reviverPh);
		params.put("receviceAddress", reciverAdd);
		params.put("Amount", "" + reciverAmount);
		params.put("IsPay", "" + isPay);
		params.put("Remark", deliveryMsg.getText().toString());

		util.post(params, Constants.URL_POST_PUBLISH_ORDER, this, 500);
	}

	@Override
	public void onGetReverseGeoCodeResult(ReverseGeoCodeResult arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onResponse(String response, int requestCode) {
		// TODO Auto-generated method stub
		try {
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");
			if (status == 0) {
				sendMsg("订单发布成功");
				finish();

			} else {
				sendMsg(msg);
			}

		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			sendMsg("服务器错误");
		} finally {
			dialog.dismiss();
			btnMakeOrder.setEnabled(true);   
		}
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		sendMsg("服务器错误");
		dialog.dismiss();
		btnMakeOrder.setEnabled(true);
	}

}

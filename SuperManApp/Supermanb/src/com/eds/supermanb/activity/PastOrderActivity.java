package com.eds.supermanb.activity;

import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.text.Html;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AbsListView.OnScrollListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import com.android.volley.VolleyError;
import com.eds.supermanb.constant.Constants;
import com.eds.supermanb.entitys.OrderInfor;
import com.eds.supermanb.entitys.User;
import com.eds.supermanb.utils.EtsBLog;
import com.eds.supermanb.utils.UserUtil;
import com.eds.supermanb.utils.VolleyUtil;
import com.eds.supermanb.utils.VolleyUtil.HTTPListener;
import com.supermanb.supermanb.R;


public class PastOrderActivity extends BaseActionBarActivity implements
		HTTPListener {
	private ArrayList<OrderInfor> orders;
	private ListView list;
	private PastOrderAdapter adapter;
	private int page = 0;
	private ProgressDialog dialog;
	private User user;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.past_order_activity);
		actionBar.setTitle("已完成订单");
		initDate();
		initView();
	}

	private void initView() {
		list = (ListView) findViewById(R.id.lv_pastOrder);
		adapter = new PastOrderAdapter();
		list.setAdapter(adapter);
		findViewById(R.id.btn_refresh).setOnClickListener(
				new OnClickListener() {
					@Override
					public void onClick(View v) {
						initDate();
						adapter.notifyDataSetChanged();
					}
				});
		list.setOnItemClickListener(new OnItemClickListener() {
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				Intent intent = new Intent(PastOrderActivity.this,
						OrderInfoActivity.class);
				intent.putExtra("Order", adapter.getItem(position));
				startActivity(intent);
			}
		});

	}

	/**
	 * 
	 * @param page
	 *            页数
	 * @param code
	 *            请求码1 加载或刷新2加载更多
	 */
	private void getOrder(int code) {
		if (code == 1) {
			page = 0;
		}
		dialog = ProgressDialog.show(this, "提示", "正在加载", false, false);
		VolleyUtil util = new VolleyUtil(this);
		HashMap<String, String> parms = new HashMap<String, String>();
		parms.put("userId", "" + user.id);
		parms.put("status", "" + Constants.ORDER_FINISH);
		parms.put("pagedIndex", "" + (page++));
		parms.put("pagedSize", "" + 10);
		util.get(parms, Constants.URL_GET_ORDER_LIST, this, code);
	}

	private void initDate() {
		orders = new ArrayList<OrderInfor>();
		user = UserUtil.readUser(this);
		getOrder(1);

	}

	private class PastOrderAdapter extends BaseAdapter {
		@Override
		public int getCount() {
			return orders.size();
		}

		@Override
		public OrderInfor getItem(int position) {
			return orders.get(position);
		}

		@Override
		public long getItemId(int position) {
			return position;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			if (convertView == null) {
				holder = new ViewHolder();
				convertView = LayoutInflater.from(PastOrderActivity.this)
						.inflate(R.layout.past_drder_item, null);
				holder.orderInfo = (TextView) convertView
						.findViewById(R.id.tv_orderInfo);
				holder.orderStatus = (TextView) convertView
						.findViewById(R.id.tv_orderStatus);
				holder.imageView = (ImageView) convertView
						.findViewById(R.id.imv_orderStatus);
				convertView.setTag(holder);
			} else {
				holder = (ViewHolder) convertView.getTag();
			}
			OrderInfor infor = getItem(position);
			String info = "<html><font color='#BBC0C7'>发布时间：</font>"
					+ infor.puvlishTime + "<br/>"
					+ "<font color='#BBC0C7'>收货人电话：</font>"
					+ infor.consigneePhone + "<br/>"
					+ "<font color='#BBC0C7'>配送员：</font>"
					+ infor.deliverymanPhone + "&nbsp;&nbsp;"
					+ infor.deliveryman + "<html>";
			holder.orderInfo.setText(Html.fromHtml(info));
			UserUtil.setOrderStatus(PastOrderActivity.this, infor,
					holder.orderStatus, holder.imageView);
			return convertView;
		}

		class ViewHolder {
			TextView orderInfo;
			TextView orderStatus;
			ImageView imageView;
		}
	}

	@Override
	public void onResponse(String response, int requestCode) {
		try {
			EtsBLog.d("finishOrder:["+response+"]");
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");

			if (status == 0) {
				JSONArray array = object.getJSONArray("Result");
				JSONObject obj;
				OrderInfor infor;
				ArrayList<OrderInfor> infors = new ArrayList<OrderInfor>();

				if (array.length() == 0) {
					sendMsg("没有更多订单");
					return;
				}
				for (int i = 0; i < array.length(); i++) {
					infor = new OrderInfor();

					obj = array.getJSONObject(i);
					infor.consigneePhone = obj.getString("RecevicePhoneNo");
					infor.deliveryman = obj.getString("superManName");
					infor.deliverymanPhone = obj.getString("superManPhone");
					infor.distance = obj.getDouble("distanceB2R");
					infor.orderStadus = obj.getInt("Status");
					infor.puvlishTime = obj.getString("PubDate");
					infor.remark = obj.getString("Remark");
					infor.shopAddress = obj.getString("PickUpAddress");
					infor.shopName = obj.getString("PickUpName");
					infor.amount = obj.getDouble("Amount");
					infor.isPay = obj.getBoolean("IsPay");
					infor.reciverTime = obj.getString("ActualDoneDate");
					infor.deliveryAddress = obj.getString("ReceviceAddress");
					infor.orderId = obj.getString("OrderNo");
					infor.consignee = obj.getString("ReceviceName");
					infors.add(infor);
				}

				if (requestCode == 1) {
					if(array.length()==10){
						list.setOnScrollListener(new OnScrollListener() {

							@Override
							public void onScrollStateChanged(AbsListView view, int scrollState) {
								switch (scrollState) {
								// 当不滚动时
								case OnScrollListener.SCROLL_STATE_IDLE:
									// 判断滚动到底部
									if (list.getLastVisiblePosition() == (list.getCount() - 1)) {
										getOrder(2);
									}
									break;
								}

							}

							@Override
							public void onScroll(AbsListView view, int firstVisibleItem,
									int visibleItemCount, int totalItemCount) {
							}
						});
					}
					
					
					orders.clear();
					orders.addAll(infors);
					adapter.notifyDataSetInvalidated();
				} else {
					orders.addAll(infors);
					adapter.notifyDataSetChanged();
				}

			} else {
				sendMsg(msg);
			}

		} catch (JSONException e) {
			e.printStackTrace();
			sendMsg("服务器错误");
		} finally {
			dialog.dismiss();
		}

	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		dialog.dismiss();
		sendMsg("服务器错误");
	}
}

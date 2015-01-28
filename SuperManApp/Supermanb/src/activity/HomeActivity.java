package activity;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Set;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import utils.EtsBLog;
import utils.UserUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.app.ProgressDialog;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.text.Html;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AbsListView.OnScrollListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import cn.jpush.android.api.JPushInterface;
import cn.jpush.android.api.TagAliasCallback;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;
import com.umeng.update.UmengDialogButtonListener;
import com.umeng.update.UmengUpdateAgent;
import com.umeng.update.UpdateStatus;

import constant.Constants;
import entitys.OrderInfor;
import entitys.User;

public class HomeActivity extends BaseActionBarActivity implements
		HTTPListener {
	private ListView list;
	private ArrayList<OrderInfor> orders;
	private OrderAdapter adapter;
	private User user;
	private int page = 0;
	private ProgressDialog dialog;
	private Button btn;
	private TextView tvPastOrder;
	private TextView tvNoAddress;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_home);
		actionBar.setTitle("呼叫配送员");
		// 极光
		JPushInterface.resumePush(getApplicationContext());
		setJPushTags();
		UmengUpdateAgent.update(this);
		UmengUpdateAgent.setDialogListener(new UmengDialogButtonListener() {
			@Override
			public void onClick(int arg0) {
				if (arg0 == UpdateStatus.NotNow) {
					UmengUpdateAgent.update(HomeActivity.this);
				}
			}
		});
	}

	// 设置极光标签
	private void setJPushTags() {
		Set<String> sets = new HashSet<String>();
		sets.add(UserUtil.readUser(this).id + "");
		JPushInterface.setTags(getApplicationContext(), sets, mAliasCallback);
	}

	private void initView() {
		list = (ListView) findViewById(R.id.list_order);
		tvNoAddress = (TextView) findViewById(R.id.tv_noAddress);

		adapter = new OrderAdapter();
		list.setAdapter(adapter);
		tvPastOrder = (TextView) findViewById(R.id.tv_pastOrder);
		tvPastOrder.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				Intent intent = new Intent(HomeActivity.this,
						PastOrderActivity.class);
				startActivity(intent);
			}
		});

		btn = (Button) findViewById(R.id.btn_initiateOrder);
		switch (user.userStadus) {
		case 1:
			btn.setText("发起订单");
			tvPastOrder.setVisibility(View.VISIBLE);
			list.setVisibility(View.VISIBLE);
			tvNoAddress.setVisibility(View.GONE);
			break;
		case 0:
			btn.setText("点击验证");
			tvPastOrder.setVisibility(View.GONE);
			list.setVisibility(View.GONE);
			tvNoAddress.setText("您还未进行验证，暂时无法发单");
			tvNoAddress.setVisibility(View.VISIBLE);
			break;
		case 2:
			btn.setText("添加发货地址");
			tvPastOrder.setVisibility(View.GONE);
			list.setVisibility(View.GONE);
			tvNoAddress.setText("您还没有添加地址，暂时无法发单");
			tvNoAddress.setVisibility(View.VISIBLE);
			break;
		case 3:
			tvPastOrder.setVisibility(View.GONE);
			tvNoAddress.setText("您已提交审核，审核通过后才可正常下单。");
			list.setVisibility(View.GONE);
			tvNoAddress.setVisibility(View.VISIBLE);
			btn.setClickable(false);
			btn.setBackgroundColor(getResources().getColor(
					R.color.button_bg_pre));
			break;
		case 4:
			tvPastOrder.setVisibility(View.GONE);
			tvNoAddress.setText("您的审核信息尚有疑问，请致电客服或重新提交审核。");
			list.setVisibility(View.GONE);
			tvNoAddress.setVisibility(View.VISIBLE);
			break;
		}
		btn.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				Intent intent = null;
				switch (user.userStadus) {
				case 1:
					intent = new Intent(HomeActivity.this,
							MakeOrderActivity.class);
					break;
				case 0:
					intent = new Intent(HomeActivity.this,
							VerificationActivity.class);
					break;
				case 4:
					intent = new Intent(HomeActivity.this,
							VerificationActivity.class);
					break;
				case 2:
					intent = new Intent(HomeActivity.this,
							AddAdressActivity.class);
					break;
				}
				if (intent != null)
					startActivity(intent);
			}
		});
		list.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				Intent intent = new Intent(HomeActivity.this,
						OrderInfoActivity.class);
				intent.putExtra("Order", adapter.getItem(position));
				startActivity(intent);

			}
		});

	}

	@Override
	protected void onResume() {
		super.onResume();
		initDate();
		initView();
	}

	private void initDate() {
		user = UserUtil.readUser(this);
		orders = new ArrayList<OrderInfor>();
		if (user.userStadus == 1)
			getOrder(1);

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

		dialog = ProgressDialog.show(this, "提示", "正在加载中", false, true);

		VolleyUtil util = new VolleyUtil(this);
		HashMap<String, String> parms = new HashMap<String, String>();
		parms.put("userId", "" + user.id);
		parms.put("status", "" + 4);
		parms.put("pagedIndex", "" + (page++));
		parms.put("pagedSize", "" + 10);
		util.get(parms, Constants.URL_GET_ORDER_LIST, this, code);
	}

	private class OrderAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return orders.size();
		}

		@Override
		public OrderInfor getItem(int position) {
			// TODO Auto-generated method stub
			return orders.get(position);
		}

		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return position;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			if (convertView == null) {
				holder = new ViewHolder();
				convertView = LayoutInflater.from(HomeActivity.this).inflate(
						R.layout.order_item, null);
				holder.orderInfor = (TextView) convertView
						.findViewById(R.id.tv_orderInfo);
				holder.orderStatus = (TextView) convertView
						.findViewById(R.id.tv_orderStatus);
				holder.imvStatus = (ImageView) convertView
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
					+ "<font color='#BBC0C7'>订单金额：</font>"
					+ "<font color='#FF4F00'>" + infor.amount + "</font>"
					+ "<html>";
			holder.orderInfor.setText(Html.fromHtml(info));
			switch (infor.orderStadus) {
			case 0:
				holder.orderStatus.setText("待接单");
				holder.imvStatus.setImageDrawable(getResources().getDrawable(
						R.drawable.orderstatus_0));
				holder.orderStatus.setTextColor(getResources().getColor(
						R.color.orderstatus_0));

				break;
			case 1:
				holder.orderStatus.setText("已完成");
				holder.imvStatus.setImageDrawable(getResources().getDrawable(
						R.drawable.orderstatus_1));
				holder.orderStatus.setTextColor(getResources().getColor(
						R.color.orderstatus_1));
				break;
			case 2:
				holder.orderStatus.setText("已接单");
				holder.imvStatus.setImageDrawable(getResources().getDrawable(
						R.drawable.orderstatus_2));
				holder.orderStatus.setTextColor(getResources().getColor(
						R.color.orderstatus_2));
				break;
			case 3:
				holder.orderStatus.setText("已取消");
				holder.imvStatus.setImageDrawable(getResources().getDrawable(
						R.drawable.orderstatus_3));
				holder.orderStatus.setTextColor(getResources().getColor(
						R.color.orderstatus_3));
				break;

			}
			return convertView;
		}

		class ViewHolder {
			TextView orderInfor;
			TextView orderStatus;
			ImageView imvStatus;
		}
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// TODO Auto-generated method stub
		Intent intent;
		switch (item.getItemId()) {
		case R.id.address:
			sendMsg("商户地址管理");
			intent = new Intent(this, AddAdressActivity.class);
//			CityMode cityMode = new CityMode();
//			cityMode.setName("北京");
//			cityMode.setPcode("7");
//			intent.putExtra("CitMode", cityMode);
			startActivity(intent);
			break;
		case R.id.verification:

			if (user.userStadus == 3) {
				sendMsg("您的信息正在审核，暂时不能修改");
				break;
			}
			if (user.userStadus == 1) {
				sendMsg("您的信息已审核，不能修改审核信息");
				break;
			}
			intent = new Intent(this, VerificationActivity.class);
			startActivity(intent);
			break;
		case R.id.order:
			// sendMsg("订单统计");
			intent = new Intent(this, OrderSatisticsActivity.class);
			startActivity(intent);

			break;
		case R.id.phone:
			// sendMsg("拨打客服电话");
		case R.id.btn_line_superMan:
			intent = new Intent(Intent.ACTION_DIAL, Uri.parse("tel:" + "10086"));
			startActivity(intent);
			break;

		case R.id.log_out:
			// sendMsg("退出登录");
			UserUtil.logOut(this);
			JPushInterface.stopPush(getApplicationContext());
			finish();
			break;
		case R.id.reflish:
			getOrder(1);
			break;

		case android.R.id.home:
			Intent i = new Intent(Intent.ACTION_MAIN);
			i.addCategory(Intent.CATEGORY_HOME);
			startActivity(i);
			break;
		}

		return true;
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.menus, menu);
		return super.onCreateOptionsMenu(menu);
	}

	@Override
	public boolean onPrepareOptionsMenu(Menu menu) {
		// TODO Auto-generated method stub
		MenuItem item = menu.findItem(R.id.verification);
		MenuItem itemUserPhone = menu.findItem(R.id.userphone);
		itemUserPhone.setTitle(user.userPhone);
		switch (user.userStadus) {
		case 1:
			item.setTitle("商家验证（已通过）");
			break;
		case 0:
			item.setTitle("商家验证（未审核）");
			break;
		case 2:
			item.setTitle("商家验证（未添加地址）");
			break;
		case 3:
			item.setTitle("商家验证（正在审核）");
			item.setCheckable(false);
			break;
		case 4:
			item.setTitle("商家验证（未通过）");
			break;
		}
		return super.onPrepareOptionsMenu(menu);
	}

	@Override
	public void onResponse(String response, int requestCode) {
		if (dialog.isShowing()) {
			dialog.dismiss();
		}
		try {
			EtsBLog.d("orderInfo:["+response+"]");
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");
			if (status == 0) {
				JSONArray array = object.getJSONArray("Result");
				JSONObject obj;
				OrderInfor infor;
				ArrayList<OrderInfor> infors = new ArrayList<OrderInfor>();
				if (array.length() == 0) {
					if (requestCode == 1) {
						list.setVisibility(View.GONE);
						tvNoAddress.setVisibility(View.VISIBLE);
						tvNoAddress.setText("点击发起订单配送员来帮您送外卖");
						return;
					}
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
					list.setVisibility(View.VISIBLE);
					tvNoAddress.setVisibility(View.GONE);
					if (infors.size() == 10) {
						list.setOnScrollListener(new OnScrollListener() {
							@Override
							public void onScrollStateChanged(AbsListView view,
									int scrollState) {
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
							public void onScroll(AbsListView view,int firstVisibleItem, int visibleItemCount,int totalItemCount) {
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
		}

	}

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if (keyCode == KeyEvent.KEYCODE_BACK) {
			Intent i = new Intent(Intent.ACTION_MAIN);
			i.addCategory(Intent.CATEGORY_HOME);
			startActivity(i);
			return true;
		}
		return super.onKeyDown(keyCode, event);
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		if (dialog.isShowing()) {
			dialog.dismiss();
		}
		sendMsg("服务器错误");
	}

	private final TagAliasCallback mAliasCallback = new TagAliasCallback() {
		@Override
		public void gotResult(int arg0, String arg1, Set<String> arg2) {
			// 极光设置标签返回
			if(arg0==0){
				EtsBLog.d("set jpush tags is success");
			}else if (arg0 == 6002) {
				// 超时，需要重新设置
				EtsBLog.d("set jpush tags is timeout");
				setJPushTags();
			}else{
				EtsBLog.d("set jpush tags is error:"+String.valueOf(arg0));
			}
		}
	};	

}

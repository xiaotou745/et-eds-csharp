package activity;

import java.text.DecimalFormat;
import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import utils.EtsBLog;
import utils.UserUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;

import constant.Constants;
import entitys.OrderInfor;
import entitys.User;

public class OrderInfoActivity extends BaseActionBarActivity implements
		OnClickListener, HTTPListener {
	private TextView tvConsigneeInfo;
	private TextView tvOrderStatus;
	private TextView tvDistance;
	private TextView tvReciverInfo;
	private TextView tvDeliveryInfo;
	private TextView tvIsPay;
	private TextView tvRemake;
	private TextView tvAmount;
	private ImageView imvOrderStatus;
	private Button btnLineReciver;
	private Button btnDelivery;
	private Button btnCanceOrder;
	private OrderInfor infor;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.order_detil_activity);
		initView();
		initDate();
		initLisenter();
	}

	private void initLisenter() {
		// TODO Auto-generated method stub
		btnLineReciver.setOnClickListener(this);
		btnDelivery.setOnClickListener(this);
		btnCanceOrder.setOnClickListener(this);
	}

	private void initDate() {
		// TODO Auto-generated method stub
		infor = (OrderInfor) getIntent().getSerializableExtra("Order");
		EtsBLog.d(infor.toString());
		if(infor.orderStadus==Constants.ORDER_NOT_RECIVED){
			btnDelivery.setVisibility(View.GONE);
			btnCanceOrder.setVisibility(View.VISIBLE);
			}
		
		tvConsigneeInfo.setText(infor.puvlishTime + "\n" + infor.shopName
				+ "\n" + infor.shopAddress);
		UserUtil.setOrderStatus(this, infor, tvOrderStatus, imvOrderStatus);
		double dis = infor.distance / 1000;
		DecimalFormat df = new DecimalFormat("0.00");// 格式化小数，不足的补0
		String num = df.format(dis);// 返回的是String类型的

		tvDistance.setText("相距" + num + "公里");
		if (infor.reciverTime.isEmpty() || "null".equals(infor.reciverTime)) {
			tvReciverInfo.setText(infor.consignee+"    "+infor.consigneePhone+"\n"+infor.deliveryAddress);
		} else {
			
			tvReciverInfo.setText(infor.reciverTime + "\n"+infor.consignee+"    "+infor.consigneePhone+"\n"
					+ infor.deliveryAddress);
		}
		if (infor.orderStadus != Constants.ORDER_NOT_RECIVED)
			tvDeliveryInfo.setText(infor.deliveryman + "    "
					+ infor.deliverymanPhone);
		if (infor.isPay) {
			tvIsPay.setText("买家已付款");
		} else {
			tvIsPay.setText("买家未付款");
		}
		tvAmount.setText("订单总额：" + infor.amount + "元");
		if("null".equals(infor.remark)){
			tvRemake.setText("配送说明： ");
		}else {
			tvRemake.setText("配送说明：\n    " + infor.remark);
		}
	}

	private void initView() {
		// TODO Auto-generated method stub
		tvConsigneeInfo = (TextView) findViewById(R.id.tv_consigneeInfo);
		tvOrderStatus = (TextView) findViewById(R.id.tv_orderStatus);
		tvDistance = (TextView) findViewById(R.id.tv_distance);
		tvReciverInfo = (TextView) findViewById(R.id.tv_reciverInfo);
		tvDeliveryInfo = (TextView) findViewById(R.id.tv_deliveryInfo);
		tvIsPay = (TextView) findViewById(R.id.tv_isPay);
		tvRemake = (TextView) findViewById(R.id.tv_remart);
		tvAmount = (TextView) findViewById(R.id.tv_amount);
		imvOrderStatus = (ImageView) findViewById(R.id.imv_orderStatus);
		btnLineReciver = (Button) findViewById(R.id.btn_lineReciver);
		btnDelivery = (Button) findViewById(R.id.btn_line_superMan);
		btnCanceOrder = (Button) findViewById(R.id.btn_cance_order);
	
		
		
		
	}

	@Override
	public void onClick(View v) {
		Intent intent;
		// TODO Auto-generated method stub
		switch (v.getId()) {
		case R.id.btn_cance_order:
			User user = UserUtil.readUser(this);
			HashMap<String, String> map = new HashMap<String, String>();
			map.put("userId", "" + user.id);
			map.put("OrderId", infor.orderId);
			VolleyUtil util = new VolleyUtil(this);
			util.get(map, Constants.URL_CANCEL_ORDER, this, 2);
			break;
		case R.id.btn_line_superMan:
			intent = new Intent(Intent.ACTION_DIAL, Uri.parse("tel:"
					+ infor.deliverymanPhone));
			startActivity(intent);
			break;
		case R.id.btn_lineReciver:
			intent = new Intent(Intent.ACTION_DIAL, Uri.parse("tel:"
					+ infor.consigneePhone));
			startActivity(intent);
			break;

		default:
			break;
		}
	}

	@Override
	public void onResponse(String response, int requestCode) {
		// TODO Auto-generated method stub

		try {
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");

			if (status == 0) {

				// JSONArray array = object.getJSONArray("Result");
				sendMsg("订单取消成功");
				finish();
			} else {
				sendMsg(msg);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			sendMsg("服务器错误");
		}

	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		// TODO Auto-generated method stub
		sendMsg(error.getMessage());
	}

}

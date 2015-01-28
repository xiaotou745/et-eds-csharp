package activity;

import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import utils.EtsBLog;
import utils.UserUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.Window;
import android.widget.Toast;
import cn.jpush.android.api.JPushInterface;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;
import com.umeng.analytics.MobclickAgent;

import constant.Constants;
import entitys.User;

public class FirstActivity extends Activity implements HTTPListener {
	User user;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getWindow().requestFeature(Window.FEATURE_NO_TITLE);
		setContentView(R.layout.first_activity);

	}

	@Override
	protected void onPause() {
		super.onPause();
		JPushInterface.onPause(this);
		MobclickAgent.onPause(this);
	}

	ProgressDialog dialog = null;
	Handler handler = new Handler() {
		public void handleMessage(Message msg) {
			// 要做的事情

			user = UserUtil.readUser(FirstActivity.this);
			Intent intent;
			if (user == null || user.id == 0) {
				intent = new Intent(FirstActivity.this, RegistActivity.class);
				startActivity(intent);
				finish();
			} else {
				VolleyUtil volleyUtil = new VolleyUtil(FirstActivity.this);
				HashMap<String, String> map = new HashMap<String, String>();
				map.put("phoneNo", "" + user.userPhone);
				map.put("passWord", "" + user.password);
				dialog = ProgressDialog.show(FirstActivity.this, "提示", "正在登录中",
						false, false);
				dialog.show();
				volleyUtil
						.post(map, Constants.URL_LOGIN, FirstActivity.this, 0);

				super.handleMessage(msg);
			}
		}
	};

	@Override
	protected void onResume() {
		super.onResume();
		JPushInterface.onResume(this);
		MobclickAgent.onResume(this);

		new Thread() {
			public void run() {
				try {
					Thread.sleep(2000);// 线程暂停10秒，单位毫秒
					Message message = new Message();
					message.what = 1;
					handler.sendMessage(message);// 发送消息
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			};
		}.start();
	}

	@Override
	public void onResponse(String response, int requestCode) {
		if (dialog != null) {
			dialog.dismiss();
		}
		try {
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");
			if (status == 0) {
				JSONObject object2 = object.getJSONObject("Result");
				user.userStadus = object2.getInt("status");
				user.address.detill = object2.getString("Address");
				user.address.city.setPcode(object2.getInt("cityId") + "");
				user.address.city.setName("city");
				user.userPhone = object2.getString("phoneNo");
				user.address.area.setName(object2.getString("district"));
				user.address.area.setPcode("" + object2.getInt("districtId"));
				user.name = object2.getString("Name");
				user.telPhone = object2.getString("Landline");
				UserUtil.saveUser(this, user);
				EtsBLog.d("autoLogin:user["+user.toString()+"]");
				Intent intent = new Intent(this, HomeActivity.class);
				startActivity(intent);
				finish();
			} else {
				Intent intent = new Intent(this, RegistActivity.class);
				startActivity(intent);
			}
		} catch (JSONException e) {
			e.printStackTrace();
			Intent intent = new Intent(this, RegistActivity.class);
			startActivity(intent);
		}
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		Intent intent = new Intent(this, RegistActivity.class);
		startActivity(intent);
	}
}

package activity;

import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import utils.StringUtil;
import utils.UserUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.EditText;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;

import constant.Constants;
import entitys.User;

public class LoginActivity extends BaseActionBarActivity  implements HTTPListener{
	private EditText etPhone;
	private EditText etPassword;
	protected ProgressDialog dialog;
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.login_activity);
		etPhone = (EditText) findViewById(R.id.editText1);
		etPassword = (EditText) findViewById(R.id.editText2);
		findViewById(R.id.textView1).setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
			Intent intent = new Intent(LoginActivity.this, ChangePasswordActivity.class);
			startActivity(intent);
			}
		});
		
		
		findViewById(R.id.btn_login).setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				
				// TODO Auto-generated method stub
				if(!StringUtil.isMobileNO(etPhone.getText().toString())){
					sendMsg("请输入正确的手机号");
					return;
				}
				if(etPassword.getText().toString().length()<6){
					sendMsg("请输入6~16位密码");
					return;
				}
				dialog = ProgressDialog.show(LoginActivity.this, "提示", "正在登录中",
						false, false);
				VolleyUtil volleyUtil = new VolleyUtil(LoginActivity.this);
				HashMap< String, String> map = new HashMap<String, String>();
				map.put("phoneNo", ""+etPhone.getText().toString());
				map.put("passWord", ""+etPassword.getText().toString());
				volleyUtil.post(map, Constants.URL_LOGIN, LoginActivity.this, 0);
/*				User user = new User();
				user.id = 5;
				user.password = "8888";
				user.userPhone="15558888";
				Address address = new Address();
				CityMode  cityMode = new CityMode();
				cityMode.setName("北京");
				address.city = cityMode;
				user.address = address;
				UserUtil.saveUser(LoginActivity.this,user);
				Intent intent = new Intent(LoginActivity.this, HomeActivity.class);
				startActivity(intent);*/
				
			}
		});
	}

	@Override
	public void onResponse(String response, int requestCode) {
		// TODO Auto-generated method stub
		try {
			JSONObject object = new JSONObject(response);
			int status = object.getInt("Status");
			String msg = object.getString("Message");
			if(status==0){
				JSONObject object2 = object.getJSONObject("Result");
				User user = new User();
				user.id = object2.getInt("userId");
				user.userStadus = object2.getInt("status");
				user.password = etPassword.getText().toString();
				user.userPhone = etPhone.getText().toString();
				user.userStadus = object2.getInt("status");
			    user.address.detill = object2.getString("Address");
			    user.address.city.setPcode(object2.getInt("cityId")+"");
				user.address.city.setName("city");
				user.shopPhone = object2.getString("phoneNo");
				user.address.area.setName(object2.getString("district"));
				user.address.area.setPcode(""+object2.getInt("districtId"));
				user.name = object2.getString("Name");
				user.telPhone = object2.getString("Landline");
				UserUtil.saveUser(this, user);
				Intent intent = new Intent(LoginActivity.this, HomeActivity.class);
				startActivity(intent);
				finish();
				
			}else {
				sendMsg(msg);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			sendMsg("服务器错误");
		}finally{
			dialog.dismiss();
		}
		
	}
	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		// TODO Auto-generated method stub
		dialog.dismiss();
		sendMsg("服务器错误");
	}
}

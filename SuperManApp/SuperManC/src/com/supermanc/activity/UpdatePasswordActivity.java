package com.supermanc.activity;

import java.util.HashMap;
import java.util.Map;

import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.supermanc.Constants;
import com.supermanc.R;
import com.supermanc.beans.UserVo;
import com.supermanc.beans.UserVo.User;
import com.supermanc.utils.ButtonUtil;
import com.supermanc.utils.UserTools;
import com.supermanc.utils.VolleyTool;
import com.supermanc.utils.VolleyTool.HTTPListener;
 
public class UpdatePasswordActivity extends BaseActivity implements OnClickListener,HTTPListener{

	private TextView phoneNumber;
	private EditText password;
	private EditText repassword;
	private Button submitUpdate;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		requestWindowFeature(Window.FEATURE_NO_TITLE);
		setContentView(R.layout.update_password_activity);
		initView();
		initListener();
	}
	
	private void initView(){
		initTitle();
		phoneNumber = (TextView)findViewById(R.id.phoneNumber);
		phoneNumber.setText(UserTools.getUser(this).getUserName());
		password = (EditText)findViewById(R.id.password);
		repassword = (EditText)findViewById(R.id.repassword);
		submitUpdate = (Button)findViewById(R.id.submitUpdate);
	}
	
	private void initTitle(){
		LinearLayout backLayout = (LinearLayout)this.findViewById(R.id.titleLeftLayout);
		backLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				UpdatePasswordActivity.this.finish();
			}
		});
		((ImageView)this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
		((TextView)this.findViewById(R.id.titleContent)).setText("修改密码");
	}
	
	private void initListener(){
		submitUpdate.setOnClickListener(this);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.submitUpdate:
			update();
			break;

		default:
			break;
		}
	}

	private void update(){
		String psd = password.getText().toString();
		String rpsd = repassword.getText().toString();
		if("".equals(psd)){
			Toast.makeText(this, "密码不能为空", Toast.LENGTH_SHORT).show();
			return;
		}
		if(!rpsd.equals(psd)){
			Toast.makeText(this, "两次输入的密码不一致", Toast.LENGTH_SHORT).show();
			return;
		}
		ButtonUtil.setDisabled(submitUpdate, this);
		Map<String,String> params = new HashMap<String,String>();
		params.put("phoneNo", UserTools.getUser(this).getUserName());
		params.put("newPassword", psd);
		VolleyTool.post(Constants.UPDATE_PASSWORD_URL, params, this, Constants.UPDATEPASSWORD, UserVo.class);
	}
	
	@Override
	public <T> void onResponse(T t, int requestCode) {
		ButtonUtil.setEnable(submitUpdate);
		if(requestCode == Constants.UPDATEPASSWORD){
			UserVo vo = (UserVo)t;
			if(vo.getStatus() == 0){
				Toast.makeText(this, "修改成功", Toast.LENGTH_SHORT).show();
				User user = UserTools.getUser(this);
				user.setPassword(password.getText().toString());
				vo.setResult(user);
				UserTools.saveUser(this, vo);
				this.finish();
			}else{
				Toast.makeText(this, vo.getMessage(), Toast.LENGTH_SHORT).show();
			}
		}
	}

	@Override
	public void onErrorResponse(VolleyError error, int requestCode) {
		ButtonUtil.setEnable(submitUpdate);
	}

}

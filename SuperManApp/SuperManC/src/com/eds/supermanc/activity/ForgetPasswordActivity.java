package com.eds.supermanc.activity;

import java.util.HashMap;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
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
import com.eds.supermanc.Constants;
import com.eds.supermanc.utils.ButtonUtil;
import com.eds.supermanc.utils.HttpRequest;
import com.eds.supermanc.utils.MD5;
import com.eds.supermanc.utils.Utils;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

public class ForgetPasswordActivity extends BaseActivity implements HTTPListener, OnClickListener {

    private EditText mPhoneNumber;
    private EditText mPassword;
    private EditText mRepassword;
    private EditText mCheckCode;
    private TextView mGetCode;
    private Button mBtnDone;
    private TextView mTvDaojishi;
    String phoneNumber, password, repassword, checkCode = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.forget_password_activity);
        initView();
        initTitle();
        initListener();
    }

    private void initView() {
        mPhoneNumber = (EditText) findViewById(R.id.phoneNumber);
        mPassword = (EditText) findViewById(R.id.password);
        mRepassword = (EditText) findViewById(R.id.repassword);
        mCheckCode = (EditText) findViewById(R.id.checkCode);
        mGetCode = (TextView) findViewById(R.id.getCode);
        mBtnDone = (Button) findViewById(R.id.btnDone);
        mTvDaojishi = (TextView) findViewById(R.id.tvDaojishi);
    }

    private void initTitle() {
        LinearLayout backLayout = (LinearLayout) this.findViewById(R.id.titleLeftLayout);
        backLayout.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                ForgetPasswordActivity.this.finish();
            }
        });
        ((ImageView) this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
        ((TextView) this.findViewById(R.id.titleContent)).setText("忘记密码");
    }

    private void initListener() {
        mGetCode.setOnClickListener(this);
        mBtnDone.setOnClickListener(this);
    }

    private void submit() {
        if (verifyData()) {
            ButtonUtil.setDisabled(mBtnDone, this);
            String strPwd = MD5.md5(password);
            Map<String, String> params = new HashMap<String, String>();
            params.put("phoneNo", phoneNumber);
            params.put("password", strPwd);
            params.put("checkCode", checkCode);
            VolleyTool.post(Constants.FORGET_PASSWORD_URL, params, this, Constants.FORGET_PASSWORD, null);
        }
    }

    private boolean verifyData() {
        phoneNumber = mPhoneNumber.getText().toString();
        password = mPassword.getText().toString();
        repassword = mRepassword.getText().toString();
        checkCode = mCheckCode.getText().toString();
        if ("".equals(phoneNumber)) {
            Toast.makeText(this, "手机号不能为空", Toast.LENGTH_SHORT).show();
            return false;
        } else if (!Utils.isMobileNO(phoneNumber)) {
            Toast.makeText(this, "请输入正确手机号", Toast.LENGTH_SHORT).show();
            return false;
        }
        if ("".equals(password)) {
            Toast.makeText(this, "密码不能为空", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (password.length() < 6) {
            Toast.makeText(this, "密码长度不能低于6位", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (!password.equals(repassword)) {
            Toast.makeText(this, "两次输入的密码不一致", Toast.LENGTH_SHORT).show();
            return false;
        }
        if ("".equals(checkCode)) {
            Toast.makeText(this, "验证码不能为空", Toast.LENGTH_SHORT).show();
            return false;
        }
        return true;
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
        case R.id.getCode:
            getCheckCode();
            break;
        case R.id.btnDone:
            submit();
            break;
        default:
            break;
        }
    }

    @Override
    public <T> void onResponse(T t, int requestCode) {
        ButtonUtil.setEnable(mBtnDone);
        String response = (String) t;
        try {
            JSONObject obj = new JSONObject(response);
            int code = obj.getInt("Status");
            String message = obj.getString("Message");
            if (code == 0) {
                if (requestCode == Constants.FORGET_PASSWORD) {
                    Toast.makeText(this, "修改密码成功，	请重新登录", Toast.LENGTH_SHORT).show();
                    this.finish();
                }
            } else {
                Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        ButtonUtil.setEnable(mBtnDone);
    }

    private int recLen = 60;
    Handler handler = new Handler();
    Runnable runnable = new Runnable() {
        @Override
        public void run() {
            recLen--;
            if (recLen > 1) {
                mTvDaojishi.setText(recLen + "秒后重新发送");
                handler.postDelayed(this, 1000);
            } else {
                mTvDaojishi.setVisibility(View.GONE);
                mGetCode.setVisibility(View.VISIBLE);
                recLen = 60;
            }
        }
    };

    private void getCheckCode() {
        phoneNumber = mPhoneNumber.getText().toString();
        if ("".equals(phoneNumber)) {
            Toast.makeText(this, "手机号不能为空", Toast.LENGTH_SHORT).show();
            return;
        } else if (!Utils.isMobileNO(phoneNumber)) {
            Toast.makeText(this, "请输入正确手机号", Toast.LENGTH_SHORT).show();
            return;
        }

        // 设置获取验证码为不显示，倒计时显示
        mTvDaojishi.setVisibility(View.VISIBLE);
        mGetCode.setVisibility(View.GONE);

        // 开始倒计时
        handler.post(runnable);

        // new SendThread().run();

        // Map<String,String> params = new HashMap<String,String>();
        // params.put("PhoneNumber", phoneNumber);
        String params = "PhoneNumber=" + phoneNumber + "&type=1";
        new SendMessageTask(params).execute();
        // VolleyTool.get(Constants.GET_CHECK_CODE, params, this,Constants.GETCHECKCODE);
        Toast.makeText(this, "短信发送成功，请注意查收", Toast.LENGTH_SHORT).show();
    }

    private class SendMessageTask extends AsyncTask {

        private String params;

        public SendMessageTask(String params) {
            this.params = params;
        }

        @Override
        protected Object doInBackground(Object... arg0) {
            HttpRequest.sendGet(Constants.GET_CHECK_CODE, params);
            return null;
        }

    }

}

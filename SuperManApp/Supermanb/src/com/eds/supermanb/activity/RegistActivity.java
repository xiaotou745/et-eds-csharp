package com.eds.supermanb.activity;

import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;

import com.android.volley.VolleyError;
import com.eds.supermanb.adapter.CityAdapter;
import com.eds.supermanb.constant.Constants;
import com.eds.supermanb.entitys.User;
import com.eds.supermanb.picker.CityMode;
import com.eds.supermanb.picker.CityWorker;
import com.eds.supermanb.utils.MD5;
import com.eds.supermanb.utils.StringUtil;
import com.eds.supermanb.utils.UserUtil;
import com.eds.supermanb.utils.VolleyUtil;
import com.eds.supermanb.utils.VolleyUtil.HTTPListener;
import com.supermanb.supermanb.R;
import com.umeng.update.UmengDialogButtonListener;
import com.umeng.update.UmengUpdateAgent;
import com.umeng.update.UpdateStatus;


public class RegistActivity extends BaseActionBarActivity implements OnClickListener, HTTPListener {

    private Spinner spinner;
    private EditText etPhone;
    private EditText etPassword;
    private EditText etCord;
    private Button btnGetCord;
    private Button btnRegist;
    private Button btnLogin;
    private TextView tvAgreement;

    private VolleyUtil util;
    private CityAdapter adapter;
    private ProgressDialog dialog;
    private CityWorker mCityWorkerDB;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        actionBar.setTitle("商户注册");
        setContentView(R.layout.regist_activity);
        initView();
        initDate();
        UmengUpdateAgent.update(this);
        UmengUpdateAgent.setDialogListener(new UmengDialogButtonListener() {
            public void onClick(int status) {
                if (status == UpdateStatus.NotNow) {
                    UmengUpdateAgent.update(RegistActivity.this);
                }
            }
        });
    }

    private void initDate() {
        mCityWorkerDB = new CityWorker(this);
        ArrayList<CityMode> filterList = new ArrayList<CityMode>();
        if (mCityWorkerDB != null) {
            ArrayList<CityMode> list = mCityWorkerDB.getCity();
            String[] filterStr = this.getResources().getStringArray(R.array.city_open_name);
            filterList = StringUtil.filterCityByName(list, filterStr);
        }
        adapter = new CityAdapter(this, filterList);
        spinner.setAdapter(adapter);
        util = new VolleyUtil(this);
    }

    private void initView() {
        btnGetCord = (Button) findViewById(R.id.btn_getcord);
        btnGetCord.setOnClickListener(this);
        btnRegist = (Button) findViewById(R.id.btn_regist);
        btnRegist.setOnClickListener(this);
        findViewById(R.id.btn_login).setOnClickListener(this);
        findViewById(R.id.tv_agreement).setOnClickListener(this);
        etCord = (EditText) findViewById(R.id.et_cord);
        etPassword = (EditText) findViewById(R.id.et_password);
        etPhone = (EditText) findViewById(R.id.et_phone);
        spinner = (Spinner) findViewById(R.id.sp_choose_city);
    }

    @Override
    public void onClick(View arg0) {
        Intent intent;
        switch (arg0.getId()) {

        case R.id.btn_getcord:
            if (!StringUtil.isMobileNO(etPhone.getText().toString())) {
                sendMsg("请输入正确的手机号");
                return;
            }
            HashMap<String, String> map = new HashMap<String, String>();
            map.put("PhoneNumber", etPhone.getText().toString());
            btnGetCord.setEnabled(false);
            util.get(map, Constants.URL_PostGetVerifyCode_B, this, 1);
            handler.post(runnable);
            break;
        case R.id.btn_login:
            intent = new Intent(this, LoginActivity.class);
            startActivity(intent);
            break;
        case R.id.btn_regist:
            HashMap<String, String> parms = new HashMap<String, String>();
            if (etPhone.getText().toString().length() != 11) {
                sendMsg("请输入11位手机号");
                return;
            }
            if (etCord.getText().length() != 6) {
                sendMsg("请输入6位验证码");
                return;
            }
            if (etPassword.getText().length() < 6) {
                sendMsg("密码要大于6位");
                return;
            }
            dialog = ProgressDialog.show(this, "提示", "莫着急，努力注册中", false, false);
            btnRegist.setEnabled(false);
            parms.put("city", adapter.getItem((int) spinner.getSelectedItemId()).getName());
            parms.put("cityId", "" + adapter.getItem((int) spinner.getSelectedItemId()).getPcode());
            parms.put("phoneNo", etPhone.getText().toString());
            String strPwd = MD5.md5(etPassword.getText().toString());
            parms.put("passWord", strPwd);
            parms.put("verifyCode", etCord.getText().toString());
            util.post(parms, Constants.URL_REGIST, RegistActivity.this, 2);
            break;
        case R.id.tv_agreement:
            break;
        default:
            break;
        }
    }

    @Override
    public void onResponse(String response, int requestCode) {
        try {
            JSONObject object = new JSONObject(response);
            int status = object.getInt("Status");
            String msg = object.getString("Message");
            if (status == 0) {
                if (requestCode == 1) {
                    sendMsg("验证码已发送");
                    btnGetCord.setClickable(true);
                    return;
                } else if (requestCode == 2) {
                    sendMsg("注册成功");
                    JSONObject obj = object.getJSONObject("Result");
                    User user = new User();
                    user.id = obj.getInt("userId");
                    user.password = etPassword.getText().toString();
                    user.userPhone = etPhone.getText().toString();
                    user.userStadus = 2;
                    CityMode cityMode = (adapter.getItem((int) spinner.getSelectedItemId()));
                    user.address.city = cityMode;
                    UserUtil.saveUser(this, user);
                    Intent intent = new Intent(this, AddAdressActivity.class);
                    startActivity(intent);
                    clearRegistData();
                }
            } else {
                sendMsg(msg);
            }

        } catch (JSONException e) {
            e.printStackTrace();
            sendMsg("服务器错误");
        } finally {
            btnRegist.setEnabled(true);
            if (dialog != null)
                dialog.dismiss();
            btnGetCord.setEnabled(true);

        }
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        sendMsg("服务器错误");
        btnRegist.setEnabled(true);
        if (dialog != null)
            dialog.dismiss();
        btnGetCord.setEnabled(true);

    }

    private int recLen = 60;
    Handler handler = new Handler();
    Runnable runnable = new Runnable() {
        @Override
        public void run() {
            recLen--;
            if (recLen > 1) {
                btnGetCord.setText(recLen + "秒后重新发送");
                btnGetCord.setEnabled(false);
                btnGetCord.setBackgroundResource(R.drawable.chack_shap_pre);
                handler.postDelayed(this, 1000);
            } else {
                btnGetCord.setText("获取验证码");
                btnGetCord.setEnabled(true);
                btnGetCord.setBackgroundResource(R.drawable.tag_shap_nor);
                recLen = 60;
            }
        }
    };

    /**
     * 清楚注册时数据
     */
    private void clearRegistData() {
        etPhone.setText("");
        etPassword.setText("");
        etCord.setText("");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mCityWorkerDB != null) {
            mCityWorkerDB.closeCityWorker();
        }
    }

}

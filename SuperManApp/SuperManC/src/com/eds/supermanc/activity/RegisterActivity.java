package com.eds.supermanc.activity;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

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
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.eds.supermanc.Constants;
import com.eds.supermanc.adapter.CityAdapter;
import com.eds.supermanc.beans.UserVo;
import com.eds.supermanc.picker.CityMode;
import com.eds.supermanc.picker.CityWorker;
import com.eds.supermanc.utils.ButtonUtil;
import com.eds.supermanc.utils.EtsCLog;
import com.eds.supermanc.utils.HttpRequest;
import com.eds.supermanc.utils.MD5;
import com.eds.supermanc.utils.UserTools;
import com.eds.supermanc.utils.Utils;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

/**
 * 注册页面 (Description)
 * 
 * @author zaokafei
 * @version 1.0
 * @date 2015-2-28
 */
public class RegisterActivity extends BaseActivity implements HTTPListener, OnClickListener {

    private EditText mPhoneNumber;
    private EditText mPassword;
    private EditText mRepassword;
    private Button mRegister;
    private EditText mCheckCode;
    private TextView mGetCode;
    private TextView mTvDaojishi;
    private Spinner mTvChooseCity;
    private String phoneNumber, password, repassword, inviteCode, checkCode = "", cityName = "", cityId = "",
            recommendPhone = "";
    private CityAdapter adapter;
    private EditText mRecommendName;// 推荐人姓名
    private EditText mRecommendPhone;// 推荐人联系方式
    private EditText mRecommendCode;// 推荐代码
    private CityWorker mCityWorkerDB;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.register_activity);
        initView();
        initTitle();
        initListener();
        initCityData();
    }

    private void initView() {
        mPhoneNumber = (EditText) findViewById(R.id.phoneNumber);
        mPassword = (EditText) findViewById(R.id.password);
        mRepassword = (EditText) findViewById(R.id.repassword);
        mCheckCode = (EditText) findViewById(R.id.checkCode);
        mRegister = (Button) findViewById(R.id.register);
        mGetCode = (TextView) findViewById(R.id.getCode);
        mTvDaojishi = (TextView) findViewById(R.id.tvDaojishi);
        mTvChooseCity = (Spinner) findViewById(R.id.sp_choose_city);
        mRecommendName = (EditText) findViewById(R.id.recommend_name);
        mRecommendPhone = (EditText) findViewById(R.id.recommend_phone);
        mRecommendCode = (EditText) findViewById(R.id.recommend_code);
    }

    private void initTitle() {
        LinearLayout backLayout = (LinearLayout) this.findViewById(R.id.titleLeftLayout);
        backLayout.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                RegisterActivity.this.finish();
            }
        });
        ((ImageView) this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
        ((TextView) this.findViewById(R.id.titleContent)).setText("注册");
    }

    /**
     * 初始化城市区域
     */
    private void initCityData() {
        mCityWorkerDB = new CityWorker(this);
        ArrayList<CityMode> filterList = new ArrayList<CityMode>();
        if (mCityWorkerDB != null) {
            ArrayList<CityMode> list = mCityWorkerDB.getCity();
            String[] filterStr = this.getResources().getStringArray(R.array.city_open_name);
            filterList = Utils.filterCityByName(list, filterStr);
        }
        adapter = new CityAdapter(this, filterList);
        mTvChooseCity.setAdapter(adapter);
    }

    private void initListener() {
        mRegister.setOnClickListener(this);
        mGetCode.setOnClickListener(this);
    }

    private void register() {
        if (verifyData()) {
            ButtonUtil.setDisabled(mRegister, this);
            cityName = adapter.getItem((int) mTvChooseCity.getSelectedItemId()).getName();
            cityId = adapter.getItem((int) mTvChooseCity.getSelectedItemId()).getPcode();
            String strPwd = MD5.md5(password);
            Map<String, String> params = new HashMap<String, String>();
            params.put("phoneNo", phoneNumber);
            params.put("passWord", strPwd);
            params.put("verifyCode", checkCode);
            params.put("city", cityName);
            params.put("cityId", cityId);
            // params.put("recommendName", mRecommendName.getText().toString().trim());
            params.put("recommendPhone", recommendPhone);
            // params.put("recommendCode", mRecommendCode.getText().toString().trim());
            VolleyTool.post(Constants.REGISTER_URL, params, this, Constants.REGISTER, UserVo.class);
        }
    }

    private boolean verifyData() {
        phoneNumber = mPhoneNumber.getText().toString();
        password = mPassword.getText().toString();
        repassword = mRepassword.getText().toString();
        checkCode = mCheckCode.getText().toString();
        recommendPhone = mRecommendPhone.getText().toString().trim();
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
        if ("".equals(checkCode)) {
            Toast.makeText(this, "验证码不能为空", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (!password.equals(repassword)) {
            Toast.makeText(this, "两次输入的密码不一致", Toast.LENGTH_SHORT).show();
            return false;
        }
        if (recommendPhone != null && !"".equals(recommendPhone) && !Utils.isMobileNO(recommendPhone)) {
            Toast.makeText(this, "请输入正确的推荐人手机号", Toast.LENGTH_SHORT).show();
            return false;
        }
        return true;
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
        String params = "PhoneNumber=" + phoneNumber + "&type=0";
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

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
        case R.id.register:
            register();
            break;
        case R.id.getCode:
            getCheckCode();
            break;
        default:
            break;
        }
    }

    @Override
    public <T> void onResponse(T t, int requestCode) {
        ButtonUtil.setEnable(mRegister);
        if (requestCode == Constants.REGISTER) {
            UserVo userVo = (UserVo) t;
            EtsCLog.d("register:result[" + userVo.toString() + "]");
            if (userVo.getStatus() == 0) {
                Toast.makeText(this, "注册成功", Toast.LENGTH_SHORT).show();
                userVo.getResult().setUserName(phoneNumber);
                userVo.getResult().setPassword(password);
                userVo.getResult().setCity(cityName);
                userVo.getResult().setCityId(cityId);
                userVo.getResult().setStatus(2);
                UserTools.saveUser(this, userVo);
                preferences.edit().putString(Constants.USER_CITY_NAME_TAG, userVo.getResult().getCity())
                        .putString(Constants.USER_CITY_ID_TAG, userVo.getResult().getCityId()).commit();
                this.setResult(Constants.SET_JPUSH_TAG_RESULT_CODE);
                this.finish();
            } else {
                Toast.makeText(this, userVo.getMessage(), Toast.LENGTH_SHORT).show();
            }
        }
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        ButtonUtil.setEnable(mRegister);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mCityWorkerDB != null) {
            mCityWorkerDB.closeCityWorker();
        }
    }

}

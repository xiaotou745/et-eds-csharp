package activity;

import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import utils.EtsBLog;
import utils.MD5;
import utils.StringUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;

import constant.Constants;

public class ChangePasswordActivity extends BaseActionBarActivity implements OnClickListener, HTTPListener {
    private EditText tvPhone;
    private EditText tvCord;
    private EditText tvPassword;
    private EditText tvPassword2;
    private Button btnCord;
    private Button btnOk;
    private String phoneNo;
    private String cord;
    private String password, password2;
    private VolleyUtil util;
    private ProgressDialog dialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        // TODO Auto-generated method stub
        super.onCreate(savedInstanceState);
        setContentView(R.layout.change_password_activity);
        initView();
    }

    private void initView() {
        // TODO Auto-generated method stub
        tvPhone = (EditText) findViewById(R.id.et_phone);
        tvPassword = (EditText) findViewById(R.id.et_password);
        tvPassword2 = (EditText) findViewById(R.id.et_password2);
        tvCord = (EditText) findViewById(R.id.et_cord);
        btnCord = (Button) findViewById(R.id.btn_getcord);
        btnOk = (Button) findViewById(R.id.btn_regist);
        btnCord.setOnClickListener(this);
        btnOk.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        util = new VolleyUtil(this);
        switch (v.getId()) {
        case R.id.btn_getcord:
            phoneNo = tvPhone.getText().toString();
            if (!StringUtil.isMobileNO(phoneNo)) {
                sendMsg("请输入正确的手机号");
                return;
            }
            handler.post(runnable);
            HashMap<String, String> map = new HashMap<String, String>();
            map.put("PhoneNumber", phoneNo);
            util.get(map, Constants.URL_CHANGE_PASSWORD_GetVerifyCode_B, this, 1);
            break;

        case R.id.btn_regist:
            phoneNo = tvPhone.getText().toString();
            cord = tvCord.getText().toString();
            password = tvPassword.getText().toString();
            password2 = tvPassword2.getText().toString();
            if (!StringUtil.isMobileNO(phoneNo)) {
                sendMsg("请输入正确的手机号");
                return;
            }
            if (cord.isEmpty() || cord.length() != 6) {
                sendMsg("请输入正确的验证码");
                return;
            }
            if (password.isEmpty() || password.length() < 6 || password.length() > 16) {
                sendMsg("请输入6~16位的密码");
                return;
            }
            if (!password.equals(password2)) {
                sendMsg("两次密码输入不一致");
            }
            dialog = ProgressDialog.show(this, "提示", "正在修改密码", false, false);
            String strPwd = MD5.md5(password);
            HashMap<String, String> hashMap = new HashMap<String, String>();
            hashMap.put("phoneNumber", phoneNo);
            hashMap.put("checkCode", cord);
            hashMap.put("password", strPwd);

            util.post(hashMap, Constants.URL_POST_CHANGE_PASSWORD, this, 2);
            break;
        }
    }

    // public static boolean isMobileNO(String mobiles) {
    // /*
    // * 移动：134、135、136、137、138、139、150、151、157(TD)、158、159、187、188 联通：130、131、132、152、155、156、185、186
    // * 电信：133、153、180、189、（1349卫通） 总结起来就是第一位必定为1，第二位必定为3或5或8，其他位置的可以为0-9
    // */
    // String telRegex = "[1][3578]\\d{9}";// "[1]"代表第1位为数字1，"[358]"代表第二位可以为3、5、8中的一个，"\\d{9}"代表后面是可以是0～9的数字，有9位。
    // if (TextUtils.isEmpty(mobiles))
    // return false;
    // else
    // return mobiles.matches(telRegex);
    // }

    @Override
    public void onResponse(String response, int requestCode) {
        // TODO Auto-generated method stub
        try {
            EtsBLog.d("changePwd<result>:" + response);
            JSONObject object = new JSONObject(response);
            int status = object.getInt("Status");
            String msg = object.getString("Message");
            if (status == 0) {
                switch (requestCode) {
                case 1:
                    sendMsg("验证码发送成功");
                    break;

                case 2:
                    sendMsg("密码修改成功");
                    finish();
                    break;
                }

            } else {
                sendMsg(msg);
            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            sendMsg("服务器错误");
        } finally {
            if (dialog != null)
                dialog.dismiss();
        }
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        // TODO Auto-generated method stub
        if (dialog != null)
            dialog.dismiss();
        sendMsg("服务器错误");
    }

    private int recLen = 60;
    Handler handler = new Handler();
    Runnable runnable = new Runnable() {
        @Override
        public void run() {
            recLen--;
            if (recLen > 1) {
                btnCord.setText(recLen + "秒后重新发送");
                btnCord.setEnabled(false);
                btnCord.setBackgroundResource(R.drawable.chack_shap_pre);
                handler.postDelayed(this, 1000);
            } else {
                btnCord.setText("获取验证码");
                btnCord.setEnabled(true);
                btnCord.setBackgroundResource(R.drawable.tag_shap_nor);

                recLen = 60;
            }
        }
    };

}

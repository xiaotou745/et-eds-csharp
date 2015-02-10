package activity;

import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import utils.EtsBLog;
import utils.StringUtil;
import utils.UserUtil;
import utils.VolleyUtil;
import utils.VolleyUtil.HTTPListener;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.widget.TextView;

import com.android.volley.VolleyError;
import com.supermanb.supermanb.R;

import constant.Constants;
import entitys.User;

public class OrderSatisticsActivity extends BaseActionBarActivity implements HTTPListener {
    private User user;
    private TextView todyCount, todyAmount, todyFinshCount, todyFinshAmount, monthCount, monthAmount, monthFinshCount,
            monthFinshAmount;
    private ProgressDialog dialog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        // TODO Auto-generated method stub
        super.onCreate(savedInstanceState);
        setContentView(R.layout.order_all_activity);
        actionBar.setTitle("订单统计");
        initView();
        initDate();

    }

    private void initView() {
        // TODO Auto-generated method stub
        todyCount = (TextView) findViewById(R.id.tv_tody_publish_count);
        todyAmount = (TextView) findViewById(R.id.tv_tody_publish_amount);
        todyFinshCount = (TextView) findViewById(R.id.tv_tody_finish_count);
        todyFinshAmount = (TextView) findViewById(R.id.tv_tody_finsh_amount);
        monthCount = (TextView) findViewById(R.id.tv_mouth_publish_count);
        monthAmount = (TextView) findViewById(R.id.tv_mouth_publish_amount);
        monthFinshCount = (TextView) findViewById(R.id.tv_mouth_finly_count);
        monthFinshAmount = (TextView) findViewById(R.id.tv_mouth_finly_amount);
    }

    private void initDate() {
        // TODO Auto-generated method stub
        user = UserUtil.readUser(this);
        HashMap<String, String> map = new HashMap<String, String>();
        map.put("userId", "" + user.id);
        VolleyUtil util = new VolleyUtil(this);
        dialog = ProgressDialog.show(this, "提示", "正在加载", false, false);
        util.get(map, Constants.URL_GET_ORDER_SATISTICS, this, 0);
    }

    @Override
    public void onResponse(String response, int requestCode) {
        try {
            EtsBLog.d("ordersStatistics:[" + response + "]");
            JSONObject object = new JSONObject(response);
            int status = object.getInt("Status");
            String msg = object.getString("Message");

            if (status == 0) {
                JSONObject obj = object.getJSONObject("Result");
                todyCount.setText(obj.getInt("todayPublish") + "单");
                todyAmount.setText(StringUtil.nullToDefaultValue(obj.getString("todayPublishAmount"), "0") + "元");
                todyFinshCount.setText(obj.getInt("todayDone") + "单");
                todyFinshAmount.setText(StringUtil.nullToDefaultValue(obj.getString("todayDoneAmount"), "0") + "元");
                monthCount.setText(obj.getInt("monthPublish") + "单");
                monthAmount.setText(StringUtil.nullToDefaultValue(obj.getString("monthPublishAmount"), "0") + "元");
                monthFinshCount.setText(obj.getInt("monthDone") + "单");
                monthFinshAmount.setText(StringUtil.nullToDefaultValue(obj.getString("monthDoneAmount"), "0") + "元");

            }
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            sendMsg("服务器错误");
        }

        dialog.dismiss();
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        // TODO Auto-generated method stub
        sendMsg("服务器错误");
        dialog.dismiss();
    }
}

package com.eds.supermanc.activity;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.AbsListView;
import android.widget.AbsListView.OnScrollListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.eds.supermanc.Constants;
import com.eds.supermanc.adapter.MyMoneyAdapter;
import com.eds.supermanc.beans.MoneyRecordVo;
import com.eds.supermanc.beans.MoneyRecordVo.MoneyRecord;
import com.eds.supermanc.utils.UserTools;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

public class MyMoneyActivity extends BaseActivity implements OnClickListener, HTTPListener {

    private TextView mMyMoney;
    private Button chongzhzi;
    private Button tixian;
    private int currentPage = 0;
    private ListView listView;
    private LinkedList<MoneyRecord> records = null;
    ProgressDialog dialog = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.my_money_activity);
        initView();
        initData();
        initList();
        initListener();
    }

    private void initView() {
        mMyMoney = (TextView) findViewById(R.id.myMoney);
        mMyMoney.setText(UserTools.getUser(this).getAmount() + "元");
        chongzhzi = (Button) findViewById(R.id.chongzhi);
        tixian = (Button) findViewById(R.id.tixian);
        listView = (ListView) findViewById(R.id.listView);
        initTitle();
    }

    private void initTitle() {
        LinearLayout backLayout = (LinearLayout) this.findViewById(R.id.titleLeftLayout);
        backLayout.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                MyMoneyActivity.this.finish();
            }
        });
        ((ImageView) this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
        ((TextView) this.findViewById(R.id.titleContent)).setText("我的余额");
    }

    private void initListener() {
        chongzhzi.setOnClickListener(this);
        tixian.setOnClickListener(this);
        listView.setOnScrollListener(new OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {
                switch (scrollState) {
                // 当不滚动时
                case OnScrollListener.SCROLL_STATE_IDLE:
                    // 判断滚动到底部
                    if (listView.getLastVisiblePosition() == (listView.getCount() - 1)) {
                        if (adapter.getCount() >= 10) {
                            currentPage++;
                            dialog = ProgressDialog.show(MyMoneyActivity.this, "提示", "正在加载中", false, false);
                            initList();
                        }
                    }
                    break;
                }
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
            }
        });
    }

    private void initData() {
        Map<String, String> params = new HashMap<String, String>();
        params.put("phoneNo", UserTools.getUser(this).getUserName());
        VolleyTool.get(Constants.GET_MY_BALANCE_URL, params, this, Constants.GETMYBALANCE, null);
    }

    private void initList() {
        Map<String, String> params = new HashMap<String, String>();
        params.put("phoneNo", UserTools.getUser(this).getUserName());
        params.put("pagedSize", "10");
        params.put("pagedIndex", currentPage + "");
        VolleyTool.get(Constants.GET_MY_BALANCE_DYNAMIC, params, this, Constants.GETMYBALANCEDYNAMIC,
                MoneyRecordVo.class);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
        case R.id.chongzhi:
        case R.id.tixian:
            call();
            break;
        default:
            break;
        }
    }

    private void call() {
        Uri telUri = Uri.parse("tel:400 0000 6");
        Intent intent = new Intent(Intent.ACTION_DIAL, telUri);
        startActivity(intent);
    }

    MyMoneyAdapter adapter = null;

    @Override
    public <T> void onResponse(T t, int requestCode) {
        if (requestCode == Constants.GETMYBALANCE) {
            String response = (String) t;
            try {
                JSONObject obj = new JSONObject(response);
                int code = obj.getInt("Status");
                String message = obj.getString("Message");
                if (code == 0) {
                    JSONObject json = new JSONObject(obj.getString("Result"));
                    mMyMoney.setText(json.getString("MyBalance") + "元");
                } else {
                    Toast.makeText(this, "获取最新金额失败，请稍后重试", Toast.LENGTH_SHORT).show();
                }
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        if (requestCode == Constants.GETMYBALANCEDYNAMIC) {
            MoneyRecordVo vo = (MoneyRecordVo) t;
            if (vo.getStatus() == 0) {
                if (currentPage == 0) {
                    records = vo.getResult();
                    if (records.size() > 0) {
                        adapter = new MyMoneyAdapter(this);
                        adapter.setData(vo.getResult());
                        listView.setAdapter(adapter);
                    } else {

                    }
                } else {
                    if (vo.getResult().size() > 0) {
                        dialog.dismiss();
                        for (int i = 0; i < vo.getResult().size(); i++) {
                            records.addLast(vo.getResult().get(i));
                        }
                        adapter.notifyDataSetChanged();
                    } else {
                        dialog.dismiss();
                        Toast.makeText(this, "没有更多记录了", Toast.LENGTH_SHORT).show();
                    }
                }
            } else {
                Toast.makeText(this, vo.getMessage(), Toast.LENGTH_SHORT).show();
            }
        }

    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        if (dialog != null) {
            dialog.dismiss();
        }
    }

}

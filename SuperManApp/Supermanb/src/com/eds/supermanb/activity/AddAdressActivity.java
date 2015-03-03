package com.eds.supermanb.activity;

import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.HashMap;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.baidu.mapapi.model.LatLng;
import com.baidu.mapapi.search.core.SearchResult;
import com.baidu.mapapi.search.geocode.GeoCodeOption;
import com.baidu.mapapi.search.geocode.GeoCodeResult;
import com.baidu.mapapi.search.geocode.GeoCoder;
import com.baidu.mapapi.search.geocode.OnGetGeoCoderResultListener;
import com.baidu.mapapi.search.geocode.ReverseGeoCodeResult;
import com.eds.supermanb.adapter.CityAdapter;
import com.eds.supermanb.constant.Constants;
import com.eds.supermanb.entitys.Address;
import com.eds.supermanb.entitys.User;
import com.eds.supermanb.picker.CityMode;
import com.eds.supermanb.picker.CityWorker;
import com.eds.supermanb.utils.EtsBLog;
import com.eds.supermanb.utils.StringUtil;
import com.eds.supermanb.utils.UserUtil;
import com.eds.supermanb.utils.VolleyUtil;
import com.eds.supermanb.utils.VolleyUtil.HTTPListener;
import com.supermanb.supermanb.R;

public class AddAdressActivity extends BaseActionBarActivity implements OnClickListener, OnGetGeoCoderResultListener,
        HTTPListener {
    private Spinner spinner1;
    // private Spinner spinner2;
    private EditText etDatileAdr;
    private GeoCoder mSearch;
    private Button btnMark;
    private CityAdapter adapter1;
    // private CityAdapter adapter2;
    private Button btnSave;
    private CityMode city;
    private EditText shopName;
    private EditText phoneNo;
    private EditText tel;
    private Address address;
    private User user;
    private String sshopName, sphoneNo, telPhone;
    private ProgressDialog dialog;
    private CityWorker mCityWorkerDB;
    private ArrayList<CityMode> areaData;
    private MyHandler mMyHandler;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.add_adress_activity);
        initView();
        initDate();
    }

    private void initDate() {
        mCityWorkerDB = new CityWorker(this);
        areaData = new ArrayList<CityMode>();
        city = user.address.city;
        address = new Address();
        address.city = city;
        if (mCityWorkerDB != null) {
            areaData = mCityWorkerDB.getAreaByCity(city.getPcode());
        }
        adapter1 = new CityAdapter(this, areaData);
        spinner1.setAdapter(adapter1);

    }

    private void initView() {
        user = UserUtil.readUser(this);
        actionBar.setTitle("地址管理");
        mSearch = GeoCoder.newInstance();
        mSearch.setOnGetGeoCodeResultListener(this);
        spinner1 = (Spinner) findViewById(R.id.sp_area);
        // spinner2 = (Spinner) findViewById(R.id.sp_d);ss
        etDatileAdr = (EditText) findViewById(R.id.et_detile_adr);
        findViewById(R.id.btn_save).setOnClickListener(this);
        findViewById(R.id.btn_mark).setOnClickListener(this);
        tel = (EditText) findViewById(R.id.et_telPhone);
        phoneNo = (EditText) findViewById(R.id.et_mphone);
        shopName = (EditText) findViewById(R.id.et_shopname);
        /* btnSave = (Button) findViewById(R.id.btn_save); */
        if (!StringUtil.isNullByString(user.address.detill)) {
            etDatileAdr.setText(user.address.detill);
        }
        if (!StringUtil.isNullByString(user.userPhone)) {
            phoneNo.setText(user.userPhone);
        }
        if (!StringUtil.isNullByString(user.name)) {
            shopName.setText(user.name);
        }
        if (!StringUtil.isNullByString(user.telPhone)) {
            tel.setText(user.telPhone);
        }
        mMyHandler = new MyHandler(this);
    }

    @Override
    public void onClick(View arg0) {
        Intent intent;
        switch (arg0.getId()) {
        case R.id.btn_mark:
            intent = new Intent(this, GeoCoderActivity.class);
            intent.putExtra("City", city.getName());
            intent.putExtra("Adress", adapter1.getItem((int) spinner1.getSelectedItemId()).toString()
                    + etDatileAdr.getText().toString());
            startActivity(intent);
            break;

        case R.id.btn_save:

            sshopName = shopName.getText().toString();
            sphoneNo = shopName.getText().toString();
            telPhone = tel.getText().toString();

            if (etDatileAdr.getText().toString().length() == 0) {
                sendMsg("请输入详细地址");
                return;
            }
            if (shopName.getText().toString().length() == 0) {
                sendMsg("商户名称不能为空");
                return;
            }
            if (!StringUtil.isMobileNO(phoneNo.getText().toString())) {
                sendMsg("请输入正确手机号");
                return;
            }

            if (shopName.getText().toString().length() == 0) {
                sendMsg("商户名称不能为空");
                return;
            }
            StringBuilder buffer = new StringBuilder(adapter1.getItem((int) spinner1.getSelectedItemId()).getName());
            buffer.append(etDatileAdr.getText().toString());
            address.area = adapter1.getItem((int) spinner1.getSelectedItemId());
            address.detill = etDatileAdr.getText().toString();
            dialog = ProgressDialog.show(this, "提示", "正在检查地址", false, true);
            mSearch.geocode(new GeoCodeOption().city(city.getName()).address(buffer.toString()));
            break;
        }

    }

    @Override
    public void onGetGeoCodeResult(GeoCodeResult arg0) {
        dialog.dismiss();
        if (arg0 == null || arg0.error != SearchResult.ERRORNO.NO_ERROR) {
            Toast.makeText(this, "抱歉，您的地址定位失败", Toast.LENGTH_LONG).show();
            return;
        }
        /*
         * mBaiduMap.clear(); mBaiduMap.addOverlay(new MarkerOptions().position(result.getLocation())
         * .icon(BitmapDescriptorFactory .fromResource(R.drawable.icon_marka)));
         * mBaiduMap.setMapStatus(MapStatusUpdateFactory.newLatLng(result .getLocation()));
         */
        String strInfo = String.format("纬度：%f 经度：%f", arg0.getLocation().latitude, arg0.getLocation().longitude);
        EtsBLog.d("shopAddres:[" + strInfo + "]");
        address.laitude = arg0.getLocation().latitude;
        address.longitude = arg0.getLocation().longitude;
        LatLng ptCenter = new LatLng(arg0.getLocation().latitude, arg0.getLocation().longitude);
        // 反Geo搜索

        StringBuffer buffer = new StringBuffer(shopName.getText().toString() + "\n" + address.detill + "\n"
                + phoneNo.getText().toString());
        if (tel.getText().toString() != null) {
            buffer.append("\n" + tel.getText().toString());
        }

        AlertDialog dialog = new AlertDialog.Builder(this).setTitle("确认信息").setMessage(buffer.toString())
                .setPositiveButton("确定", new android.content.DialogInterface.OnClickListener() {

                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        // TODO Auto-generated method stub

                        HashMap<String, String> parms = new HashMap<String, String>();
                        parms.put("businessName", "" + shopName.getText().toString());
                        parms.put("userId", "" + user.id);
                        parms.put("Address", "" + address.detill);

                        parms.put("phoneNo", "" + phoneNo.getText().toString());
                        parms.put("landLine", "" + tel.getText().toString());
                        parms.put("districtName", address.area.getName());
                        parms.put("districtId", "" + address.area.getPcode());
                        parms.put("latitude", "" + address.laitude);
                        parms.put("longitude", "" + address.longitude);
                        VolleyUtil util = new VolleyUtil(AddAdressActivity.this);
                        util.post(parms, Constants.URL_ADD_ADDRESS, AddAdressActivity.this, 0);
                    }
                }).setNegativeButton("取消", null).create();
        dialog.show();

        // mSearch.reverseGeoCode(new
        // ReverseGeoCodeOption().location(ptCenter));

    }

    @Override
    public void onGetReverseGeoCodeResult(ReverseGeoCodeResult arg0) {
    }

    @Override
    public void onResponse(String response, int requestCode) {
        // TODO Auto-generated method stub
        try {
            JSONObject object = new JSONObject(response);
            int status = object.getInt("Status");
            String msg = object.getString("Message");
            if (status == 0) {
                EtsBLog.d("addAdress is success");
                JSONObject object2 = object.getJSONObject("Result");

                user.userStadus = object2.getInt("status");
                user.address = address;
                user.name = sshopName;
                user.telPhone = telPhone;
                user.shopPhone = sphoneNo;
                UserUtil.saveUser(AddAdressActivity.this, user);
                sendMsg("添加地址成功");
                if (user.userStadus == 0) {
                    Intent intent = new Intent(this, VerificationActivity.class);
                    startActivity(intent);
                }
                finish();
            } else {
                sendMsg(msg);
            }

        } catch (JSONException e) {
            e.printStackTrace();
            sendMsg("服务器错误");
        }

    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        sendMsg("" + error);
    }

    @Override
    protected void onResume() {
        super.onResume();
        mMyHandler.sendEmptyMessageDelayed(0, 200);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mCityWorkerDB != null) {
            mCityWorkerDB.closeCityWorker();
        }
    }

    private void setSelectAreaIndex() {
        if (user != null && areaData != null && user.address.area != null) {
            int index = StringUtil.getIndexOfListByKey(areaData, user.address.area.getName());
            spinner1.setSelection(index);
        } else {
            spinner1.setSelection(0);
        }
    }

    private static class MyHandler extends Handler {
        private final WeakReference<Activity> mActivity;

        public MyHandler(Activity act) {
            mActivity = new WeakReference<Activity>(act);
        }

        @Override
        public void handleMessage(Message msg) {
            Activity mAct = mActivity.get();
            if (mAct != null) {
                ((AddAdressActivity) mAct).setSelectAreaIndex();
            }
        }
    }
}

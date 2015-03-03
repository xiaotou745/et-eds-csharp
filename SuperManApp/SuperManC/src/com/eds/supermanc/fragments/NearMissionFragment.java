package com.eds.supermanc.fragments;

import java.util.HashMap;
import java.util.Map;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.VolleyError;
import com.baidu.location.BDLocation;
import com.baidu.location.BDLocationListener;
import com.baidu.location.LocationClient;
import com.baidu.location.LocationClientOption;
import com.eds.supermanc.Constants;
import com.eds.supermanc.adapter.HomeMissionAdapter;
import com.eds.supermanc.beans.MissionBean;
import com.eds.supermanc.beans.UserVo.User;
import com.eds.supermanc.utils.UserTools;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

/**
 * 附近任务页面 (Description)
 * 
 * @author zaokafei
 * @version 1.0
 * @date 2015-2-28
 */
public class NearMissionFragment extends Fragment implements HTTPListener {

    private RelativeLayout mFlagmentLayout;
    private RelativeLayout loadLayout;
    private ListView listView;
    private TextView info;
    private boolean isFirstLoc = true;
    private double lati = 0d;
    private double longit = 0d;

    // 定位相关
    LocationClient mLocClient;
    public MyLocationListenner myListener = new MyLocationListenner();

    private Context mContext;

    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        // 获取对父activity的引用
        mContext = activity;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        mFlagmentLayout = (RelativeLayout) inflater.inflate(R.layout.mission_fragment, container, false);
        initView();
        initMyLocation();
        return mFlagmentLayout;
    }

    private void initView() {
        listView = (ListView) mFlagmentLayout.findViewById(R.id.listView);
        info = (TextView) mFlagmentLayout.findViewById(R.id.info);
        loadLayout = (RelativeLayout) mFlagmentLayout.findViewById(R.id.loadLayout);
        loadLayout.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
            }
        });
    }

    private void initData() {
        Map<String, String> params = new HashMap<String, String>();
        params.put("latitude", lati + "");
        params.put("longitude", longit + "");
        if (UserTools.getUser(mContext) != null) {
            User user = UserTools.getUser(mContext);
            params.put("city", user.getCity());
            params.put("cityId", user.getCityId());
        } else {
            params.put("city", "");
            params.put("cityId", "");
        }
        VolleyTool.post(Constants.GET_NEAR_MISSION_URL, params, this, Constants.GETNEARMISSION, MissionBean.class);
    }

    @Override
    public void onResume() {
        super.onResume();
        refresh();
    }

    public void refresh() {
        beforeRequest();
        if (lati != 0 && longit != 0 && !isFirstLoc) {
            initData();
        } else {
            initMyLocation();
        }
    }

    @Override
    public <T> void onResponse(T t, int requestCode) {
        afterRequestComplete();
        if (requestCode == Constants.GETNEARMISSION) {
            MissionBean mb = (MissionBean) t;
            if (mb != null && mb.getResult().size() > 0) {
                HomeMissionAdapter adapter = new HomeMissionAdapter(this.getActivity());
                adapter.setData(mb.getResult());
                listView.setAdapter(adapter);
                info.setVisibility(View.GONE);
                listView.setVisibility(View.VISIBLE);
            } else {
                info.setVisibility(View.VISIBLE);
                listView.setVisibility(View.GONE);
            }
        }
    }

    @Override
    public void onErrorResponse(VolleyError error, int requestCode) {
        afterRequestComplete();
        info.setVisibility(View.GONE);
        listView.setVisibility(View.GONE);
    }

    private void beforeRequest() {
        loadLayout.setVisibility(View.VISIBLE);
    }

    private void afterRequestComplete() {
        loadLayout.setVisibility(View.GONE);
    }

    private void initMyLocation() {
        // 定位初始化
        mLocClient = new LocationClient(this.getActivity());
        mLocClient.registerLocationListener(myListener);
        LocationClientOption option = new LocationClientOption();
        option.setOpenGps(true);// 打开gps
        option.setAddrType("all");// 定位之后 需要设置这个 才能获取city和address
        option.setCoorType("bd09ll"); // 设置坐标类型
        option.setScanSpan(1000);
        mLocClient.setLocOption(option);
        mLocClient.start();
    }

    /**
     * 定位SDK监听函数
     */
    public class MyLocationListenner implements BDLocationListener {

        @Override
        public void onReceiveLocation(BDLocation location) {
            if (location == null) {
                afterRequestComplete();
                Toast.makeText(getActivity(), "无法获取当前位置", Toast.LENGTH_SHORT).show();
                return;
            }
            if (isFirstLoc) {
                isFirstLoc = false;
                lati = location.getLatitude();
                longit = location.getLongitude();
                refresh();
            }
        }

        public void onReceivePoi(BDLocation poiLocation) {
        }
    }
}

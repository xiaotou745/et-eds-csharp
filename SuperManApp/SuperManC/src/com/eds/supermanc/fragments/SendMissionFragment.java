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

import com.android.volley.VolleyError;
import com.eds.supermanc.Constants;
import com.eds.supermanc.adapter.HomeMissionAdapter;
import com.eds.supermanc.beans.MissionBean;
import com.eds.supermanc.beans.UserVo.User;
import com.eds.supermanc.utils.EtsCLog;
import com.eds.supermanc.utils.UserTools;
import com.eds.supermanc.utils.VolleyTool;
import com.eds.supermanc.utils.VolleyTool.HTTPListener;
import com.supermanc.R;

/**
 * 送餐任务
 * 
 * @author zaokafei
 * @version 1.0
 * @date 2015-2-28
 */
public class SendMissionFragment extends Fragment implements HTTPListener {

    private RelativeLayout mFlagmentLayout;
    private RelativeLayout loadLayout;
    private ListView listView;
    private TextView info;
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
        // refresh();
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
        if (UserTools.getUser(mContext) != null) {
            User user = UserTools.getUser(mContext);
            params.put("city", user.getCity());
            params.put("cityId", user.getCityId());
        } else {
            params.put("city", "");
            params.put("cityId", "");
        }
        EtsCLog.d("SendMissionFragment:post ");
        VolleyTool.post(Constants.GET_SEND_MISSION_URL, params, this, Constants.GETSENDMISSION, MissionBean.class);
    }

    @Override
    public void onResume() {
        super.onResume();
        refresh();
    }

    public void refresh() {
        beforeRequest();
        initData();
    }

    @Override
    public <T> void onResponse(T t, int requestCode) {
        afterRequestComplete();
        if (requestCode == Constants.GETSENDMISSION) {
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
}

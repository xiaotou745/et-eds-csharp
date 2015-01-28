package com.supermanc.fragments;

import com.android.volley.VolleyError;
import com.supermanc.Constants;
import com.supermanc.R;
import com.supermanc.adapter.HomeMissionAdapter;
import com.supermanc.beans.MissionBean;
import com.supermanc.utils.VolleyTool;
import com.supermanc.utils.VolleyTool.HTTPListener;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class LatestMissionFragment extends Fragment implements HTTPListener{
	
	private RelativeLayout mFlagmentLayout;
	private RelativeLayout loadLayout;
	private ListView listView;
	private TextView info;
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		mFlagmentLayout = (RelativeLayout)inflater.inflate(R.layout.mission_fragment, container, false);
		initView();
		refresh();
		return mFlagmentLayout;
	}
	
	private void initView(){
		listView = (ListView)mFlagmentLayout.findViewById(R.id.listView);
		info = (TextView)mFlagmentLayout.findViewById(R.id.info);
		loadLayout = (RelativeLayout)mFlagmentLayout.findViewById(R.id.loadLayout);
		loadLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {}
		});
	}
	
	private void initData(){
		VolleyTool.get(Constants.GET_LATEST_MISSION_URL, null, this, Constants.GETLATESTMISSION, MissionBean.class);
	}

	@Override
	public void onResume() {
		super.onResume();
		refresh();
	}

	public void refresh(){
		beforeRequest();
		initData();
	}
	
	@Override
	public <T> void onResponse(T t, int requestCode) {
		afterRequestComplete();
		if(requestCode == Constants.GETLATESTMISSION){
			MissionBean mb = (MissionBean)t;
			if(mb != null && mb.getResult().size() > 0){
				HomeMissionAdapter adapter = new HomeMissionAdapter(this.getActivity());
				adapter.setData(mb.getResult());
				listView.setAdapter(adapter);
				info.setVisibility(View.GONE);
				listView.setVisibility(View.VISIBLE);
			}else{
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
	
	private void beforeRequest(){
		loadLayout.setVisibility(View.VISIBLE);
	}
	
	private void afterRequestComplete(){
		loadLayout.setVisibility(View.GONE);
	}
}

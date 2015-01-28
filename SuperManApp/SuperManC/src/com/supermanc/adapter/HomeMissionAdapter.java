package com.supermanc.adapter;

import java.util.ArrayList;
import com.supermanc.R;
import com.supermanc.activity.BaiduMapActivity;
import com.supermanc.activity.LoginActivity;
import com.supermanc.activity.MissionDetailActivity;
import com.supermanc.beans.MissionBean;
import com.supermanc.utils.UserTools;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;

public class HomeMissionAdapter extends BaseAdapter{

	private ArrayList<MissionBean.Mission> missions;
	private Context context;
	
	public HomeMissionAdapter(Context context){
		this.context = context;
	}
	
	public void setData(ArrayList<MissionBean.Mission> missions){
		this.missions = missions;
	}
	
	@Override
	public int getCount() {
		return missions.size();
	}

	@Override
	public Object getItem(int position) {
		return missions.get(position);
	}

	@Override
	public long getItemId(int position) {
		return position;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		Holder holder;
		if(convertView == null){
			convertView = LayoutInflater.from(context).inflate(R.layout.home_mission_adapter, null);
			holder = new Holder();
			holder.income = (TextView)convertView.findViewById(R.id.incomeMoney);
			holder.distance = (TextView)convertView.findViewById(R.id.distance);
			holder.publishTime = (TextView)convertView.findViewById(R.id.incomeTime);
			holder.fromShopName = (TextView)convertView.findViewById(R.id.shopName);
			holder.fromAddress = (TextView)convertView.findViewById(R.id.shopAddress);
			holder.toUserName = (TextView)convertView.findViewById(R.id.toUserName);
			holder.toUserAddress = (TextView)convertView.findViewById(R.id.toUserAddress);
			holder.money = (TextView)convertView.findViewById(R.id.orderMoney);
			holder.fromAddressLayout = (LinearLayout)convertView.findViewById(R.id.fromAddressIcon);
			holder.toAddressLayout = (LinearLayout)convertView.findViewById(R.id.toAddressIcon);
			convertView.setTag(holder);
		}else{
			holder = (Holder)convertView.getTag();
		}
		final MissionBean.Mission mb = missions.get(position);
		holder.income.setText(mb.getIncome());
		holder.distance.setText(mb.getDistance());
		holder.publishTime.setText(mb.getPubDate());
		holder.fromShopName.setText(mb.getBusinessName());
		holder.fromAddress.setText(mb.getPickUpCity()+mb.getPickUpAddress());
		holder.toUserName.setText(mb.getReceviceName());
		holder.toUserAddress.setText(mb.getReceviceCity()+mb.getReceviceAddress());
		holder.money.setText(mb.getAmount());
		holder.fromAddressLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent();
				intent.setClass(context, BaiduMapActivity.class);
				intent.putExtra("city", mb.getPickUpCity());
				intent.putExtra("detailAddress", mb.getPickUpAddress());
				context.startActivity(intent);
			}
		});
		holder.toAddressLayout.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent();
				intent.setClass(context, BaiduMapActivity.class);
				intent.putExtra("city", mb.getReceviceCity());
				intent.putExtra("detailAddress", mb.getReceviceAddress());
				context.startActivity(intent);
			}
		});
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent();
				if(UserTools.getUser(context) == null){
					intent.setClass(context, LoginActivity.class);
				}else{
					intent.putExtra("mission", mb);
					intent.setClass(context, MissionDetailActivity.class);
				}
				context.startActivity(intent);
			}
		});
		return convertView;
	}

	private class Holder{
		private TextView income;
		private TextView distance;
		private TextView publishTime;
		private TextView fromShopName;
		private TextView fromAddress;
		private TextView toUserName;
		private TextView toUserAddress;
		private TextView money;
		private LinearLayout fromAddressLayout;
		private LinearLayout toAddressLayout;
	}
	
}

package com.eds.supermanc.adapter;

import java.util.ArrayList;

import com.eds.supermanc.activity.MissionDetailActivity;
import com.eds.supermanc.beans.MissionBean;
import com.eds.supermanc.beans.MissionBean.Mission;
import com.supermanc.R;
import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class MyMissionAdapter extends BaseAdapter{

	private ArrayList<MissionBean.Mission> missions;
	private Context context;
	
	public MyMissionAdapter(Context context){
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
			convertView = LayoutInflater.from(context).inflate(R.layout.my_mission_adapter, null);
			holder = new Holder();
			holder.fromShopName = (TextView)convertView.findViewById(R.id.shopName);
			holder.fromAddress = (TextView)convertView.findViewById(R.id.shopAddress);
			holder.toUserName = (TextView)convertView.findViewById(R.id.toUserName);
			holder.toUserAddress = (TextView)convertView.findViewById(R.id.toUserAddress);
			holder.money = (TextView)convertView.findViewById(R.id.orderMoney);
			holder.statusIcon = (ImageView)convertView.findViewById(R.id.statusIcon);
			holder.statusMessage = (TextView)convertView.findViewById(R.id.statusMessage);
			convertView.setTag(holder);
		}else{
			holder = (Holder)convertView.getTag();
		}
		final Mission mb = missions.get(position);
		holder.fromShopName.setText(mb.getBusinessName());
		holder.fromAddress.setText(mb.getPickUpCity()+mb.getPickUpAddress());
		holder.toUserName.setText(mb.getReceviceName());
		holder.toUserAddress.setText(mb.getReceviceCity()+mb.getReceviceAddress());
		holder.money.setText(mb.getAmount()+"元");
		
		if(mb.getStatus() == 1){
			holder.statusIcon.setBackgroundResource(R.drawable.b_o1);
			holder.statusMessage.setText("已完成");
		}else if(mb.getStatus() == 2){
			holder.statusIcon.setBackgroundResource(R.drawable.b_o2);
			holder.statusMessage.setText("已接单");
		}else if(mb.getStatus() == 3){
			holder.statusIcon.setBackgroundResource(R.drawable.b_o3);
			holder.statusMessage.setText("已取消");
		}
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent();
				intent.putExtra("mission", mb);
				intent.setClass(context, MissionDetailActivity.class);
				context.startActivity(intent);
			}
		});
		return convertView;
	}

	private class Holder{
		private ImageView statusIcon;
		private TextView statusMessage;
		private TextView fromShopName;
		private TextView fromAddress;
		private TextView toUserName;
		private TextView toUserAddress;
		private TextView money;
	}
	
}

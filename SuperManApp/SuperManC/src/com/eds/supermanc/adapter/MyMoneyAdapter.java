package com.eds.supermanc.adapter;

import java.util.LinkedList;

import com.eds.supermanc.beans.MoneyRecordVo.MoneyRecord;
import com.supermanc.R;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class MyMoneyAdapter extends BaseAdapter{

	private LinkedList<MoneyRecord> records;
	private Context context;
	public MyMoneyAdapter() {
		// TODO Auto-generated constructor stub
	}
	public MyMoneyAdapter(Context context){
		this.context = context;
	}
	
	public void setData(LinkedList<MoneyRecord> records){
		this.records = records;
	}
	
	@Override
	public int getCount() {
		return records.size();
	}

	@Override
	public Object getItem(int position) {
		return records.get(position);
	}

	@Override
	public long getItemId(int position) {
		return position;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		Holder holder;
		if(convertView == null){
			convertView = LayoutInflater.from(context).inflate(R.layout.my_money_adapter, null);
			holder = new Holder();
			holder.name = (TextView)convertView.findViewById(R.id.name);
			holder.money = (TextView)convertView.findViewById(R.id.money);
			holder.date = (TextView)convertView.findViewById(R.id.date);
			convertView.setTag(holder);
		}else{
			holder = (Holder)convertView.getTag();
		}
		final MoneyRecord mr = records.get(position);
		holder.name.setText(mr.getMyIncomeName());
		holder.money.setText(mr.getMyInComeAmount());
		holder.date.setText(mr.getInsertTime());
		return convertView;
	}

	private class Holder{
		private TextView name;
		private TextView money;
		private TextView date;
	}
	
}

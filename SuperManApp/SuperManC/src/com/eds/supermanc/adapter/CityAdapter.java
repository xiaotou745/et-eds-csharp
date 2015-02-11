package com.eds.supermanc.adapter;

import java.util.ArrayList;

import android.R;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.eds.supermanc.picker.CityMode;

public class CityAdapter extends BaseAdapter {

    private Context context;
    private ArrayList<CityMode> citys;

    public CityAdapter(Context context, ArrayList<CityMode> citys) {
        this.citys = citys;
        this.context = context;
    }

    @Override
    public int getCount() {
        // TODO Auto-generated method stub
        return citys.size();
    }

    @Override
    public CityMode getItem(int arg0) {
        // TODO Auto-generated method stub
        return citys.get(arg0);
    }

    @Override
    public long getItemId(int arg0) {
        // TODO Auto-generated method stub
        return arg0;
    }

    ViewHolder holder;

    @Override
    public View getView(int arg0, View arg1, ViewGroup arg2) {
        if (arg1 == null) {
            holder = new ViewHolder();
            arg1 = LayoutInflater.from(context).inflate(R.layout.simple_spinner_item, null);
            holder.textView = (TextView) arg1;
            arg1.setTag(holder);

        } else {
            holder = (ViewHolder) arg1.getTag();
        }

        holder.textView.setText(getItem(arg0).getName().toString().trim());

        return arg1;
    }

    class ViewHolder {
        TextView textView;
    }

}

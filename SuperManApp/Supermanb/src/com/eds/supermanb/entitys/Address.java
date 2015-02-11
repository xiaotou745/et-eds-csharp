package com.eds.supermanb.entitys;

import java.io.Serializable;

import com.eds.supermanb.picker.CityMode;


public class Address implements Serializable {
public 	int id ;
	public CityMode city;
	public CityMode area;
	public String detill;
	public double longitude;
	public double laitude;
	public Address(){
		city = new CityMode();
		area = new CityMode();
	}
	@Override
	public String toString() {
		StringBuffer strBuf = new StringBuffer();
		strBuf.append("Address:")
		.append("[")		
		.append("id=").append(String.valueOf(id)).append(",")
		.append("city=").append(city.toString()).append(",")
		.append("area=").append(area.toString()).append(",")
		.append("longitude=").append(String.valueOf(longitude)).append(",")
		.append("laitude=").append(String.valueOf(laitude)).append("]");
		return strBuf.toString();
	}
	

}

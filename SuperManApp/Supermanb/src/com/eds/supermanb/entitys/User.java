package com.eds.supermanb.entitys;

import java.io.Serializable;

public class User implements Serializable {
	public int id;
	
	
	/**
	 * 0 未审核
	 * 1 已通过
	 * 2 未审核且未添加地址
	 * 3 审核中
	 * 4 审核被拒绝
	 */
	
	public int userStadus;
	public String userPhone;
	public String shopPhone;
	public String telPhone;
	public String name;
	public String password;
	public Address address;
	public User(){
		address = new Address();
	}
	@Override
	public String toString() {
		StringBuffer strBuf = new StringBuffer();
		strBuf.append("User:")
		.append("[")
		.append("userStadus=").append(String.valueOf(userStadus)).append(",")
		.append("userPhone=").append(userPhone).append(",")
		.append("shopPhone=").append(shopPhone).append(",")
		.append("telPhone=").append(telPhone).append(",")
		.append("name=").append(name).append(",")
		.append("address=").append(address.toString())
		.append("]");
		return strBuf.toString();
	}
	

}

package com.supermanc.beans;

import java.io.Serializable;
import java.util.ArrayList;

public class MissionBean implements Serializable{

	private int Status;
	private String Message;
	private ArrayList<Mission> Result;
	
	public int getStatus() {
		return Status;
	}

	public void setStatus(int status) {
		Status = status;
	}

	public String getMessage() {
		return Message;
	}

	public void setMessage(String message) {
		Message = message;
	}

	public ArrayList<Mission> getResult() {
		return Result;
	}

	public void setResult(ArrayList<Mission> result) {
		Result = result;
	}

	@Override
	public String toString() {
		return "MissionBean [Status=" + Status + ", Message=" + Message
				+ ", Result=" + Result + "]";
	}

	public class Mission implements Serializable{
		private String userId;
		private String OrderNo;
		private String income;
		private String distance;
		private String pubDate;
		private String businessName;
		private String pickUpCity;
		private String pickUpAddress;
		private String businessPhone;
		private String receviceName;
		private String receviceCity;
		private String receviceAddress;
		private String recevicePhone;
		private String Amount;
		private boolean IsPay;
		private String Remark;
		private int Status;    // 0待抢单 1订单完成 2已接单 3单取消
		private String distanceB2R;
		
		public String getDistanceB2R() {
			return distanceB2R;
		}
		public void setDistanceB2R(String distanceB2R) {
			this.distanceB2R = distanceB2R;
		}
		public String getPickUpCity() {
			return pickUpCity;
		}
		public void setPickUpCity(String pickUpCity) {
			this.pickUpCity = pickUpCity;
		}
		public String getReceviceCity() {
			return receviceCity;
		}
		public void setReceviceCity(String receviceCity) {
			this.receviceCity = receviceCity;
		}
		public String getUserId() {
			return userId;
		}
		public void setUserId(String userId) {
			this.userId = userId;
		}
		public String getOrderNo() {
			return OrderNo;
		}
		public void setOrderNo(String orderNo) {
			OrderNo = orderNo;
		}
		public String getIncome() {
			return income;
		}
		public void setIncome(String income) {
			this.income = income;
		}
		public String getDistance() {
			return distance;
		}
		public void setDistance(String distance) {
			this.distance = distance;
		}
		public String getPubDate() {
			return pubDate;
		}
		public void setPubDate(String pubDate) {
			this.pubDate = pubDate;
		}
		public String getBusinessName() {
			return businessName;
		}
		public void setBusinessName(String businessName) {
			this.businessName = businessName;
		}
		public String getPickUpAddress() {
			return pickUpAddress;
		}
		public void setPickUpAddress(String pickUpAddress) {
			this.pickUpAddress = pickUpAddress;
		}
		public String getBusinessPhone() {
			return businessPhone;
		}
		public void setBusinessPhone(String businessPhone) {
			this.businessPhone = businessPhone;
		}
		public String getReceviceName() {
			return receviceName;
		}
		public void setReceviceName(String receviceName) {
			this.receviceName = receviceName;
		}
		public String getReceviceAddress() {
			return receviceAddress;
		}
		public void setReceviceAddress(String receviceAddress) {
			this.receviceAddress = receviceAddress;
		}
		public String getRecevicePhone() {
			return recevicePhone;
		}
		public void setRecevicePhone(String recevicePhone) {
			this.recevicePhone = recevicePhone;
		}
		public String getAmount() {
			return Amount;
		}
		public void setAmount(String amount) {
			Amount = amount;
		}
		public boolean isIsPay() {
			return IsPay;
		}
		public void setIsPay(boolean isPay) {
			IsPay = isPay;
		}
		public String getRemark() {
			return Remark;
		}
		public void setRemark(String remark) {
			Remark = remark;
		}
		public int getStatus() {
			return Status;
		}
		public void setStatus(int status) {
			Status = status;
		}
		
		@Override
		public String toString() {
			return "Mission [userId=" + userId + ", OrderNo=" + OrderNo
					+ ", income=" + income + ", distance=" + distance
					+ ", pubDate=" + pubDate + ", businessName=" + businessName
					+ ", pickUpCity=" + pickUpCity + ", pickUpAddress="
					+ pickUpAddress + ", businessPhone=" + businessPhone
					+ ", receviceName=" + receviceName + ", receviceCity="
					+ receviceCity + ", receviceAddress=" + receviceAddress
					+ ", recevicePhone=" + recevicePhone + ", Amount=" + Amount
					+ ", IsPay=" + IsPay + ", Remark=" + Remark + ", Status="
					+ Status + ", distanceB2R=" + distanceB2R + "]";
		}
		
	}
}

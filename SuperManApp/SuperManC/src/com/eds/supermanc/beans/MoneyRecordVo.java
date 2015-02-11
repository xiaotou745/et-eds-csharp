package com.eds.supermanc.beans;

import java.util.LinkedList;

public class MoneyRecordVo {

	private int Status;
	private String Message;
	private LinkedList<MoneyRecord> Result;
	
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

	public LinkedList<MoneyRecord> getResult() {
		return Result;
	}

	public void setResult(LinkedList<MoneyRecord> result) {
		Result = result;
	}

	@Override
	public String toString() {
		return "MoneyRecordVo [Status=" + Status + ", Message=" + Message
				+ ", Result=" + Result + "]";
	}

	public class MoneyRecord{
		private String MyIncomeName;
		private String MyInComeAmount;
		private String InsertTime;
		public String getMyIncomeName() {
			return MyIncomeName;
		}
		public void setMyIncomeName(String myIncomeName) {
			MyIncomeName = myIncomeName;
		}
		public String getMyInComeAmount() {
			return MyInComeAmount;
		}
		public void setMyInComeAmount(String myInComeAmount) {
			MyInComeAmount = myInComeAmount;
		}
		public String getInsertTime() {
			return InsertTime;
		}
		public void setInsertTime(String insertTime) {
			InsertTime = insertTime;
		}
		@Override
		public String toString() {
			return "MoneyRecord [MyIncomeName=" + MyIncomeName
					+ ", MyInComeAmount=" + MyInComeAmount + ", InsertTime="
					+ InsertTime + "]";
		}
		
	}
}

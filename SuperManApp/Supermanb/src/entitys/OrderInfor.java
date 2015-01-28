package entitys;

import java.io.Serializable;

public class OrderInfor implements Serializable {
	
public String orderId;	
public String puvlishTime;
public String consignee;
public String consigneePhone;
public  String consigneeAddress;
public String distributingFee;
public double amount;
public int orderStadus;
public String deliveryman;
public String deliverymanPhone;
public String deliveryAddress;
//配送说明
public String remark;
public  boolean isPay;
public  String shopName;
public  String shopAddress;

public  String reciverTime;
public double distance;
@Override
public String toString() {
	StringBuffer sbf = new StringBuffer();
	sbf.append("orderinfo:")
	.append("[")
	.append("orderId=").append(String.valueOf(orderId)).append(",")
	.append("puvlishTime=").append(String.valueOf(puvlishTime)).append(",")
	.append("consignee=").append(String.valueOf(consignee)).append(",")
	.append("consigneePhone=").append(String.valueOf(consigneePhone)).append(",")
	.append("consigneeAddress=").append(String.valueOf(consigneeAddress)).append(",")
	.append("distributingFee=").append(String.valueOf(distributingFee)).append(",")
	.append("amount=").append(String.valueOf(amount)).append(",")
	.append("orderStadus=").append(String.valueOf(orderStadus)).append(",")
	.append("deliveryman=").append(String.valueOf(deliveryman)).append(",")
	.append("deliverymanPhone=").append(String.valueOf(deliverymanPhone)).append(",")
	.append("deliveryAddress=").append(String.valueOf(deliveryAddress)).append(",")
	.append("remark=").append(String.valueOf(remark)).append(",")
	.append("isPay=").append(String.valueOf(isPay)).append(",")
	.append("shopName=").append(String.valueOf(shopName)).append(",")
	.append("shopAddress=").append(String.valueOf(shopAddress)).append(",")
	.append("reciverTime=").append(String.valueOf(reciverTime)).append(",")
	.append("distance=").append(String.valueOf(distance))
	.append("]");	
	return sbf.toString();
}


/*distance,
pubDate,
businessName,
pickUpAddress,
businessPhone,
receviceName,
receviceAddress,
recevicePhone,
superManName,
superManPhone,
Amount,
IsPay,
Remark,
Status*/


}

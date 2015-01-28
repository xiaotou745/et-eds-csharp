package utils;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.StreamCorruptedException;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.util.Base64;
import android.widget.ImageView;
import android.widget.TextView;

import com.supermanb.supermanb.R;

import entitys.OrderInfor;
import entitys.User;

public class UserUtil {
    public static User readUser(Context context) {
        User user = null;
        SharedPreferences preferences = context.getSharedPreferences("base64", Activity.MODE_PRIVATE);
        String productBase64 = preferences.getString("user", "utf-8");
        try {
            // 读取字节
            byte[] base64 = Base64.decode(productBase64.getBytes(), Base64.DEFAULT);

            // 封装到字节流
            ByteArrayInputStream bais = new ByteArrayInputStream(base64);

            // 再次封装
            ObjectInputStream bis = new ObjectInputStream(bais);
            try {
                // 读取对象
                user = (User) bis.readObject();
            } catch (ClassNotFoundException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        } catch (StreamCorruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return user;
    }

    public static void saveUser(Context context, User user) {
        SharedPreferences preferences = context.getSharedPreferences("base64", Activity.MODE_PRIVATE);
        // 创建字节输出流
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        try {
            // 创建对象输出流，并封装字节流
            ObjectOutputStream oos = new ObjectOutputStream(baos);
            // 将对象写入字节流
            oos.writeObject(user);
            // 将字节流编码成base64的字符窜
            String oAuth_Base64 = new String(Base64.encode(baos.toByteArray(), Base64.DEFAULT));
            Editor editor = preferences.edit();
            editor.putString("user", oAuth_Base64);

            editor.commit();
        } catch (IOException e) {
            // TODO Auto-generated
        }
    }

    public static void logOut(Context context) {
        SharedPreferences preferences = context.getSharedPreferences("base64", Activity.MODE_PRIVATE);
        Editor editor = preferences.edit();
        editor.putString("user", "");
        editor.commit();
    }

    public static void setOrderStatus(Context context, OrderInfor infor, TextView orderStatus, ImageView imvStatus) {
        switch (infor.orderStadus) {
        case 0:
            orderStatus.setText("待接单");
            imvStatus.setImageDrawable(context.getResources().getDrawable(R.drawable.orderstatus_0));
            orderStatus.setTextColor(context.getResources().getColor(R.color.orderstatus_0));

            break;
        case 1:
            orderStatus.setText("已完成");
            imvStatus.setImageDrawable(context.getResources().getDrawable(R.drawable.orderstatus_1));
            orderStatus.setTextColor(context.getResources().getColor(R.color.orderstatus_1));
            break;
        case 2:
            orderStatus.setText("已接单");
            imvStatus.setImageDrawable(context.getResources().getDrawable(R.drawable.orderstatus_2));
            orderStatus.setTextColor(context.getResources().getColor(R.color.orderstatus_2));
            break;
        case 3:
            orderStatus.setText("已取消");
            imvStatus.setImageDrawable(context.getResources().getDrawable(R.drawable.orderstatus_3));
            orderStatus.setTextColor(context.getResources().getColor(R.color.orderstatus_3));
            break;

        }
    }

}

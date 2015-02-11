package com.eds.supermanb.activity;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.ThumbnailUtils;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.ImageView;
import android.widget.Toast;

import com.eds.supermanb.constant.Constants;
import com.eds.supermanb.entitys.User;
import com.eds.supermanb.utils.ImageUploadAsyncTask;
import com.eds.supermanb.utils.UserUtil;
import com.eds.supermanb.utils.HttpRequest.HttpRequestListener;
import com.supermanb.supermanb.R;


public class VerificationActivity extends BaseActionBarActivity implements
		HttpRequestListener {

	/**
	 * 去上传文件
	 */
	protected static final int TO_UPLOAD_FILE = 1;
	/**
	 * 上传文件响应
	 */
	protected static final int UPLOAD_FILE_DONE = 2; //
	/**
	 * 选择文件
	 */
	public static final int TO_SELECT_PHOTO = 3;
	/**
	 * 上传初始化
	 */
	private static final int UPLOAD_INIT_PROCESS = 4;
	/**
	 * 上传中
	 */
	private static final int UPLOAD_IN_PROCESS = 5;

	private ImageView imvEg, imvUploding;
	private String picPath;
	private Bitmap bitmap;
	private User user;
	private ProgressDialog dialog;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.verification_activity);
		actionBar.setTitle("e代送商家验证");
		user = UserUtil.readUser(this);
		initView();

	}

	private void toUploadFile() {
		/*
		 * uploadImageResult.setText("正在上传中...");
		 * progressDialog.setMessage("正在上传文件..."); progressDialog.show();
		 */
		/*
		 * String fileKey = "pic"; UploadUtil uploadUtil =
		 * UploadUtil.getInstance();;
		 * uploadUtil.setOnUploadProcessListener(this); //设置监听器监听上传状态
		 * 
		 * Map<String, String> params = new HashMap<String, String>();
		 * params.put("UserId", "11111"); uploadUtil.uploadFile(
		 * picPath,fileKey,
		 * "http://10.8.8.148:8091/BusinessAPI/PostLogin_B",params);
		 */
	}

	private void initView() {
		imvEg = (ImageView) findViewById(R.id.imv_eg);
		imvUploding = (ImageView) findViewById(R.id.imv_uploding);
		imvUploding.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				Intent intent = new Intent(VerificationActivity.this,
						SelectPicActivity.class);
				startActivityForResult(intent, TO_SELECT_PHOTO);
			}
		});
		findViewById(R.id.btn_regist).setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if (bitmap != null) {

					// String file = new String(Base64Coder.encodeLines(data2));
					File file2 = null;
					if (picPath != null) {
						file2 = new File(picPath);
					}

					Map<String, String> params = new HashMap<String, String>();
					params.put("UserId", "" + user.id);
					//params.put("UserId", "" + 0);

					Map<String, File> files = new HashMap<String, File>();
					files.put("uploadfile", file2);
					dialog = ProgressDialog.show(VerificationActivity.this,
							"提示", "正在上传", false, false);
					ImageUploadAsyncTask iuat = new ImageUploadAsyncTask(
							params, files,
							Constants.host + Constants.URL_AUDIT,
							VerificationActivity.this);
					iuat.execute();

				} else {
					Toast.makeText(VerificationActivity.this, "上传的文件路径出错",
							Toast.LENGTH_LONG).show();
				}
			}
		});

	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (resultCode == Activity.RESULT_OK && requestCode == TO_SELECT_PHOTO) {
			picPath = data.getStringExtra(SelectPicActivity.KEY_PHOTO_PATH);
			Uri uri = Uri.parse(picPath);

			String[] pojo = { MediaStore.Images.Media.DATA };
			Cursor cursor = managedQuery(uri, pojo, null, null, null);

			if (cursor != null) {
				int columnIndex = cursor.getColumnIndexOrThrow(pojo[0]);
				cursor.moveToFirst();
				picPath = cursor.getString(columnIndex);

			}

			try {
				InputStream stream = getContentResolver().openInputStream(uri);
				bitmap = BitmapFactory.decodeStream(stream);
				LayoutParams params = imvUploding.getLayoutParams();
				/*
				 * params.height = params.width;
				 * imvUploding.setLayoutParams(params);
				 */

				imvUploding.setImageBitmap(ThumbnailUtils.extractThumbnail(
						bitmap, imvEg.getHeight(), imvEg.getHeight()));
			} catch (FileNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			// Bitmap bm = BitmapFactory.decodeFile(picPath);

		}
		super.onActivityResult(requestCode, resultCode, data);
	}

	@Override
	public void httpError() {
		// TODO Auto-generated method stub
		sendMsg("图片上传失败");
		dialog.dismiss();
	}

	@Override
	public void httpSuccess(String msg) {

		try {
			JSONObject object = new JSONObject(msg);
			int status = object.getInt("Status");
			String message = object.getString("Message");
			if (status == 0) {
				JSONObject object2 = object.getJSONObject("Result");

				sendMsg("图片上传成功，正在审核");
				user.userStadus = object2.getInt("status");
				UserUtil.saveUser(this, user);
				Intent intent = new Intent(this, HomeActivity.class);
				startActivity(intent);
				finish();

			} else {
				sendMsg(message);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			dialog.dismiss();
		}
	}
}

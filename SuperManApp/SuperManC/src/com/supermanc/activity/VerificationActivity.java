package com.supermanc.activity;

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
import android.os.Handler;
import android.os.Message;
import android.provider.MediaStore;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.supermanc.Constants;
import com.supermanc.R;
import com.supermanc.beans.UserVo;
import com.supermanc.beans.UserVo.User;
import com.supermanc.utils.HttpRequest.HttpRequestListener;
import com.supermanc.utils.ImageUploadAsyncTask;
import com.supermanc.utils.UserTools;

public class VerificationActivity extends BaseActivity implements HttpRequestListener {

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
     * 选择第二章图片
     */
    public static final int TO_SELECT_PHOTO_SECOND = 6;
    /**
     * 上传初始化
     */
    private static final int UPLOAD_INIT_PROCESS = 4;
    /**
     * 上传中
     */
    private static final int UPLOAD_IN_PROCESS = 5;

    private ImageView imvUploding, imvUploding1;
    private String picPath, picPath1;
    private ProgressBar progressBar;
    private Bitmap bitmap, bitmap1;
    ProgressDialog dialog = null;
    private EditText trueName;
    private EditText idNo;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.verification_activity);
        initView();
        initTitle();
    }

    private void initTitle() {
        LinearLayout backLayout = (LinearLayout) this.findViewById(R.id.titleLeftLayout);
        backLayout.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                VerificationActivity.this.finish();
            }
        });
        ((ImageView) this.findViewById(R.id.titleLogo)).setVisibility(View.VISIBLE);
        ((TextView) this.findViewById(R.id.titleContent)).setText("身份验证");
    }

    private Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what) {
            case TO_UPLOAD_FILE:
                // toUploadFile();
                break;

            case UPLOAD_INIT_PROCESS:
                progressBar.setMax(msg.arg1);
                break;
            case UPLOAD_IN_PROCESS:
                progressBar.setProgress(msg.arg1);
                break;
            case UPLOAD_FILE_DONE:
                break;
            default:
                break;
            }
            super.handleMessage(msg);
        }

    };

    private void initView() {
        trueName = (EditText) findViewById(R.id.trueName);
        idNo = (EditText) findViewById(R.id.idNo);

        imvUploding = (ImageView) findViewById(R.id.imv_uploding);
        imvUploding1 = (ImageView) findViewById(R.id.imv_uploding1);

        progressBar = (ProgressBar) findViewById(R.id.progressBar1);
        imvUploding.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                // TODO Auto-generated method stub
                Intent intent = new Intent(VerificationActivity.this, SelectPicActivity.class);
                startActivityForResult(intent, TO_SELECT_PHOTO);
            }
        });
        imvUploding1.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(VerificationActivity.this, SelectPicActivity.class);
                startActivityForResult(intent, TO_SELECT_PHOTO_SECOND);
            }
        });
        findViewById(R.id.btnVerify).setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                if (bitmap != null) {
                    File file2 = null;
                    if (picPath != null) {
                        file2 = new File(picPath);
                    }
                    File file3 = new File(picPath1);

                    Map<String, String> params = new HashMap<String, String>();
                    String tn = trueName.getText().toString();
                    String idc = idNo.getText().toString();
                    if ("".equals(tn)) {
                        Toast.makeText(VerificationActivity.this, "请输入真实姓名", Toast.LENGTH_SHORT).show();
                        return;
                    }
                    if ("".equals(idc)) {
                        Toast.makeText(VerificationActivity.this, "请输入身份证", Toast.LENGTH_SHORT).show();
                        return;
                    }
                    params.put("IDCard", idc);
                    params.put("trueName", tn);
                    params.put("userId", UserTools.getUser(VerificationActivity.this).getUserId());

                    Map<String, File> files = new HashMap<String, File>();
                    files.put("uploadfile", file2);
                    files.put("uploadfile1", file3);

                    dialog = ProgressDialog.show(VerificationActivity.this, "提示", "正在加载中", false, false);
                    ImageUploadAsyncTask iuat = new ImageUploadAsyncTask(params, files, Constants.UPLOAD_FILES,
                            VerificationActivity.this);
                    iuat.execute();
                } else {
                    Toast.makeText(VerificationActivity.this, "上传的文件路径出错", Toast.LENGTH_LONG).show();
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
                // cursor.close();
            }

            try {
                InputStream stream = getContentResolver().openInputStream(uri);
                bitmap = BitmapFactory.decodeStream(stream);
                imvUploding.setImageBitmap(ThumbnailUtils.extractThumbnail(bitmap, imvUploding.getWidth(),
                        imvUploding.getWidth()));
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }

        }
        if (resultCode == Activity.RESULT_OK && requestCode == TO_SELECT_PHOTO_SECOND) {
            picPath1 = data.getStringExtra(SelectPicActivity.KEY_PHOTO_PATH);
            Uri uri = Uri.parse(picPath1);

            String[] pojo = { MediaStore.Images.Media.DATA };
            Cursor cursor = managedQuery(uri, pojo, null, null, null);
            if (cursor != null) {
                int columnIndex = cursor.getColumnIndexOrThrow(pojo[0]);
                cursor.moveToFirst();
                picPath1 = cursor.getString(columnIndex);
                // cursor.close();
            }

            try {
                InputStream stream = getContentResolver().openInputStream(uri);
                bitmap1 = BitmapFactory.decodeStream(stream);
                imvUploding1.setImageBitmap(ThumbnailUtils.extractThumbnail(bitmap1, imvUploding1.getWidth(),
                        imvUploding1.getWidth()));
            } catch (FileNotFoundException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
        super.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void httpError() {
        if (dialog != null) {
            dialog.dismiss();
        }
        Toast.makeText(this, "上传失败，请稍后重试", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void httpSuccess(String msg) {
        if (dialog != null) {
            dialog.dismiss();
        }
        try {
            JSONObject obj = new JSONObject(msg);
            int status = obj.getInt("Status");
            String message = obj.getString("Message");
            if (status == 0) {
                Toast.makeText(this, "上传成功，请等待后台工作人员审核", Toast.LENGTH_LONG).show();
                UserVo vo = new UserVo();
                User user = UserTools.getUser(this);
                user.setStatus(3);// 设置正在审核中
                vo.setResult(user);
                UserTools.saveUser(this, vo);
                this.finish();
            } else {
                Toast.makeText(this, message, Toast.LENGTH_LONG).show();
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }
}

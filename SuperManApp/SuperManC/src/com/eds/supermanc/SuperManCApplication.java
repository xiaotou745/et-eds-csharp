package com.eds.supermanc;

import android.app.Application;
import android.os.StrictMode;
import android.text.TextUtils;
import cn.jpush.android.api.JPushInterface;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.ImageLoader;
import com.android.volley.toolbox.Volley;
import com.baidu.mapapi.SDKInitializer;
import com.eds.supermanc.utils.EtsCLog;
import com.eds.supermanc.utils.LruBitmapCache;

public class SuperManCApplication extends Application {

    // private static final String TAG = SuperManCApplication.class.getSimpleName();
    private static SuperManCApplication mInstance;
    private RequestQueue mRequestQueue;
    private ImageLoader mImageLoader;

    public static synchronized SuperManCApplication getInstance() {
        return mInstance;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        JPushInterface.setDebugMode(Constants.MODE_DEBUG);
        JPushInterface.init(this);
        // 在使用 SDK 各组间之前初始化 context 信息，传入 ApplicationContext
        SDKInitializer.initialize(this.getApplicationContext());
        mInstance = this;
        // 启动检测策略
        // setStrickModeParams();
    }

    public RequestQueue getRequestQueue() {
        if (mRequestQueue == null) {
            mRequestQueue = Volley.newRequestQueue(getApplicationContext());
        }
        return mRequestQueue;
    }

    public ImageLoader getImageLoader() {
        getRequestQueue();
        if (mImageLoader == null) {
            mImageLoader = new ImageLoader(this.mRequestQueue, new LruBitmapCache());
        }
        return this.mImageLoader;
    }

    public <T> void addToRequestQueue(Request<T> req, String tag) {
        req.setTag(TextUtils.isEmpty(tag) ? EtsCLog.S_LOGTAG : tag);
        VolleyLog.d("Adding request to queue: %s", req.getUrl());
        getRequestQueue().add(req);
    }

    public <T> void addToRequestQueue(Request<T> req) {
        req.setTag(EtsCLog.S_LOGTAG);
        getRequestQueue().add(req);
    }

    public void cancelPendingRequests(Object tag) {
        if (mRequestQueue != null) {
            mRequestQueue.cancelAll(tag);
        }
    }

    /**
     * 设置监控策略
     */
    public void setStrickModeParams() {
        StrictMode.setThreadPolicy(new StrictMode.ThreadPolicy.Builder().detectDiskReads().detectDiskWrites()
                .detectNetwork().penaltyLog().penaltyDialog().build());
        StrictMode.setVmPolicy(new StrictMode.VmPolicy.Builder().detectLeakedSqlLiteObjects().penaltyLog()
                .penaltyDeath().build());
    }
}

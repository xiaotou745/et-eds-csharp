package com.eds.supermanb.utils;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.Map;

import android.content.Context;

import com.android.volley.AuthFailureError;
import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request.Method;
import com.android.volley.RequestQueue;
import com.android.volley.Response.ErrorListener;
import com.android.volley.Response.Listener;
import com.android.volley.VolleyError;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.eds.supermanb.constant.Constants;


public class VolleyUtil {

    private Context context;
    RequestQueue queue;

    public VolleyUtil(Context context) {
        this.context = context;
        queue = Volley.newRequestQueue(context);
        VolleyLog.setTag(EtsBLog.S_LOGTAG);
    }

    public void post(final Map<String, String> parms, String url, final HTTPListener listener, final int requestCode) {
        url = Constants.host + url;
        StringRequest request = new StringRequest(Method.POST, url, new Listener<String>() {
            public void onResponse(String response) {
                listener.onResponse(response, requestCode);
            };
        }, new ErrorListener() {
            public void onErrorResponse(VolleyError error) {
                listener.onErrorResponse(error, requestCode);
            };
        }) {
            @Override
            protected Map<String, String> getParams() throws AuthFailureError {
                if (Constants.MODE_DEBUG && parms != null) {
                    StringBuffer sbf = new StringBuffer();
                    sbf.append("parms:<post>[");
                    for (String key : parms.keySet()) {
                        sbf.append(key).append("=").append(parms.get(key).toString());
                    }
                    EtsBLog.d(sbf.toString());
                }
                return parms;
            }
        };
        request.setRetryPolicy(new DefaultRetryPolicy(DefaultRetryPolicy.DEFAULT_TIMEOUT_MS * 2,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
        queue.add(request);
    }

    public void get(final Map<String, String> params, String url, final HTTPListener listener, final int requestCode) {
        url = Constants.host + url;
        StringBuilder sb = new StringBuilder(url);
        if (params != null) {
            sb.append('?');
            // 迭代Map拼接请求参数 .`

            for (Map.Entry<String, String> entry : params.entrySet()) {
                try {
                    sb.append(entry.getKey()).append('=')
                            .append(URLEncoder.encode(entry.getValue(), Constants.CODE_TYPE)).append('&');
                } catch (UnsupportedEncodingException e) {
                    e.printStackTrace();
                }
            }
            sb.deleteCharAt(sb.length() - 1);// 删除最后一个"&"
        }
        url = sb.toString();
        EtsBLog.d("parms:<get>[" + url.substring(url.indexOf("?") + 1) + "]");
        StringRequest request = new StringRequest(Method.GET, url, new Listener<String>() {
            public void onResponse(String response) {
                listener.onResponse(response, requestCode);
            };
        }, new ErrorListener() {
            public void onErrorResponse(VolleyError error) {
                listener.onErrorResponse(error, requestCode);
            };
        });
        queue.add(request);
    }

    public interface HTTPListener {
        void onResponse(String response, int requestCode);

        void onErrorResponse(VolleyError error, int requestCode);
    }

}

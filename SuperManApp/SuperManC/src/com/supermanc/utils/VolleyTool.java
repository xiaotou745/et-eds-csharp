package com.supermanc.utils;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.Map;

import org.json.JSONObject;

import android.content.Context;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.Response.ErrorListener;
import com.android.volley.Response.Listener;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.ImageLoader;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.google.gson.Gson;
import com.supermanc.Constants;
import com.supermanc.SuperManCApplication;

public class VolleyTool {

    public static RequestQueue getRequestQueue() {
        return SuperManCApplication.getInstance().getRequestQueue();
    }

    public static ImageLoader getImageLoader() {
        return SuperManCApplication.getInstance().getImageLoader();
    }

    public static <T> void post(String url, JSONObject jsonBody, final HTTPListener httpListener,
            final int requestCode, Class<T> t) {
        JsonObjectRequest jsonRequest = new JsonObjectRequest(url, jsonBody, new Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                httpListener.onResponse(response.toString(), requestCode);
            }
        }, new ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Context context = SuperManCApplication.getInstance().getApplicationContext();
                Toast.makeText(context, VolleyErrorHelper.getMessage(error, context), Toast.LENGTH_LONG).show();
            }
        });
        jsonRequest.setRetryPolicy(new DefaultRetryPolicy(DefaultRetryPolicy.DEFAULT_TIMEOUT_MS * 2,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
        SuperManCApplication.getInstance().addToRequestQueue(jsonRequest);
    }

    public static <T> void post(String url, final Map<String, String> params, final HTTPListener httpListener,
            int requestCode, Class<T> t) {
        doRequest(Request.Method.POST, url, params, httpListener, requestCode, t);
    }

    public static <T> void get(String url, final Map<String, String> params, final HTTPListener httpListener,
            int requestCode, Class<T> t) {
        StringBuilder sb = new StringBuilder(url);
        if (params != null) {
            sb.append('?');
            for (Map.Entry<String, String> entry : params.entrySet()) {
                try {
                    sb.append(entry.getKey()).append('=')
                            .append(URLEncoder.encode(entry.getValue(), Constants.CODE_TYPE)).append('&');
                } catch (UnsupportedEncodingException e) {
                    e.printStackTrace();
                }
            }
            sb.deleteCharAt(sb.length() - 1);
        }
        EtsCLog.d("param<get>[" + sb.toString() + "]");
        doRequest(Request.Method.GET, sb.toString(), null, httpListener, requestCode, t);
    }

    private static <T> void doRequest(int type, String url, final Map<String, String> params,
            final HTTPListener httpListener, final int requestCode, final Class<T> t) {
        StringRequest postRequest = new StringRequest(type, url, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                EtsCLog.d("result:[" + response + "]");
                if (t == null) {
                    // 返回字段少的时候，直接传入String类型 方便解析
                    httpListener.onResponse(response, requestCode);
                } else {
                    // 返回字段多的时候，直接传入实体类型 方便解析
                    Gson gson = new Gson();
                    T obj = gson.fromJson(response, t);
                    httpListener.onResponse(obj, requestCode);
                }
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Context context = SuperManCApplication.getInstance().getApplicationContext();
                Toast.makeText(context, VolleyErrorHelper.getMessage(error, context), Toast.LENGTH_LONG).show();
                httpListener.onErrorResponse(error, requestCode);
            }
        }) {
            @Override
            protected Map<String, String> getParams() throws AuthFailureError {
                StringBuffer sbf = new StringBuffer();
                sbf.append("parms:<post>[");
                for (String key : params.keySet()) {
                    sbf.append(key).append("=").append(params.get(key).toString());
                }
                EtsCLog.d(sbf.toString() + "]");
                return params;
            }
        };
        postRequest.setRetryPolicy(new DefaultRetryPolicy(DefaultRetryPolicy.DEFAULT_TIMEOUT_MS * 2,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES, DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
        SuperManCApplication.getInstance().addToRequestQueue(postRequest);
    }

    public interface HTTPListener {
        <T> void onResponse(T t, int requestCode);

        void onErrorResponse(VolleyError error, int requestCode);
    }
}

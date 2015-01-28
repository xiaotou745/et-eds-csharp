package com.supermanc.utils;

import com.android.volley.AuthFailureError;
import com.android.volley.NetworkError;
import com.android.volley.NetworkResponse;
import com.android.volley.NoConnectionError;
import com.android.volley.ServerError;
import com.android.volley.TimeoutError;
import com.android.volley.VolleyError;
import android.content.Context;

public class VolleyErrorHelper {
	/**
	 * Returns appropriate message which is to be displayed to the user 
	 * against the specified error object.
	 * 
	 * @param error
	 * @param context
	 * @return
	 */
	public static String getMessage(Object error, Context context) {
		if (error instanceof TimeoutError) {
			//return context.getResources().getString(R.string.volley_timeout);
			return "连接服务器超时";
		}
		/*else if (isServerProblem(error)) {
			return handleServerError(error, context);
		}*/
		else if (isNetworkProblem(error)) {
			//return context.getResources().getString(R.string.volley_no_internet);
			return "服务器当前无法访问,请确认您的网络";
		}
		return "服务器当前无法访问";
		//return context.getResources().getString(R.string.volley_generic_error);
	}

	/**
	 * Determines whether the error is related to network
	 * @param error
	 * @return
	 */
	private static boolean isNetworkProblem(Object error) {
		return (error instanceof NetworkError) || (error instanceof NoConnectionError);
	}
	/**
	 * Determines whether the error is related to server
	 * @param error
	 * @return
	 */
	private static boolean isServerProblem(Object error) {
		return (error instanceof ServerError) || (error instanceof AuthFailureError);
	}
	/**
	 * Handles the server error, tries to determine whether to show a stock message or to 
	 * show a message retrieved from the server.
	 * 
	 * @param err
	 * @param context
	 * @return
	 */
	private static String handleServerError(Object err, Context context) {
		VolleyError error = (VolleyError) err;

		NetworkResponse response = error.networkResponse;

		if (response != null) {
			switch (response.statusCode) {
			case 404:
			case 422:
			case 401:
				try {
					// server might return error like this { "error": "Some error occured" }
					// Use "Gson" to parse the result
					/*HashMap<String, String> result = new Gson().fromJson(new String(response.data),
							new TypeToken<Map<String, String>>() {
					}.getType());

					if (result != null && result.containsKey("error")) {
						return result.get("error");
					}*/

				} catch (Exception e) {
					e.printStackTrace();
				}
				// invalid request
				return error.getMessage();

			default:
				//return context.getResources().getString(R.string.generic_server_down);
			}
		}
		return "服务器错误";
		//return context.getResources().getString(R.string.volley_generic_error);
	}
}

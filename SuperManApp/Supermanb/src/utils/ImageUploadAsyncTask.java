package utils;

import java.io.File;
import java.util.Map;

import utils.HttpRequest.HttpRequestListener;


import android.os.AsyncTask;

public class ImageUploadAsyncTask extends AsyncTask<Integer, Integer, String> {  

	private Map<String, String> params;
	private Map<String, File> files;
	private String url;
	private HttpRequestListener listener;
	
	public ImageUploadAsyncTask(Map<String, String> params,Map<String, File> files,String url,HttpRequestListener listener){
		this.params = params;
		this.files = files;
		this.url = url;
		this.listener =  listener;
	}

	/**  
	 * 这里的Integer参数对应AsyncTask中的第一个参数   
	 * 这里的String返回值对应AsyncTask的第三个参数  
	 * 该方法并不运行在UI线程当中，主要用于异步操作，所有在该方法中不能对UI当中的空间进行设置和修改  
	 * 但是可以调用publishProgress方法触发onProgressUpdate对UI进行操作  
	 */  
	@Override  
	protected String doInBackground(Integer... param) {
		try {
			return HttpRequest.post(url, params, files);
		} catch (Exception e) {
			e.printStackTrace();
			return "fail";
		}  
	}  


	/**  
	 * 这里的String参数对应AsyncTask中的第三个参数（也就是接收doInBackground的返回值）  
	 * 在doInBackground方法执行结束之后在运行，并且运行在UI线程当中 可以对UI空间进行设置  
	 * 无效的用户
	 */  
	@Override  
	protected void onPostExecute(String result) {
		if("fail".equals(result)){
			listener.httpError();
		}else{
			listener.httpSuccess(result);
		}
	}  


	//该方法运行在UI线程当中,并且运行在UI线程当中 可以对UI空间进行设置  
	@Override  
	protected void onPreExecute() {  
	}  
	/**  
	 * 这里的Intege参数对应AsyncTask中的第二个参数  
	 * 在doInBackground方法当中，，每次调用publishProgress方法都会触发onProgressUpdate执行  
	 * onProgressUpdate是在UI线程中执行，所有可以对UI空间进行操作  
	 */  
	@Override  
	protected void onProgressUpdate(Integer... values) {  
	}  

}

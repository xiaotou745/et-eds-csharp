package activity;

import android.app.ActionBar;
import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.Window;
import android.widget.Toast;
import cn.jpush.android.api.JPushInterface;

import com.umeng.analytics.MobclickAgent;





public class BaseActionBarActivity extends Activity  {
	ActionBar actionBar;
	public Context mContext;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getWindow().requestFeature(Window.FEATURE_ACTION_BAR);
		mContext = this;
		actionBar = this.getActionBar();
		actionBar.setDisplayHomeAsUpEnabled(true);
	//	actionBar.setIcon(getResources().getDrawable(R.drawable.back));
		//actionBar.set(getResources().getDrawable(R.drawable.back));
		
	}
@Override
public boolean onOptionsItemSelected(MenuItem item) {
	// TODO Auto-generated method stub
    if(item.getItemId() == android.R.id.home)
    {
        finish();
        return true;
    }
	return super.onOptionsItemSelected(item);
}


	public void sendMsg(String msg){
		Toast.makeText(this, msg, 0).show();
	}
	@Override
	protected void onResume() {
		super.onResume();
		JPushInterface.onResume(this);
		MobclickAgent.onResume(this);
	}
	@Override
	protected void onPause() {
		super.onPause();
		JPushInterface.onPause(this);
		MobclickAgent.onPause(this);
	}
	
	
}
 
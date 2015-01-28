package activity;


import android.app.Activity;
import android.os.Bundle;
import android.widget.Toast;

import cn.jpush.android.api.JPushInterface;

import com.baidu.mapapi.map.BaiduMap;
import com.baidu.mapapi.map.BitmapDescriptorFactory;
import com.baidu.mapapi.map.MapStatusUpdateFactory;
import com.baidu.mapapi.map.MapView;
import com.baidu.mapapi.map.MarkerOptions;
import com.baidu.mapapi.search.core.SearchResult;
import com.baidu.mapapi.search.geocode.GeoCodeOption;
import com.baidu.mapapi.search.geocode.GeoCodeResult;
import com.baidu.mapapi.search.geocode.GeoCoder;
import com.baidu.mapapi.search.geocode.OnGetGeoCoderResultListener;
import com.baidu.mapapi.search.geocode.ReverseGeoCodeResult;
import com.supermanb.supermanb.R;
import com.umeng.analytics.MobclickAgent;

public class GeoCoderActivity extends Activity implements OnGetGeoCoderResultListener{
	private BaiduMap mBaiduMap = null;
	private MapView mMapView = null;
	private GeoCoder mSearch = null;
	private String adress;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_geocoder);
		mMapView = (MapView) findViewById(R.id.bmapView);
		mBaiduMap = mMapView.getMap();
		
		mSearch = GeoCoder.newInstance();
		mSearch.setOnGetGeoCodeResultListener(this);
		mSearch.geocode(new GeoCodeOption().city(getIntent().getStringExtra("City")).address(getIntent().getStringExtra("Adress")));
		
	}
	@Override
	public void onGetGeoCodeResult(GeoCodeResult arg0) {
		if (arg0 == null || arg0.error != SearchResult.ERRORNO.NO_ERROR) {
			Toast.makeText(this, "抱歉，未能找到结果", Toast.LENGTH_LONG)
					.show();
			return;
		}
		mBaiduMap.clear();
		
		mBaiduMap.addOverlay(new MarkerOptions().position(arg0.getLocation())
				.icon(BitmapDescriptorFactory
						.fromResource(R.drawable.icon_gcoding)));
		
		mBaiduMap.setMapStatus(MapStatusUpdateFactory.newLatLng(arg0
				.getLocation()));
		String strInfo = String.format("纬度：%f 经度：%f",
				arg0.getLocation().latitude, arg0.getLocation().longitude);
		Toast.makeText(this, strInfo, Toast.LENGTH_LONG).show();
		
	}
	@Override
	public void onGetReverseGeoCodeResult(ReverseGeoCodeResult arg0) {
		// TODO Auto-generated method stub
		
	}
	@Override
	protected void onPause() {
		super.onPause();
		JPushInterface.onPause(this);
		MobclickAgent.onPause(this);
	}
	@Override
	protected void onResume() {
		super.onResume();
		JPushInterface.onResume(this);
		MobclickAgent.onResume(this);
	}
}

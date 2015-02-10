package picker;

import java.util.ArrayList;
import java.util.List;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

public class CityWorker {

    private CityDataHelper cityData;
    private SQLiteDatabase db;

    public CityWorker(Context context) {
        cityData = new CityDataHelper(context);
        cityData.openDatabase();
        db = cityData.getDatabase();
    }

    public List<CityMode> getProvince() {
        List<CityMode> list = new ArrayList<CityMode>();
        list.add(new CityMode());
        Cursor cursor = null;
        try {
            String sql = "select * from province";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            while (!cursor.isLast()) {
                String code = cursor.getString(cursor.getColumnIndex("code"));
                byte bytes[] = cursor.getBlob(2);
                String name = new String(bytes, "UTF-8");
                CityMode proc = new CityMode();
                proc.setName(name.trim());
                proc.setPcode(code.trim());
                list.add(proc);
                cursor.moveToNext();
            }
            String code = cursor.getString(cursor.getColumnIndex("code"));
            byte bytes[] = cursor.getBlob(2);
            String name = new String(bytes, "UTF-8");
            CityMode proc = new CityMode();
            proc.setName(name.trim());
            proc.setPcode(code.trim());
            list.add(proc);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        list.add(new CityMode());
        return list;
    }

    public ArrayList<CityMode> getCity() {
        ArrayList<CityMode> list = new ArrayList<CityMode>();
        Cursor cursor = null;
        try {
            String sql = "select * from city";
            cursor = db.rawQuery(sql, null);
            if (cursor != null) {
                cursor.moveToFirst();
                do {
                    String code = cursor.getString(cursor.getColumnIndex("code"));
                    byte bytes[] = cursor.getBlob(2);
                    String name = new String(bytes, "UTF-8");
                    CityMode proc = new CityMode();
                    proc.setName(name.trim());
                    proc.setPcode(code.trim());
                    list.add(proc);
                } while (cursor.moveToNext());
            }
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        return list;
    }

    public List<CityMode> getCityByProvince(String pcode) {
        List<CityMode> list = new ArrayList<CityMode>();
        list.add(new CityMode());
        Cursor cursor = null;
        try {
            String sql = "select * from city where pcode='" + pcode + "'";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            while (!cursor.isLast()) {
                String code = cursor.getString(cursor.getColumnIndex("code"));
                byte bytes[] = cursor.getBlob(2);
                String name = new String(bytes, "UTF-8");
                CityMode city = new CityMode();
                city.setName(name.trim());
                city.setPcode(code.trim());
                list.add(city);
                cursor.moveToNext();
            }
            String code = cursor.getString(cursor.getColumnIndex("code"));
            byte bytes[] = cursor.getBlob(2);
            String name = new String(bytes, "UTF-8");
            CityMode city = new CityMode();
            city.setName(name.trim());
            city.setPcode(code.trim());
            list.add(city);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        list.add(new CityMode());
        return list;
    }

    public ArrayList<CityMode> getAreaByCity(String city) {
        ArrayList<CityMode> list = new ArrayList<CityMode>();
        // list.add(new CityMode());
        Cursor cursor = null;
        try {
            String sql = "select * from district where pcode='" + city + "'";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            while (!cursor.isLast()) {
                String code = cursor.getString(cursor.getColumnIndex("code"));
                // byte bytes[] = cursor.getBlob(2);
                String name = cursor.getString(cursor.getColumnIndex("name"));
                CityMode area = new CityMode();
                area.setName(name.trim());
                area.setPcode(code.trim());
                list.add(area);
                cursor.moveToNext();
            }
            String code = cursor.getString(cursor.getColumnIndex("code"));
            // byte bytes[] = cursor.getBlob(2);
            String name = cursor.getString(cursor.getColumnIndex("name"));
            CityMode area = new CityMode();
            area.setName(name.trim());
            area.setPcode(code.trim());
            list.add(area);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        list.add(new CityMode());
        return list;
    }

    /**
     * 鏍规嵁鍖哄煙code 鑾峰彇褰撳墠鐨勫尯鍩�
     * 
     * @param pcode
     * @return
     */
    public String[] getArea(String code) {
        List<CityMode> list = new ArrayList<CityMode>();
        String pcode = null;
        String name = null;
        Cursor cursor = null;
        try {
            String sql = "select * from district where code='" + code + "'";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            pcode = cursor.getString(3);
            byte bytes[] = cursor.getBlob(2);
            name = new String(bytes, "UTF-8");

        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        return new String[] { name.trim(), pcode.trim() };
    }

    /**
     * 鏍规嵁褰撳墠鍩庡競pcode 鑾峰彇褰撳墠鐨勫煄甯�
     * 
     * @param pcode
     * @return
     */
    public String[] getCity(String code) {
        List<CityMode> list = new ArrayList<CityMode>();
        String pcode = null;
        String name = null;
        Cursor cursor = null;
        try {
            String sql = "select * from city where code='" + code + "'";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            pcode = cursor.getString(3);
            byte bytes[] = cursor.getBlob(2);
            name = new String(bytes, "UTF-8");

        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        return new String[] { name.trim(), pcode.trim() };
    }

    /**
     * 鏍规嵁褰撳墠鐪佸競pcode 鑾峰彇褰撳墠鐨勭渷甯�
     * 
     * @param pcode
     * @return
     */
    public String getProvince(String code) {
        List<CityMode> list = new ArrayList<CityMode>();
        String name = null;
        Cursor cursor = null;
        try {
            String sql = "select * from province where code='" + code + "'";
            cursor = db.rawQuery(sql, null);
            cursor.moveToFirst();
            byte bytes[] = cursor.getBlob(2);
            name = new String(bytes, "UTF-8");

        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            cursor.close();
        }
        return name.trim();
    }

    public void closeCityWorker() {
        cityData.closeDatabase();
        db.close();
    }

}

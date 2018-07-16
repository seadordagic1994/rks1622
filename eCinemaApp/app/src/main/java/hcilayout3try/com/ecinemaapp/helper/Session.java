package hcilayout3try.com.ecinemaapp.helper;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import java.util.ArrayList;

import hcilayout3try.com.ecinemaapp.MyApp;
import hcilayout3try.com.ecinemaapp.models.MovieProjections;
import hcilayout3try.com.ecinemaapp.models.User;

/**
 * Created by SeadOrdagić on 28.05.2018..
 */

 public class Session {

    private static final String PREFS_NAME = "DatotekaZaSharedPrefernces6";

    private static SharedPreferences prefs;

    public  Session(Context cntx) {
        // TODO Auto-generated constructor stub
        prefs = PreferenceManager.getDefaultSharedPreferences(cntx);
    }

    public static void setUser(User u) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putString("user", MyGson.build().toJson(u));
        ed.commit();

    }

    public static User getUser() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        String  str = sp.getString("user", "");
        if(str.length() == 0 || str == null)
            return null;

        return MyGson.build().fromJson(str, User.class);
    }

    public static void setMovieId(int id) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putInt("movieId", id);
        ed.commit();

    }

    public static int getMovieId() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        return sp.getInt("movieId", 0);
    }

    public static void setLocation(LocationParameters p) {
        String str = MyGson.build().toJson(p);
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putString("locationParamters", str);
        ed.commit();

    }

    public static LocationParameters getLocation() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);

        String  str = sp.getString("locationParamters", "");
        if(str.length() == 0)
            return null;

        return MyGson.build().fromJson(str, LocationParameters.class);
    }


    public static void setVPAmount(int id) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putInt("vp", id);
        ed.commit();

    }

    public static int getVpAmount() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        return sp.getInt("vp", 0);
    }


    public static void setCinemaId(int id) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putInt("CinemaId", id);
        ed.commit();

    }

    public static int getCinemaId() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        return sp.getInt("CinemaId", 0);
    }

    public static void setSearchText(String text) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putString("search", text);
        ed.commit();

    }

    public static String getSearchText() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);

        String  str = sp.getString("search", "");
        if(str.length() == 0)
            return null;

        return sp.getString("search", str);
    }

    public static void setMaxSeats(int id) {
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putInt("seatsMax", id);
        ed.commit();

    }

    public static int getMaxSeats() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        return sp.getInt("seatsMax", 0);
    }

    public static void setProjection(MovieProjections.Projection projection) {

        String str = MyGson.build().toJson(projection);
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putString("projection", str);
        ed.commit();

    }

    public static MovieProjections.Projection getProjection() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);

        String  str = sp.getString("projection", "");
        if(str.length() == 0)
            return null;

        return MyGson.build().fromJson(str, MovieProjections.Projection.class);
    }

    public static void setTimesForProjection(MovieProjections projection) {

        String str = MyGson.build().toJson(projection);
        SharedPreferences sp =  MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);
        SharedPreferences.Editor ed=sp.edit();
        ed.putString("projectionTimes", str);
        ed.commit();

    }

    public static MovieProjections getTimesForProjection() {
        SharedPreferences sp=MyApp.getContext().getSharedPreferences("key", Context.MODE_PRIVATE);

        String  str = sp.getString("projectionTimes", "");
        if(str.length() == 0)
            return null;

        return MyGson.build().fromJson(str, MovieProjections.class);
    }
}

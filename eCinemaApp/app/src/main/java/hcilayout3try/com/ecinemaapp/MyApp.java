package hcilayout3try.com.ecinemaapp;

/**
 * Created by SeadOrdagić on 28.05.2018..
 */

import android.app.Application;
import android.content.Context;

public class MyApp extends Application
{

    private static Context context;

    public static Context getContext()
    {
        return context;
    }

    @Override
    public void onCreate()
    {
        super.onCreate();
        context = getApplicationContext();
    }
}

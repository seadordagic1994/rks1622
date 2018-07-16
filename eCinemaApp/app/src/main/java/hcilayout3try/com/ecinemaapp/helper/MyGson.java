package hcilayout3try.com.ecinemaapp.helper;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

/**
 * Created by SeadOrdagić on 28.05.2018..
 */

public class MyGson {

    public static Gson build()
    {
        GsonBuilder builder = new GsonBuilder();
        builder.setDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        return builder.create();
    }

}

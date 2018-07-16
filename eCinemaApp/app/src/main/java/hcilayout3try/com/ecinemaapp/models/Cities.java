package hcilayout3try.com.ecinemaapp.models;


import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.ArrayList;

/**
 * Created by SeadOrdagić on 21.05.2018..
 */

public class Cities {

    @SerializedName("managers")
    private ArrayList<City> cities;


    public ArrayList<City> cities() {
        return cities;
    }

    public void setManagers(ArrayList<City> cities) {
        this.cities = cities;
    }
}

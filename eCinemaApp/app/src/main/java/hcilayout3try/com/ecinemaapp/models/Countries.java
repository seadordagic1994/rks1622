package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 30.05.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class Countries {

    @SerializedName("CountryID")
    @Expose
    private Integer countryID;
    @SerializedName("Name")
    @Expose
    private String name;

    public Integer getCountryID() {
        return countryID;
    }

    public void setCountryID(Integer countryID) {
        this.countryID = countryID;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }


}

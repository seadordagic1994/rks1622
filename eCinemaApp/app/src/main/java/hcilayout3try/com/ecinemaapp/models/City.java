package hcilayout3try.com.ecinemaapp.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

/**
 * Created by SeadOrdagić on 21.05.2018..
 */

public class City {
    @SerializedName("CityID")
    @Expose
    private Integer cityID;
    @SerializedName("Name")
    @Expose
    private String name;
    @SerializedName("PostalCode")
    @Expose
    private String postalCode;

    public Integer getCityID() {
        return cityID;
    }

    public void setCityID(Integer cityID) {
        this.cityID = cityID;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPostalCode() {
        return postalCode;
    }

    public void setPostalCode(String postalCode) {
        this.postalCode = postalCode;
    }
}

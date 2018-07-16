package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 30.05.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Cinemas {
    @SerializedName("CinemaID")
    @Expose
    private Integer cinemaID;
    @SerializedName("Name")
    @Expose
    private String name;
    @SerializedName("Address")
    @Expose
    private String address;
    @SerializedName("CityID")
    @Expose
    private Integer cityID;

    public Integer getCinemaID() {
        return cinemaID;
    }

    public void setCinemaID(Integer cinemaID) {
        this.cinemaID = cinemaID;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public Integer getCityID() {
        return cityID;
    }

    public void setCityID(Integer cityID) {
        this.cityID = cityID;
    }

}

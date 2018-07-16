package hcilayout3try.com.ecinemaapp.helper;

import java.io.Serializable;

/**
 * Created by SeadOrdagić on 15.07.2018..
 */

public class LocationParameters implements Serializable {

    public  double Latitude;
    public  double Longitude;
    public boolean GPSAquired;

    public LocationParameters(double Latitude,double Longitude,boolean GPSAquired)
    {
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.GPSAquired = GPSAquired;
    }
}

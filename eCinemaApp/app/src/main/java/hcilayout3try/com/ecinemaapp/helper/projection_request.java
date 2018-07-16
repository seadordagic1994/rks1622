package hcilayout3try.com.ecinemaapp.helper;

/**
 * Created by SeadOrdagić on 27.05.2018..
 */

public class projection_request {
    private  int RVisitorID;
    private double Latitude;
    private  double Longitude;
    private boolean GPSAquired;
    private String SearchQuery;

    public projection_request(int RVisitorID, double Latitude,double Longitude,boolean GPSAquired,String SearchQuery)
    {
        this.RVisitorID = RVisitorID;
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.GPSAquired = GPSAquired;
        this.SearchQuery = SearchQuery;
    }
}

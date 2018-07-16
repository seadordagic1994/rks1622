package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 16.07.2018..
 */

public class movieRatingParameters {
    public String RVisitorID;
    public String MovieID;
    public String Rating;

    public movieRatingParameters(String RVisitorID, String MovieID, String Rating)
    {
        this.RVisitorID = RVisitorID;
        this.MovieID = MovieID;
        this.Rating = Rating;
    }
}

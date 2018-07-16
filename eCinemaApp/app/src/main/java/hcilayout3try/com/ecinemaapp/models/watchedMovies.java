package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 31.05.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class watchedMovies {

    @SerializedName("MovieID")
    @Expose
    private Integer movieID;
    @SerializedName("MovieName")
    @Expose
    private String movieName;
    @SerializedName("MovieCover")
    @Expose
    private String movieCover;
    @SerializedName("MovieRating")
    @Expose
    private Integer movieRating;

    public Integer getMovieID() {
        return movieID;
    }

    public void setMovieID(Integer movieID) {
        this.movieID = movieID;
    }

    public String getMovieName() {
        return movieName;
    }

    public void setMovieName(String movieName) {
        this.movieName = movieName;
    }

    public String getMovieCover() {
        return movieCover;
    }

    public void setMovieCover(String movieCover) {
        this.movieCover = movieCover;
    }

    public Integer getMovieRating() {
        return movieRating;
    }

    public void setMovieRating(Integer movieRating) {
        this.movieRating = movieRating;
    }

}
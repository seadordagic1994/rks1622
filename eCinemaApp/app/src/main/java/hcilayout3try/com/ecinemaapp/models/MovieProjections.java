package hcilayout3try.com.ecinemaapp.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;
import java.util.List;

/**
 * Created by SeadOrdagić on 27.05.2018..
 */

public class MovieProjections implements Serializable {
    @SerializedName("Duration")
    @Expose
    private String Duration;
    @SerializedName("Age")
    @Expose
    private String Age;
    @SerializedName("Release")
    @Expose
    private String Release;
    @SerializedName("MovieName")
    @Expose
    private String movieName;
    @SerializedName("MovieID")
    @Expose
    private Integer movieID;
    @SerializedName("CinemaHallID")
    @Expose
    private Integer cinemaHallID;
    @SerializedName("DateTimeStart")
    @Expose
    private String dateTimeStart;
    @SerializedName("TicketPrice")
    @Expose
    private Double ticketPrice;
    @SerializedName("MovieCover")
    @Expose
    private String movieCover;
    @SerializedName("Projections")
    @Expose
    private List<Projection> projections = null;

    public String getDuration() {
        return Duration;
    }

    public void setDuration(String Duration) {
        this.Duration = Duration;
    }

    public String getAge() {
        return Age;
    }

    public void setAge(String Age) {
        this.Age = Age;
    }

    public String getRelease() {
        return Release;
    }

    public void setRelease(String Release) {
        this.Release = Release;
    }


    public String getMovieName() {
        return movieName;
    }

    public void setMovieName(String movieName) {
        this.movieName = movieName;
    }

    public Integer getMovieID() {
        return movieID;
    }

    public void setMovieID(Integer movieID) {
        this.movieID = movieID;
    }

    public Integer getCinemaHallID() {
        return cinemaHallID;
    }

    public void setCinemaHallID(Integer cinemaHallID) {
        this.cinemaHallID = cinemaHallID;
    }

    public String getDateTimeStart() {
        return dateTimeStart;
    }

    public void setDateTimeStart(String dateTimeStart) {
        this.dateTimeStart = dateTimeStart;
    }

    public Double getTicketPrice() {
        return ticketPrice;
    }

    public void setTicketPrice(Double ticketPrice) {
        this.ticketPrice = ticketPrice;
    }

    public String getMovieCover() {
        return movieCover;
    }

    public void setMovieCover(String movieCover) {
        this.movieCover = movieCover;
    }

    public List<Projection> getProjections() {
        return projections;
    }

    public void setProjections(List<Projection> projections) {
        this.projections = projections;
    }

    public class Projection  implements  Serializable{

        @SerializedName("ProjectionID")
        @Expose
        private Integer projectionID;
        @SerializedName("MovieID")
        @Expose
        private Integer movieID;
        @SerializedName("CinemaHallID")
        @Expose
        private Integer cinemaHallID;
        @SerializedName("DateTimeStart")
        @Expose
        private String dateTimeStart;
        @SerializedName("Name")
        @Expose
        private String name;
        @SerializedName("TicketPrice")
        @Expose
        private Double ticketPrice;
        @SerializedName("CinemaHallName")
        @Expose
        private String cinemaHallName;
        @SerializedName("CinemaName")
        @Expose
        private String cinemaName;

        public Integer getProjectionID() {
            return projectionID;
        }

        public void setProjectionID(Integer projectionID) {
            this.projectionID = projectionID;
        }

        public Integer getMovieID() {
            return movieID;
        }

        public void setMovieID(Integer movieID) {
            this.movieID = movieID;
        }

        public Integer getCinemaHallID() {
            return cinemaHallID;
        }

        public void setCinemaHallID(Integer cinemaHallID) {
            this.cinemaHallID = cinemaHallID;
        }

        public String getDateTimeStart() {
            return dateTimeStart;
        }

        public void setDateTimeStart(String dateTimeStart) {
            this.dateTimeStart = dateTimeStart;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Double getTicketPrice() {
            return ticketPrice;
        }

        public void setTicketPrice(Double ticketPrice) {
            this.ticketPrice = ticketPrice;
        }

        public String getCinemaHallName() {
            return cinemaHallName;
        }

        public void setCinemaHallName(String cinemaHallName) {
            this.cinemaHallName = cinemaHallName;
        }

        public String getCinemaName() {
            return cinemaName;
        }

        public void setCinemaName(String cinemaName) {
            this.cinemaName = cinemaName;
        }

    }
}

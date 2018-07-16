package hcilayout3try.com.ecinemaapp.api;

/**
 * Created by SeadOrdagić on 21.05.2018..
 */

import android.telecom.Call;

import java.util.ArrayList;
import java.util.List;

import hcilayout3try.com.ecinemaapp.helper.projection_request;
import hcilayout3try.com.ecinemaapp.helper.reservationDetails;
import hcilayout3try.com.ecinemaapp.models.BuyTicketParameters;
import hcilayout3try.com.ecinemaapp.models.Cinemas;
import hcilayout3try.com.ecinemaapp.models.City;
import hcilayout3try.com.ecinemaapp.models.Countries;
import hcilayout3try.com.ecinemaapp.models.MovieProjections;
import hcilayout3try.com.ecinemaapp.models.RVisitors;
import hcilayout3try.com.ecinemaapp.models.Seats;
import hcilayout3try.com.ecinemaapp.models.User;
import hcilayout3try.com.ecinemaapp.models.UserParameters;
import hcilayout3try.com.ecinemaapp.models.movieRatingParameters;
import hcilayout3try.com.ecinemaapp.models.userReservations;
import hcilayout3try.com.ecinemaapp.models.vp_packs;
import hcilayout3try.com.ecinemaapp.models.vp_params;
import hcilayout3try.com.ecinemaapp.models.watchedMovies;
import retrofit.Callback;
import retrofit.http.Body;
import retrofit.http.GET;
import retrofit.http.POST;
import retrofit.http.Path;

public interface Api {

    @GET("/api/Info/GetCitiesByCountry/{id}")
    void getCities(@Path("id") int id, Callback<ArrayList<City>> uscb);

    @GET("/api/VirtualPoints/GetVirtualPointsPacks")
    void getVpPacks(Callback<ArrayList<vp_packs>> uscb);

    @GET("/api/Info/GetCinemasByCity/{id}")
    void getCinemas(@Path("id") int id, Callback<ArrayList<Cinemas>> uscb);

    @GET("/api/RecommendationSystem/GetWatchedMovies/{id}")
    void getWatchedMovies(@Path("id") int id, Callback<ArrayList<watchedMovies>> uscb);

    @GET("/api/Info/GetCountries")
    void getCountries(Callback<ArrayList<Countries>> uscb);

    @GET("/api/UserProjections/GetProjectionSeats/{value}")
    void getSeats(@Path("value") int value, Callback<ArrayList<Seats>> uscb);

    @GET("/api/UserReservations/GetUserReservations/{UserId}")
    void getReservations(@Path("UserId") int UserId, Callback<ArrayList<userReservations>> uscb);

    @POST("/api/VirtualPoints/PostBuyVirtualPointPack")
    void setVP(@Body vp_params body, Callback<String> uscb);

    @POST("/api/Users/PostUsersForAndroid")
    void registerUser(@Body RVisitors body, Callback<Integer> uscb);

    @POST("/api/UserReservations/PostConfirmReservation")
    void setReservationConfirmation(@Body BuyTicketParameters body, Callback<String> uscb);

    @POST("/api/UserReservations/PostCancelReservation")
    void setReservationCancel(@Body BuyTicketParameters body, Callback<String> uscb);

    @POST("/api/Users/PostUsersLogin")
    void getUser(@Body UserParameters body, Callback<User> uscb);

    @GET("/api/Users/GetUsers/{RVisitorID}")
    void getUserById(@Path("UserId") int UserId, Callback<User> uscb);

    @GET("/api/UserProjections/GetProjectionsForCinema/{UserId}")
    void allProjections(@Path("UserId") int UserId, Callback<List<MovieProjections>> projections);

    @POST("/api/UserProjections/SearchForProjections")
    void projections(@Body projection_request body, Callback<List<MovieProjections>> projections);

    @POST("/api/RecommendationSystem/PostMovieRating")
    void RateMovie(@Body movieRatingParameters body, Callback<String> message);

    @POST("/api/UserProjections/SearchForProjections")
    ArrayList<MovieProjections> projections(@Body projection_request body);
    @POST("/api/UserReservations/PostMakeReservation")
    void makeReservation(@Body reservationDetails body,Callback<String> info);

}

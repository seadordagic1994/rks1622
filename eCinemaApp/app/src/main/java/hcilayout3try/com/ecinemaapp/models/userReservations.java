package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 30.05.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.Date;

public class userReservations {

    @SerializedName("ReservationID")
    @Expose
    private Integer reservationID;
    @SerializedName("MovieName")
    @Expose
    private String movieName;
    @SerializedName("OrderDate")
    @Expose
    private String orderDate;
    @SerializedName("ExpireDate")
    @Expose
    private String expireDate;
    @SerializedName("Discount")
    @Expose
    private Integer discount;
    @SerializedName("Total")
    @Expose
    private Double total;

    public Integer getReservationID() {
        return reservationID;
    }

    public void setReservationID(Integer reservationID) {
        this.reservationID = reservationID;
    }

    public String getMovieName() {
        return movieName;
    }

    public void setMovieName(String movieName) {
        this.movieName = movieName;
    }

    public String getOrderDate() {
        return orderDate;
    }

    public void setOrderDate(String orderDate) {
        this.orderDate = orderDate;
    }

    public String getExpireDate() {
        return expireDate;
    }

    public void setExpireDate(String expireDate) {
        this.expireDate = expireDate;
    }

    public Integer getDiscount() {
        return discount;
    }

    public void setDiscount(Integer discount) {
        this.discount = discount;
    }

    public Double getTotal() {
        return total;
    }

    public void setTotal(Double total) {
        this.total = total;
    }

}

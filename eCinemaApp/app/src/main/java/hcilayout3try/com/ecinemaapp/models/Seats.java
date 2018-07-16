package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 28.05.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Seats {

    @SerializedName("SeatID")
    @Expose
    private Integer seatID;
    @SerializedName("SeatRowID")
    @Expose
    private Integer seatRowID;
    @SerializedName("SeatColumnID")
    @Expose
    private Integer seatColumnID;
    @SerializedName("IsReserved")
    @Expose
    private Boolean isReserved;
    @SerializedName("SeatRowLabel")
    @Expose
    private String seatRowLabel;
    @SerializedName("SeatColumnLabel")
    @Expose
    private Integer seatColumnLabel;

    public Integer getSeatID() {
        return seatID;
    }

    public void setSeatID(Integer seatID) {
        this.seatID = seatID;
    }

    public Integer getSeatRowID() {
        return seatRowID;
    }

    public void setSeatRowID(Integer seatRowID) {
        this.seatRowID = seatRowID;
    }

    public Integer getSeatColumnID() {
        return seatColumnID;
    }

    public void setSeatColumnID(Integer seatColumnID) {
        this.seatColumnID = seatColumnID;
    }

    public Boolean getIsReserved() {
        return isReserved;
    }

    public void setIsReserved(Boolean isReserved) {
        this.isReserved = isReserved;
    }

    public String getSeatRowLabel() {
        return seatRowLabel;
    }

    public void setSeatRowLabel(String seatRowLabel) {
        this.seatRowLabel = seatRowLabel;
    }

    public Integer getSeatColumnLabel() {
        return seatColumnLabel;
    }

    public void setSeatColumnLabel(Integer seatColumnLabel) {
        this.seatColumnLabel = seatColumnLabel;
    }

}
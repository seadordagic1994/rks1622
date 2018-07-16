package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 01.06.2018..
 */

public class BuyTicketParameters {
    private String RVisitorID;
    private String ReservationID;

    public BuyTicketParameters(String visitor, String Reservation)
    {
        RVisitorID = visitor;
        ReservationID = Reservation;
    }

}

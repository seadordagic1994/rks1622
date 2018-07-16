package hcilayout3try.com.ecinemaapp.helper;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by SeadOrdagić on 29.05.2018..
 */

public class reservationDetails {

    private  int RVisitorID;
    private int ProjectionID;
    private  List<Integer> SelectedSeats = new ArrayList<Integer>();

    public reservationDetails(int RVisitorID, int ProjectionID)
    {
        this.RVisitorID = RVisitorID;
        this.ProjectionID = ProjectionID;
        this.SelectedSeats = reservedSeats.getSeats();
    }
}

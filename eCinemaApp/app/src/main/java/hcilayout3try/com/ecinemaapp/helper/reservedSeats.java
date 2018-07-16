package hcilayout3try.com.ecinemaapp.helper;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by SeadOrdagić on 29.05.2018..
 */

public class reservedSeats {

   public static List<Integer> seats = new ArrayList<Integer>();

    public static void AddSeat(Integer seatId)
    {
        seats.add(seatId);
    }

    public static List<Integer> getSeats()
    {
        return seats;
    }

    public static void RemoveSeat(Integer seatId)
    {
        seats.remove(seatId);
    }
    public static void RemoveAll()
    {
        seats.removeAll(seats);
    }
}

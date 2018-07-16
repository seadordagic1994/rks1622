package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 01.06.2018..
 */

public class vp_params {
    private String RVisitorID;
    private String VirtualPointsPacketID;

    public vp_params(String visitor, String pack)
    {
        RVisitorID = visitor;
        VirtualPointsPacketID = pack;
    }
}

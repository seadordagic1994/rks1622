package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 01.06.2018..
 */

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class vp_packs {

    @SerializedName("VirtualPointsPacketID")
    @Expose
    private Integer virtualPointsPacketID;
    @SerializedName("Amount")
    @Expose
    private Integer amount;

    public Integer getVirtualPointsPacketID() {
        return virtualPointsPacketID;
    }

    public void setVirtualPointsPacketID(Integer virtualPointsPacketID) {
        this.virtualPointsPacketID = virtualPointsPacketID;
    }

    public Integer getAmount() {
        return amount;
    }

    public void setAmount(Integer amount) {
        this.amount = amount;
    }
}

package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 01.06.2018..
 */
        import com.google.gson.annotations.Expose;
        import com.google.gson.annotations.SerializedName;

public class User {

    @SerializedName("UserID")
    @Expose
    private Integer userID;
    @SerializedName("FirstName")
    @Expose
    private String FirstName;
    @SerializedName("LastName")
    @Expose
    private String LastName;
    @SerializedName("Email")
    @Expose
    private String Email;
    @SerializedName("VirtualPoints")
    @Expose
    private Integer VirtualPoints;
    @SerializedName("PhoneNumber")
    @Expose
    private String PhoneNumber;

    public Integer getUserID() {
        return userID;
    }

    public void setUserID(Integer userID) {
        this.userID = userID;
    }

    public String getFirstName() {
        return FirstName;
    }

    public void setFirstName(String firstName) {
        this.FirstName = firstName;
    }

    public String getLastName() {
        return LastName;
    }

    public void setLastName(String lastName) {
        this.LastName = lastName;
    }

    public String getEmail() {
        return Email;
    }

    public void setEmail(String email) {
        this.Email = email;
    }

    public Integer getVirtualPoints() {
        return VirtualPoints;
    }

    public void setVirtualPoints(Integer virtualPoints) {
        this.VirtualPoints = virtualPoints;
    }

    public String getPhoneNumber() {
        return PhoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) {
        this.PhoneNumber = phoneNumber;
    }

}
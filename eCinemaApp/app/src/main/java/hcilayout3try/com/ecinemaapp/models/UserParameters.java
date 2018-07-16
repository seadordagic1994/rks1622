package hcilayout3try.com.ecinemaapp.models;

/**
 * Created by SeadOrdagić on 01.06.2018..
 */

public class UserParameters {
    private String Username;
    private String PasswordHash;

    public UserParameters(String username, String password)
    {
        Username = username;
        PasswordHash = password;
    }
}

package hcilayout3try.com.ecinemaapp;

import android.*;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.LocationParameters;
import hcilayout3try.com.ecinemaapp.helper.MyGson;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.location;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.Cities;
import hcilayout3try.com.ecinemaapp.models.City;
import hcilayout3try.com.ecinemaapp.models.User;
import hcilayout3try.com.ecinemaapp.models.UserParameters;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;


public class MainActivity extends AppCompatActivity {

    private EditText username;
    private EditText password;
    public  url httpConn = new url();
    private TextView usernametxt;

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 99;
    public static final String[] LOCATION_PERMISSIONS = new String[] {  android.Manifest.permission.ACCESS_FINE_LOCATION, android.Manifest.permission.ACCESS_COARSE_LOCATION };


    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        if(Session.getUser() != null)
        {
            Intent myIntent = new Intent(MainActivity.this, Main2Activity.class);
            //myIntent.putExtra("longitude", value); //Optional parameters
            MainActivity.this.startActivity(myIntent);
        }


        username = (EditText) findViewById(R.id.username);
        password = (EditText) findViewById(R.id.password);
        usernametxt = (TextView) findViewById(R.id.usernametxt);

        Button login = (Button) findViewById(R.id.submit);
        Button register = (Button) findViewById(R.id.register);

        register.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent myIntent = new Intent(MainActivity.this, RegisterActivity.class);
                //myIntent.putExtra("longitude", value); //Optional parameters
                MainActivity.this.startActivity(myIntent);
            }
        });

            login.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    do_btnLogin_click();
                }
            });

    }

    private void do_btnLogin_click() {
        RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();

        Api restInt=radapter.create(Api.class);

        Loading.Load(MainActivity.this, "","Processing account ...");

        restInt.getUser(new UserParameters(((TextView) findViewById(R.id.username)).getText()+"", ((TextView) findViewById(R.id.password)).getText()+""), new Callback<User>() {
            @Override
            public void success(User user, Response response) {
                Loading.Dissmis();
                Session.setUser(user);

                Session.setVPAmount(user.getVirtualPoints());
                Intent myIntent = new Intent(MainActivity.this, Main2Activity.class);
                //myIntent.putExtra("longitude", value); //Optional parameters
                MainActivity.this.startActivity(myIntent);
            }

            @Override
            public void failure(RetrofitError error) {
                Loading.Dissmis();
                Toast.makeText(MainActivity.this, "Invalid username or password !", Toast.LENGTH_SHORT).show();
                Toast.makeText(MainActivity.this, error.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }


}

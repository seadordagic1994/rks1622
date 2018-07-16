package hcilayout3try.com.ecinemaapp.helper;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.widget.Toast;

/**
 * Created by SeadOrdagić on 15.07.2018..
 */

public  class location  extends AppCompatActivity{

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 99;
    public static final String[] LOCATION_PERMISSIONS = new String[] {  Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION };


}

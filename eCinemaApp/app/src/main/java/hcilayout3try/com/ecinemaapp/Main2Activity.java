package hcilayout3try.com.ecinemaapp;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.LocationParameters;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.Cinemas;
import hcilayout3try.com.ecinemaapp.models.Cities;
import hcilayout3try.com.ecinemaapp.models.City;
import hcilayout3try.com.ecinemaapp.models.Countries;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;

public class Main2Activity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener,
        BlankFragment.OnFragmentInteractionListener,
        Reservations_fragment.OnFragmentInteractionListener,
        AllProjectionsFragment.OnFragmentInteractionListener,
        watchedMoviesRateFragment.OnFragmentInteractionListener,
        buyVp.OnFragmentInteractionListener,
        userProfileFragment.OnFragmentInteractionListener,
        projectionTimesFragment.OnFragmentInteractionListener{

    public url httpConn = new url();
    public RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();
    public Api restInt=radapter.create(Api.class);

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 99;
    public static final String[] LOCATION_PERMISSIONS = new String[] {  android.Manifest.permission.ACCESS_FINE_LOCATION, android.Manifest.permission.ACCESS_COARSE_LOCATION };


    @Override
    public void onFragmentInteraction(Uri uri){
        //We can keep this empty
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        if(Session.getUser() == null)
        {
            Intent myIntent = new Intent(Main2Activity.this, MainActivity.class);
            //myIntent.putExtra("longitude", value); //Optional parameters
            Main2Activity.this.startActivity(myIntent);

        }

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main2);

        if (hasLocationPermissions(this, LOCATION_PERMISSIONS)) {
            Location location = getLocation();
            //location.getLatitude();
            if(location != null) {
                Session.setLocation(new LocationParameters(location.getLatitude(), location.getLongitude(), true));
                Toast.makeText(this, "Location: ON", Toast.LENGTH_SHORT).show();
            }
            else {
                Toast.makeText(this, "GPS na emulatoru iz nekog razloga ne funkcioniše", Toast.LENGTH_LONG).show();
                Session.setLocation(new LocationParameters(0, 0, false));
            }

        } else {
            ActivityCompat.requestPermissions(this, LOCATION_PERMISSIONS, MY_PERMISSIONS_REQUEST_LOCATION);
        }


        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder mBuilder =  new AlertDialog.Builder(Main2Activity.this);
                final View mView = getLayoutInflater().inflate(R.layout.search_layout, null);


                mBuilder.setView(mView);
                final AlertDialog dialog = mBuilder.create();
                dialog.show();

                ((Button) mView.findViewById(R.id.pretragaBtn)).setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Session.setSearchText(((EditText) mView.findViewById(R.id.pretraga)).getText()+"");

                        setFragment(new BlankFragment(), "projections search");
                        getSupportActionBar().setTitle("Projections");
                        dialog.dismiss();
                    }
                });

            }
        });

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
        View hView =  navigationView.getHeaderView(0);
        TextView nav_user = (TextView)hView.findViewById(R.id.userName);
        nav_user.setText(Session.getUser().getFirstName() + " " + Session.getUser().getLastName());
        ((TextView) hView.findViewById(R.id.userEmail)).setText(Session.getUser().getEmail());
        ((TextView) hView.findViewById(R.id.vpoints)).setText(Session.getVpAmount() + " VP");



        setFragment(new BlankFragment(),"projections");
        getSupportActionBar().setTitle(R.string.projections);
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main2, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {

        if(item.getItemId() == R.id.recomandedProjections) {

            setFragment(new BlankFragment(), "projections");
            getSupportActionBar().setTitle("Recommended projections");
        }
        if(item.getItemId() == R.id.allProjections) {
            AlertDialog.Builder mBuilder =  new AlertDialog.Builder(Main2Activity.this);
            final View mView = getLayoutInflater().inflate(R.layout.get_cinema_from_city, null);

            final List<String> countriesName = new ArrayList<String>();
            countriesName.add("Select country:");
            final List<String> cityNames = new ArrayList<String>();
            cityNames.add("Select city:");
            final List<String> cinemaNames = new ArrayList<String>();
            cinemaNames.add("Select cinema:");



            //GET COUNTRIES
            restInt.getCountries(new Callback<ArrayList<Countries>>() {
                @Override
                public void success(final ArrayList<Countries> countries, Response response) {

                    for (int i = 0; i < countries.size(); i++)
                        countriesName.add(countries.get(i).getName());

                    ArrayAdapter<String> adapter = new ArrayAdapter<String>(
                            Main2Activity.this, android.R.layout.simple_spinner_item,countriesName);

                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    Spinner sItems = (Spinner) mView.findViewById(R.id.countries);
                    sItems.setAdapter(adapter);

                    sItems.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                        @Override
                        public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                            if(i != 0) {
                                String selectedItem = adapterView.getItemAtPosition(i).toString();
                                for(Countries c : countries)
                                {
                                    if(c.getName().equals(selectedItem)) {
                                    //GET CITIES
                                    restInt.getCities(c.getCountryID(), new Callback<ArrayList<City>>() {
                                        @Override
                                        public void success(final ArrayList<City> cities, Response response) {
                                            cityNames.removeAll(cityNames);
                                            cinemaNames.removeAll(cinemaNames);
                                            for (int i = 0; i < cities.size(); i++)
                                                cityNames.add(cities.get(i).getName());

                                            ArrayAdapter<String> adapter = new ArrayAdapter<String>(
                                                    Main2Activity.this, android.R.layout.simple_spinner_item,cityNames);

                                            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                                            Spinner sItems = (Spinner) mView.findViewById(R.id.cities);
                                            sItems.setAdapter(adapter);

                                            sItems.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                                                @Override
                                                public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                                                        String selectedItem = adapterView.getItemAtPosition(i).toString();
                                                        for(City c : cities) {
                                                            if(c.getName().equals(selectedItem))
                                                            {
                                                                cinemaNames.removeAll(cinemaNames);
                                                                //GET CINEMAS
                                                                restInt.getCinemas(c.getCityID(), new Callback<ArrayList<Cinemas>>() {
                                                                    @Override
                                                                    public void success(final ArrayList<Cinemas> cinemas, Response response) {
                                                                        cinemaNames.removeAll(cinemaNames);
                                                                        for (int i = 0; i < cinemas.size(); i++)
                                                                            cinemaNames.add(cinemas.get(i).getName());

                                                                        ArrayAdapter<String> adapter = new ArrayAdapter<String>(
                                                                                Main2Activity.this, android.R.layout.simple_spinner_item,cinemaNames);

                                                                        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                                                                        Spinner sItems = (Spinner) mView.findViewById(R.id.cinemas);
                                                                        sItems.setAdapter(adapter);
                                                                        sItems.getEmptyView();

                                                                        sItems.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                                                                            @Override
                                                                            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                                                                                //PUT CINEMA ID IN SESSION
                                                                                String selectedItem = adapterView.getItemAtPosition(i).toString();
                                                                                for(Cinemas c : cinemas) {
                                                                                    if(c.getName().equals(selectedItem)) {
                                                                                            Session.setCinemaId(c.getCinemaID());

                                                                                    }
                                                                                }

                                                                            }

                                                                            @Override
                                                                            public void onNothingSelected(AdapterView<?> adapterView) {

                                                                            }
                                                                        });
                                                                    }

                                                                    @Override
                                                                    public void failure(RetrofitError error) {
                                                                        Toast.makeText(Main2Activity.this, "No cinemas for this city !", Toast.LENGTH_SHORT).show();
                                                                        Session.setCinemaId(0);

                                                                        Spinner sItems = (Spinner) mView.findViewById(R.id.cinemas);
                                                                        sItems.setAdapter(null);

                                                                    }
                                                                });
                                                            }
                                                        }

                                                }

                                                @Override
                                                public void onNothingSelected(AdapterView<?> adapterView) {

                                                }
                                            });
                                        }

                                        @Override
                                        public void failure(RetrofitError error) {
                                            Toast.makeText(Main2Activity.this, "No cities for this country !", Toast.LENGTH_SHORT).show();
                                            Session.setCinemaId(0);

                                            Spinner city = (Spinner) mView.findViewById(R.id.cities);
                                            city.setAdapter(null);
                                            Spinner sItems = (Spinner) mView.findViewById(R.id.cinemas);
                                            sItems.setAdapter(null);

                                        }
                                    });
                                    }
                                }
                            }

                        }

                        @Override
                        public void onNothingSelected(AdapterView<?> adapterView) {

                        }
                    });


                }

                @Override
                public void failure(RetrofitError error) {
                    Toast.makeText(Main2Activity.this, error.getMessage(), Toast.LENGTH_LONG).show();
                }

        });

            mBuilder.setView(mView);
            final AlertDialog dialog = mBuilder.create();
            dialog.show();

            Button confirm = (Button) mView.findViewById(R.id.getProjectionsBtn);

            confirm.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    if(Session.getCinemaId() != 0)
                    {
                        setFragment(new AllProjectionsFragment(), "All projections");
                        getSupportActionBar().setTitle("Projections");
                        dialog.dismiss();
                    }
                    else
                        Toast.makeText(Main2Activity.this, "Select cinema !", Toast.LENGTH_SHORT).show();
                }
            });

           // setFragment(new BlankFragment(), "projections");
            //getSupportActionBar().setTitle(R.string.projections);
        }
        if(item.getItemId() == R.id.reservations) {
            setFragment(new Reservations_fragment(), "reservations");
            getSupportActionBar().setTitle(R.string.reseravations);
        }
        if(item.getItemId() == R.id.buyVirtualPoints) {
            setFragment(new buyVp(), "reservations");
            getSupportActionBar().setTitle("Buy virtual points");
        }
        if(item.getItemId() == R.id.rateProjections) {
            setFragment(new watchedMoviesRateFragment(), "rateMovies");
            getSupportActionBar().setTitle("Watched movies");
        }
        if(item.getItemId() == R.id.profileDetails) {
            setFragment(new userProfileFragment(), "userProfile");
            getSupportActionBar().setTitle("Profile details");
        }
        if(item.getItemId() == R.id.logout) {
            Session.setUser(null);

            Intent myIntent = new Intent(Main2Activity.this, MainActivity.class);
            //myIntent.putExtra("longitude", value); //Optional parameters
            Main2Activity.this.startActivity(myIntent);

        }
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    protected void setFragment(Fragment fragment, String tag) {
        android.support.v4.app.FragmentTransaction t = getSupportFragmentManager().beginTransaction();
        t.replace(R.id.fragmert_place, fragment,tag);
        t.addToBackStack(null);
        t.commit();
    }


    //GEO LOKACIJA AKTIVIRANJE
    public  boolean hasLocationPermissions(Context context, String[] permissions) {
        boolean hasPermissions = true;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            hasPermissions = false;
            for(String permission : permissions) {
                hasPermissions = ActivityCompat.checkSelfPermission(context, permission) == PackageManager.PERMISSION_GRANTED;
            }
        }
        return hasPermissions;

    }

    public  void requestPermissions(Activity activity, String[] permissions, int requestCode) {
        ActivityCompat.requestPermissions(activity, permissions, requestCode);
    }


    public  boolean checkIfPermissionsWereGranted(int[] grantResults) {
        for (int grantResult : grantResults) {
            if (grantResult != PackageManager.PERMISSION_GRANTED) {
                return false;
            }
        }
        return true;
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {
        switch (requestCode) {
            case MY_PERMISSIONS_REQUEST_LOCATION: {
                if (checkIfPermissionsWereGranted(grantResults)) {
                    Location location = getLocation();

                    Session.setLocation(new LocationParameters(location.getLatitude(), location.getLongitude(), true));
                } else {
                    Toast.makeText(this, "Sistem preporuke zahtijeva geolokaciju za bolju preporuku projekcija", Toast.LENGTH_LONG).show();
                }
                break;
            }
        }
    }

    @SuppressLint("MissingPermission")
    public  Location getLocation() {
        LocationManager locationManager = (LocationManager)  this.getSystemService(Context.LOCATION_SERVICE);
        return locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
    }
}

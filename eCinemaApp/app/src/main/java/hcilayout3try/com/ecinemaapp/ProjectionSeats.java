package hcilayout3try.com.ecinemaapp;

import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.media.Image;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Gravity;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;
import android.widget.Toast;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.function.ToLongBiFunction;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.reservationDetails;
import hcilayout3try.com.ecinemaapp.helper.reservedSeats;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.City;
import hcilayout3try.com.ecinemaapp.models.Seats;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;

public class ProjectionSeats extends AppCompatActivity {
    public url httpConn = new url();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        Loading.Load(this, "", "Loading. Please wait ...");
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_projection_seats);
        final Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Select seats");
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        final RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();
        final Api restInt=radapter.create(Api.class);

        final View linearLayout =  findViewById(R.id.seats_layout);
        final LinearLayout layout = (LinearLayout) findViewById(R.id.info);
        final TableLayout seats = (TableLayout) findViewById(R.id.seatTable);

        TextView cinemaHall = (TextView) linearLayout.findViewById(R.id.cinemaHallSeats);
        cinemaHall.setText("Cinema hall: " + Session.getProjection().getCinemaHallName());

            restInt.getSeats(Session.getProjection().getProjectionID(), new Callback<ArrayList<Seats>>() {
            @Override
            public void success(ArrayList<Seats> model, Response response) {
                Loading.Dissmis();
                Session.setMaxSeats(0);

                int width = 0;
                int height = 0;

                for(int i = 0; i < model.size();i++)
                {
                    if(model.get(i).getSeatRowID() > width)
                        width  = model.get(i).getSeatRowID();

                    if(model.get(i).getSeatColumnID() > height)
                        height  = model.get(i).getSeatColumnID();

                }

                for(int j = 0; j < width +1; j++) {

                    TableRow rowTitle = new TableRow(ProjectionSeats.this);
                    rowTitle.setGravity(Gravity.CENTER);
                    rowTitle.setTextAlignment(View.TEXT_ALIGNMENT_CENTER);
                    TextView empty = new TextView(ProjectionSeats.this);

                    if(j==0)
                        rowTitle.addView(empty);
                    else {
                        empty.setText(Character.toString((char) (j + 64)));
                        rowTitle.addView(empty);
                    }

                    for (int i = 1; i < height +1; i++) {

                        if(j == 0) {
                            TextView title = new TextView(ProjectionSeats.this);
                            title.setText(i + "");
                            title.setGravity(Gravity.CENTER);
                            rowTitle.addView(title);
                        }
                        else {
                            CheckBox button = new CheckBox(ProjectionSeats.this);
                            button.setTag(j + "_" + i);
                            button.setVisibility(View.INVISIBLE);
                            button.setBackgroundColor(Color.TRANSPARENT);
                            button.setPadding(0,0,0,0);

                            rowTitle.addView(button);
                        }
                    }
                    seats.addView(rowTitle);
                }


                for(int i = 0; i < model.size(); i++)
                {
                    final CheckBox tv = (CheckBox) seats.findViewWithTag(model.get(i).getSeatRowID() + "_" + model.get(i).getSeatColumnID());
                    tv.setVisibility(View.VISIBLE);
                    if(model.get(i).getIsReserved())
                    {
                        tv.setButtonDrawable(R.drawable.seat_disabled);

                        tv.setClickable(false);
                    }
                        else {
                        tv.setButtonDrawable(R.drawable.seat);
                        tv.setId(model.get(i).getSeatID());
                        tv.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                            @Override
                            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                                if(tv.isChecked()) {
                                    if (Session.getMaxSeats() < 4) {
                                        reservedSeats.AddSeat(tv.getId());
                                        Session.setMaxSeats(Session.getMaxSeats()+1);
                                    }
                                    else {
                                        tv.setChecked(false);
                                    }
                                }
                                else
                                {
                                    Session.setMaxSeats(Session.getMaxSeats()-1);
                                    reservedSeats.RemoveSeat(tv.getId());
                                    tv.setClickable(true);
                                }
                            }
                        });
                    }

                }

            }

            @Override
            public void failure(RetrofitError error) {


            }

        });

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder builder = new AlertDialog.Builder(ProjectionSeats.this);
                builder.setMessage("Confirm reservation ?")
                        .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                            if(reservedSeats.getSeats().size() == 0)
                            {
                                Toast.makeText(ProjectionSeats.this, "Select min. 1 seat !",Toast.LENGTH_SHORT).show();
                            }
                            else {
                                restInt.makeReservation(new reservationDetails(Session.getUser().getUserID(), Session.getProjection().getProjectionID()), new Callback<String>() {
                                    @Override
                                    public void success(String model, Response response) {

                                        Toast.makeText(ProjectionSeats.this, "Reservation made successfully",Toast.LENGTH_LONG).show();
                                        reservedSeats.RemoveAll();

                                        Intent myIntent = new Intent(ProjectionSeats.this, Main2Activity.class);
                                        //myIntent.putExtra("longitude", value); //Optional parameters
                                        ProjectionSeats.this.startActivity(myIntent);
                                    }
                                    @Override
                                    public void failure(RetrofitError error) {
                                        Toast.makeText(ProjectionSeats.this, "Error making reservation!",Toast.LENGTH_SHORT).show();
                                    }

                                });
                            }

                            }
                        })
                        .setNegativeButton("No", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                // User cancelled the dialog
                            }
                        });
                // Create the AlertDialog object and return it
                AlertDialog dialog = builder.create();
                dialog.show();
            }
        });
    }
    @Override
    public boolean onSupportNavigateUp(){
        finish();
        return true;
    }


}

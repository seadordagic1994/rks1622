package hcilayout3try.com.ecinemaapp;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.location.Location;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.text.Layout;
import android.text.format.DateFormat;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.text.SimpleDateFormat;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.stream.Collectors;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.MyGson;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.projection_request;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.City;
import hcilayout3try.com.ecinemaapp.models.MovieProjections;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;
import retrofit.converter.GsonConverter;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link BlankFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link BlankFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class BlankFragment extends Fragment
 {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;
    public url httpConn = new url();

    private OnFragmentInteractionListener mListener;

    public BlankFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment BlankFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static BlankFragment newInstance(String param1, String param2) {
        BlankFragment fragment = new BlankFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        Loading.Load(getActivity(), "", "Loading. Please wait ...");
        LocationManager lm = (LocationManager) getActivity().getSystemService(Context.LOCATION_SERVICE);
        double longitude = 0;
        double latitude = 0;



        //final Location location = lm.getLastKnownLocation(LocationManager.GPS_PROVIDER);
           // if(location != null) {
             //   longitude = location.getLongitude();
               // latitude = location.getLatitude();}


        // Inflate the layout for this fragment
        final View view = inflater.inflate(R.layout.fragment_blank, container, false);

        final ListView projekcije = (ListView) view.findViewById(R.id.listProjections);

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();


        RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();
        Api restInt=radapter.create(Api.class);

        final List<MovieProjections> listaProjekcija = new ArrayList<MovieProjections>();
        String search = (Session.getSearchText() != null) ? Session.getSearchText() : "";

        restInt.projections(new projection_request(Session.getUser().getUserID(),Session.getLocation().Latitude,Session.getLocation().Longitude,Session.getLocation().GPSAquired, search), new Callback<List<MovieProjections>>() {
            @Override
            public void success(final List<MovieProjections> model, Response response) {
                if(Session.getSearchText() != null)
                    Session.setSearchText("");
                Loading.Dissmis();
                projekcije.setAdapter(new BaseAdapter() {
                    @Override
                    public int getCount() {
                        return model.size();
                    }

                    @Override
                    public Object getItem(int i) {
                        return model.get(i);
                    }

                    @Override
                    public long getItemId(int i) {
                        return i;
                    }

                    @Override
                    public View getView(int i, View view, ViewGroup viewGroup) {

                        if( view == null)
                        {
                            final LayoutInflater inflater = (LayoutInflater)getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                            view = inflater.inflate(R.layout.single_projection_list_item, viewGroup, false);
                        }
                        listaProjekcija.add(model.get(i));

                        TextView movieName = (TextView) view.findViewById(R.id.movieName);
                        TextView duration = (TextView) view.findViewById(R.id.duration);
                        TextView age = (TextView) view.findViewById(R.id.age);
                        TextView release = (TextView) view.findViewById(R.id.release);
                        ImageView cover = (ImageView) view.findViewById(R.id.cover);

                        byte[] decodedString = Base64.decode(model.get(i).getMovieCover(), Base64.DEFAULT);
                        Bitmap bmp = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

                        cover.setImageBitmap(bmp);
                        movieName.setText(model.get(i).getMovieName());
                        duration.setText(model.get(i).getDuration()+" min");
                        age.setText(model.get(i).getAge());
                        release.setText(model.get(i).getRelease());

                        return view;
                    }
                });

                projekcije.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                    @Override
                    public void onItemClick(AdapterView<?> adapterView, View view, final int i, long l) {

                        Session.setTimesForProjection(listaProjekcija.get(i));

                        setFragment(new projectionTimesFragment(), "projectionTimes");
                        ((AppCompatActivity)getActivity()).getSupportActionBar().setTitle("Projection times");

                    }
                });
            }

            @Override
            public void failure(RetrofitError error) {
                Loading.Dissmis();
                CharSequence text = "nesto nije uredu";
                int duration = Toast.LENGTH_LONG;

                Toast toast = Toast.makeText(getActivity(), error.getMessage(), duration);
                toast.show();

            }
        });



        return view;
    }

    protected void setFragment(Fragment fragment, String tag) {
        android.support.v4.app.FragmentTransaction t = getActivity().getSupportFragmentManager().beginTransaction();
        t.replace(R.id.fragmert_place, fragment,tag);
        t.addToBackStack(null);
        t.commit();
    }
    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}

package hcilayout3try.com.ecinemaapp;

import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.graphics.drawable.LayerDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RatingBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;
import java.util.List;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.projection_request;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.MovieProjections;
import hcilayout3try.com.ecinemaapp.models.movieRatingParameters;
import hcilayout3try.com.ecinemaapp.models.watchedMovies;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link watchedMoviesRateFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link watchedMoviesRateFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class watchedMoviesRateFragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    public url httpConn = new url();

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    public watchedMoviesRateFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment watchedMoviesRateFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static watchedMoviesRateFragment newInstance(String param1, String param2) {
        watchedMoviesRateFragment fragment = new watchedMoviesRateFragment();
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

        final View view = inflater.inflate(R.layout.fragment_watched_movies_rate, container, false);

        final ListView projekcije = (ListView) view.findViewById(R.id.watchedMoviesList);

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();


        RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();
       final Api restInt=radapter.create(Api.class);


        restInt.getWatchedMovies(Session.getUser().getUserID(), new Callback<ArrayList<watchedMovies>>() {
            @Override
            public void success(final ArrayList<watchedMovies> model, Response response) {
                Loading.Dissmis();
                if (model.size() == 0)
                {
                    Toast.makeText(getActivity(), "You haven't seen any movies yet!", Toast.LENGTH_LONG).show();
                }
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
                    public View getView(final int i, View view, ViewGroup viewGroup) {

                        if( view == null)
                        {
                            final LayoutInflater inflater = (LayoutInflater)getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                            view = inflater.inflate(R.layout.watched_movies_list_single_item, viewGroup, false);
                        }


                        TextView movieName = (TextView) view.findViewById(R.id.myImageViewText);
                        ImageView cover = (ImageView) view.findViewById(R.id.myImageView);

                        byte[] decodedString = Base64.decode(model.get(i).getMovieCover(), Base64.DEFAULT);
                        Bitmap bmp = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

                        cover.setImageBitmap(bmp);
                        movieName.setText(model.get(i).getMovieName());

                        final RatingBar ratingBar = new RatingBar(getContext(), null, android.R.attr.ratingBarStyle);

                        RelativeLayout.LayoutParams params= new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT,ViewGroup.LayoutParams.WRAP_CONTENT);
                        params.addRule(RelativeLayout.BELOW, R.id.myImageView);
                        params.addRule(RelativeLayout.ALIGN_END, R.id.myImageView);
                        ratingBar.setLayoutParams(params);
                        ratingBar.setTag(model.get(i).getMovieID());
                        LayerDrawable stars = (LayerDrawable) ratingBar.getProgressDrawable();
                        stars.getDrawable(2).setColorFilter(getResources().getColor(R.color.redHighlighted), PorterDuff.Mode.SRC_ATOP);
                        stars.getDrawable(0).setColorFilter(getResources().getColor(R.color.lightGray), PorterDuff.Mode.SRC_ATOP);
                        stars.getDrawable(1).setColorFilter(getResources().getColor(R.color.lightGray), PorterDuff.Mode.SRC_ATOP);
                        ratingBar.setIsIndicator(false);

                        ratingBar.setNumStars(5);
                        ratingBar.setStepSize(1);
                        ratingBar.setRating(model.get(i).getMovieRating());


                        ratingBar.setOnRatingBarChangeListener(new RatingBar.OnRatingBarChangeListener() {
                            @Override
                            public void onRatingChanged(RatingBar ratingBar, float rating, boolean fromUser) {
                                restInt.RateMovie(new movieRatingParameters( Session.getUser().getUserID() + "",model.get(i).getMovieID()+"",Math.round(rating)+""), new Callback<String>() {
                                    @Override
                                    public void success(String s, Response response) {
                                        Toast.makeText(getActivity(), "Movie successfully rated", Toast.LENGTH_SHORT).show();
                                    }

                                    @Override
                                    public void failure(RetrofitError error) {
                                        Toast.makeText(getActivity(), error.getMessage(), Toast.LENGTH_SHORT).show();
                                    }
                                });
                            }
                        });

                        ((RelativeLayout) view.findViewById(R.id.relativelayoutRate)).addView(ratingBar);


                        return view;
                    }
                });
            }

            @Override
            public void failure(RetrofitError error) {
                CharSequence text = "nesto nije uredu";
                int duration = Toast.LENGTH_SHORT;

                Toast toast = Toast.makeText(getActivity(), error.getMessage(), duration);
                toast.show();
            }
        });



        return view;
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

package hcilayout3try.com.ecinemaapp;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.models.MovieProjections;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link projectionTimesFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link projectionTimesFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class projectionTimesFragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    public projectionTimesFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment projectionTimesFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static projectionTimesFragment newInstance(String param1, String param2) {
        projectionTimesFragment fragment = new projectionTimesFragment();
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
        View mView = inflater.inflate(R.layout.projection_times_single_dialog, container, false);



        final ListView projekcije = (ListView) mView.findViewById(R.id.projectionTimes_list);
        final MovieProjections projectionTimes = Session.getTimesForProjection();

        TextView movieName = (TextView) mView.findViewById(R.id.movieNameProjections);
        movieName.setText(projectionTimes.getMovieName());

        projekcije.setAdapter(new BaseAdapter() {
            @Override
            public int getCount() {
                return projectionTimes.getProjections().size();
            }

            @Override
            public Object getItem(int i) {
                return projectionTimes.getProjections().get(i);
            }

            @Override
            public long getItemId(int i) {
                return 0;
            }

            @Override
            public View getView(int i, View view, ViewGroup viewGroup) {

                Session.setMovieId(projectionTimes.getMovieID());
                if( view == null)
                {
                    final LayoutInflater inflater = (LayoutInflater)getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                    view = inflater.inflate(R.layout.projection_times_list_item, viewGroup, false);
                }

                TextView cinema = (TextView) view.findViewById(R.id.cinema);
                TextView cinemaHall = (TextView) view.findViewById(R.id.cinemaHall);
                TextView start = (TextView) view.findViewById(R.id.startTime);
                TextView ticket = (TextView) view.findViewById(R.id.ticketPrice);
                TextView tech = (TextView) view.findViewById(R.id.techType);

                cinema.setText(projectionTimes.getProjections().get(i).getCinemaName());
                cinemaHall.setText(projectionTimes.getProjections().get(i).getCinemaHallName());
                start.setText(projectionTimes.getProjections().get(i).getDateTimeStart().split("T")[0] + " " + projectionTimes.getProjections().get(i).getDateTimeStart().split("T")[1]);
                ticket.setText(projectionTimes.getProjections().get(i).getTicketPrice() + "");
                tech.setText(projectionTimes.getProjections().get(i).getName());

                return view;
            }
        });

        projekcije.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                Intent myIntent = new Intent(getActivity(), ProjectionSeats.class);
                Session.setProjection(projectionTimes.getProjections().get(i));
                getActivity().startActivity(myIntent);
            }
        });
        // Inflate the layout for this fragment

        return mView;
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

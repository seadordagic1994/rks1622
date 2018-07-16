package hcilayout3try.com.ecinemaapp;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.PorterDuff;
import android.graphics.Typeface;
import android.graphics.drawable.LayerDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RatingBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.vp_packs;
import hcilayout3try.com.ecinemaapp.models.vp_params;
import hcilayout3try.com.ecinemaapp.models.watchedMovies;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link buyVp.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link buyVp#newInstance} factory method to
 * create an instance of this fragment.
 */
public class buyVp extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    public url httpConn = new url();

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    public buyVp() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment buyVp.
     */
    // TODO: Rename and change types and number of parameters
    public static buyVp newInstance(String param1, String param2) {
        buyVp fragment = new buyVp();
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
        // Inflate the layout for this fragment
        Loading.Load(getActivity(), "", "Loading. Please wait ...");

        final View view = inflater.inflate(R.layout.fragment_buy_vp, container, false);

        final ListView vpList = (ListView) view.findViewById(R.id.listVp);

        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();


        RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();
        final Api restInt=radapter.create(Api.class);


        restInt.getVpPacks( new Callback<ArrayList<vp_packs>>() {
            @Override
            public void success(final ArrayList<vp_packs> model, Response response) {
                Loading.Dissmis();
                vpList.setAdapter(new BaseAdapter() {
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
                    public View getView(int i, View mView, ViewGroup viewGroup) {

                        if( mView == null)
                        {
                            final LayoutInflater inflater = (LayoutInflater)getActivity().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                            mView = inflater.inflate(R.layout.buy_vp_list_single_item, viewGroup, false);
                        }

                        ((TextView)mView.findViewById(R.id.point)).setText(model.get(i).getAmount() + " VP");


                        return mView;
                    }
                });

                vpList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                    @Override
                    public void onItemClick(AdapterView<?> adapterView, View view,final int i, long l) {
                        restInt.setVP(new vp_params(Session.getUser().getUserID()+"", model.get(i).getVirtualPointsPacketID()+""), new Callback<String>() {
                            @Override
                            public void success(String s, Response response) {
                                Session.setVPAmount(Session.getVpAmount() + model.get(i).getAmount());
                                Toast.makeText(getActivity(), "Successfully done !", Toast.LENGTH_SHORT).show();
                                Intent myIntent = new Intent(getContext(), Main2Activity.class);
                                //myIntent.putExtra("longitude", value); //Optional parameters
                                getActivity().startActivity(myIntent);

                            }

                            @Override
                            public void failure(RetrofitError error) {
                                Toast.makeText(getContext(), error.getMessage(), Toast.LENGTH_SHORT);
                            }
                        });
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

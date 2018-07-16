package hcilayout3try.com.ecinemaapp;

import android.animation.Animator;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Base64;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.MultiFormatWriter;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;
import com.google.zxing.qrcode.QRCodeWriter;
import com.journeyapps.barcodescanner.BarcodeEncoder;

import java.text.DecimalFormat;
import java.util.ArrayList;

import hcilayout3try.com.ecinemaapp.api.Api;
import hcilayout3try.com.ecinemaapp.helper.Loading;
import hcilayout3try.com.ecinemaapp.helper.Session;
import hcilayout3try.com.ecinemaapp.helper.url;
import hcilayout3try.com.ecinemaapp.models.BuyTicketParameters;
import hcilayout3try.com.ecinemaapp.models.userReservations;
import retrofit.Callback;
import retrofit.RestAdapter;
import retrofit.RetrofitError;
import retrofit.client.Response;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link Reservations_fragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link Reservations_fragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class Reservations_fragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private Animator mCurrentAnimator;
    private int mShortAnimationDuration;
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    public url httpConn = new url();

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    public Reservations_fragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment Reservations_fragment.
     */
    // TODO: Rename and change types and number of parameters
    public static Reservations_fragment newInstance(String param1, String param2) {
        Reservations_fragment fragment = new Reservations_fragment();
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
        // Inflate the layout for this fragment

        final View view = inflater.inflate(R.layout.fragment_reservations_fragment, container, false);
        final ListView rezervacije = (ListView) view.findViewById(R.id.listReservation);

        RestAdapter radapter=new RestAdapter.Builder().setEndpoint(httpConn.urlString).build();

        final Api restInt=radapter.create(Api.class);

        restInt.getReservations(Session.getUser().getUserID(), new Callback<ArrayList<userReservations>>() {
            @Override
            public void success(final ArrayList<userReservations> userReservations, Response response) {
                Loading.Dissmis();

                if (userReservations.size() == 0)
                {
                    Toast.makeText(getActivity(), "You have no reservations !", Toast.LENGTH_LONG).show();
                }

                rezervacije.setAdapter(new BaseAdapter() {
                    @Override
                    public int getCount() {
                        return userReservations.size();
                    }

                    @Override
                    public Object getItem(int i) {
                        return userReservations.get(i);
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
                            view = inflater.inflate(R.layout.single_reservation_list_item, viewGroup, false);
                        }

                        ((TextView) view.findViewById(R.id.movieName)).setText(userReservations.get(i).getMovieName());

                        //Log.w("myApp", "datum je: " + userReservations.get(i).getOrderDate());
                        ((TextView) view.findViewById(R.id.orderDate)).setText(userReservations.get(i).getOrderDate().split("T")[0] + " " +userReservations.get(i).getOrderDate().split("T")[1].split(":")[0] + ":" + userReservations.get(i).getOrderDate().split("T")[1].split(":")[1]);
                        ((TextView) view.findViewById(R.id.expireDate)).setText(userReservations.get(i).getExpireDate().split("T")[0] + " " + userReservations.get(i).getExpireDate().split("T")[1].split(":")[0] + ":" + userReservations.get(i).getExpireDate().split("T")[1].split(":")[1]);
                       ((TextView) view.findViewById(R.id.ticketPrice)).setText(new DecimalFormat("0.00##").format(userReservations.get(i).getTotal()) + " KM");
                        final ImageView code = (ImageView) view.findViewById(R.id.code);


                        MultiFormatWriter multiFormatWriter = new MultiFormatWriter();
                        BitMatrix bitMatrix = null;
                        try {
                            bitMatrix = multiFormatWriter.encode(userReservations.get(i).getReservationID()+"", BarcodeFormat.QR_CODE,300,300);
                        } catch (WriterException e) {
                            e.printStackTrace();
                        }
                        BarcodeEncoder barcodeEncoder = new BarcodeEncoder();
                        Bitmap bitmpap = barcodeEncoder.createBitmap(bitMatrix);
                        code.setImageBitmap(bitmpap);

                        RelativeLayout.LayoutParams paramsCancel= new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT,ViewGroup.LayoutParams.WRAP_CONTENT);
                        paramsCancel.addRule(RelativeLayout.ALIGN_PARENT_RIGHT, 1);
                        paramsCancel.addRule(RelativeLayout.BELOW, R.id.code);

                        final Button cancel = new Button(getContext());
                        cancel.setLayoutParams(paramsCancel);
                        cancel.setTag(userReservations.get(i).getReservationID()+ "_" + "1");
                        cancel.setText("Cancel");
                        cancel.setMinimumWidth(0);
                        cancel.setMinHeight(0);
                        cancel.setMinimumHeight(0);
                        cancel.setMinWidth(0);
                        cancel.setTextSize(10);
                        cancel.setPadding(25,25,25,25);
                        cancel.setStateListAnimator(null);

                        cancel.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {

                                restInt.setReservationCancel(new BuyTicketParameters(((String) cancel.getTag()).split("_")[1], ((String) cancel.getTag()).split("_")[0]), new Callback<String>() {
                                    @Override
                                    public void success(String s, Response response) {
                                        Toast.makeText(getContext(), s, Toast.LENGTH_SHORT).show();
                                        Intent myIntent = new Intent(getContext(), Main2Activity.class);
                                        //myIntent.putExtra("longitude", value); //Optional parameters
                                        getContext().startActivity(myIntent);
                                    }

                                    @Override
                                    public void failure(RetrofitError error) {
                                        Toast.makeText(getContext(), error.getMessage(), Toast.LENGTH_SHORT).show();
                                    }
                                });

                            }
                        });

                        ((RelativeLayout)view).addView(cancel);

                        RelativeLayout.LayoutParams paramsConfirm= new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT,ViewGroup.LayoutParams.WRAP_CONTENT);
                        paramsConfirm.addRule(RelativeLayout.ALIGN_PARENT_RIGHT, 1);
                        paramsConfirm.setMargins(0,0,115,0);
                        paramsConfirm.addRule(RelativeLayout.BELOW, R.id.code);

                        final Button confirm = new Button(getContext());
                        confirm.setLayoutParams(paramsConfirm);
                        confirm.setTag(userReservations.get(i).getReservationID()+ "_" + "1");
                        confirm.setText("Confirm");
                        confirm.setMinimumWidth(0);
                        confirm.setMinHeight(0);
                        confirm.setMinimumHeight(0);
                        confirm.setMinWidth(0);
                        confirm.setTextSize(10);
                        confirm.setPadding(25,25,25,25);
                        confirm.setStateListAnimator(null);

                        confirm.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                              restInt.setReservationConfirmation(new BuyTicketParameters(((String) confirm.getTag()).split("_")[1], ((String) confirm.getTag()).split("_")[0]), new Callback<String>() {
                                  @Override
                                  public void success(String s, Response response) {
                                      Toast.makeText(getContext(), s, Toast.LENGTH_SHORT).show();
                                      Intent myIntent = new Intent(getContext(), Main2Activity.class);
                                      //myIntent.putExtra("longitude", value); //Optional parameters
                                      getContext().startActivity(myIntent);
                                  }

                                  @Override
                                  public void failure(RetrofitError error) {
                                      Toast.makeText(getContext(), error.getMessage(), Toast.LENGTH_SHORT).show();
                                  }
                              });

                            }
                        });

                        ((RelativeLayout)view).addView(confirm);



                        return view;

                    }
                });
            }

            @Override
            public void failure(RetrofitError error) {

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

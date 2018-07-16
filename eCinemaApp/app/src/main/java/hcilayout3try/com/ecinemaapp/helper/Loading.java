package hcilayout3try.com.ecinemaapp.helper;

import android.app.ProgressDialog;
import android.content.Context;

/**
 * Created by SeadOrdagić on 31.05.2018..
 */

public class Loading {

    public static ProgressDialog dialog;

    public static void Load(Context c,String header, String message)
    {
        dialog = ProgressDialog.show(c, header, message, true);
    }

    public static void Dissmis()
    {
        dialog.dismiss();
    }
}

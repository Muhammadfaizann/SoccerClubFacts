using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SoccerClubFacts.Resources.values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoccerClubFacts.Resources
{
    class Prefs
    {

        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext;

        private static String PREFERENCE_ACCESS_KEY = "PREFERENCE_ACCESS_KEY";

        public Prefs(Context context)
        {
            this.mContext = context;
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }

        public void saveAccessKey(string key)
        {
            mPrefsEditor.PutString(PREFERENCE_ACCESS_KEY, key);
            mPrefsEditor.Commit();
        }

        public string getAccessKey()
        {
            return mSharedPrefs.GetString(PREFERENCE_ACCESS_KEY, "");
        }


        public string getLanguage(String key)
        {
            return mSharedPrefs.GetString(key, "eng");

        }


        public void setLanguage(String key,String value)
        {
            mPrefsEditor.PutString(key,value);
            mPrefsEditor.Commit();

        }




        public string getControl(String key)
        {
            return mSharedPrefs.GetString(key, Constants.CLICK_KEY);

        }


        public void setControl(String key, String value)
        {
            mPrefsEditor.PutString(key, value);
            mPrefsEditor.Commit();

        }

        public int getLastFact(String key)
        {
            return mSharedPrefs.GetInt(key, 0);

        }


        public void setLastFact(String key, int value)
        {
            mPrefsEditor.PutInt(key, value);
            mPrefsEditor.Commit();

        }





        public string getSounds(String key)
        {
            return mSharedPrefs.GetString(key, "on");

        }


        public void setSounds(String key, String value)
        {
            mPrefsEditor.PutString(key, value);
            mPrefsEditor.Commit();

        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SoccerClubFacts.Resources;
using SoccerClubFacts.Resources.values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoccerClubFacts
{
    [Activity(Label = "LanguageAct")]
    public class LanguageAct : Activity
    {

        ImageView langImg;
        TextView langTv;

        private Prefs prefs;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.language);

            itemsDefinitions();

            prefs = new Prefs(this);


            checkLanguage();

            langImg.Click += (sender, e) =>
            {


                if (prefs.getLanguage(Constants.LANG_KEY).Equals("ru"))
                {
                    prefs.setLanguage(Constants.LANG_KEY, "eng");

                    checkLanguage();
                }
                else
                {
                    prefs.setLanguage(Constants.LANG_KEY, "ru");
                    checkLanguage();
                }

            };



        }

        private void itemsDefinitions()
        {

        
            langImg = FindViewById<ImageView>(Resource.Id.langImg);
            langTv = FindViewById<TextView>(Resource.Id.langTv);




        }




        private void checkLanguage()
        {

            if (prefs.getLanguage(Constants.LANG_KEY).Equals("ru"))
            {


                langImg.SetImageResource(Resource.Drawable.lang_img_ru);
                langTv.SetText(Resource.String.lang_txt_ru);

            }
            else
            {
                langImg.SetImageResource(Resource.Drawable.lang_img_eng);
                langTv.SetText(Resource.String.lang_txt_eng);
            }


        }


        public override void OnBackPressed()
        {

            Intent intent = new Intent(this, typeof(MainMenu));

            StartActivity(intent);
            Finish();

        }

    }
}
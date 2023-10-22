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
    [Activity(Label = "SettingAct")]
    public class SettingAct : Activity
    {


        private TextView musicTv;
        private ImageView musicImg;

        private TextView soundTv;
        private ImageView soundImg;


        private TextView controlTv;
        private ImageView controlImg;

        private TextView settingTv;


        private Prefs prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.setting);

            itemsDefinitions();

            prefs = new Prefs(this);


            checkLanguage();

            checkSounds();
            checkControl();


            soundImg.Click += (sender, e) => {

                if (prefs.getSounds(Constants.SOUND_KEY).Equals("off"))
                {

                    prefs.setSounds(Constants.SOUND_KEY, "on");

                    checkSounds();
                }
                else
                {


                    prefs.setSounds(Constants.SOUND_KEY, "off");

                    checkSounds();

                }

               




            };



            musicImg.Click += (sender, e) => {

                if (prefs.getSounds(Constants.MUSIC_KEY).Equals("off"))
                {

                    prefs.setSounds(Constants.MUSIC_KEY, "on");

                    checkSounds();
                }
                else
                {


                    prefs.setSounds(Constants.MUSIC_KEY, "off");

                    checkSounds();

                }






            };


            controlImg.Click += (sender, e) =>
            {


                if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.CLICK_KEY))
                {
                    prefs.setControl(Constants.CONTROL_TYPE_KEY, Constants.SHAKE_KEY);
                    checkControl();
                    

                }
                else if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.SHAKE_KEY))
                {
                    prefs.setControl(Constants.CONTROL_TYPE_KEY, Constants.CLICK_KEY);
                    checkControl();


                }
            };

        }

        private void itemsDefinitions()
        {


        musicTv = FindViewById<TextView>(Resource.Id.musicTv);
            musicImg = FindViewById<ImageView>(Resource.Id.musicImg);

            soundTv = FindViewById<TextView>(Resource.Id.soundTv);
            soundImg = FindViewById<ImageView>(Resource.Id.soundImg);
            
            controlTv = FindViewById<TextView>(Resource.Id.controlTv);
            controlImg = FindViewById<ImageView>(Resource.Id.controlImg);
            
            settingTv = FindViewById<TextView>(Resource.Id.settingTv);


        }


        private void checkLanguage()
        {
            if (prefs.getLanguage(Constants.LANG_KEY).Equals("ru"))
            {
                musicTv.SetText(Resource.String.music_txt_ru);
                soundTv.SetText(Resource.String.Sound_txt_ru);
                controlTv.SetText(Resource.String.control_txt_ru);

                settingTv.SetText(Resource.String.setting_txt_ru);


            }
            else
            {
                musicTv.SetText(Resource.String.music_txt_eng);
                soundTv.SetText(Resource.String.sound_txt_eng);
                controlTv.SetText(Resource.String.control_txt_eng);


                settingTv.SetText(Resource.String.setting_txt_eng);
            }


        }


        private void checkControl()
        {


            if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.CLICK_KEY)){

                controlImg.SetImageResource(Resource.Drawable.control_1);

            }
            else if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.SHAKE_KEY)) 
            {
                controlImg.SetImageResource(Resource.Drawable.control_2);

            }



        }


        private void checkSounds()
        {
            if (prefs.getSounds(Constants.SOUND_KEY).Equals("off"))
            {


               
                soundImg.SetImageResource(Resource.Drawable.sound_off);
            }
            else
            {

               
                soundImg.SetImageResource(Resource.Drawable.sound_on);

            }


            if (prefs.getSounds(Constants.MUSIC_KEY).Equals("off"))
            {


                musicImg.SetImageResource(Resource.Drawable.music_off);
               
            }
            else
            {

                musicImg.SetImageResource(Resource.Drawable.music_on);
     

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
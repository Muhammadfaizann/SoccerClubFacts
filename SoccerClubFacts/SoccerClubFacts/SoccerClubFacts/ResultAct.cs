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
using Android.Media;


namespace SoccerClubFacts
{
    [Activity(Label = "ResultAct")]
    public class ResultAct : Activity
    {

        private ImageView more_img_result;

        private ImageView restart_img_result;


        private ScrollView rvResutBg;
        private TextView factTitle;
        private TextView factTxt;

        private String factTxtStr = " ";

        private Prefs prefs;

        //SOUNDS:
        MediaPlayer mediaPlayer;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.result_act);

            prefs = new Prefs(this);

            hideNavBar();
            itemsDefinitions();

            initMediaPlayer(Resource.Raw.music);

            checkSounds();



            int factTitleRes= Intent.GetIntExtra(Constants.FACT_TITLE_KEY,0);
            int factTxtRes = Intent.GetIntExtra(Constants.FACT_TXT_KEY, 0);
            int factImgRes = Intent.GetIntExtra(Constants.FACT_IMG_KEY, 0);

            //QUICK CHECK IF ALL IS GOING WELL:
            if(factTitleRes != 0&& factTxtRes != 0 && factImgRes != 0)
            {

                 factTxtStr = GetText(factTxtRes);

                rvResutBg.SetBackgroundResource(factImgRes);
                factTitle.SetText(factTitleRes);
                //factTxt.SetText(factTxtRes);

                if (factTxtStr.Length > 100)
                {
                    //factTxt.Text = factTxtStr.Substring(0, 100);
                    factTxt.Text = factTxtStr.Substring(0, 100)+"....";
                }
                else
                {
                    factTxt.Text = factTxtStr.Substring(0, 50);
                }
                

                

            }





            restart_img_result.Click += (sender, e) =>
            {

                resetMediaPlayer();

                Intent intent = new Intent(this, typeof(MainMenu));

                StartActivity(intent);
                Finish();
            };






            more_img_result.Click += (sender, e) =>
            {

                factTxt.Text = factTxtStr;
      

            };

        }

        private void itemsDefinitions()
        {



            rvResutBg = FindViewById<ScrollView>(Resource.Id.rvResutBg);
            factTitle = FindViewById<TextView>(Resource.Id.factTitle);
            factTxt = FindViewById<TextView>(Resource.Id.factTxt);


            restart_img_result = FindViewById<ImageView>(Resource.Id.restart_img_result);
            more_img_result = FindViewById<ImageView>(Resource.Id.more_img_result);



        }



        private void hideNavBar()
        {
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }


        private void initMediaPlayer(int musicRes)
        {
            mediaPlayer = new MediaPlayer();
            //mediaPlayer = MediaPlayer.Create(this, Resource.Raw.music);
            mediaPlayer = MediaPlayer.Create(this, musicRes);



            //mediaPlayer.SetAudioAttributes(
            // new AudioAttributes.Builder()
            //.SetContentType(AudioAttributes.CONTENT_TYPE_MUSIC)
            //.setUsage(AudioAttributes.USAGE_MEDIA)
            //.build()
            //);

            //mediaPlayer.SetAudioStreamType(AudioManager.STREAM_MUSIC);
            mediaPlayer.SetVolume(1.0f, 1.0f);
            //mediaPlayer.setVolume(0.2f, 0.2f);


        }


        private void checkSounds()
        {
            if (prefs.getSounds(Constants.MUSIC_KEY).Equals("on"))
            {

                mediaPlayer.Start();
            }

        }

        private void resetMediaPlayer()
        {

            if (mediaPlayer != null)
            {
                if (mediaPlayer.IsPlaying)
                {
                    //mediaPlayer.stop();
                    mediaPlayer.Reset();

                }
                mediaPlayer.Release();
                mediaPlayer = null;
            }

        }


        public override void OnBackPressed()
        {
            resetMediaPlayer();

            Intent intent = new Intent(this, typeof(MainMenu));

            StartActivity(intent);
            Finish();

        }



    }

    
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoccerClubFacts.Resources.values;
using Android.Views.Animations;
using Java.Util;
using Android.Graphics.Drawables;

using Xamarin.Essentials;
using Android.Hardware;
using SoccerClubFacts.Resources;
using SoccerClubFacts.Resources.values;
using Random = System.Random;
using Android.Media;

namespace SoccerClubFacts.Resources
{
    [Activity(Label = "MainMenu")]
    public class MainMenu : Activity,ISensorEventListener
    {

        ImageView img_main_menu;
        ImageView setting_main_menu;
        ImageView lang_main_menu;


        TextView shake_txt_tv_main_menu;

        private Prefs prefs;


        Handler h = new Handler();

        //SOUNDS:
        MediaPlayer mediaPlayer;

        List<int> factsTitleEng = new List<int>();
        List<int> factsTxtEng = new List<int>();


        List<int> factsTitleRu = new List<int>();
        List<int> factsTxtRu = new List<int>();

        List<int> factsImgsEng = new List<int>();


        //SHAKE:
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.Game;


        SensorManager sensorManager;
        float last_x = 0.0f;
        float last_y = 0.0f;
        float last_z = 0.0f;
        long last_Time = 0;
        static readonly object _syncLock = new object();
        float ThreadShould = 1000;//3000;//MINI SPEED

        bool hasUpdated = false;
        DateTime lastUpdate;


        const int ShakeDetectionTimeLapse = 250;
        const double ShakeThreshold = 800;


        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.main_menu);

            //HIDE NAVIGATION BAR:
            hideNavBar();
             prefs = new Prefs(this);


            initMediaPlayer(Resource.Raw.music);

            initFactsData();



            itemsDefinitions();


            checkLanguage();
           


            checkSounds();


            
            sensorManage();
            
            
            img_main_menu.Click += (sender, e) => {

                if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.CLICK_KEY))
                {

                    getRandomFactAndGoNext();
                }

                };

                // Register for reading changes, be sure to unsubscribe when finished
                //.ShakeDetected += Accelerometer_ShakeDetected;



              setting_main_menu.Click += (sender, e) => {
                    // Perform action on click


                    resetMediaPlayer();

                 Intent intent = new Intent(this, typeof(SettingAct));
                StartActivity(intent);
                Finish();



            };


            lang_main_menu.Click += (sender, e) => {
                // Perform action on click

                resetMediaPlayer();

                Intent intent = new Intent(this, typeof(LanguageAct));
                StartActivity(intent);
                Finish();


            };



           



        }

        
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
              
                //AnimationDrawable animation = (AnimationDrawable)img_main_menu.Drawable;
                //animation.Start();
            }
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

        private void itemsDefinitions()
        {


            img_main_menu = FindViewById<ImageView>(Resource.Id.img_main_menu);
            setting_main_menu = FindViewById<ImageView>(Resource.Id.setting_main_menu);
            lang_main_menu = FindViewById<ImageView>(Resource.Id.lang_main_menu);


            shake_txt_tv_main_menu = FindViewById<TextView>(Resource.Id.shake_txt_tv_main_menu);
        }

        private void checkLanguage()
        {

            if (prefs.getLanguage(Constants.LANG_KEY).Equals("ru"))
            {
                shake_txt_tv_main_menu.SetText(Resource.String.shake_txt_ru);
            }
            else
            {
                shake_txt_tv_main_menu.SetText(Resource.String.shake_txt_eng);
            }

        }



        private void getRandomFactAndGoNext()
        {
            //CREATE ANIMATION BEFORE STARTING:
            AnimationDrawable animation = (AnimationDrawable)img_main_menu.Drawable;
            animation.Start();

            if (prefs.getSounds(Constants.SOUND_KEY).Equals("on"))
            {
                //STOP CURRENT MUSIC AND PLAY CLICK:
                resetMediaPlayer();

                initMediaPlayer(Resource.Raw.click);
                mediaPlayer.Start();

            }
            
            

            //GETTING A RANDOM FACT:
            Random rnd = new Random();

            int factNumb = rnd.Next(1, 14);


            //AVOID GETTING THE SAME FACT TWICE AGAIN:
            if (prefs.getLastFact(Constants.LAST_FACT_KEY) == factNumb)
            {
                 factNumb = rnd.Next(1, 14);
            }
            prefs.setLastFact(Constants.LAST_FACT_KEY, factNumb);



            //TODO: PLAY IF

            Action myAction = () =>
            {

                resetMediaPlayer();

                //CHECK LANGUAGE TO GO NEXT WITH FACT IN THE PREFS LANGUAGE:
                if (prefs.getLanguage(Constants.LANG_KEY).Equals("eng"))
                {
                    Intent intent = new Intent(this, typeof(ResultAct));
                    intent.PutExtra(Constants.FACT_TITLE_KEY, factsTitleEng[factNumb]);
                    intent.PutExtra(Constants.FACT_TXT_KEY, factsTxtEng[factNumb]);
                    intent.PutExtra(Constants.FACT_IMG_KEY, factsImgsEng[factNumb]);
                    StartActivity(intent);
                    Finish();
                }else if (prefs.getLanguage(Constants.LANG_KEY).Equals("ru"))
                {
                    Intent intent = new Intent(this, typeof(ResultAct));
                    intent.PutExtra(Constants.FACT_TITLE_KEY, factsTitleRu[factNumb]);
                    intent.PutExtra(Constants.FACT_TXT_KEY, factsTxtRu[factNumb]);
                    intent.PutExtra(Constants.FACT_IMG_KEY, factsImgsEng[factNumb]);
                    StartActivity(intent);
                    Finish();

                }

            };

            h.PostDelayed(myAction, 1000);





        }





        private void checkSounds()
        {

            if (prefs.getSounds(Constants.MUSIC_KEY).Equals("on"))
            {

               // mediaPlayer.Start();
            }

        }

        private void initFactsData()
        {
        

            factsTitleEng.Add(Resource.String.fact_1_title_eng);
            factsTitleEng.Add(Resource.String.fact_2_title_eng);
            factsTitleEng.Add(Resource.String.fact_3_title_eng);
            factsTitleEng.Add(Resource.String.fact_4_title_eng);
            factsTitleEng.Add(Resource.String.fact_5_title_eng);
            factsTitleEng.Add(Resource.String.fact_6_title_eng);
            factsTitleEng.Add(Resource.String.fact_7_title_eng);
            factsTitleEng.Add(Resource.String.fact_8_title_eng);
            factsTitleEng.Add(Resource.String.fact_9_title_eng);
            factsTitleEng.Add(Resource.String.fact_10_title_eng);
            factsTitleEng.Add(Resource.String.fact_11_title_eng);
            factsTitleEng.Add(Resource.String.fact_12_title_eng);
            factsTitleEng.Add(Resource.String.fact_13_title_eng);
            factsTitleEng.Add(Resource.String.fact_14_title_eng);



            factsTxtEng.Add(Resource.String.fact_1_txt_eng);
            factsTxtEng.Add(Resource.String.fact_2_txt_eng);
            factsTxtEng.Add(Resource.String.fact_3_txt_eng);
            factsTxtEng.Add(Resource.String.fact_4_txt_eng);
            factsTxtEng.Add(Resource.String.fact_5_txt_eng);
            factsTxtEng.Add(Resource.String.fact_6_txt_eng);
            factsTxtEng.Add(Resource.String.fact_7_txt_eng);
            factsTxtEng.Add(Resource.String.fact_8_txt_eng);
            factsTxtEng.Add(Resource.String.fact_9_txt_eng);
            factsTxtEng.Add(Resource.String.fact_10_txt_eng);
            factsTxtEng.Add(Resource.String.fact_11_txt_eng);
            factsTxtEng.Add(Resource.String.fact_12_txt_eng);
            factsTxtEng.Add(Resource.String.fact_13_txt_eng);
            factsTxtEng.Add(Resource.String.fact_14_txt_eng);



            factsTitleRu.Add(Resource.String.fact_1_title_ru);
            factsTitleRu.Add(Resource.String.fact_2_title_ru);
            factsTitleRu.Add(Resource.String.fact_3_title_ru);
            factsTitleRu.Add(Resource.String.fact_4_title_ru);
            factsTitleRu.Add(Resource.String.fact_5_title_ru);
            factsTitleRu.Add(Resource.String.fact_6_title_ru);
            factsTitleRu.Add(Resource.String.fact_7_title_ru);
            factsTitleRu.Add(Resource.String.fact_8_title_ru);
            factsTitleRu.Add(Resource.String.fact_9_title_ru);
            factsTitleRu.Add(Resource.String.fact_10_title_ru);
            factsTitleRu.Add(Resource.String.fact_11_title_ru);
            factsTitleRu.Add(Resource.String.fact_12_title_ru);
            factsTitleRu.Add(Resource.String.fact_13_title_ru);
            factsTitleRu.Add(Resource.String.fact_14_title_ru);





            factsTxtRu.Add(Resource.String.fact_1_txt_ru);
            factsTxtRu.Add(Resource.String.fact_2_txt_ru);
            factsTxtRu.Add(Resource.String.fact_3_txt_ru);
            factsTxtRu.Add(Resource.String.fact_4_txt_ru);
            factsTxtRu.Add(Resource.String.fact_5_txt_ru);
            factsTxtRu.Add(Resource.String.fact_6_txt_ru);
            factsTxtRu.Add(Resource.String.fact_7_txt_ru);
            factsTxtRu.Add(Resource.String.fact_8_txt_ru);
            factsTxtRu.Add(Resource.String.fact_9_txt_ru);
            factsTxtRu.Add(Resource.String.fact_10_txt_ru);
            factsTxtRu.Add(Resource.String.fact_11_txt_ru);
            factsTxtRu.Add(Resource.String.fact_12_txt_ru);
            factsTxtRu.Add(Resource.String.fact_13_txt_ru);
            factsTxtRu.Add(Resource.String.fact_14_txt_ru);





            factsImgsEng.Add(Resource.Drawable.result_1_bg);
            factsImgsEng.Add(Resource.Drawable.result_2_bg);
            factsImgsEng.Add(Resource.Drawable.result_3_bg);
            factsImgsEng.Add(Resource.Drawable.result_4_bg);
            factsImgsEng.Add(Resource.Drawable.result_5_bg);
            factsImgsEng.Add(Resource.Drawable.result_6_bg);
            factsImgsEng.Add(Resource.Drawable.result_7_bg);
            factsImgsEng.Add(Resource.Drawable.result_8_bg);
            factsImgsEng.Add(Resource.Drawable.result_9_bg);
            factsImgsEng.Add(Resource.Drawable.result_10_bg);
            factsImgsEng.Add(Resource.Drawable.result_11_bg);
            factsImgsEng.Add(Resource.Drawable.result_12_bg);
            factsImgsEng.Add(Resource.Drawable.result_13_bg);
            factsImgsEng.Add(Resource.Drawable.result_14_bg);


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

        private void sensorManage()
        {

            // Register this as a listener with the underlying service.
            //sensorManager = GetSystemService(SensorService) as Android.Hardware.SensorManager;
            //sensor = sensorManager.GetDefaultSensor(Android.Hardware.SensorType.Accelerometer);
            //sensorManager.RegisterListener(this, sensor, Android.Hardware.SensorDelay.Game);


            sensorManager = (SensorManager)GetSystemService(Context.SensorService);

        }

        void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Toast.MakeText(this, "ssssssssss", ToastLength.Long).Show();
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Toast.MakeText(this, "ssssssssss", ToastLength.Long).Show();
            }
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

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

            //throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {

            if (prefs.getControl(Constants.CONTROL_TYPE_KEY).Equals(Constants.SHAKE_KEY))
            {
                //MICROSOFT WAY:
                // Register for reading changes, be sure to unsubscribe when finished
                //Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;


                if (e.Sensor.Type == Android.Hardware.SensorType.Accelerometer)
                {

                    lock (_syncLock)//TO PREVENT READ SENSOR MORE THAN TIME
                    {


                        float x = e.Values[0];
                        float y = e.Values[1];
                        float z = e.Values[2];

                        long Current_Time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; ;
                        if ((Current_Time - last_Time) > 100)
                        {
                            long different = Current_Time - last_Time;
                            last_Time = Current_Time;
                            float speed = Math.Abs((x + y + z) - (last_x + last_y + last_z)) / different * 10000;

                            if (speed > ThreadShould)
                            {
                                //Toast.MakeText(this, "shake detected w/ speed: " + speed, ToastLength.Short).Show();


                                sensorManager.UnregisterListener(this);

                                Vibrator vibrator = (Vibrator)GetSystemService(Context.VibratorService);
                                vibrator.Vibrate(100);

                                getRandomFactAndGoNext();
                            }
                            last_x = x;
                            last_y = y;
                            last_z = z;
                        }


                        /*

                            DateTime curTime = System.DateTime.Now;
                    if (hasUpdated == false)
                    {
                        hasUpdated = true;
                        lastUpdate = curTime;
                        last_x = x;
                        last_y = y;
                        last_z = z;
                    }
                    else
                    {
                        if ((curTime - lastUpdate).TotalMilliseconds > ShakeDetectionTimeLapse)
                        {
                            float diffTime = (float)(curTime - lastUpdate).TotalMilliseconds;
                            lastUpdate = curTime;
                            float total = x + y + z - last_x - last_y - last_z;
                            float speed = Math.Abs(total) / diffTime * 10000;

                            if (speed > ShakeThreshold)
                            {
                                Toast.MakeText(this, "shake detected w/ speed: " + speed, ToastLength.Short).Show();




                                }

                            last_x = x;
                            last_y = y;
                            last_z = z;
                        }
                    }

                        */
                    }


                }

            }

            
        }

        protected override void OnResume()
        {
            base.OnResume();

            sensorManager.RegisterListener(this,
                sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                SensorDelay.Ui);
        }


        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }




        public override void OnBackPressed()
        {

            resetMediaPlayer();
            Finish();

        }


    }

    
}
using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;
using Android.Content;

namespace Timemasters.Droid
{
    [Activity(Label = "@string/MainActivityLabel", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> messages = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            //handle send event
            Button sendButton = FindViewById<Button>(Resource.Id.SendButton);
            sendButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MessageHistoryActivity));
                intent.PutStringArrayListExtra("messages", messages);
                StartActivity(intent);
            };

            EditText editText = FindViewById<EditText>(Resource.Id.EditText);

            /*
            editText.setOnClickListener(
                new View.OnClickListener()
                {
                    public void onClick(View view)
                {
                    messages.add ....
                }
            });
            */


        }
    }
}


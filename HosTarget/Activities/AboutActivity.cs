using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HosTarget.Activities
{
    [Activity(Label = "About")]
    public class AboutActivity : Activity
    {
        private ImageView imgAbout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.About);

            FindViews();

            HandleEvents();
        }

        private void FindViews()
        {
            imgAbout = FindViewById<ImageView>(Resource.Id.imgAbout);
        }

        private void HandleEvents()
        {
            imgAbout.Click += imgAbout_Click;
        }

        private void imgAbout_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionCall);
            intent.SetData(Android.Net.Uri.Parse("tel:00201220303335"));
            StartActivity(intent);
        }
    }
}
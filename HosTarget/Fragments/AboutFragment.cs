using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Android.Support.V4.App;
using Java.IO;

namespace HosTarget.Fragments
{
    public class AboutFragment : Fragment
    {
        private TextView txtEmail;
        private TextView txtMobile;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.About, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();
        }
        private void FindViews()
        {
            txtEmail = this.View.FindViewById<TextView>(Resource.Id.txtEmail);
            txtMobile = this.View.FindViewById<TextView>(Resource.Id.txtMobile);
        }

        private void HandleEvents()
        {
            txtEmail.Click += Email_Click;
            txtMobile.Click += Mobile_Click;
        }

        private void Email_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionSendto);
                intent.SetData(Android.Net.Uri.Parse("mailto:hossam.hassan@outlook.com"));

                intent.PutExtra(Intent.ExtraSubject, "HOS Target");

                intent.PutExtra(Intent.ExtraSubject, "[Enter your message here]");

                StartActivity(intent);
            }
            catch (Android.Content.ActivityNotFoundException ex)
            {
                Toast.MakeText(this.Context, "There are no email clients installed.", ToastLength.Short).Show();
            }
            catch (FileNotFoundException ex)
            {
                ex.PrintStackTrace();
            }
            catch (IOException ex)
            {
                ex.PrintStackTrace();
            }
        }

        private void Mobile_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionCall);
            intent.SetData(Android.Net.Uri.Parse("tel:00201220303335"));
            StartActivity(intent);
        }
    }
}
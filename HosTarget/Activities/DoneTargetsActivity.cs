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
    [Activity(Label = "DoneTargetsActivity")]
    public class DoneTargetsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            TextView textview = new TextView(this);
            textview.Text = "This is the Done Targets tab";

            this.SetContentView(textview);
        }
    }
}
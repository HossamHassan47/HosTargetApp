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

using HosTarget.DbContext;
using Android.Support.V4.App;

namespace HosTarget.Fragments
{
    public class TargetFragment : Fragment
    {
        public TargetItem targetItem { get; set; }

        TextView txtSubject;
        TextView txtDescription;
        TextView txtTargetDate;
        RadioGroup rbtngrpPriority;
        RadioGroup rbtngrpTargetState;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View rootView = inflater.Inflate(Resource.Layout.TargetFragment, container, false);
            
            rootView.FindViewById<TextView>(Resource.Id.txtSubject).Text = this.targetItem.Subject; ;
            rootView.FindViewById<TextView>(Resource.Id.txtDescription).Text = this.targetItem.Description; 
            rootView.FindViewById<TextView>(Resource.Id.txtTargetDate).Text = this.targetItem.TargetDate.ToShortDateString();
            rootView.FindViewById<TextView>(Resource.Id.txtPriority).Text = this.targetItem.Priority.ToString();

            rootView.FindViewById<TextView>(Resource.Id.txtState).Text = this.targetItem.State;

            return rootView;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}
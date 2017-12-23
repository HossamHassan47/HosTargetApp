using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HosTarget.DbContext;
using HosTarget.Activities;
using Android.Support.V4.App;

namespace HosTarget.Fragments
{
    public class TargetBaseFragment : Fragment
    {
        protected ListView listView;
        protected TargetDbRepository targetDbRepository;
        protected List<TargetItem> targets;

        public TargetBaseFragment()
        {
            targetDbRepository = new TargetDbRepository();
        }

        protected void FindViews()
        {
            listView = this.View.FindViewById<ListView>(Resource.Id.lstvwTargets);
        }

        protected void HandleEvents()
        {
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_LongClick;
        }

        protected void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = this.targets[e.Position];

            var addEditTarget = new Intent(this.View.Context, typeof(AddEditTargetActivity));

            addEditTarget.PutExtra("Title", "Edit Target");

            addEditTarget.PutExtra("targetId", t.Id);

            this.StartActivity(addEditTarget);
        }

        protected void ListView_LongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var t = this.targets[e.Position];

            var tasksIntent = new Intent(this.Activity, typeof(TasksTabActivity));

            tasksIntent.PutExtra("targetId", t.Id);
            tasksIntent.PutExtra("targetSubject", t.Subject);

            this.StartActivity(tasksIntent);
        }
    }
}
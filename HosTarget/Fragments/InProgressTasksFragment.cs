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
using HosTarget.Adapter;

namespace HosTarget.Fragments
{
    public class InProgressTasksFragment : TaskBaseFragment
    {
        public InProgressTasksFragment(int targetItemId) : base(targetItemId)
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.InProgressTasksFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnStart()
        {
            base.OnStart();

            this.BindTasksList();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();
            HandleEvents();

            this.BindTasksList();
        }

        private void BindTasksList()
        {
            tasks = targetDbRepository.GetTasksBy(targetItemId, TaskState.InProgress);

            listView.Adapter = new TaskAdapter(this.Activity, tasks);
        }
    }
}
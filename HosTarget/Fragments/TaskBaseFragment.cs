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
using HosTarget.Activities;
using Android.Support.V4.App;

namespace HosTarget.Fragments
{
    public class TaskBaseFragment : Fragment
    {
        protected ListView listView;
        protected TargetDbRepository targetDbRepository;
        protected List<TaskItem> tasks;
        protected int targetItemId;

        public TaskBaseFragment(int targetItemId)
        {
            targetDbRepository = new TargetDbRepository();
            this.targetItemId = targetItemId;
        }

        protected void FindViews()
        {
            listView = this.View.FindViewById<ListView>(Resource.Id.lstvwTasks);
        }

        protected void HandleEvents()
        {
            listView.ItemClick += ListView_ItemClick;
        }

        protected void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = this.tasks[e.Position];

            var addEditTask = new Intent(this.View.Context, typeof(AddEditTaskActivity));

            addEditTask.PutExtra("Title", "Edit Task");
            addEditTask.PutExtra("TaskId", t.Id);
            addEditTask.PutExtra("TargetItemId", t.TargetItemId);

            this.StartActivity(addEditTask);
        }
        
    }
}
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
    using Android.Graphics;

    [Activity(Label = "Tasks")]
    public class TasksMainActivity : TabActivity
    {
        private int TargetId;

        private string TargetSubject;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.Tasks);

            this.TargetId = this.Intent.GetIntExtra("targetId", -1);
            this.TargetSubject = this.Intent.GetStringExtra("targetSubject") ?? "";

            this.Title = "Tasks - " + this.TargetSubject;

            // Add Tabs
            // To-Do
            this.CreateTab(typeof(TasksTabActivity), "ToDo", "To-Do", Resource.Drawable.Grey);

            // In Progress
            this.CreateTab(typeof(TasksTabActivity), "InProgress", "In Progress", Resource.Drawable.Yellow);

            // Done 
            this.CreateTab(typeof(TasksTabActivity), "Done", "Done", Resource.Drawable.Green);
        }

        private void CreateTab(Type activityType, string tag, string label, int drawableId)
        {
            var intent = new Intent(this, activityType);

            intent.PutExtra("TaskState", tag);
            intent.PutExtra("TargetItemId", this.TargetId);

            intent.AddFlags(ActivityFlags.NewTask);

            var spec = this.TabHost.NewTabSpec(tag);

            var drawableIcon = this.Resources.GetDrawable(drawableId);

            spec.SetIndicator(label, drawableIcon);
            spec.SetContent(intent);

            this.TabHost.AddTab(spec);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.TasksOptionMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mnuAddNewTask:
                    var addEditTask = new Intent(this, typeof(AddEditTaskActivity));
                    addEditTask.PutExtra("Title", "Add New Task");
                    addEditTask.PutExtra("TargetItemId", this.TargetId);

                    this.StartActivity(addEditTask);
                    return true;
               
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            this.OpenMainActivity();
        }

        private void OpenMainActivity()
        {
            var intent = new Intent(this, typeof(HomeTabsActivity));
            intent.SetFlags(ActivityFlags.ReorderToFront);
            this.StartActivity(intent);
        }
    }
}
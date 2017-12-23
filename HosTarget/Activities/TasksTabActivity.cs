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
using HosTarget.Fragments;
using HosTarget.Adapter;
using Android.Support.V4.App;

namespace HosTarget.Activities
{
    [Activity(Label = "Targets", Icon = "@drawable/icon")]
    public class TasksTabActivity : FragmentActivity
    {
        private int TargetId;

        private string TargetSubject;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TaskTabsView);

            this.TargetId = this.Intent.GetIntExtra("targetId", -1);
            this.TargetSubject = this.Intent.GetStringExtra("targetSubject") ?? "";

            this.Title = "Tasks - " + this.TargetSubject;

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            
            this.SetTabs();
        }

        private void SetTabs()
        {
            var pager = FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.pager);

            pager.AddOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            // Create tabs on top to navigate
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Dashboard", Resource.Drawable.dashboard));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "New", Resource.Drawable.Grey));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "In Progress", Resource.Drawable.Yellow));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Done", Resource.Drawable.Green));
            
            // Add Tab Fragments
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            adaptor.AddFragment(new DashboardTasksFragment(this.TargetId));
            adaptor.AddFragment(new ToDoTasksFragment(this.TargetId));
            adaptor.AddFragment(new InProgressTasksFragment(this.TargetId));
            adaptor.AddFragment(new DoneTasksFragment(this.TargetId));

            // Set adapter
            pager.Adapter = adaptor;
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
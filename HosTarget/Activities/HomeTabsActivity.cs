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
    [Activity(Label = "Targets", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeTabsActivity : FragmentActivity
    {
        private ImageButton btnAddNewTarget;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // hide action bar
            ActionBar.SetDisplayShowTitleEnabled(false);
            
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            
            SetContentView(Resource.Layout.HomeTabsView);

            this.SetTabs();

            this.FindViews();
            this.HandleEvents();
        }

        private void FindViews()
        {
            btnAddNewTarget = this.FindViewById<ImageButton>(Resource.Id.btnAddNewTarget);
        }

        private void HandleEvents()
        {
            btnAddNewTarget.Click += BtnAddNewTarget_Click;
        }

        private void BtnAddNewTarget_Click(object sender, EventArgs e)
        {
            var addEditTarget = new Intent(this, typeof(AddEditTargetActivity));
            addEditTarget.PutExtra("Title", "Add New Target");

            this.StartActivity(addEditTarget);
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
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "About", Resource.Drawable.HOS_Logo));

            // Add Tab Fragments
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            adaptor.AddFragment(new DashboardFragment());
            adaptor.AddFragment(new NewTargetsFragment());
            adaptor.AddFragment(new InProgressTargetsFragment());
            adaptor.AddFragment(new DoneTargetsFragment());
            adaptor.AddFragment(new AboutFragment());

            // Set adapter
            pager.Adapter = adaptor;
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm");
            alert.SetMessage("Are you sure you want to exist HOS Targets?");

            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                //Process.KillProcess(Process.MyPid());
                this.FinishAffinity();
                //System.Environment.Exit(0);
            });

            alert.SetNegativeButton("No", (senderAlert, args) =>
            {
                //Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}
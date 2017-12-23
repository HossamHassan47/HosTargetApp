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
using Android.Support.V4.App;
using SQLite;
using System.IO;
using HosTarget.Adapter;
using Android.Support.V4.View;
using HosTarget.DbContext;
using Android.Support.V4.Widget;

namespace HosTarget.Activities.Target
{
    [Activity(Label = "HOS Targets", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/BlueTheme")]
    public class TargetSlideActivity : FragmentActivity
    {
        private string defaultState = "New";
        private object locker = new object();
        private SQLiteConnection db;

        TargetPagerAdapter targetPagerAdapter;
        ViewPager viewPager;
        DrawerLayout drawerLayout;
        ListView listViewTargetState;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.TargetSlideActivity);

            //drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawerLayout);


            // Initialze db connection
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
            this.db = new SQLiteConnection(dbPath);

            List<TargetItem> lstTargets = this.GetTargetList(this.defaultState);

            targetPagerAdapter = new TargetPagerAdapter(SupportFragmentManager, lstTargets);
            
            viewPager = this.FindViewById<ViewPager>(Resource.Id.targetPager);

            viewPager.Adapter = targetPagerAdapter;

            //drawerLayout = (DrawerLayout)FindViewById(Resource.Id.drawerLayout);
            
            listViewTargetState = this.FindViewById<ListView>(Resource.Id.listViewTargetState);
            listViewTargetState.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemActivated1, new string[] { "New", "In Progress", "Done" });

            listViewTargetState.SetItemChecked(0, true);
            listViewTargetState.ItemClick += ListViewTargetState_ItemClick;
        }

        private void ListViewTargetState_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            drawerLayout = listViewTargetState.Parent as DrawerLayout;
            drawerLayout.CloseDrawer(listViewTargetState);

            string state;
            switch(e.Position)
            {
                case 0:
                    state = "New";
                    break;
                case 1:
                    state = "InProgress";
                    break;
                case 2:
                    state = "Done";
                    break;
                default:
                    state = "New";
                    break;

            }

            List<TargetItem> lstTargets = this.GetTargetList(state);

            targetPagerAdapter.TargetItems = lstTargets;

            viewPager.CurrentItem = 0;
        }

        public List<TargetItem> GetTargetList(string state)
        {
            return this.db.Table<TargetItem>().Where(t=> t.State == state).OrderBy(t => t.Subject).ToList();
        }
    }
}
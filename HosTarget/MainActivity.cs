using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;

using HosTarget.DbContext;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using HosTarget.Adapter;
using Android.Database;

namespace HosTarget
{
    using Activities.Target;
    using Android.Graphics;

    using HosTarget.Activities;

    using Path = System.IO.Path;

    [Activity(Label = "HOS Targets", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/BlueTheme")]
    public class MainActivity : TabActivity
    {
        private object locker = new object();

        private SQLiteAsyncConnection db;

        private string sortby = "name";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);

            this.Title = "My Targets";

            try
            {
                // Initialze db connection
                var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
                lock (this.locker)
                {
                    this.db = new SQLiteAsyncConnection(dbPath);
                }

                // for 1st time only
                this.InitializeDb();

                //this.CreateTab(typeof(DashboardActivity), "Dashboard", "Dashboard", Resource.Drawable.Grey);

                // Add Target Tabs
                // New Targets
                this.CreateTab(typeof(NewTargetsActivity), "New_Targets", "New", Resource.Drawable.Grey);

                // In Progress Targets
                this.CreateTab(typeof(NewTargetsActivity), "InProgress_Targets", "In Progress", Resource.Drawable.Yellow);

                // Done Targets
                this.CreateTab(typeof(NewTargetsActivity), "Done_Targets", "Done", Resource.Drawable.Green);
                
            }
            catch (Exception ex)
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("OK", (senderAlert, args) => {
                    
                });
                
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        private void CreateTab(Type activityType, string tag, string label, int drawableId)
        {
            var intent = new Intent(this, activityType);

            intent.PutExtra("TargetState", label);

            intent.AddFlags(ActivityFlags.NewTask);

            var spec = this.TabHost.NewTabSpec(tag);

            var drawableIcon = this.Resources.GetDrawable(drawableId);

            spec.SetIndicator(label, drawableIcon);
            spec.SetContent(intent);

            this.TabHost.AddTab(spec);
            
        }

        public void InitializeDb()
        {
            lock (this.locker)
            {
                this.db.CreateTableAsync<TargetItem>().Wait();

                this.db.CreateTableAsync<TaskItem>().Wait();

                //this.db.InsertAsync(new TargetItem { Subject = "target 2" });
                //this.db.InsertAsync(new TargetItem { Subject = "target 3" });
                //this.db.InsertAsync(new TargetItem { Subject = "target 4" });
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.TargetOptionMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mnuAddNewTarget:
                    var addEditTarget = new Intent(this, typeof(AddEditTargetActivity));
                    addEditTarget.PutExtra("Title", "Add New Target");

                    this.StartActivity(addEditTarget);
                    return true;
                case Resource.Id.mnuDashboard:
                    var dashboard = new Intent(this, typeof(DashboardActivity));

                    this.StartActivity(dashboard);
                    return true;
                case Resource.Id.mnuAbout:
                    var about = new Intent(this, typeof(AboutActivity));
                    
                    this.StartActivity(about);
                    return true;
                case Resource.Id.mnuDetailedView:
                    var detailedView = new Intent(this, typeof(TargetSlideActivity));
                    this.StartActivity(detailedView);
                    return true;
                case Resource.Id.mnuTakePicture:
                    var takePictureIntent = new Intent(this, typeof(TakePictureActivity));
                    this.StartActivity(takePictureIntent);
                    return true;
                case Resource.Id.mnuSortByDate:
                    ((NewTargetsActivity)this.CurrentActivity).SortTargetsBy("date");
                    return true;
                case Resource.Id.mnuSortByPriority:
                    ((NewTargetsActivity)this.CurrentActivity).SortTargetsBy("priority");
                    return true;
                case Resource.Id.mnuSortByName:
                    ((NewTargetsActivity)this.CurrentActivity).SortTargetsBy("name");
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}


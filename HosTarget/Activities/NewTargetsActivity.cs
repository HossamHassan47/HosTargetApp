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
using Android.Database;

using HosTarget.DbContext;
using System.IO;
using System.Threading.Tasks;
using HosTarget.Adapter;
using SQLite;

namespace HosTarget.Activities
{
    using System.Collections.ObjectModel;

    [Activity(Label = "New")]
    public class NewTargetsActivity : Activity
    {
        #region Definitions

        private object locker = new object();

        private SQLiteAsyncConnection db;

        private ListView lstvwTargets;

        private List<TargetItem> targetItems = new List<TargetItem>();
        
        private string targetState;

        private string sortBy = "name";
        private string sortMode = "asc";
        #endregion

        #region Load
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.NewTargets);
            
            this.InitializeViews();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await this.RefreshList();
        }

        #endregion

        #region Helper Methods

        private void InitializeViews()
        {
            // Initialze db connection
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
            this.db = new SQLiteAsyncConnection(dbPath);

            // for 1st time only
            //this.InitializeDb();

            this.targetState = this.Intent.GetStringExtra("TargetState") ?? "New";

            // Target List View
            this.lstvwTargets = this.FindViewById<ListView>(Resource.Id.lstvwTargets);
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

        public async Task RefreshList()
        {
            var results = this.db.Table<TargetItem>().OrderBy(t => t.Subject);

            var itemsResult = await results.ToListAsync();

            this.targetItems = itemsResult.FindAll(t => t.State == this.targetState.Replace(" ", ""));
            
            if (this.sortBy == "priority")
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.Priority).ToList()
                    : this.targetItems.OrderBy(t => t.Priority).ToList();
            }
            else if (this.sortBy == "date")
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.TargetDate).ToList()
                    : this.targetItems.OrderBy(t => t.TargetDate).ToList();
            }
            else
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.Subject).ToList()
                    : this.targetItems.OrderBy(t => t.Subject).ToList();
            }

            this.lstvwTargets.Adapter = new TargetAdapter(this, this.targetItems);
            this.lstvwTargets.ItemClick += this.OnTargetItemClick;
            this.lstvwTargets.ItemLongClick += this.OnTargetItemLongClick;
        }

        public void SortTargetsBy(string sortby)
        {
            if (this.sortBy == sortby)
            {
                this.sortMode = this.sortMode == "asc" ? "desc" : "asc";
            }
            else
            {
                this.sortMode = "asc";
            }

            if (sortby == "priority")
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.Priority).ToList()
                    : this.targetItems.OrderBy(t => t.Priority).ToList();
            }
            else if (sortby == "date")
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.TargetDate).ToList()
                    : this.targetItems.OrderBy(t => t.TargetDate).ToList();
            }
            else
            {
                this.targetItems = this.sortMode == "desc"
                    ? this.targetItems.OrderByDescending(t => t.Subject).ToList()
                    : this.targetItems.OrderBy(t => t.Subject).ToList();
            }

            this.sortBy = sortby;

            this.lstvwTargets.Adapter = new TargetAdapter(this, this.targetItems);
            this.lstvwTargets.ItemClick += this.OnTargetItemClick;
            this.lstvwTargets.ItemLongClick += this.OnTargetItemLongClick;
        }
        #endregion

        #region Click Events

        void OnTargetItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = this.targetItems[e.Position];

            var addEditTarget = new Intent(this, typeof(AddEditTargetActivity));

            addEditTarget.PutExtra("Title", "Edit Target");

            addEditTarget.PutExtra("targetId", t.Id);

            this.StartActivity(addEditTarget);

            //Toast.MakeText(this, t.Subject, ToastLength.Short).Show();

            //PopupMenu menu = new PopupMenu(this, listView);
            //menu.MenuInflater.Inflate(Resource.Menu.target_popup_menu, menu.Menu);

            //menu.MenuItemClick += (s1, arg1) =>
            //{
            //    Toast.MakeText(this, string.Format("{0} selected", arg1.Item.TitleFormatted), ToastLength.Short).Show();

            //    //Console.WriteLine("{0} selected", arg1.Item.TitleFormatted);
            //};
            //menu.DismissEvent += (s2, arg2) => {
            //    Toast.MakeText(this, "menu dismissed", ToastLength.Short).Show();
            //    //Console.WriteLine("menu dismissed");
            //};

            //menu.Show();
        }

        void OnTargetItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var t = this.targetItems[e.Position];

            var tasksIntent = new Intent(this, typeof(TasksMainActivity));
            
            tasksIntent.PutExtra("targetId", t.Id);
            tasksIntent.PutExtra("targetSubject", t.Subject);

            this.StartActivity(tasksIntent);
        }

        #endregion

        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm");
            alert.SetMessage("Are you sure you want to exist HOS Targets?");

            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                //Process.KillProcess(Process.MyPid());
                this.Parent.FinishAffinity();
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
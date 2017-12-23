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
    using System.IO;

    using HosTarget.DbContext;

    using SQLite;

    [Activity(Label = "Add/Edit Target")]
    public class AddEditTargetActivity : Activity
    {
        private object locker = new object();

        private SQLiteAsyncConnection db;

        private int targetId;

        private TargetItem targetItem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.AddEditTarget);

            this.Title = this.Intent.GetStringExtra("Title") ?? "Add New Target";
            this.targetId = this.Intent.GetIntExtra("targetId", -1);

            // Initialze db connection
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
            this.db = new SQLiteAsyncConnection(dbPath);

            // Load for Edit
            this.LoadTargetForEdit(this.targetId);
        }

        private void LoadTargetForEdit(int targetId)
        {
            if (targetId <= 0)
            {
                return;
            }

            lock (this.locker)
            {
                this.targetItem = this.db.GetAsync<TargetItem>(targetId).Result;

                this.FindViewById<EditText>(Resource.Id.txtSubject).Text = this.targetItem.Subject;
                this.FindViewById<EditText>(Resource.Id.txtDescription).Text = this.targetItem.Description;
                this.FindViewById<EditText>(Resource.Id.txtTargetDate).Text = this.targetItem.TargetDate.ToShortDateString();

                switch ((int)this.targetItem.Priority)
                {
                    case 1:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpPriority).Check(Resource.Id.rbtnLow);
                        break;
                    case 2:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpPriority).Check(Resource.Id.rbtnMedium);
                        break;
                    case 3:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpPriority).Check(Resource.Id.rbtnHigh);
                        break;
                    default:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpPriority).Check(Resource.Id.rbtnLow);
                        break;
                }

                switch (this.targetItem.State)
                {
                    case "New":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTargetState).Check(Resource.Id.rbtnNew);
                        break;
                    case "InProgress":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTargetState).Check(Resource.Id.rbtnInProgress);
                        break;
                    case "Done":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTargetState).Check(Resource.Id.rbtnDone);
                        break;
                    default:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTargetState).Check(Resource.Id.rbtnNew);
                        break;
                }
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.AddEditOptionMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.mnuSave:
                    // Collect target data
                    var subject = this.FindViewById<EditText>(Resource.Id.txtSubject).Text;
                    var description = this.FindViewById<EditText>(Resource.Id.txtDescription).Text;

                    DateTime targetDate;
                    DateTime.TryParse(this.FindViewById<EditText>(Resource.Id.txtTargetDate).Text, out targetDate);

                    var priority = 1;
                    var rbtngPriority = this.FindViewById<RadioGroup>(Resource.Id.rbtngrpPriority);
                    switch (rbtngPriority.CheckedRadioButtonId)
                    {
                        case Resource.Id.rbtnLow:
                            priority = 1;
                            break;
                        case Resource.Id.rbtnMedium:
                            priority = 2;
                            break;
                        case Resource.Id.rbtnHigh:
                            priority = 3;
                            break;
                    }

                    var state = TargetState.New.ToString();
                    var rbtngTargetState = this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTargetState);
                    switch (rbtngTargetState.CheckedRadioButtonId)
                    {
                        case Resource.Id.rbtnNew:
                            state = TargetState.New.ToString();
                            break;
                        case Resource.Id.rbtnInProgress:
                            state = TargetState.InProgress.ToString();
                            break;
                        case Resource.Id.rbtnDone:
                            state = TargetState.Done.ToString();
                            break;
                    }

                    lock (this.locker)
                    {
                        if (this.targetId <= 0)
                        {
                            // Insert New
                            this.db.InsertAsync(
                                new TargetItem
                                {
                                    Subject = subject,
                                    Description = description,
                                    Priority = priority,
                                    State = state,
                                    TargetDate = targetDate
                                });
                        }
                        else
                        {
                            // Update
                            this.targetItem.Subject = subject;
                            this.targetItem.Description = description;
                            this.targetItem.TargetDate = targetDate;
                            this.targetItem.Priority = priority;
                            this.targetItem.State = state;

                            this.db.UpdateAsync(this.targetItem);
                        }

                    }

                    this.OpenMainActivity();
                    return true;
                case Resource.Id.mnuDelete:
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Confirm");
                    alert.SetMessage("Are you sure you want to delete this target?");
                    alert.SetPositiveButton("Delete", (senderAlert, args) =>
                    {
                        lock (this.locker)
                        {
                            if (this.targetItem != null)
                            {
                                this.db.DeleteAsync(this.targetItem);

                                this.db.ExecuteAsync("DELETE FROM TaskItem WHERE TargetItemId = " + this.targetId);
                            }
                        }

                        this.OpenMainActivity();
                    });

                    alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                    {
                        Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
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
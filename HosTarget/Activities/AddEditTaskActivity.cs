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

    [Activity(Label = "AddEditTaskActivity")]
    public class AddEditTaskActivity : Activity
    {
        private object locker = new object();

        private SQLiteAsyncConnection db;

        private int taskId;

        private int TargetItemId;

        private TaskItem taskItem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.AddEditTask);

            this.Title = this.Intent.GetStringExtra("Title") ?? "Add New Task";
            this.taskId = this.Intent.GetIntExtra("TaskId", -1);
            this.TargetItemId = this.Intent.GetIntExtra("TargetItemId", -1);

            // Initialze db connection
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
            this.db = new SQLiteAsyncConnection(dbPath);

            // Load for Edit
            this.LoadTaskForEdit(this.taskId);
        }

        private void LoadTaskForEdit(int taskId)
        {
            if (taskId <= 0)
            {
                return;
            }

            lock (this.locker)
            {
                this.taskItem = this.db.GetAsync<TaskItem>(taskId).Result;

                this.FindViewById<EditText>(Resource.Id.txtTaskTitle).Text = this.taskItem.Title;
                this.FindViewById<EditText>(Resource.Id.txtTaskDescription).Text = this.taskItem.Description;
                this.FindViewById<EditText>(Resource.Id.txtTaskRemaining).Text = this.taskItem.Remaining.ToString();

                switch (this.taskItem.State)
                {
                    case "ToDo":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTaskState).Check(Resource.Id.rbtnTaskToDo);
                        break;
                    case "InProgress":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTaskState).Check(Resource.Id.rbtnTaskInProgress);
                        break;
                    case "Done":
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTaskState).Check(Resource.Id.rbtnTaskDone);
                        break;
                    default:
                        this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTaskState).Check(Resource.Id.rbtnTaskToDo);
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
                    var mainIntentSave = new Intent(this, typeof(TasksTabActivity)).SetFlags(ActivityFlags.ReorderToFront);
                    
                    // Collect target data
                    var title = this.FindViewById<EditText>(Resource.Id.txtTaskTitle).Text;
                    var description = this.FindViewById<EditText>(Resource.Id.txtTaskDescription).Text;

                    decimal remaining;
                    Decimal.TryParse(this.FindViewById<EditText>(Resource.Id.txtTaskRemaining).Text, out remaining);

                    var state = TargetState.New.ToString();
                    var rbtngrpTaskState = this.FindViewById<RadioGroup>(Resource.Id.rbtngrpTaskState);
                    switch (rbtngrpTaskState.CheckedRadioButtonId)
                    {
                        case Resource.Id.rbtnTaskToDo:
                            state = TaskState.ToDo.ToString();
                            break;
                        case Resource.Id.rbtnTaskInProgress:
                            state = TaskState.InProgress.ToString();
                            break;
                        case Resource.Id.rbtnTaskDone:
                            state = TaskState.Done.ToString();
                            break;
                    }

                    lock (this.locker)
                    {
                        if (this.taskId <= 0)
                        {
                            // Insert New
                            this.db.InsertAsync(
                                new TaskItem
                                {
                                    Title = title,
                                    Description = description,
                                    Remaining = remaining,
                                    State = state,
                                    TargetItemId = this.TargetItemId
                                });
                        }
                        else
                        {
                            // Update
                            this.taskItem.Title = title;
                            this.taskItem.Description = description;
                            this.taskItem.Remaining = remaining;
                            this.taskItem.State = state;

                            this.db.UpdateAsync(this.taskItem);
                        }

                    }

                    this.StartActivity(mainIntentSave);
                    return true;
                case Resource.Id.mnuDelete:
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);

                    alert.SetTitle("Confirm");
                    alert.SetMessage("Are you sure you want to delete this task?");
                    alert.SetPositiveButton("Delete", (senderAlert, args) =>
                    {
                        var tasksMainIntent = new Intent(this, typeof(TasksTabActivity)).SetFlags(ActivityFlags.ReorderToFront);

                        lock (this.locker)
                        {
                            if (this.taskItem != null)
                            {
                                this.db.DeleteAsync(this.taskItem);
                            }
                        }

                        this.StartActivity(tasksMainIntent);
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
            var tasksMainIntentBack = new Intent(this, typeof(TasksTabActivity)).SetFlags(ActivityFlags.ReorderToFront);

            this.StartActivity(tasksMainIntentBack);
        }
    }
}
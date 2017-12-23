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

    using BarChart;

    using HosTarget.DbContext;

    using SQLite;

    using Path = System.IO.Path;

    [Activity(Label = "My Targets - Dashboard", MainLauncher = false)]
    public class DashboardActivity : Activity
    {
        private object locker = new object();
        private SQLiteAsyncConnection db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.Dashboard);

            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");
            lock (this.locker)
            {
                this.db = new SQLiteAsyncConnection(dbPath);
            }

            // Targets
            var results = this.db.Table<TargetItem>();

            var itemsResult = results.ToListAsync().Result;

            var newCount = itemsResult.Count(t => t.State.ToLower() == "new");
            var inprogressCount = itemsResult.Count(t => t.State.ToLower() == "inprogress");
            var doneCount = itemsResult.Count(t => t.State.ToLower() == "done");
            
            var lstBarModels = new List<BarModel>
                               {
                                   new BarModel
                                   {
                                       Value = newCount,
                                       Color = Color.WhiteSmoke,
                                       Legend = "New",
                                       ValueCaptionHidden = false,
                                       ValueCaption = newCount.ToString()
                                   },
                                   new BarModel
                                   {
                                       Value = inprogressCount,
                                       Color = Color.Yellow,
                                       Legend = "In Progress",
                                       ValueCaptionHidden = false,
                                       ValueCaption = inprogressCount.ToString()
                                   },
                                   new BarModel
                                   {
                                       Value = doneCount,
                                       Color = Color.GreenYellow,
                                       Legend = "Done",
                                       ValueCaptionHidden = false,
                                       ValueCaption = doneCount.ToString()
                                   }
                               };

            var barChart = this.FindViewById<BarChartView>(Resource.Id.barChartTargets);
            barChart.ItemsSource = lstBarModels;
            barChart.LegendColor = Color.Black;


            // tasks
            var tasks = this.db.Table<TaskItem>();

            var tasksResult = tasks.ToListAsync().Result;

            var todoCount = tasksResult.Count(t => t.State.ToLower() == "todo");
            var inprogCount = tasksResult.Count(t => t.State.ToLower() == "inprogress");
            var doneTaskCount = tasksResult.Count(t => t.State.ToLower() == "done");

            var lstBarModelsTasks = new List<BarModel>
                               {
                                   new BarModel
                                   {
                                       Value = todoCount,
                                       Color = Color.WhiteSmoke,
                                       Legend = "To-Do",
                                       ValueCaptionHidden = false,
                                       ValueCaption = todoCount.ToString()
                                   },
                                   new BarModel
                                   {
                                       Value = inprogCount,
                                       Color = Color.Yellow,
                                       Legend = "In Progress",
                                       ValueCaptionHidden = false,
                                       ValueCaption = inprogCount.ToString()
                                   },
                                   new BarModel
                                   {
                                       Value = doneTaskCount,
                                       Color = Color.GreenYellow,
                                       Legend = "Done",
                                       ValueCaptionHidden = false,
                                       ValueCaption = doneTaskCount.ToString()
                                   }
                               };

            var barChartTasks = this.FindViewById<BarChartView>(Resource.Id.barChartTasks);
            barChartTasks.ItemsSource = lstBarModelsTasks;
            barChartTasks.LegendColor = Color.Black;

        }
    }
}
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

using Android.Graphics;
using BarChart;
using HosTarget.DbContext;
using Android.Support.V4.App;

namespace HosTarget.Fragments
{
    public class DashboardFragment : Fragment
    {
        TargetDbRepository targetDbRepository = new TargetDbRepository();
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            this.SetTargetsChart();

            this.SetTasksChart();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            return inflater.Inflate(Resource.Layout.Dashboard, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnStart()
        {
            base.OnStart();

            this.SetTargetsChart();

            this.SetTasksChart();
        }


        private void SetTargetsChart()
        {
            // Targets
            var allTargets = targetDbRepository.GetAllTargets();

            var newCount = allTargets.Count(t => t.State.ToLower() == "new");
            var inprogressCount = allTargets.Count(t => t.State.ToLower() == "inprogress");
            var doneCount = allTargets.Count(t => t.State.ToLower() == "done");

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

            var barChart = this.View.FindViewById<BarChartView>(Resource.Id.barChartTargets);
            barChart.ItemsSource = lstBarModels;
            //barChart.LegendColor = Color.Black;
        }

        private void SetTasksChart()
        {
            // tasks
            var allTasks = targetDbRepository.GetAllTasks();

            var todoCount = allTasks.Count(t => t.State.ToLower() == "todo");
            var inprogCount = allTasks.Count(t => t.State.ToLower() == "inprogress");
            var doneTaskCount = allTasks.Count(t => t.State.ToLower() == "done");

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

            var barChartTasks = this.View.FindViewById<BarChartView>(Resource.Id.barChartTasks);
            barChartTasks.ItemsSource = lstBarModelsTasks;
            //barChartTasks.LegendColor = Color.Black;
        }
    }
}
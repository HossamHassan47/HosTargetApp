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
    public class DashboardTasksFragment : Fragment
    {
        TargetDbRepository targetDbRepository = new TargetDbRepository();
        int targetItemId;

        public DashboardTasksFragment(int targetItemId)
        {
            targetDbRepository = new TargetDbRepository();

            this.targetItemId = targetItemId;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.DashboardTasks, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            
            this.SetTasksChart();
        }

        public override void OnStart()
        {
            base.OnStart();
            
            this.SetTasksChart();

        }

        private void SetTasksChart()
        {
            // tasks
            var allTasks = targetDbRepository.GetAllTasks();

            var todoCount = targetDbRepository.GetTasksBy(this.targetItemId, TaskState.ToDo).Count;
            var inprogCount = targetDbRepository.GetTasksBy(this.targetItemId, TaskState.InProgress).Count;
            var doneTaskCount = targetDbRepository.GetTasksBy(this.targetItemId, TaskState.Done).Count;

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
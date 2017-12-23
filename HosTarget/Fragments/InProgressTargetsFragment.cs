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
using HosTarget.DbContext;
using HosTarget.Adapter;

namespace HosTarget.Fragments
{
    public class InProgressTargetsFragment : TargetBaseFragment
    {
        private string sortBy = "name";
        private string sortMode = "asc";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.InProgressTargetsFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();

            HandleEvents();

            this.BindTargetsList();
        }

        public override void OnStart()
        {
            base.OnStart();

            this.BindTargetsList();
        }

        private void BindTargetsList()
        {
            targets = targetDbRepository.GetTargetsByState(TargetState.InProgress);

            if (this.sortBy == "priority")
            {
                targets = this.sortMode == "desc"
                    ? targets.OrderByDescending(t => t.Priority).ToList()
                    : targets.OrderBy(t => t.Priority).ToList();
            }
            else if (this.sortBy == "date")
            {
                targets = this.sortMode == "desc"
                    ? targets.OrderByDescending(t => t.TargetDate).ToList()
                    : targets.OrderBy(t => t.TargetDate).ToList();
            }
            else
            {
                targets = this.sortMode == "desc"
                    ? targets.OrderByDescending(t => t.Subject).ToList()
                    : targets.OrderBy(t => t.Subject).ToList();
            }

            listView.Adapter = new TargetAdapter(this.Activity, targets);
        }
    }
}
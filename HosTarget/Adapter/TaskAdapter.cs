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

namespace HosTarget.Adapter
{
    using HosTarget.DbContext;

    public class TaskAdapter : BaseAdapter<TaskItem>
    {
        List<TaskItem> items;
        Activity context;

        public TaskAdapter(Activity context, List<TaskItem> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TaskItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return items[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            View view = convertView;

            if (view == null) // no view to re-use, create new
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TaskView, null);
            }

            view.FindViewById<TextView>(Resource.Id.txtTaskTitle).Text = item.Title.Length < 22
                ? item.Title
                : item.Title.Substring(0, 21) + "...";

            view.FindViewById<TextView>(Resource.Id.txtTaskDescription).Text = item.Description == null
                ? ""
                : (item.Description.Length < 31 ? item.Description : item.Description.Substring(0, 30) + "...");

            view.FindViewById<TextView>(Resource.Id.txtTaskRemaining).Text = item.Remaining.ToString();

            switch (item.State)
            {
                case "ToDo":
                    view.FindViewById<ImageView>(Resource.Id.imgTaskStatus).SetImageResource(Resource.Drawable.Grey);
                    break;
                case "InProgress":
                    view.FindViewById<ImageView>(Resource.Id.imgTaskStatus).SetImageResource(Resource.Drawable.Yellow);
                    break;
                case "Done":
                    view.FindViewById<ImageView>(Resource.Id.imgTaskStatus).SetImageResource(Resource.Drawable.Green);
                    break;
                default:
                    view.FindViewById<ImageView>(Resource.Id.imgTaskStatus).SetImageResource(Resource.Drawable.Grey);
                    break;
            }
            
            return view;
        }
    }
}
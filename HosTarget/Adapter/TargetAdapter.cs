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
using HosTarget.DbContext;

namespace HosTarget.Adapter
{
    public class TargetAdapter : BaseAdapter<TargetItem>
    {
        List<TargetItem> items;
        Activity context;

        public TargetAdapter(Activity context, List<TargetItem> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TargetItem this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.TargetView, null);
            }

            view.FindViewById<TextView>(Resource.Id.txtTargetSubject).Text = item.Subject.Length < 22
                ? item.Subject
                : item.Subject.Substring(0, 21) + "...";

            view.FindViewById<TextView>(Resource.Id.txtDescription).Text = item.Description == null
                ? ""
                : (item.Description.Length < 31 ? item.Description : item.Description.Substring(0, 30) + "...");

            view.FindViewById<TextView>(Resource.Id.txtTargetDate).Text = item.TargetDate.ToShortDateString();

            switch(item.State)
            {
                case "New":
                    view.FindViewById<ImageView>(Resource.Id.imgTargetStatus).SetImageResource(Resource.Drawable.Grey);
                    break;
                case "InProgress":
                    view.FindViewById<ImageView>(Resource.Id.imgTargetStatus).SetImageResource(Resource.Drawable.Yellow);
                    break;
                case "Done":
                    view.FindViewById<ImageView>(Resource.Id.imgTargetStatus).SetImageResource(Resource.Drawable.Green);
                    break;
                default:
                    view.FindViewById<ImageView>(Resource.Id.imgTargetStatus).SetImageResource(Resource.Drawable.Grey);
                    break;
            }

            switch ((int)item.Priority)
            {
                case 1:
                    view.FindViewById<ImageView>(Resource.Id.imgPriority).SetImageResource(Resource.Drawable.low);
                    break;
                case 2:
                    view.FindViewById<ImageView>(Resource.Id.imgPriority).SetImageResource(Resource.Drawable.medium);
                    break;
                case 3:
                    view.FindViewById<ImageView>(Resource.Id.imgPriority).SetImageResource(Resource.Drawable.high);
                    break;
                default:
                    view.FindViewById<ImageView>(Resource.Id.imgPriority).SetImageResource(Resource.Drawable.low);
                    break;
            }

            return view;
        }
    }
}
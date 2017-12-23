using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HosTarget.Adapter
{
    public class TargetCursorAdapter : CursorAdapter
    {
        Activity context;

        public TargetCursorAdapter(Activity context, ICursor c, bool autoRequery) : base(context, c, autoRequery)
        {
            this.context = context;
        }

        public override void BindView(View view, Context context, ICursor cursor)
        {
            var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.Text = cursor.GetString(1); // 'name' is column 1 in the cursor query

            //var txtTargetSubject = view.FindViewById<TextView>(Resource.Id.txtTargetSubject);
            //var txtDescription = view.FindViewById<TextView>(Resource.Id.txtDescription);
            //var imgTargetStatus = view.FindViewById<ImageView>(Resource.Id.imgTargetStatus);

            //var subject = cursor.GetString(1); // Subject
            //var description = cursor.GetString(2); // Description
            //var state = cursor.GetString(4); // State

            //txtTargetSubject.Text = subject;
            //txtDescription.Text = description;
            
            //switch (state)
            //{
            //    case "New":
            //        imgTargetStatus.SetImageResource(Resource.Drawable.Grey);
            //        break;
            //    case "InProgress":
            //        imgTargetStatus.SetImageResource(Resource.Drawable.Yellow);
            //        break;
            //    case "Done":
            //        imgTargetStatus.SetImageResource(Resource.Drawable.Green);
            //        break;
            //    default:
            //        imgTargetStatus.SetImageResource(Resource.Drawable.Grey);
            //        break;
            //}
        }

        public override View NewView(Context context, ICursor cursor, ViewGroup parent)
        {
            return this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, parent, false);
        }
    }
}
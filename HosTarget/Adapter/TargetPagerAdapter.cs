using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using HosTarget.DbContext;
using HosTarget.Fragments;
using Java.Lang;

namespace HosTarget.Adapter
{
    class TargetPagerAdapter : FragmentStatePagerAdapter
    {
        List<TargetItem> targetItems;
        public List<TargetItem> TargetItems
        {
            set
            {
                this.targetItems = value;
                NotifyDataSetChanged();
            }
        }

        public TargetPagerAdapter(FragmentManager fm, List<TargetItem> targetItems) : base(fm)
        {
            this.targetItems = targetItems;
        }

        public override int Count
        {
            get
            {
                return targetItems.Count;
            }
        }

        public override Fragment GetItem(int position)
        {
            TargetFragment targetFragment = new TargetFragment();
            targetFragment.targetItem = this.targetItems[position];

            return targetFragment;
        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }
    }
}
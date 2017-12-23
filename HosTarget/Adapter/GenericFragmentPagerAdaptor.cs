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

namespace HosTarget.Adapter
{
    public class GenericFragmentPagerAdaptor : FragmentPagerAdapter
    {
        private IList<Fragment> _fragmentList = new List<Fragment>();

        public GenericFragmentPagerAdaptor(FragmentManager fm)
            : base(fm) { }

        public override int Count
        {
            get { return _fragmentList.Count; }
        }

        public override Fragment GetItem(int position)
        {
            return _fragmentList[position];
        }

        public void AddFragment(Fragment fragment)
        {
            _fragmentList.Add(fragment);
        }
    }
}
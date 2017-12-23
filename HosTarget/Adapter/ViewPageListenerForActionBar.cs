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
using Android.Support.V4.View;

namespace HosTarget.Adapter
{
    public class ViewPageListenerForActionBar : ViewPager.SimpleOnPageChangeListener
    {
        private ActionBar _bar;

        public ViewPageListenerForActionBar(ActionBar bar)
        {
            _bar = bar;
        }

        public override void OnPageSelected(int position)
        {
            _bar.SetSelectedNavigationItem(position);
        }
    }

    public static class ViewPagerExtensions
    {
        public static ActionBar.Tab GetViewPageTab(this ViewPager viewPager, ActionBar actionBar, string text, int icon)
        {
            var tab = actionBar.NewTab();

            // remove this line if don't want to use text
            tab.SetText(text);
            
            // remove this line if don't want to use icon
            tab.SetIcon(icon);

            tab.TabSelected += (o, e) =>
            {
                viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, false);
            };

            return tab;
        }
    }
}
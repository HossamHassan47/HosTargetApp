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
using SQLite;
using System.IO;

namespace HosTarget.DbContext
{
    public class TargetDbRepository
    {
        private object locker = new object();
        private SQLiteConnection db;

        public TargetDbRepository()
        {
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "HosTarget.db3");

            lock (this.locker)
            {
                this.db = new SQLiteConnection(dbPath);

                this.db.CreateTable<TargetItem>();

                this.db.CreateTable<TaskItem>();
            }
        }
        
        public List<TargetItem> GetAllTargets()
        {
            lock (this.locker)
            {
                return this.db.Table<TargetItem>().ToList();
            }
        }

        public List<TargetItem> GetTargetsByState(TargetState state)
        {
            lock (this.locker)
            {
                var strState = state.ToString();
                return this.db.Table<TargetItem>().Where(t => t.State == strState).ToList();
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            lock (this.locker)
            {
                return this.db.Table<TaskItem>().ToList();
            }
        }

        public List<TaskItem> GetTasksBy(int targetItemId, TaskState state)
        {
            lock (this.locker)
            {
                var strState = state.ToString();

                return this.db.Table<TaskItem>()
                    .Where(t => t.TargetItemId == targetItemId && t.State == strState)
                    .OrderBy(t => t.Title)
                    .ToList();
            }
        }
    }
}
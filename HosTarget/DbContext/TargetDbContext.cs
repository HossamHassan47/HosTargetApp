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
using Android.Database.Sqlite;

namespace HosTarget.DbContext
{
    public class TargetDbContext: SQLiteOpenHelper
    {
        #region Definitions

        public static readonly string create_table_sql_target =
           @"CREATE TABLE [TargetItem] (
                [_id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, 
                [Subject] TEXT NOT NULL UNIQUE, 
                [Description] TEXT, 
                [TargetDate] DATE, 
                [State] TEXT, 
                [Priority] TEXT 
            )";

        public static readonly string DatabaseName = "HosTarget.db";

        public static readonly int DatabaseVersion = 1;

        private object locker = new object();

        private readonly SQLiteAsyncConnection db;

        #endregion

        public TargetDbContext(Context context) : base(context, DatabaseName, null, DatabaseVersion)
        {
            //var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), 
            //    "HosTarget.db3");

            //db = new SQLiteAsyncConnection(dbPath);
        }

        public void InitializeDb()
        {
            lock (locker)
            {
                db.CreateTableAsync<TargetItem>().Wait();

                db.CreateTableAsync<TaskItem>().Wait();
            }
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(create_table_sql_target);

            // seed with data
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#1')");
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#2')");
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#3')");
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#4')");
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#5')");
            db.ExecSQL("INSERT INTO TargetItem (Subject) VALUES ('Target#6')");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            // not required until second version :)
            //throw new NotImplementedException();
        }
    }

    [Table("TargetItem")]
    public class TargetItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(50), Unique]
        public string Subject { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public DateTime TargetDate { get; set; }

        [MaxLength(20)]
        public string State { get; set; }

        public decimal Priority { get; set; }
    }

    [Table("TaskItem")]
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }
        
        [MaxLength(20)]
        public string State { get; set; }
        
        public decimal Remaining { get; set; }
        
        public int TargetItemId { get; set; }
    }

    public enum TargetState
    {
        New,
        InProgress,
        Done
    }

    public enum TaskState
    {
        ToDo,
        InProgress,
        Done
    }
    
}
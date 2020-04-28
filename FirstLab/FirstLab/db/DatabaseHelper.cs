using System;
using System.IO;
using FirstLab.entities;
using SQLite;

namespace FirstLab.db
{
    public class DatabaseHelper
    {
        public SQLiteConnection Connection { get; }

        public DatabaseHelper()
        {
            Connection = DbInit();
        }

        private SQLiteConnection DbInit()
        {
            var connection = new SQLiteConnection(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"),
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex
            );
            connection.CreateTable<InstallationEntity>();
            connection.CreateTable<CurrentEntity>();
            connection.CreateTable<ValueEntity>();
            connection.CreateTable<StandardEntity>();
            connection.CreateTable<IndexEntity>();
            return connection;
        }
    }
}
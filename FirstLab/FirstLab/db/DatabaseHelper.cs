using System;
using System.Collections.Generic;
using System.IO;
using FirstLab.entities;
using FirstLab.network.models;
using LaYumba.Functional;
using SQLite;
using SQLiteNetExtensions.Extensions;
using InstallationsReplacingFunc =
    System.Func<System.Collections.Generic.List<FirstLab.network.models.Installation>, LaYumba.Functional.Either<
        LaYumba.Functional.Error, System.Collections.Generic.List<FirstLab.network.models.Installation>>>;


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
            CreateTables(connection);
            return connection;
        }

        public static void CreateTables(SQLiteConnection connection)
        {
            connection.CreateTable<InstallationEntity>();
            connection.CreateTable<CurrentEntity>();
            connection.CreateTable<ValueEntity>();
            connection.CreateTable<StandardEntity>();
            connection.CreateTable<IndexEntity>();
        }

        public static Func<SQLiteConnection, InstallationsReplacingFunc>
            ReplaceInstallations => connection => list =>
        {
            try
            {
                connection.RunInTransaction(() =>
                {
                    connection.DeleteAll<InstallationEntity>();
                    connection.DeleteAll<StandardEntity>();
                    connection.DeleteAll<IndexEntity>();
                    connection.DeleteAll<ValueEntity>();
                    connection.InsertAllWithChildren(list.Map(
                        it => it.ToInstallationEntity(new List<Current>())), recursive: true);
                });

                return list;
            }
            catch (SQLiteException e)
            {
                return new SqlError(e.Message);
            }
        };

        public static Either<Error, List<Installation>> ReplaceInstallations2(List<Installation> installations) =>
            ReplaceInstallations(App.Database.Connection)(installations);
    }

    public sealed class SqlError : Error
    {
        public SqlError(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
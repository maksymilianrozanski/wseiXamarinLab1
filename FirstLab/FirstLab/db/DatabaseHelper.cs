using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    connection.DeleteAll<CurrentEntity>();
                    connection.InsertAllWithChildren(list.Map(
                        it => it.ToInstallationEntity()), recursive: true);
                });

                return list;
            }
            catch (SQLiteException e)
            {
                return new SqlError(e.Message);
            }
        };

        public static InstallationsReplacingFunc ReplaceInstallations2 => ReplaceInstallations(App.Database.Connection);

        public static Func<SQLiteConnection,
            Func<(Measurements, Installation), Either<Error, (Measurements, Installation)>>> ReplaceCurrent =>
            connection =>
                pair =>
                {
                    try
                    {
                        connection.RunInTransaction(() =>
                        {
                            var currentEntity = pair.Item1.current.ToCurrentEntity();
                            var installationEntity = connection.Get<InstallationEntity>(pair.Item2.id);

                            connection.Table<CurrentEntity>()
                                .Where(entity => entity.InstallationId == pair.Item2.id)
                                .Delete(entity => true);

                            connection.InsertWithChildren(currentEntity, true);
                            installationEntity.CurrentEntity = currentEntity;
                            connection.UpdateWithChildren(installationEntity);
                        });
                        return pair;
                    }
                    catch (SQLiteException e)
                    {
                        return new SqlError(e.Message);
                    }
                };

        public static Either<Error, (Measurements, Installation)> ReplaceCurrent2(
            (Measurements, Installation) measurementInstallation)
            => ReplaceCurrent(App.Database.Connection)(measurementInstallation);

        public static Func<SQLiteConnection,
            Func<Either<Error, List<InstallationEntity>>>> LoadInstallationEntities =>
            connection => () =>
            {
                try
                {
                    return connection.GetAllWithChildren<InstallationEntity>(entity => true, true);
                }
                catch (SQLiteException e)
                {
                    return new SqlError(e.Message);
                }
            };

        public static Func<Either<Error, List<InstallationEntity>>> LoadInstallationEntities2 =>
            LoadInstallationEntities(App.Database.Connection);

        public static Func<SQLiteConnection,
            Func<int, Either<Error, Option<CurrentEntity>>>> LoadMeasurementByInstallationId =>
            connection => installationId =>
            {
                try
                {
                    return (Option<CurrentEntity>) connection.GetAllWithChildren<CurrentEntity>(
                            it => it.InstallationId == installationId, true)
                        .FirstOrDefault();
                }
                catch (SQLiteException e)
                {
                    return new SqlError(e.Message);
                }
            };

        public static Func<int, Either<Error, Option<CurrentEntity>>> LoadMeasurementByInstallationId2 =>
            LoadMeasurementByInstallationId(App.Database.Connection);


        public sealed class SqlError : Error
        {
            public SqlError(string message)
            {
                Message = message;
            }

            public override string Message { get; }
        }
    }
}
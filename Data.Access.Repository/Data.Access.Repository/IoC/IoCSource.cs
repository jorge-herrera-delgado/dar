using System.Collections.Generic;
using Data.Access.Repository.LegacyDataBase.MySqlDb;
using Data.Access.Repository.LegacyDataBase.OracleDB;
using Data.Access.Repository.LegacyDataBase.SqlDb;
using Data.Access.Repository.LegacyFile.JsonFile;
using Data.Access.Repository.LegacyNonSql.MongoDB;
using Data.Access.Repository.SourceStorage.Engine;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Data.Access.Repository.IoC
{
    public static class IoCSource
    {
        private static readonly Container RegContainer;
        private static bool _wasRegistered;

        static IoCSource()
        {
            RegContainer = new Container();
            _wasRegistered = false;
        }

        /// <summary>
        /// Method to register assemblies.
        /// </summary>
        public static void Register()
        {
            if (_wasRegistered) return;

            RegContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assemblies = new[] { typeof(ISourceStorage<>).Assembly };

            RegContainer.Register<MySqlDataBase>(Lifestyle.Singleton);
            RegContainer.Register<SqlDataBase>(Lifestyle.Singleton);
            RegContainer.Register<OracleDataBase>(Lifestyle.Singleton);
            RegContainer.Register<JsonFileBase>(Lifestyle.Singleton);
            RegContainer.Register<MongoDataBase>(Lifestyle.Singleton);

            RegContainer.Collection.Register(typeof(ISourceStorage<>), assemblies);

            RegContainer.Verify();
            _wasRegistered = true;
        }

        /// <summary>
        /// Method to get an instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T IoCGetInstance<T>() where T : class => RegContainer.GetInstance<T>();

        /// <summary>
        /// Method to get all instances by type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> IoCGetAllInstances<T>() where T : class => RegContainer.GetAllInstances<T>();
    }
}

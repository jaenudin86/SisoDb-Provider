using System;
using System.Data.SqlServerCe;
using SisoDb.Core;
using SisoDb.Core.Io;
using SisoDb.Providers.DbSchema;
using SisoDb.Providers.SqlCe4.Resources;
using SisoDb.Structures;
using SisoDb.Structures.Schemas;

namespace SisoDb.Providers.SqlCe4
{
    public class SqlCe4Database : ISisoDatabase
    {
        private readonly SqlCe4ConnectionInfo _innerConnectionInfo;

        public string Name 
        {
            get { return _innerConnectionInfo.Name; }
        }

        public string FilePath
        {
            get { return _innerConnectionInfo.FilePath; }
        }

        public ISisoConnectionInfo ConnectionInfo
        {
            get { return _innerConnectionInfo; }
        }

        public IStructureSchemas StructureSchemas { get; set; }

        public IStructureBuilder StructureBuilder { get; set; }

        public IDbSchemaManager DbSchemaManager { get; set; }

        internal SqlCe4Database(ISisoConnectionInfo connectionInfo)
        {
            _innerConnectionInfo = new SqlCe4ConnectionInfo(connectionInfo.AssertNotNull("connectionInfo"));

            StructureSchemas = SisoEnvironment.Resources.ResolveStructureSchemas();
            StructureBuilder = SisoEnvironment.Resources.ResolveStructureBuilder();
            DbSchemaManager = SisoEnvironment.Resources.ResolveDbSchemaManager();
        }

        public void EnsureNewDatabase()
        {
            if(IoHelper.FileExists(FilePath))
                IoHelper.DeleteIfFileExists(FilePath);

            CreateIfNotExists();
        }

        public void CreateIfNotExists()
        {
            if(IoHelper.FileExists(FilePath))
                return;

            using (var engine = new SqlCeEngine(ConnectionInfo.ConnectionString.PlainString))
            {
                engine.CreateDatabase();
            }
        }

        public void InitializeExisting()
        {
            if(!IoHelper.FileExists(FilePath))
                throw new SisoDbException(SqlCe4Exceptions.SqlCe4Database_InitializeExisting_DbDoesNotExist.Inject(Name));
            

            //using(var dbClient = new SqlCe4DbClient(ConnectionInfo, true))
            //{
            //    dbClient.
            //}
        }

        public void DeleteIfExists()
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public void DropStructureSet<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void UpsertStructureSet<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void UpdateStructureSet<TOld, TNew>(Func<TOld, TNew, StructureSetUpdaterStatuses> onProcess)
            where TOld : class
            where TNew : class
        {
            throw new NotImplementedException();
        }

        public IQueryEngine CreateQueryEngine()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            throw new NotImplementedException();
        }
    }
}
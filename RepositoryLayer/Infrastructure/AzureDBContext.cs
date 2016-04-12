using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;
using Core.Domain;
using AppInterfaces.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using System.Data.Entity.Validation;
using Core.Helper;

namespace RepositoryLayer
{
    public class AzureDBContext : DbContext, IEntitiesContext
    {

        #region ctor
        public AzureDBContext() : base("name=AzureDBContext")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            //Database.SetInitializer<AzureDBContext>(new DBInitializer());
        }
        public AzureDBContext(string connectionString) : base(connectionString)
        {
        }
        #endregion

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Users>();            
            modelBuilder.Entity<Program>();
            modelBuilder.Entity<Race>();
            modelBuilder.Entity<InsuranceType>();
            modelBuilder.Entity<ServiceRequest>();
            modelBuilder.Entity<PreferredLanguage>();
            modelBuilder.Entity<Ethnicity>();
            modelBuilder.Entity<ReferralSource>();            
            modelBuilder.Entity<Log>();
            modelBuilder.Entity<LogTypes>();
            modelBuilder.Entity<LogProperty>();
            modelBuilder.Entity<LogPropertyChange>();
            modelBuilder.Entity<LogInfo>();
            modelBuilder.Entity<Roles>();
            modelBuilder.Entity<Screen>();
            modelBuilder.Entity<ScreenAction>();
            modelBuilder.Entity<Module>();
            modelBuilder.Entity<ScreenPermission>();
            modelBuilder.Entity<UserRoles>();
            modelBuilder.Entity<LoginAuthentication>();
            modelBuilder.Entity<ModulePermission>();
            modelBuilder.Entity<webpages_Membership>();
            modelBuilder.Entity<SecUserPasswordHistory>();            

            base.OnModelCreating(modelBuilder);
        }


        /***********************************/
        /*
            ///Register Classes
        public System.Data.Entity.DbSet<Core.Domain.User> Users { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Patient> Patients { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Program> Programs { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Race> Races { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.InsuranceType> InsuranceTypes { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ServiceRequest> ServiceRequests { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.PreferredLanguage> PreferredLanguages { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Ethnicity> Ethnicities { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ReferralSource> ReferralSources { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Scheduler> Scheduler { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Log> Log { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.LogType> LogType { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.LogProperty> LogProperty { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.LogPropertyChange> LogPropertyChange { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.LogInfo> LogInfoes { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Roles> Roles { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.screen> screen { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ScreenAction> screenaction { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.Module> module { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ScreenPermission> screenpermission { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.UserRoles> userroles { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.LoginAuthentication> loginAuthentication { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ModulePermission> modulePermission { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.webpages_Membership> webpages_membership { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.SecUserPasswordHistory> secUserPasswordHistory { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ScanMasterPages> scanMasterPages { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ScannedRecord> scannedRecord { get; set; }
        public System.Data.Entity.DbSet<Core.Domain.ScannedRecordDetails> scannedRecordDetails { get; set; }
        */

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            var context = ((IObjectContextAdapter)(this)).ObjectContext;

            var connection = this.Database.Connection;
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000;

                if (parameters != null)
                    foreach (var p in parameters)
                    {
                        if (p != null)
                            cmd.Parameters.Add(p);
                    }
                IList<TEntity> result;

                using (var reader = cmd.ExecuteReader())
                {
                    result = context.Translate<TEntity>(reader).ToList();
                    for (int i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);
                }


                return result;
            }
        }
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            //var alreadyAttached = Set<TEntity>().Local.Where(x => x.Id == entity.Id).FirstOrDefault();
            if (entity.Id > 0)
            {
                var alreadyAttached = Set<TEntity>().Local.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (alreadyAttached == null)
                {
                    //attach new entity
                    Set<TEntity>().Attach(entity);
                    return entity;
                }
                else
                {
                    //entity is already loaded.
                    return alreadyAttached;
                }
            }
            else
            {
                return entity;
            }
        }
        public int ExecuteStoredProcedureNonQuery(string commandText, params object[] parameters)
        {
            var context = ((IObjectContextAdapter)(this)).ObjectContext;

            //var connection = context.Connection;
            var connection = this.Database.Connection;
            //Don't close the connection after command execution

            //open the connection for use
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            //create a command object
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;

                // move parameters to command object
                if (parameters != null)
                    foreach (var p in parameters)
                    {
                        if (p != null)
                            cmd.Parameters.Add(p);
                    }

                var rowseffected = cmd.ExecuteNonQuery();


                //  this.Database.ExecuteSqlCommand(EXEC " + storeProcName + " @Paramtable";)

                // var rowseffected = cmd.ExecuteScalar();
                return int.Parse(rowseffected.ToString());
            }
        }
        public static bool GetLogInfoStatus(string ModuleName)
        {
            bool returnVal = false;
            var conn = new SqlConnection();
            var sCmd = new SqlCommand
            {
                CommandText = "GetLogs",
                CommandTimeout = 600,
                CommandType = CommandType.StoredProcedure
            };
            sCmd.Parameters.AddWithValue("ModuleName", ModuleName);
            SqlDataReader drResults = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    sCmd.Connection = conn;
                    drResults = sCmd.ExecuteReader(CommandBehavior.CloseConnection);
                    sCmd.Dispose();

                    if (drResults.HasRows)
                    {
                        while (drResults.Read())
                        {
                            int rowsAffected = (drResults["RowsAffected"] != DBNull.Value) ? int.Parse(drResults["RowsAffected"].ToString()) : 0;
                            if (rowsAffected > 0)
                                returnVal = true;
                        }

                        drResults.Close();
                        drResults.Dispose();
                    }
                    drResults = null;
                }
                else
                {
                    throw new Exception("Database connection could not be established.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Recurly GetLogs from [Framework].[dbo].[GetLogs].", ex);
            }
            finally
            {
                if (drResults != null && !drResults.IsClosed)
                {
                    drResults.Close();
                    drResults.Dispose();
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                conn.Dispose();
            }
            return returnVal;
        }

        /// <summary>
        /// List of types that will be logged by the application
        /// </summary>
        private List<LogTypes> TypesToLog { get; set; }
        public override int SaveChanges()
        {
            //Added By Chander
            // 18-2-2015
            TypesToLog = Set<LogTypes>().ToList();
            //get a list of the changes that the user made
            var logs = new List<Log>();
            var properties = new List<LogPropertyChange>();
            int? userId = null;
            DateTime now = DateTime.Now;
            foreach (var entry in this.ChangeTracker.Entries<object>())
            {
                var entryType = entry.Entity.GetType();
                var typeInfo = (from i in TypesToLog
                                where i.Key == entryType.Name || entryType.Name.StartsWith(i.Key + "_")
                                select i).FirstOrDefault();
                if (typeInfo != null)
                {
                    int objectId = 0;
                    string clientId = "";
                    int? parentId = null;
                    Guid logId = Guid.NewGuid();
                    var message = "";
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            objectId = (int)entry.CurrentValues[typeInfo.IdName];
                            if (!string.IsNullOrWhiteSpace(typeInfo.ClientIdName))
                                //Value that identifies the object for the user (Name, number, etc)
                                clientId = (entry.CurrentValues[typeInfo.ClientIdName] ?? string.Empty).ToString();
                            message = string.Format("A new {0} was created ({1})", typeInfo.Name, clientId);
                            if (!string.IsNullOrWhiteSpace(typeInfo.ParentIdName))
                                //Id of the parent (to be logged in children objects)
                                parentId = (int)entry.CurrentValues[typeInfo.ParentIdName];
                            break;
                        case EntityState.Deleted:
                            objectId = (int)entry.OriginalValues[typeInfo.IdName];
                            if (!string.IsNullOrWhiteSpace(typeInfo.ClientIdName))
                                //Value that identifies the object for the user (Name, number, etc)
                                clientId = (entry.OriginalValues[typeInfo.ClientIdName] ?? string.Empty).ToString();
                            message = string.Format("The {0} {1} was deleted.", typeInfo.Name, clientId);
                            if (!string.IsNullOrWhiteSpace(typeInfo.ParentIdName))
                                //Id of the parent (to be logged in children objects)
                                parentId = (int)entry.OriginalValues[typeInfo.ParentIdName];
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Modified:
                            objectId = (int)entry.CurrentValues[typeInfo.IdName];

                            if (!string.IsNullOrWhiteSpace(typeInfo.ParentIdName))
                                //Id of the parent (to be logged in children objects)
                                parentId = (int)entry.CurrentValues[typeInfo.ParentIdName];
                            if (!string.IsNullOrWhiteSpace(typeInfo.ClientIdName))
                                //Value that identifies the object for the user (Name, number, etc)
                                clientId = (entry.OriginalValues[typeInfo.ClientIdName] ?? string.Empty).ToString();
                            var sb = new StringBuilder();
                            sb.AppendLine(string.Format("The {0} {1} was updated.", typeInfo.Name, clientId));
                            //Go through each of the properties that will be logged
                            foreach (var property in FieldsToLog(entryType))
                            {
                                string originalValue = entry.OriginalValues[property.Key] == null ? "" : entry.OriginalValues[property.Key].ToString();
                                string currentValue = entry.CurrentValues[property.Key] == null ? "" : entry.CurrentValues[property.Key].ToString();

                                if (originalValue != currentValue)
                                {
                                    if (property.LogValues)
                                    {
                                        sb.AppendLine(string.Format("-The value of {0} changed from {1} to {2}.", property.Name, originalValue, currentValue));
                                        //Log the changes to each property
                                        Guid LogPropertyChangeId = Guid.NewGuid();
                                        properties.Add(new LogPropertyChange { LogId = logId, LogPropertyChangeId = LogPropertyChangeId, PropertyKey = property.Key, PreviousValue = originalValue, NewValue = currentValue, EncryptionKey = LogHelper.GetSHA1HashData(logId + LogPropertyChangeId.ToString() + property.Key + originalValue + currentValue) });
                                    }
                                    else
                                        sb.AppendLine(string.Format("-The value of {0} was changed.", property.Name));
                                }
                            }
                            message = sb.ToString();
                            break;
                        case EntityState.Unchanged:
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        logs.Add(new Log
                        {
                            LogId = logId,
                            ObjectId = objectId,
                            ParentId = parentId,
                            TypeKey = typeInfo.Key,
                            OperationKey = entry.State.ToString(),
                            UserId = userId,
                            Representative = false,
                            Message = message,
                            Created = now,
                            Processed = true,
                            SendEmail = false,
                            WriteAsFile = false
                        });
                    }
                }
            }

            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }


            //write the list of user changes to the log table
            foreach (var log in logs)
            {
                Set<Log>().Add(log);
            }

            foreach (var property in properties)
            {
                Set<LogPropertyChange>().Add(property);

            }

            return base.SaveChanges();
        }

        private Dictionary<Type, List<LogProperty>> AuditInfo { get; set; }

        /// <summary>
        ///	We are useing reflection to figure out what fields we need to worry about
        ///   We are caching the results of the reflection so that we only need to do this
        ///   one time per entity type
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private IEnumerable<LogProperty> FieldsToLog(Type entityType)
        {
            if (this.AuditInfo == null)
                this.AuditInfo = new Dictionary<Type, List<LogProperty>>();

            if (!this.AuditInfo.ContainsKey(entityType))
            {

                var properties = Set<LogProperty>().Where(a => a.TypeKey == entityType.Name).ToList();

                this.AuditInfo.Add(entityType, properties);

                //var auditPropertyInfo = new List<LogProperty>();
                //var auditFields = typeof(IAuditable).GetProperties().Select(x => x.Name).ToList();
                //foreach (var property in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                //{
                //	if (!property.GetGetMethod().IsVirtual)
                //		auditPropertyInfo.Add(property.Name);
                //}
                //this.AuditInfo.Add(entityType, auditPropertyInfo);
            }

            return this.AuditInfo[entityType];
        }

    }
}

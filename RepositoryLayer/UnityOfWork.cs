using AppInterfaces.Infrastructure;
using AppInterfaces.Repository;
using Core;
using Core.Domain;
using Core.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;

namespace RepositoryLayer
{


    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private IDictionary<Type, object> _repositories;

        private bool _disposed;

        private IEntitiesContext _context;



        private TransactionScope transaction;

        public void StartTransaction()
        {
            this.transaction = new TransactionScope();
        }

        public void CommitTransaction()
        {
            this.transaction.Complete();
        }
        public IEntitiesContext Context
        {
            get { return _context; }
        }
        /// <summary>
        /// List of properties for each type that will be logged by the application
        /// </summary>
        private Dictionary<Type, List<LogProperty>> AuditInfo { get; set; }

        /// <summary>
        /// List of types that will be logged by the application
        /// </summary>
        private List<LogTypes> TypesToLog { get; set; }
        public UnitOfWork(IEntitiesContext context)
        {
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
            _context = context;
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public EfRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as EfRepository<T>;
            }
            EfRepository<T> repo = new EfRepository<T>(_context);
            repositories.Add(typeof(T), repo);
            return repo;
        }
        public void SaveChanges()
        {
            //Added By Chander
            // 18-2-2015
            TypesToLog = this.Repository<LogTypes>().GetAll().ToList();
            //get a list of the changes that the user made
            var logs = new List<Log>();
            var properties = new List<LogPropertyChange>();
            int? userId = null;
            DateTime now = DateTime.Now;
            foreach (var entry in _context.ChangeTracker.Entries<object>())
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
                _context.SaveChanges();
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
                this.Repository<Log>().Add(log);
            }

            foreach (var property in properties)
            {
                this.Repository<LogPropertyChange>().Add(property);

            }

            _context.SaveChanges();
        }
        //public void SaveChanges()
        //{

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        List<string> errorMessages = new List<string>();
        //        foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
        //        {
        //            string entityName = validationResult.Entry.Entity.GetType().Name;
        //            foreach (DbValidationError error in validationResult.ValidationErrors)
        //            {
        //                errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
        //            }
        //        }
        //    }

        //}


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

                var properties = this.Repository<LogProperty>().GetAll(a => a.TypeKey == entityType.Name).ToList();

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

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.transaction.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }


       
        

        /// <summary>
        /// Commits transaction and closes database connection.
        /// </summary>
        public void Commit()
        {
            try
            {
               // _transaction.Commit();
            }
            finally
            {
               // Session.Close();
            }
        }

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        public void Rollback()
        {
            try
            {
             //  _transaction.Rollback();
            }
            finally
            {
              //  Session.Close();
            }
        }

        public void RegenerateContext()
        {
            //throw new NotImplementedException();
        }
    }
}


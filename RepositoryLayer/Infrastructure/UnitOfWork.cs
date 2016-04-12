using AppInterfaces.Infrastructure;
using AppInterfaces.Repository;
using Core.Domain;
using Core.Helper;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;

namespace RepositoryInfrastructure
{
    /// <summary>
    /// Unit of work implementation for having single instance of context and doing DB operation as transaction
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    //public abstract class UnitOfWork<TContext> : IUnitOfWork
    //    where TContext : DbContext
    public abstract class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// DB context
        /// </summary>
        protected IEntitiesContext _context;

        protected UnitOfWork(IEntitiesContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        /// <summary>
        /// Regenerates the context.
        /// </summary>
        /// <remarks>WARNING: Calling with dirty context will save changes automatically</remarks>
        //public void RegenerateContext()
        //{
        //    if (_context != null)
        //    {
        //        _context.SaveChanges();
        //    }
        //    _context = Create();
        //}

        /// <summary>
        /// Saves Context changes.
        /// </summary>
        //public void Save()
        //{
        //    Context.SaveChanges();
        //}

        #region " Dispose "

        ///// <summary>
        ///// To detect redundant calls
        ///// </summary>
        //private bool _disposedValue;

        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing">IsDisposing</param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            if (_dbContext != null)
        //            {
        //                _dbContext.Dispose();
        //            }
        //        }
        //    }
        //    _disposedValue = true;
        //}

        #region " IDisposable Support "

        /// <summary>
        /// Dispose the object
        /// </summary>
        //public void Dispose()
        //{
        //    Dispose(true);

        //    GC.SuppressFinalize(this);
        //}

        #endregion " IDisposable Support "

        #endregion " Dispose "

        private IDictionary<Type, object> _repositories;

        private bool _disposed;

        // private DbContext _context;
        /// <summary>
        /// List of properties for each type that will be logged by the application
        /// </summary>

        public virtual IEntitiesContext Context
        {
            get
            {
                return _context;
            }
        }
        #region commented code
        //public UnitOfWork(DbContext context)
        //{
        //    _repositories = new Dictionary<Type, object>();
        //    _disposed = false;
        //    //_context = context;
        //}

        /*  public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

          public IRepository<T> Repository<T>() where T : class
          {
              if (repositories.Keys.Contains(typeof(T)) == true)
              {
                  return repositories[typeof(T)] as IRepository<T>;
              }
              IRepository<T> repo = new Repository<T>(this);
              repositories.Add(typeof(T), repo);
              return repo;
          }
          public void SaveChanges()
          {
              //Added By Chander
              // 18-2-2015
              TypesToLog = this.Repository<LogType>().GetAll().ToList();
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
         */ //public void SaveChanges()
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
        #endregion

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    Rollback();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            Rollback();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region transaction

        private TransactionScope _transaction;
        public void StartTransaction()
        {
            var scopeOption = new TransactionOptions();
            scopeOption.IsolationLevel = IsolationLevel.ReadCommitted;
            _transaction = new TransactionScope(TransactionScopeOption.Required, scopeOption);

        }
        public void CommitTransaction()
        {
            if (_context.ChangeTracker.HasChanges())
                _context.SaveChanges();
            _transaction.Complete();
        }

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (_transaction != null)
                    _transaction.Dispose();
            }
            finally
            {
                //  Session.Close();
            }
        }
        #endregion
    }
}
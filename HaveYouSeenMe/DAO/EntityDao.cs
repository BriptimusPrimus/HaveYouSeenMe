using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.DAO
{
    public abstract class EntityDao<TEntity> : IDao<TEntity> //where TEntity : EntityObject, new()
    {
        #region Helper methods

        protected Entities Context
        {
            get
            {
                return ObjectContextManager.Context;
            }
        }

        private string entitySetName;
        protected string EntitySetName
        {
            get
            {
                if (String.IsNullOrEmpty(entitySetName))
                {
                    entitySetName = Context.GetEntitySetName(typeof(TEntity).Name);
                }
                return entitySetName;
            }
        }

        #endregion

        #region IDao<TEntity> Members

        public abstract TEntity Save(TEntity entity);

        public abstract TEntity Update(TEntity entity);

        public abstract void Delete(TEntity entity);

        #endregion
    }
}
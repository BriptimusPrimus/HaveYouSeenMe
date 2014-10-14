using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveYouSeenMe.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;

namespace HaveYouSeenMe.DAO
{
    public class ObjectContextManager
    {
        private static readonly Entities context = new Entities();
        private ObjectContextManager() { }
        public static Entities Context { get { return context; } }
    }

    public static class ContextExtensions
    {
        public static string GetEntitySetName(this DbContext context, string entityTypeName)
        {
            //var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
            var objContext = ((IObjectContextAdapter)context).ObjectContext;
            var container = objContext.MetadataWorkspace.GetEntityContainer(objContext.DefaultContainerName, DataSpace.CSpace);
            string entitySetName = (from meta in container.BaseEntitySets
                                    where meta.ElementType.Name == entityTypeName
                                    select meta.Name).FirstOrDefault();
            return entitySetName;
        }    
    }
}
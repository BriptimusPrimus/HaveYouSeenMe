using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveYouSeenMe.Models;

namespace HaveYouSeenMe.Utilities
{
    public class ModelsConverter
    {
        //convert an object to another of type T
        //returns null when conversion is not possible 
        public static T ConvertModel<T>(Object obj)
            where T : class, ITransformableViewModel, new()
        {
            var t = new T();
            if (!t.GetValues(obj))
            { 
                //if not convertible return null
                return null; 
            }
            return t;
        }

        //returns a list of a generic class that implements ITransformableViewModel
        //obtaining the values from any object list
        public static IEnumerable<T> ConvertList<T>(IEnumerable<Object> original) 
            where T : ITransformableViewModel, new()
        {            
            List<T> list = new List<T>();
            foreach (var obj in original)
            {
                var t = new T();
                //GetValues() returns false when obj not applicable
                if (t.GetValues(obj))
                {
                    //only add to results when conversion was succesfull
                    list.Add(t);
                }
            }
            return list;        
        }

    }
}
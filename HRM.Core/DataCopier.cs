using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace HRM.Core
{
    public static class DataCopier
    {

        public static object Copy(object source, object destination)
        {
            if (source != null)
            {

                if (typeof(IEnumerable).IsAssignableFrom(source.GetType()) && !source.GetType().Equals(typeof(string)))
                {
                 
                    if (source.GetType().GetGenericArguments().Count() == 0 && destination.GetType().GetGenericArguments().Count() == 0 && typeof(System.Collections.ArrayList)!=destination.GetType())
                    {
                        IEnumerator enumerator = ((IEnumerable)source).GetEnumerator();
                        IEnumerator enumerator1 = ((IEnumerable)destination).GetEnumerator();
                        Type genericType = null;
                        while (enumerator1.MoveNext())
                        {
                            genericType = enumerator1.Current.GetType();
                            break;
                        }
                        int i=0;
                        while (enumerator.MoveNext())
                        {
                            object des = Activator.CreateInstance(genericType);
                            object src = enumerator.Current;
                            des = Copy(src, des);
                            ((IList)destination)[i] = des;
                            i++;
                        }
                    }
                    else if ((source.GetType().GetGenericArguments().Count() == 1 && destination.GetType().GetGenericArguments().Count() == 1) || typeof(System.Collections.ArrayList) == destination.GetType())
                    {
                        Type genericType = typeof(System.Object);

                        if(destination.GetType().GetGenericArguments().Count()==1)
                            genericType = destination.GetType().GetGenericArguments().First();
                        IEnumerator enumerator1 = ((IEnumerable)source).GetEnumerator();
                        while (enumerator1.MoveNext())
                        {
                            object des = Activator.CreateInstance(genericType);
                            object src = enumerator1.Current;
                            des = Copy(src, des);
                            destination.GetType().GetMethod("Add").Invoke(destination, new[] { des });
                        }

                    }


                }
                else if (source.GetType().IsInterface || source.GetType().IsClass && !source.GetType().Equals(typeof(string)))
                {
                    traverseObjectProperties(source, destination);
                }
                else
                {
                    if (destination.GetType() == source.GetType())
                    {
                        destination = source;
                    }
                    else if (destination.GetType() == typeof(System.Object))
                    {
                        destination = (object)source;
                    }
                    else
                    {
                        var converter = TypeDescriptor.GetConverter(destination.GetType());
                        try
                        {
                            destination = converter.ConvertFrom(source.ToString());
                        }
                        catch
                        {

                        }
                    }
                }
               
            }
           
            return destination;
           
        }

        private static void traverseObjectProperties(object source, object destination)
        {
            PropertyInfo[] destinationPropertiesArray = destination.GetType().GetProperties();
            PropertyInfo[] sourcePropertiesArray = source.GetType().GetProperties();

            if (destinationPropertiesArray != null && destinationPropertiesArray.Length > 0 && sourcePropertiesArray != null && sourcePropertiesArray.Length > 0)
            {
                foreach (PropertyInfo sourceProperty in sourcePropertiesArray)
                {


                    object sourceObj = sourceProperty.GetValue(source, null);
                    if (sourceObj != null && sourceObj.ToString() != "")
                    {
                        PropertyInfo destinationProperty = destinationPropertiesArray.Where(x => x.Name.Equals(sourceProperty.Name)).FirstOrDefault();
                        if (destinationProperty != null)
                        {
                            object destinationObj = destinationProperty.GetValue(destination, null);
                            if (setObject(destination, sourceProperty, sourceObj, destinationProperty, destinationObj))
                            {
                            }
                            else
                            {
                                setValue(destination, sourceProperty, sourceObj, destinationProperty);
                            }

                        }
                    }

                }
            }
        }

        private static bool setObject(object destination, PropertyInfo sourceProperty, object sourceObj, PropertyInfo destinationProperty, object destinationObj)
        {
            if (typeof(IEnumerable).IsAssignableFrom(sourceProperty.PropertyType) && !sourceProperty.PropertyType.Equals(typeof(string)) && typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType) && !destinationProperty.PropertyType.Equals(typeof(string)))
            {
                if (destinationObj == null)
                {
                    if (destinationProperty.PropertyType.GetConstructors().All(c => c.GetParameters().Length == 0))
                    {

                        var d = Activator.CreateInstance(destinationProperty.PropertyType);
                        destinationProperty.SetValue(destination, d, null);
                        destinationObj = destinationProperty.GetValue(destination, null);
                    }
                    else
                    {
                        int ct = 0;
                        IEnumerator enumerator = ((IEnumerable)sourceObj).GetEnumerator();
                        while (enumerator.MoveNext())
                            ct++;
                        if (destinationProperty.PropertyType.Name.Contains("[]"))
                        {
                            var array = Activator.CreateInstance(destinationProperty.PropertyType, ct);
                            destinationProperty.SetValue(destination, array, null);
                            destinationObj = destinationProperty.GetValue(destination, null);
                        }
                        else
                        {
                            var d = Activator.CreateInstance(destinationProperty.PropertyType);
                            destinationProperty.SetValue(destination, d, null);
                            destinationObj = destinationProperty.GetValue(destination, null);
                        }
                        
                    }
                }
                Copy(sourceObj, destinationObj);
                return true;
            }
            else if (sourceProperty.PropertyType.IsInterface || sourceProperty.PropertyType.IsClass && !sourceProperty.PropertyType.Equals(typeof(string)))
            {
                if (destinationProperty.PropertyType.IsInterface || destinationProperty.PropertyType.IsClass && !destinationProperty.PropertyType.Equals(typeof(string)) && destinationProperty.PropertyType.Name.Equals(sourceProperty.PropertyType.Name))
                {
                    if (destinationObj == null)
                    {
                        if (destinationProperty.PropertyType.GetConstructors().All(c => c.GetParameters().Length == 0))
                        {

                            var d = Activator.CreateInstance(destinationProperty.PropertyType);
                            destinationProperty.SetValue(destination, d, null);
                            destinationObj = destinationProperty.GetValue(destination, null);
                        }
                    }
                    Copy(sourceObj, destinationObj);
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private static void setValue(object destination, PropertyInfo sourceProperty, object sourceObj, PropertyInfo destinationProperty)
        {
            if (destinationProperty.PropertyType.Name.Equals(sourceProperty.PropertyType.Name))
            {
                destinationProperty.SetValue(destination, sourceObj, null);
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(destinationProperty.PropertyType);
                try
                {
                    var result = converter.ConvertFrom(sourceObj.ToString());
                    destinationProperty.SetValue(destination, result, null);
                }
                catch
                {

                }

            }
        }

        public static object CopyFrom(this object destination, object source)
        {

            return Copy(source, destination);
        }
    }
}

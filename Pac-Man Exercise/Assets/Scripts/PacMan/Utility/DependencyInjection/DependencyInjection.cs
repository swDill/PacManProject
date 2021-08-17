using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace PacMan.Utility.DependencyInjection
{
    /*
    * Simple implementation of dependency injection, eliminates the need for the use of Singletons, allows systems to be more locked down.
    * The process of injecting references is a bit backwards for simplicity.
    */
    public static class DependencyInjection
    {
        // List of known dependencies
        private static readonly List<object> Dependencies = new List<object>(); 
        
        // Request for the target object to be filled with known dependencies
        public static void RequestDependencies(object targetObject)
        {
            Stopwatch injectTime = new Stopwatch();
            injectTime.Start();
            
            FieldInfo[] fields = targetObject.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                Attribute injectableTarget = field.GetCustomAttributes(typeof(InjectableAttribute)).First();

                if (injectableTarget == null) continue;
                
                object dependency = Dependencies.FirstOrDefault(d => d.GetType() == field.FieldType);

                if (dependency == null) continue;
                
                field.SetValue(targetObject, dependency);
            }
            
            injectTime.Stop();
        }
        
        // Memorize reference of a dependency to be injected when requested
        public static void InjectAsType<T>(T newInstance)
        {
            if (Dependencies.OfType<T>().Any())
            {
                Debug.LogWarning($"Replacing previous type of { typeof(T) } with new instance.");
                Dependencies.RemoveAll(old => old.GetType() == typeof(T));
            }

            AddAndInjectNewInstance(newInstance);
        }
        
        // Add a new dependency to the list of known dependencies
        private static void AddAndInjectNewInstance<T>(T newInstance)
        {
            Debug.Log($"Adding instance of type { typeof(T) } to known dependencies.");
            
            Dependencies.Add(newInstance);
            
            //TODO Automatically inject new reference into previously injected fields.
        }

        // Remove a dependency from the known dependencies list 
        public static void DeleteDependency<T>()
        {
            Dependencies.Remove(Dependencies.OfType<T>().FirstOrDefault());
        }
    }
}
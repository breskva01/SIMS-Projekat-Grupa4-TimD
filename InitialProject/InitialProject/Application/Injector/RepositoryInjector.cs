﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Injector
{
    public class RepositoryInjector
    {
        private static Dictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        public static T Get<T>()
        {
            var type = typeof(T);
            return (T)Get(type);
        }

        private static object Get(Type type)
        {
            var target = ResolveType(type);
            var constructor = target.GetConstructors()[0];
            return constructor.Invoke(null);
        }

        private static Type ResolveType(Type type)
        {
            if (mappings.Keys.Contains(type))
            {
                return mappings[type];
            }

            return type;
        }

        public static void Map<T, V>() where V : T
        {
            mappings.Add(typeof(T), typeof(V));
        }

        public static void Clear()
        {
            mappings.Clear();
        }
    }

}

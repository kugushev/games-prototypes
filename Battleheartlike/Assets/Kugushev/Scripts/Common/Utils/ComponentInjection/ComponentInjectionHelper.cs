using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kugushev.Scripts.Common.Utils.ComponentInjection
{
    public static class ComponentInjectionHelper
    {
        private static readonly Dictionary<Type, IReadOnlyList<PropertyInfo>> PropertiesBag =
            new Dictionary<Type, IReadOnlyList<PropertyInfo>>();

        private static readonly Dictionary<Type, IReadOnlyList<Type>> InterfacesBag =
            new Dictionary<Type, IReadOnlyList<Type>>();

        public static IReadOnlyList<PropertyInfo> FindInjectableProperties(object obj)
        {
            if (obj == null)
                return Array.Empty<PropertyInfo>();

            var type = obj.GetType();
            
            if (!PropertiesBag.TryGetValue(type, out var injectableProperties))
            {
                var allProperties = type.GetProperties();

                var properties = new List<PropertyInfo>(allProperties.Length);
                foreach (var property in allProperties)
                {
                    if (property.GetCustomAttribute<ComponentInjectionAttribute>() != null)
                    {
                        // find the class that is responsible for private set than interface
                        var declaringType = property.DeclaringType;
                        if (declaringType != null)
                        {
                            var targetProperty = declaringType.GetProperty(property.Name);
                            properties.Add(targetProperty);
                        }
                    }
                }

                PropertiesBag[type] = injectableProperties = properties;
            }

            return injectableProperties;
        }

        public static IReadOnlyList<Type> FindInterfaces(object obj)
        {
            if (obj == null)
                return Array.Empty<Type>();

            var type = obj.GetType();

            if (!InterfacesBag.TryGetValue(type, out var interfaces))
                InterfacesBag[type] = interfaces = type.GetInterfaces();

            return interfaces;
        }

        public static void AssignIfPossible(object obj, PropertyInfo pi, object assignable,
            IReadOnlyList<Type> interfaces)
        {
            foreach (var type in interfaces)
                if (pi.PropertyType == type)
                {
                    pi.SetValue(obj, assignable);
                    return;
                }
        }
    }
}
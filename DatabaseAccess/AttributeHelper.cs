using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DatabaseAccess
{
    public static class AttributeHelper
    {
        private static readonly Dictionary<Type, PropertyData[]> _databaseEntityInfo = new Dictionary<Type, PropertyData[]>();

        static AttributeHelper()
        {
            var databaseEntities =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                    .Where(c => c.IsSubclassOf(typeof(DatabaseEntity)) && !c.IsAbstract);

            foreach (Type entity in databaseEntities)
            {
                LoadEntityInfo(entity);
            }
        }

        private static void LoadEntityInfo(Type type)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propertyData = properties.Select(p => new PropertyData(p, p.GetCustomAttribute<DataColumnAttribute>())).ToArray();
            _databaseEntityInfo.Add(type, propertyData);
        }

        public static PropertyData[] GetPropertyData(Type t)
        {
            return _databaseEntityInfo[t];
        }
    }
}

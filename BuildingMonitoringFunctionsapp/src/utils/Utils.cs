using System;
using System.Collections.Generic;
using System.Data;

namespace BuildingMonitoringFunctionsapp.src.utils
{
    internal class Utils
    {
        //  Return list of type T
        public static List<T> getList<T>(IDataReader reader)
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    var propType = prop.PropertyType;

                    // Convert and set value for specific property inside RoomConfig class
                    prop.SetValue(obj, Convert.ChangeType(reader[prop.Name].ToString(), propType));
                }
            }
            return list;
        }
    }
}

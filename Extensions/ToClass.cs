using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bible_Bot.Extensions
{
    public static class Extension
    {  
        public static T ToClass<T>(this Dictionary<string, AttributeValue> dict) where T : new()
        {
            var type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var property = type.GetProperty(kv.Key);
                if (property != null)
                {
                    if (!string.IsNullOrEmpty(kv.Value.S))
                    {
                        property.SetValue(obj, kv.Value.S);
                    }
                    else if (!string.IsNullOrEmpty(kv.Value.N))
                    {
                        property.SetValue(obj, Convert.ToInt32(kv.Value.N));
                    }
                    else
                    {
                        property.SetValue(obj, kv.Value.SS);
                    }
                }
            }
            return (T)obj;
        }
    }
}

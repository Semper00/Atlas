using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool TryGetCustomAttribute<TAttribute>(this MethodInfo method, out TAttribute result) where TAttribute : Attribute
        {
            result = method.GetCustomAttribute<TAttribute>();

            return result != null;
        }

        public static T As<T>(this object obj) => obj is T t ? t : default;
    }
}

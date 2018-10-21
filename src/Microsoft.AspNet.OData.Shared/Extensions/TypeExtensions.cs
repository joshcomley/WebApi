using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData.Common;

namespace Microsoft.AspNet.OData.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsCollection(this Type type)
        {
            Type elementType;
            return type.IsCollection(out elementType);
        }

        public static bool IsCollection(this Type type, out Type elementType)
        {
            if (type == null)
            {
                throw Error.ArgumentNull("type");
            }

            elementType = type;

            // see if this type should be ignored.
            if (type == typeof(string))
            {
                return false;
            }

            Type collectionInterface
                = type.GetInterfaces()
                    .Union(new[] { type })
                    .FirstOrDefault(
                        t => t.GetTypeInfo().IsGenericType
                             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (collectionInterface != null)
            {
                elementType = collectionInterface.GetGenericArguments().Single();
                return true;
            }

            return false;
        }
    }
}
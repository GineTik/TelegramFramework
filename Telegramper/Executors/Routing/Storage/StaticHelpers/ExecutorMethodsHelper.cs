﻿using System.Reflection;
using Telegramper.Executors.Attributes.BaseAttributes;

namespace Telegramper.Executors.Routing.Storage.StaticHelpers
{
    internal static class ExecutorMethodsHelper
    {
        public static IEnumerable<MethodInfo> TakeExecutorMethodsFrom(IEnumerable<Type> executorsTypes)
        {
            return executorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetAttribute>().Count() > 0)
                // check return type
                .Select(method => method.ReturnType == typeof(Task) ? method :
                        throw new Exception($"Return type of method {method.Name} not Task"));
        }
    }
}

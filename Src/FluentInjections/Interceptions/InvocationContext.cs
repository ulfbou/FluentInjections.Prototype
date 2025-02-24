// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;


namespace FluentInjections.Interceptions
{
    public class InvocationContext
    {
        public object Target { get; }
        public MethodInfo Method { get; }
        public object[] Arguments { get; }
        public IDictionary<string, object> Items { get; }

        public InvocationContext(object target, MethodInfo method, object[] arguments, IDictionary<string, object> items = null)
        {
            Target = target;
            Method = method;
            Arguments = arguments;
            Items = items ?? new ConcurrentDictionary<string, object>();
        }

        public T GetArgument<T>(int index)
        {
            if (index < 0 || index >= Arguments.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Argument index is out of range.");
            }
            if (Arguments[index] is T argument)
            {
                return argument;
            }
            throw new InvalidCastException($"Argument at index {index} is not of type {typeof(T).FullName}.");
        }

        public void SetArgument(int index, object value)
        {
            if (index < 0 || index >= Arguments.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Argument index is out of range.");
            }
            Arguments[index] = value;
        }

        public T GetItem<T>(string key)
        {
            if (Items.TryGetValue(key, out object? value))
            {
                if (value is T item)
                {
                    return item;
                }
                throw new InvalidCastException($"Item with key '{key}' is not of type {typeof(T).FullName}.");
            }
            throw new KeyNotFoundException($"Item with key '{key}' not found.");
        }

        public void SetItem(string key, object value)
        {
            Items[key] = value;
        }
    }
}

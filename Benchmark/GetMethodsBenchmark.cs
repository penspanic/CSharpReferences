using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    public class GetMethodsBenchmark
    {
        Type[] TargetTypes() => new [] { typeof(System.IO.File), typeof(System.String), typeof(BenchmarkDotNet.Engines.Engine) };

        [Params(10000)]
        public int N;

        [Benchmark]
        public void NoCaching()
        {
            MethodInfo[] result = null;
            Action<MethodInfo> action = Foo;
            for (int i = 0; i < N; ++i)
            {
                foreach (Type type in TargetTypes())
                {
                    result = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.Static);
                    if (result != null)
                    {
                        foreach (MethodInfo methodInfo in result)
                        {
                            action(methodInfo);
                        }
                    }
                }
            }
        }

        [Benchmark]
        public void Caching()
        {
            IReadOnlyList<MethodInfo> result = null;
            Action<MethodInfo> action = Foo;
            Func<MethodInfo, bool> methodFilter = m => m.IsPublic || m.IsPrivate;
            for (int i = 0; i < N; ++i)
            {
                foreach (Type type in TargetTypes())
                {
                    TypeInfoHolderHelper.GetMethods(type, methodFilter);
                    if (result != null)
                    {
                        foreach (MethodInfo methodInfo in result)
                        {
                            action(methodInfo);
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private void Foo(MethodInfo methodInfo)
        {
            
        }

        public void DoMethodThings()
        {
            Type[] targetTypes = new[] {typeof(int), typeof(System.IO.File), typeof(System.Diagnostics.Activity)};

            // raw
            foreach (Type type in targetTypes)
            {
                var result = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            }
            // cached
            foreach (Type type in targetTypes)
            {
                var result = TypeInfoHolderHelper.GetMethods(type, (m) => m.IsPublic && m.IsStatic == false);
            }
        }
    }

    public static class TypeInfoHolder<T>
    {
        private static Dictionary<Func<MethodInfo, bool>, List<MethodInfo>> _methods = new Dictionary<Func<MethodInfo, bool>, List<MethodInfo>>();
        private static Dictionary<Func<FieldInfo, bool>, List<FieldInfo>> _fields = new Dictionary<Func<FieldInfo, bool>, List<FieldInfo>>();
        private static Dictionary<Func<PropertyInfo, bool>, List<PropertyInfo>> _properties = new Dictionary<Func<PropertyInfo, bool>, List<PropertyInfo>>();

        public static IReadOnlyList<MethodInfo> GetMethods(Func<MethodInfo, bool> condition)
        {
            if (_methods.ContainsKey(condition) == true)
            {
                return _methods[condition];
            }

            _methods.Add(condition, typeof(T).GetMethods().Where(m => condition(m) == true).ToList());
            return _methods[condition];
        }

        public static IReadOnlyList<FieldInfo> GetFields(Func<FieldInfo, bool> condition)
        {
            if (_fields.ContainsKey(condition) == true)
            {
                return _fields[condition];
            }

            _fields.Add(condition, typeof(T).GetFields().Where(m => condition(m) == true).ToList());
            return _fields[condition];
        }

        public static IReadOnlyList<PropertyInfo> GetProperties(Func<PropertyInfo, bool> condition)
        {
            if (_properties.ContainsKey(condition) == true)
            {
                return _properties[condition];
            }

            _properties.Add(condition, typeof(T).GetProperties().Where(m => condition(m) == true).ToList());
            return _properties[condition];
        }
    }

    public static class TypeInfoHolderHelper
    {
        private static Type _typeInfoHolderType = typeof(TypeInfoHolder<>);
        static TypeInfoHolderHelper()
        {
            
        }

        public static IReadOnlyList<MethodInfo> GetMethods(Type type, Func<MethodInfo, bool> condition)
        {
            return (IReadOnlyList<MethodInfo>) _typeInfoHolderType.MakeGenericType(type).GetMethod("GetMethods")
                .Invoke(null, new[] {condition});
        }

        public static IReadOnlyList<FieldInfo> GetFields(Type type, Func<FieldInfo, bool> condition)
        {
            return (IReadOnlyList<FieldInfo>) _typeInfoHolderType.MakeGenericType(type).GetMethod("GetFields")
                .Invoke(null, new[] {condition});
        }

        public static IReadOnlyList<PropertyInfo> GetProperties(Type type, Func<PropertyInfo, bool> condition)
        {
            return (IReadOnlyList<PropertyInfo>) _typeInfoHolderType.MakeGenericType(type).GetMethod("GetProperties")
                .Invoke(null, new[] {condition});
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    public class ConstructorBenchmark
    {
        public class Foo
        {
            public string StringMember;
            public int IntMember;
            public Tuple<int, string, bool> TupleMember;
            public ValueTuple<int, string, bool> ValueTupleMember;
        }

        [Params(10000)]
        public int N;

        [Benchmark]
        public void CreateInstance()
        {
            Foo variable = null;
            for (int i = 0; i < N; ++i)
            {
                variable = new Foo();
            }
        }

        [Benchmark]
        public void CreateInstance_Activator()
        {
            Foo variable = null;
            for (int i = 0; i < N; ++i)
            {
                variable = Activator.CreateInstance<Foo>();
            }
        }

        [Benchmark]
        public void CreateInstance_Expression()
        {
            ConstructorInfo ctor = typeof(Foo).GetConstructors().First();
            ObjectActivator<Foo> createdActivator = GetActivator<Foo>(ctor);

            Foo variable = null;
            for (int i = 0; i < N; ++i)
            {
                variable = createdActivator.Invoke();
            }
        }

        // from https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/
        public delegate T ObjectActivator<T>(params object[] args);
        public static ObjectActivator<T> GetActivator<T>
            (ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();                  

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");
 
            Expression[] argsExp =
                new Expression[paramsInfo.Length];            

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;              

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);              

                Expression paramCastExp =
                    Expression.Convert (paramAccessorExp, paramType);              

                argsExp[i] = paramCastExp;
            }                  

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor,argsExp);                  

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);              

            //compile it
            ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace POC.Storage
{
    class Activator
    {
        private static Dictionary<string, Type> s_typeCache = new Dictionary<string, Type>();
        private static Dictionary<Type, ObjectActivator> s_activatorCache = new Dictionary<Type, ObjectActivator>();

        public static ObjectActivator GetActivator(string assemblyQualifiedName, Type[] constructorParameterTypes)
        {
            if (!s_typeCache.ContainsKey(assemblyQualifiedName))
            {
                s_typeCache[assemblyQualifiedName] = Type.GetType(assemblyQualifiedName);
            }
            return GetActivator(s_typeCache[assemblyQualifiedName], constructorParameterTypes);
        }

        public static ObjectActivator GetActivator(Type type, Type[] constructorParameterTypes)
        {
            if (!s_activatorCache.ContainsKey(type))
            {
                var ctor = type.GetConstructor(constructorParameterTypes);
                var act = GetActivator(ctor);
                s_activatorCache[type] = act;
            }
            return s_activatorCache[type];
        }


        // http://grantbyrne.com/post/activatorcreateinstancealternativetesting/
        // https://rogerjohansson.blog/2008/02/28/linq-expressions-creating-objects/

        internal delegate object ObjectActivator(params object[] args);

        private static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            var param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;
                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            var newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            var lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            var compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }
    }
}

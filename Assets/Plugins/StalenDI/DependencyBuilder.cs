using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Stalen.DI
{
    public struct DependencyBuilder<T>
    {
        public T TargetObject { get; }


        public DependencyBuilder(T targetObject)
        {
            TargetObject = targetObject;
        }


        public DependencyBuilder<T> To<TService>(Expression<Func<T, TService>> injector) where TService: class
        {
            var service = Containers.GetService<TService>();

            var member = (MemberExpression)injector.Body;
            if (member.Member is PropertyInfo property)
            {
                property.SetValue(TargetObject, service);
            }

            return this;
        }
    }
}
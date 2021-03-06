﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace King.David.Consulting.Common.Extensions
{
    public static class AttributeExtensions
    {
        public static IEnumerable<Attribute> GetAttributes<T>(this T obj)
        {
            var type = typeof(T);
            return type.GetTypeInfo().GetCustomAttributes(true);
        }

        public static IEnumerable<Attribute> GetAttributesOn<T>(this T obj, Expression<Func<T, object>> expression)
        {
            IEnumerable<Attribute> attributes = new List<Attribute>();

            var callExpression = expression.Body as MethodCallExpression;
            if (callExpression != null)
                attributes = callExpression.Method.GetCustomAttributes(true);

            var body = expression.Body as UnaryExpression;
            var memberExpression = body?.Operand as MemberExpression;
            if (memberExpression != null)
                attributes = memberExpression.Member.GetCustomAttributes(true);

            return attributes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DatabaseAccess
{
    public static class ExpressionParser
    {
        private static readonly Dictionary<ExpressionType, string> _mapping = new Dictionary<ExpressionType, string>
        {
            { ExpressionType.And, " AND " },
            { ExpressionType.AndAlso, " AND " },
            { ExpressionType.GreaterThan, " > " },
            { ExpressionType.GreaterThanOrEqual, " >= " },
            { ExpressionType.LessThan, " < " },
            { ExpressionType.LessThanOrEqual, " <= " },
            { ExpressionType.Equal, " = " },
            { ExpressionType.NotEqual, " <> " }
        };

        private static string _template = "{0} {1} {2}";

        public static string GetWhereClause(BinaryExpression e)
        {
            string middle = _mapping[e.NodeType];
            string left = GetExpressionMember(e.Left);
            string right = GetExpressionMember(e.Right);

            return string.Format(_template, left, middle, right);
        }

        private static string GetExpressionMember(Expression e)
        {
            string value = string.Empty;
            var valueConstant = e as ConstantExpression;
            if (valueConstant != null)
            {
                return valueConstant.Value.ToString();
            }

            var valueMember = e as MemberExpression;
            if (valueMember != null)
            {
                return GetValue(valueMember);
            }

            var valueExpression = e as BinaryExpression;
            if (valueExpression != null)
            {
                return GetWhereClause(valueExpression);
            }

            var valueParameter = e as ParameterExpression;
            if (valueParameter != null)
            {
                return _mapping[ExpressionType.Parameter];
            }

            var valueUnary = e as UnaryExpression;
            if (valueUnary != null)
            {
                var method = valueUnary.Operand as MethodCallExpression;
                if (method == null)
                {
                    return value;
                }

                var hasParameters = method.Arguments.Any(a => a.NodeType == ExpressionType.Parameter);

                if (hasParameters)
                {
                    throw new ArgumentException("Wrong condition");
                }

                return Expression.Lambda(method).Compile().DynamicInvoke().ToString();
            }

            return value;
        }

        public class ParsedExpression
        {
            public string Value { get; set; }

            public ExpressionType Type { get; set; }
        }

        private static string GetValue(MemberExpression member)
        {
            DataColumnAttribute attribute = (DataColumnAttribute)member.Member.GetCustomAttribute(typeof(DataColumnAttribute));
            if (attribute != null)
            {
                return attribute.Column;
            }

            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            Func<object> getter = getterLambda.Compile();
            return getter().ToString();
        }
    }
}

using System;
using System.Linq.Expressions;

namespace Expenses.WPF
{
    internal static class Utilities
    {
        public static string GetPropertyName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        internal static bool ValidateRequiredString(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        internal static bool ValidateStringLength(string value, int maximumLength)
        {
            if (value == null) { return true; }
            return (value.Length <= maximumLength);
        }

        internal static bool ValidatePositive(decimal value)
        {
            return (value > 0);
        }
    }
}

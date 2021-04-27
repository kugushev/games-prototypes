using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;
using AssertionMethod = JetBrains.Annotations.AssertionMethodAttribute;
using NotNull = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Kugushev.Scripts.Common.Utils
{
    public static class Asserting
    {
        [AssertionMethod]
        public static void NotNull([NotNull] object? obj, [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj, nameof(obj), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6, [NotNull] object? obj7,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
            AssertNotNull(obj7, nameof(obj7), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6, [NotNull] object? obj7,
            [NotNull] object? obj8,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
            AssertNotNull(obj7, nameof(obj7), callerMember, callerLine);
            AssertNotNull(obj8, nameof(obj8), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6, [NotNull] object? obj7,
            [NotNull] object? obj8, [NotNull] object? obj9,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
            AssertNotNull(obj7, nameof(obj7), callerMember, callerLine);
            AssertNotNull(obj8, nameof(obj8), callerMember, callerLine);
            AssertNotNull(obj9, nameof(obj9), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6, [NotNull] object? obj7,
            [NotNull] object? obj8, [NotNull] object? obj9, [NotNull] object? obj10,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
            AssertNotNull(obj7, nameof(obj7), callerMember, callerLine);
            AssertNotNull(obj8, nameof(obj8), callerMember, callerLine);
            AssertNotNull(obj9, nameof(obj9), callerMember, callerLine);
            AssertNotNull(obj10, nameof(obj10), callerMember, callerLine);
        }

        [AssertionMethod]
        public static void NotNull([NotNull] object? obj1, [NotNull] object? obj2, [NotNull] object? obj3,
            [NotNull] object? obj4, [NotNull] object? obj5, [NotNull] object? obj6, [NotNull] object? obj7,
            [NotNull] object? obj8, [NotNull] object? obj9, [NotNull] object? obj10, [NotNull] object? obj11,
            [CallerMemberName] string callerMember = "",
            [CallerLineNumber] int callerLine = default)
        {
            AssertNotNull(obj1, nameof(obj1), callerMember, callerLine);
            AssertNotNull(obj2, nameof(obj2), callerMember, callerLine);
            AssertNotNull(obj3, nameof(obj3), callerMember, callerLine);
            AssertNotNull(obj4, nameof(obj4), callerMember, callerLine);
            AssertNotNull(obj5, nameof(obj5), callerMember, callerLine);
            AssertNotNull(obj6, nameof(obj6), callerMember, callerLine);
            AssertNotNull(obj7, nameof(obj7), callerMember, callerLine);
            AssertNotNull(obj8, nameof(obj8), callerMember, callerLine);
            AssertNotNull(obj9, nameof(obj9), callerMember, callerLine);
            AssertNotNull(obj10, nameof(obj10), callerMember, callerLine);
            AssertNotNull(obj11, nameof(obj11), callerMember, callerLine);
        }

        [AssertionMethod]
        private static void AssertNotNull([NotNull] object? obj, string parameterName, string callerMember,
            int callerLine)
        {
#nullable disable
            string message = obj != null
                ? "" // to avoid extra gc we create a message only if assertion alerts
                : $"Parameter {parameterName}, caller {callerMember} at {callerLine}";
            Assert.IsNotNull(obj, message);
        }
#nullable restore
    }
}
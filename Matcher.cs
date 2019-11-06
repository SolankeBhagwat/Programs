using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Matcher
{
    public static class CollectionMatcher<T>
    {
        //public static IEnumerable<T> Match(IEnumerable<T> expectedItems)
        //{
        //    return CollectionMatcher<IEnumerable<T>, T>.Match(expectedItems);
        //}

        public static bool Match(IEnumerable<T> actualItems, IEnumerable<T> expectedItems)
        {
            if (actualItems == null && expectedItems == null)
            {
                return true;
            }

            if (actualItems == null || expectedItems == null)
            {
                return false;
            }

            var actualItemsCount = actualItems.Count();
            var expectedItemsCount = expectedItems.Count();

            if (actualItemsCount != expectedItemsCount)
            {
                return false;
            }

            // Assumption is that the order should always be same in actual and expected. Hence we directly compared the values.
            for (int i = 0; i < actualItemsCount; i++)
            {
                var actual = actualItems.ElementAt(i);
                var expected = expectedItems.ElementAt(i);

                var match = PropertyMatcher<T>.Match(actual, expected);
                if (!match)
                {
                    return false;
                }
            }

            return true;
        }
    }

    //public static class CollectionMatcher<TCollection, T> where TCollection : IEnumerable<T>
    //{
    //    public static TCollection Match(TCollection expectedItems)
    //    {
    //        return Moq.Match.Create<TCollection>(actualItems =>
    //        {
    //            if (actualItems == null && expectedItems == null)
    //                return true;

    //            if (actualItems == null || expectedItems == null)
    //                return false;

    //            var num1 = actualItems.Count();
    //            var num2 = expectedItems.Count();

    //            if (num1 != num2)
    //                return false;

    //            for (var index = 0; index < num1; ++index)
    //            {
    //                if (!PropertyMatcher<T>.Match(actualItems.ElementAt<T>(index), expectedItems.ElementAt<T>(index)))
    //                    return false;
    //            }
    //            return true;
    //        });
    //    }
    //}

    public static class PropertyMatcher<T>
    {
        //public static T Match(T expected)
        //{
        //    return Moq.Match.Create<T>(actual => Match(actual, expected));
        //}

        public static bool Match(T actual, T expected)
        {
            if (typeof(T).IsValueType)
                return Equals(actual, expected);

            if (actual == null != (expected == null))
                return false;

            if (actual == null)
                return true;

            foreach (var property in typeof(T).GetProperties().Where(p => p.GetIndexParameters().Length == 0))
            {
                if ((property.PropertyType == typeof(Collection<string>)))
                {
                    var sourceList = ((Collection<string>)property.GetValue(actual));
                    var targetList = ((Collection<string>)property.GetValue(expected));

                    if (sourceList == null && targetList == null)
                        return true;

                    if ((targetList == null || sourceList == null) || (sourceList.Count != targetList.Count))
                        return false;

                    for (int i = 0; i < sourceList.Count; i++)
                    {
                        if (!targetList.Contains(sourceList[i]))
                        {
                            return false;
                        }
                    }
                }
                else if (!(property.PropertyType == typeof(Collection<string>)))
                {
                    if (!Equals(property.GetValue(actual), property.GetValue(expected)))
                        return false;
                }
            }

            return true;
        }
    }
}

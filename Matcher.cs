using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Helpers
{
    public static class ExportCollectionMatcher<T> where T : class
    {
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

            var IsMatch = false;

            foreach (var actualItem in actualItems)
            {
                foreach (var expectedItem in expectedItems)
                {
                    if (ExportPropertyMatcher<T>.Match(actualItem, expectedItem))
                    {
                        IsMatch = true;
                        expectedItems.ToList().Remove(expectedItem);
                        break;
                    }
                }

                if (!IsMatch)
                {
                    return false;
                }
            }

            return true;
        }

    }

    public static class ExportPropertyMatcher<T> where T : class
    {
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
                    if (!Equals(property.GetValue(actual).ToString().Trim(), property.GetValue(expected).ToString().Trim()))
                        return false;
                }
            }

            return true;
        }
    }
}

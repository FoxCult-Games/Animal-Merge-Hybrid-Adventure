namespace FoxCultGames.Utilities
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class CollectionExtensions
    {
        public static T RandomItem<T>(this IEnumerable<T> collection)
        {
            T result = default;
            var totalElements = 0;

            foreach (var entry in collection)
            {
                totalElements++;
                if (Random.Range(0, totalElements) == 0)
                    result = entry;
            }

            return result;
        }
    }
}
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils
{
    public static class SubstitutiveRandom
    {
        private static float? _nextFloatRangeResult;

        public static void SubstituteNextRange(float result) => _nextFloatRangeResult = result;

        public static float Range(float minInclusive, float maxInclusive)
        {
            if (_nextFloatRangeResult == null)
                return Random.Range(minInclusive, maxInclusive);

            var result = _nextFloatRangeResult.Value;
            _nextFloatRangeResult = null;
            return result;
        }
    }
}
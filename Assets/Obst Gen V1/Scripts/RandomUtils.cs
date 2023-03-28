using UnityEngine;

namespace Obst_Gen_V1.Scripts
{
    public static class RandomUtils
    {
        /// <summary>
        /// Returns a random vector3 between min and max. (Inclusive)
        /// </summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// https://gist.github.com/Ashwinning/269f79bef5b1d6ee1f83
        private static Vector3 GetRandomVector3Between (Vector3 min, Vector3 max)
        {
            return min + Random.Range (0, 1) * (max - min);
        }

        /// <summary>
        /// Gets the random vector3 between the min and max points provided.
        /// Also uses minPadding and maxPadding (in metres).
        /// maxPadding is the padding amount to be added on the other Vector3's side.
        /// Setting minPadding and maxPadding to 0 will make it return inclusive values.
        /// </summary>
        /// <returns>The <see cref="UnityEngine.Vector3"/>.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// <param name="minPadding">Minimum padding.</param>
        /// <param name="maxPadding">Max padding.</param>
        /// https://gist.github.com/Ashwinning/269f79bef5b1d6ee1f83
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max, float minPadding, float maxPadding)
        {
            //minpadding as a value between 0 and 1
            float distance = Vector3.Distance(min, max);
            Vector3 point1 = min + minPadding * (max - min);
            Vector3 point2 = max + maxPadding * (min - max);
            return GetRandomVector3Between(point1, point2);
        }
    }
}

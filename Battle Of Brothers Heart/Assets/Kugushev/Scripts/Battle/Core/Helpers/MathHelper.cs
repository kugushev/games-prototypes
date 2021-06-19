using UnityEngine;

namespace Kugushev.Scripts.Battle.Core.Helpers
{
    public class MathHelper
    {
        public static int FindCircleCircleIntersections(Vector2 c0, float r0, Vector2 c1, float r1,
            out Vector2 intersection1,
            out Vector2 intersection2)
        {
            // Find the distance between the centers.
            float dx = c0.x - c1.x;
            float dy = c0.y - c1.y;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);

            if (Mathf.Abs(dist - (r0 + r1)) < 0.00001)
            {
                intersection1 = Vector2.Lerp(c0, c1, r0 / (r0 + r1));
                intersection2 = intersection1;
                return 1;
            }

            // See how many solutions there are.
            if (dist > r0 + r1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }

            if (dist < Mathf.Abs(r0 - r1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }

            if ((dist == 0) && (r0 == r1))
            {
                // No solutions, the circles coincide.
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }

            // Find a and h.
            float a = (r0 * r0 -
                r1 * r1 + dist * dist) / (2 * dist);
            float h = Mathf.Sqrt(r0 * r0 - a * a);

            // Find P2.
            float cx2 = c0.x + a * (c1.x - c0.x) / dist;
            float cy2 = c0.y + a * (c1.y - c0.y) / dist;

            // Get the points P3.
            intersection1 = new Vector2(
                cx2 + h * (c1.y - c0.y) / dist,
                cy2 - h * (c1.x - c0.x) / dist);
            intersection2 = new Vector2(
                cx2 - h * (c1.y - c0.y) / dist,
                cy2 + h * (c1.x - c0.x) / dist);

            return 2;
        }
    }
}
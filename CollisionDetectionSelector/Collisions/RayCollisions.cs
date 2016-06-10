using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class RayCollisions {
        public static float Max(float value, float max) {
            float result = value;
            if (result > max) {
                result = max;
            }
            return result;
        }
        public static bool PointOnRay(Point p, Ray r) {
            //point and ray same then return true
            Vector3 newNorm = new Vector3(p.X - r.Position.X, p.Y - r.Position.Y, p.Z - r.Position.Z);
            newNorm.Normalize();

            float d = Vector3.Dot(newNorm, r.Normal);

            return Math.Abs(1f - d) < 0.000001f;//use really small epsilon
        }
        public static Point ClosestPoint(Ray r, Point c) {
            //t is local, no need to pass it out of
            float t = 0f;
            //construct line segment out of ray
            Point rNormPos = new Point(r.Normal.X + r.Position.X,
                                        r.Normal.Y + r.Position.Y,
                                        r.Normal.Z + r.Position.Z);
            Line ab = new Line(r.Position, rNormPos);

            Vector3 a = new Vector3(r.Position.X, r.Position.Y, r.Position.Z);
            Vector3 b = new Vector3(rNormPos.X, rNormPos.Y, rNormPos.Z);

            t = Vector3.Dot(c.ToVector() - a, ab.ToVector()) / Vector3.Dot(ab.ToVector(), ab.ToVector());

            t = Max(t, 0f);

            Point d = new Point(a + (r.Normal * t));

            return d;
        }
    }
}

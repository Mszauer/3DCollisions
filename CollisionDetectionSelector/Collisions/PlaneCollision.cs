using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class PlaneCollision {
        public static bool PointOnPlane(Point unknown,Plane p) {
            //iff Vector3.Dot(unknow,normal(of plane)) - Distance == 0
            //iff ax+by+cz-d=result
            //0 == on plane, + = above plane, - = below plane
            float result = Vector3.Dot((new Vector3(unknown.X, unknown.Y, unknown.Z)), p.Normal) - p.Distance;
            return Math.Abs(0f - result) < 0.00001f; //essentially 0
        }
        public static float DistanceFromPlane(Point unknown, Plane p) {
            //Vector3.Dot(normal,unknown) = plane.distance
            float result = Vector3.Dot((new Vector3(unknown.X, unknown.Y, unknown.Z)), p.Normal) - p.Distance;
            return result;
        }
        public static Point ClosestPoint(Plane p, Point unknown,bool normalized = true) {
            float distance = Vector3.Dot((new Vector3(unknown.X, unknown.Y, unknown.Z)), p.Normal) - p.Distance;
            if (!normalized) {
                distance = distance / (Vector3.Dot(p.Normal, p.Normal));
            }
            Point result = new Point(p.Normal * distance);
            unknown.X -= result.X;
            unknown.Y -= result.Y;
            unknown.Z -= result.Z;
            return unknown;

        }
        public static Point IntersectPlanes(Plane p1, Plane p2, Plane p3) {
            Vector3 m1 = new Vector3(p1.Normal.X, p2.Normal.X, p3.Normal.X);
            Vector3 m2 = new Vector3(p1.Normal.Y, p2.Normal.Y, p3.Normal.Z);
            Vector3 m3 = new Vector3(p1.Normal.Z, p2.Normal.Z, p3.Normal.Z);
            Vector3 d = new Vector3(p1.Distance, p2.Distance, p3.Distance);

            Vector3 u = Vector3.Cross(m2, m3);
            Vector3 v = Vector3.Cross(m1, d);


            float denom = Vector3.Dot(m1, u);

            if (Math.Abs(denom) < 0.00001f) {
                throw new SystemException();
                return new Point(0, 0, 0);
            }

            return new Point(
                Vector3.Dot(d, u) / denom,
                Vector3.Dot(m3, v) / denom,
                -Vector3.Dot(m2, v) / denom
            );
        }
    }
}

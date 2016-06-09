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
    }
}

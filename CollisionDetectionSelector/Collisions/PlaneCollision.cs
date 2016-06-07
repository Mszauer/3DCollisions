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
        public static float DistanceFromPlane(Point point, Plane plane) {
            float dist = 0f;
            //Vector3.Dot(point, normal(of plane)) = distance
            dist = Vector3.Dot(new Vector3(point.X, point.Y, point.Z), plane.Normal);
            return dist;
        }
    }
}

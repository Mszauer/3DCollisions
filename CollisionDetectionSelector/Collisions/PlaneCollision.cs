using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class PlaneCollision {
        public bool PointOnPlane(Point a, Point inside,Plane p) {
            //iff Vector3.Dot(a-inside,P) = 0
            if (Vector3.Dot((new Vector3(a.X,a.Y,a.Z)-new Vector3(inside.X, inside.Y, inside.Z)),p.Normal) == 0){
                return true;
            }
            return false;
        }
        public Point ClosestPoint(Point a,Plane p) {
            // p.A*x1+p.B*y1+p.Z*1+d / lengthsquared(a)
            float distance = ((p.Normal.X * a.X) + (p.Normal.Y * a.Y) + (p.Normal.Z * a.Z))/*squared because bottom is too?*/ / Vector3.LengthSquared(p.Normal);

            //return point?
            return result;
        }
    }
}

using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class SphereCollisions {
        public static bool PointInSphere(Sphere sphere, Point p) {
            Vector3 dist = sphere.vPosition - p.Position;
            float rSq = sphere.Radius * sphere.Radius;
            float distSq = Vector3.LengthSquared(dist);
            return rSq > distSq;
        }
        public static Point ClosestPoint(Sphere sphere, Point p) {
            //subtract point from center of sphere
            //order matters
            Vector3 dist =  p.Position- sphere.vPosition;
            //normalize vector
            dist = Vector3.Normalize(dist);
            //multiply by radius
            dist *= sphere.Radius;
            //points from center to edge of sphere now
            //add position of sphere to new vector
            dist += sphere.vPosition;
            //now this points from origin to edge of circle
            return new Point(dist);
        } 
    }
}

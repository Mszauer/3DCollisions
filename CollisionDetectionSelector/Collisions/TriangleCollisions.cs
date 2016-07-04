using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class TriangleCollisions {
        //Same Side Technique
        public static bool PointInTriangle(Point _p, Triangle t) {
            //locals for easier math
            Vector3 p = new Vector3(_p.X, _p.Y, _p.Z);
            Vector3 a = new Vector3(t.p0.X, t.p0.Y, t.p0.Z);
            Vector3 b = new Vector3(t.p1.X, t.p1.Y, t.p1.Z);
            Vector3 c = new Vector3(t.p2.X, t.p2.Y, t.p2.Z);

            //transform unknown point into triangles origin
            a -= p;
            b -= p;
            c -= p;

            // The point should be moved too, so they are both
            // relative, but because we don't use p in the
            // equation anymore, we don't need it!
            // p -= p;

            //compute normal vectors for triangles
            //u = normal of PBC
            //v = normal of PCA
            //w = normal of PAB
            Vector3 u = Vector3.Cross(b, c);
            Vector3 v = Vector3.Cross(c, a);
            Vector3 w = Vector3.Cross(a, b);

            //test to see if normals are facing same direction
            if (Vector3.Dot(u, v) < 0f) {
                return false;
            }
            if (Vector3.Dot(u, w) < 0.0f) {
                return false;
            }

            //normals face same way
            return true;
        }
        public static bool PointInTriangle(Triangle t,Point p) {
            //utility
            return PointInTriangle(p, t);
        }
        
        public static Point ClosestPointTriangle(Point _p,Triangle t) {
            Point point = new Point(_p.X, _p.Y, _p.Z);
            //create plane out fo triangle
            Plane plane = new Plane(t.p0, t.p1, t.p2);

            //get closest point to plane
            point = Collisions.PlaneCollision.ClosestPoint(plane, point);

            if (PointInTriangle(t, point)) {
                //if point is in triangle, return it
                return new Point(point.X, point.Y, point.Z);
            }

            //break triangle down into Line components
            Line AB = new Line(t.p0, t.p1);
            Line BC = new Line(t.p1, t.p2);
            Line CA = new Line(t.p2, t.p0);

            //find closest point to each line
            Point c1 = Collisions.LineCollisions.ClosestPoint(AB, point);
            Point c2 = Collisions.LineCollisions.ClosestPoint(BC, point);
            Point c3 = Collisions.LineCollisions.ClosestPoint(CA, point);

            //mag is magnitudeSquared. Magnitude to unknown point
            float mag1 = (point.ToVector() - c1.ToVector()).LengthSquared();
            float mag2 = (point.ToVector() - c2.ToVector()).LengthSquared();
            float mag3 = (point.ToVector() - c3.ToVector()).LengthSquared();

            //find smallest magnitude(shortest distance)
            float min = System.Math.Min(mag1, mag2);
            min = System.Math.Min(min, mag3);

            if (min== mag1) {
                return c1;
            }
            else if (min == mag2) {
                return c2;
            }
            return c3;
        }
        public static Point ClosestPointTriangle(Triangle t, Point p) {
            return ClosestPointTriangle(p, t);
        }
    }
}

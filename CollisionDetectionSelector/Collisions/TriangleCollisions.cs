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
        
        
    }
}

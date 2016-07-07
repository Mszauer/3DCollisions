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
        public static bool SphereIntersect(Triangle triangle, Sphere sphere) {
            //get closest point on triangle to center of sphere
            Point p = ClosestPointTriangle(sphere.Position, triangle);

            //check distanceSq between center and point on triangle
            float distSq = Vector3.LengthSquared(sphere.vPosition - p.ToVector());

            //if distance is < r2 then there is a collision
            if (distSq < (sphere.Radius * sphere.Radius)) {
                return true;
            }

            return false;
        }

        public static bool SphereIntersect(Sphere sphere, Triangle triangle) {
            return SphereIntersect(triangle, sphere);
        }

        public static bool AABBIntersect(Triangle triangle, AABB aabb) {
            //get triangle corners as vectors
            Vector3[] v = new Vector3[3] { triangle.p0.ToVector(),
                                          triangle.p1.ToVector(),
                                          triangle.p2.ToVector() };

            //convert aabb to center-extents
            Vector3 center = aabb.Center.ToVector();
            Vector3 extent = aabb.Extents;

            //translate triangle so aabb is center of world
            for (int i = 0; i < 3; i++) {
                v[i] -= center;
            }

            Vector3[] f = new Vector3[3] { v[1] - v[0]/*A-B */, v[2] - v[1]/*B-C */, v[0] - v[2] /*A-C */};

            //find face normals of aabb (normals are xyz axis
            Vector3[] u = new Vector3[3] { new Vector3(1.0f,0.0f,0.0f),
                                           new Vector3(0.0f,1.0f,0.0f),
                                           new Vector3(0.0f,0.0f,1.0f)};

            //create all possible axis
            //u=face normals of AABB
            //f = edge vectors of triangle ABC
            for (int _u = 0; _u < u.Length; _u++) {
                for(int _f = 0; _f < f.Length; _f++) {
                    Vector3 testAxis = Vector3.Cross(u[_u], f[_f]);
                    //Test SAT
                    if (!TriangleAABBSat(v, u, extent, testAxis)) {
                        return false;
                    }
                }
               
                // for these tests we are conceptually checking if the bounding box
                // of the triangle intersects the bounding box of the AABB
                // that is to say, the seperating axis for all tests are axis aligned:
                // axis1: (1, 0, 0), axis2: (0, 1, 0), axis3 (0, 0, 1)
                // Do the SAT given the 3 primary axis of the AABB
                if (!TriangleAABBSat(v, u, extent, u[_u])) {
                    return false;
                }
            }
            
            // Finally, we have one last axis to test, the face normal of the triangle
            // We can get the normal of the triangle by crossing the first two line segments
            Vector3 triangleNormal = Vector3.Cross(f[0], f[1]);
            if (!TriangleAABBSat(v, u, extent, triangleNormal)) {
                return false;
            }

            // Passed testing for all 13 seperating axis that exist!
            return true;
        }
        public static bool AABBIntersect(AABB aabb, Triangle triangle) {
            return AABBIntersect(triangle, aabb);
        }
        private static bool TriangleAABBSat(Vector3[] v, Vector3[] u,Vector3 extents,Vector3 testingAxii) {
            // Project all 3 vertices of the triangle onto the Seperating axis
            float p0 = Vector3.Dot(v[0], testingAxii);
            float p1 = Vector3.Dot(v[1], testingAxii);
            float p2 = Vector3.Dot(v[2], testingAxii);
            // Project the AABB onto the seperating axis
            // We don't care about the end points of the prjection
            // just the length of the half-size of the AABB
            // That is, we're only casting the extents onto the 
            // seperating axis, not the AABB center. We don't
            // need to cast the center, because we know that the
            // aabb is at origin compared to the triangle!
            float r = extents.X * Math.Abs(Vector3.Dot(u[0], testingAxii)) +
                        extents.Y * Math.Abs(Vector3.Dot(u[1], testingAxii)) +
                        extents.Z * Math.Abs(Vector3.Dot(u[2], testingAxii));
            // Now do the actual test, basically see if either of
            // the most extreme of the triangle points intersects r
            // You might need to write Min & Max functions that take 3 arguments
            if (System.Math.Max(-(System.Math.Max(System.Math.Max(p0, p1), p2)), System.Math.Min(System.Math.Min(p0, p1), p2)) > r) {
                // This means BOTH of the points of the projected triangle
                // are outside the projected half-length of the AABB
                // Therefore the axis is seperating and we can exit
                return false;
            }
            return true;
        }

        public static bool PlaneTriangleIntersection(Triangle triangle, Plane plane) {
            //test each point of triangle against plane
            float aDist = Collisions.PlaneCollision.DistanceFromPlane(triangle.p0, plane);
            float bDist = Collisions.PlaneCollision.DistanceFromPlane(triangle.p1, plane);
            float cDist = Collisions.PlaneCollision.DistanceFromPlane(triangle.p2, plane);

            //if all 3 have same sign, no collision
            if (aDist < 0 && bDist < 0 && cDist < 0) {
                return false;
            }
            else if (aDist > 0 && bDist > 0 && cDist > 0) {
                return false;
            }
            //if all 3 distances are 0 there is a collision, lies on the plane
            if (System.Math.Abs(aDist) <= 0.0001f && System.Math.Abs(bDist) <= 0.0001f && System.Math.Abs(cDist) <= 0.0001f) {
                return true;
            }
            //otherwise at least one point is on other side of plane
            return true;
        }
        public static bool PlaneTriangleIntersection(Plane plane,Triangle triangle) {
            return PlaneTriangleIntersection(triangle, plane);
        }


        public static bool TriangleTriangleIntersection(Triangle t1,Triangle t2) {
            Vector3[] t1Edges = new Vector3[3] { t1.p1.ToVector() - t1.p0.ToVector(),
                                                 t1.p2.ToVector() - t1.p1.ToVector(),
                                                 t1.p0.ToVector() - t1.p2.ToVector() };

            Vector3[] t2Edges = new Vector3[3] {t2.p1.ToVector()-t2.p0.ToVector(),
                                                t2.p2.ToVector()-t2.p1.ToVector(),
                                                t2.p0.ToVector()-t2.p2.ToVector()};
            //test 11axis
            //face normal of t1
            Vector3 t1FaceNormal = Vector3.Cross(t1Edges[0], t1Edges[1]);
            if (TestAxis(t1, t2, t1FaceNormal)) {
                //seperating axis found
                return true;
            }

            //face normal of t2
            Vector3 t2FaceNormal = Vector3.Cross(t2Edges[0], t2Edges[1]);
            if (TestAxis(t1, t2, t2FaceNormal)) {
                //seperating axis found
                return true;
            }

            //cross products of each of the 3 edges
            for (int i = 0; i < 3; i++) {
                Vector3 testAxis = Vector3.Cross(t1Edges[i], t2Edges[i]);
                if (TestAxis(t1, t2, testAxis)) {
                    //seperating axis found
                    return true;
                }
            }

            //no seperating axis found, no intersection
            return false;
        }
        private static Vector2 GetInterval(Triangle triangle, Vector3 axis) {
            //normalize axis
            Vector2 interval = new Vector2();
            //interval.X = min
            //interval.Y = max
            interval.X = Vector3.Dot(axis, triangle.p0.ToVector());
            interval.Y = interval.X;

            float result = Vector3.Dot(axis,triangle.p1.ToVector());
            interval.X = System.Math.Min(interval.X, result);
            interval.Y = System.Math.Max(interval.Y, result);

            result = Vector3.Dot(axis, triangle.p2.ToVector());
            interval.X = System.Math.Min(interval.X, result);
            interval.Y = System.Math.Max(interval.Y, result);
            return interval;
        }
        private static bool TestAxis(Triangle triangle1, Triangle triangle2,Vector3 axis) {
            Vector3 axisNorm = new Vector3(axis.X,axis.Y,axis.Z);
            axisNorm.Normalize();
            Vector2 i1 = GetInterval(triangle1, axisNorm);
            Vector2 i2 = GetInterval(triangle2, axisNorm);

            if (i1.Y < i2.X /*i1.max < i2.min*/ || i2.Y < i1.X /*i2.max < i1.min*/) {
                //intervals overlap on given axis
                return true;
            }
            return false;//no collision
        }
    }
}

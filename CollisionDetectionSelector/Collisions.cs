using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

class Collisions {
    #region AABB
    public static bool PointInAABB(AABB aabb, Point point) {
        bool passed = true;
        if (!(point.X < aabb.Min.X) && !(aabb.Min.X < point.X)) {
            passed = false;
        }
        if (!(point.Y < aabb.Min.Y) && !(aabb.Min.Y < point.Y)) {
            passed = false;
        }
        if (!(point.Z < aabb.Min.Z) && !(aabb.Min.Z < point.Z)) {
            passed = false;
        }
        return passed;
    }

    public static Point ClosestPoint(AABB aabb, Point point) {
        Point result = new Point(point);
        if (point.X > aabb.Max.X) {
            //outside box towards max
            result.X = aabb.Max.X;
        }
        else if (point.X < aabb.Min.X) {
            //outside box toawrds min
            result.X = aabb.Min.X;
        }
        if (point.Y > aabb.Max.Y) {
            result.Y = aabb.Max.Y;
        }
        else if (point.Y < aabb.Min.Y) {
            result.Y = aabb.Min.Y;
        }
        if (point.Z > aabb.Max.Z) {
            result.Z = aabb.Max.Z;
        }
        else if (point.Z < aabb.Min.Z) {
            result.Z = aabb.Min.Z;
        }
        return result;
    }
    #endregion
    #region Line
    public static float Clamp(float value, float max, float min) {
        float result = value;
        if (result > max) {
            result = max;
        }
        else if (result < min) {
            result = min;
        }
        return result;
    }
    public static bool PointOnLine(Point p, Line line) {
        float m = (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);//rise over run
        float b = line.Start.Y - m * line.Start.X;

        //at this point, evaluation equation is:
        //return point.y==m*p.x+b
        //wont work because we use floats
        //floating error can be accumulated, episilon testing
        if (Math.Abs(p.Y - (m * p.X + b)) < 0.0001f) {
            return true;
        }
        return false;
    }
    public static Point ClosestPoint(Line ab, Point c, out float t) {
        Vector3 a = new Vector3(ab.Start.X, ab.Start.Y, ab.Start.Z);
        Vector3 b = new Vector3(ab.End.X, ab.End.Y, ab.End.Z);

        //project c onto b, then find the
        //parametrized position d(t) = a+t*(b-a)
        //t = Dot(c - a, ab) / Dot(ab, ab);
        t = Vector3.Dot((c.ToVector() - a), ab.ToVector()) / Vector3.Dot(ab.ToVector(), ab.ToVector());

        //clamp t to 0-1 range
        //if t is outside range, it is outside line
        t = Clamp(t, 1f, 0f);

        //compute project position from clamped t
        Point distance = new Point(a + (ab.ToVector() * t));

        //return result
        return distance;
    }
    public static Point ClosestPoint(Line ab, Point c) {
        float t = 0f;
        return ClosestPoint(ab, c, out t);
    }

    #endregion
    #region Plane
    public static bool PointOnPlane(Point unknown, Plane p) {
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
    public static Point ClosestPoint(Plane p, Point unknown, bool normalized = true) {
        float distance = Vector3.Dot((new Vector3(unknown.X, unknown.Y, unknown.Z)), p.Normal) - p.Distance;
        if (!normalized) {
            distance = distance / (Vector3.Dot(p.Normal, p.Normal));
        }
        Point result = new Point(p.Normal * distance);

        return new Point(unknown.X - result.X, unknown.Y - result.Y, unknown.Z - result.Z);
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
    #endregion
    #region Ray
    public static float Max(float value, float max) {
        float result = value;
        if (result < max) {
            result = max;
        }
        return result;
    }
    public static bool PointOnRay(Point p, Ray r) {
        //point and ray same then return true
        Vector3 newNorm = new Vector3(p.X - r.Position.X, p.Y - r.Position.Y, p.Z - r.Position.Z);
        newNorm.Normalize();
        if (p.X == r.Position.X && p.Y == r.Position.Y && p.Z == r.Position.Z) {
            return true;
        }
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
    #endregion
    #region Sphere
    public static bool PointInSphere(Sphere sphere, Point p) {
        Vector3 dist = sphere.vPosition - p.Position;
        float rSq = sphere.Radius * sphere.Radius;
        float distSq = Vector3.LengthSquared(dist);
        return rSq > distSq;
    }
    public static Point ClosestPoint(Sphere sphere, Point p) {
        //subtract point from center of sphere
        //order matters
        Vector3 dist = p.Position - sphere.vPosition;
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
    #endregion
    #region Triangle
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
    public static bool PointInTriangle(Triangle t, Point p) {
        //utility
        return PointInTriangle(p, t);
    }

    public static Point ClosestPointTriangle(Point _p, Triangle t) {
        Point point = new Point(_p.X, _p.Y, _p.Z);
        //create plane out fo triangle
        Plane plane = new Plane(t.p0, t.p1, t.p2);

        //get closest point to plane
        point = ClosestPoint(plane, point);

        if (PointInTriangle(t, point)) {
            //if point is in triangle, return it
            return new Point(point.X, point.Y, point.Z);
        }

        //break triangle down into Line components
        Line AB = new Line(t.p0, t.p1);
        Line BC = new Line(t.p1, t.p2);
        Line CA = new Line(t.p2, t.p0);

        //find closest point to each line
        Point c1 = ClosestPoint(AB, point);
        Point c2 = ClosestPoint(BC, point);
        Point c3 = ClosestPoint(CA, point);

        //mag is magnitudeSquared. Magnitude to unknown point
        float mag1 = (point.ToVector() - c1.ToVector()).LengthSquared();
        float mag2 = (point.ToVector() - c2.ToVector()).LengthSquared();
        float mag3 = (point.ToVector() - c3.ToVector()).LengthSquared();

        //find smallest magnitude(shortest distance)
        float min = System.Math.Min(mag1, mag2);
        min = System.Math.Min(min, mag3);

        if (min == mag1) {
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

    public static bool TriangleAABBIntersect(Triangle triangle, AABB aabb) {
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
            for (int _f = 0; _f < f.Length; _f++) {
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
    public static bool TriangleAABBIntersect(AABB aabb, Triangle triangle) {
        return TriangleAABBIntersect(triangle, aabb);
    }
    private static bool TriangleAABBSat(Vector3[] v, Vector3[] u, Vector3 extents, Vector3 testingAxii) {
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
        float aDist = DistanceFromPlane(triangle.p0, plane);
        float bDist = DistanceFromPlane(triangle.p1, plane);
        float cDist = DistanceFromPlane(triangle.p2, plane);

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
    public static bool PlaneTriangleIntersection(Plane plane, Triangle triangle) {
        return PlaneTriangleIntersection(triangle, plane);
    }


    public static bool TriangleTriangleIntersection(Triangle t1, Triangle t2) {
        //test 11axis
        //face normal of t1
        if (TestAxis(t1, t2, t1.p1.ToVector(), t1.p0.ToVector(), t1.p2.ToVector(), t1.p1.ToVector())) {
            //seperating axis found
            return false;
        }

        //face normal of t2
        if (TestAxis(t1, t2, t2.p1.ToVector(), t2.p0.ToVector(), t2.p2.ToVector(), t2.p1.ToVector())) {
            //seperating axis found
            return false;
        }
        /*
        //Vector3[] t1Edges = new Vector3[3] { t1.p1.ToVector() - t1.p0.ToVector(),
        //                                     t1.p2.ToVector() - t1.p1.ToVector(),
        //                                     t1.p0.ToVector() - t1.p2.ToVector() };
        */
        //go through each of the edges (0x0,0x1,0x2,1x0,1x,1x2,2x0,2x1,2x2)
        if (TestAxis(t1, t2, t1.p1.ToVector(), t1.p0.ToVector(), t2.p1.ToVector(), t2.p0.ToVector())) { //0x0
            return false;
        }
        if (TestAxis(t1, t2, t1.p1.ToVector(), t1.p0.ToVector(), t2.p2.ToVector(), t2.p1.ToVector())) { //0x1
            return false;
        }
        if (TestAxis(t1, t2, t1.p1.ToVector(), t1.p0.ToVector(), t2.p0.ToVector(), t2.p2.ToVector())) { //0x2
            return false;
        }
        if (TestAxis(t1, t2, t1.p2.ToVector(), t1.p1.ToVector(), t2.p1.ToVector(), t2.p0.ToVector())) { //1x0
            return false;
        }
        if (TestAxis(t1, t2, t1.p2.ToVector(), t1.p1.ToVector(), t2.p2.ToVector(), t2.p1.ToVector())) { //1x1
            return false;
        }
        if (TestAxis(t1, t2, t1.p2.ToVector(), t1.p1.ToVector(), t2.p0.ToVector(), t2.p2.ToVector())) { //1x2
            return false;
        }
        if (TestAxis(t1, t2, t1.p0.ToVector(), t1.p2.ToVector(), t2.p1.ToVector(), t2.p0.ToVector())) { //2x0
            return false;
        }
        if (TestAxis(t1, t2, t1.p0.ToVector(), t1.p2.ToVector(), t2.p2.ToVector(), t2.p1.ToVector())) { //2x1
            return false;
        }
        if (TestAxis(t1, t2, t1.p0.ToVector(), t1.p2.ToVector(), t2.p0.ToVector(), t2.p2.ToVector())) { //2x2
            return false;
        }
        //no seperating axis found, no intersection
        return true;
    }
    private static Vector2 GetInterval(Triangle triangle, Vector3 axis) {
        //normalize axis
        Vector2 interval = new Vector2();
        //interval.X = min
        //interval.Y = max
        interval.X = Vector3.Dot(axis, triangle.p0.ToVector());
        interval.Y = interval.X;

        float result = Vector3.Dot(axis, triangle.p1.ToVector());
        interval.X = System.Math.Min(interval.X, result);
        interval.Y = System.Math.Max(interval.Y, result);

        result = Vector3.Dot(axis, triangle.p2.ToVector());
        interval.X = System.Math.Min(interval.X, result);
        interval.Y = System.Math.Max(interval.Y, result);
        return interval;
    }

    private static bool TestAxis(Triangle triangle1, Triangle triangle2, Vector3 a, Vector3 b, Vector3 c, Vector3 d) {
        Vector3 axis = Vector3.Cross(a - b, c - d);

        if (axis.LengthSquared() < 0.0001f) {
            //axis is zero, try other combination
            Vector3 n = Vector3.Cross(a - b, c - a);
            axis = Vector3.Cross(a - b, n);
            if (axis.LengthSquared() < 0.0001f) {
                //axis still zero, not seperating axis
                return false;
            }
        }
        Vector3 axisNorm = new Vector3(axis.X, axis.Y, axis.Z);
        axisNorm.Normalize();
        Vector2 i1 = GetInterval(triangle1, axisNorm);
        Vector2 i2 = GetInterval(triangle2, axisNorm);

        if (i1.Y < i2.X /*i1.max < i2.min*/ || i2.Y < i1.X /*i2.max < i1.min*/) {
            //intervals overlap on given axis
            return true;
        }
        return false;//no collision
    }

    public static bool RaycastTriangle(Ray ray, Triangle triangle, out float t) {
        //Break triangle into Vectors
        Line ab = new Line(triangle.p0, triangle.p1);
        Line bc = new Line(triangle.p1, triangle.p2);
        Line ca = new Line(triangle.p2, triangle.p0);

        if (!RaycastNoNormal(ray, new Plane(triangle.p0, triangle.p1, triangle.p2), out t)) {
            //no collision?
            return false;
        }
        Point i = new Point(ray.Position.ToVector() + ray.Normal * t);

        //v = orthogonal(perpendicular) line to BC, and passes through triangle.p0 / A
        //v = AB - projection(bc onto ab)
        Vector3 v = ab.ToVector() - Vector3.Cross(ab.ToVector(), bc.ToVector());

        //a = 1- (v dot ai / v dot ab)
        Line ai = new Line(triangle.p0, i);
        float a = 1 - (Vector3.Dot(v, ai.ToVector()) / Vector3.Dot(v, ab.ToVector()));
        //bounds check (0min 1 max)
        if (0.0f > a || a > 1.0f) {
            return false; //out of bounds
        }
        //v = orthogonal(perpendicular) line to CA, and passes through triangle.p1 / B
        //v = BC - projection(bc onto ca)
        v = bc.ToVector() - Vector3.Cross(bc.ToVector(), ca.ToVector());

        //b = 1 - (v dot bi / v dot bc)
        Line bi = new Line(triangle.p1, i);
        float b = 1 - (Vector3.Dot(v, bi.ToVector()) / Vector3.Dot(v, bc.ToVector()));
        //bounds check (0min 1 max)
        if (0.0f > b || b > 1.0f) {
            return false; // out of bounds
        }

        //if a and b are between 0 and 1, then c will always be too
        return true;
    }
    // Conveniance method, returns t without an out param
    // If no collision happened, will return -1
    public static float RaycastTriangle(Ray ray, Triangle triangle) {
        float t = -1;
        if (!RaycastTriangle(ray, triangle, out t)) {
            return -1;
        }
        return t;
    }

    // Conveniance method, returns the point of intersection
    public static bool RaycastTriangle(Ray ray, Triangle triangle, out Point p) {
        float t = -1;
        bool result = RaycastTriangle(ray, triangle, out t);
        p = new Point(ray.Position.ToVector() + ray.Normal * t);
        return result;
    }

    private static bool RaycastNoNormal(Ray ray, Plane plane, out float t) {
        float nd = Vector3.Dot(ray.Normal, plane.Normal);
        float pn = Vector3.Dot(ray.Position.ToVector(), plane.Normal);

        if (System.Math.Abs(0 - nd) == 0.0001f) {
            t = -1;
            return false;
        }
        t = (plane.Distance - pn) / nd;

        if (t >= 0f) {
            return true;
        }
        return false;
    }

    public static bool LinecastTriangle(Line line, Triangle triangle, out Point result) {
        Ray r = new Ray();
        r.Position = new Point(line.Start.X, line.Start.Y, line.Start.Z);
        r.Normal = line.End.ToVector() - line.Start.ToVector();

        float t = -1;
        if (!RaycastTriangle(r, triangle, out t)) {
            result = new Point(0f, 0f, 0f);
            return false;
        }
        if (t < 0) {
            result = new Point(line.Start.ToVector());
            return false;
        }
        if (t * t > line.LengthSq) {
            result = new Point(line.End.ToVector());
            return false;
        }
        result = new Point(r.Position.ToVector() + r.Normal * t);
        return true;
    }
    #endregion
    #region FRUSTUM
    private static float HalfSpaceTest(Plane plane, Point point) {
        return Vector3.Dot((new Vector3(point.X, point.Y, point.Z)), plane.Normal) + plane.Distance;

    }
    public static bool Intersects(Plane[] frustum, Point point) {
        //loop through all frustum planes and do half space
        //between the point and each plane
        foreach (Plane p in frustum) {
            //if any half space tests are <0 return false
            if (Math.Abs(HalfSpaceTest(p, point)) < 0.0001f) {
                return false;
            }
        }
        //return true by default
        return true;
    }
    public static bool Intersects(Point point,Plane[] frustum) {
        return Intersects(frustum, point);
    }
    #endregion
}
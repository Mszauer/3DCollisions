using Math_Implementation;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;

class LinesAndRays {
    #region HelperFunctions
    public static float Max(float a, float b) { return System.Math.Max(a, b); }
    public static float Min(float a, float b) { return System.Math.Min(a, b); }
    #endregion
    public static bool RaycastSphere(Ray ray, Sphere sphere, out float t) {
        //look at image to understand how to get each point
        //https://gdbooks.gitbooks.io/3dcollisions/content/Chapter3/raycast_image_4.png
        Vector3 p0 = new Vector3(ray.Position.X, ray.Position.Y, ray.Position.Z);
        Vector3 d = new Vector3(ray.Normal.X, ray.Normal.Y, ray.Normal.Z);
        Vector3 c = new Vector3(sphere.Position.X, sphere.Position.Y, sphere.Position.Z);
        float r = sphere.Radius;

        Vector3 e = c - p0;
        //length would cause floating point error
        float eSq = Vector3.LengthSquared(e);
        float a = Vector3.Dot(e, d);
        float b = (float)System.Math.Sqrt(eSq - (a * a));
        float f = (float)System.Math.Sqrt((r * r) - (b * b));

        //no collision
        if (((r * r) - eSq + (a * a)) < 0f) {
            t = -1;//-1 is invalid
            return false;
        }
        //ray is inside
        else if (eSq < (r * r)) {
            t =  a + f;//reverse direction
            if (t >= 0) {
                return true;
            }
            return false;
        }
        //else return normal intersection
        t = a - f;
        if (t >= 0) {
            return true;
        }
        return false;
    }
    public static float RaycastSphere(Ray ray, Sphere sphere) {
        float t = -1;
        if (!RaycastSphere(ray,sphere,out t)) {
            return -1;
        }
        return t;
    }
    public static bool RaycastSphere(Ray ray, Sphere sphere, out Point p) {
        float t = -1;
        bool result = RaycastSphere(ray, sphere, out t);
        p = new Point(ray.Position.ToVector() + ray.Normal*t);
        return result;
    }
    public static bool RaycastAABB(Ray ray, AABB aabb,out float t) {
        float t1 = (aabb.Min.X - ray.Position.X) / ray.Normal.X;
        float t2 = (aabb.Max.X - ray.Position.X) / ray.Normal.X;
        float t3 = (aabb.Max.Y - ray.Position.Y) / ray.Normal.Y;
        float t4 = (aabb.Min.Y - ray.Position.Y) / ray.Normal.Y;
        float t5 = (aabb.Max.Z - ray.Position.Z) / ray.Normal.Z;
        float t6 = (aabb.Min.Z - ray.Position.Z) / ray.Normal.Z;

        //find the biggest of the smallest
        float tmin = Max(Max(Min(t1, t2),Min(t3, t4)),Min(t5, t6));
        //find the smallest of the biggest
        float tmax = Min(Min(Max(t1, t2),Max(t3, t4)),Max(t5, t6));

        // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behing us
        if (tmax < 0) {
            t = -1;
            return false; //intersects before origin
        }
        // if tmin > tmax, ray doesn't intersect AABB
        if (tmin > tmax) {
            t = 1;
            return false;//no intersection
        }
        if (tmin < 0f) {
            t = tmax;
            if (t >= 0) {
                return true;
            }
        }
        t = tmin;
        if (t >= 0) {
            return true;
        }
        return false;
    }
    public static float RaycastAABB(Ray ray,AABB aabb) {
        float t = -1;
        if (!RaycastAABB(ray,aabb,out t)) {
            return -1;
        }
        return t;
    }
    public static bool RaycastAABB(Ray ray,AABB aabb, out Point p) {
        float t = -1;
        bool result = RaycastAABB(ray, aabb, out t);
        p = new Point(ray.Position.ToVector() + ray.Normal * t);
        return result;
    }
    public static bool RaycastPlane(Ray ray, Plane plane, out float t) {
        float nd = Vector3.Dot(ray.Normal, plane.Normal);
        float pn = Vector3.Dot(ray.Position.ToVector(), plane.Normal);

        if (nd >= 0f) {
            t = -1;
            return false;
        }
        t = (plane.Distance - pn) / nd;
        
        if (t >= 0f) {
            return true;
        }
        return false;
    }
    public static float RaycastPlane(Ray ray,Plane plane) {
        float t = -1;
        if (!RaycastPlane(ray,plane,out t)) {
            return -1;
        }
        return t;
    }
    public static bool RaycastPlane(Ray ray, Plane plane, out Point p) {
        float t = -1;
        bool result = RaycastPlane(ray, plane, out t);
        p = new Point(ray.Position.ToVector() + ray.Normal*t);
        return result;
    }
    public static bool LinecastSphere(Line line, Sphere sphere, out Point result) {
        Ray r = new Ray();
        r.Position = new Point(line.Start.X, line.Start.Y, line.Start.Z);
        //normal points from start to end, by value
        //the normal setter will automatically normalize this
        r.Normal = (line.End.ToVector() - line.Start.ToVector());

        //line case logic
        float t = -1;
        if (!RaycastSphere(r,sphere,out t)) { //this changes t
            //if raycast returns false the point was never on the line
            result = new Point(0f, 0f, 0f);
            return false;
        }
        //if t is < 0 , point is behind the start point
        if (t < 0) {
            //by value, call new point don't use refernece
            result = new Point(line.Start.ToVector());
            return false;
        }
        //if t is longer than length of line, intersection is after start point
        else if (t*t > line.LengthSq) {
            result = new Point(line.End.ToVector());
            return false;
        }
        // If we made it here, the line intersected the sphere
        result = new Point(r.Position.ToVector() + r.Normal * t);
        return true;

    }
    public static bool LinecastAABB(Line line, AABB aabb, out Point result) {
        //create ray out of line
        Ray r = new Ray();
        r.Position = new Point(line.Start.X, line.Start.Y, line.Start.Z);
        r.Normal = (line.End.ToVector() - line.Start.ToVector());

        //begin linecast logic
        float t = -1;
        if (!RaycastAABB(r,aabb, out t)) {
            //false = point was never in aabb
            result = new Point(0f, 0f, 0f);
            return false;
        }
        if (t < 0) { //behind start point
            result = new Point(line.Start.ToVector());
            return false;
        }
        else if (t*t > line.LengthSq) {//intersection after start point
            result = new Point(line.End.ToVector());
            return false;
        }
        //passed all tests
        result = new Point(r.Position.ToVector() + r.Normal * t);
        return true;

    }
}
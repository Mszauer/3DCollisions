using Math_Implementation;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;

class LinesAndRays {
    public static bool Raycast(Ray ray, Sphere sphere, out float t) {
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
            return -1;//-1 is invalid
        }
        //ray is inside
        else if (eSq < (r * r)) {
            return a + f;//reverse direction
        }
        //else return normal intersection
        return a - f;
    }
    public static float RayCast(Ray ray, Sphere sphere) {
        float t = -1;
        if (!Raycast(ray,sphere,out t)) {
            return -1;
        }
        return t;
    }
    public static bool Raycast(Ray ray, Sphere sphere, out Point p) {
        float t = -1;
        bool result = Raycast(ray, sphere, out t);
        p = new Point(ray.Position.ToVector() + ray.Normal*t);
        return result;
    }
}
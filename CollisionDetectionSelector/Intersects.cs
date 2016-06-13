using Math_Implementation;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;


class Intersects {
    public static bool SphereSphereIntersect(Sphere a, Sphere b) {
        //find squared distance
        Vector3 d = new Vector3(a.Position.X - b.Position.X,
                                a.Position.Y - b.Position.Y,
                                a.Position.Z - b.Position.Z);
        float distSq = Vector3.Dot(d, d); //same as .LengthSquared

        //intersection if distSq < sumRadii
        float radiiSum = a.Radius + b.Radius;
        float radiiSq = radiiSum * radiiSum;

        return distSq <= radiiSq; //less than or equal means intersection. 
    }
    public static bool SphereAABBIntersect(Sphere sphere,AABB aabb) {
        //find closest point
        Vector3 closestPoint = CollisionDetectionSelector.Collisions.AABBCollisions.ClosestPoint(aabb, sphere.Position).ToVector();
        Vector3 differenceVec = sphere.Position.ToVector() - closestPoint;

        float distSq = Vector3.LengthSquared(differenceVec);
        float radiusSq = sphere.Radius * sphere.Radius;

        return distSq < radiusSq;
    }
    public static bool SphereAABBIntersect(AABB aabb,Sphere sphere) {
        return SphereAABBIntersect(sphere, aabb);
    }
    public static bool SpherePlaneIntersect(Sphere sphere, Plane plane) {
        Vector3 closestPoint = CollisionDetectionSelector.Collisions.PlaneCollision.ClosestPoint(plane, sphere.Position).ToVector();
        Vector3 distance = sphere.Position.ToVector() - closestPoint;

        float distSq = Vector3.LengthSquared(distance);
        float radiusSq = sphere.Radius * sphere.Radius;

        return distSq <= radiusSq;
    }
    public static bool SpherePlaneIntersect(Plane plane, Sphere sphere) {
        return SpherePlaneIntersect(sphere, plane);
    }
}

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
        Point closest = CollisionDetectionSelector.Collisions.PlaneCollision.ClosestPoint(plane, sphere.Position);

        Vector3 closestPoint = closest.ToVector();

        Vector3 spherePos = sphere.Position.ToVector();

        Vector3 distance = spherePos - closestPoint;

        float distSq = Vector3.LengthSquared(distance);
        float radiusSq = sphere.Radius * sphere.Radius;

        return distSq <= radiusSq;
    }
    public static bool SpherePlaneIntersect(Plane plane, Sphere sphere) {
        return SpherePlaneIntersect(sphere, plane);
    }
    public static bool AABBAABBIntersect(AABB a,AABB b) {
        return
            (a.Min.X <= b.Max.X && a.Max.X >= b.Min.X) &&
            (a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y) &&
            (a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z);
    }
    public static bool AABBPlaneIntersect(AABB aabb,Plane plane) {
        //find center point of AABB
        Point center = new Point(aabb.Max.X + aabb.Min.X * 0.5f,
                                 aabb.Max.Y + aabb.Min.Y * 0.5f,
                                 aabb.Max.Z + aabb.Min.Z * 0.5f);

        //find positive extents
        Point extents = new Point(aabb.Max.X - center.X,
                                  aabb.Max.Y - center.Y,
                                  aabb.Max.Z - center.Z);

        //project interval radius of b onto L(t) = b.c + t * p.n
        float radius = extents.X * System.Math.Abs(plane.Normal.X) + extents.Y * System.Math.Abs(plane.Normal.Y) + extents.Z * System.Math.Abs(plane.Normal.Z);

        //compute distance of box center from plane
        float s = Vector3.Dot(plane.Normal, center.ToVector()) - plane.Distance;

        //intersection occurs when distance s falls within +-radius of plane
        //espsilon???!
        return (System.Math.Abs(s) <= radius);
        //last box should return true, but it returns false.
        //difference is .0000xx
    }
    public static bool AABBPlaneIntersect(Plane p,AABB aabb) {
        return AABBPlaneIntersect(aabb, p);
    }
}

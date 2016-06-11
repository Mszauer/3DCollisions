using Math_Implementation;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;


namespace CollisionDetectionSelector {
    class Intersections {
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
    }
}

using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class ClosestPointTriangle : Application {
        protected Point[] points = new Point[] {
            new Point(1f, 1f, 1f),
            new Point(-1f, -3f, -4f),
            new Point(2, 4, -1),
            new Point(-2.732051f, 6.732051f, 1.732051f),
            new Point(3, 7, -4)
        };
        Point[] tests = new Point[] {
            new Point(1, 3.5f, -1.5f),
            new Point(2, 2, -3),
            new Point(2, 4, -1),
            new Point(-1, 5, 0),
            new Point(3, 4, -0.9999995f),
        };

        Triangle triangle = new Triangle(new Point(-1.0f, 5.0f, 0.0f), new Point(2.0f, 2.0f, -3.0f), new Point(5.0f, 5.0f, 0.0f));

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(4f);
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            for (int i = 0; i < points.Length; ++i) {
                Point c = Collisions.TriangleCollisions.ClosestPointTriangle(triangle, points[i]);
                if (!PointCompare(c, tests[i])) {
                    LogError("Expected point " + i + " to be: " + tests[i] + ", got: " + c);
                }
            }
        }

        protected bool PointCompare(Point p1, Point p2) {
            float epsilon = 0.00001f;

            float x = System.Math.Abs(p1.X - p2.X);
            float y = System.Math.Abs(p1.Y - p2.Y);
            float z = System.Math.Abs(p1.Z - p2.Z);

            if (x > epsilon) return false;
            if (y > epsilon) return false;
            if (z > epsilon) return false;
            return true;
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0.0f, 0.0f, 1.0f);
            triangle.Render();

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < points.Length; ++i) {
                Point closest = Collisions.TriangleCollisions.ClosestPointTriangle(triangle, points[i]);
                GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
                GL.Vertex3(closest.X, closest.Y, closest.Z);
            }
            GL.End();

            GL.Color3(0.0f, 1.0f, 0.0f);
            foreach (Point point in points) {
                point.Render();
            }
        }
    }
}
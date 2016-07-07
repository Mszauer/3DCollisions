using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class TriangleTriangleIntersection : Application {
        Triangle test = new Triangle(new Point(5, 6, 7), new Point(6, 5, 4), new Point(1, 2, 3));
        Triangle[] triangles = new Primitive.Triangle[] {
            new Triangle(new Point(-1.0f, 5.0f, 0.0f), new Point(2.0f, 2.0f, -3.0f), new Point(5.0f, 5.0f, 0.0f)),
            new Triangle(new Point(-1, -1, 0), new Point(0, 1, 0), new Point(1, -1, 0)),
            new Triangle(new Point(-1.0f, -5.0f, 0.0f), new Point(2.0f, -2.0f, -3.0f), new Point(5.0f, -5.0f, 0.0f)),
            new Triangle(new Point(5, 6, 7), new Point(6, 5, 4), new Point(1, 2, 3)),
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(4f);
            GL.Disable(EnableCap.CullFace);

            Vector3 center = test.p0.ToVector() + test.p1.ToVector() + test.p2.ToVector();
            center *= 1.0f / 3.0f;

            test.p0.X -= center.X;
            test.p0.Y -= center.Y;
            test.p0.Z -= center.Z;

            test.p1.X -= center.X;
            test.p1.Y -= center.Y;
            test.p1.Z -= center.Z;

            test.p2.X -= center.X;
            test.p2.Y -= center.Y;
            test.p2.Z -= center.Z;
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0.0f, 0.0f, 1.0f);
            test.Render();

            foreach (Triangle triangle in triangles) {
                if (Collisions.TriangleCollisions.TriangleTriangleIntersection(triangle, test)) {
                    GL.Color3(0f, 1f, 0f);
                }
                else {
                    GL.Color3(1f, 0f, 0f);
                }
                triangle.Render();
            }
        }
    }
}
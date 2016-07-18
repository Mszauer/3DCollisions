using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class TriangleSphereIntersection : Application {
        Sphere[] spheres = new Sphere[] {
            new Sphere(new Point(2, 4, -1), 0.5f),
            new Sphere(new Point(-1.0f, 5.0f, 0.0f), 0.5f),
            new Sphere(new Point(2f, 2f, 2f), 1f),
            new Sphere(new Point(-2f, -2f, -2f), 1f)
        };

        Triangle triangle = new Triangle(new Point(-1.0f, 5.0f, 0.0f), new Point(2.0f, 2.0f, -3.0f), new Point(5.0f, 5.0f, 0.0f));

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(4f);
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            bool[] expected = new bool[] { true, true, false, false };
            for (int i = 0; i < spheres.Length; ++i) {
                bool result = Collisions.SphereIntersect(triangle, spheres[i]);
                if (result != expected[i]) {
                    LogError("Expected sphere " + i + " to " +
                        (expected[i] ? " intersect" : " NOT intersect") +
                        " the triangle");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0.0f, 0.0f, 1.0f);
            triangle.Render();

            foreach (Sphere sphere in spheres) {
                if (Collisions.SphereIntersect(triangle, sphere)) {
                    GL.Color3(0f, 1f, 0f);
                }
                else {
                    GL.Color3(1f, 0f, 0f);
                }
                sphere.Render();
            }
        }
    }
}
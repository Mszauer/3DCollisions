using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class RaycastSphere : Application {
        public class Touple {
            public Ray ray;
            public Sphere sphere;
            public bool result;

            public Touple(float rayX, float rayY, float rayZ, float normX, float normY, float normZ,
                float sphereX, float sphereY, float sphereZ, float rad, bool res) {
                ray = new Ray(new Point(rayX, rayY, rayZ), new Vector3(normX, normY, normZ));
                sphere = new Sphere(new Point(sphereX, sphereY, sphereZ), rad);
                result = res;
            }
        }

        Touple[] touples = new Touple[] {
            new Touple(-2, 1, 0, 2, 0, 0, 2, 0, 0, 2, true),
            new Touple(-2, 0, 0, 2, 0, 0, 2, 2, 0, 2, true),
            new Touple(-2, 0, 0, 2, 0, 0, 0, 0, 0, 2, true),
            new Touple(-2, 2, 0, 2, -1, 2, 0, 0, 0, 2, true),
            new Touple(2, 1, 0, 2, 0, 0, 2, 0, 0, 2, true),
            new Touple(-2, 1, 0, -1, 0, 0, 2, 0, 0, 2, false),
            new Touple(-5, 1, 0, 2, 0.4f, 0, 2, 0, 0, 2, false)
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            foreach (Touple touple in touples) {
                float t = 0f;
                if (Collisions.Raycast(touple.ray, touple.sphere, out t) != touple.result) {
                    LogError("Expected ray: " + touple.ray + "\nTo " +
                        (touple.result ? "intersect" : "not intersect")
                    + " sphere: " + touple.sphere);
                }
            }
        }

        public override void Render() {
            //GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            base.Render();
            DrawOrigin();

            GL.Color3(1f, 0f, 0f);
            for (int i = 0; i < 3; ++i) {
                touples[i].sphere.Render();
            }

            GL.Color3(0f, 1f, 0f);
            foreach (Touple touple in touples) {
                touple.ray.Render();
                if (touple.result) {
                    Point p = new Point();
                    Collisions.Raycast(touple.ray, touple.sphere, out p);
                    GL.Color3(0f, 0f, 1f);
                    p.Render();
                    GL.Color3(0f, 1f, 0f);
                }
            }
        }

        private void Log(string s) {
            System.Console.WriteLine(s);
        }
    }
}
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class RaycastAABB : Application {
        public AABB test = new AABB(new Point(0.5f, 0.5f, 0.5f), new Point(2f, 2f, 2f));

        public Ray[] rays = new Ray[] {
            new Ray(new Point(-2, -2, -2), new Vector3(2, 2, 2)),
            new Ray(new Point(0f, 0f, 0f), new Vector3(0f, 1f, 0f)),
            new Ray(new Point(0f, 0f, 0f), new Vector3(-1f, 0f, 0f)),
            new Ray(new Point(1f, 1f, 1f), new Vector3(1f, 1f, 0f)),
            new Ray(new Point(0.4f, 1f, 1f), new Vector3(-1f, 0f, 0f)),
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            bool[] results = new bool[] {
                true, false, false, true, false
            };

            float t;
            for (int i = 0; i < results.Length; ++i) {
                if (LinesAndRays.RaycastAABB(rays[i], test, out t) != results[i]) {
                    LogError("Expected ray at index: " + i + " to " +
                        (results[i] ? "intersect" : "not intersect") +
                        " the aabb");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0f, 1f, 0f);
            test.Render();

            float t;
            foreach (Ray ray in rays) {
                if (LinesAndRays.RaycastAABB(ray, test, out t)) {
                    GL.Color3(1f, 0f, 0f);
                }
                else {
                    GL.Color3(0f, 0f, 1f);
                }
                ray.Render();
            }
        }

        private void Log(string s) {
            System.Console.WriteLine(s);
        }
    }
}
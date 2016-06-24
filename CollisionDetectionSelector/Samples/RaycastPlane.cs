using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class RaycastPlane : Application {
        public Plane test = new Plane(new Vector3(1f, 1f, 0f), 1f);

        public Ray[] rays = new Ray[] {
            new Ray(new Point(0f, 0f, 0f), new Vector3(0f, -1f, 0f)),
            new Ray(new Point(0.5f, 0.5f, 0f), new Vector3(-1f, -1f, 0f)),
            new Ray(new Point(1f, 1f, 0f), new Vector3(1f, 1f, 0f)),
            new Ray(new Point(1f, 1f, -3f), new Vector3(0f, 0f, 1f)),
            new Ray(new Point(2f, 2f, 3f), new Vector3(0f, -1f, 0f)),
            new Ray(new Point(3f, 3f, 3f), new Vector3(-3f, -3f, -3f)),
            new Ray(new Point(1f, 1f, 3f), new Vector3(-2f, -3f, 1f)),
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            bool[] results = new bool[] {
                false, false, false, false, true, true, true
            };

            float t;
            for (int i = 0; i < results.Length; ++i) {
                if (LinesAndRays.RaycastPlane(rays[i], test, out t) != results[i]) {
                    LogError("Expected ray at index: " + i + " to " +
                        (results[i] ? "intersect" : "not intersect") +
                        " the plane");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(1f, 1f, 1f);
            test.Render(5f);


            float t;
            foreach (Ray ray in rays) {
                if (LinesAndRays.RaycastPlane(ray, test, out t)) {
                    Point colPoint = new Point();
                    LinesAndRays.RaycastPlane(ray, test, out colPoint);
                    GL.Color3(0f, 1f, 0f);
                    colPoint.Render();
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
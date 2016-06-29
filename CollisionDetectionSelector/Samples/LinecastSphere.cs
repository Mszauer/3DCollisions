using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class LinecastSphere : Application {
        public Line[] lines = new Line[] {
            new Line(new Point(2.5f, 2.5f, 2.5f), new Point(4, 4, 4)), // false
            new Line(new Point(1f, 1f, 0f), new Point(0f, 0f, 0f)), // false
            new Line(new Point(-1f, -1f, 0f), new Point(-3f, 0f, 0f)), // true
        };
        public Sphere sphere = new Sphere();

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(5f);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            sphere.Position = new Point(0f, 0f, 0f);
            sphere.Radius = 2;

            bool[] results = new bool[] { false, false, true };
            Point result = new Point();
            for (int i = 0; i < results.Length; ++i) {
                if (LinesAndRays.LinecastSphere(lines[i], sphere, out result) != results[i]) {
                    LogError("Line at index " + i + " was " +
                        (results[i] ? "expected" : "not expected") +
                        "to intersect the test SPHERE");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            Point result = new Point();
            foreach (Line line in lines) {
                if (LinesAndRays.LinecastSphere(line, sphere, out result)) {
                    GL.Color3(1f, 0f, 1f);
                    result.Render();
                    GL.Color3(0f, 2f, 0f);
                }
                else {
                    GL.Color3(1f, 0f, 0f);
                }
                line.Render();
            }

            GL.Color3(0f, 0f, 1f);
            sphere.Render();
        }

        private void Log(string s) {
            System.Console.WriteLine(s);
        }
    }
}
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class PointSample : Application {
        Point[] points = null;

        public override void Initialize(int width, int height) {
            points = new Point[100];
            for (int i = 0; i < 100; ++i) {
                points[i] = new Point(i - 50, i - 50, i - 50);
            }
            GL.PointSize(2f);
        }

        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(lookAt.OpenGL);

            for (int i = 0; i < points.Length; ++i) {
                points[i].Render();
            }
        }

        public override void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Matrix4 perspective = Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
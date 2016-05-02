using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class SphereSample : Application {
        Sphere sphere = new Sphere(new Point(4, 0, 0), 3);
        Sphere sphere2 = new Sphere();

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        public override void Render() {
            Matrix4 lookAt = Matrix4.LookAt(new Vector3(0.0f, 5.0f, 20), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(lookAt.OpenGL);

            GL.Begin(PrimitiveType.Lines);
            Line(1f, 0f, 0f);
            Line(0f, 1f, 0f);
            Line(0f, 0f, 1f);
            GL.End();

            GL.Color3(0f, 0f, 1f);
            sphere2.Render();
            GL.Color3(1f, 0f, 0f);
            sphere.Render();
        }

        void Line(float x, float y, float z) {
            GL.Color3(x, y, z);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(x, y, z);
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
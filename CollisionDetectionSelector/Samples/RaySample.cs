using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class RaySample : Application {
        protected Vector3 cameraAngle = new Vector3(120.0f, -10f, 20.0f);
        protected float rads = (float)(System.Math.PI / 180.0f);

        Line[] origin = new Line[] {
            new Line(new Point(0, 0, 0), new Point(1, 0, 0)),
            new Line(new Point(0, 0, 0), new Point(0, 1, 0)),
            new Line(new Point(0, 0, 0), new Point(0, 0, 1))
        };

        Ray[] random = new Ray[] {
            new Ray(),
            new Ray(new Point(8, 2, -7), new Vector3(1, 7, -3)),
            new Ray(new Point(-2, 3, 2), new Vector3(-4, -5, 3)),
            new Ray(new Vector3(-3, 2, 3), new Vector3(0, 5, 1)),
            new Ray(new Vector3(7, -1, -4), new Vector3(-9, 5, -8)),
        };

        float[][] colors = new float[][] {
            new float[] { 1f, 0f, 1f },
            new float[] { 0f, 1f, 1f },
            new float[] { 1f, 1f, 0f },
            new float[] { 1f, 1f, 1f },
            new float[] { 0.5f, 0.7f, 0.2f },
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);

            /*System.Random r = new System.Random();
            string output = "new Ray(new Point(" + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + "), new Vector3(" + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ")),\n";
            for (int i = 0; i < 3; ++i) {
                output += "new Ray(new Point(" + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + "), new Vector3(" + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ", " + (r.Next() % 20 - 10) + ")),\n";
            }
            System.IO.File.WriteAllText(@"C:\Users\WinVPC\Desktop\Rays.txt", output);*/
        }

        public override void Render() {
            Vector3 eyePos = new Vector3();
            eyePos.X = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));
            eyePos.Y = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.Y * rads);
            eyePos.Z = -cameraAngle.Z * (float)System.Math.Cos(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));

            Matrix4 lookAt = Matrix4.LookAt(eyePos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            GL.Color3(1f, 0f, 0f);
            origin[0].Render();
            GL.Color3(0f, 1f, 0f);
            origin[1].Render();
            GL.Color3(0f, 0f, 1f);
            origin[2].Render();

            for (int i = 0; i < random.Length; ++i) {
                GL.Color3(colors[i][0], colors[i][1], colors[i][2]);
                random[i].Render();
            }
        }

        public override void Update(float deltaTime) {
            cameraAngle.X += 45.0f * deltaTime;
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
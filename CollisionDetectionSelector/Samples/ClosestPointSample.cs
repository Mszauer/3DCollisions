using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class ClosestPointSample : Application {
        protected Vector3 cameraAngle = new Vector3(120.0f, -10f, 20.0f);
        protected float rads = (float)(System.Math.PI / 180.0f);

        Point[] testPoints = new Point[] {
            new Point(1, 0, 0),
            new Point(0, 1, 0),
            new Point(0, 0, 1),
            new Point(0.5f, 0.5f, 0.5f),
            new Point(2f, 2f, 2f),
            new Point(3f, 3f, 3f),
            new Point(4f, 4f, 4f),
        };

        Point farPoint = new Point(0f, 0f, 7f);
        Sphere testSphere = new Sphere(1f, 1f, 0f, 2f);

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PointSize(2f);

            bool[] results = new bool[] { true, true, true, true, false, false, false };
            for (int i = 0; i < testPoints.Length; ++i) {
                bool collision = Collisions.SphereCollisions.PointInSphere(testSphere, testPoints[i]);
                if (collision != results[i]) {
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                }
                System.Console.Write("Point: " + testPoints[i]);
                if (collision) {
                    System.Console.WriteLine(" is in sphere");
                }
                else {
                    System.Console.WriteLine(" is not in sphere");
                }
                System.Console.ResetColor();
            }

            Vector3 expected = new Vector3(0.719944f, 0.719944f, 1.960392f);
            Point closest = Collisions.SphereCollisions.ClosestPoint(testSphere, farPoint);
            if (expected != closest.ToVector()) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
            }
            System.Console.WriteLine("Closest point: " + closest);
            if (expected != closest.ToVector()) {
                System.Console.WriteLine("Expected: " + expected.X + ", " + expected.Y + ", " + expected.Z);
            }
            System.Console.ResetColor();
        }

        public override void Render() {
            Vector3 eyePos = new Vector3();
            eyePos.X = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));
            eyePos.Y = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.Y * rads);
            eyePos.Z = -cameraAngle.Z * (float)System.Math.Cos(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));

            Matrix4 lookAt = Matrix4.LookAt(eyePos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            DrawOrigin();

            GL.Color3(1f, 1f, 1f);
            testSphere.Render();

            foreach (Point point in testPoints) {
                if (Collisions.SphereCollisions.PointInSphere(testSphere, point)) {
                    GL.Color3(1f, 0f, 0f);
                }
                else {
                    GL.Color3(0f, 1f, 0f);
                }
                point.Render();
            }

            GL.Color3(0f, 0f, 1f);
            farPoint.Render();

            Point closest = Collisions.SphereCollisions.ClosestPoint(testSphere, farPoint);
            GL.Color3(1f, 0f, 1f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(testSphere.Position.X, testSphere.Position.Y, testSphere.Position.Z);
            GL.Vertex3(closest.X, closest.Y, closest.Z);
            GL.End();

            GL.Color3(1f, 1f, 0f);
            closest.Render();

            GL.Color3(0f, 1f, 1f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(closest.X, closest.Y, closest.Z);
            GL.Vertex3(farPoint.X, farPoint.Y, farPoint.Z);
            GL.End();
        }

        public override void Update(float deltaTime) {
            cameraAngle.X += 45.0f * deltaTime;
        }

        protected void DrawOrigin() {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);
            GL.End();
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
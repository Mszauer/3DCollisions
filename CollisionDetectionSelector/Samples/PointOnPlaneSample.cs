using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class PointOnPlaneSample : Application {
        protected Vector3 cameraAngle = new Vector3(120.0f, -10f, 20.0f);
        protected float rads = (float)(System.Math.PI / 180.0f);

        Plane plane = new Plane(new Point(5, 6, 7), new Point(6, 5, 4), new Point(1, 2, 3));
        Point[] points = new Point[] {
            new Point(0f, 0f, 0f),
            new Point(2f, 6f, 1f),
            new Point(3f, 1f, -3f),
            new Point(-2f, -1f, 2f),
            new Point(7f, 7f, -7f),
            new Point(3f, 2f, -1f),
            new Point(10f, 3f, -10f),
            new Point(2f, 8f, -7f),
            new Point(-1.632993f, 3.265986f, -1.632993f),
            new Point(-1.020621f, 3.265986f, -2.245366f)
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(2f);
            plane.Distance = 4f;

            int[] expected = new int[] { -1, -1, -1, -1, 1, -1, -1, 1, 0, 0 };
            int[] result = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };

            if (expected.Length != result.Length) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("Gabor messed up!");
            }

            if (result.Length != points.Length) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("Gabor messed up!");
            }

            for (int i = 0; i < points.Length; ++i) {
                Point point = points[i];
                if (Collisions.PlaneCollision.PointOnPlane(point, plane)) {
                    result[i] = 0;
                }
                else {
                    if (Collisions.PlaneCollision.DistanceFromPlane(point, plane) < 0f) {
                        result[i] = -1;
                    }
                    else {
                        result[i] = 1;
                    }
                }
            }

            for (int i = 0; i < points.Length; ++i) {
                if (result[i] == 3) {
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    System.Console.WriteLine("Gabor messed up!");
                }

                if (expected[i] != result[i]) {
                    System.Console.ForegroundColor = System.ConsoleColor.Red;
                    System.Console.Write("Point " + i + " was expected ");
                    if (expected[i] == 0) {
                        System.Console.Write("on ");
                    }
                    else if (expected[i] == 1) {
                        System.Console.Write("in front of ");
                    }
                    else if (expected[i] == -1) {
                        System.Console.Write("behind the ");
                    }
                    System.Console.Write("plane, but was found ");
                    if (result[i] == 0) {
                        System.Console.Write("on");
                    }
                    else if (result[i] == 1) {
                        System.Console.Write("in front");
                    }
                    else if (result[i] == -1) {
                        System.Console.Write("behind");
                    }
                    System.Console.WriteLine("\n\tPlane: " + plane.ToString());
                    System.Console.WriteLine("\tPoint: " + points[i].ToString() + "\n");
                }
            }
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
            plane.Render(7f);

            foreach (Point point in points) {
                if (Collisions.PlaneCollision.PointOnPlane(point, plane)) {
                    GL.Color3(0f, 1f, 0f);
                }
                else {
                    if (Collisions.PlaneCollision.DistanceFromPlane(point, plane) < 0f) {
                        GL.Color3(1f, 0f, 0f);
                    }
                    else {
                        GL.Color3(0f, 0f, 1f);
                    }
                }
                point.Render();
            }
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
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class AABBSample : Application {
        protected Vector3 cameraAngle = new Vector3(120.0f, -10f, 20.0f);
        protected float rads = (float)(System.Math.PI / 180.0f);

        AABB aabb1 = new AABB();
        AABB aabb2 = new AABB(new Point(-3, -2, -3), new Point(-1, -1, -2));
        AABB aabb3 = new AABB(new Point(5, 5, 5), new Vector3(2, 4, 3));


        public override void Initialize(int width, int height) {
            // Seeing the back face of a square gives a better
            // overview of the actual geometry
            //GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PointSize(4f);

            System.Console.ResetColor();
            CheckAABBMinMax(aabb1, 1, -1, -1, -1, 1, 1, 1);
            CheckAABBCenterExtents(aabb1, 1, 0, 0, 0, 1, 1, 1);
            System.Console.WriteLine("AABB1: " + aabb1);
            System.Console.WriteLine("\tCenter: " + aabb1.Center);
            System.Console.WriteLine("\tExtents: " + aabb1.Extents.X + ", " + aabb1.Extents.Y + ", " + aabb1.Extents.Z);

            System.Console.ResetColor();
            CheckAABBMinMax(aabb2, 2, -3, -2, -3, -1, -1, -2);
            CheckAABBCenterExtents(aabb2, 2, -2, -1.5f, -2.5f, 1, 0.5f, 0.5f);
            System.Console.WriteLine("AABB2: " + aabb2);
            System.Console.WriteLine("\tCenter: " + aabb2.Center);
            System.Console.WriteLine("\tExtents: " + aabb2.Extents.X + ", " + aabb2.Extents.Y + ", " + aabb2.Extents.Z);

            System.Console.ResetColor();
            CheckAABBMinMax(aabb3, 3, 3, 1, 2, 7, 9, 8);
            CheckAABBCenterExtents(aabb3, 3, 5, 5, 5, 2, 4, 3);
            System.Console.WriteLine("AABB3: " + aabb3);
            System.Console.WriteLine("\tCenter: " + aabb3.Center);
            System.Console.WriteLine("\tExtents: " + aabb3.Extents.X + ", " + aabb3.Extents.Y + ", " + aabb3.Extents.Z);
        }

        protected void CheckAABBCenterExtents(AABB aabb, int num, float cx, float cy, float cz, float ex, float ey, float ez) {
            if (aabb.Center.X != cx || aabb.Center.Y != cy || aabb.Center.Z != cz) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("AABB" + num + ", center is wrong");
                System.Console.WriteLine("\tGot: (" + aabb.Center.X + ", " + aabb.Center.Y + ", " + aabb.Center.Z + ")");
                System.Console.WriteLine("\tExpected: (" + cx + ", " + cy + ", " + cz + ")");
            }

            if (aabb.Extents.X != ex || aabb.Extents.Y != ey || aabb.Extents.Z != ez) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("AABB" + num + ", extents is wrong");
                System.Console.WriteLine("\tGot: (" + aabb.Extents.X + ", " + aabb.Extents.Y + ", " + aabb.Extents.Z + ")");
                System.Console.WriteLine("\tExpected: (" + ex + ", " + ey + ", " + ez + ")");
            }
        }

        protected void CheckAABBMinMax(AABB aabb, int num, float minX, float minY, float minz, float maxX, float maxY, float maxZ) {
            Point t = aabb.Min;
            if (t.X != minX || t.Y != minY || t.Z != minz) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("AABB" + num + ", min is wrong");
                System.Console.WriteLine("\tGot: (" + t.X + ", " + t.Y + ", " + t.Z + ")");
                System.Console.WriteLine("\tExpected: (" + minX + ", " + minY + ", " + minz + ")");
            }
            t = aabb.Max;
            if (t.X != maxX || t.Y != maxY || t.Z != maxZ) {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("AABB" + num + ", max is wrong");
                System.Console.WriteLine("\tGot: (" + t.X + ", " + t.Y + ", " + t.Z + ")");
                System.Console.WriteLine("\tExpected: (" + maxX + ", " + maxY + ", " + maxZ + ")");
            }
        }

        public override void Render() {
            Vector3 eyePos = new Vector3();
            eyePos.X = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));
            eyePos.Y = cameraAngle.Z * -(float)System.Math.Sin(cameraAngle.Y * rads);
            eyePos.Z = -cameraAngle.Z * (float)System.Math.Cos(cameraAngle.X * rads * (float)System.Math.Cos(cameraAngle.Y * rads));

            Matrix4 lookAt = Matrix4.LookAt(eyePos, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            GL.LoadMatrix(Matrix4.Transpose(lookAt).Matrix);

            DrawAABBDebug(aabb1);
            DrawAABBDebug(aabb2);
            DrawAABBDebug(aabb3);
        }

        protected void DrawAABBDebug(AABB aabb) {
            GL.Color3(1f, 1f, 1f);
            aabb.Render();
            GL.Color3(1f, 1f, 0f);
            aabb.Min.Render();
            GL.Color3(1f, 0f, 1f);
            aabb.Max.Render();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(aabb.Center.X, aabb.Center.Y, aabb.Center.Z);
            GL.Vertex3(aabb.Center.X + aabb.Extents.X, aabb.Center.Y, aabb.Center.Z);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(aabb.Center.X, aabb.Center.Y, aabb.Center.Z);
            GL.Vertex3(aabb.Center.X, aabb.Center.Y + aabb.Extents.Y, aabb.Center.Z);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(aabb.Center.X, aabb.Center.Y, aabb.Center.Z);
            GL.Vertex3(aabb.Center.X, aabb.Center.Y, aabb.Center.Z + aabb.Extents.Z);
            GL.End();
            GL.Color3(0f, 1f, 1f);
            aabb.Center.Render();
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
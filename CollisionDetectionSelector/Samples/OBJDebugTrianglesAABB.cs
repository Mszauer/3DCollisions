using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;
using CollisionDetectionSelector;

namespace CollisionDetectionSelector.Samples {
    class OBJDebugTrianglesAABB : Application {
        OBJLoader obj = null;

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });

            obj = new OBJLoader("Assets/suzanne.obj");

            if (obj.NumCollisionTriangles != 968) {
                LogError("Suzanne triangle count expected at 968, got: " + obj.NumCollisionTriangles);
            }
            AABB test = new AABB(new Point(-1.367188f, -0.984375f, -0.851562f), new Point(1.367188f, 0.984375f, 0.851562f));
            if (!AlmostEqual(test, obj.BoundingBox)) {
                LogError("Expected bounting box:\n" + test.ToString() + "\n Got bounding box: \n" + obj.BoundingBox.ToString());
            }
        }

        private bool AlmostEqual(float f1, float f2) {
            return (System.Math.Abs(f1 - f2) <= float.Epsilon *
                System.Math.Max(1.0f, System.Math.Max(
                    System.Math.Abs(f1), System.Math.Abs(f2))));
        }

        private bool AlmostEqual(Point p1, Point p2) {
            return AlmostEqual(p1.X, p2.X) && AlmostEqual(p1.Y, p2.Y) && AlmostEqual(p1.Z, p2.Z);
        }

        private bool AlmostEqual(AABB aabb1, AABB aabb2) {
            return AlmostEqual(aabb1.Min, aabb2.Min) && AlmostEqual(aabb1.Max, aabb2.Max);
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.PushMatrix();
            GL.Scale(3.0f, 3.0f, 3.0f);
            obj.DebugRender();
            GL.PopMatrix();
        }
    }
}
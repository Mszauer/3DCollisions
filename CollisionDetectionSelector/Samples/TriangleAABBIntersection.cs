using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class TriangleAABBIntersection : Application {
        AABB[] aabbs = new AABB[] {
            new AABB(new Point(2, 4, -1), new Vector3(0.5f, 0.5f, 0.5f)),
            new AABB(new Point(-1.0f, 5.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f)),
            new AABB(new Point(0.0f, 0.0f, 0.0f), new Vector3(5f, 7f, 5f)),
            new AABB( new Point(2.0f, 3.0f, -3.0f), new Vector3(3f, 0.5f, 2f)),


            new AABB(new Point(2f, 2f, 2f), new Vector3(1.0f, 1.0f, 1.0f)),
            new AABB(new Point(-2f, -2f, -2f), new Vector3(1.0f, 1.0f, 1.0f))
        };

        Triangle triangle = new Triangle(new Point(-1.0f, 5.0f, 0.0f), new Point(2.0f, 2.0f, -3.0f), new Point(5.0f, 5.0f, 0.0f));

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(4f);
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            bool[] expected = new bool[] { true, true, true, true, false, false };
            for (int i = 0; i < aabbs.Length; ++i) {
                bool result = Collisions.TriangleAABBIntersect(triangle, aabbs[i]);
                if (result != expected[i]) {
                    LogError("Expected aabb " + i + " to " +
                        (expected[i] ? " intersect" : " NOT intersect") +
                        " the triangle");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0.0f, 0.0f, 1.0f);
            triangle.Render();

            foreach (AABB aabb in aabbs) {
                if (Collisions.TriangleAABBIntersect(triangle, aabb)) {
                    GL.Color3(0f, 1f, 0f);
                }
                else {
                    GL.Color3(1f, 0f, 0f);
                }
                aabb.Render();
            }
        }
    }
}
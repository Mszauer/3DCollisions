using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class AABBPlaneIntersection : Application {
        Plane test = new Plane();

        AABB[] aabbs = new AABB[] {
            null, null, null, null // Size = 4
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PointSize(5f);

            test.Normal = new Vector3(0f, 1f, 0.5f);
            aabbs[0] = new AABB(new Point(-2f, -2f, -2f), new Point(-1f, -1f, -1f));
            aabbs[1] = new AABB(new Point(2f, 1f, 2f), new Point(4f, 3f, 4f));
            aabbs[2] = new AABB(new Point(1f, 0f, 1f), new Point(0f, -1f, 0f));
            aabbs[3] = new AABB(new Point(5f, 5f, 5f), new Point(-5f, -5f, -5f));

            bool[] results = new bool[] {
                false, false, true, true
            };
            int t = 0;

            for (int i = 0; i < aabbs.Length; ++i) {
                if (Intersects.AABBPlaneIntersect(aabbs[i], test) != results[t++]) {
                    LogError("Expected aabb " + i + " to " +
                        (results[t - 1] ? "intersect" : "not intersect") +
                    " the plane");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            for (int i = 0; i < aabbs.Length; ++i) {
                GL.Color3(0f, 0f, 1f);
                if (Intersects.AABBPlaneIntersect(test, aabbs[i])) {
                    GL.Color3(1f, 0f, 0f);
                }
                aabbs[i].Render();
            }

            GL.Color3(0f, 0f, 1f);
            test.Render(5f);
        }
    }
}
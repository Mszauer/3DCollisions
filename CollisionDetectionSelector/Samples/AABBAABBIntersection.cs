using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class AABBtoAABBIntersection : Application {
        AABB[] aabbs = new AABB[] {
            null, null, null, null // Size = 4
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            aabbs[0] = new AABB(new Point(-0.5f, -2f, -0.5f), new Point(0.5f, 2f, 0.5f));
            aabbs[1] = new AABB();
            aabbs[2] = new AABB(new Point(1f, 1f, 1f), new Point(3f, 3f, 3f));
            aabbs[3] = new AABB(new Point(-3f, -3f, -3f), new Point(-4f, -4f, -4f));

            bool[] results = new bool[] {
                true, true, false, false, true, true, true, false, false,
                true, true, false, false, false, false, true,
            };
            int t = 0;

            for (int i = 0; i < aabbs.Length; ++i) {
                for (int j = 0; j < aabbs.Length; ++j) {
                    if (Intersects.AABBAABBIntersect(aabbs[i], aabbs[j]) != results[t++]) {
                        LogError("aabb " + i + " and " + j + " should " +
                            (results[t - 1] ? "" : "not ") + "intersect"
                        );
                    }
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            for (int i = 0; i < aabbs.Length; ++i) {
                GL.Color3(0f, 0f, 1f);
                for (int j = 0; j < aabbs.Length; ++j) {
                    if (i != j && Intersects.AABBAABBIntersect(aabbs[i], aabbs[j])) {
                        GL.Color3(1f, 0f, 0f);
                        break;
                    }
                }
                aabbs[i].Render();
            }
        }
    }
}

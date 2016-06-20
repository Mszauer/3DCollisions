using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class PlanePlaneIntersection : Application {
        Plane[] planes = new Plane[] {
            null, null, null, null, null // Size = 5
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PointSize(5f);

            planes[0] = new Plane();
            planes[1] = new Plane(new Vector3(2f, 0f, 1f), 0f);
            planes[2] = new Plane(new Vector3(0f, 1f, 0f), -2f);
            planes[3] = new Plane(new Vector3(-1f, 1f, 2f), 3f);
            planes[4] = new Plane(new Vector3(1f, 0f, 0.5f), 3f);

            bool[] results = new bool[] {
                false, true,  true, true,  true, true, false, true,  true, false, true,
                true,  false, true, true,  true, true, true,  false, true, true,  false,
                true, true,  false,
            };
            int t = 0;

            for (int i = 0; i < planes.Length; ++i) {
                for (int j = 0; j < planes.Length; ++j) {
                    if (Intersects.PlanePlaneIntersect(planes[i], planes[j]) != results[t++]) {
                        LogError("[" + (t - 1) + "] Expected plane " + i + " to " +
                            (results[t - 1] ? "intersect" : "not intersect") +
                        " plane " + j);
                    }
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            for (int i = 0; i < planes.Length; ++i) {
                planes[i].Render();
            }
        }
    }
}
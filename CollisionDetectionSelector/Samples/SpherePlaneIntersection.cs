using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class SpherePlaneIntersection : Application {
        Sphere test = new Sphere(new Point(1f, 0f, 1f), 2f);

        Plane[] planes = new Plane[] {
            null, null, null, null, null // Size = 5
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PointSize(5f);

            planes[0] = new Plane();
            planes[1] = new Plane(new Vector3(0f, 1f, 0.2f), 3f);
            planes[2] = new Plane(new Vector3(0f, 1f, 0f), -2f);
            planes[3] = new Plane(new Vector3(-1f, 1f, 2f), 3f);
            planes[4] = new Plane(new Vector3(1f, 0f, 0f), 3f);

            bool[] results = new bool[] {
                true, false, true, false, true
            };
            int t = 0;

            for (int i = 0; i < planes.Length; ++i) {
                if (Intersects.SpherePlaneIntersect(planes[i], test) != results[t++]) {
                    LogError("Expected plane " + i + " to " +
                        (results[t - 1] ? "intersect" : "not intersect") +
                    " the sphere");
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Disable(EnableCap.CullFace);
            for (int i = 0; i < planes.Length; ++i) {
                GL.Color3(0f, 0f, 1f);
                if (Intersects.SpherePlaneIntersect(test, planes[i])) {
                    GL.Color3(1f, 0f, 0f);
                }
                planes[i].Render();
            }
            GL.Enable(EnableCap.CullFace);

            GL.Color3(0f, 0f, 1f);
            test.Render();
        }
    }
}
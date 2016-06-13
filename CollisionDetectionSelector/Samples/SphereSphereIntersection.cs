using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class SphereSphereIntersection : Application {
        Sphere[] spheres = new Sphere[] {
            null, null, null, null // Size = 4
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            spheres[0] = new Sphere(new Point(0f, 0f, 0f), 2f);
            spheres[1] = new Sphere(new Point(0f, 3.5f, 0f), 1f);
            spheres[2] = new Sphere(new Point(0f, -3f, 0f), 1f);
            spheres[3] = new Sphere(new Point(1f, 0f, 0f), 1f);

            bool[] results = new bool[] {
                true, false, true, true, false, true, false, false, true,
                false, true, false, true, false, false, true
            };
            int t = 0;

            for (int i = 0; i < spheres.Length; ++i) {
                for (int j = 0; j < spheres.Length; ++j) {
                    if (Intersects.SphereSphereIntersect(spheres[i], spheres[j]) != results[t++]) {
                        LogError("sphere " + i + " and " + j + " should " +
                            (results[t - 1] ? "" : "not ") + "intersect"
                        );
                    }
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            for (int i = 0; i < spheres.Length; ++i) {
                GL.Color3(0f, 0f, 1f);
                for (int j = 0; j < spheres.Length; ++j) {
                    if (i != j && Intersects.SphereSphereIntersect(spheres[i], spheres[j])) {
                        GL.Color3(1f, 0f, 0f);
                        break;
                    }
                }
                spheres[i].Render();
            }
        }
    }
}
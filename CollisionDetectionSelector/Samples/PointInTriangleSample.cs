using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Samples {
    class PointInTriangle : Application {
        protected Point[] points = null;

        Triangle triangle = new Triangle(new Point(-1.0f, 1.0f, 1.0f), new Point(0.0f, -1.0f, 1.0f), new Point(1.0f, 1.0f, 1.0f));

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.PointSize(4f);
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            int i_min = -2;
            int i_max = 3;
            int j_min = -2;
            int j_max = 3;
            int k_min = -1;
            int k_max = 3;

            int indexer = 0;
            for (int i = i_min; i < i_max; ++i) {
                for (int j = j_min; j < j_max; ++j) {
                    for (int k = k_min; k < k_max; ++k) {
                        indexer++;
                    }
                }
            }

            points = new Point[indexer + 3];
            indexer = 0;

            for (int i = i_min; i < i_max; ++i) {
                for (int j = j_min; j < j_max; ++j) {
                    for (int k = k_min; k < k_max; ++k) {
                        points[indexer++] = new Point(i, j, k);
                    }
                }
            }

            points[indexer++] = new Point(0.5f, 0.5f, 1f);
            points[indexer++] = new Point(0.5f, -0.25f, 1f);
            points[indexer++] = new Point(-0.25f, -0.25f, 1f);


            int[] colliding = new int[] { 34, 46, 50, 54, 74, 100, 102 };
            int col_index = 0;

            for (int i = 0; i < points.Length; ++i) {
                if (col_index < colliding.Length && i == colliding[col_index]) {
                    if (!Collisions.TriangleCollisions.PointInTriangle(triangle, points[i])) {
                        LogError("point " + i + " SHOULD be colliding!");
                    }

                    col_index++;
                }
                else {
                    if (Collisions.TriangleCollisions.PointInTriangle(triangle, points[i])) {
                        LogError("point " + i + " should NOT be colliding!");
                    }
                }
            }
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Color3(0.0f, 0.0f, 1.0f);
            triangle.Render();

            foreach (Point point in points) {
                if (Collisions.TriangleCollisions.PointInTriangle(triangle, point)) {
                    GL.Color3(0.0f, 1.0f, 0.0f);
                }
                else {
                    GL.Color3(1.0f, 0.0f, 0.0f);
                }

                point.Render();
            }
        }
    }
}
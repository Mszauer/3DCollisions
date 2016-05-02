using Math_Implementation;
using OpenTK.Graphics.OpenGL;
using System;

namespace CollisionDetectionSelector.Primitive {
    class Sphere {
        Vector3 point = new Vector3();
        float radius = 1f;

        public Point Position {
            get {
                return new Point(point.X, point.Y, point.Z);
            }
            set {
                point.X = value.X;
                point.Y = value.Y;
                point.Z = value.Z;
            }
        }
        public float Radius {
            get {
                return radius;
            }
            set {
                radius = value;
            }
        }
        public Sphere() {
            CreateVBO();
        }
        public Sphere(Vector3 p, float r) {
            Position.X = p.X;
            Position.Y = p.Y;
            Position.Z = p.Z;
            radius = r;
            CreateVBO();
        }
        public Sphere(Point p, float r) {
            Position.X = p.X;
            Position.Y = p.Y;
            Position.Z = p.Z;
            radius = r;
            CreateVBO();
        }
        public Sphere(float x, float y, float z, float r) {
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            radius = r;
            CreateVBO();
        }

        #region Rendering
        private float[] verts = null;
        private float[] norms = null;
        private uint[] indices = null;

        private void CreateVBO(uint rings = 10, uint sectors = 15) {
            // From:
            // http://stackoverflow.com/questions/5988686/creating-a-3d-sphere-in-opengl-using-visual-c/5989676#5989676
            // http://stackoverflow.com/questions/7957254/connecting-sphere-vertices-opengl
            float R = 1f / (float)(rings - 1);
            float S = 1f / (float)(sectors - 1);
            float M_PI = 3.14159265358979323846f;
            float M_PI_2 = 1.57079632679489661923f;

            verts = new float[rings * sectors * 3];
            norms = new float[rings * sectors * 3];
            indices = new uint[rings * sectors * 4];

            int v = 0;
            int n = 0;
            int i = 0;

            for (int r = 0; r < rings; r++) {
                for (int s = 0; s < sectors; s++) {
                    float y = (float)Math.Sin(-M_PI_2 + M_PI * r * R);
                    float x = (float)Math.Cos(2f * M_PI * s * S) * (float)Math.Sin(M_PI * r * R);
                    float z = (float)Math.Sin(2f * M_PI * s * S) * (float)Math.Sin(M_PI * r * R);

                    verts[v++] = (x /* * radius*/);
                    verts[v++] = (y /* * radius*/);
                    verts[v++] = (z /* * radius*/);

                    norms[n++] = (x);
                    norms[n++] = (y);
                    norms[n++] = (z);
                }
            }

            if (v != verts.Length) {
                Console.WriteLine("ERROR, Wrong number of verts!");
            }
            if (n != norms.Length) {
                Console.WriteLine("ERROR, Wrong number of norms!");
            }

            for (int r = 0; r < rings - 1; r++) {
                for (int s = 0; s < sectors - 1; s++) {
                    indices[i++] = ((uint)(r * sectors + s));
                    indices[i++] = ((uint)(r * sectors + (s + 1)));
                    indices[i++] = ((uint)((r + 1) * sectors + (s + 1)));
                    indices[i++] = ((uint)((r + 1) * sectors + s));
                }
            }

            if (i != indices.Length) {
                Console.WriteLine("ERROR, Wrong number of indices!");
            }
        }
        public void Render() {
            GL.PushMatrix();
            GL.Translate(point.X, point.Y, point.Z);
            GL.Scale(radius, radius, radius);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            GL.VertexPointer(3, VertexPointerType.Float, 0, verts);
            GL.NormalPointer(NormalPointerType.Float, 0, norms);
            GL.DrawElements(PrimitiveType.Quads, indices.Length, DrawElementsType.UnsignedInt, indices);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.PopMatrix();
        }
        #endregion
        public override string ToString() {
            return "Position: (" + point.X + ", " + point.Y + ", " + point.Z + "), Radius: " + radius;
        }
    }
}

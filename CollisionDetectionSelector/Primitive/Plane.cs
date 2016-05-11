using System;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace CollisionDetectionSelector.Primitive {
    class Plane {
        protected Vector3 _normal = new Vector3();

        public Vector3 Normal {
            get {
                return _normal;
            }
            //normalized when set
            set {
                _normal = Vector3.Normalize(value);
            }
        }
        public float Distance = 0f;

        public Plane() {
            //default plane
            //0,0,1 distance is 0
            Normal = new Vector3(0, 0, 1);
            Distance = 0f;
        }
        public Plane(Vector3 norm,float dist) {
            Normal = norm;
            Distance = dist;
        }
        public Plane(Point a, Point b, Point c) {
            Vector3 ab = new Vector3(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
            Vector3 ac = new Vector3(c.X - a.X, c.Y - a.Y, c.Z - a.Z);
            //construct plane from point
            Normal = Vector3.Normalize(Vector3.Cross(ab, ac));
            Distance = Vector3.Dot(Normal, new Vector3(a.X,a.Y,a.Z));
        }
        // No need to edit anything below this
        public void Render(float scale = 1f) {
            //Debug Normal
            /* // GL.Color3(1f, 1f, 0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(Normal.X * 500, Normal.Y * 500, Normal.Z * 500);
            GL.End(); */

            // Construct plane orientation
            Vector3 forward = new Vector3(Normal.X, Normal.Y, Normal.Z);
            Vector3 up = new Vector3(0f, 1f, 0f);
            Vector3 right = Vector3.Cross(forward, up);
            up = Vector3.Cross(right, forward);

            // Because this is going to be a matrix, it needs to be normalized
            forward.Normalize();
            right.Normalize();
            up.Normalize();

            // Create plane model matrix
            Matrix4 rot = new Matrix4(right.X, up.X, -forward.X, 0.0f,
                right.Y, up.Y, -forward.Y, 0.0f,
                right.Z, up.Z, -forward.Z, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            Matrix4 trans = Matrix4.Translate(Normal * Distance);
            Matrix4 model = trans * rot;

            // Load matrix and render plane
            GL.PushMatrix();
            GL.MultMatrix(model.OpenGL);
            GL.Scale(scale, scale, scale);
            //GL.Color3(1f, 1f, 1f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-1, -1, 0);
            GL.Vertex3(-1, 1, 0);
            GL.Vertex3(1, 1, 0);
            GL.Vertex3(1, -1, 0);
            GL.End();
            GL.PopMatrix();
        }

        public override string ToString() {
            return "N: (" + Normal.X + ", " + Normal.Y + ", " + Normal.Z + "), D: " + Distance;
        }
    }
}

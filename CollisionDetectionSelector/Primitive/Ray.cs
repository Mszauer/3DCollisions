using System;
using Math_Implementation;
using OpenTK.Graphics.OpenGL;

namespace CollisionDetectionSelector.Primitive {
    class Ray {
        public Point Position;
        private Vector3 _normal;

        public Vector3 Normal {
            get {
                return _normal;
            }
            set {
                _normal = value;
                _normal.Normalize();
            }
        }

        public Ray() {
            //ray at origin
            //points down to z axis
            Position = new Point(0, 0, 0);
            _normal = new Vector3(0, 0, 1);
        }
        public Ray(Point p, Vector3 dir) {
            Position = new Point(p.X, p.Y, p.Z);
            _normal = new Vector3(dir.X, dir.Y, dir.Z);
            _normal.Normalize();
        }
        public Ray(Vector3 p, Vector3 d) {
            //normalize!
            Position = new Point(p.X, p.Y, p.Z);
            _normal = new Vector3(d.X, d.Y, d.Z);
            _normal.Normalize();
        }
        public void Render() {
            GL.Begin(PrimitiveType.Lines);
            Vector3 start = Position.ToVector();
            Vector3 end = start + _normal * 5000f;
            GL.Vertex3(start.X, start.Y, start.Z);
            GL.Vertex3(end.X, end.Y, end.Z);
            GL.End();
        }
        public override string ToString() {
            string result = "P: (" + Position.X + ", " + Position.Y + ", " + Position.Z + "), ";
            result += "D: ( " + _normal.X + ", " + _normal.Y + ", " + _normal.Z + ")";
            return result;
        }
    }
}

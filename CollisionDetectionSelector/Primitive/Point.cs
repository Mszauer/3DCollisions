using Math_Implementation;
using OpenTK.Graphics.OpenGL;

namespace CollisionDetectionSelector.Primitive {
    class Point {
        protected Vector3 position = new Vector3();
        public Vector3 Position {
            get {
                return position;
            }
            set {
                if (Position == null) {
                    Position = new Vector3();
                }
                position.X = value.X;
                position.Y = value.Y;
                position.Z = value.Z;
            }
        }
        public float X {
            get {
                return position.X;
            }
            set {
                position.X = value;
            }
        }
        public float Y {
            get {
                return position.Y;
            }
            set {
                position.Y = value;
            }
        }
        public float Z {
            get {
                return position.Z;
            }
            set {
                position.Z = value;
            }
        }
        public Point() {
            Position = new Vector3();
        }
        public Point(float x, float y, float z) {
            Position = new Vector3(x, y, z);
        }
        public Point(Vector3 v) {
            Position = new Vector3(v.X, v.Y, v.Z);
        }
        public void FromVector(Vector3 v) {
            Position = new Vector3(v.X, v.Y, v.Z);
        }
        #region Rendering
        public void Render() {
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(Position.X, Position.Y, Position.Z);
            GL.End();
        }
        public override string ToString() {
            return "(" + Position.X + ", " + Position.Y + ", " + Position.Z + ")";
        }
        #endregion
    }
}

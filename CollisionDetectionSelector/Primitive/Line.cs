using System;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace CollisionDetectionSelector.Primitive {
    class Line {
        public Point Start;
        public Point End;
        public float Length {
            get {
                Vector3 line = End.ToVector() - Start.ToVector();
                return line.Length();
            }
        }
        public float LengthSq {
            get {
                Vector3 line = End.ToVector()-Start.ToVector();
                return line.LengthSquared();
            }
        }
        public Line(Line copy) {
            Start = new Point(copy.Start.X,copy.Start.Y,copy.Start.Z);
            End = new Point(copy.End.X,copy.End.Y,copy.End.Z);
        }
        public Line(Point p1, Point p2) {
            Start = new Point(p1);
            End = new Point(p2);
        }
        public Vector3 ToVector() {
            return new Vector3(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
        }
        public void Render() {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(Start.X, Start.Y, Start.Z);
            GL.Vertex3(End.X, End.Y, End.Z);
            GL.End();
        }
        public override string ToString() {
            string result = "Start: (" + Start.X + ", " + Start.Y + ", " + Start.Z + "), ";
            result += "End: ( " + End.X + ", " + End.Y + ", " + End.Z + ")";
            return result;
        }
    }
}

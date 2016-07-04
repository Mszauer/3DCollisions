using System;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace CollisionDetectionSelector.Primitive {
    class Triangle {
        public Point p0 = new Point();
        public Point p1 = new Point();
        public Point p2 = new Point();
        public Triangle() {

        }
        public Triangle(Point _p0,Point _p1,Point _p2) {
            p0 = new Point(_p0.ToVector());
            p1 = new Point(_p1.ToVector());
            p2 = new Point(_p2.ToVector());
        }
        public Triangle(Vector3 _p0,Vector3 _p1,Vector3 _p2) {
            p0 = new Point(_p0);
            p1 = new Point(_p1);
            p2 = new Point(_p2);
        }
        public void Render() {
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(p0.X,p0.Y,p0.Z);
            GL.Vertex3(p1.X, p1.Y, p1.Z);
            GL.Vertex3(p2.X, p2.Y, p2.Z);
            GL.End();
        }
        public override string ToString() {
            return "p0: " + p0.ToString() + ", p1: " + p1.ToString() + ", p2: " + p2.ToString();
        }
    }
}

using System;
using OpenTK.Graphics.OpenGL;
using Math_Implementation;

namespace CollisionDetectionSelector.Primitive {
    class AABB {
        public Point Min = new Point();
        public Point Max = new Point();

        public Point Center {
            get {
                return new Point((Max.X - Min.X) / 2, (Max.Y - Min.Y) / 2, (Max.Z - Min.Z) / 2);
            }
        }
        public Vector3 Extents {
            get {
                //maybe use center?
                //max-center || center-min = extent?
                return new Vector3((Max.X - Min.X) / 2, (Max.Y - Min.Y) / 2, (Max.Z - Min.Z) / 2);

            }
        }
        public bool IsValid {
            get {
                return (Min.X < Max.X && Min.Y < Max.Y && Min.Z < Max.Z);
            }
        }
        public void Fix() {
            //store old Min
            Point _min = new Point(Min);
            //change new "Min" to previous max
            Min = Max;
            //set new max to previous Min?
            Max = _min;
        }
        public AABB() {
            //make unit AABB
            //min=-1, max=+1 on all axiis
        }
        public AABB(Point min, Point max) {
            //set min/max
            Min = min;
            Max = max;
            //can be invalid and might need fix
            if (IsValid) {

            }
        }
        public AABB(Point center,Vector3 extents) {

        }
        public void Render() {
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex3(Min.X, Min.Y, Max.Z);
            GL.Vertex3(Max.X, Min.Y, Max.Z);
            GL.Vertex3(Max.X, Max.Y, Max.Z);
            GL.Vertex3(Min.X, Max.Y, Max.Z);

            GL.Vertex3(Max.X, Min.Y, Max.Z);
            GL.Vertex3(Max.X, Min.Y, Min.Z);
            GL.Vertex3(Max.X, Max.Y, Min.Z);
            GL.Vertex3(Max.X, Max.Y, Max.Z);

            GL.Vertex3(Min.X, Max.Y, Max.Z);
            GL.Vertex3(Max.X, Max.Y, Max.Z);
            GL.Vertex3(Max.X, Max.Y, Min.Z);
            GL.Vertex3(Min.X, Max.Y, Min.Z);

            GL.Vertex3(Min.X, Min.Y, Min.Z);
            GL.Vertex3(Min.X, Max.Y, Min.Z);
            GL.Vertex3(Max.X, Max.Y, Min.Z);
            GL.Vertex3(Max.X, Min.Y, Min.Z);

            GL.Vertex3(Min.X, Min.Y, Min.Z);
            GL.Vertex3(Max.X, Min.Y, Min.Z);
            GL.Vertex3(Max.X, Min.Y, Max.Z);
            GL.Vertex3(Min.X, Min.Y, Max.Z);

            GL.Vertex3(Min.X, Min.Y, Min.Z);
            GL.Vertex3(Min.X, Min.Y, Max.Z);
            GL.Vertex3(Min.X, Max.Y, Max.Z);
            GL.Vertex3(Min.X, Max.Y, Min.Z);

            GL.End();
        }

        public override string ToString() {
            string result = "Min: (" + Min.X + ", " + Min.Y + ", " + Min.Z + "), ";
            result += "Max: ( " + Max.X + ", " + Max.Y + ", " + Max.Z + ")";
            return result;
        }
    }
}

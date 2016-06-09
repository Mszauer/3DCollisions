using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class LineCollisions {
        public static bool PointOnLine(Point p,Line line) {
            float m = (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);//rise over run
            float b = line.Start.Y - m * line.Start.X;

            //at this point, evaluation equation is:
            //return point.y==m*p.x+b
            //wont work because we use floats
            //floating error can be accumulated, episilon testing
            if (Math.Abs(p.Y-(m*p.X+b)) < 0.0001f) {
                return true;
            }
            return false;
        }
    }
}

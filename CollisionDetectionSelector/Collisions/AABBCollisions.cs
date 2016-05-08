using System;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector.Collisions {
    class AABBCollisions {
        public static bool PointInAABB(AABB aabb, Point point) {
            bool passed = true;
            if (!(point.X < aabb.Min.X) && !(aabb.Min.X < point.X)) {
                passed = false;
            }
            if (!(point.Y < aabb.Min.Y) && !(aabb.Min.Y < point.Y)) {
                passed = false;
            }
            if (!(point.Z < aabb.Min.Z) && !(aabb.Min.Z < point.Z)) {
                passed = false;
            }
            return passed;
        }

        public static Point ClosestPoint(AABB aabb, Point point) {
            Point result = new Point(point);
            if (point.X > aabb.Max.X) {
                //outside box towards max
                result.X = aabb.Max.X;
            }
            else if (point.X < aabb.Min.X) {
                //outside box toawrds min
                result.X = aabb.Min.X;
            }
            if (point.Y > aabb.Max.Y) {
                result.Y = aabb.Max.Y;
            }
            else if (point.Y < aabb.Min.Y) {
                result.Y = aabb.Min.Y;
            }
            if (point.Z > aabb.Max.Z) {
                result.Z = aabb.Max.Z;
            }
            else if (point.Z < aabb.Min.Z) {
                result.Z = aabb.Min.Z;
            }
            return result;
        }

    }
}

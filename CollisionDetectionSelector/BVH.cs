using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;
using Math_Implementation;

class BVHNode {
    protected static int maxDepth = 3;

    public List<BVHNode> Children = null;
    public List<Triangle> Triangles = null;
    public AABB AABB = null;

    public BVHNode(AABB aabb) {
        //store aabb by value
        AABB = new AABB(aabb.Min, aabb.Max);
        //assume its leaf by default
        Triangles = new List<Triangle>();
        Children = null;
    }
    public bool IsLeaf {
        get {
            return Children == null && Triangles != null;
        }
    }
    //Public facing split function
    public void Split() {
        Split(0);
    }

    //private recursive split
    protected void Split(int depth) {
        //if node is leaf, we can split it
        if (IsLeaf) {
            //only split if it contains triangles
            if (Triangles.Count > 0) {
                //only split if were below maximum BVH depth
                if (depth < maxDepth) {
                    Children = new List<BVHNode>();
                    Vector3 center = AABB.Center.ToVector();
                    Vector3 extent = AABB.Extents;

                    Vector3 TFL = center + new Vector3(-extent.X, +extent.Y, -extent.Z);
                    Vector3 TFR = center + new Vector3(+extent.X, +extent.Y, -extent.Z);
                    Vector3 TBL = center + new Vector3(-extent.X, +extent.Y, +extent.Z);
                    Vector3 TBR = center + new Vector3(+extent.X, +extent.Y, +extent.Z);
                    Vector3 BFL = center + new Vector3(-extent.X, -extent.Y, -extent.Z);
                    Vector3 BFR = center + new Vector3(+extent.X, -extent.Y, -extent.Z);
                    Vector3 BBL = center + new Vector3(-extent.X, -extent.Y, +extent.Z);
                    Vector3 BBR = center + new Vector3(+extent.X, -extent.Y, +extent.Z);

                    Children.Add(new BVHNode(new AABB(new Point(TFL), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(TFR), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(TBL), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(TBR), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(BFL), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(BFR), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(BBL), new Point(center))));
                    Children.Add(new BVHNode(new AABB(new Point(BBR), new Point(center))));
                }
            }
        }
        //if this node was just split, it is neither leaf
        //or a normal mode, because it has both triangles and children
        //fix by turning into non leaf node
        if (Triangles != null && Children != null) {
            foreach(Triangle triangle in Triangles) {
                foreach(BVHNode child in Children) {
                    if (Collisions.TriangleAABBIntersect(child.AABB, triangle)) {
                        child.Triangles.Add(triangle);
                    }
                }
            }
            //make sure node is not a leaf
            Triangles.Clear();
            Triangles = null;
        }

        //Splitting is a recursive function, if node isnt a leaf
        //split all of leaf nodes ( if possible)
        if (Children != null) {
            foreach(BVHNode child in Children) {
                child.Split(depth + 1);
            }
        }
    }
}


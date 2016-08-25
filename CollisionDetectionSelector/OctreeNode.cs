using System;
using System.Collections.Generic;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;
using OpenTK.Graphics.OpenGL;

namespace CollisionDetectionSelector {
    class OctreeNode {
        public AABB Bounds = null;
        public OctreeNode Parent = null;
        public List<OctreeNode> Children = null;
        public List<OBJ> Contents = null;

        protected bool debugVisited = false;

        public OctreeNode(Point pos, float halfSize,OctreeNode parent) {
            Bounds = new AABB(
                new Point(pos.X - halfSize, pos.Y - halfSize, pos.Z - halfSize),
                new Point(pos.X + halfSize, pos.Y + halfSize, pos.Z + halfSize));
            Parent = parent;
            Contents = new List<OBJ>();
            //keep children null, this is a leaf node
        }

        //recursive, MUST be called BEFORE objects inserted
        public void Split(int level) {
            // Bounds.Extents.x is the same as y and z, all nodes are square
            // We want each child node to be half as big as this node
            float splitSize = Bounds.Extents.X / 2.0f;
            // Also, the constructor of the Octree node takes in the CENTER
            // position of the new node, therefore we want to create new
            // nodes from the center of this node + or - 1/4 the size of
            // this node. This is similar to BVH, but different in size.
            Vector3[] childPattern = new Vector3[] {
                new Vector3(+1f,-1f,-1f),//Right,top, Front
                new Vector3(+1f,-1f,+1f),//Right,top, Back
                new Vector3(+1f,+1f,-1f),//Right,Bottom, Front
                new Vector3(+1f,+1f,+1f),//Right,Bottom, Back
                new Vector3(-1f,-1f,-1f),//Left, Top, Front
                new Vector3(-1f,-1f,+1f),//Left, Top, Back
                new Vector3(-1f,+1f,-1f),//Left, Bottom, Front
                new Vector3(-1f,+1f,+1f),//Left, Bottom, Back
            };

            //create child nodes
            Children = new List<OctreeNode>();
            Contents = null;//no longer a leaf node
            foreach (Vector3 offset in childPattern) {
                //account for the center of current node
                Point position = new Point(Bounds.Center.ToVector() + offset * splitSize);
                OctreeNode child = new OctreeNode(position, splitSize, this);
                Children.Add(child);
            }

            //if no max depth, go recursive for all children
            if (level > 1) {
                foreach (OctreeNode child in Children) {
                    child.Split(level - 1);
                }
            }
        }

        public bool Insert(OBJ obj) {
            //bounding sphere is model space, we need world
            Sphere worldSpaceSphere = new Sphere();
            worldSpaceSphere.Position = new Point(Matrix4.MultiplyPoint(obj.WorldMatrix, obj.BoundingSphere.Position.ToVector()));
            float scale = Math.Max(obj.WorldMatrix[0, 0], Math.Max(obj.WorldMatrix[1, 1], obj.WorldMatrix[2, 2]));
            worldSpaceSphere.Radius = obj.BoundingSphere.Radius * scale;
            if (Intersects.SphereAABBIntersect(worldSpaceSphere, Bounds)) {
                //we know obj intersects node if a leaf
                //add to list, if not recurse
                if (Children != null) {
                    foreach(OctreeNode child in Children) {
                        child.Insert(obj);
                    }
                }
                else {
                    Contents.Add(obj);
                }
                return true;
            }
            return false;
        }

        public void Remove(OBJ obj) {
            if (Children != null) {
                foreach(OctreeNode child in Children) {
                    child.Remove(obj);
                }
            }
            else {
                Contents.Remove(obj);
            }
        }

        public bool Update(OBJ obj) {
            if(obj.Children != null) {
                foreach(OBJ child in obj.Children) {
                    Update(child);
                }
            }
            Remove(obj);
            return Insert(obj);
        }

        public void DebugRender() {
            if (debugVisited) {
                GL.Color3(0f, 1f, 0f);
            }
            else {
                GL.Color3(0f, 0f, 1f);
            }

            Bounds.Render();
            if (Children != null) {
                foreach(OctreeNode node in Children) {
                    node.DebugRender();
                }
            }
        }

        public OBJ Raycast(Ray ray,out float t) {
            debugVisited = true;


            //none leaf nodes
            if (Children != null) {
                foreach(OctreeNode child in Children) {
                    //AABB Ray intersection?
                    if (LinesAndRays.RaycastAABB(ray,child.Bounds,out t)){
                        //recursively call raycast
                        OBJ result = child.Raycast(ray, out t);
                        //return whatever is hit
                        if(result != null) {
                            return result;
                        }
                    }
                }
            }
            //leaf node
            //bounds already intersect ray
            else if (Contents != null) {
                //loop through all children
                foreach(OBJ content in Contents) {
                    //return first hit child
                    if(Intersects.OBJRaycast(ray,content,out t)) {
                        return content;
                    }
                }
            }
            t = 0f;
            return null;
        }
        public void DebugRenderOnlyVisitedNodes() {
            if (!debugVisited) {
                return;
            }
            if(Children != null) {
                foreach(OctreeNode node in Children) {
                    node.DebugRenderOnlyVisitedNodes();
                }
            }
            else {
                GL.Color3(0f, 1f, 0f);
            }
        }
        public int Render(Plane[] frustum) {
            int total = 0;
            //render logic
            if (!Collisions.Intersects(frustum, Bounds)) {
                return 0;
            }
            if (Contents != null) {
                foreach(OBJ content in Contents) {
                    content.NonRecursiveRender(frustum);
                    total++;
                }
            }
            //recurse through all children
            if(Children != null) {
                foreach(OctreeNode child in Children) {
                    total += child.Render(frustum);
                }
            }
            return total;
        }
        public void ResetRenderFlag() {
            if (Contents != null) {
                foreach(OBJ content in Contents) {
                    content.ResetRenderFlag();
                }
            }
            if(Children != null) {
                foreach(OctreeNode child in Children) {
                    child.ResetRenderFlag();
                }
            }
        }
    }
}

using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using System.Collections.Generic;

namespace CollisionDetectionSelector.Primitive {
    class OBJ {
        
        #region Node
        public OBJ Parent = null;
        public List<OBJ> Children = new List<OBJ>();
        #endregion
        OBJLoader model = null;

        //expose model bvhRoot
        public BVHNode BVHRoot {
            get {
                return model.BvhRoot;
            }
        }
        //end

        protected Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        protected Vector3 rotation = new Vector3(0.0f, 0.0f, 0.0f);
        protected Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

        protected Matrix4 worldMatrix;
        protected bool dirty = true;//true by default
        public Matrix4 WorldMatrix {
            get {
                if (dirty) {
                    Matrix4 translation = Matrix4.Translate(position);

                    Matrix4 pitch = Matrix4.XRotation(rotation.X);
                    Matrix4 yaw = Matrix4.YRotation(rotation.Y);
                    Matrix4 roll = Matrix4.ZRotation(rotation.Z);
                    Matrix4 orientation = roll * pitch * yaw;

                    Matrix4 scaling = Matrix4.Scale(scale);
                    worldMatrix = translation * orientation * scaling;
                }
                return worldMatrix;
            }
        }
        public Vector3 Position {
            get { return position; }
            set {
                position = value;
                dirty = true;
            }
        }
        public Vector3 Rotation {
            get { return rotation; }
            set {
                rotation = value;
                dirty = true;
            }
        }
        public Vector3 Scale {
            get { return scale; }
            set {
                scale = value;
                dirty = true;
            }
        }
        public AABB BoundingBox {
            get {
                return model.BoundingBox;
            }
        }
        public Sphere BoundingSphere {
            get {
                return model.BoundingSphere;
            }
        }
        public Triangle[] Mesh {
            get {
                return model.CollisionMesh;
            }
        }
        public OBJ(OBJLoader loader) {
            model = loader;
        }

        private void ChildrenRender(bool normal,bool bvh, bool debug) {
            if (Children != null) {
                foreach(OBJ child in Children) {
                    if (normal) {
                        child.Render();
                    }
                    else if (bvh) {
                        child.RenderBVH();
                    }
                    else if (debug) {
                        child.DebugRender();
                    }
                    if(child.Children != null) {
                        child.ChildrenRender(normal, bvh, debug);
                    }
                }
            }
        }
        public void Render() {
            GL.PushMatrix();
            //always getter
            GL.MultMatrix(WorldMatrix.OpenGL);
            model.Render();
            ChildrenRender(true, false, false);
            GL.PopMatrix();
        }
        public void DebugRender() {
            GL.PushMatrix();
            GL.MultMatrix(WorldMatrix.OpenGL);
            model.DebugRender();
            GL.PopMatrix();
        }

        #region BVH
        public void RenderBVH() {
            GL.PushMatrix();
            GL.MultMatrix(WorldMatrix.OpenGL);
            model.RenderBVH();
            GL.PopMatrix();
        }
        #endregion

        public override string ToString() {
            return "Triangle count: " + model.NumCollisionTriangles;
        }
        
    }
}

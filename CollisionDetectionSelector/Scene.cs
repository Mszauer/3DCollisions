using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;

namespace CollisionDetectionSelector {
    class Scene {
        public OBJ RootObject = new OBJ(null); //always null
        public OctreeNode Octree = null;

        public void Initialize(float octreeSize) {
            Octree = new OctreeNode(new Point(0, 0, 0), octreeSize, null);
            Octree.Split(3);
        }

        public void Render() {
            RootObject.Render();

            GL.Disable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            Octree.DebugRender();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            Octree.DebugRenderOnlyVisitedNodes();

            GL.Enable(EnableCap.Lighting);
        }
        
        public OBJ Raycast(Ray ray, out float t) {
            return Octree.Raycast(ray, out t);
        }
        public OBJ Raycast(Ray ray) {
            float t = 0.0f;
            return Octree.Raycast(ray, out t);
        }
        
    }
}

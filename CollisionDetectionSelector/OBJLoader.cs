using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using CollisionDetectionSelector.Primitive;
using Math_Implementation;

class OBJLoader {
    #region BVH
    protected BVHNode bvhRoot = null;
    public BVHNode BvhRoot {
        get {
            return bvhRoot;
        }
    }
    //non-recursive public api
    public void RenderBVH() {
        RenderBVH(bvhRoot);
    }
    //recursive private API
    private void RenderBVH(BVHNode node) {
        node.AABB.Render();
        if(node.Children!= null) {
            foreach(BVHNode child in node.Children) {
                RenderBVH(child);
            }
        }
    }
    
    #endregion
    public int NumCollisionTriangles {
        get {
            return collisionMesh.Length;
        }
    }
    public AABB BoundingBox {
        get {
            return containerAABB;
        }
    }
    public Sphere BoundingSphere {
        get {
            return containerSphere;
        }
    }

    protected int vertexBuffer = -1;

    protected bool hasNormals = false;
    protected bool hasUvs = false;

    protected int numVerts = 0;
    protected int numNormals = 0;
    protected int numUvs = 0;

    protected string materialPath = null;

    protected Triangle[] collisionMesh = null;
    protected AABB containerAABB = null;
    protected Sphere containerSphere = null;

    public Triangle[] CollisionMesh {
        get {
            return collisionMesh;
        }
    }
    public OBJLoader(string path) {
        List<float> vertices = new List<float>();
        List<float> normals = new List<float>();
        List<float> texCoord = new List<float>();

        List<uint> vertIndex = new List<uint>();
        List<uint> normIndex = new List<uint>();
        List<uint> uvIndex = new List<uint>();

        List<float> vertexData = new List<float>();
        List<float> normalData = new List<float>();
        List<float> uvData = new List<float>();

        using (TextReader reader = File.OpenText(path)) {
            string line = reader.ReadLine();
            while (line != null ) {
                if (string.IsNullOrEmpty(line)||line.Length < 2) {
                    continue;
                }
                line.ToLower();
                string[] content = line.Split(' ');
                if (content[0] == "usemtl") {
                    materialPath = content[1];
                }
                else if (content[0] == "v") {
                    //add vertex
                        //vertices
                        vertices.Add(System.Convert.ToSingle(content[1]));
                        vertices.Add(System.Convert.ToSingle(content[2]));
                        vertices.Add(System.Convert.ToSingle(content[3]));
                }
                else if (content[0] == "vt") {
                    // vertex texture
                    texCoord.Add(System.Convert.ToSingle(content[1]));
                    texCoord.Add(System.Convert.ToSingle(content[2]));

                }
                else if (content[0] == "vn") {
                    //vertex normal
                    //normals
                    normals.Add(System.Convert.ToSingle(content[1]));
                    normals.Add(System.Convert.ToSingle(content[2]));
                    normals.Add(System.Convert.ToSingle(content[3]));
                }
                else if (content[0] == "f") {
                    //face
                    for (int i = 1; i < content.Length ; i++) { //loop through values
                        string[] subsplit = content[i].Split('/'); // split based on /
                        if (!string.IsNullOrEmpty(subsplit[0])) {
                            //vertindex
                            vertIndex.Add(System.Convert.ToUInt32(subsplit[0]) -1);
                        }
                        if (!string.IsNullOrEmpty(subsplit[1])) {
                            //uvindex
                            uvIndex.Add(System.Convert.ToUInt32(subsplit[1]) - 1);
                        }
                        if (!string.IsNullOrEmpty(subsplit[2])) {
                            //normindex
                            normIndex.Add(System.Convert.ToUInt32(subsplit[2]) - 1);
                        }
                    }
                }
                else if (content[0] == "s") {
                    //specular
                }

                line = reader.ReadLine();
            }
                
        }
        for (int i = 0; i < vertIndex.Count; i++) {
            vertexData.Add(vertices[(int)vertIndex[i] * 3 + 0]);
            vertexData.Add(vertices[(int)vertIndex[i] * 3 + 1]);
            vertexData.Add(vertices[(int)vertIndex[i] * 3 + 2]);
        }
        for (int i = 0; i < normIndex.Count; i++) {
            normalData.Add(normals[(int)normIndex[i] * 3 + 0]);
            normalData.Add(normals[(int)normIndex[i] * 3 + 1]);
            normalData.Add(normals[(int)normIndex[i] * 3 + 2]);
        }
        for (int i = 0; i < uvIndex.Count; i++) {
            uvData.Add(texCoord[(int)uvIndex[i] * 2 + 0]);
            uvData.Add(texCoord[(int)uvIndex[i] * 2 + 1]);
        }

        //create AABB container
        containerAABB = new AABB(new Point(vertices[0], vertices[1], vertices[2]), new Point(vertices[0], vertices[1], vertices[2]));
        for (int i = 0; i < vertexData.Count; i += 3) { //vertexData.Count-2 because we pre-check ahead
            if (vertexData[i+0] < containerAABB.Min.X) {
                containerAABB.Min.X = vertexData[i + 0];
            }
            if (vertexData[i + 0] > containerAABB.Max.X) {
                containerAABB.Max.X = vertexData[i + 0];
            }
            if (vertexData[i + 1] < containerAABB.Min.Y) {
                containerAABB.Min.Y = vertexData[i + 1];
            }
            if (vertexData[i + 1] > containerAABB.Max.Y) {
                containerAABB.Max.Y = vertexData[i + 1];
            }
            if (vertexData[i + 2] < containerAABB.Min.Z) {
                containerAABB.Min.Z = vertexData[i + 2];
            }
            if (vertexData[i + 2] > containerAABB.Max.Z) {
                containerAABB.Max.Z = vertexData[i + 2];
            }
        }
        //end

        //create bvh root
        bvhRoot = new BVHNode(containerAABB);
        //end

        //Create wireframe model and triangles
        collisionMesh = new Triangle[vertexData.Count / 9];


        for (int i = 0; i < vertexData.Count; i += 9) {
            Point p0, p1, p2;

            p0 = new Point(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

            int j = i + 3;
            p1 = new Point(vertexData[j], vertexData[j + 1], vertexData[j + 2]);

            int k = j + 3;
            p2 = new Point(vertexData[k], vertexData[k + 1], vertexData[k + 2]);

            collisionMesh[i / 9] = new Triangle(p0, p1, p2);// i / 9 == increments of 1

            //add triangles to bvh root
            bvhRoot.Triangles.Add(collisionMesh[i / 9]);

        }
        //end

        //BVHNode split + shake
        bvhRoot.Split();
        bvhRoot.Shake();
        //end

        //create containerSphere
        float maxRadiusSq = containerAABB.Extents.LengthSquared();
        float minRadiusSq = new Vector3(containerAABB.Center.X - containerAABB.Min.X, containerAABB.Center.Y - containerAABB.Min.Y, containerAABB.Center.Z - containerAABB.Min.Z).LengthSquared();

        containerSphere = new Sphere(containerAABB.Center,(float)System.Math.Sqrt(System.Math.Max(maxRadiusSq,minRadiusSq)));
        
        //end
        hasNormals = normalData.Count > 0;
        hasUvs = uvData.Count > 0;

        numVerts = vertexData.Count;
        numUvs = uvData.Count;
        numNormals = normalData.Count;

        List<float> data = new List<float>();//create linear array of data
        data.AddRange(vertexData); //add everything from vertData
        data.AddRange(normalData);//add everything from normData
        data.AddRange(uvData);//add everything from uvData

        vertexBuffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);//bind array
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Count * sizeof(float)), data.ToArray(), BufferUsageHint.StaticDraw);//transfer array to gpu
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//release array
    }
    public void Destroy() {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        GL.DeleteBuffer(vertexBuffer);

        vertexBuffer = -1;
    }
    public void Render(bool useNormals = true, bool useTextures = true) {
        if (vertexBuffer == -1) {
            return;
        }
        if (!hasNormals) {
            useNormals = false;
        }
        if (!hasUvs) {
            useTextures = false;
        }
        //enable client states , check arguments
        GL.EnableClientState(ArrayCap.VertexArray);
        if (hasNormals) {
            GL.EnableClientState(ArrayCap.NormalArray);
        }
        if (hasUvs) {
            GL.EnableClientState(ArrayCap.TextureCoordArray);
        }
        //bind array buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer,vertexBuffer);
        //set pointers
        GL.VertexPointer(3, VertexPointerType.Float, 0, new System.IntPtr(0));
        if (hasNormals) {
            GL.NormalPointer(NormalPointerType.Float, 0, new System.IntPtr(numVerts * sizeof(float)));
        }
        if (hasUvs) {
            GL.TexCoordPointer(2, TexCoordPointerType.Int, 0, new System.IntPtr((numVerts + numNormals) * sizeof(float)));
        }

        //call GL.DrawArrays, always triangles
        GL.DrawArrays(PrimitiveType.Triangles, 0, numVerts);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        if (hasUvs) {
            GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
        if (hasNormals) {
            GL.DisableClientState(ArrayCap.NormalArray);
        }
        GL.DisableClientState(ArrayCap.VertexArray);
    }
    public void DebugRender() {
        containerAABB.Render();
        containerSphere.Render();
        foreach(Triangle triangle in collisionMesh) {
            triangle.Render();
        }
    }
}
using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;
using CollisionDetectionSelector;

namespace CollisionDetectionSelector.Samples {
    class SceneSample01 : Application {
        Scene scene = new Scene();
        OBJ cubeNode = null;

        OBJLoader suzane = null;
        OBJLoader cube = null;
        OBJLoader torus = null;

        Ray[] rays = new Ray[] {
            new Ray(new Point(0.0f, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)),
            new Ray(new Point(-1f, -3f, -5f), new Vector3(0f, 0f, 1f))
        };

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.PointSize(5f);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.0f, 0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });

            suzane = new OBJLoader("Assets/suzanne.obj");
            cube = new OBJLoader("Assets/cube.obj");
            torus = new OBJLoader("Assets/torus.obj");

            OBJ node = new OBJ(suzane);
            node.Parent = scene.RootObject;
            node.Parent.Children.Add(node);
            node.Position = new Vector3(2.0f, 0.0f, 0.0f);

            node = new OBJ(suzane);
            node.Parent = scene.RootObject.Children[0]; // suzane reference
            node.Parent.Children.Add(node);
            node.Position = new Vector3(-2.0f, -1.0f, 0.0f);
            node.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            node.Rotation = new Vector3(90.0f, 0f, 0f);
            Matrix4 m = node.WorldMatrix;

            node = new OBJ(suzane);
            node.Parent = scene.RootObject.Children[0]; // suzane reference
            node.Parent.Children.Add(node);
            node.Position = new Vector3(0.0f, 0.0f, -2.0f);
            node.Rotation = new Vector3(0f, 180f, 0f);

            node = new OBJ(cube);
            node.Parent = scene.RootObject;
            node.Parent.Children.Add(node);
            node.Position = new Vector3(-2.0f, 3.0f, 1.0f);
            node.Rotation = new Vector3(45.0f, 0.0f, 0.0f);
            cubeNode = node;

            node = new OBJ(torus);
            node.Parent = cubeNode;
            node.Parent.Children.Add(node);
            node.Position = new Vector3(0.0f, 0.0f, -1.0f);

            bool[] b_res = new bool[] { true, false };
            float[] t_res = new float[] { 2.730469f, 0f };

            float t = 0.0f;
            for (int i = 0; i < rays.Length; ++i) {
                bool result = scene.Raycast(rays[i], out t) != null;
                if (result != b_res[i]) {
                    System.Console.WriteLine("ray " + i + ", expected: " + b_res[i].ToString() + ", got: " + result.ToString());
                }
                if (!CMP(t, t_res[i])) {
                    System.Console.WriteLine("ray " + i + "t, expected: " + t_res[i].ToString() + ", got: " + t.ToString());
                }
            }
        }

        bool CMP(float x, float y) {
            return System.Math.Abs(x - y) < 0.00001f;
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Enable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            scene.Render();
            GL.Disable(EnableCap.Lighting);

            foreach (Ray r in rays) {
                if (scene.Raycast(r) != null) {
                    GL.Color3(1f, 0f, 0f);
                }
                else {
                    GL.Color3(0f, 0f, 1f);
                }
                r.Render();
            }
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            Vector3 cubeRotation = cubeNode.Rotation;
            cubeRotation.X += 45.0f * deltaTime;
            cubeNode.Rotation = cubeRotation;
        }
    }
}
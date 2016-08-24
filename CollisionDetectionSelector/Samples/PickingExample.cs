using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;
using CollisionDetectionSelector;

namespace CollisionDetectionSelector.Samples {
    class SceneSample04 : Application {
        Scene scene = new Scene();

        OBJLoader cube = null;

        Sphere debugSphere = new Sphere(new Point(), 0.5f);
        Ray debugRay = new Ray();
        AABB debugBox = null;

        Matrix4 modelView = new Matrix4();
        Matrix4 projection = new Matrix4();
        float[] viewport = new float[4];

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.PointSize(5f);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.5f, -0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });

            scene.Initialize(7f);

            cube = new OBJLoader("Assets/cube.obj");

            // Because the debug AABB we are actually using for picking has no lighting
            // let's actually render an OBJ in the exact position of it!
            scene.RootObject.Children.Add(new OBJ(cube));
            scene.RootObject.Children[0].Parent = scene.RootObject;
            scene.RootObject.Children[0].Position = new Vector3(3f, -7f, -1f);
            scene.RootObject.Children[0].Scale = new Vector3(7f, 5f, 4f);

            // Gotta set the World space points of the debug AABB
            // to the same thing as the visual node!
            Matrix4 world = scene.RootObject.Children[0].WorldMatrix;
            debugBox = scene.RootObject.Children[0].BoundingBox; // TEMP
            Vector3 newMin = Matrix4.MultiplyPoint(world, debugBox.Min.ToVector());
            Vector3 newMax = Matrix4.MultiplyPoint(world, debugBox.Max.ToVector());
            // Construct the final collision box
            debugBox = new AABB(new Point(newMin), new Point(newMax));
        }

        public override void Render() {
            base.Render();
            DrawOrigin();

            GL.Enable(EnableCap.Lighting);
            scene.Render(false);
            GL.Disable(EnableCap.Lighting);

            float[] rawModelView = new float[16];
            GL.GetFloat(GetPName.ModelviewMatrix, rawModelView);

            float[] rawProjection = new float[16];
            GL.GetFloat(GetPName.ProjectionMatrix, rawProjection);

            GL.GetFloat(GetPName.Viewport, viewport);

            modelView = Matrix4.Transpose(new Matrix4(rawModelView));
            projection = Matrix4.Transpose(new Matrix4(rawProjection));

            GL.Color3(1f, 0f, 0f);
            debugSphere.Render();
        }

        public override void Update(float deltaTime) {
            // Don't rotate the scene
            //base.Update(deltaTime);

            float[] viewport = new float[] { 0f, 0f, Window.Width, Window.Height };

            Vector3 near = Matrix4.Unproject(new Vector3(Window.Mouse.X, Window.Mouse.Y, 0.0f), modelView, projection, viewport);
            Vector3 far = Matrix4.Unproject(new Vector3(Window.Mouse.X, Window.Mouse.Y, 1.0f), modelView, projection, viewport);

            debugRay = new Ray(near, Vector3.Normalize(far - near));
            Point p = new Point();
            if (LinesAndRays.RaycastAABB(debugRay, debugBox, out p)) {
                debugSphere.Position = p;
            }
            else {
                // Off-screen
                debugSphere.Position = new Point(-5000, -5000, -5000);
            }
        }
    }
}
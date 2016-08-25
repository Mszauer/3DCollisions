using OpenTK.Graphics.OpenGL;
using Math_Implementation;
using CollisionDetectionSelector.Primitive;
using CollisionDetectionSelector;

namespace CollisionDetectionSelector.Samples {
    class CameraSample : Application {
        Scene scene = new Scene();
        OBJLoader cube = null;
        Camera camera = new Camera();

        void AddCubeToSceneRoot(Vector3 position, Vector3 scale) {
            scene.RootObject.Children.Add(new OBJ(cube));
            int count = scene.RootObject.Children.Count - 1;
            scene.RootObject.Children[count].Parent = scene.RootObject;
            scene.RootObject.Children[count].Position = position;
            scene.RootObject.Children[count].Scale = scale;

            // Record object with spacial partitioning tree
            scene.Octree.Insert(scene.RootObject.Children[count]);
        }

        public override void Initialize(int width, int height) {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.PointSize(5f);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0.5f, -0.5f, 0.5f, 0.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0f, 1f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1f, 1f, 1f, 1f });

            scene.Initialize(70f);
            cube = new OBJLoader("Assets/cube.obj");

            AddCubeToSceneRoot(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(50.0f, 1.0f, 50.0f));
            for (int i = 0; i < 10; ++i) {
                for (int j = 0; j < 10; ++j) {
                    AddCubeToSceneRoot(new Vector3(25 - i * 5, 0.0f, 25 - j * 5), new Vector3(1.0f, 5.0f, 1.0f));
                }
            }

            // Set the camera look at!
            camera.LookAt(new Vector3(50.0f, 20.0f, 50.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            // Unit tests!
            Matrix4 expectedTrans = new Matrix4();
            expectedTrans[0, 3] = expectedTrans[2, 3] = 50f;
            expectedTrans[1, 3] = 20f;
            if (expectedTrans != camera.Translation) {
                System.Console.WriteLine("Error! Translation matrix is wrong!");
            }

            Matrix4 expectedRot = new Matrix4();
            expectedRot[0, 0] = 0.7071068f;
            expectedRot[2, 0] = -expectedRot[0, 0];
            expectedRot[1, 1] = 0.9622504f;
            expectedRot[2, 2] = expectedRot[0, 2] = 0.6804138f;
            expectedRot[0, 1] = expectedRot[2, 1] = -0.1924501f;
            expectedRot[1, 2] = 0.2721655f;
            if (expectedRot != camera.Orientation) {
                System.Console.WriteLine("Error! Rotation matrix is wrong!");
            }

            Matrix4 camView = camera.ViewMatrix;
            Matrix4 matView = Matrix4.LookAt(new Vector3(50, 20, 50), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            if (camView != matView) {
                System.Console.WriteLine("ERROR! expected camera and matrix view's to be the same!");
            }
            //frustum
            Plane[] frustum = new Plane[6];
            frustum[0] = new Plane(new Vector3(0.2381448f, -0.2721655f, -1.598973f), 44.82524f);
            frustum[1] = new Plane(new Vector3(-1.598973f, -0.2721655f, 0.2381448f), 44.82524f);
            frustum[2] = new Plane(new Vector3(-1.013747f, 1.394501f, -1.013747f), 36.74234f);
            frustum[3] = new Plane(new Vector3(-0.3470805f, -1.938832f, -0.3470805f), 36.74234f);
            frustum[4] = new Plane(new Vector3(-1.360841f, -0.5443366f, -1.360841f), 73.4747f);
            frustum[5] = new Plane(new Vector3((float)1.364946E-05, (float)5.453825E-06, (float)1.364946E-05), 923.8687f);

            for (int i = 0; i < 6; ++i) {
                if (camera.Frustum[i].Normal != frustum[i].Normal) {
                    if (Vector3.Normalize(camera.Frustum[i].Normal) == Vector3.Normalize(frustum[i].Normal)) {
                        System.Console.WriteLine("\tLooks like the normal of your frustum plane is NOT NORMALIZED!");
                    }
                    else {
                        System.Console.WriteLine("Detected error in frustum, plane " + i);
                    }
                    System.Console.WriteLine("\tExpected: " + camera.Frustum[i].Normal);
                    System.Console.WriteLine("\tGot: " + frustum[i].Normal);
                }
                if (System.Math.Abs(camera.Frustum[i].Distance - frustum[i].Distance) > 0.0001f) {
                    System.Console.WriteLine("Wrong distance for plane: " + i);
                    System.Console.WriteLine("\tExpected: " + camera.Frustum[i].Distance);
                    System.Console.WriteLine("\tGot: " + frustum[i].Distance);
                }
            }
            //Point in frustum
            if (!Collisions.Intersects(new Point(), camera.Frustum)) {
                System.Console.WriteLine("Error with point in frustum! 0");
            }
            if (Collisions.Intersects(new Point(-5000, -5000, -5000), camera.Frustum)) {
                System.Console.WriteLine("Error with point in frustum! 1");
            }
            if (!Collisions.Intersects(new Point(2, 30, 4), camera.Frustum)) {
                System.Console.WriteLine("Error with point in frustum! 2");
            }
            //Sphere in frustum
            if (!Collisions.Intersects(new Sphere(), camera.Frustum)) {
                System.Console.WriteLine("Error with sphere in frustum! 0");
            }
            if (Collisions.Intersects(new Sphere(-5000, -5000, -5000, 1f), camera.Frustum)) {
                System.Console.WriteLine("Error with sphere in frustum! 1");
            }
            if (!Collisions.Intersects(new Sphere(2, 30, 4, 4f), camera.Frustum)) {
                System.Console.WriteLine("Error with sphere in frustum! 2");
            }
            //AABB in frustum
            if (!Collisions.Intersects(new AABB(), camera.Frustum)) {
                System.Console.WriteLine("Error with aabb in frustum! 0");
            }
            if (Collisions.Intersects(new AABB(new Point(-650, -650, -650), new Point(-600, -600, -600)), camera.Frustum)) {
                System.Console.WriteLine("Error with aabb in frustum! 1");
            }
            if (!Collisions.Intersects(new AABB(new Point(2, 30, 4), new Point(25, 35, 45)), camera.Frustum)) {
                System.Console.WriteLine("Error with aabb in frustum! 2");
            }
        }
        public override void Update(float deltaTime) {
            float xDelta = (float)Window.Mouse.XDelta / (float)Window.Width;
            float yDelta = (float)Window.Mouse.YDelta / (float)Window.Height;
            float zoom = (float)Window.Mouse.WheelDelta;

            if (Window.Mouse[OpenTK.Input.MouseButton.Left]) {
                if (xDelta != 0.0f || yDelta != 0.0f) {
                    float xPan = xDelta * deltaTime * -900.0f;
                    float yPan = yDelta * deltaTime * 900.0f;
                    camera.Pan(xPan, yPan);
                }
            }

            if (zoom != 0) {
                zoom = zoom < 0 ? -1f : zoom;
                zoom = zoom > 0 ? 1f : zoom;
                camera.Zoom(zoom * deltaTime * 50.0f);
            }

            if (Window.Mouse[OpenTK.Input.MouseButton.Right]) {
                if (xDelta != 0.0f || yDelta != 0.0f) {
                    float xPan = xDelta * deltaTime * -900.0f;
                    float yPan = yDelta * deltaTime * 900.0f;
                    camera.Pivot(xPan, yPan);
                }
            }
        }
        public override void Render() {
            GL.LoadMatrix(camera.ViewMatrix.OpenGL);
            DrawOrigin();

            GL.Enable(EnableCap.Lighting);
            int numRendered = scene.Octree.Render(camera.Frustum);
            scene.Octree.ResetRenderFlag();
            Window.Title = "Rendered: " + numRendered;
            GL.Disable(EnableCap.Lighting);
        }
    }
}
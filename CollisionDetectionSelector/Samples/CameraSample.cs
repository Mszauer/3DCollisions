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

            scene.Initialize(7f);
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
            //Matrix4 viewMat = Matrix4.LookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            GL.LoadMatrix(/*viewMat.OpenGL*/camera.ViewMatrix.OpenGL);
            DrawOrigin();

            GL.Enable(EnableCap.Lighting);
            scene.Render(false);
            GL.Disable(EnableCap.Lighting);
        }
    }
}
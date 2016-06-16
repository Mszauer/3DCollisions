using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

    public class Application {
        public static Application Instance {
            get; private set;
        }
        public OpenTK.GameWindow Window = null;
        private Math_Implementation.Vector3 _baseCameraAngle = new Math_Implementation.Vector3(120.0f, -10.0f, 20.0f);
        private float _baseRads = (float)(System.Math.PI / 180.0f);

        public Application() {
            Instance = this;
        }

        #region CommonMethods
        public void LogError(string error) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + error);
            Console.ResetColor();
        }

        public void DrawOrigin() {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);
            GL.End();
        }
        #endregion

    //logic driving functions that are overwritten
    #region InheritableLogic
    public virtual void Initialize(int width,int height) {

        }

        public virtual void Update(float dTime) {
            _baseCameraAngle.X += 45.0f * dTime;
        }
        public virtual void Render() {
        // Again, Vector3 and Matrix4 must be fully scoped, because
        // the OpenTK namespace contains both classes already
        Math_Implementation.Vector3 eyePos = new Math_Implementation.Vector3();
        eyePos.X = _baseCameraAngle.Z * -(float)System.Math.Sin(_baseCameraAngle.X * _baseRads * (float)System.Math.Cos(_baseCameraAngle.Y * _baseRads));
        eyePos.Y = _baseCameraAngle.Z * -(float)System.Math.Sin(_baseCameraAngle.Y * _baseRads);
        eyePos.Z = -_baseCameraAngle.Z * (float)System.Math.Cos(_baseCameraAngle.X * _baseRads * (float)System.Math.Cos(_baseCameraAngle.Y * _baseRads));

        Math_Implementation.Matrix4 lookAt =
            Math_Implementation.Matrix4.LookAt(eyePos,
            new Math_Implementation.Vector3(0.0f, 0.0f, 0.0f),
            new Math_Implementation.Vector3(0.0f, 1.0f, 0.0f));
        GL.LoadMatrix(Math_Implementation.Matrix4.Transpose(lookAt).Matrix);
    }
        public virtual void Shutdown() {

        }
    // Has a default implementation, making the need to override in child
    // classes purley optional
    public virtual void Resize(int width, int height) {
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            float aspect = (float)width / (float)height;
            Math_Implementation.Matrix4 perspective =
                Math_Implementation.Matrix4.Perspective(60, aspect, 0.01f, 1000.0f);
            GL.LoadMatrix(Math_Implementation.Matrix4.Transpose(perspective).Matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    #endregion

    #region EntryPoint

    //entry point of application, not overwritten
    public virtual void Main(string[] args) {
            Window = new OpenTK.GameWindow();
            Window.Load += new EventHandler<EventArgs>(OpenTKInitialize);
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(OpenTKUpdate);
            Window.RenderFrame += new EventHandler<FrameEventArgs>(OpenTKRender);
            Window.Unload += new EventHandler<EventArgs>(OpenTKShutdown);
            Window.Title = "Sample Application";
            Window.ClientSize = new System.Drawing.Size(800, 600);
            Instance.Resize(800, 600);
            Window.VSync = VSyncMode.On;
            Window.Run(60.0f);
            Window.Dispose();
        }

        #endregion

        // if default entry point, default functions
        #region OpenTKCallbacks

        private void OpenTKInitialize(object sender, EventArgs e) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("OpenGL Vendor: " + GL.GetString(StringName.Vendor));
            Console.WriteLine("OpenGL Renderer: " + GL.GetString(StringName.Renderer));
            Console.WriteLine("OpenGL Version: " + GL.GetString(StringName.Version));
            Console.ResetColor();
            Instance.Initialize(800, 600);
        }

        private void OpenTKUpdate(object sender, FrameEventArgs e) {
            float dTime = (float)e.Time;
            Instance.Update(dTime);
        }

        private void OpenTKRender(object sender, FrameEventArgs e) {
            GL.ClearColor(Color.CadetBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            Instance.Render();
            Window.SwapBuffers();
        }

        private void OpenTKShutdown(object sender, EventArgs e) {
            Instance.Shutdown();
        }

        private void OpenTKResize(EventArgs e) {
            Instance.Resize(Instance.Window.Width, Instance.Window.Height);
        }
        #endregion

    }
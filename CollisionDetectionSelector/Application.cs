using System;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

    class Application {
        public static Application Instance {
            get; private set;
        }
        public OpenTK.GameWindow Window = null;

        public Application() {
            Instance = this;
        }

        //logic driving functions that are overwritten
        #region InheritableLogic
        public virtual void Initialize(int width,int height) {

        }
        public virtual void Resize(int width,int height) {

        }
        public virtual void Update(float dTime) {

        }
        public virtual void Render() {

        }
        public virtual void Shutdown() {

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
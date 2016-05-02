using Math_Implementation;
using OpenTK.Graphics.OpenGL;

class Point {
    protected Vector3 position = new Vector3();
    public Vector3 Position {
        get;//todo
        set;//todo
    }
    public float X { get; set; }//todo
    public float Y { get; set; }//todo
    public float Z { get; set; }//todo
    public Point() {
        //todo
    }
    public Point(float x,float y,float z) {
        //todo
    }
    public Point(Vector3 v) {
        //todo make new
    }
    public void FromVector(Vector3 v) {
        //todo make new
    }
    #region Rendering
    public void Render() {
        GL.Begin(PrimitiveType.Points);
        GL.Vertex3(Position.X, Position.Y, Position.Z);
        GL.End();
    }
    public override string ToString() {
        return "(" + Position.X + ", " + Position.Y + ", " + Position.Z + ")";
    }
    #endregion
}

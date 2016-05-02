using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Implementation {
    class MatrixStack {
        protected List<Matrix4> stack = null;
        public int Count {
            get {
                if (stack != null) {
                    return 0;
                }
                return stack.Count;
            }
        }
        public Matrix4 this[int i] {
            get {
                return stack[i];
            }

        }
        public MatrixStack() {
            stack = new List<Matrix4>();
            stack.Add(new Matrix4());
        }
        public void Push() {
            //new matrix
            Matrix4 top = new Matrix4();
            //copy values of top matrix
            for (int i = 0; i < stack[stack.Count - 1].Matrix.Length; i++) {
                top.Matrix[i] = stack[stack.Count-1].Matrix[i];
            }
            //add new copy to top
            stack.Add(top);
        }
        public void Load(Matrix4 matrix) {
            for (int i = 0; i <matrix.Matrix.Length; i++) {
                stack[stack.Count-1].Matrix[i] = matrix.Matrix[i];
            }
        }
        public void Mul(Matrix4 matrix) {
            stack[stack.Count - 1] = stack[stack.Count - 1] * matrix;
        }
        public void Pop() {
            stack.RemoveAt(stack.Count - 1);
        }
        public float[] OpenGL {
            get {
                return stack[stack.Count - 1].OpenGL;
            }
        }
    }
}

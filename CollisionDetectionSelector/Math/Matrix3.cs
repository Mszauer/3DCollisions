using System;

namespace Math_Implementation {
    class Matrix3 {
        //i = row, j = col
        public float[] Matrix = null;
        public float this[int i,int j] {
            get {
                return Matrix[(i * 3) + j];
            }
            set {
                Matrix[(i * 3) + j] = value;
            }
        }
        public float this[int i] {
            get { return Matrix[i]; }
            set { Matrix[i] = value; }
        }
        public Matrix3() {
            Matrix = new float[] {1,0,0,
                                  0,1,0,
                                  0,0,1 };
        }
        public Matrix3(params float[] values) {
            Matrix = new float[9];
            if (values.Length != 9) {
                Console.WriteLine("Invalid amount of values for Matrix3, Values length: " + values.Length);
                throw new System.Exception();
            }
            for (int i = 0; i < 9; i++) {
                Matrix[i] = values[i];
            }
        }
        //scalar operators
        public static Matrix3 operator +(Matrix3 matrixA,Matrix3 matrixB) {
            Matrix3 result = null;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix3 operator -(Matrix3 matrixA, Matrix3 matrixB) {
            Matrix3 result = null;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result[i, j] = matrixA[i, j] - matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix3 operator /(Matrix3 matrixA, Matrix3 matrixB) {
            Matrix3 result = null;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result[i, j] = matrixA[i, j] / matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix3 operator *(Matrix3 matrixA, float scale) {
            Matrix3 result = new Matrix3();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result[i, j] = matrixA[i, j] * scale;
                }
            }
            return result;
        }
        private static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.00001;
        }
        public static bool operator ==(Matrix3 matrixA, Matrix3 matrixB) {
            for (int i = 0; i < 3; i++) {
                for(int j = 0; j < 3; j++) {
                    if (!Fequal(matrixA[i,j],matrixB[i, j])) {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator != (Matrix3 matrixA,Matrix3 matrixB) {
            return !(matrixA == matrixB);
        }
        //Matrix Vector multiplication
        public static Vector3 operator *(Matrix3 matrix,Vector3 vector) {
            Vector3 result = new Vector3();
            result[0] = (matrix[0, 0] * vector[0]) + (matrix[0, 1] * vector[1]) + (matrix[0, 2] * vector[2]);
            result[1] = (matrix[1, 0] * vector[0]) + (matrix[1, 1] * vector[1]) + (matrix[1, 2] * vector[2]);
            result[2] = (matrix[2, 0] * vector[0]) + (matrix[2, 1] * vector[1]) + (matrix[2, 2] * vector[2]);
            return result;
        }
        //Matrix multiplication
        public static Matrix3 operator *(Matrix3 matrixA,Matrix3 matrixB) {
            Matrix3 result = new Matrix3();
            result[0, 0] = matrixA[0, 0] * matrixB[0, 0] + matrixA[0, 1] * matrixB[1, 0] + matrixA[0, 2] * matrixB[2, 0];
            result[0, 1] = matrixA[0, 0] * matrixB[0, 1] + matrixA[0, 1] * matrixB[1, 1] + matrixA[0, 2] * matrixB[2, 1];
            result[0, 2] = matrixA[0, 0] * matrixB[0, 2] + matrixA[0, 1] * matrixB[1, 2] + matrixA[0, 2] * matrixB[2, 2];
            result[1, 0] = matrixA[1, 0] * matrixB[0, 0] + matrixA[1, 1] * matrixB[1, 0] + matrixA[1, 2] * matrixB[2, 0];
            result[1, 1] = matrixA[1, 0] * matrixB[0, 1] + matrixA[1, 1] * matrixB[1, 1] + matrixA[1, 2] * matrixB[2, 1];
            result[1, 2] = matrixA[1, 0] * matrixB[0, 2] + matrixA[1, 1] * matrixB[1, 2] + matrixA[1, 2] * matrixB[2, 2];
            result[2, 0] = matrixA[2, 0] * matrixB[0, 0] + matrixA[2, 1] * matrixB[1, 0] + matrixA[2, 2] * matrixB[2, 0];
            result[2, 1] = matrixA[2, 0] * matrixB[0, 1] + matrixA[2, 1] * matrixB[1, 1] + matrixA[2, 2] * matrixB[2, 1];
            result[2, 2] = matrixA[2, 0] * matrixB[0, 2] + matrixA[2, 1] * matrixB[1, 2] + matrixA[2, 2] * matrixB[2, 2];
            return result;
        }

        //Transpose, row = col, col = row
        public static Matrix3 Transpose(Matrix3 matrix) {
            Matrix3 result = new Matrix3();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    result[i, j] = matrix[j, i];
                }
            }
            return result;
        }

        //Determinant
        public static float Determinant(Matrix3 matrix) {
            return (matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]))
                 - (matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]))
                 + (matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]));
        }
        public float Determinant() {
            return (this[0, 0] * (this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1]))
                 - (this[0, 1] * (this[1, 0] * this[2, 2] - this[1, 2] * this[2, 0]))
                 + (this[0, 2] * (this[1, 0] * this[2, 1] - this[1, 1] * this[2, 0]));
        }

        //Minor
        public static Matrix3 Minor(Matrix3 matrix) {
            Matrix3 result = new Matrix3();
            result[0, 0] = matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1];
            result[0, 1] = matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0];
            result[0, 2] = matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0];
            result[1, 0] = matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1];
            result[1, 1] = matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0];
            result[1, 2] = matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0];
            result[2, 0] = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
            result[2, 1] = matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0];
            result[2, 2] = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            return result;
        }
        public void Minor() {
            Matrix3 matrix = this;
            this[0, 0] = matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1];
            this[0, 1] = matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0];
            this[0, 2] = matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0];
            this[1, 0] = matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1];
            this[1, 1] = matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0];
            this[1, 2] = matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0];
            this[2, 0] = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
            this[2, 1] = matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0];
            this[2, 2] = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }
        //cofactor
        public static Matrix3 CoFactor(Matrix3 matrix) {
            Matrix3 result = new Matrix3();
            result[0, 0] = (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1]) * 1;
            result[0, 1] = (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0]) * -1;
            result[0, 2] = (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]) * 1;
            result[1, 0] = (matrix[0, 1] * matrix[2, 2] - matrix[0, 2] * matrix[2, 1]) * -1;
            result[1, 1] = (matrix[0, 0] * matrix[2, 2] - matrix[0, 2] * matrix[2, 0]) * 1;
            result[1, 2] = (matrix[0, 0] * matrix[2, 1] - matrix[0, 1] * matrix[2, 0]) * -1;
            result[2, 0] = (matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1]) * 1;
            result[2, 1] = (matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0]) * -1;
            result[2, 2] = (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]) * 1;
            return result;
        }
        //adjugate
        public static Matrix3 Adjugate(Matrix3 matrix) {
            Matrix3 result = new Matrix3();
            result = Matrix3.Transpose(CoFactor(matrix));
            return result;
        }
        //Inverse
        public static Matrix3 Inverse(Matrix3 matrix) {
            Matrix3 result = Adjugate(matrix);
            float determinant = Determinant(matrix);
            result *= 1.0f/determinant;
            return result;
        }
        public static Matrix3 Scale(float x, float y, float z) {
            return new Matrix3(x, 0, 0,
                               0, y, 0,
                               0, 0, z);
        }
        public static Matrix3 Scale(Vector3 xyz) {
            return new Matrix3(xyz[0], 0, 0,
                               0, xyz[1], 0,
                               0, 0, xyz[2]);
        }
        public static Matrix3 XRotation(float theta) {
            theta = theta * (float)(Math.PI / 180); //convert to rads
            Matrix3 result = new Matrix3(1, 0, 0,
                                         0, (float)Math.Cos(theta), -(float)Math.Sin(theta),
                                         0, (float)Math.Sin(theta), (float)Math.Cos(theta));
            return result;
        }
        public static Matrix3 YRotation(float theta) {
            theta = theta * (float)(Math.PI / 180);
            Matrix3 result = new Matrix3((float)Math.Cos(theta), 0, (float)Math.Sin(theta),
                                         0, 1, 0,
                                         -(float)Math.Sin(theta), 0, (float)Math.Cos(theta));
            return result;
        }
        public static Matrix3 ZRotation(float theta) {
            theta = theta * (float)(Math.PI / 180);
            Matrix3 result = new Matrix3((float)Math.Cos(theta), -(float)Math.Sin(theta), 0,
                                         (float)Math.Sin(theta), (float)Math.Cos(theta), 0,
                                         0, 0, 1);
            return result;
        }
    }
}

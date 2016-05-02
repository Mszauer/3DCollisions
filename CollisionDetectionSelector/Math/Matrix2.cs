using System;

namespace Math_Implementation {
    class Matrix2 {
        public float[] Matrix;

        public float this[int i] {
            get {
                return Matrix[i];
            }
            set {
                Matrix[i] = value;
            }
        }

        public float this[int i, int j] {
            get {
                return Matrix[(i * 2) + j];
            }
            set {
                Matrix[(i * 2) + j] = value;
            }
        }

        public Matrix2() {
            Matrix = new float[] { 1, 0,
                                   0, 1 };
        }

        public Matrix2(params float[] values) {
            if (values.Length != 4) {
                Console.WriteLine("Invalid amount of numbers, Values.Length: " + values.Length);
                throw new System.Exception();
            }
            Matrix = new float[4];
            for (int i = 0; i < 4; i++) {
                Matrix[i] = values[i];
            }
        }

        // start here

        public float GetValue(int i, int j) {// i =row j = col
            return Matrix[(i * 2) + j];
        }
        public static float GetValue(Matrix2 matrix, int i, int j) {
            return matrix.Matrix[(i * 2) + j];
        }
        public static Matrix2 Add(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }
        public void Add(Matrix2 matrix) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    this[i, j] = matrix[i, j] + this[i, j];
                }
            }
        }
        public static Matrix2 operator +(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return result;
        }
        public static Matrix2 Subtract(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return result;
        }
        public void Subtract(Matrix2 matrix) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    this[i, j] = matrix[i, j] - this[i, j];
                }
            }
        }
        public static Matrix2 operator -(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return result;
        }
        public static Matrix2 Multiply(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            result[0, 0] = (matrix1[0, 0] * matrix2[0, 0]) + (matrix1[0, 1] * matrix2[1, 0]);
            result[0, 1] = (matrix1[0, 0] * matrix2[0, 1]) + (matrix1[0, 1] * matrix2[1, 1]);
            result[1, 0] = (matrix1[1, 0] * matrix2[0, 0]) + (matrix1[1, 1] * matrix2[0, 1]);
            result[1, 1] = (matrix1[1, 0] * matrix2[0, 1]) + (matrix1[1, 1] * matrix2[1, 1]);
            return result;
        }
        public void Multiply(Matrix2 matrix) {
            this[0, 0] = (this[0, 0] * matrix[0, 0]) + (this[0, 1] * matrix[1, 0]);
            this[0, 1] = (this[0, 0] * matrix[0, 1]) + (this[0, 1] * matrix[1, 1]);
            this[1, 0] = (this[1, 0] * matrix[0, 0]) + (this[1, 1] * matrix[0, 1]);
            this[1, 1] = (this[1, 0] * matrix[0, 1]) + (this[1, 1] * matrix[1, 1]);
        }
        public static Matrix2 operator *(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            result[0, 0] = (matrix1[0, 0] * matrix2[0, 0]) + (matrix1[0, 1] * matrix2[1, 0]);
            result[0, 1] = (matrix1[0, 0] * matrix2[0, 1]) + (matrix1[0, 1] * matrix2[1, 1]);
            result[1, 0] = (matrix1[1, 0] * matrix2[0, 0]) + (matrix1[1, 1] * matrix2[0, 1]);
            result[1, 1] = (matrix1[1, 0] * matrix2[0, 1]) + (matrix1[1, 1] * matrix2[1, 1]);
            return result;
        }
        public static Matrix2 ScalarMultiply(Matrix2 matrix, float scale) {
            Matrix2 result = matrix;
            for (int i = 0; i < 4; i++) {
                result[i] *= scale;
            }
            return result;
        }
        public void ScalarMultiply(float scale) {
            for (int i = 0; i < 4; i++) {
                this[i] *= scale;
            }
        }
        public static Matrix2 operator *(Matrix2 matrix, float scale) {
            Matrix2 result = matrix;
            for (int i = 0; i < 4; i++) {
                result[i] *= scale;
            }
            return result;
        }
        public static Matrix2 ScalarDivide(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            result[0, 0] = matrix1[0, 0] / matrix2[0, 0];
            result[0, 1] = matrix1[0, 1] / matrix2[0, 1];
            result[1, 0] = matrix1[1, 0] / matrix2[1, 0];
            result[1, 1] = matrix1[1, 1] / matrix2[1, 1];
            return result;
        }
        public void ScalarDivide(Matrix2 matrix) {
            Matrix2 result = new Matrix2();
            this[0, 0] /= matrix[0, 0];
            this[0, 1] /= matrix[0, 1];
            this[1, 0] /= matrix[1, 0];
            this[1, 1] /= matrix[1, 1];
        }
        public static Matrix2 operator /(Matrix2 matrix1, Matrix2 matrix2) {
            Matrix2 result = new Matrix2();
            result[0, 0] = matrix1[0, 0] / matrix2[0, 0];
            result[0, 1] = matrix1[0, 1] / matrix2[0, 1];
            result[1, 0] = matrix1[1, 0] / matrix2[1, 0];
            result[1, 1] = matrix1[1, 1] / matrix2[1, 1];
            return result;
        }
        public static Matrix2 Transpose(Matrix2 matrix) {
            return new Matrix2(matrix[0, 0], matrix[1, 0], matrix[0, 1], matrix[1, 1]);
        }
        public void Transpose() {
            float[] m = Transpose(this).Matrix;
            for (int i = 0; i < 4; i++) {
                Matrix[i] = m[i];
            }
        }
        public static float Determinant(Matrix2 matrix) {
            float result = (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
            return result;
        }
        public float Determinant() {
            float result = (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
            return result;
        }
        public static Matrix2 Adjugate(Matrix2 matrix) {
            Matrix2 m = matrix;
            m[0, 0] = matrix[1, 1];
            m[0, 1] = -matrix[0, 1];
            m[1, 0] = -matrix[1, 0];
            m[1, 1] = matrix[0, 0];
            return m;
        }
        public void Adjugate() {
            Matrix2 m = this;
            this[0, 0] = m[1, 1];
            this[0, 1] = -m[0, 1];
            this[1, 0] = -m[1, 0];
            this[1, 1] = m[0, 0];
        }
        public static Matrix2 Inverse(Matrix2 matrix) {
            if (Determinant(matrix) == 0) {
                return matrix;
            }
            return (Adjugate(matrix) * (1 / Determinant(matrix)));
        }
        public void Inverse() {
            if (Determinant() == 0) {
                return;
            }
            Matrix2 m = Adjugate(this) * (1 / Determinant());
            for (int i = 0; i < 4; i++) {
                this[i] = m[i];
            }
        }
        public static Vector2 Multiply(Matrix2 matrix, Vector2 vector) {
            Vector2 result = new Vector2();
            result[0] = (matrix[0, 0] * vector[0]) + (matrix[0, 1] * vector[1]);
            result[1] = (matrix[1, 0] * vector[0]) + (matrix[1, 1] * vector[1]);
            return result;                       
        }
        public Vector2 Multiply(Vector2 vector) {
            Vector2 result = new Vector2();
            result[0] = (this[0, 0] * vector[0]) + (this[0, 1] * vector[1]);
            result[1] = (this[1, 0] * vector[0]) + (this[1, 1] * vector[1]);
            return result;
        }
        public static Vector2 operator *(Matrix2 matrix, Vector2 vector) {
            Vector2 result = new Vector2();
            result[0] = (matrix[0, 0] * vector[0]) + (matrix[0, 1] * vector[1]);
            result[1] = (matrix[1, 0] * vector[0]) + (matrix[1, 1] * vector[1]);
            return result;
        }
        public static Matrix2 Rotation(float angle) {
            angle = angle * (float)(Math.PI / 180);
            Matrix2 result = new Matrix2((float)Math.Cos(angle),-(float)Math.Sin(angle),(float)Math.Sin(angle),(float)Math.Cos(angle));
            return result;
        }
    }
}

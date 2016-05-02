using System;

namespace Math_Implementation {
    class Matrix4 {
        public float[] Matrix = null;
        public float[] OpenGL {
            get {
                return Transpose(this).Matrix;
            }
        }
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
                return Matrix[(i * 4) + j];
            }
            set {
                Matrix[(i * 4) + j] = value;
            }
        }
        public Matrix4() {
            Matrix = new float[] {1,0,0,0,
                                   0,1,0,0,
                                   0,0,1,0,
                                   0,0,0,1};
        }
        public Matrix4(params float[] values) {
            Matrix = new float[16];
            if (values.Length != 16) {
                Console.WriteLine("Invalid amount of matrices added: " + values.Length);
                throw new System.Exception();
            }
            for (int i = 0; i < values.Length; i++) {
                Matrix[i] = values[i];
            }
        }
        //scalar operators
        public static Matrix4 operator +(Matrix4 matrixA, Matrix4 matrixB) {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    result[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix4 operator -(Matrix4 matrixA, Matrix4 matrixB) {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    result[i, j] = matrixA[i, j] - matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix4 operator /(Matrix4 matrixA, Matrix4 matrixB) {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    result[i, j] = matrixA[i, j] / matrixB[i, j];
                }
            }
            return result;
        }
        public static Matrix4 operator *(Matrix4 matrixA, float scale) {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    result[i, j] = matrixA[i, j] * scale;
                }
            }
            return result;
        }
        private static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.00001;
        }
        public static bool operator ==(Matrix4 matrixA, Matrix4 matrixB) {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (!Fequal(matrixA[i, j], matrixB[i, j])) {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator !=(Matrix4 matrixA, Matrix4 matrixB) {
            return !(matrixA == matrixB);
        }
        //vector*matrix
        public static Vector4 operator *(Matrix4 matrixA, Vector4 vectorA) {
            Vector4 result = new Vector4();
            result[0] = (matrixA[0, 0] * vectorA[0]) + (matrixA[0, 1] * vectorA[1]) + (matrixA[0, 2] * vectorA[2]) + (matrixA[0, 3] * vectorA[3]);
            result[1] = (matrixA[1, 0] * vectorA[0]) + (matrixA[1, 1] * vectorA[1]) + (matrixA[1, 2] * vectorA[2]) + (matrixA[1, 3] * vectorA[3]);
            result[2] = (matrixA[2, 0] * vectorA[0]) + (matrixA[2, 1] * vectorA[1]) + (matrixA[2, 2] * vectorA[2]) + (matrixA[2, 3] * vectorA[3]);
            result[3] = (matrixA[3, 0] * vectorA[0]) + (matrixA[3, 1] * vectorA[1]) + (matrixA[3, 2] * vectorA[2]) + (matrixA[3, 3] * vectorA[3]);
            return result;
        }
        //matrix matrix multiplication
        public static Matrix4 operator *(Matrix4 matrixA, Matrix4 matrixB) {
            Matrix4 result = new Matrix4();
            result[0, 0] = matrixA[0, 0] * matrixB[0, 0] + matrixA[0, 1] * matrixB[1, 0] + matrixA[0, 2] * matrixB[2, 0] + matrixA[0, 3] * matrixB[3, 0];
            result[0, 1] = matrixA[0, 0] * matrixB[0, 1] + matrixA[0, 1] * matrixB[1, 1] + matrixA[0, 2] * matrixB[2, 1] + matrixA[0, 3] * matrixB[3, 1];
            result[0, 2] = matrixA[0, 0] * matrixB[0, 2] + matrixA[0, 1] * matrixB[1, 2] + matrixA[0, 2] * matrixB[2, 2] + matrixA[0, 3] * matrixB[3, 2];
            result[0, 3] = matrixA[0, 0] * matrixB[0, 3] + matrixA[0, 1] * matrixB[1, 3] + matrixA[0, 2] * matrixB[2, 3] + matrixA[0, 3] * matrixB[3, 3];
            //
            result[1, 0] = matrixA[1, 0] * matrixB[0, 0] + matrixA[1, 1] * matrixB[1, 0] + matrixA[1, 2] * matrixB[2, 0] + matrixA[1, 3] * matrixB[3, 0];
            result[1, 1] = matrixA[1, 0] * matrixB[0, 1] + matrixA[1, 1] * matrixB[1, 1] + matrixA[1, 2] * matrixB[2, 1] + matrixA[1, 3] * matrixB[3, 1];
            result[1, 2] = matrixA[1, 0] * matrixB[0, 2] + matrixA[1, 1] * matrixB[1, 2] + matrixA[1, 2] * matrixB[2, 2] + matrixA[1, 3] * matrixB[3, 2];
            result[1, 3] = matrixA[1, 0] * matrixB[0, 3] + matrixA[1, 1] * matrixB[1, 3] + matrixA[1, 2] * matrixB[2, 3] + matrixA[1, 3] * matrixB[3, 3];
            //
            result[2, 0] = matrixA[2, 0] * matrixB[0, 0] + matrixA[2, 1] * matrixB[1, 0] + matrixA[2, 2] * matrixB[2, 0] + matrixA[2, 3] * matrixB[3, 0];
            result[2, 1] = matrixA[2, 0] * matrixB[0, 1] + matrixA[2, 1] * matrixB[1, 1] + matrixA[2, 2] * matrixB[2, 1] + matrixA[2, 3] * matrixB[3, 1];
            result[2, 2] = matrixA[2, 0] * matrixB[0, 2] + matrixA[2, 1] * matrixB[1, 2] + matrixA[2, 2] * matrixB[2, 2] + matrixA[2, 3] * matrixB[3, 2];
            result[2, 3] = matrixA[2, 0] * matrixB[0, 3] + matrixA[2, 1] * matrixB[1, 3] + matrixA[2, 2] * matrixB[2, 3] + matrixA[2, 3] * matrixB[3, 3];
            //
            result[3, 0] = matrixA[3, 0] * matrixB[0, 0] + matrixA[3, 1] * matrixB[1, 0] + matrixA[3, 2] * matrixB[2, 0] + matrixA[3, 3] * matrixB[3, 0];
            result[3, 1] = matrixA[3, 0] * matrixB[0, 1] + matrixA[3, 1] * matrixB[1, 1] + matrixA[3, 2] * matrixB[2, 1] + matrixA[3, 3] * matrixB[3, 1];
            result[3, 2] = matrixA[3, 0] * matrixB[0, 2] + matrixA[3, 1] * matrixB[1, 2] + matrixA[3, 2] * matrixB[2, 2] + matrixA[3, 3] * matrixB[3, 2];
            result[3, 3] = matrixA[3, 0] * matrixB[0, 3] + matrixA[3, 1] * matrixB[1, 3] + matrixA[3, 2] * matrixB[2, 3] + matrixA[3, 3] * matrixB[3, 3];
            return result;
        }
        //transpose
        public static Matrix4 Transpose(Matrix4 matrixA) {
            Matrix4 result = new Matrix4();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    result[i, j] = matrixA[j, i];
                }
            }
            return result;
        }
        public static Matrix4 Minor(Matrix4 matrixA) {
            Matrix4 result = new Matrix4();
            result[0, 0] = Matrix3.Determinant(new Matrix3(matrixA[1, 1], matrixA[1, 2], matrixA[1, 3], matrixA[2, 1], matrixA[2, 2], matrixA[2, 3], matrixA[3, 1], matrixA[3, 2], matrixA[3, 3]));
            result[0, 1] = Matrix3.Determinant(new Matrix3(matrixA[1, 0], matrixA[1, 2], matrixA[1, 3], matrixA[2, 0], matrixA[2, 2], matrixA[2, 3], matrixA[3, 0], matrixA[3, 2], matrixA[3, 3]));
            result[0, 2] = Matrix3.Determinant(new Matrix3(matrixA[1, 0], matrixA[1, 1], matrixA[1, 3], matrixA[2, 0], matrixA[2, 1], matrixA[2, 3], matrixA[3, 0], matrixA[3, 1], matrixA[3, 3]));
            result[0, 3] = Matrix3.Determinant(new Matrix3(matrixA[1, 0], matrixA[1, 1], matrixA[1, 2], matrixA[2, 0], matrixA[2, 1], matrixA[2, 2], matrixA[3, 0], matrixA[3, 1], matrixA[3, 2]));
            result[1, 0] = Matrix3.Determinant(new Matrix3(matrixA[0, 1], matrixA[0, 2], matrixA[0, 3], matrixA[2, 1], matrixA[2, 2], matrixA[2, 3], matrixA[3, 1], matrixA[3, 2], matrixA[3, 3]));
            result[1, 1] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 2], matrixA[0, 3], matrixA[2, 0], matrixA[2, 2], matrixA[2, 3], matrixA[3, 0], matrixA[3, 2], matrixA[3, 3]));
            result[1, 2] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 3], matrixA[2, 0], matrixA[2, 1], matrixA[2, 3], matrixA[3, 0], matrixA[3, 1], matrixA[3, 3]));
            result[1, 3] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 2], matrixA[2, 0], matrixA[2, 1], matrixA[2, 2], matrixA[3, 0], matrixA[3, 1], matrixA[3, 2]));
            result[2, 0] = Matrix3.Determinant(new Matrix3(matrixA[0, 1], matrixA[0, 2], matrixA[0, 3], matrixA[1, 1], matrixA[1, 2], matrixA[1, 3], matrixA[3, 1], matrixA[3, 2], matrixA[3, 3]));
            result[2, 1] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 2], matrixA[0, 3], matrixA[1, 0], matrixA[1, 2], matrixA[1, 3], matrixA[3, 0], matrixA[3, 2], matrixA[3, 3]));
            result[2, 2] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 3], matrixA[1, 0], matrixA[1, 1], matrixA[1, 3], matrixA[3, 0], matrixA[3, 1], matrixA[3, 3]));
            result[2, 3] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 2], matrixA[1, 0], matrixA[1, 1], matrixA[1, 2], matrixA[3, 0], matrixA[3, 1], matrixA[3, 2]));
            result[3, 0] = Matrix3.Determinant(new Matrix3(matrixA[0, 1], matrixA[0, 2], matrixA[0, 3], matrixA[1, 1], matrixA[1, 2], matrixA[1, 3], matrixA[2, 1], matrixA[2, 2], matrixA[2, 3]));
            result[3, 1] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 2], matrixA[0, 3], matrixA[1, 0], matrixA[1, 2], matrixA[1, 3], matrixA[2, 0], matrixA[2, 2], matrixA[2, 3]));
            result[3, 2] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 3], matrixA[1, 0], matrixA[1, 1], matrixA[1, 3], matrixA[2, 0], matrixA[2, 1], matrixA[2, 3]));
            result[3, 3] = Matrix3.Determinant(new Matrix3(matrixA[0, 0], matrixA[0, 1], matrixA[0, 2], matrixA[1, 0], matrixA[1, 1], matrixA[1, 2], matrixA[2, 0], matrixA[2, 1], matrixA[2, 2]));
            return result;
        }
        public static Matrix4 CoFactor(Matrix4 matrixA) {
            Matrix4 result = Minor(matrixA);
            result[0, 0] = result[0, 0] * 1;
            result[0, 1] = result[0, 1] * -1;
            result[0, 2] = result[0, 2] * 1;
            result[0, 3] = result[0, 3] * -1;

            result[1, 0] = result[1, 0] * -1;
            result[1, 1] = result[1, 1] * 1;
            result[1, 2] = result[1, 2] * -1;
            result[1, 3] = result[1, 3] * 1;

            result[2, 0] = result[2, 0] * 1;
            result[2, 1] = result[2, 1] * -1;
            result[2, 2] = result[2, 2] * 1;
            result[2, 3] = result[2, 3] * -1;

            result[3, 0] = result[3, 0] * -1;
            result[3, 1] = result[3, 1] * 1;
            result[3, 2] = result[3, 2] * -1;
            result[3, 3] = result[3, 3] * 1;
            return result;
        }
        public static Matrix4 Adjugate(Matrix4 matrixA) {
            return Transpose(CoFactor(matrixA));
        }
        public static float Determinant(Matrix4 matrix) {
            Matrix4 cofactor = CoFactor(matrix);
            return (matrix[0, 0] * cofactor[0, 0]) + (matrix[0, 1] * cofactor[0, 1]) + (matrix[0, 2] * cofactor[0, 2]) + (matrix[0, 3] * cofactor[0, 3]);
        }
        public static Matrix4 Inverse(Matrix4 matrixA) {
            Matrix4 result = Adjugate(matrixA);
            float determinant = Determinant(matrixA);
            result *= 1.0f / determinant;
            return result;
        }
        public static Matrix4 XRotation(float theta) {
            theta = theta * (float)(Math.PI / 180); //convert to rads
            Matrix4 result = new Matrix4(1, 0, 0, 0,
                                         0, (float)Math.Cos(theta), -(float)Math.Sin(theta), 0,
                                         0, (float)Math.Sin(theta), (float)Math.Cos(theta), 0,
                                         0,0,0,1);
            return result;
        }
        public static Matrix4 YRotation(float theta) {
            theta = theta * (float)(Math.PI / 180);
            Matrix4 result = new Matrix4((float)Math.Cos(theta), 0, (float)Math.Sin(theta), 0,
                                         0, 1, 0, 0,
                                         -(float)Math.Sin(theta), 0, (float)Math.Cos(theta), 0,
                                         0,0,0,1);
            return result;
        }
        public static Matrix4 ZRotation(float theta) {
            theta = theta * (float)(Math.PI / 180);
            Matrix4 result = new Matrix4((float)Math.Cos(theta), -(float)Math.Sin(theta), 0, 0,
                                         (float)Math.Sin(theta), (float)Math.Cos(theta), 0, 0,
                                         0, 0, 1, 0,
                                         0,0,0,1);
            return result;
        }
        public static Matrix4 AngleAxis(float angle, float u, float v, float w) {
            Matrix4 result = new Matrix4();
            float L = (u * u + v * v + w * w);
            float l = (float)Math.Sqrt(L);
            angle = angle * (float)(Math.PI / 180.0); //converting to radian value
            float u2 = u * u;
            float v2 = v * v;
            float w2 = w * w;

            result[0, 0] = (u2 + (v2 + w2) * (float)Math.Cos(angle)) / L;
            result[0, 1] = (u * v * (1 - (float)Math.Cos(angle)) - w * l * (float)Math.Sin(angle)) / L;
            result[0, 2] = (u * w * (1 - (float)Math.Cos(angle)) + v * l * (float)Math.Sin(angle)) / L;
            result[0, 3] = 0.0f;

            result[1, 0] = (u * v * (1 - (float)Math.Cos(angle)) + w * l * (float)Math.Sin(angle)) / L;
            result[1, 1] = (v2 + (u2 + w2) * (float)Math.Cos(angle)) / L;
            result[1, 2] = (v * w * (1 - (float)Math.Cos(angle)) - u * l * (float)Math.Sin(angle)) / L;
            result[1, 3] = 0.0f;

            result[2, 0] = (u * w * (1 - (float)Math.Cos(angle)) - v * l * (float)Math.Sin(angle)) / L;
            result[2, 1] = (v * w * (1 - (float)Math.Cos(angle)) + u * l * (float)Math.Sin(angle)) / L;
            result[2, 2] = (w2 + (u2 + v2) * (float)Math.Cos(angle)) / L;
            result[2, 3] = 0.0f;

            result[3, 0] = 0.0f;
            result[3, 1] = 0.0f;
            result[3, 2] = 0.0f;
            result[3, 3] = 1.0f;
            return result;
        }
        public static Matrix4 Translate(Vector3 vectorA) {
            Matrix4 result = new Matrix4();
            result[0, 3] = vectorA[0];
            result[1, 3] = vectorA[1];
            result[2, 3] = vectorA[2];
            result[3, 3] = 1.0f;
            return result;
        }
        public static Matrix4 Scale(Vector3 vectorA) {
            Matrix4 result = new Matrix4();
            result[0, 0] = vectorA[0];
            result[1, 1] = vectorA[1];
            result[2, 2] = vectorA[2];
            result[3, 3] = 1.0f;
            return result;
        }
        public static Vector3 MultiplyVector(Matrix4 matrixA, Vector3 vectorA) {
            Vector3 result = new Vector3();
            result[0] = (matrixA[0, 0] * vectorA[0]) + (matrixA[0, 1] * vectorA[1]) + (matrixA[0, 2] * vectorA[2]) + (matrixA[0, 3] * 0.0f);
            result[1] = (matrixA[1, 0] * vectorA[0]) + (matrixA[1, 1] * vectorA[1]) + (matrixA[1, 2] * vectorA[2]) + (matrixA[1, 3] * 0.0f);
            result[2] = (matrixA[2, 0] * vectorA[0]) + (matrixA[2, 1] * vectorA[1]) + (matrixA[2, 2] * vectorA[2]) + (matrixA[2, 3] * 0.0f);
            return result;
        }
        public static Vector3 MultiplyPoint(Matrix4 matrixA, Vector3 vectorA) {
            Vector3 result = new Vector3();
            result[0] = (matrixA[0, 0] * vectorA[0]) + (matrixA[0, 1] * vectorA[1]) + (matrixA[0, 2] * vectorA[2]) + (matrixA[0, 3] * 1.0f);
            result[1] = (matrixA[1, 0] * vectorA[0]) + (matrixA[1, 1] * vectorA[1]) + (matrixA[1, 2] * vectorA[2]) + (matrixA[1, 3] * 1.0f);
            result[2] = (matrixA[2, 0] * vectorA[0]) + (matrixA[2, 1] * vectorA[1]) + (matrixA[2, 2] * vectorA[2]) + (matrixA[2, 3] * 1.0f);
            return result;
        }
        public static Matrix4 Ortho(float left, float right, float bottom, float top, float near, float far) {
            Matrix4 result = new Matrix4();
            result[0, 0] = 2.0f / (right - left);
            result[0, 3] = -((right + left) / (right - left));
            result[1, 1] = 2.0f / (top - bottom);
            result[1, 3] = -((top + bottom) / (top - bottom));
            result[2, 2] = -2.0f / (far - near);
            result[2, 3] = -((far + near) / (far - near));
            result[3, 3] = 1.0f;
            return result;
        }
        public static Matrix4 Frustum(float left, float right, float bottom, float top, float near, float far) {
            Matrix4 result = new Matrix4();
            result[0, 0] = (2.0f * near) / (right - left);
            result[0, 2] = (right + left) / (right - left);
            result[1, 1] = (2.0f * near) / (top - bottom);
            result[1, 2] = (top + bottom) / (top - bottom);
            result[2, 2] = -((far + near) / (far - near));
            result[2, 3] = -((2.0f * far * near) / (far - near));
            result[3, 2] = -1.0f;
            result[3, 3] = 0.0f;
            return result;
        }
        public static Matrix4 LookAt(Vector3 position, Vector3 target, Vector3 worldUp) {
            Vector3 cameraForward = Vector3.Normalize(target - position);
            Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(cameraForward, worldUp));
            Vector3 cameraUp = Vector3.Cross(cameraRight, cameraForward);

            Matrix4 rot = new Matrix4(cameraRight.X, cameraUp.X, -cameraForward.X, 0.0f,
                                      cameraRight.Y, cameraUp.Y, -cameraForward.Y, 0.0f,
                                      cameraRight.Z, cameraUp.Z, -cameraForward.Z, 0.0f,
                                      0.0f, 0.0f, 0.0f, 1.0f);
            Matrix4 trans = Translate(position * -1.0f);
            return Transpose(rot) * trans;
        }
        public static Matrix4 Perspective(float fov, float aspectRatio, float zNear, float zFar) {
            float yMax = zNear * (float)Math.Tan(fov * (Math.PI / 360.0f));
            float xMax = yMax * aspectRatio;
            return Frustum(-xMax, xMax, -yMax, yMax, zNear, zFar);
        }
    }
}

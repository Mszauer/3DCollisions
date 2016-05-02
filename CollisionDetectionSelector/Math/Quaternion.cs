using System;

namespace Math_Implementation {
    class Quaternion {
        public float X = 0.0f;
        public float Y = 0.0f;
        public float Z = 0.0f;
        public float W = 1.0f;
        public float this[int i] {
            get {
                if (i == 0) {
                    return X;
                }
                else if (i == 1) {
                    return Y;
                }
                else if (i == 2) {
                    return Z;
                }
                else if (i == 3) {
                    return W;
                }
                return 0;
            }
            set {
                if (i == 0) {
                    X = value;
                }
                else if (i == 1) {
                    Y = value;
                }
                else if (i == 2) {
                    Z = value;
                }
                else if (i == 3) {
                    W = value;
                }
            }
        }
        public Quaternion(float x,float y,float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Quaternion() {
            X = Y = Z = 0.0f;
            W = 1.0f;
        }
        public static float Length(Quaternion q) {
            return (float)Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
        }
        public static float LengthSquared(Quaternion q) {
            return (q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
        }
        public static Quaternion Normalize(Quaternion q) {
            Quaternion result = new Quaternion();
            float qLength = Length(q);
            result.X /= qLength;
            result.Y /= qLength;
            result.Z /= qLength;
            result.W /= qLength;
            return result;
        }
        public void Normalize() {
            float qLength = Length(this);
            X /= qLength;
            Y /= qLength;
            Z /= qLength;
            W /= qLength;
        }
        public static Quaternion AngleAxis(float angle, Vector3 axis) {
            angle = angle * (float)(Math.PI / 180.0f);
            Quaternion result = new Quaternion();
            float sin = (float)Math.Sin(angle);
            axis = Vector3.Normalize(axis);
            result.W = (float)Math.Cos(angle);
            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            return result;
        }
        public static Quaternion AngleAxis(float angle,float x,float y, float z) {
            angle = angle * (float)(Math.PI / 180.0f);
            angle /= 2.0f;
            Vector3 axis = new Vector3(x, y, z);
            Quaternion result = new Quaternion();
            float sin = (float)Math.Sin(angle);
            axis = Vector3.Normalize(axis);
            result.W = (float)Math.Cos(angle);
            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            return result;
        }
        public static Matrix4 ToMatrix(Quaternion q) {
            Matrix4 result = new Matrix4();
            float xSq = q.X * q.X;
            float ySq = q.Y * q.Y;
            float zSq = q.Z * q.Z;
            float wSq = q.W * q.W;
            float twoX = 2.0f * q.X;
            float twoY = 2.0f * q.Y;
            float twoW = 2.0f * q.W;
            float xy = twoX * q.Y;
            float xz = twoX * q.Z;
            float yz = twoY * q.Z;
            float wx = twoW * q.X;
            float wy = twoW * q.Y;
            float wz = twoW * q.Z;
            result[0, 0] = wSq + xSq - ySq - zSq;
            result[0, 1] = xy - wz;
            result[0, 2] = xz + wy;
            result[0, 3] = 0.0f;
            result[1, 0] = xy + wz;
            result[1, 1] = wSq - xSq + ySq - zSq;
            result[1, 2] = yz - wx;
            result[1, 3] = 0.0f;
            result[2, 0] = xz - wy;
            result[2, 1] = yz + wx;
            result[2, 2] = wSq - xSq - ySq + zSq;
            result[2, 3] = 0.0f;
            result[3, 0] = 0.0f;
            result[3, 1] = 0.0f;
            result[3, 2] = 0.0f;
            result[3, 3] = 1.0f;
            return result;
        }
        public Matrix4 ToMatrix() {
            Matrix4 result = new Matrix4();
            float xSq = X * X;
            float ySq = Y * Y;
            float zSq = Z * Z;
            float wSq = W * W;
            float twoX = 2.0f *X;
            float twoY = 2.0f *Y;
            float twoW = 2.0f *W;
            float xy = twoX * Y;
            float xz = twoX * Z;
            float yz = twoY * Z;
            float wx = twoW * X;
            float wy = twoW * Y;
            float wz = twoW * Z;
            result[0, 0] = wSq + xSq - ySq - zSq;
            result[0, 1] = xy - wz;
            result[0, 2] = xz + wy;
            result[0, 3] = 0.0f;
            result[1, 0] = xy + wz;
            result[1, 1] = wSq - xSq + ySq - zSq;
            result[1, 2] = yz - wx;
            result[1, 3] = 0.0f;
            result[2, 0] = xz - wy;
            result[2, 1] = yz + wx;
            result[2, 2] = wSq - xSq - ySq + zSq;
            result[2, 3] = 0.0f;
            result[3, 0] = 0.0f;
            result[3, 1] = 0.0f;
            result[3, 2] = 0.0f;
            result[3, 3] = 1.0f;
            return result;
        }
        public static Quaternion Inverse(Quaternion q) {
            Quaternion result = new Quaternion();
            result.X = -q.X;
            result.Y = -q.Y;
            result.Z = -q.Z;
            result.W = q.W;
            return result;
        }
        public Quaternion Inverse() {
            float x = -X;
            float y = -Y;
            float z = -Z;
            return new Quaternion(x, y, z, W);
        }
        public static float Dot(Quaternion a, Quaternion b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }
        public float Dot(Quaternion a) {
            return X * a.X + Y * a.Y + Z * a.Z + W * a.W;
        }

        public static Quaternion operator *(Quaternion A, Quaternion B) {
            Quaternion result = new Quaternion();
            result.X = A.W * B.X + A.X * B.W + A.Y * B.Z - A.Z * B.Y;
            result.Y = A.W * B.Y - A.X * B.Z + A.Y * B.W + A.Z * B.X;
            result.Z = A.W * B.Z + A.X * B.Y - A.Y * B.X + A.Z * B.W;
            result.W = A.W * B.W - A.X * B.X - A.Y * B.Y - A.Z * B.Z;
            return result;
        }
        public static Vector3 operator *(Quaternion q, Vector3 v) {
            Vector3 t = Vector3.Cross(new Vector3(q.X, q.Y, q.Z), v) * 2.0f;
            Vector3 _v = v + (t * q.W) + Vector3.Cross(new Vector3(q.X, q.Y, q.Z), t);
            return _v;
        }
        public static Quaternion Multiply(Quaternion A, Quaternion B) {
            Quaternion result = new Quaternion();
            result.X = A.W * B.X + A.X * B.W + A.Y * B.Z - A.Z * B.Y;
            result.Y = A.W * B.Y - A.X * B.Z + A.Y * B.W + A.Z * B.X;
            result.Z = A.W * B.Z + A.X * B.Y - A.Y * B.X + A.Z * B.W;
            result.W = A.W * B.W - A.X * B.X - A.Y * B.Y - A.Z * B.Z;
            return result;
        }
        public static Quaternion FromEuler(float x,float y, float z) {
            float radConversion = (float)Math.PI / 180.0f;
            x *= radConversion;
            y *= radConversion;
            z *= radConversion;
            Quaternion result = new Quaternion();
            float cosX = (float)Math.Cos(x/2);
            float sinX = (float)Math.Sin(x/2);
            float cosY = (float)Math.Cos(y/2);
            float sinY = (float)Math.Sin(y/2);
            float cosZ = (float)Math.Cos(z/2);
            float sinZ = (float)Math.Sin(z/2);
            float cosYZ = cosY * cosZ;
            float sinYZ = sinY * sinZ;
            result.W = (cosYZ * sinX) + (sinYZ * sinX);
            result.X = (cosYZ * sinX) + (sinYZ * cosX);
            result.Y = (sinY * cosZ * cosX) + (cosY * sinZ * sinX);
            result.Z = (cosY * sinZ * cosX) - (sinY * cosZ * sinX);
            return result;
        }
        public static Vector3 ToEuler(Quaternion q) {
            float x, y, z = 0.0f;
            q = Normalize(q);
            float test = (q.X * q.Y) + (q.Z * q.W);
            if (test > 0.499) {//north singularity
                x = 0.0f * 57.2958f;
                y = 2.0f * (float)Math.Atan2(q.X,q.W) * 57.2958f;
                z = (float)Math.PI / 2.0f * 57.2958f;
                return new Vector3(x, y, z);
            }
            else if (test < -0.499) { //south singluarity
                x = 0.0f * 57.2958f;
                y = -2.0f * (float)Math.Atan2(q.X, q.W) * 57.2958f;
                z = -(float)Math.PI / 2.0f * 57.2958f;
                return new Vector3(x, y, z);
            }
            float xSq = q.X * q.X;
            float ySq = q.Y * q.Y;
            float zSq = q.Z * q.Z;
            x = (float)Math.Atan2((2.0f * q.X * q.W - 2.0f * q.Y * q.Z), (1.0f - (2.0f * xSq) - (2.0f * zSq))) * 57.2958f;
            y = (float)Math.Atan2((2.0f * q.Y * q.W - 2.0f * q.X * q.Z), (1.0f - (2.0f * ySq) - (2.0f * zSq))) * 57.2958f;
            z = (float)Math.Asin(2.0f * test) * 57.2958f;
            return new Vector3(x, y, z);
        }
        public Vector3 ToEuler() {
            float x, y, z = 0.0f;
            Normalize();
            float test = (X * Y) + (Z * W);
            if (test > 0.499f) {//north singularity
                x = 0.0f * 57.2958f;
                y = 2.0f * (float)Math.Atan2(X, W) * 57.2958f;
                z = (float)Math.PI / 2.0f * 57.2958f;
                return new Vector3(x, y, z);
            }
            else if (test < -0.499f) { //south singluarity
                x = 0.0f * 57.2958f;
                y = -2.0f * (float)Math.Atan2(X, W) * 57.2958f;
                z = -(float)Math.PI / 2.0f * 57.2958f;
                return new Vector3(x, y, z);
            }
            float xSq = X * X;
            float ySq = Y * Y;
            float zSq = Z * Z;
            x = (float)Math.Atan2((2.0f * X * W - 2.0f * Y * Z), (1.0f - (2.0f * xSq) - (2.0f * zSq))) * 57.2958f;
            y = (float)Math.Atan2((2.0f * Y * W - 2.0f * X * Z), (1.0f - (2.0f * ySq) - (2.0f * zSq))) * 57.2958f;
            z = (float)Math.Asin(2.0f * test) * 57.2958f;
            return new Vector3(x, y, z);

        }
        private static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.00001;
        }
        public static bool operator ==(Quaternion a, Quaternion b) {
            return (Fequal(a.X, b.X) && Fequal(a.Y, b.Y) && Fequal(a.Z, b.Z) && Fequal(a.W, b.W));
        }
        public static bool operator != (Quaternion a, Quaternion b) {
            return !(a==b);
        }
    }
}

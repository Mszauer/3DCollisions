using System;


namespace Math_Implementation {
    class Vector4 {
        public float X = 0.0f;
        public float Y = 0.0f;
        public float Z = 0.0f;
        public float W = 0.0f;
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
        public Vector4() {
            X = Y = Z = W = 0.0f;
        }
        public Vector4(float x,float y,float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public static Vector4 operator +(Vector4 vectorA,Vector4 vectorB) {
            return new Vector4(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z, vectorA.W + vectorB.W);
        }
        public static Vector4 operator -(Vector4 vectorA, Vector4 vectorB) {
            return new Vector4(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y, vectorA.Z - vectorB.Z, vectorA.W - vectorB.W);
        }
        public static Vector4 operator *(Vector4 vectorA, Vector4 vectorB) {
            return new Vector4(vectorA.X * vectorB.X, vectorA.Y * vectorB.Y, vectorA.Z * vectorB.Z, vectorA.W * vectorB.W);
        }
        public static Vector4 operator *(Vector4 vectorA, float scale) {
            return new Vector4(vectorA.X * scale, vectorA.Y * scale, vectorA.Z * scale, vectorA.W * scale);
        }
        public static Vector4 operator /(Vector4 vectorA, Vector4 vectorB) {
            return new Vector4(vectorA.X / vectorB.X, vectorA.Y / vectorB.Y, vectorA.Z / vectorB.Z, vectorA.W / vectorB.W);
        }
        public static float Dot(Vector4 vectorA,Vector4 vectorB) {
            float x = vectorA.X * vectorB.X;
            float y = vectorA.Y * vectorB.Y;
            float z = vectorA.Z * vectorB.Z;
            return x + y + z;
        }
        public float Dot(Vector4 vectorA) {
            float x = vectorA.X * X;
            float y = vectorA.Y * Y;
            float z = vectorA.Z * Z;
            return x + y + z;
        }
        public static float Length(Vector4 vectorA) {
            return (float)Math.Sqrt(Dot(vectorA, vectorA));
        }
        public float Length() {
            return (float)Math.Sqrt(Dot(this, this));
        }
        public static float LengthSquared(Vector4 vectorA) {
            return Dot(vectorA, vectorA);
        }
        public float LengthSquared() {
            return Dot(this, this);
        }
        public static Vector4 Normalize(Vector4 vectorA) {
            float length = Length(vectorA);
            return new Vector4(vectorA.X / length, vectorA.Y / length, vectorA.Z / length, vectorA.Z);
        }
        public void Normalize() {
            float length = Length(this);
            X /= length;
            Y /= length;
            Z /= length;
        }
        public static float AngleBetween(Vector4 vectorA,Vector4 vectorB) {
            return (float)Math.Acos(Dot(Normalize(vectorA), Normalize(vectorB)));
        }
        public float AngleBetween(Vector4 vectorA) {
            return (float)Math.Acos(Dot(Normalize(vectorA), Normalize(this)));
        }
        public static Vector4 Projection(Vector4 vectorA,Vector4 vectorB) {
            float projectionLength = Dot(vectorA, vectorB) / Length(vectorB);
            return Normalize(vectorB) * projectionLength;
        }
        public static Vector4 Perpendicular(Vector4 vectorA,Vector4 vectorB) {
            return vectorB-Projection(vectorA, vectorB);
        }
        public static Vector4 Cross(Vector4 vectorA,Vector4 vectorB) {
            Vector4 result = new Vector4();
            result.X = (vectorA.Y * vectorB.Z) - (vectorA.Z * vectorB.Y );
            result.Y = (vectorA.Z * vectorB.X) - (vectorA.X * vectorB.Z );
            result.Z = (vectorA.X * vectorB.Y) - (vectorA.Y * vectorB.X );
            return result;
        }
        public static Vector4 Negate(Vector4 vectorA) {
            return new Vector4(vectorA.X *= -1, vectorA.Y * -1, vectorA.Z * -1,vectorA.W);
        }
        public void Negate() {
            X *= -1;
            Y *= -1;
            Z *= -1;
        }
        public static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.0001;
        }
        public static bool operator ==(Vector4 vectorA, Vector4 vectorB) {
            return Fequal(vectorA.X,vectorB.X) && Fequal(vectorA.Y,vectorB.Y) && Fequal(vectorA.Z,vectorB.Z) ? true : false;
        }
        public static bool operator !=(Vector4 vectorA, Vector4 vectorB) {
            return !Fequal(vectorA.X,vectorB.X) || !Fequal(vectorA.Y,vectorB.Y) || !Fequal(vectorA.Z,vectorB.Z) ? true : false;
        }
        public static bool operator >=(Vector4 vectorA, Vector4 vectorB) {
            return vectorA.LengthSquared() >= vectorB.LengthSquared() ? true : false;
        }
        public static bool operator <=(Vector4 vectorA, Vector4 vectorB) {
            return vectorA.LengthSquared() <= vectorB.LengthSquared() ? true : false;
        }
        public static bool operator <(Vector4 vectorA, Vector4 vectorB) {
            return vectorA.LengthSquared() < vectorB.LengthSquared() ? true : false;
        }
        public static bool operator >(Vector4 vectorA, Vector4 vectorB) {
            return vectorA.LengthSquared() > vectorB.LengthSquared() ? true : false;
        }
    }
}

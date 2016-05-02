using System;


namespace Math_Implementation {
    class Vector3 {
        public float X = 0;
        public float Y = 0;
        public float Z = 0;
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
                return 0;
            }
            set {
                if (i == 0) {
                    X = value; ;
                }
                else if (i == 1) {
                    Y = value;
                }
                else if (i == 2) {
                    Z = value;
                }
            }
        }
        public Vector3() {
            X = Y = Z = 0.0f;
        }
        public Vector3(float x,float y,float z) {
            X = x;
            Y = y;
            Z = z;
        }
        public static Vector3 operator +(Vector3 vectorA, Vector3 vectorB) {
            return new Vector3(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z);
        }
        public static Vector3 operator -(Vector3 vectorA, Vector3 vectorB) {
            return new Vector3(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y, vectorA.Z - vectorB.Z);
        }
        public static Vector3 operator *(Vector3 vectorA, float scale) {
            return new Vector3(vectorA.X * scale, vectorA.Y * scale, vectorA.Z * scale);
        }
        public static Vector3 operator /(Vector3 vectorA,float scale) {
            return new Vector3(vectorA.X / scale, vectorA.Y / scale, vectorA.Z / scale);
        }
        public static Vector3 operator *(Vector3 vectorA,Vector3 vectorB) {
            return new Vector3(vectorA.X * vectorB.X, vectorA.Y * vectorB.Y, vectorA.Z * vectorB.Z);
        }
        public static Vector3 operator /(Vector3 vectorA,Vector3 vectorB) {
            return new Vector3(vectorA.X / vectorB.X, vectorA.Y / vectorB.Y, vectorA.Z / vectorB.Z);
        }
        public static float Dot(Vector3 vectorA,Vector3 vectorB) {
            float xComponent = vectorA.X * vectorB.X;
            float yComponent = vectorA.Y * vectorB.Y;
            float zComponent = vectorA.Z * vectorB.Z;
            return xComponent + yComponent + zComponent;
        }
        public float Dot(Vector3 vector) {
            float xComponent = this.X * vector.X;
            float yComponent = this.Y * vector.Y;
            float zComponent = this.Z * vector.Z;
            return xComponent + yComponent + zComponent;
        }
        public static float Length(Vector3 vectorA) {
            return (float)Math.Sqrt(Dot(vectorA, vectorA));
        }
        public float Length() {
            return (float)Math.Sqrt(Dot(this, this));
        }
        public static float LengthSquared(Vector3 vectorA) {
            return Dot(vectorA, vectorA);
        }
        public float LengthSquared() {
            return Dot(this, this);
        }
        public static Vector3 Normalize(Vector3 vectorA) {
            float length = Length(vectorA);
            float x = vectorA.X / length;
            float y = vectorA.Y / length;
            float z = vectorA.Z / length;
            return new Vector3(x, y, z);
        }
        public void Normalize() {
            float length = Length(this);
             X /= length;
             Y /= length;
             Z /= length;
        }
        public static float AngleBetween(Vector3 vectorA,Vector3 vectorB) {
            return (float)Math.Acos(Dot(Normalize(vectorA), Normalize(vectorB)));
        }
        public float AngleBetween(Vector3 vectorA) {
            return (float)Math.Acos(Dot(Normalize(this), Normalize(vectorA)));
        }
        public static Vector3 Projection(Vector3 vectorA,Vector3 vectorB) {
            float projectionLength = (Dot(vectorA, vectorB) / (Length(vectorB)));
            return Normalize(vectorB) * projectionLength;
        }
        public static Vector3 Perpendicular(Vector3 vectorA,Vector3 vectorB) {
            return (vectorB - Projection(vectorA, vectorB));
        }
        public static Vector3 Cross(Vector3 vectorA,Vector3 vectorB) {
            Vector3 result = new Vector3();
            result.X = (vectorA.Y * vectorB.Z) - (vectorA.Z * vectorB.Y);
            result.Y = (vectorA.Z * vectorB.X) - (vectorA.X * vectorB.Z);
            result.Z = (vectorA.X * vectorB.Y) - (vectorA.Y * vectorB.X);
            return result;
        }
        public void Cross(Vector3 vectorA) {
            X = (this.Y * vectorA.Z) - (this.Z * vectorA.Y);
            Y = (this.Z * vectorA.X) - (this.X * vectorA.Z);
            Z = (this.X * vectorA.Y) - (this.Y * vectorA.X);
        }
        public void Negate() {
            X *= -1.0f;
            Y *= -1.0f;
            Z *= -1.0f;
        }
        public static void Negate(Vector3 vectorA) {
            vectorA.X *= -1.0f;
            vectorA.Y *= -1.0f;
            vectorA.Z *= -1.0f;
        }
        public static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.0001;
        }
        public static bool operator ==(Vector3 vectorA, Vector3 vectorB) {
            return Fequal(vectorA.X, vectorB.X) && Fequal(vectorA.Y, vectorB.Y) && Fequal(vectorA.Z, vectorB.Z) ? true : false;
        }
        public static bool operator !=(Vector3 vectorA, Vector3 vectorB) {
            return !Fequal(vectorA.X, vectorB.X) || !Fequal(vectorA.Y,vectorB.Y) || !Fequal(vectorA.Z, vectorB.Z) ? true : false;
        }
        public static bool operator >=(Vector3 vectorA, Vector3 vectorB) {
            return vectorA.LengthSquared() >= vectorB.LengthSquared() ? true : false;
        }
        public static bool operator <=(Vector3 vectorA, Vector3 vectorB) {
            return vectorA.LengthSquared() <= vectorB.LengthSquared() ? true : false;
        }
        public static bool operator <(Vector3 vectorA, Vector3 vectorB) {
            return vectorA.LengthSquared() < vectorB.LengthSquared() ? true : false;
        }
        public static bool operator >(Vector3 vectorA, Vector3 vectorB) {
            return vectorA.LengthSquared() > vectorB.LengthSquared() ? true : false;
        }
    }
}

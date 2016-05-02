using System;


namespace Math_Implementation {
    public class Vector2 {
        public float X = 0.0f;
        public float Y = 0.0f;
        public float this[int i] {
            get {
                if (i == 0) {
                    return X;
                }
                else if (i == 1) {
                    return Y;
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
            }
        }
        public Vector2(float _x, float _y) {
            X = _x;
            Y = _y;
        }
        public Vector2() {
            X = 0;
            Y = 0;
        }
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2) {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public static Vector2 Add (Vector2 vector1, Vector2 vector2) {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public void Add(Vector2 vector) {
            X += vector.X;
            Y += vector.Y;
        }
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
            return result;
        }
        public static Vector2 Subtract(Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
            return result;
        }
        public void Subtract(Vector2 vector) {
            X -= vector.X;
            Y -= vector.Y;
        }
        public static Vector2 operator *(Vector2 vector, float scale) {
            Vector2 result = new Vector2(vector.X * scale, vector.Y * scale);
            return result;
        }
        public static Vector2 ScalarMultiply(Vector2 vector, float scale) {
            Vector2 result = new Vector2(vector.X * scale, vector.Y * scale);
            return result;
        }
        public void ScalarMultiply(float scale) {
            X *= scale;
            Y *= scale;
        }
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
            return result;
        }
        public static Vector2 ScalarMultiply (Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
            return result;
        }
        public void ScalarMultiply(Vector2 scale) {
            X *= scale.X;
            Y *= scale.Y;
        }
        public static Vector2 operator /(Vector2 vector, float scale) {
            Vector2 result = new Vector2(vector.X / scale, vector.Y / scale);
            return result;
        }
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X / vector2.X, vector1.Y / vector2.Y);
            return result;
        }
        public static Vector2 ScalarDivide (Vector2 vector, float scale) {
            Vector2 result = new Vector2(vector.X / scale, vector.Y / scale);
            return result;
        }
        public static Vector2 ScalarDivide(Vector2 vector1, Vector2 vector2) {
            Vector2 result = new Vector2(vector1.X / vector2.X, vector1.Y / vector2.Y);
            return result;
        }
        public void ScalarDivide(float scale) {
            X /= scale;
            Y /= scale;
        }
        public void ScalarDivide(Vector2 scale) {
            X /= scale.X;
            Y /= scale.Y;
        }
        public static float Dot(Vector2 vector1, Vector2 vector2) {
            float xComponent = vector1.X * vector2.X;
            float yComponent = vector1.Y * vector2.Y;
            return xComponent + yComponent;
        }
        public  float Dot(Vector2 vector) {
            float xComponent = X * vector.X;
            float yComponent = Y * vector.Y;
            return xComponent + yComponent;
        }
        public static float Length(Vector2 vector) {
            return (float)Math.Sqrt((double)Dot(vector, vector));
        }
        public float Length() {
            return (float)Math.Sqrt((double)Dot(this, this));
        }
        public float LengthSquared() {
            return Dot(this, this);
        }
        public static float LengthSquared(Vector2 vector) {
            return Dot(vector, vector);
        }
        public static Vector2 Normalize(Vector2 vector) {
            float length = Length(vector);
            vector.X /= length;
            vector.Y /= length;
            return new Vector2(vector.X, vector.Y);
        }
        public void Normalize() {
            X /= Length();
            Y /= Length();
        }
        public float AngleBetween(Vector2 vector) {
            return (float)Math.Acos((double)Dot(Normalize(this), Normalize(vector)));
        }
        public static float AngleBetween(Vector2 vector1, Vector2 vector2) {
            return (float)Math.Acos((double)Dot(Normalize(vector1), Normalize(vector2)));
        }
        public static Vector2 operator -(Vector2 vector) {
            return new Vector2(vector.X * -1.0f, vector.Y * -1.0f);
        }
        public void Negate() { 
            X *= -1.0f;
            Y *= -1.0f;
        }
        public static void Negate(Vector2 vector) {
            vector.X *= -1.0f;
            vector.Y *= -1.0f;
        }
        private static bool Fequal(float a, float b) {
            return Math.Abs(a - b) < 0.00001;
        }
        public static bool operator ==(Vector2 vector1,Vector2 vector2) {
            return Fequal(vector1.X,vector2.X) && Fequal(vector1.Y,vector2.Y) ? true : false; 
        }
        public static bool operator !=(Vector2 vector1, Vector2 vector2) {
            return !Fequal(vector1.X, vector2.X) || !Fequal(vector1.Y, vector2.Y) ? true : false;
        }
        public static bool operator >=(Vector2 vector1, Vector2 vector2) {
            return vector1.LengthSquared() >= vector2.LengthSquared() ? true : false;
        }
        public static bool operator <=(Vector2 vector1, Vector2 vector2) {
            return vector1.LengthSquared() <= vector2.LengthSquared() ? true : false;
        }
        public static bool operator <(Vector2 vector1, Vector2 vector2) {
            return vector1.LengthSquared() < vector2.LengthSquared() ? true : false;
        }
        public static bool operator >(Vector2 vector1, Vector2 vector2) {
            return vector1.LengthSquared() > vector2.LengthSquared() ? true : false;
        }
    }
}

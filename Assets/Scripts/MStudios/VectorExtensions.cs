using UnityEngine;

namespace MStudios
{
    public static class VectorExtensions
    {
        public static Vector2 AsFloatVector2(this Vector2Int self)
        {
            return new Vector2(self.x,self.y);
        }
        
        public static Vector2Int With(this Vector2Int self, int? x = null, int? y = null)
        {
            int newX = x ?? self.x;
            int newY = y ?? self.y;

            return new Vector2Int(newX,newY);
        }
        
        public static Vector3 AsVector3(this Vector2 self)
        {
            return new Vector3(self.x, self.y, 0);
        }
        
        public static Vector3 AsVector3(this Vector2Int self)
        {
            return new Vector3(self.x, self.y, 0);
        }

        public static Vector3 With(this Vector3 self, float? x = null, float? y = null, float? z = null)
        {
            float newX = x ?? self.x;
            float newY = y ?? self.y;
            float newZ = z ?? self.z;
            
            return new Vector3(newX,newY,newZ);
        }

        public static Vector3 Multiply(this Vector3 self, Vector3 other)
        {
            return new Vector3(self.x * other.x, self.y * other.y, self.z * other.z);
        }
    }
}
using UnityEngine;

namespace MStudios
{
    public static class VectorExtensions
    {
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
    }
}
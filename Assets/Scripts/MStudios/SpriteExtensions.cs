using UnityEngine;

namespace MStudios
{
    public static class SpriteExtensions
    {
        public static Vector2 CenterOffset(this Sprite self)
        {
            return new Vector2(
                self.pivot.x / self.rect.width - 0.5f,
                self.pivot.y / self.rect.height - 0.5f
            );
        }
    }
}
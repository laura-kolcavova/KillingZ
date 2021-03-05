using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using KillingZ.LEntity;

namespace KillingZ.GFX
{
    class GameCamera
    {
        private Vector2 position;
        private Handler handler;

        public GameCamera(Handler handler, Vector2 position)
        {
            this.handler = handler;
            this.position = position;
        }

        public void CenterOnEntity(Entity e)
        {
            float nx = MathHelper.Clamp(e.position.X + e.width / 2 - handler.GetWidth() / 2, 0, handler.GetWorld().widthPx - handler.GetWidth());
            float ny = MathHelper.Clamp(e.position.Y + e.height / 2 - handler.GetHeight() / 2, 0, handler.GetWorld().heightPx - handler.GetHeight());

            position = new Vector2(nx, ny);
        }

        public float GetX() { return position.X; }
        public float GetY() { return position.Y; }
    }
}

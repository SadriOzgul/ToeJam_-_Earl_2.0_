
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

 namespace ToeJam___Earl_2._0_
{
    public class Camera2D
    {
        private readonly Viewport _viewport;
        private Vector2 _position;
        private float _zoom;
        private float _rotation;

        public Matrix Transform { get; private set; }

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
            _zoom = 1f;
            _rotation = 0f;
            _position = Vector2.Zero;
            UpdateTransform();
        }

        public void Follow(Vector2 target)
        {
            // Center the camera on the target
            _position = target - new Vector2(_viewport.Width / 2, _viewport.Height / 2);
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            Transform =
                Matrix.CreateTranslation(new Vector3(-_position, 0)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1f);
        }
    }
}

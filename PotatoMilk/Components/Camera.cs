using PotatoMilk.Helpers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace PotatoMilk.Components
{
    [ComponentName("camera")]
    public class Camera : IComponent, IStateful
    {
        public string TypeName { get; private set; }
        public GameObject GameObject { get; private set; }
        internal View SFMLView { get; private set; } = new();

        public Vector2f WorldViewportSize
        {
            get => SFMLView.Size;
            set => SFMLView.Size = value;
        }

        public Vector2f ScreenViewportPos
        {
            get => new Vector2f(SFMLView.Viewport.Left, SFMLView.Viewport.Top);
            set => SFMLView.Viewport = new FloatRect(value, new Vector2f(SFMLView.Viewport.Width, SFMLView.Viewport.Height));
        }

        public Vector2f ScreenViewportSize
        {
            get => new Vector2f(SFMLView.Viewport.Width, SFMLView.Viewport.Height);
            set => SFMLView.Viewport = new FloatRect(new Vector2f(SFMLView.Viewport.Left, SFMLView.Viewport.Top), value);
        }

        private int renderPriority;
        public int RenderPriority
        {
            get => renderPriority;
            set
            {
                renderPriority = value;
                StateUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool DoClear { get; set; }
        public Color ClearColor { get; set; }

        private Transform transform;

        public event EventHandler StateUpdated;

        public void Initialize(GameObject container, Dictionary<string, object> data, string typeName)
        {
            if (GameObject != null)
                throw new Exception("Already initialized");
            TypeName = typeName;
            transform = ComponentHelper.TryGetComponent<Transform>(container, nameof(Camera));
            GameObject = container;

            WorldViewportSize = ComponentHelper.TryGetDataValue<Vector2f>(data, "world_viewport_size", new());
            ScreenViewportPos = ComponentHelper.TryGetDataValue<Vector2f>(data, "screen_viewport_pos", new());
            ScreenViewportSize = ComponentHelper.TryGetDataValue<Vector2f>(data, "screen_viewport_size", new(1f, 1f));
            RenderPriority = ComponentHelper.TryGetDataValue<int>(data, "render_priority", 0);
            DoClear = ComponentHelper.TryGetDataValue<bool>(data, "do_clear", true);
            ClearColor = ComponentHelper.TryGetDataValue<Color>(data, "clear_color", new Color(128, 128, 128));

            transform.StateUpdated += UpdateViewPos;
            UpdateViewPos(null, null);
        }

        private void UpdateViewPos(object sender, EventArgs args)
        {
            SFMLView.Center = transform.Position;
        }
    }
}

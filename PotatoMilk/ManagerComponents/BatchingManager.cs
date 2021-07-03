using PotatoMilk.Components;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace PotatoMilk.ManagerComponents
{
    class BatchingManager
    {
        Dictionary<Texture, QuadBatch> quadBatches = new();
        Dictionary<QuadRenderer, QuadBatch> containingBatches = new();
        public void TrackComponent(IComponent component)
        {
            if (component is QuadRenderer rend)
            {
                rend.StateUpdated += OnQuadRendererUpdate;
                if (rend.Texture is not null)
                    AddQuadRenderer(rend);
            }
        }

        private void AddQuadRenderer(QuadRenderer rend)
        {
            if (!quadBatches.ContainsKey(rend.Texture))
                quadBatches.Add(rend.Texture, new QuadBatch(rend.Texture));
            quadBatches[rend.Texture].Add(rend);
            containingBatches.Add(rend, quadBatches[rend.Texture]);
        }

        private void OnQuadRendererUpdate(object sender, EventArgs args)
        {
            QuadRenderer rend = (QuadRenderer)sender;
            if (rend.Texture != null && quadBatches.ContainsKey(rend.Texture) && quadBatches[rend.Texture].Contains(rend))
            {
                quadBatches[rend.Texture].UpdatePosition(rend);
            }
            else
            {
                if (containingBatches.ContainsKey(rend))
                {
                    QuadBatch curBatch = containingBatches[rend];
                    curBatch.Remove(rend);
                    containingBatches.Remove(rend);
                }
                if (rend.Texture is not null)
                    AddQuadRenderer(rend);
            }
        }

        public void UntrackComponent(IComponent component)
        {
            if (component is QuadRenderer rend)
            {
                if (rend.Texture is not null)
                {
                    QuadBatch curBatch = containingBatches[rend];
                    curBatch.Remove(rend);
                    containingBatches.Remove(rend);
                }
                rend.StateUpdated -= OnQuadRendererUpdate;
            }
        }

        public void Draw(RenderWindow window)
        {
            foreach (var batch in quadBatches)
            {
                window.Draw(batch.Value.VertexArray, batch.Value.RenderStates);
            }
        }
    }
}

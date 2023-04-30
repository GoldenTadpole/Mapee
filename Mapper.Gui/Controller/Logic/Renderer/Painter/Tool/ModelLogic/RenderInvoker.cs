namespace Mapper.Gui.Logic
{
    public class RenderInvoker : IRenderInvoker
    {
        public Renderer Renderer { get; set; }

        public RenderInvoker(Renderer renderer) 
        {
            Renderer = renderer;
        }

        public void Render()
        {
            Renderer.Render();
        }
    }
}

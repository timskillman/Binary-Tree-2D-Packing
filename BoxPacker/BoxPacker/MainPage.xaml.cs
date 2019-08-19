using System;
using System.ComponentModel;
using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace BoxPacker
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Packer packer;
        private const uint BoxNumber = 150;
        private const int BoxWidth = 200;
        private const int BoxHeight = 200;
        private float containerWidth = 300f;
        private float containerHeight = 300f;

        public MainPage()
        {
            InitializeComponent();

            CreateBoxes(BoxWidth, BoxHeight, BoxNumber);
            packer.Pack(containerWidth, containerHeight);
        }

        private void CreateBoxes(int maxWidth, int maxHeight, uint number)
        {
            packer = new Packer();

            Random rand = new Random();

            for (uint i=0; i < number; i++)
            {
                Packer.Box box = new Packer.Box() {
                    width = rand.Next(maxWidth),
                    height = rand.Next(maxHeight)
                };
                packer.AddBox(box);
                //Console.WriteLine(box.width.ToString() + "x" + box.height.ToString());
            }
        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.LightGray);
            Random rand = new Random();

            foreach (Packer.Box box in packer.boxes)
            {
                if (box.position!=null)
                {
                    SKRect boxrect = new SKRect(box.position.x, box.position.y, box.position.x + box.width, box.position.y + box.height);
                    uint red = 100 + (uint)rand.Next(155);
                    uint grn = 100 + (uint)rand.Next(155);
                    uint blu = 100 + (uint)rand.Next(155);

                    canvas.DrawRect(boxrect, new SKPaint() {
                        Color = new SKColor((byte)red, (byte)grn, (byte)blu)
                    });
                }
            }
        }

        private void Redo_Pressed(object sender, EventArgs e)
        {
            CreateBoxes(BoxWidth, BoxHeight, BoxNumber);
            packer.Pack(containerWidth, containerHeight);
            SKcanvas.InvalidateSurface();
        }
    }

}

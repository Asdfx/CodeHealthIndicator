using System.Windows;
using System.Windows.Media;

namespace CodeHealthIndicator
{
    internal static class Statics
    {
        public static ImageSource GoodHealthGlyph = GetGlyph(Colors.Green);
        public static ImageSource ModerateHealthGlyph = GetGlyph(Colors.Orange);
        public static ImageSource BadHealthGlyph = GetGlyph(Colors.Red);

        private static ImageSource GetGlyph(Color color)
        {
            var rectangle = new RectangleGeometry(new Rect(0, 0, 24, 24));

            var brush = new SolidColorBrush(color);

            var drawing = new GeometryDrawing
            {
                Geometry = rectangle,
                Brush = brush
            };

            return new DrawingImage(drawing);
        }
    }
}

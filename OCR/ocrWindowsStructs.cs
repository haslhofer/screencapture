using System.Collections.Generic;

namespace screencapture
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class BoundingRect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public bool IsEmpty { get; set; }
    }

    public class Word
    {
        public BoundingRect BoundingRect { get; set; }
        public string Text { get; set; }
    }

    public class Line
    {
        public string Text { get; set; }
        public List<Word> Words { get; set; }
    }

    public class Root2
    {
        public List<Line> Lines { get; set; }
        public string Text { get; set; }
        public int TextAngle { get; set; }
    }


}
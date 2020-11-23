namespace screencapture
{
    public class ConfidenceScore
    {
        public string Hashtag {get;set;}
        public float Confidence {get;set;}

        public string GetDebug()
        {
            //return "Hashtag:" + Hashtag + " Confidence:" + Confidence.ToString();
            return "#" + Hashtag;
        }
    }
}

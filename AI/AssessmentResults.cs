using System.Collections.Generic;

namespace screencapture
{
    public class AssessmentResult
    {
        public List<ConfidenceScore> ConfidenceScoreResults {get;set;}
        public List<NerResponse> RecognizedEntities {get;set;}
        public string CapturedText {get;set;}
        public string PathToImage {get;set;}

    }
}

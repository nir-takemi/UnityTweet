using UnityEngine;

public class UnityTweetSample : MonoBehaviour
{
    public void CallTweet()
    {
        ylib.Services.UnityTweet.Tweet("Tweetテストです", "tag1", "tag2", "tag3");
    }
    public void CallTweetWithGameURL()
    {
        ylib.Services.UnityTweet.TweetWithGameURL("TweetWithGameURLテストです", "tagA", "tagB", "tagC");
    }
    public void CallTweetWithCaptureImage()
    {
        ylib.Services.UnityTweet.TweetWithCaptureImage("TweetWithCaptureImageテストです", "tagI", "tagII", "tagIII");
    }
    public void CallTweetWithCaptureImageTextBaseURL()
    {
        ylib.Services.UnityTweet.TweetWithCaptureImageTextBaseURL("TweetWithCaptureImageテストです", "tagI", "tagII", "tagIII");
    }
}

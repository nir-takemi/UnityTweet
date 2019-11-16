using UnityEngine;

namespace ylib.Services
{
    using ylib.Services.Internal;

    public class UnityTweet : MonoBehaviour
    {
        private static UnityTweetBase tweetObj;

        public void Awake()
        {
            // UnityTweetBaseの派生スクリプトが同GameObjectにあるのを前提にPrefabは作っておく
            tweetObj = GetComponent<UnityTweetBase>();
        }

        public static void Tweet(string text, params string[] hashTags)
        {
            tweetObj.Tweet(text, hashTags);
        }
        public static void TweetWithGameURL(string text, params string[] hashTags)
        {
            tweetObj.TweetWithGameURL(text, hashTags);
        }
        public static void TweetWithCaptureImage(string text, params string[] hashTags)
        {
            tweetObj.IsTextBaseURL = false;
            tweetObj.TweetWithCaptureImage(text, hashTags);
        }
        public static void TweetWithCaptureImageTextBaseURL(string text, params string[] hashTags)
        {
            tweetObj.IsTextBaseURL = true;
            tweetObj.TweetWithCaptureImage(text, hashTags);
        }
    }
}

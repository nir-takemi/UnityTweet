using UnityEngine;

namespace ylib.Services.Internal
{
    [CreateAssetMenu(menuName = "ylib/TweetSettingAsset")]
    public class TweetSetting : ScriptableObject
    {
        public string GameURL;

        public string ImgurClientId;
    }
}

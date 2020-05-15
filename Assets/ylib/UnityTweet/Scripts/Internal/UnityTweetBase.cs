using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;


namespace ylib.Services.Internal
{
    public abstract class UnityTweetBase : MonoBehaviour
    {
        private const float cCaptureWaitTime = 1f;

        private const string cTweetURL = "http://twitter.com/intent/tweet?";

        protected const string cImageSuffix = "png";

#if !UNITY_EDITOR && UNITY_WEBGL
        // Plugins
        [DllImport("__Internal")] private static extern void OpenNewWindow(string URL);
#endif

        protected TweetSetting setting;

        // Twitterクライアントによっては、単純にURLの順序でcard表示するかどうかが決まるため、textベースでURLを並べられるようにだけしておく
        public bool IsTextBaseURL { get; set; }

        /// <summary>
        /// イメージアップロード処理(サブクラスでアップローダに合わせて実装が必要)
        /// </summary>
        /// <param name="imageData">イメージデータ</param>
        /// <param name="afterUpload">アップしたイメージのURLを引数に指定してアップロード後に必ず実行するアクション</param>
        protected abstract IEnumerator Upload(byte[] imageData, Action<string> afterUpload);


        public void Awake()
        {
            IsTextBaseURL = false;

            setting = Resources.Load<TweetSetting>("TweetSetting");
        }


        /// <summary>
        /// 通常tweet処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        public void Tweet(string text, params string[] hashTags)
        {
            _Tweet(text, null, hashTags);
        }

        /// <summary>
        /// GameのURL(TweetSetting.assetで定義)込みでのtweet処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        public void TweetWithGameURL(string text, params string[] hashTags)
        {
            string gameURL = setting.GameURL;
            _Tweet(text, gameURL == "" ? null : gameURL, hashTags);
        }

        /// <summary>
        /// 現状のゲーム画面のスクショとGameのURL(TweetSetting.assetで定義)込みでのtweet処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        public void TweetWithCaptureImage(string text, params string[] hashTags)
        {
            StartCoroutine(_TweetWithCaptureImage(text, null, hashTags));
        }

        /// <summary>
        /// 現状のゲーム画面のスクショとGameのURL(TweetSetting.assetで定義)込みでのtweet処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="actOnAfterCapture">キャプチャー後のアクション</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        public void TweetWithCaptureImageAndAfterCaptureAction(string text, System.Action actOnAfterCapture, params string[] hashTags)
        {
            StartCoroutine(_TweetWithCaptureImage(text, actOnAfterCapture, hashTags));
        }

        /// <summary>
        /// キャプチャ処理
        /// </summary>
        /// <returns>キャプチャイメージの保存パス</returns>
        private string CaptureScreen()
        {
            string fileName = string.Format("{0:yyyyMMddHmmssFFFF}.{1}", DateTime.Now, cImageSuffix);
            string filePath = Application.persistentDataPath + "/" + fileName;

            // モバイルプラットフォームはファイル名指定で、勝手にpersistentDataPath配下となる
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            ScreenCapture.CaptureScreenshot(fileName);
#else
            ScreenCapture.CaptureScreenshot(filePath);
#endif
            // pathは同一になるはずなので、一律filePathを返してあげる
            return filePath;
        }

        /// <summary>
        /// Tweet処理のメイン処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="url">card表示したいURL</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        private void _Tweet(string text, string url, params string[] hashTags)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(cTweetURL);
            builder.Append("&text=" + UnityWebRequest.EscapeURL(text));
            builder.Append("&url=" + UnityWebRequest.EscapeURL(url));
            if (0 < hashTags.Length)
            {
                string hashTagText = string.Join(",", hashTags);
                builder.Append("&hashtags=" + UnityWebRequest.EscapeURL(hashTagText));
            }

            string tweetURL = builder.ToString();
#if !UNITY_EDITOR && UNITY_WEBGL
            OpenNewWindow(tweetURL);
#else
            Application.OpenURL(tweetURL);
#endif
        }

        /// <summary>
        /// キャプチャ込みのTweet処理のメイン処理
        /// </summary>
        /// <param name="text">呟く内容</param>
        /// <param name="hashTags">ハッシュタグ(#なしで指定)</param>
        private IEnumerator _TweetWithCaptureImage(string text, System.Action actOnAfterCapture, params string[] hashTags)
        {
            // capture
            string filePath = CaptureScreen();

            // キャプチャー処理の待ち時間
            float startTime = Time.time;
            while (File.Exists(filePath) == false)
            {
                if (Time.time - startTime > cCaptureWaitTime)
                {
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }

            if (actOnAfterCapture != null)
            {
                actOnAfterCapture();
            }

            byte[] imageData = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            // upload
            string uploadedImgURL = "";

            yield return Upload(imageData, (uploadedURL) =>
            {
                uploadedImgURL = uploadedURL;
            });

            // GameURLが設定されていればtextの後ろに付与してあげる
            string textJoinGameUrl = text;
            string gameURL = setting.GameURL;

            if (IsTextBaseURL)
            {
                if (gameURL != "")
                {
                    textJoinGameUrl = string.Format("{0}\n{1}\n{2}", text, uploadedImgURL, gameURL);
                }
                else
                {
                    textJoinGameUrl = string.Format("{0}\n{1}", text, uploadedImgURL);
                }

                // tweet
                _Tweet(textJoinGameUrl, "", hashTags);
            }
            else
            {
                if (gameURL != "")
                {
                    textJoinGameUrl = string.Format("{0}\n{1}", text, gameURL);
                }

                // tweet
                _Tweet(textJoinGameUrl, uploadedImgURL, hashTags);
            }
        }
    }
}

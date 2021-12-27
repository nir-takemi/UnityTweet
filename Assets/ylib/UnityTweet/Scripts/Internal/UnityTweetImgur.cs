using System;
using System.Collections;
using System.Xml.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace ylib.Services.Internal
{
    public class UnityTweetImgur : UnityTweetBase
    {

        /// <summary>
        /// イメージアップロード処理(Imgur)
        /// </summary>
        /// <param name="imageData">イメージデータ</param>
        /// <param name="afterUpload">アップしたイメージのURLを引数に指定してアップロード後に必ず実行するアクション</param>
        protected override IEnumerator Upload(byte[] imageData, Action<string> afterUpload)
        {
            string uploadedURL = "";

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("image", Convert.ToBase64String(imageData));
            wwwForm.AddField("type", "base64");

            UnityWebRequest www;
            www = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwForm);
            www.SetRequestHeader("AUTHORIZATION", "Client-ID " + setting.ImgurClientId);
            yield return www.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
                {
                Debug.LogError(www.error);
                yield break;
            }
            else
            {
                XDocument xDoc = XDocument.Parse(www.downloadHandler.text);
                uploadedURL = xDoc.Element("data").Element("link").Value;

                // 画像リンクだとcard表示してくれないので、拡張子削った画像ページをURLとして扱ってあげる
                foreach (string suffix in cImageSuffixList)
                {
                    uploadedURL = uploadedURL.Replace("." + suffix, "");
                }
            }

            afterUpload(uploadedURL);
        }
    }
}

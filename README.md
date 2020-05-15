# UnityTweet
Unity上からTwitter上にtweetするためのパッケージ。

# 対象ビルド
- iOS
- Android
- WebGL

# 対象画像アップローダーサービス
- Imgur

# 準備
## unitypackageのDL
以下から最新のものをダウンロードしてください
https://github.com/nir-takemi/UnityTweet/releases

## Imgurの場合
1. アカウント登録：https://imgur.com/register?redirect=https%3A%2F%2Fimgur.com%2F
   
2. アプリ登録：https://api.imgur.com/oauth2/addclient
　![image](https://user-images.githubusercontent.com/10418442/68995593-b91c1080-08d2-11ea-9308-58ea7e5ff89a.png)
   1. Application name:アプリ名
   2. Authorization type:OAuth 2 authorization with a callback URL
   3. Authorization callback URL:なんでも（使用しない）
   4. Application website:なんでも（使用しない）
   5. Email:なんでも（使用しない）
   6. Description:なんでも（使用しない）
3. 登録後に表示されるClientIDを控える
4. 追って確認したい場合：https://imgur.com/account/settings/apps

# 実装
1. DLした.unitypackageをmenuからimport
![image](https://user-images.githubusercontent.com/10418442/68995076-22992080-08cd-11ea-8c88-e435b6d40dd4.png)

2. Sampleは任意で、その他にチェックがされていることを確認の上import
![image](https://user-images.githubusercontent.com/10418442/68995098-4fe5ce80-08cd-11ea-8e74-de0defbd4c3a.png)

3. Tweet設定
   1. ylib > UnityTweet > Resources > TweetSetting.assetを開く
   2. GameURL:アプリの公開ページなどのURLを入力
   1. ImgurClientId:準備で控えたClientIDを入力
![image](https://user-images.githubusercontent.com/10418442/68995199-4315aa80-08ce-11ea-95a1-556a0e1a314d.png)

4. prefab(ylib > UnityTweet > Resources > Prefabs > GO_TweetImgur)をtweetしたいscene上に置く
![image](https://user-images.githubusercontent.com/10418442/69003456-c9210800-0945-11ea-8233-643a4c4b2b52.png)

5. コード上で以下のように処理を書く
```Tweet
ylib.Services.UnityTweet.Tweet("単につぶやく", "tag1", "tag2");

ylib.Services.UnityTweet.TweetWithGameURL("GameURLと一緒につぶやく", "tag1", "tag2");

ylib.Services.UnityTweet.TweetWithCaptureImage("キャプチャと一緒につぶやく", "tag1", "tag2");
```

6. 表示例
![image](https://user-images.githubusercontent.com/10418442/68995359-43af4080-08d0-11ea-820a-1eebe53653c1.png)

# その他
## WebGL
### Tweet時に「404 Not Found.」が表示される
- 原因分かってないですが、OpenNewWindow.jslibのguidを変更することで解消できることを確認しています。
お手数ですが、以下の手順をお試し願います。
   - Assets/ylib/UnityTweet/Plugins/WebGL/OpenNewWindow.jslibを複製
   - 複製元のファイルを削除
   - 複製したファイルを `OpenNewWindow.jslib` にリネーム

# SimpleMicRemote の概要
マイク入力でキーボードのシュミレート及びアプリ起動を行うツールです。  
Chromeの音声認識を使用し、設定したキーワードを話すと、それに対応する動作を行います。  
![icon](https://github.com/misaki01/SimpleMicRemote/blob/image/README/icon.png)  

# 機能説明
音声認識とマッチングさせるキーワードは自由に設定が可能です。  
例えば、「コピー」というキーワードにキーボード入力の「Ctrl+C」を紐づければ、  
「コピー」と発言するだけでキーボードに触れなくてもコピー処理が行えるようになります。  
  
設定可能な動作は以下のとおりです。  
1. キーボードのキーを押す
    1. 任意のキーボードのキーの押す
    1. 「Ctrl+C」といった複合的なキーを押す（Winキーも含めた複合的な入力も設定可能 ）  
      （例：Win+Alt+→：仮想ディスクトップ切り替え）
    1. 特定のキーを押したままにする動作も設定可能
1. 指定したファイルを実行
    1. Exeファイルを指定  
      ⇒ そのアプリケーションを起動
    1. テキスト、Excel等のファイルを指定   
      ⇒ そのファイルを開く既定のアプリケーションで起動
    1. フォルダを指定した場合  
      ⇒ そのフォルダを開く
  
具体的な説明は下記のURLの動画を参照してください。  
[ニコニコ動画：マイクでPCの操作を行えるツールを作ってみた。](https://www.nicovideo.jp/watch/sm34754175)  
  
# ダウンロード
EXEは以下のURLで公開しております。  
[EXEのダウンロード](https://drive.google.com/file/d/1WScc-yonPnNDPomsmNQnYekVgb8bDh5E)  
  
# 動作環境
動作させるには下記が必要です。  
1. マイク
1. Chrome
1. .NET Framework 4.5 以上
  
# 対応言語
日本語のみ  
  
# 開発環境
<table>
<tr><th align="left">言語</th><td>C#</td></tr>
<tr><th align="left">開発ツール：Ver0.5まで</th><td>Visual Studio 2017 Community版</td></tr>
<tr><th align="left">開発ツール：Ver0.6以降</th><td>Visual Studio 2019 Community版</td></tr>
<tr><th align="left">フレームワーク</th><td>.NET Framework 4.5</td></tr>
<tr><th align="left">NuGet等の外部ライブラリ</th><td>使用していない</td></tr>
</table>
  
# Ｑ＆Ａ
Ｑ＆Ａは、この [Wiki](https://github.com/misaki01/SimpleMicRemote/wiki/Q&A) にまとめてあります。  
  
# ライセンス
MITライセンスです。  
詳細は「LICENSE」ファイルを見てください。  
    
# 著作者
みさきさん（自分です）

# NNS_T ニコ生サーチ(🍞)
[![Build status](https://ci.appveyor.com/api/projects/status/rjdt756hw6l8ragb/branch/master?svg=true)](https://ci.appveyor.com/project/TN8001/nns-t/branch/master)
[![MIT License](http://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/TN8001/NNS_T/blob/master/LICENSE)
![アプリスクリーンショット](https://github.com/TN8001/NNS_T/blob/master/AppImage.png)
## 概要
ニコ生を定期的に監視をして新規放送が始まったら、トースト通知風（Google Chromeの通知のほうが近いです）のポップアップを出して、お知らせするアプリです。
## 特徴
* niconico公式APIでの監視のため比較的？安心
* コミュニティ・チャンネルごとの通知ミュート（除外）機能
* 無駄におしゃれな見た目
## ダウンロード
[最新バイナリ](https://github.com/TN8001/NNS_T/releases/download/v1.1.9/NNS_T.zip)（Windows10でのみ動作確認）
## 使い方
**注意事項**に同意された方のみ使用してください。
1. ダウンロードしたNNS_T.zipを展開し、適当なフォルダに入れます
2. NNS_T.exeをダブルクリックします
3. Windows Defenderの注意喚起が出ますが、「詳細情報」を押すと「実行」ボタンが出ますので実行します（次回からは出なくなります）
4. 入力エリアにキーワードを入れて🔍ボタンを押します
5. NNS_T本体や通知の放送タイトルをクリックすると、放送ページをデフォルトブラウザで開きます

[画像付きの詳しい説明へ](https://github.com/TN8001/NNS_T/blob/master/How2Use/README.md)
## ライセンス
[MIT](https://github.com/TN8001/NNS_T/blob/master/LICENSE)
## 注意事項
* [niconicoコンテンツ検索API](http://site.nicovideo.jp/search-api-docs/search.html)
の規約はアプリ利用者にも適用されます  
↑リンクの最後にあるAPI利用規約を**必ず確認してください!!**
* niconicoのwebページでの検索結果と異なることがあります  
APIの仕様のようなのでこちらにはどうしようもありません。（確認した相違点： webページではコミュ名も検索対象になるところ、APIでは対象になりません）

* ユーザーフォルダ（c:\ユーザー\\[ユーザー名]\AppData\Local\NNS_T）
に設定を保存します  
使用を中止する場合はインストールしたフォルダとともに、上記NNS_Tフォルダも削除してください。
* Windows10以外での動作はわかりません（確認する手段がありませんので）
* 一切責任は持ちません
## 謝辞
ハンバーガーメニュー等見た目のカスタマイズに、下記を利用させて頂いております。  
[MahApps.Metro](https://github.com/MahApps/MahApps.Metro) Copyright (c) 2016 MahApps

アプリアイコン以外のアイコンはすべて、下記を利用させて頂いております。  
[MahApps.Metro.IconPacks GitHub Octicons](https://github.com/MahApps/MahApps.Metro.IconPacks) Copyright (c) 2016 MahApps, Copyright (c) 2018 GitHub Inc.

検索結果の解析に、下記を利用させて頂いております。  
[Newtonsoft.Json](https://www.newtonsoft.com/json) Copyright (c) 2007 James Newton-King

## 更新履歴
* 2017/11/13 ver1.0.0 初回リリース
* 2017/11/14 ver1.0.1 bugfix
* 2017/11/14 ver1.0.2 bugfix 小変更
* 2017/11/16 ver1.1.0 レイアウト見直し
* 2017/11/17 ver1.1.1 bugfix デザイン小変更
* 2017/11/18 ver1.1.2 bugfix niconico検索ページを開くボタン追加
* 2017/12/01 ver1.1.3 PC-9801風BEEP音調整 開くブラウザ設定追加
* 2017/12/02 ver1.1.4 bugfix
* 2017/12/19 ver1.1.5 bugfix まれに放送中のアイテムが消える 放送中にタグを追加された場合表示されない
* 2018/02/13 ver1.1.6 bugfix ミュート部屋判定にバグ
* 2018/02/18 ver1.1.7 タイトル変更追従 AngleSharp非依存化
* 2018/03/06 ver1.1.8 bugfix 通知のミュートボタンが機能していなかった 検索ボタンをdisableにしないように変更
* 2018/04/15 ver1.1.9 要望対応


﻿# 詳細説明
## メインウィンドウ
![メインウィンドウ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/MainWindow.png)
1. 消音トグルボタン  
オプション設定の状態によらず、消音のON・OFFを切り替えます。  
赤がON（通知音を出さない）、グレーがOFF（オプション設定の通知音に従う）です。

2. Githubボタン  
GithubのNNS_Tのページを、規定ブラウザで開きます。

3. メニューボタン  
ページ選択メニューを開きます。

4. 検索ページボタン  
検索ページを出します。

5. 通知除外ページボタン  
通知除外管理ページを出します。

6. 設定ページボタン  
オプション設定ページを出します。

7. アプリについてページボタン  
このアプリについてのページを出します。


### 検索ページ
![検索ページ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/Search.png)

メインのページ 検索条件とその結果
1. キーワード入力エリア  
検索キーワードを入力します。空の状態で薄く書式が出ます。    
例：[ボーカル OR ヴォーカル ミク -歌ってみた]の場合  
[ボーカル]または[ヴォーカル]を含むものの中で、さらに[ミク]を含み、[歌ってみた]を含まない もの  
カーソルが表示されている間は、以前の入力のまま自動検索されます。エンターキーを押すか、検索ボタンで直ちに再検索します。  

2. 検索オプション開閉トグルボタン  
検索オプションを開閉します。

3. クリアボタン  
入力内容をクリアします。

4. 検索ボタン  
直ちに再検索します。  
自動検索中は一瞬ボタンが使えなくなっているので注意してください。

5. 検索ターゲットラジオボタン  
    * タグ検索  
    キーワードがタグに完全一致するものを検索します。
    * キーワード検索  
    キーワードが部分一致するものを検索します。

6. 検索ターゲットチェックボックス  
どこからキーワード検索するかを選択します。

7. 公式をミュートする  
設定ページのものと同じです。そちらの説明をご覧ください。

8. ミュートを表示する    
設定ページのものと同じです。そちらの説明をご覧ください。

9. ミュートトグルボタン  
クリックでミュート・ミュート解除を切り替えます。  
ミュートを非表示している場合は、通知除外ページで解除できます。

10. 生放送リンク  
クリックすると規定ブラウザで、放送ページを開きます。  
ドラッグで他のアプリに落とすことも可能です。（NCV[ニコ生コメントビューア]と、Google Chromeで確認‎）

### 通知除外ページ
![通知除外ページ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/Mute.png)

通知表示を除外するコミュニティ・チャンネルの管理
1. リンク  
クリックすると規定ブラウザで、コミュニティ・チャンネルページを開きます。  
ドラッグで他のアプリに落とすことも可能です。（Google Chromeで確認‎）

2. 削除ボタン  
通知除外を解除します。

### 設定ページ
![設定ページ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/Settings.png)

オプション設定
1. 通知
    * 常に出す  
    ウィンドウ状態によらず、常に通知を出します。
    * 非アクティブ時に出す  
    ほかのアプリを使用中にだけ通知を出します。
    * アイコン化時に出す  
    最小化した時だけ通知を出します。
    * 常に出さない  
    通知機能を使わないようにします。

2. 通知音
    * Windows設定  
    通知を出す際に、Windows設定の通知音を鳴らします。
    * PC-9801風BEEP音  
    通知を出す際に、ピポッといった音を鳴らします。
    * 音を出さない  
    通知を出す際に、音を鳴らしません。  

3. サウンドを開く  
Windowsのサウンド設定ウィンドウを開きます。
###### 通知音の場所
![サウンドWindows設定画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/Sound.png)

プログラム イベント→Windows→通知 の音を使用します。  

4. 📢  
通知音をテストで鳴らします。

5. 音量ミキサーを開く  
音量ミキサーを開きます。
[Windows設定]はNNS_Tのアイコンのボリュームで、[PC-9801風BEEP音]はシステム音のボリュームで調整できます。

6. 通知表示期間  
通知が自動的に消えるまでの時間 1秒～60秒

7. 検索間隔  
niconicoAPIに検索に行く間隔 10秒～1800秒（30分）

8. 説明文を取得しない  
生放送の詳細（生放送画面の下に出ている文章）を取得しないようにします。  
表示サイズの都合上冒頭の十数文字しか表示できませんが通信データの半分以上を占めますので、いらない場合はチェックを入れると通信量半減・多少動作が軽くなると思います。

9. 公式をミュートする  
公式放送を通知しないようにします。  
公式には部屋の概念が無いようで、個別に通知除外する方法がありませんでした。個別に通知除外したい場合は、検索キーワードを工夫して対応してください。

10. ミュートを表示する  
通知除外した放送を検索結果一覧に出すようにします。

11. 設定ファイルフォルダを開く  
設定ファイルのあるフォルダをエクスプローラーで開きます。

12. インストールフォルダを開く  
NNS_T.exeのあるフォルダをエクスプローラーで開きます。

### アプリについてページ
![アプリについてページ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/About.png)

バージョン情報・謝辞

## 通知ウィンドウ
![通知ウィンドウ画像](https://github.com/TN8001/NNS_T/blob/master/How2Use/Toast.png)

新規生放送があった場合、メインウィンドウの右下に、通知を最前面でポップアップします。（マルチモニタでの検証はできていません）  
複数の放送があった場合まとめて表示します。（３～４件は一覧できますがそれ以上はスクロールしてください）  
設定時間で自動的に消えますが、マウスカーソルが乗っている間は消えません。

1. 生放送リンク  
クリックすると規定ブラウザで、放送ページを開きます。  
ドラッグで他のアプリに落とすことも可能です。（NCV[ニコ生コメントビューア]と、Google Chromeで確認‎）

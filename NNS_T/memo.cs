﻿// タスク管理 実験用ファイル

///////////////////////////////////////////////////////////////////////////////
// バグ・不具合 bug  早急に対処が必要なもの

// bug 古い放送が新規扱いに
//100件以上hit状態で取得間に新規放送よりhit内の終了放送が多いと100件より先にあったデータが追加される
//      リスト内100件    範囲外
//■■■■■～■■■■■ □□□…
//新規で3件あったら後ろの黒3つが範囲外になる  問題ない
//□□□■■■■■～■■ ■■■□□□…
//新規１件 黒で終了３件あった場合 右の白２件がリスト中に混入してしまう  問題あり！
//□■■～■■■■■□□ □…
//先頭より古いものははじく １分以上？２分以上？？


///////////////////////////////////////////////////////////////////////////////
// 改善点 todo  対応したいと思っているが実装が進んでいないもの

//todo 起動時にページアイコンが選択されない

//todo インストールフォルダ 設定ファイルフォルダを開く機能

//todo 音量調整

//todo 同時取得画像が多いと通知中に画像ロードが間に合わない時がある
//Imageのキャッシュ周りはできれば触りたくない。。。ｗ

//todo 検索ボタンの一瞬disable問題
//再入時にどうするか。。。




///////////////////////////////////////////////////////////////////////////////
// 要望 enhancement  あると嬉しいor要望があったが どうするか悩んでいるもの

//enhancement 今タイマが作動しているか確認したい できれば裏コマンドでトグル
//表示方法 操作方法

//enhancement 検索結果が0の時何か表示 もしくはhit数を表示
//表示方法

//enhancement ページヘッダが無駄っぽい
//バランスが。。。

//enhancement 複数追加時 通知とリストのアイテムの順が逆


//enhancement アイコンがダサい
//。。。


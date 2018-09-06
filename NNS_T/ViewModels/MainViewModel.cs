using Microsoft.Win32;
using NNS_T.Models;
using NNS_T.Models.NicoAPI;
using NNS_T.Utility;
using NNS_T.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NNS_T.ViewModels
{
    public class MainViewModel : Observable
    {
        #region public property
        ///<summary>レスポンスエラー理由</summary>
        public string ErrorStatus { get => _ErrorStatus; set => Set(ref _ErrorStatus, value); }
        private string _ErrorStatus;

        ///<summary>一時ミュート</summary>
        public bool IsTemporaryMuted { get => ToastWindow.IsTemporaryMuted; set => Set(ref ToastWindow.IsTemporaryMuted, value); }

        ///<summary>通常・最大化・最小化</summary>
        public WindowState WindowState { get => _WindowState; set => Set(ref _WindowState, value); }
        private WindowState _WindowState;

        ///<summary>取得タイマonoff</summary>
        public bool IsTimerEnabled { get => _IsTimerEnabled; set { if(Set(ref _IsTimerEnabled, value)) timer.IsEnabled = value; } }
        private bool _IsTimerEnabled;

        ///<summary>ヒットカウント</summary>
        public int HitCount { get => _HitCount; set => Set(ref _HitCount, value); }
        private int _HitCount;

        ///<summary>新しいバージョンがある</summary>
        public bool NewVersionPublished { get => _NewVersionPublished; set => Set(ref _NewVersionPublished, value); }
        private bool _NewVersionPublished;

        ///<summary>新しいバージョンの概要</summary>
        public string NewVersionMassage { get => _NewVersionMassage; set => Set(ref _NewVersionMassage, value); }
        private string _NewVersionMassage;

        ///<summary>検索結果放送コレクション</summary>
        public ObservableCollection<LiveItemViewModel> Items { get; } = new ObservableCollection<LiveItemViewModel>();

        ///<summary>ユーザー設定</summary>
        public SettingsModel Settings { get; }

        ///<summary>検索コマンド</summary>
        public RelayCommand SearchCommand { get; }

        ///<summary>設定保存コマンド</summary>
        public RelayCommand SaveCommand { get; }

        ///<summary>放送ミュート解除コマンド</summary>
        public RelayCommand<RoomModel> UnMuteCommand { get; }

        ///<summary>ブラウザで開くコマンド</summary>
        public RelayCommand<string> OpenBrowserCommand { get; }

        ///<summary>ブラウザ選択コマンド</summary>
        public RelayCommand<string> SelectBrowserPathCommand { get; }

        ///<summary>ProcessStartコマンド</summary>
        public RelayCommand<string> ProcessStartCommand { get; }

        ///<summary>規定ファイラで開くコマンド</summary>
        public RelayCommand<FolderType> OpenFolderCommand { get; }

        ///<summary>通知音を鳴らすコマンド</summary>
        public RelayCommand PlaySoundCommand { get; }

        ///<summary>取得タイマonoff（裏）コマンド</summary>
        public RelayCommand ToggleTimerCommand { get; }

        ///<summary>ニコニコ検索ページにジャンプコマンド</summary>
        public RelayCommand NicoWebCommand { get; }
        #endregion

        private DispatcherTimer timer = new DispatcherTimer();
        private NicoApi nicoApi = new NicoApi(ProductInfo.Name);
        // 条件変更フラグ
        private bool isDirty = true;


        public MainViewModel()
        {
            var configPath = GetConfigPath();
            try
            {
                Settings = SettingsHelper.Load<SettingsModel>(configPath);
                // 旧版を使っていてUpdateCheck機能がついてから初めての起動の場合
                // (true falseを決めていない状態)
                if(Settings.UpdateCheck == null)
                {
                    // 一応確認を入れる
                    Settings.UpdateCheck = InfoUpdateCheck();

                    // UpdateCheckより前のバージョンで対応済みだが無駄な処理なので
                    // この際ここで解消（一回処理すれば次回以降必要ない）

                    // 何故かIconUrlで同定していたため複数追加されている可能性あり
                    Settings.Mute.Items = new MuteCollection(Settings.Mute.Items.Distinct());
                }
            }
            catch
            {
                Debug.WriteLine("fail Deserialize");

                // 旧版を使っていない場合はチェックするがデフォでいいでしょ
                Settings = new SettingsModel { UpdateCheck = true };
            }

            if(Settings.UpdateCheck == true)
            {
                var url = "https://github.com/TN8001/NNS_T/releases.atom";
                var checker = new UpdateChecker(url, ProductInfo.Version);
                var text = checker.GetNewVersionString();

                if(text != "")
                {
                    NewVersionMassage = "新しいバージョンがあります\n\n" + text;
                    NewVersionPublished = true;
                }
            }


            // 検索間隔 条件変更フラグ 更新
            Settings.Search.PropertyChanged += (s, e) =>
            {
                var search = (SearchModel)s;
                if(e.PropertyName == nameof(search.IntervalSec))
                    timer.Interval = TimeSpan.FromSeconds(search.IntervalSec);
                else
                    isDirty = true;
            };
            // 公式をミュート 変更
            Settings.Mute.PropertyChanged += (s, e) =>
            {
                var mute = (MuteModel)s;
                if(e.PropertyName != nameof(mute.Official)) return;

                foreach(var item in Items.Where(x => x.ProviderType == ProviderType.Official))
                    item.IsMuted = mute.Official;
            };

            SearchCommand = new RelayCommand(async () => await SearchCommandImplAsync());
            SaveCommand = new RelayCommand(() => SettingsHelper.Save(Settings, configPath));

            // 各アイテム内からバインド可能なようにインジェクション
            LiveItemViewModel._ToggleMuteCommand = new RelayCommand<LiveItemViewModel>(async (liveItem) =>
            {
                if(liveItem.ProviderType == ProviderType.Official) return; // 来ないはず

                try
                {
                    var name = await nicoApi.GetRoomNameAsync(liveItem.RoomID);
                    var room = new RoomModel(liveItem.RoomID, name, liveItem.IconUrl);
                    if(liveItem.IsMuted)
                        Settings.Mute.Items.Add(room);
                    else
                        Settings.Mute.Items.Remove(room);
                }
                catch(Exception e)
                {
                    Debug.WriteLine($"MuteCommand error {e.Message}");
                    ErrorStatus = e.Message;
                }
            });

            UnMuteCommand = new RelayCommand<RoomModel>((room) =>
            {
                Settings.Mute.Items.Remove(room);
                var liveItem = Items.FirstOrDefault(x => x.RoomID == room.ID);
                if(liveItem == null) return;

                liveItem.IsMuted = false;
            });
            ProcessStartCommand = new RelayCommand<string>((s) =>
            {
                try
                {
                    var index = s.IndexOf(' ');
                    if(index < 0)
                        Process.Start(s);
                    else
                        Process.Start(s.Substring(0, index), s.Substring(index + 1));
                }
                catch { /* NOP */ Debug.WriteLine($"ProcessStartCommand error"); }
            });

            //Edgeの場合
            //shell:AppsFolder\Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge
            OpenBrowserCommand = new RelayCommand<string>((s) =>
            {
                try
                {
                    if(string.IsNullOrEmpty(Settings.BrowserPath))
                        Process.Start(s);
                    else
                        Process.Start(Settings.BrowserPath, s);
                }
                catch { /* NOP */ Debug.WriteLine($"OpenBrowserCommand error"); }
            });
            SelectBrowserPathCommand = new RelayCommand<string>((s) =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "ブラウザを選択",
                    FilterIndex = 0,
                    Filter = "ブラウザ(.exe)|*.exe"
                };
                var result = openFileDialog.ShowDialog();
                if(result == true)
                    Settings.BrowserPath = openFileDialog.FileName;
            });
            OpenFolderCommand = new RelayCommand<FolderType>((f) =>
            {
                try
                {
                    string path;
                    switch(f)
                    {
                        case FolderType.Assembly:
                            path = AppDomain.CurrentDomain.BaseDirectory;
                            break;
                        case FolderType.Settings:
                            var p = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                            path = Path.Combine(p, ProductInfo.Name);
                            break;
                        default:
                            return;
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true,
                        Verb = "open",
                    });
                }
                catch { /* NOP */ Debug.WriteLine($"OpenFolderCommand error"); }
            });
            PlaySoundCommand = new RelayCommand(() => ToastWindow.PlaySound());
            ToggleTimerCommand = new RelayCommand(() => IsTimerEnabled = !IsTimerEnabled);
            NicoWebCommand = new RelayCommand(() =>
            {
                var q = CreateQuery();
                var s = nicoApi.GetSearchUrl(q, Settings.Mute.Official);
                OpenBrowserCommand.Execute(s);
            });

            timer.Interval = TimeSpan.FromSeconds(Settings.Search.IntervalSec);
            timer.Tick += (s, e) => SearchCommand.Execute();

            SearchCommand.Execute();
        }

        private CancellationTokenSource cts;
        private object lockObj = new object();
        private async Task SearchCommandImplAsync()
        {
            Debug.WriteLine("SearchCommand");

            // 取得時間があんまり安定しないので取得中は止めてみる
            // よって実際の取得間隔は設定以上の間隔が開く
            IsTimerEnabled = false;
            // 空のQueryの場合タイマ停止のまま
            if(string.IsNullOrEmpty(Settings.Search.Query)) return;

            try
            {
                // 再入時に前回をキャンセル
                lock(lockObj)
                {
                    if(cts != null)
                    {
                        Debug.WriteLine($"cts.Cancel");
                        cts.Cancel();
                        cts.Dispose();
                    }
                    cts = new CancellationTokenSource();
                }

                var q = CreateQuery();
                var response = await nicoApi.GetResponseAsync(Services.Live, q, cts.Token);
                HitCount = response.Meta.TotalCount;
                if(isDirty)
                    Items.Clear();
                ItemsUpdate(response);
                ErrorStatus = null;
                isDirty = false;
                IsTimerEnabled = true;

                Debug.WriteLine("正常終了");
            }
            catch(OperationCanceledException)
            {
                Debug.WriteLine($"キャンセル");
            }
            catch(Exception e)
            {
                Debug.WriteLine($"SearchCommandImplAsync error {e.Message}");

                ErrorStatus = e.Message;
                HitCount = 0;
                IsTimerEnabled = true;
            }
        }

        private Query CreateQuery() => new Query
        {
            Keyword = Settings.Search.Query,
            Targets = Settings.Search.Targets,
            Fields = Settings.Search.Fields,
            Filters = new NameValueCollection { { "liveStatus", "onair" } },
            Sort = "-startTime",
            Limit = 100,
            Context = ProductInfo.Name,
        };

        private int ItemsUpdate(Response response)
        {
            // ToArrayとか多すぎｗ

            var responseItems = response.Data.Select(x => new LiveItemViewModel(x)).ToArray();
            var removeItems = Items.Except(responseItems).ToArray();
            var updateItems = Items.Intersect(responseItems).ToArray();
            var count = removeItems.Count();
            if(count > 0) Debug.WriteLine($"Remove count:{count}");

            foreach(var item in removeItems)
                item.Delete(Items);

            foreach(var item in updateItems)
            {
                var i = Array.Find(responseItems, x => x.Equals(item));

                item.Update(i);
            }

            var addItems = responseItems.Except(Items)
                                        .Where(x => !x.MemberOnly || !Settings.Search.HideMemberOnly)
                                        .OrderBy(x => x.StartTime).ToArray();
            var addCount = addItems.Count();
            if(addCount > 0) Debug.WriteLine($"Add count:{addCount}");

            foreach(var item in addItems)
            {
                if(item.ProviderType == ProviderType.Official && Settings.Mute.Official)
                    item.IsMuted = true;
                else if(Settings.Mute.Items.Any(x => x.ID == item.RoomID))
                {
                    item.IsMuted = true;
                    // 新サムネ対応入れ替え処理 2018/02/13
                    // http://icon.nimg.jp/community/373/co3734277.jpg
                    // to
                    // https://secure-dcdn.cdn.nimg.jp/comch/community-icon/128x128/co3734277.jpg
                    var m = Settings.Mute.Items.First(x => x.ID == item.RoomID);
                    if(m.IconUrl != item.IconUrl) m.IconUrl = item.IconUrl;
                }

                Items.Insert(0, item);
                item.IsLoaded = true;
            }

            ShowToast(addItems.Where(x => !x.IsMuted).Reverse());

            return addCount;
        }

        private void ShowToast(IEnumerable<LiveItemViewModel> items)
        {
            if(isDirty) return;

            switch(Settings.Notify.State)
            {
                case NotifyState.Always:
                    break;

                case NotifyState.Inactive:
                    if(Application.Current.MainWindow.IsActive) return;
                    break;

                case NotifyState.Minimize:
                    if(WindowState != WindowState.Minimized) return;
                    break;

                default:
                    return;
            }

            ToastWindow.ShowToast(items);
        }
        private string GetConfigPath()
        {
            var cmds = Environment.GetCommandLineArgs();
            if(cmds.Count() > 1)
            {
                var name = Path.GetFileName(cmds[1]);
                if(name == "") return null;

                var dir = Path.GetDirectoryName(SettingsHelper.GetDefaultPath());
                return Path.Combine(dir, name);
            }

            return null;
        }
        // UpdateCheckの説明MessageBox表示
        private bool? InfoUpdateCheck()
        {
            var message =
@"今更ですが更新確認機能が付きました。

起動時に新しいバージョンが出ていないかを確認します。
確認するだけで実際に更新するのは手動になります。

新バージョンが出ていた場合、タイトルバーのGitHubボタンが黄色になります。
クリックするとリリースページに飛びます。

更新確認機能を使いますか？（Settingsページでいつでも変更可能です）";

            var r = MetroMessageBox.Show(message, "NNS_T", "はい,いいえ,後で", MessageBoxImage.Information, 0, 2);
            if(r == "はい") return true;
            if(r == "いいえ") return false;
            return null;
        }
    }
}

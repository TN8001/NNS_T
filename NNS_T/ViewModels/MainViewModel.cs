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

        ///<summary>取得中</summary>
        public bool IsBusy { get => _IsBusy; set { if(Set(ref _IsBusy, value)) SearchCommand.RaiseCanExecuteChanged(); } }
        private bool _IsBusy;

        ///<summary>一時ミュート</summary>
        public bool IsTemporaryMuted { get => ToastWindow.IsTemporaryMuted; set => Set(ref ToastWindow.IsTemporaryMuted, value); }

        ///<summary>通常・最大化・最小化</summary>
        public WindowState WindowState { get => _WindowState; set => Set(ref _WindowState, value); }
        private WindowState _WindowState;

        ///<summary>取得タイマonoff</summary>
        public bool IsTimerEnabled { get => _IsTimerEnabled; set { if(Set(ref _IsTimerEnabled, value)) timer.IsEnabled = value; } }
        private bool _IsTimerEnabled = true;

        ///<summary>ヒットカウント</summary>
        public int HitCount { get => _HitCount; set => Set(ref _HitCount, value); }
        private int _HitCount;

        ///<summary>検索結果放送コレクション</summary>
        public ObservableCollection<LiveItemViewModel> Items { get; } = new ObservableCollection<LiveItemViewModel>();

        ///<summary>ユーザー設定</summary>
        public SettingsModel Settings { get; }

        ///<summary>検索コマンド</summary>
        public RelayCommand SearchCommand { get; }

        ///<summary>設定保存コマンド</summary>
        public RelayCommand SaveCommand { get; }

        ///<summary>放送ミュートコマンド</summary>
        public RelayCommand<LiveItemViewModel> MuteCommand { get; }

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
        // 設定ファイルフルパス コマンドラインで指定
        // （ファイル名のみ採用しdirはユーザーフォルダ固定
        private readonly string configPath;

        public MainViewModel()
        {
            configPath = GetConfigPath();
            Settings = SettingsHelper.LoadOrDefault<SettingsModel>(configPath);

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

            SearchCommand = new RelayCommand(async () => await SearchCommandImplAsync(), () => !IsBusy);
            SaveCommand = new RelayCommand(() => SettingsHelper.Save(Settings, configPath));
            MuteCommand = new RelayCommand<LiveItemViewModel>(async (liveItem) =>
            {
                if(liveItem.ProviderType == ProviderType.Official) return; // 来ないはず

                var name = await nicoApi.GetRoomNameAsync(liveItem.RoomID);
                var room = new RoomModel(liveItem.RoomID, name, liveItem.IconUrl);
                if(liveItem.IsMuted)
                    Settings.Mute.Items.Add(room);
                else
                    Settings.Mute.Items.Remove(room);
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
                catch { /* NOP */ }
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
                catch { /* NOP */ }
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
                catch { /* NOP */ }
            });
            PlaySoundCommand = new RelayCommand(() => ToastWindow.PlaySound());
            ToggleTimerCommand = new RelayCommand(() => IsTimerEnabled = !IsTimerEnabled);
            NicoWebCommand = new RelayCommand(() =>
            {
                var q = new Query
                {
                    Keyword = Settings.Search.Query,
                    Targets = Settings.Search.Targets,
                };

                var s = nicoApi.GetSearchUrl(q, Settings.Mute.Official);
                OpenBrowserCommand.Execute(s);
            });

            timer.Interval = TimeSpan.FromSeconds(Settings.Search.IntervalSec);
            timer.Tick += (s, e) =>
            {
                if(SearchCommand.CanExecute())
                    SearchCommand.Execute();
            };

            SearchCommand.Execute();
        }


        private async Task SearchCommandImplAsync()
        {
            Debug.WriteLine("SearchCommand");
            if(IsBusy) return; // 通らないはず

            if(string.IsNullOrEmpty(Settings.Search.Query))
            {
                IsTimerEnabled = false;
                return;
            }
            // 取得時間があんまり安定しないので取得中は止めてみる
            // よって実際の取得間隔は設定以上の間隔が開く
            IsTimerEnabled = true;
            timer.Stop();

            IsBusy = true;
            Response response;
            try
            {
                var t = DateTime.Now;
                var q = new Query
                {
                    Keyword = Settings.Search.Query,
                    Targets = Settings.Search.Targets,
                    Fields = Settings.Search.Fields,
                    Filters = new NameValueCollection { { "liveStatus", "onair" } },
                    Sort = "-startTime",
                    Limit = 100,
                    Context = ProductInfo.Name,
                };
                response = await nicoApi.GetResponseAsync(Services.Live, q);
            }
            catch(NicoApiRequestException e)
            {
                Debug.WriteLine($"NicoApiRequestException {e.Message}");
                ErrorStatus = e.Message;
                HitCount = 0;
                IsBusy = false;
                if(IsTimerEnabled)
                    timer.Start();
                return;
            }

            ErrorStatus = null;
            HitCount = response.Meta.TotalCount;
            if(isDirty)
                Items.Clear();
            ItemsUpdate(response);

            IsBusy = false;
            isDirty = false;
            if(IsTimerEnabled)
                timer.Start();
        }

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
                //if(i == null) continue;

                item.Update(i);
            }

            // ToDo 放送途中でのタグ追加を考慮していなかった
            // 表示をどうするか。。
            // 1)時間順で並び替えをやめる（ある意味非常に楽
            // 2)時間順でそのまま追加（見えなくてもしょうがない
            // とりあえず1)でやってみる
            // ↑のため ↓の小細工を中止

            //// 2分の根拠はないが1分だと取り逃しが出そうな気がする
            //var first = Items.FirstOrDefault();
            //var time = first != null ? first.StartTime - TimeSpan.FromMinutes(2) : DateTime.MinValue;
            var addItems = responseItems.Except(Items)
                                        //.Where(x => x.StartTime > time)
                                        .OrderBy(x => x.StartTime).ToArray();
            var addCount = addItems.Count();
            if(addCount > 0) Debug.WriteLine($"Add count:{addCount}");

            foreach(var item in addItems)
            {
                if(item.ProviderType == ProviderType.Official && Settings.Mute.Official)
                    item.IsMuted = true;
                else if(Settings.Mute.Items.Any(x => x.IconUrl == item.IconUrl))
                    item.IsMuted = true;

                Items.Insert(0, item);
                item.IsLoaded = true;
            }

            ShowToast(addItems.Where(x => !x.IsMuted).Reverse());

            return addCount;
        }

        private void ShowToast(IEnumerable<LiveItemViewModel> items)
        {
            if(isDirty) return;
            //if(!Settings.Notify.IsEnabled) return;

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
    }
}

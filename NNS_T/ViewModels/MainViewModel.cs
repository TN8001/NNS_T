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

        ///<summary>規定ブラウザで開くコマンド</summary>
        public RelayCommand<string> ProcessStartCommand { get; }

        ///<summary>規定ブラウザで開くコマンド</summary>
        public RelayCommand<FolderType> OpenFolderCommand { get; }

        #region デバッグ用
        // どこかに表示してもいいのですが あまりごちゃごちゃさせたくないので
        ///<summary>取得タイマonoff</summary>
        public bool IsTimerEnabled { get => timer.IsEnabled; set { timer.IsEnabled = value; OnPropertyChanged(); } }

        ///<summary>ヒットカウント</summary>
        public int HitCount { get => _HitCount; set => Set(ref _HitCount, value); }
        private int _HitCount;

        ///<summary>追加カウント</summary>
        public int AddCount { get => _AddCount; set => Set(ref _AddCount, value); }
        private int _AddCount;

        ///<summary>アイテムカウント</summary>
        public int ItemCount { get => _ItemCount; set => Set(ref _ItemCount, value); }
        private int _ItemCount;

        ///<summary>レスポンス取得にかかった時間</summary>
        public TimeSpan ResponseTime { get => _ResponseTime; set => Set(ref _ResponseTime, value); }
        private TimeSpan _ResponseTime;

        ///<summary>WorkingSet64</summary>
        public string WorkingSet64 { get => _Mem; set => Set(ref _Mem, value); }
        private string _Mem;

        ///<summary>一覧クリアコマンド</summary>
        public RelayCommand ClearCommand { get; }
        #endregion
        #endregion

        private DispatcherTimer timer = new DispatcherTimer();
        private NicoApi nicoApi = new NicoApi(ProductInfo.Name);
        // 条件変更フラグ
        private bool isDirty = true;
        // 設定ファイルパス コマンドラインで指定（ファイル名のみ採用
        private readonly string configPath;

        public MainViewModel()
        {
            configPath = GetConfigPath();
            Settings = SettingsHelper.LoadOrDefault<SettingsModel>(configPath);

            // 検索間隔 条件変更フラグ
            Settings.Search.PropertyChanged += (s, e) =>
            {
                var search = (SearchModel)s;
                if(e.PropertyName == nameof(search.IntervalSec))
                    timer.Interval = TimeSpan.FromSeconds(search.IntervalSec);
                else
                    isDirty = true;
            };
            // 公式ミュート
            Settings.Mute.PropertyChanged += (s, e) =>
            {
                var mute = (MuteModel)s;
                if(e.PropertyName != nameof(mute.OfficialIgnored)) return;

                foreach(var item in Items.Where(x => x.ProviderType == ProviderType.Official))
                    item.IsMuted = mute.OfficialIgnored;
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
                {
                    Debug.WriteLine(Settings.Mute.Items.Count);
                    var b = Settings.Mute.Items.Remove(room);
                    Debug.WriteLine(Settings.Mute.Items.Count);
                    Debug.WriteLine($"Remove:{b}");
                }
            });
            UnMuteCommand = new RelayCommand<RoomModel>((room) =>
            {
                Settings.Mute.Items.Remove(room);
                var liveItem = Items.FirstOrDefault(x => x.RoomID == room.ID);
                if(liveItem == null) return;

                liveItem.IsMuted = false;
            });
            ProcessStartCommand = new RelayCommand<string>((s) => Process.Start(s));
            ClearCommand = new RelayCommand(() => Items.Clear());
            OpenFolderCommand = new RelayCommand<FolderType>((f) =>
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
            CheckMemory();

            // 取得時間があんまり安定しないので取得中は止めてみる
            // よって実際の取得間隔は設定以上の間隔が開く
            timer.Stop();
            if(string.IsNullOrEmpty(Settings.Search.Query)) return;

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
                ResponseTime = DateTime.Now - t;
            }
            catch(NicoApiRequestException e)
            {
                Debug.WriteLine($"NicoApiRequestException {e.Message}");
                ErrorStatus = e.Message;
                HitCount = 0;
                IsBusy = false;
                timer.Start();
                return;
            }

            ErrorStatus = null;
            HitCount = response.Meta.TotalCount;
            AddCount = ItemsUpdate(response);
            ItemCount = Items.Count;
            IsBusy = false;
            isDirty = false;
            timer.Start();
        }

        private int ItemsUpdate(Response response)
        {
            // ToArrayとか多すぎｗ

            var responseItems = response.Data.Select(x => new LiveItemViewModel(x)).ToArray();
            var removeItems = Items.Except(responseItems).ToArray();
            var count = removeItems.Count();
            if(count > 0) Debug.WriteLine($"Remove count:{count}");

            foreach(var item in removeItems)
                Items.Remove(item);

            foreach(var item in Items)
            {
                var i = Array.Find(responseItems, x => x.Equals(item));
                if(i == null) continue;

                item.Update(i);
            }

            // fix?? bug 古い放送が新規扱いに
            // 2分の根拠はないが1分だと取り逃しが出そうな気がする
            var first = Items.FirstOrDefault();
            var time = first != null ? first.StartTime - TimeSpan.FromMinutes(2) : DateTime.MinValue;
            var addItems = responseItems.Except(Items)
                                        .Where(x => x.StartTime > time)
                                        .OrderBy(x => x.StartTime).ToArray();
            var addCount = addItems.Count();
            if(addCount > 0) Debug.WriteLine($"Add count:{addCount}");

            foreach(var item in addItems)
            {
                if(item.ProviderType == ProviderType.Official && Settings.Mute.OfficialIgnored)
                    item.IsMuted = true;
                else if(Settings.Mute.Items.Any(x => x.IconUrl == item.IconUrl))
                    item.IsMuted = true;

                Items.Insert(0, item);
                item.IsLoaded = true;
            }

            ShowToast(addItems.Where(x => !x.IsMuted).Reverse());

            return addCount;
        }

        private void ShowToast(IEnumerable<LiveItemViewModel> list)
        {
            if(isDirty) return;
            if(!Settings.Notify.IsEnabled) return;

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

            ToastWindow.ShowToast(list);
        }
        private string GetConfigPath()
        {
            var cmds = Environment.GetCommandLineArgs();
            if(cmds.Count() > 1)
            {
                var s = Path.GetFileName(cmds[1]);
                if(s == "") return null;

                var p = Path.GetDirectoryName(SettingsHelper.GetDefaultPath());
                return Path.Combine(p, s);
            }

            return null;
        }

        [Conditional("DEBUG")]
        private void CheckMemory() => WorkingSet64 = $"{Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} MB";
    }
}

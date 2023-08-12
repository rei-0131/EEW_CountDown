using KyoshinMonitorLib;
using KyoshinMonitorLib.ApiResult.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kyoshin_Rei_WPF
{
#pragma warning disable CS4014
    public partial class MainWindow : Window
    {
        Dictionary<string, List<string>> Quake_Dict = new();
        Dictionary<string, List<string>> Quake_tmp = new();
        public static double my_lat = 35;
        public static double my_lon = 136;
        public static int p_seconds = 999;
        public static int s_seconds = 999;
        public static List<int> nums = new();
        public static ApiResult<Eew> result_eew;
        public static bool start = false;
        public static DateTime targetTime;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i <= 1000; i++)
            {
                nums.Add(i);
            }
            Geteew();
            Disp();
        }

        private async Task Geteew()
        {
            
            while (true)
            {
                try
                {
                    Dispatcher.InvokeAsync(() =>
                    { 
                        time_ref.Opacity = 1; 
                    });
                    using var webApi = new WebApi();
                    result_eew = await webApi.GetEewInfo(DateTime.Now.AddSeconds(-1));
                    webApi.Dispose();
                    Dispatcher.InvokeAsync(() =>
                    {
                        time_ref.Opacity = 0; 
                    });
                }
                catch (Exception)
                {
                    ;
                }
                await Task.Delay(2000);
            }
        }

        private async Task Disp()
        {
            await Task.Delay(2000);
            while (true)
            {
                
                targetTime = DateTime.Now.AddSeconds(-1);
                now_time.Text = targetTime.ToString("yyyy/MM/dd HH:mm:ss");
                await Task.Run(() =>
                {
                    if (result_eew.Data != null || result_eew != null)
                    {
                        if (result_eew.Data.Result.Message != "データがありません")
                        {
                            App.Current.Windows.OfType<Window>().Where(w => w.WindowState == WindowState.Minimized).First().Show();
                            var mai_second = (targetTime - DateTime.ParseExact(result_eew.Data.ReportId.ToString(), "yyyyMMddHHmmss", null)).TotalSeconds;
                            if (Quake_Dict.ContainsKey(result_eew.Data.ReportId.ToString()) == false)
                            {
                                Quake_tmp.Add(result_eew.Data.ReportId.ToString(), new List<string>() {});
                                p_seconds = 999;
                                s_seconds = 999;
                                foreach (var n in Quake_tmp)
                                {
                                    Quake_Dict.Add(n.Key, n.Value);
                                }
                            }
                            if (Quake_Dict.ContainsKey(result_eew.Data.ReportId.ToString()))
                            {
                                bool tmp_Tr = false;
                                foreach (var n in Quake_Dict)
                                {
                                    if (n.Key == result_eew.Data.ReportId.ToString())
                                    {
                                        if (Quake_Dict[n.Key].Count != 0)
                                        {
                                            for (var x = 0; x < Quake_Dict[n.Key].Count; x++)
                                            {
                                                if (Quake_Dict[n.Key][x] == result_eew.Data.ReportNum.Value.ToString())
                                                {
                                                    tmp_Tr = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (tmp_Tr == false)
                                {
                                    int depth_int = result_eew.Data.Depth.Value;
                                    var dis = CalculateDistance(my_lat, my_lon, result_eew.Data.Location.Latitude, result_eew.Data.Location.Longitude);
                                    if (Quake_Dict[result_eew.Data.ReportId.ToString()].Count == 0)
                                    {
                                        Thread thread0 = new(new ThreadStart(() =>
                                        {
                                            TravelTimeTableConverter.ImportData();
                                            bool p_bo = false;
                                            (double, double) ps_item;

                                            foreach (int x in nums)
                                            {
                                                if(p_bo == false)
                                                {
                                                    ps_item = TravelTimeTableConverter.GetValue(depth_int, x);
                                                    var p = ps_item.Item1;
                                                    if (p >= dis)
                                                    {
                                                        p_seconds = x;
                                                        p_bo=true;
                                                    }
                                                }
                                                ps_item = TravelTimeTableConverter.GetValue(depth_int, x);
                                                var s = ps_item.Item2;
                                                if (s >= dis)
                                                {
                                                    s_seconds = x;
                                                    break;
                                                }
                                            }
                                        }));
                                        thread0.Start();
                                    }
                                    else if (Quake_Dict[result_eew.Data.ReportId.ToString()].Count % 4 == 0)
                                    {
                                        Thread thread = new(new ThreadStart(() =>
                                        {
                                            TravelTimeTableConverter.ImportData();
                                            bool p_bo = false;
                                            (double, double) ps_item;

                                            foreach (int x in nums)
                                            {
                                                if (p_bo == false)
                                                {
                                                    ps_item = TravelTimeTableConverter.GetValue(depth_int, x);
                                                    var p = ps_item.Item1;
                                                    if (p >= dis)
                                                    {
                                                        p_seconds = x;
                                                        p_bo = true;
                                                    }
                                                }
                                                ps_item = TravelTimeTableConverter.GetValue(depth_int, x);
                                                var s = ps_item.Item2;
                                                if (s >= dis)
                                                {
                                                    s_seconds = x;
                                                    break;
                                                }
                                            }
                                        }));

                                        thread.Start();
                                    }

                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Quake_Dict[result_eew.Data.ReportId.ToString()].Add(result_eew.Data.ReportNum.Value.ToString());
                                        EEW_Status.Text = "緊急地震速報が発表されました";
                                        if (result_eew.Data.AlertFlag.ToString() == "予報")
                                        {
                                            EEW_Level.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                                        }
                                        else if (result_eew.Data.AlertFlag.ToString() == "警報")
                                        {
                                            EEW_Level.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                                        }
                                        EEW_Level.Text = result_eew.Data.AlertFlag.ToString();
                                        mag.Text = "M" + result_eew.Data.Magunitude.Value.ToString();
                                        region_point.Text = result_eew.Data.RegionName.ToString();
                                        s_progressbar.Minimum = 0;
                                        s_progressbar.Maximum = s_seconds;

                                        if (result_eew.Data.IsFinal == true)
                                        {
                                            report_num.Text = "最終報";
                                        }
                                        else
                                        {
                                            report_num.Text = "#" + result_eew.Data.ReportNum.Value.ToString();
                                        }
                                        double intensity_dou = CalculateIntensity(depth_int, result_eew.Data.Magunitude.Value, dis);
                                        if (intensity_dou < 0.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_0.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 0.5 && intensity_dou < 1.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_1.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 1.5 && intensity_dou < 2.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_2.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 2.5 && intensity_dou < 3.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_3.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 3.5 && intensity_dou < 4.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_4.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 4.5 && intensity_dou < 5.0)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_5_1.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 5.0 && intensity_dou < 5.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_5_2.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 5.5 && intensity_dou < 6.0)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_6_1.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 6.0 && intensity_dou < 6.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_6_2.png", UriKind.Relative));
                                        }
                                        else if (intensity_dou >= 6.5)
                                        {
                                            max_intensity.Source = new BitmapImage(new Uri("Resources/s_7.png", UriKind.Relative));
                                        }
                                    }));
                                }
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    if (p_seconds == 999)
                                    {
                                        p_second.Text = "計算中";
                                    }
                                    else if ((p_seconds - mai_second) < 0)
                                    {
                                        p_second.Text = "0秒";
                                    }
                                    else
                                    {
                                        p_second.Text = Math.Round((p_seconds - mai_second),2) + "秒";
                                    }

                                    if (s_seconds == 999)
                                    {
                                        s_second.Text = "計算中";
                                    }
                                    else if ((s_seconds - mai_second) < 0)
                                    {
                                        s_progressbar.Value = s_seconds;
                                        s_second.Text = "0秒";
                                    }
                                    else
                                    {
                                        s_progressbar.Value = mai_second;
                                        s_second.Text = Math.Round((s_seconds - mai_second),2) + "秒";
                                    }
                                }));
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                EEW_Status.Text = "緊急地震速報は発表されていません";
                                EEW_Level.Text = "--";
                                report_num.Text = "#--";
                                region_point.Text = "---------------";
                                mag.Text = "M-.-";
                                p_second.Text = "--秒";
                                s_second.Text = "--秒";
                                s_progressbar.Value = 0;
                                max_intensity.Source = null;
                            }));
                        }
                    }
                });
                await Task.Delay(16);
            }
        }

        public static double CalculateDistance(double lat1, double lon1, float lat2, float lon2)
        {
            double radius = 6371;

            double dLat = (lat2 - lat1) * Math.PI / 180f;

            double dLon = (lon2 - lon1) * Math.PI / 180f;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180f) * Math.Cos(lat2 * Math.PI / 180f) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return radius * c;
        }

        private class TravelTimeTableConverter
        {
            public static void ImportData()
            {
                var file = Properties.Resources.tjma2001;
                _table = Regex.Replace(file, "\x20+", " ")
                  .Trim()
                  .Replace("\r", "")
                  .Split('\n')
                  .Select(x => x.Split(' '))
                  .Select(x =>
                      new TravelTimeTable
                      {
                          P = double.Parse(x[1]),
                          S = double.Parse(x[3]),
                          Depth = int.Parse(x[4]),
                          Distance = int.Parse(x[5])
                      })
                  .ToArray();
            }

            public static (double, double) GetValue(int depth, int time)
            {
                if (depth > 700 || time > 2000) return (double.NaN, double.NaN);

                var values = _table.Where(x => x.Depth == depth).ToArray();
                if (values.Length == 0) return (double.NaN, double.NaN);

                var p1 = values.LastOrDefault(x => x.P <= time);
                var p2 = values.FirstOrDefault(x => x.P >= time);
                if (p1 == null || p2 == null) return (double.NaN, double.NaN);
                var p = (time - p1.P) / (p2.P - p1.P) * (p2.Distance - p1.Distance) + p1.Distance;

                var s1 = values.LastOrDefault(x => x.S <= time);
                var s2 = values.FirstOrDefault(x => x.S >= time);
                if (s1 == null || s2 == null) return (p, double.NaN);
                var s = (time - s1.S) / (s2.S - s1.S) * (s2.Distance - s1.Distance) + s1.Distance;
                return (p, s);
            }

            private static TravelTimeTable[] _table;

            private class TravelTimeTable
            {
                public double P { get; set; }
                public double S { get; set; }
                public int Depth { get; set; }
                public int Distance { get; set; }
            }
        }

        public static double CalculateIntensity(int depth, float magJMA, double dis)
        {
            double magW = magJMA - 0.171;

            double hypocenterDistance = Math.Sqrt(depth * depth + dis * dis) - Math.Pow(10, (0.5 * magW - 1.85)) / 2;

            hypocenterDistance = Math.Max(hypocenterDistance, 3);

            double gpv600 = Math.Pow(10, (0.58 * magW + 0.0038 * depth - 1.29 - Math.Log10(hypocenterDistance + 0.0028 * Math.Pow(10, (0.5 * magW))) - 0.002 * hypocenterDistance));

            double pgv400 = gpv600 * 1.31;

            double intensity = 2.68 + 1.72 * Math.Log10(pgv400);

            return intensity;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Disp();
        }
    }
}

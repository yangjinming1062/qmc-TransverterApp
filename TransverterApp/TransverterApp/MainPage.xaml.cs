using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace TransverterApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> fileList { get; set; } = new ObservableCollection<string>();

        private string dirpath = Path.Combine(Environment.GetEnvironmentVariable("ANDROID_STORAGE"), "emulated", "0", "qqmusic", "song");
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void ReadFiles(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            Switch mqms = ((Content as Grid).Children[2] as StackLayout).Children[0] as Switch;
            if (bt.Text == "读取")
            {
                fileList.Clear();
                if (Directory.Exists(dirpath))
                {
                    try
                    {
                        foreach (string f in Directory.EnumerateFiles(dirpath))
                        {
                            var qmcRe = Regex.Match(Path.GetExtension(f), @"qmc");
                            if (mqms.IsToggled)
                            {
                                var re = Regex.Match(f, @"\s\[\S{4,5}\]");
                                if (re.Success)
                                {
                                    File.Move(f, f.Replace(re.Value, ""));
                                    if (qmcRe.Success)
                                        fileList.Add(Path.GetFileName(f.Replace(re.Value, "")));
                                }
                                else
                                {
                                    if (qmcRe.Success)
                                        fileList.Add(Path.GetFileName(f));
                                }
                            }
                            else
                            {
                                if (qmcRe.Success)
                                    fileList.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch
                    {
                        await DisplayAlert("警告", "请先赋予读写存储空间的权限，否则无法读写文件", "知道了");
                        return;
                    }
                }
                bt.Text = "解码";
            }
            else
            {
                Switch saveQmc = ((Content as Grid).Children[1] as StackLayout).Children[0] as Switch;
                if (bt.Text != "解码中...")
                {
                    bt.Text = "解码中...";
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                Decoder decoder = new Decoder();
                                string ext = Path.GetExtension(fileList[i]);
                                string outfile = Path.Combine(dirpath, fileList[i].Replace(ext, ext.Contains("flac") ? ".flac" : ".mp3"));
                                decoder.Convert(Path.Combine(dirpath, fileList[i]), outfile);
                                if (!saveQmc.IsToggled)
                                {
                                    File.Delete(Path.Combine(dirpath, fileList[i]));
                                }
                                fileList[i] = "[解码完成]" + fileList[i];
                            }
                            bt.Text = "读取";
                        }
                        catch (Exception)
                        {

                        }
                    });
                }
            }
        }
    }
}

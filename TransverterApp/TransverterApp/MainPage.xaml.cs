using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Xamarin.Forms.PlatformConfiguration;

namespace TransverterApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> fileList { get; set; } = new ObservableCollection<string>();
        string dirpath = Path.Combine(Environment.GetEnvironmentVariable("ANDROID_STORAGE"), "emulated", "0", "qqmusic", "song");
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void ReadFiles(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            ListView ls = (this.Content as Grid).Children[1] as ListView;
            if (bt.Text == "读取")
            {
                fileList.Clear();
                if (Directory.Exists(dirpath))
                {
                    try
                    {
                        foreach (var f in Directory.EnumerateFiles(dirpath))
                        {
                            if (Regex.Match(Path.GetExtension(f), @"qmc").Success)
                                fileList.Add(Path.GetFileName(f));
                        }
                    }
                    catch
                    {
                        await DisplayAlert("警告", "请先赋予读写存储空间的权限，否则无法读写文件", "知道了");
                    }
                }
                bt.Text = "解码";
            }
            else
            {
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
                                fileList[i] = "[解码完成]" + fileList[i];
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }).Wait();

                    bt.Text = "读取";
                }
            }
        }
    }
}

using System;
using Xamarin.Essentials;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ObservableCollection<Label> fileList = new ObservableCollection<Label>();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void ReadFiles(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Text == "读取")
            {
                fileList.Clear();
                string basepath = Environment.GetEnvironmentVariable("ANDROID_STORAGE");
                string dirpath = Path.Combine(basepath, "emulated", "0", "qqmusic", "song");
                if(true)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dirpath);
                    foreach (var f in dirInfo.GetFiles())
                    {
                        Label lb = new Label();
                        lb.Text = f.Name;
                        fileList.Add(lb);
                    }
                }
                bt.Text = "转换";
            }
            else
            {
                bt.Text = "读取";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using JecPizza.Infostucture.Extensions;
using JecPizza.Models;
using JecPizzaTestUI;

namespace JecPizza.Views.Dialogs
{

    public partial class EmailTemplateDialog : UserControl
    {

        public Member CurrentMember { get; set; }
        public DirectoryInfo TemplatesDir { get; set; }
        private const string TemplatesDirectory = @"C:\Study\School\JecPizza\JecPizza\Content\MailTemplates\";

        public ObservableCollection<string> SelectedPropertiesCollection { get; set; }
        public ObservableCollection<string> PropertiesCollection { get; set; }

        public EmailTemplateDialog()
        {
            InitializeComponent();

            SelectedPropertiesCollection = new ObservableCollection<string>();
            PropertiesCollection = EmailTemplateBulderHelper.GetProperties(typeof(Member)).ToObservableCollection();
            CurrentMember = new Member()
            {
                MemberId = "2012541515",
                UserName = "Fallen",
                Image = "noImg",
                Dob = DateTime.Now,
                Address = "千葉県松戸殿平賀390ハイツフレグランスB２０３",
                Email = "20jn0102@gmail.com",
                Password = "lalaal",
                Sex = true,
                RegisterDate = DateTime.Now,
                PhoneNumber = "080-4441-0581"
            };

            TemplatesDir = new DirectoryInfo(TemplatesDirectory);


            cb.ItemsSource = PropertiesCollection;
            lv.ItemsSource = TemplatesDir.EnumerateFiles().Select(p => p.Name).Where(el => !el.EndsWith(".json")).ToObservableCollection();
            lb.ItemsSource = SelectedPropertiesCollection;
        }


        private void TemplateChanged(object Sender, SelectionChangedEventArgs E)
        {
            FileStream file;
            string filePath = TemplatesDirectory + lv.SelectedItem;
            txb.Text = string.Empty;

            if (!File.Exists(filePath)) return;


            file = new FileStream(filePath, FileMode.Open);
            using var sr = new StreamReader(file);


            while (!sr.EndOfStream)
            {
                txb.Text += sr.ReadLine() + "\n";
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PropertiesCollection.Count == 0 || SelectedPropertiesCollection.Contains(cb.SelectedItem.ToString())) return;

            SelectedPropertiesCollection.Add(cb.SelectedItem.ToString());
            PropertiesCollection.Remove(cb.SelectedItem.ToString());
            cb.SelectedIndex = 0;

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lb.SelectedItem == null) return;

            PropertiesCollection.AddFirst(lb.SelectedItem.ToString());
            SelectedPropertiesCollection.Remove(lb.SelectedItem.ToString());

        }

        private void Txb_OnTextChanged(object sender, TextChangedEventArgs E) => btn_startPrew.IsEnabled = !string.IsNullOrEmpty(txb.Text);

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SelectedPropertiesCollection.Count == 0) return;

            string filePath = TemplatesDirectory + lv.SelectedItem;
            IDictionary<int, string> templData = new Dictionary<int, string>();
            var tempList = new HashSet<int>();
            using var file = new FileStream(filePath, FileMode.Open);
            using var sr = new StreamReader(file);


            while (!sr.EndOfStream)
            {
                string[] fields = sr.ReadLine()?.Split("[");

                if (fields?.Length <= 1) continue;

                tempList.Add(int.Parse(fields?[1].Remove(fields[1].IndexOf(']')) ?? "0"));
            }

            if (tempList.Count != SelectedPropertiesCollection.Count)
            {
                MessageBox.Show("Not Correct format!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }



            foreach (int i in tempList)
                templData.Add(i, EmailTemplateBulderHelper.GetPropValue(CurrentMember, SelectedPropertiesCollection[i - 1].ToString())?.ToString() ?? "No Date");

            foreach (KeyValuePair<int, string> data in templData)
                txb.Text = txb.Text.Replace("[" + data.Key + "]", data.Value);

        }

        private void OnSaveASClicked(object Sender, RoutedEventArgs E)
        {

            if (SelectedPropertiesCollection.Count > 0)
            {
                string data = $"{lv.SelectedItem}:[{string.Join(",", SelectedPropertiesCollection)}]";
                var res = EmailTemplateBulderHelper.SaveNewTemplateAsJson(App.GetActiveWindow, data);

                MessageBox.Show(res ? "File Saved" : "Saving is failed");
            }

        }

      

        private void UpdCollBtn_OnClick(object Sender, RoutedEventArgs E) => tempsLv.ItemsSource = TemplatesDir.EnumerateFiles().Select(p => p.Name).Where(el => el.EndsWith(".json")).ToObservableCollection();

        private void OnTemplateChanged(object sender, SelectionChangedEventArgs E)
        {

            FileStream file;


            string fpath = TemplatesDirectory + tempsLv.SelectedItem;

            if (!File.Exists(fpath)) return;

            file = new FileStream(fpath, FileMode.Open);
            using var fr = new StreamReader(file, Encoding.Unicode);

            string tempData = fr.ReadLine();

            string[] data = tempData?.ToString()?.Split(":");

            if (data == null || data.Length <= 1) return;

            string filePath = TemplatesDirectory + data[0];
            txb.Text = string.Empty;

            if (!File.Exists(filePath)) return;

            List<string> prop = new List<string>(data[1].Replace("[", "").Replace("]", "").Split(',').Select(val => EmailTemplateBulderHelper.GetPropValue(CurrentMember, val)?.ToString() ?? "NoDate"));

            if (prop.Count == 0) return;


            file = new FileStream(filePath, FileMode.Open);
            using var sr = new StreamReader(file);
            string fdata = string.Empty;

            while (!sr.EndOfStream)
            {
                fdata += sr.ReadLine() + "\n";
            }

            for (var i = 1; i <= prop.Count; i++)
                fdata = fdata.Replace("[" + i + "]", prop[i - 1]);

            txb.Text = fdata;

        }

        private void OnTemplateRemove(object Sender, RoutedEventArgs E)
        {
            if (tempsLv.SelectedItem == null) return;

            string filePath = TemplatesDirectory + tempsLv.SelectedItem;

            if (!File.Exists(filePath)) return;

            var res = MessageBox.Show($"{tempsLv.SelectedItem}テンプレート削除でよろしいですか？", "Templator", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Cancel) return;

            File.Delete(filePath);

            MessageBox.Show("削除完了！", "Templator");
            tempsLv.ItemsSource = TemplatesDir.EnumerateFiles().Select(p => p.Name).Where(el => el.EndsWith(".json")).ToObservableCollection();
        }

        private void OnNewClicked(object Sender, RoutedEventArgs E) => tb.SelectedIndex = 0;
    }
}

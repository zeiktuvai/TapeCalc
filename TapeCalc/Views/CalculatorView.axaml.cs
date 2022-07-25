using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using TapeCalc.Models;
using System;

namespace TapeCalc.Views
{
    public partial class CalculatorView : UserControl
    {
        public CalculatorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);            
        }
        public void ClearTextEditor(object sender, RoutedEventArgs e)
        {
            this.FindControl<TextEditor>("editor").Text = "";            
        }
        public async void SaveTextEditor(object sender, RoutedEventArgs e)
        {
            if (Globals.DocumentLoaded == false)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExtension = "*.tape";
                dialog.Title = "Save calculator collection";
                dialog.Filters = new List<FileDialogFilter> {
                    new FileDialogFilter() { Extensions = new List<string>() { "tape" }, Name = "TapeCalc files" }
                    };
                Window parent = (Window)Parent.Parent.Parent;
                string result = await dialog.ShowAsync(parent);

                if (result != null)
                {
                    var json = JsonConvert.SerializeObject(Helpers.TextProcessor.ConvertTextEditorText(this.FindControl<TextEditor>("editor").Document));
                    File.WriteAllText(result, json);
                    Globals.DocumentLoaded = true;
                    Globals.DocumentPath = result;
                }
            } else
            {
                var json = JsonConvert.SerializeObject(Helpers.TextProcessor.ConvertTextEditorText(this.FindControl<TextEditor>("editor").Document));

                File.WriteAllText(Globals.DocumentPath, json);
            }
        }

        public async void SaveAsTextEditor(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExtension = "*.tape";
            dialog.Title = "Save calculator collection";
            dialog.Filters = new List<FileDialogFilter> {
                    new FileDialogFilter() { Extensions = new List<string>() { "tape" }, Name = "TapeCalc files" }
                    };
            Window parent = (Window)Parent.Parent.Parent;
            string result = await dialog.ShowAsync(parent);

            if (result != null)
            {
                var json = JsonConvert.SerializeObject(Helpers.TextProcessor.ConvertTextEditorText(this.FindControl<TextEditor>("editor").Document));
                File.WriteAllText(result, json);
                Globals.DocumentLoaded = true;
                Globals.DocumentPath = result;
            }
        }

        public async void OpenTextEditor(object sender, RoutedEventArgs e)
        {
            List<CalculatorLineItemModel> items = new List<CalculatorLineItemModel>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.AllowMultiple = false;
            dialog.Title = "Open calculator collection";
            dialog.Filters = new List<FileDialogFilter> {
                new FileDialogFilter() { Extensions = new List<string>() { "tape" }, Name = "TapeCalc files" }
            };
            dialog.Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Window parent = (Window)Parent.Parent.Parent;
            var result = await dialog.ShowAsync(parent);
            

            if (result != null)
            {
                using (StreamReader file = new StreamReader(result[0]))
                {
                    Globals.DocumentLoaded = true;
                    Globals.DocumentPath = result[0];
                    string read_json = await file.ReadToEndAsync();
                    items = JsonConvert.DeserializeObject<List<CalculatorLineItemModel>>(read_json);
                }
            
                this.FindControl<TextEditor>("editor").Text = Helpers.TextProcessor.ConvertTextEditorText(items);
            }

        }
    }
}

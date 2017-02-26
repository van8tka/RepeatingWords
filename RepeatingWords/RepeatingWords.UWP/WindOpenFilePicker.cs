using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Xamarin.Forms;

[assembly:Dependency(typeof(RepeatingWords.UWP.WindOpenFilePicker))]

namespace RepeatingWords.UWP
{
    public class WindOpenFilePicker : IWindOpenFilePicker
    {
        public async Task<List<string>> LoadTextFrWindowsAsync()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            //отображение файлов в виде списка
            openPicker.ViewMode = PickerViewMode.List;
            //точка входа 
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            //кнопка по умолчанию
            openPicker.CommitButtonText = "Open";
            //фильтр отображаемых файлов
            openPicker.FileTypeFilter.Add(".txt");
            //получаем файл
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    List<string> lines = new List<string>();
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lines.Add(line);
                    }
                    return lines;
                }

            }
            else
                return null;
        }
    }
 }

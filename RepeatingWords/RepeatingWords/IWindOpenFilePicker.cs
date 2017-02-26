using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepeatingWords
{
   public interface IWindOpenFilePicker
    {
        Task<List<string>> LoadTextFrWindowsAsync();//загр текста из файла
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RepeatingWords
{
  public interface IFolderWorker
    {
        string GetRootPath();
        Task<List<string>> GetFoldersAsync(string path = null);
    }
}

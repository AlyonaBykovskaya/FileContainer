using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileContainer
{
    [Serializable()]
    public class FileEntry
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public List<FileEntry> fileContainer = new List<FileEntry>();

   
    }
   
}

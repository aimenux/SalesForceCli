using System.Collections.Generic;

namespace Lib.Services
{
    public class SalesForceResults<T>
    {
        public string InputFile { get; set; }

        public string OutputFile { get; set; }

        public ICollection<T> Records { get; set; }
    }
}
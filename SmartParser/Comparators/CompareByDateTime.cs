using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParser.Comparators
{
    public class FileInfo_Compartors 
    {
        public class CompareByCreationTime : IComparer<FileInfo>
        {
            public int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null)
                {
                    throw new NullReferenceException("Function arguments x and y were null.");
                }

                return x.CreationTime.CompareTo(y.CreationTime);
            }
        }

        public class CompareByLastAccessTime : IComparer<FileInfo>
        {
            public int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null)
                {
                    throw new NullReferenceException("Function arguments x and y were null.");
                }

                return x.LastAccessTime.CompareTo(y.LastAccessTime);
            }
        }

        public class CompareByLastWriteTime : IComparer<FileInfo>
        {
            public int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null)
                {
                    throw new NullReferenceException("Function arguments x and y were null.");
                }

                return x.LastWriteTime.CompareTo(y.LastWriteTime);
            }
        }

        public class CompareByName : IComparer<FileInfo>
        {
            public int Compare(FileInfo? x, FileInfo? y)
            {
                if (x == null || y == null)
                {
                    throw new NullReferenceException("Function arguments x and y were null.");
                }

                return x.Name.CompareTo(y.Name);
            }
        }

    }
}

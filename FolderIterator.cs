using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Security.Cryptography;

namespace foldercompare
{
    internal class FolderIterator
    {
        private string _source;
        private string _destination;

        public FolderIterator(string source, string destination)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source should be specified.", nameof(source));
            if (string.IsNullOrEmpty(destination))
                throw new ArgumentException("Destination should be specified.", nameof(destination));

            this._source = source;
            this._destination = destination;
        }

        public void Compare()
        {
            CompareFolderContent(_source);
            Console.WriteLine("Done.");
        }

        private void CompareFolderContent(string directory) {
            var files = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
            foreach(var fileA in files)
            {
                var relativeFile = fileA.Replace(_source, string.Empty).TrimStart('\\', '/');
                var fileB = Path.Combine(this._destination, relativeFile);

                if (!File.Exists(fileB)) {
                    Console.WriteLine($"DoesNotExists: {fileB}");
                    continue;
                }

                try
                {
                    var hash1 = GetFileHash(fileA);
                    var hash2 = GetFileHash(fileB);
                    if (!hash1.SequenceEqual(hash2))
                    {
                        Console.WriteLine($"DoesNotMatch: {fileB}");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            var folders = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach(var folder in folders)
            {
                CompareFolderContent(folder);
            }
        }

        private byte[] GetFileHash(string path) {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    return sha1.ComputeHash(bs);
                }
            }
        }
    }
}
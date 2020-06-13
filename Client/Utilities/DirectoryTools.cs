using System;
using System.IO;

namespace Client.Utilities {
    public static class DirectoryTools {
        public static DirectoryInfo CreateDir(string path, string dirName) {
            if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(dirName)) return null;
            string directory = Path.Combine(path, dirName);
            return Directory.CreateDirectory(directory);
        }

        public static void RecursiveDelete(string folderpath) {
            DirectoryInfo baseDir = new DirectoryInfo(folderpath);
            if (!baseDir.Exists) return;
            foreach (var dir in baseDir.EnumerateDirectories()) {
                RecursiveDelete(dir.FullName);
            }
            baseDir.Delete(true);
        }
    }
}

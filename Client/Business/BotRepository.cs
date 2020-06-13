using Client.Models;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Client.Business {
    /// <summary>
    /// This class manages a bot repository.
    /// The bots will be saved using serialization.
    /// </summary>
    class BotRepository : IBotRepository {
        private IList<Bot> _botStore;
        private readonly DirectoryInfo _botsFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotRepository"/> class.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        public BotRepository(string folderName) {

            _botsFolder = DirectoryTools.CreateDir(AppDomain.CurrentDomain.BaseDirectory, folderName);

            //var ext = new List<string> { "jpg", "gif", "png" };
            //var myFiles = Directory
            //    .EnumerateFiles(dir.FullName, "*.*", SearchOption.AllDirectories)
            //    .Where(s => ext.Contains(Path.GetExtension(s).ToLowerInvariant()));


        }

        /// <summary>
        /// Saves the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public void Save(Bot bot) {
            if (!_botStore.Contains(bot))
                _botStore.Add(bot);

            Serialize(bot);
        }

        /// <summary>
        /// Saves all bots in the repository.
        /// </summary>
        public void SaveAll() {
            foreach (var item in _botStore) {
                Serialize(item);
            }            
        }

        /// <summary>
        /// Deletes the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public void Delete(Bot bot) {
            _botStore.Remove(bot);
        }

        /// <summary>
        /// Resets the repository.
        /// </summary>
        public void RepositoryReset() {
            //foreach (var item in _botsFolder.GetFiles("*.talos")) {
            //    File.Delete(item.FullName);
            //}
            SaveAll();
            _botStore = new List<Bot>();
        }

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Bots</returns>
        public IList<Bot> FindAll() {
            foreach (var item in _botsFolder.GetFiles("*.talos")) {
                Deserialize(item);
            }
            return _botStore;
        }

        /// <summary>
        /// Returns the specific bots from te repository.
        /// </summary>
        /// <returns>A List with specific Bots</returns>
        public IList<Bot> FindSelect(IList<string> fileNameList) {
            foreach (var item in _botsFolder.GetFiles("*.talos")) {

                foreach (var filename in fileNameList) {
                    Debug.WriteLine($"{filename} and {item.Name}");
                }

                if (!fileNameList.Contains(item.Name)) break;
                Debug.WriteLine("WE ARE HERE");
                Deserialize(item);
            }

            if (_botStore == null) {
                _botStore = new List<Bot>();
            }

            return _botStore;
        }

        /// <summary>
        /// Serializes a bots to a file.
        /// </summary>
        private void Serialize(Bot bot) {
            if (bot == null) return;

            using (var stream = File.Open(Path.Combine(_botsFolder.FullName,Path.GetFileNameWithoutExtension(bot.FilePath) + ".talos"), FileMode.OpenOrCreate)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, bot);
            }
        }

        /// <summary>
        /// Deserializes a bot or creates an empty List.
        /// </summary>
        private void Deserialize(FileInfo file) {
            if (file == null) return;

            Bot botFromFile;
            using (var stream = File.Open(file.FullName, FileMode.Open)) {
                var formatter = new BinaryFormatter();
                botFromFile = (Bot)formatter.Deserialize(stream);
            }

            if (_botStore == null) {
                _botStore = new List<Bot>();
            }

            _botStore.Add(botFromFile);
        }
    }
}

using Client.Models;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Controls.Primitives;

namespace Client.Business {
    /// <summary>
    /// This class manages a bot repository.
    /// The bots will be saved using serialization.
    /// </summary>
    class BotRepository : IBotRepository {
        private IList<Bot> _botStore;
        private readonly string _dataFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotRepository"/> class.
        /// </summary>
        /// <param name="folderName">The folder name.</param>
        public BotRepository(string fileName) {
            fileName = string.Concat(fileName + ".talon");
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Deserialize();
        }

        /// <summary>
        /// Saves the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public void Save(Bot bot) {
            if (!_botStore.Contains(bot)) _botStore.Add(bot);
            Serialize();
        }

        /// <summary>
        /// Deletes the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        public void Delete(Bot bot) {
            _botStore.Remove(bot);
            Serialize();
        }

        /// <summary>
        /// Resets the repository.
        /// </summary>
        public void RepositoryReset() {
            File.Delete(_dataFile);
            _botStore = new List<Bot>();
        }

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Bots</returns>
        public IList<Bot> FindAll() {
            return _botStore;
        }

        /// <summary>
        /// Serializes a bots to a file.
        /// </summary>
        private void Serialize() {
            using (var stream = File.Open(_dataFile, FileMode.OpenOrCreate)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _botStore);
            }
        }

        /// <summary>
        /// Deserializes all notes or creates an empty List.
        /// </summary>
        private void Deserialize() {
            if (File.Exists(_dataFile))
                using (var stream = File.Open(_dataFile, FileMode.Open)) {
                    var formatter = new BinaryFormatter();
                    _botStore = (IList<Bot>)formatter.Deserialize(stream);
                }
            else
                _botStore = new List<Bot>();
        }
    }
}

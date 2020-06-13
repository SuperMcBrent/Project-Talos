using Client.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Client.Business {
    /// <summary>
    /// This class manages a profile repository.
    /// The profiles will be saved using serialization.
    /// </summary>
    class ProfileRepository : IProfileRepository {
        private IList<Profile> _profileStore;
        private readonly string _dataFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileRepository"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public ProfileRepository(string fileName) {
            fileName = string.Concat(fileName + ".talos");
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            Deserialize();
        }

        /// <summary>
        /// Looking a profile by ID
        /// </summary>
        /// <param name="id">The profile id.</param>
        /// <returns>A profile or null</returns>
        public Profile GetById(Guid id) {
            return _profileStore.SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Replace the old profiles list with new ones.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        public void SaveAll(IList<Profile> profiles) {
            _profileStore = profiles;

            Serialize();
        }

        /// <summary>
        /// Deletes the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Delete(Profile profile) {
            _profileStore.Remove(profile);
        }

        /// <summary>
        /// Resets the repository.
        /// </summary>
        public void RepositorySoftReset() {
            _profileStore = new List<Profile>();
        }

        /// <summary>
        /// Resets the repository. Deletes datafile.
        /// </summary>
        public void RepositoryHardReset() {
            Debug.WriteLine("File = GONE");
            File.Delete(_dataFile);
            _profileStore = new List<Profile>();
        }


        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Profiles</returns>
        public IList<Profile> FindAll() {
            Deserialize();
            return _profileStore;
        }

        /// <summary>
        /// Serializes all the profiles to a file.
        /// </summary>
        private void Serialize() {
            Debug.WriteLine("CRITICAL MOMENT SAVE PROFILES TO FILE");
            using (var stream = File.Open(_dataFile, FileMode.OpenOrCreate)) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, _profileStore);
            }
        }

        /// <summary>
        /// Deserializes all profiles or creates an empty List.
        /// </summary>
        private void Deserialize() {
            if (File.Exists(_dataFile)) {
                Debug.WriteLine("File already there");
                using (var stream = File.Open(_dataFile, FileMode.Open)) {
                    var formatter = new BinaryFormatter();
                    _profileStore = (IList<Profile>)formatter.Deserialize(stream);
                }
            } else {
                Debug.WriteLine("File not found");
                _profileStore = new List<Profile>();
                // add default
                _profileStore.Add(
                    new Profile("DefaultUserName", "DefaultProfile") {
                        IsDefault = true,
                        IsActive = true
                    }
                );
                Serialize();
            }
        }
    }
}

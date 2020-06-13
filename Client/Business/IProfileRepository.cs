using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Business {
    public interface IProfileRepository {
        /// <summary>
        /// Replace the old profiles list with a new one.
        /// </summary>
        /// <param name="profiles">The categories.</param>
        void SaveAll(IList<Profile> profiles);

        /// <summary>
        /// Deletes the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        void Delete(Profile profile);

        /// <summary>
        /// Resets the repository.
        /// </summary>
        void RepositorySoftReset();

        /// <summary>
        /// Resets the repository. Deletes the datafile.
        /// </summary>
        void RepositoryHardReset();

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Profiles</returns>
        IList<Profile> FindAll();
    }
}

using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Business {
    public interface IBotRepository {
        /// <summary>
        /// Saves the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        void Save(Bot bot);

        /// <summary>
        /// Deletes the specified bot.
        /// </summary>
        /// <param name="bot">The bot.</param>
        void Delete(Bot bot);

        /// <summary>
        /// Resets the repository.
        /// </summary>
        void RepositoryReset();

        /// <summary>
        /// Returns the entire repository.
        /// </summary>
        /// <returns>A List with all Bots</returns>
        IList<Bot> FindAll();

        /// <summary>
        /// Returns the specific bots from te repository.
        /// </summary>
        /// <returns>A List with specific Bots</returns>
        IList<Bot> FindSelect(IList<string> pathList);
    }
}

using System;
using ChatRoom.Interface.IRepository.Message;
using ChatRoom.Interface.IRepository.User;

namespace ChatRoom.Interface
{
    /// <summary>
    /// Abtraction layer between functional and database context
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets<see cref="Users"/> context
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Gets<see cref="Messages"/> context
        /// </summary>
        IMessageRepository Messages { get; }

        int Complete();
    }
}

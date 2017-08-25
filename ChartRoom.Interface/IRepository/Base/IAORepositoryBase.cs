using System.Collections.Generic;
using ChatRoom.Common.CommonModel;

namespace ChatRoom.Interface.IRepository.Base
{
    public interface IAORepositoryBase<TEntity> where TEntity : Entity.Base.EnitityBase
    {
        ResultWrapper Insert(TEntity t, string tableName = null, bool identity = false);

        ResultWrapper Update(TEntity t, string tableName = null);

        ResultWrapper Delete(TEntity t, string tableName = null);

        IList<TEntity> Get(TEntity t = default(TEntity), bool page = false, string tableName = null, string chooseFiled = "*");

        int GetTotal(TEntity t, string tableName = null);
        TEntity GetDataById(int id, string tableName, string chooseFiled);
    }
}

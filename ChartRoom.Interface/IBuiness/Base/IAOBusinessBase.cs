using System.Collections.Generic;
using ChatRoom.Common.CommonModel;

namespace ChatRoom.Interface.IBuiness.Base
{
    public interface IAOBusinessBase<TModel> where TModel:class
    {
        ResultWrapper Insert(TModel t, TModel old = null, string tableName = null, bool identity = false);

        ResultWrapper Update(TModel t, TModel old = null, string tableName = null);

        ResultWrapper Delete(TModel t, string tableName = null);

        IList<TModel> Get(TModel t = default(TModel), bool page = false, string tableName = null, string chooseFiled = "*");
        TModel GetDataById(int id, string tableName = null, string chooseFiled = "*");

        int GetTotal(TModel t, string tableName = null);

        bool Exists(TModel t, string tableName = null);
    }
}
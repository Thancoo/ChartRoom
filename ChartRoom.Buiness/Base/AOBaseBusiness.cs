using System;
using System.Collections.Generic;
using System.Linq;
using ChatRoom.Common.Attribute;
using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IBuiness.Base;
using ChatRoom.Interface.IRepository.Base;
using ChatRoom.Repository.Base;
using E = ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Buiness.Base
{
    public abstract class AOBaseBusiness<TModel,TEntity>:IAOBusinessBase<TModel> where TModel:M.Base.ModelBase where TEntity:E.Base.EnitityBase
    {
        private readonly IAORepositoryBase<TEntity> repositoryBase;

        protected AOBaseBusiness()
        {
            this.repositoryBase = new AoBaseRepository<TEntity>();
        }
        public abstract TModel EntityToModel(TEntity data);
        public abstract TEntity ModelToEntity(TModel data);
        public ResultWrapper Insert(TModel t, TModel old = null, string tableName = null, bool identity = false)
        {
            if (old != null && this.Exists(old))
            {
                return new ResultWrapper
                {
                    State = false
                };
            }
            return this.repositoryBase.Insert(ModelToEntity(t), tableName:tableName,identity:identity);
        }

        public ResultWrapper Update(TModel t, TModel old = null, string tableName = null)
        {
            if (old != null && this.ExistsUpdate(old))
            {
                return new ResultWrapper
                {
                    State = false
                };
            }

            return this.repositoryBase.Update(ModelToEntity(t), tableName:tableName);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <returns>
        /// The <see cref="ResultWrapper"/>.
        /// </returns>
        public ResultWrapper Delete(TModel t, string tableName = null)
        {
            return this.repositoryBase.Delete(ModelToEntity(t), tableName);
        }

        public IList<TModel> Get(TModel t = default(TModel), bool page = false, string tableName = null, string chooseFiled = "*")
        {
            return this.repositoryBase.Get(ModelToEntity(t), page, tableName, chooseFiled).Select(EntityToModel).ToList();
        }

        public TModel GetDataById(int id, string tableName = null, string chooseFiled = "*")
        {
            var res=this.repositoryBase.GetDataById(id, tableName, chooseFiled);
            if (res == null)
                return null;
            return EntityToModel(res);
        }

        public int GetTotal(TModel t, string tableName = null)
        {
            return this.repositoryBase.GetTotal(ModelToEntity(t), tableName);
        }

        public bool Exists(TModel t, string tableName = null)
        {
            return this.repositoryBase.Get(ModelToEntity(t)).ToList().Count > 0;
        }

        public bool ExistsUpdate(TModel t, string tableName = null)
        {
            var idOld = "Id";
            var type = t.GetType();
            if (type.GetProperty(idOld) == null)
            {
                //如果不是，则找Attribute为PrimaryKey的字段
                idOld = type.GetProperties().AsParallel().Where(l => l.GetCustomAttributes(false).Any(z => z is PrimaryKeyAttribute)
            && l.GetCustomAttributes(false).Where(z => z is PrimaryKeyAttribute).Any(kk => (kk as PrimaryKeyAttribute).IsPrimaryKey)).FirstOrDefault()?.Name;
            }
            t.GetType().GetProperty(idOld).SetValue(t, 0);
            var res = this.repositoryBase.Get(ModelToEntity(t));
            if (res.Count == 0)
            {
                return false;
            }

            var count = res.Where(o => Convert.ToInt32(o.GetType().GetProperty("Id").GetValue(o)) == Convert.ToInt32(idOld)).ToList().Count;
            return count <= 0;
        }
    }
}

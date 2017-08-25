using System;
using System.Collections.Generic;
using System.Linq;
using ChatRoom.Common.Attribute;
using Dapper;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.Enum;
using ChatRoom.Common.Utils;
using System.Data.SqlClient;
using ChatRoom.Interface.IRepository.Base;
using MySql.Data.MySqlClient;

namespace ChatRoom.Repository.Base
{
    public class AoBaseRepository<T>: IAORepositoryBase<T> where T:Entity.Base.EnitityBase
    {
        public MySqlConnection Connection => ConfigurationHelper.GetSqlConnection();
        public ResultWrapper Insert(T t, string tableName = null, bool identity = false)
        {
            var type = t.GetType();
            tableName = (tableName ?? type.Name);
            var insert = $@"INSERT INTO `{tableName}` (";
            var values = " VALUES(";
            var props = type.GetProperties().AsParallel().Where(l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
            ||(l.GetCustomAttributes(false).Where(z=>(z is OptionIgnoreAttribute)&& (!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Insert).Equals(RepositoryOption.Insert))).Any())).ToList();
            var @params = new DynamicParameters();
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var propValue = prop.GetValue(t);

                if (propValue == null)
                {
                    continue;
                }

                if (propType.IsValueType && propValue.Equals(Activator.CreateInstance(propType)))
                {
                    continue;
                }

                insert += $"`{prop.Name}`,";
                values += $"@{prop.Name},";
                @params.Add(prop.Name, propValue);
            }

            if (type.GetProperty("Available") != null)
            {
                insert += @"CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,Available)";
                values += @"GetDBDate(),'System',GetDBDate(),'System',1)";
            }
            else {
                insert = insert.Substring(0,insert.Length - 1) + ")";
                values = values.Substring(0,values.Length - 1) + ")";
            }

            var query = insert + values;

            try
            {
                var resultWrapper = new ResultWrapper
                {
                    State = true
                };
                using (var conn = this.Connection)
                {
                    if (identity)
                    {
                        query += ";SELECT @@IDENTITY";
                        resultWrapper.Data= conn.ExecuteScalar<string>(query, @params);
                    }
                    else
                    {
                        conn.ExecuteScalar<string>(query, @params);
                    }
                }
                return resultWrapper;
            }
            catch (Exception exception)
            {
                return new ResultWrapper
                {
                    State = false,
                    Exception = exception,
                    Message  = exception.Message
                };
            }
        }

        public ResultWrapper Update(T t, string tableName = null)
        {
            var type = t.GetType();

            tableName = tableName ?? type.Name;

            var query =type.GetProperty("UpdatedOn")==null?$@"UPDATE `{tableName}` SET UpdatedOn = GetDBDate(),": $"UPDATE `{tableName}` SET ";
            query = type.GetProperty("UpdatedBy") == null ? string.Concat(query, "`UpdatedBy`='System',") : query;

            var props = type.GetProperties().Where(l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
|| (l.GetCustomAttributes(false).Where(z => (z is OptionIgnoreAttribute)&&(!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Update).Equals(RepositoryOption.Update))).Any())).ToList();
            var @params = new DynamicParameters();
            string primarykey = type.GetProperties().AsParallel().Where(l => l.GetCustomAttributes(false).Any(z => z is PrimaryKeyAttribute)
&& l.GetCustomAttributes(false).Where(z => z is PrimaryKeyAttribute).Any(kk => (kk as PrimaryKeyAttribute).IsPrimaryKey)).FirstOrDefault()?.Name;
            if (string.IsNullOrEmpty(primarykey))
            {
                primarykey = "Id";
            }
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var value = prop.GetValue(t);

                if (value == null)
                {
                    continue;
                }

                if (propType != typeof(bool) && propType.IsValueType && value.Equals(Activator.CreateInstance(propType)))
                {
                    continue;
                }

                query += string.Format("`{0}` = @{0},", prop.Name);
                @params.Add(prop.Name, value);
            }

            query = query.Remove(query.Length - 1);
            query += string.Format(" WHERE {0} = @{0} AND `Available`=1", primarykey);
            @params.Add(primarykey, type.GetProperty(primarykey).GetValue(t));

            try
            {
                using (var conn = this.Connection)
                {
                    conn.Execute(query, @params);
                }
                return new ResultWrapper
                {
                    State = true
                };
            }
            catch (Exception exception)
            {
                return new ResultWrapper
                {
                    State = false,
                    Exception = exception
                };
            }
        }

        public ResultWrapper Delete(T t, string tableName = null)
        {
            try
            {
                var type = t.GetType();
                tableName = tableName ?? t.GetType().Name;

                var query = $@"UPDATE `{tableName}` SET UpdatedOn = GetDBDate(),UpdatedBy='System', [Available] = 1 where 1=1 ";
                var props = type.GetProperties().Where(l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
|| (l.GetCustomAttributes(false).Where(z => (z is OptionIgnoreAttribute)&& (!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Delete).Equals(RepositoryOption.Delete))).Any())).ToList();
                var @params = new DynamicParameters();
                var temp = string.Empty;
                foreach (var prop in props)
                {
                    var propType = prop.PropertyType;
                    var value = prop.GetValue(t);

                    if (value == null)
                    {
                        continue;
                    }

                    if (propType.IsValueType && value.Equals(Activator.CreateInstance(propType)))
                    {
                        continue;
                    }

                    temp += string.Format("[{0}] = @{0} and", prop.Name);
                    @params.Add(prop.Name, value);
                }

                var tempStr = temp.Split(new[] { "and" }, StringSplitOptions.RemoveEmptyEntries);

                query += string.Join(" and ", tempStr);
                using (var conn = this.Connection)
                {
                    conn.Execute(query, @params);
                }
                return new ResultWrapper
                {
                    State = true
                };
            }
            catch (Exception exception)
            {
                return new ResultWrapper
                {
                    State = false,
                    Exception = exception
                };
            }
        }

        public IList<T> Get(T t, bool page = false, string tableName = null, string chooseFiled = "*")
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var @params = new DynamicParameters();
            var query = type.GetProperty("Available") != null
                ? string.Format(@"SELECT " + chooseFiled + "FROM `{0}` WHERE 1 = 1", tableName)
                : string.Format(
                    @"SELECT " + chooseFiled + "FROM `{0}` WHERE `Available` = 1",
                    tableName);
                var props = type.GetProperties().AsParallel().Where(
                    l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
                         || ((l.GetCustomAttributes(false)
                             .Where(z => (z is OptionIgnoreAttribute) &&
                                         (!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Select).Equals(
                                             RepositoryOption.Select))).Any()))).ToList();
            foreach (var prop in props)
            {
                var propType = prop.PropertyType;
                var value = prop.GetValue(t);

                if (value == null)
                {
                    continue;
                }

                if (propType.IsValueType && value.Equals(Activator.CreateInstance(propType)))
                {
                    continue;
                }

                query += string.Format(" AND `{0}` = @{0}", prop.Name);

                @params.Add(prop.Name, value);
            }
            using (var conn = this.Connection)
            {
                if (!page)
                    return conn.Query<T>(query, @params).ToList();
                query += $@"ORDER BY UpdatedOn DESC
                                LIMIT @Index,@Rows";
                @params.Add("Index", type.GetProperty("Index").GetValue(t));
                @params.Add("Rows", type.GetProperty("Rows").GetValue(t));
                return conn.Query<T>(query, @params).ToList();
            }
        }

        public int GetTotal(T t, string tableName = null)
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var @params = new DynamicParameters();
            var ignores = new[] { "Page", "Rows", "Available" };
            var query = type.GetProperty("Available") == null
                            ? $@"SELECT COUNT(1) FROM `{tableName}` WHERE 1 = 1"
                            : $@"SELECT COUNT(1) FROM `{tableName}` WHERE `Available` = 1";
            if (t == null)
            {
                using (var conn = this.Connection)
                {
                    return conn.ExecuteScalar<int>(query, @params);
                }
            }
            var props = type.GetProperties().AsParallel().Where(l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
|| (l.GetCustomAttributes(false).Where(z => (z is OptionIgnoreAttribute)&&(!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Select).Equals(RepositoryOption.Select))).Any())).ToList();
            foreach (var prop in props)
            {
                if (ignores.Contains(prop.Name))
                {
                    continue;
                }

                var propType = prop.PropertyType;
                var value = prop.GetValue(t);

                if (value == null)
                {
                    continue;
                }

                if (propType.IsValueType && value.Equals(Activator.CreateInstance(propType)))
                {
                    continue;
                }

                query += string.Format(" AND `{0}` = @{0}", prop.Name);
                @params.Add(prop.Name, value);
            }
            using (var conn = this.Connection)
            {
                return conn.ExecuteScalar<int>(query, @params);
            }
        }

        public IList<T> Get(T t, string order, bool isDesc, bool page = false, string tableName = null, string chooseFiled = "*")
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var @params = new DynamicParameters();
            var ignores = new[] { "Index", "Rows", "Available" };
            var query = type.GetProperty("Available") == null
                            ? string.Format(@"SELECT " + chooseFiled + "FROM `{0}` WHERE 1 = 1", tableName)
                            : string.Format(
                                @"SELECT " + chooseFiled + "FROM `{0}` WHERE `Available` = 1",
                                tableName);
            if (t != null)
            {
                var props = type.GetProperties().AsParallel().Where(l => (!l.GetCustomAttributes(false).Any(z => z is OptionIgnoreAttribute))
|| (l.GetCustomAttributes(false).Where(z => (z is OptionIgnoreAttribute)&& (!((z as OptionIgnoreAttribute).ROption & RepositoryOption.Select).Equals(RepositoryOption.Select))).Any())).ToList();
                foreach (var prop in props)
                {
                    if (ignores.Contains(prop.Name))
                    {
                        continue;
                    }

                    var propType = prop.PropertyType;
                    var value = prop.GetValue(t);

                    if (value == null)
                    {
                        continue;
                    }

                    if (propType.IsValueType && value.Equals(Activator.CreateInstance(propType)))
                    {
                        continue;
                    }

                    query += string.Format(" AND `{0}` = @{0}", prop.Name);

                    @params.Add(prop.Name, value);
                }
            }
            using (var conn = this.Connection)
            {
                if (!page)
                {
                    conn.Query<T>(query, @params);
                }
                query +=
                    $@"ORDER BY {order} {(isDesc ? @"DESC" : string.Empty)}
                                 LIMIT @Index,@Rows";
                @params.Add("Index", type.GetProperty("Index").GetValue(t));
                @params.Add("Rows", type.GetProperty("Rows").GetValue(t));

                return conn.Query<T>(query, @params).ToList();
            }
        }

        public bool Exists(T t, string tableName = null)
        {
            return GetTotal(t, tableName) > 0;
        }

        public T GetDataById(int id, string tableName, string chooseFiled)
        {
            var type = typeof(T);
            tableName = tableName ?? type.Name;
            var sql = "Select * From "+ tableName+" Where Id=@id";
            using (var conn = this.Connection)
            {
                return conn.Query<T>(sql, new{id}).FirstOrDefault();
            }
        }
    }
}

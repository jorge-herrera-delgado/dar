using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Data.Access.Repository.LegacyDataBase.Extension
{
    public static class QueryExtensions
    {
        public static IEnumerable<IDictionary> QueryEx(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = cnn.Query(sql, param, transaction, buffered, commandTimeout, commandType) as IEnumerable<IDictionary<string, object>>;
            return result.Select(r => r.Distinct().ToDictionary(d => d.Key, d => d.Value));
        }

        public static string QueryToJson(this IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var res = cnn.Query(sql, param, transaction, buffered, commandTimeout, commandType);
            return JsonConvert.SerializeObject(res);
        }

        public static string QueryToJson<T>(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var result = cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            return JsonConvert.SerializeObject(result);
        }
    }
}

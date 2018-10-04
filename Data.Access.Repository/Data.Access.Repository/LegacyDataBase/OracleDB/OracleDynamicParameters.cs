using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Data.Access.Repository.Helper;
using Oracle.ManagedDataAccess.Client;

namespace Data.Access.Repository.LegacyDataBase.OracleDB
{
    public sealed class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private static readonly Dictionary<SqlMapper.Identity, Action<IDbCommand, object>> ParamReaderCache = new Dictionary<SqlMapper.Identity, Action<IDbCommand, object>>();

        private readonly Dictionary<string, ParamInfo> _parameters = new Dictionary<string, ParamInfo>();
        private List<object> _templates;

        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        public OracleDynamicParameters()
        {
        }

        /// <summary>
        /// construct a dynamic parameter bag
        /// </summary>
        /// <param name="template">can be an anonymous type or a DynamicParameters bag</param>
        public OracleDynamicParameters(object template)
        {
            AddDynamicParams(template);
        }

        /// <summary>
        /// Append a whole object full of params to the dynamic
        /// EG: AddDynamicParams(new {A = 1, B = 2}) // will add property A and B to the dynamic
        /// </summary>
        /// <param name="param"></param>
        public void AddDynamicParams(
#if CSHARP30
			object param
#else
            dynamic param
#endif
        )
        {
            var obj = param as object;
            if (obj == null) return;

            var subDynamic = obj as OracleDynamicParameters;
            if (subDynamic == null)
            {
                var dictionary = obj as IEnumerable<KeyValuePair<string, object>>;
                if (dictionary == null)
                {
                    _templates = _templates ?? new List<object>();
                    _templates.Add(obj);
                }
                else
                {
                    foreach (var kvp in dictionary)
                    {
#if CSHARP30
						Add(kvp.Key, kvp.Value, null, null, null);
#else
                        Add(kvp.Key, kvp.Value);
#endif
                    }
                }
            }
            else
            {
                if (subDynamic._parameters != null)
                {
                    foreach (var kvp in subDynamic._parameters)
                    {
                        _parameters.Add(kvp.Key, kvp.Value);
                    }
                }

                if (subDynamic._templates == null) return;

                _templates = _templates ?? new List<object>();
                foreach (var t in subDynamic._templates)
                    _templates.Add(t);
            }
        }

        /// <summary>
        /// Add a parameter to this dynamic parameter list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        public void Add(
#if CSHARP30
			string name, object value, DbType? dbType, ParameterDirection? direction, int? size
#else
            string name, object value = null, OracleDataType? dbType = null, ParameterDirection? direction = null, int? size = null
#endif
        )
        {
            OracleDbType? dataType;
            if (dbType != null)
                dataType = dbType.ConvertTo<OracleDbType>();
            else
                dataType = null;

            _parameters[Clean(name)] = new ParamInfo { Name = name, Value = value, ParameterDirection = direction ?? ParameterDirection.Input, DbType = dataType, Size = size };
        }

        private static string Clean(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;

            switch (name[0])
            {
                case '@':
                case ':':
                case '?':
                    return name.Substring(1);
            }
            return name;
        }

        void SqlMapper.IDynamicParameters.AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            AddParameters(command, identity);
        }

        /// <summary>
        /// Add all the parameters needed to the command just before it executes
        /// </summary>
        /// <param name="command">The raw command prior to execution</param>
        /// <param name="identity">Information about the query</param>
        private void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            if (_templates != null)
            {
                foreach (var template in _templates)
                {
                    var newIdent = identity.ForDynamicParameters(template.GetType());

                    Action<IDbCommand, object> appender;
                    lock (ParamReaderCache)
                    {
                        if (!ParamReaderCache.TryGetValue(newIdent, out appender))
                        {
                            appender = SqlMapper.CreateParamInfoGenerator(newIdent, false, true);
                            ParamReaderCache[newIdent] = appender;
                        }
                    }

                    appender(command, template);
                }
            }

            foreach (var param in _parameters.Values)
            {
                var name = Clean(param.Name);
                var add = !((OracleCommand)command).Parameters.Contains(name);
                OracleParameter p;
                if (add)
                {
                    p = ((OracleCommand)command).CreateParameter();
                    p.ParameterName = name;
                }
                else
                    p = ((OracleCommand)command).Parameters[name];

                var val = param.Value;
                p.Value = val ?? DBNull.Value;
                p.Direction = param.ParameterDirection;
                var s = val as string;
                if (s?.Length <= 4000)
                    p.Size = 4000;

                if (param.Size != null)
                    p.Size = param.Size.Value;

                if (param.DbType != null)
                    p.OracleDbType = param.DbType.Value;

                if (add)
                    command.Parameters.Add(p);

                param.AttachedParam = p;
            }
        }

        /// <summary>
        /// All the names of the param in the bag, use Get to yank them out
        /// </summary>
        public IEnumerable<string> ParameterNames => _parameters.Select(p => p.Key);

        /// <summary>
        /// Get the value of a parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>The value, note DBNull.Value is not returned, instead the value is returned as null</returns>
        public T Get<T>(string name)
        {
            var val = _parameters[Clean(name)].AttachedParam.Value;
            if (val != DBNull.Value) return (T)val;
            if (default(T) != null)
                throw new ApplicationException("Attempting to cast a DBNull to a non nullable type!");

            return default(T);
        }
    }
}

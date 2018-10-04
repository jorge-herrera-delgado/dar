using System.Collections.Generic;
using Data.Access.Repository.Repository.Engine.Connection.Model;

namespace Data.Access.Repository.Repository.Engine.Connection
{
    public interface IConnectionProvider
    {
        IEnumerable<ConnectionSchema> ConnectionSchemas { get; set; }
    }
}

using Newtonsoft.Json;

namespace Data.Access.Repository.Helper
{
    internal class JsonMapper
    {
        private readonly object _obj;

        /// <summary>
        /// Instance of the class to map any object
        /// </summary>
        /// <param name="obj"></param>
        public JsonMapper(object obj)
        {
            _obj = obj;
        }

        /// <summary>
        /// Gets the Model from Json Mapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModel<T>() where T : class
            => _obj != null
                ? JsonConvert.DeserializeObject<T>(_obj.ToString())
                : null;

        public string GetJson()
            => JsonConvert.SerializeObject(_obj, Formatting.Indented);
    }
}

using MongoDB.Driver;

namespace ModelBindingSample.Services
{
    public class MongoSettings
    {
        public string Connection { get; internal set; }
        public string Database { get; internal set; }
        public string Issues { get; internal set; }
    }
}
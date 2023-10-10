using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly int _port;
        private ConnectionMultiplexer _ConnectionMultiplexer;
        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Connect() => _ConnectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        //redis ile birden fazla veritabanı default olarak geliyor.amaç farklı veritabanlarıdan farklı işler yapabilmek.loglama,test,product veritabanı vs gibi işlemler kolaylıkla yapılsın diye.biz burada db=1 diye 1.veritabanına değerleri kaydetsin istiyoruz
        public IDatabase GetDb(int db = 1) => _ConnectionMultiplexer.GetDatabase(db);
    }
}

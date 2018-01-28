using Classes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TesteMongodb.Controllers
{
    public class ValuesController : ApiController
    {
        private IMongoDatabase db;
        public ValuesController()
        {
            string connectionString = @"mongodb://" + //Default
                                        "bduser:Bdmongo#159@" + //usuario e senha
                                        "cluster0-shard-00-00-6ce27.mongodb.net:27017," + //host1
                                        "cluster0-shard-00-01-6ce27.mongodb.net:27017," + //host2
                                        "cluster0-shard-00-02-6ce27.mongodb.net:27017" + //host3
                                        //"/TesteMongodb" + //data base
                                        "?replicaSet=Cluster0-shard-0&ssl=true"; //options

            MongoClient client = new MongoClient(connectionString);
            db = client.GetDatabase("TesteMongodb");
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            List<Cidade> cidades = db.GetCollection<Cidade>("cidades").Find(_ => true).ToList();

            var listaCidades = new string[cidades.Count];

            for (int i = 0; i < cidades.Count; i++)
            {
                listaCidades[i] = JsonConvert.SerializeObject(cidades[0]);
            }

            return listaCidades;
        }

        // GET api/values/5
        public string Get(string nome)
        {
            Cidade cidade = db.GetCollection<Cidade>("cidades").Find(c => c.Nome == nome).ToList().FirstOrDefault();
            return JsonConvert.SerializeObject(cidade);
        }

        // POST api/values
        public void Post(string nome, string estado)
        {
            var cidades = db.GetCollection<Cidade>("cidades");

            Cidade cidade = cidades.Find(c => c.Nome == nome).ToList().FirstOrDefault();
            cidade.Estado = estado;

            cidades.ReplaceOne(c => c._id == cidade._id, cidade);
        }

        // PUT api/values/put?nome=&estado=&pais=
        public void Put(string nome, string estado, string pais)
        {
            var cidades = db.GetCollection<Cidade>("cidades");

            Cidade cidade = new Cidade()
            {
                Nome = nome,
                Estado = estado,
                Pais = pais
            };

            cidades.InsertOne(cidade);
        }

        // PUT api/values/put?nome=&pais=
        public void Put(string nome, string pais)
        {
            var cidades = db.GetCollection<Cidade>("cidades");

            Cidade cidade = new Cidade()
            {
                Nome = nome,
                Pais = pais
            };

            cidades.InsertOne(cidade);
        }

        // DELETE api/values/Roma
        public void Delete(string nome)
        {
            var cidades = db.GetCollection<Cidade>("cidades");

            cidades.DeleteOne(c => c.Nome == nome);
        }
    }
}

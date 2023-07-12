using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yinyang.Utilities.MySql.Test
{
    [TestClass]
    public class TestMySql
    {
        private readonly string _connectionString;

        private IConfiguration Configuration { get; }

        public TestMySql()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<TestMySql>()
                .AddEnvironmentVariables()
                .Build();

            _connectionString = Configuration["mysql"];
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException();
            }
        }

        [TestMethod]
        public void EasySelect()
        {
            MySqlConnect.ConnectionString = _connectionString;
            using (var MySql = new MySqlConnect(_connectionString))
            {
                var result = MySql.EasySelect<EntityTest>("select * from test where id = 1;").First();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                MySql.Close();
            }
        }

        [TestMethod]
        public void ExecuteReaderFirst()
        {
            using (var MySql = new MySqlConnect(_connectionString))
            {
                MySql.Open();
                MySql.CommandText = "select * from test where id = 1;";
                var result = MySql.ExecuteReaderFirst<EntityTest>();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                MySql.Close();
            }
        }

        [TestMethod]
        public void Select()
        {
            using (var MySql = new MySqlConnect(_connectionString))
            {
                MySql.Open();
                MySql.CommandText = "select * from test where id = 1;";
                var result = MySql.ExecuteReader<EntityTest>().First();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);
                MySql.Close();
            }
        }

        [TestMethod]
        public void SelectCount()
        {
            using (var MySql = new MySqlConnect(_connectionString))
            {
                MySql.Open();
                MySql.CommandText = "select count(*) from test where id = 1;";
                var result = MySql.ExecuteScalarToInt();

                Assert.AreEqual(1, result);
                MySql.Close();
            }
        }

        [TestMethod]
        public void StoredProcedure()
        {
            using (var MySql = new MySqlConnect(_connectionString))
            {
                MySql.Open();
                MySql.ChangeCommandType(CommandType.StoredProcedure);
                MySql.CommandText = "gettestdata";
                MySql.AddParameter("@uid", 1);
                var result = MySql.ExecuteReaderFirst<EntityTest>();

                var answer = new EntityTest {id = 1, key = 1, value = "あいうえお"};
                Assert.AreEqual(answer.id, result.id);
                Assert.AreEqual(answer.key, result.key);
                Assert.AreEqual(answer.value, result.value);

                MySql.Close();
            }
        }

        [TestMethod]
        public void TableRowsCount()
        {
            using (var MySql = new MySqlConnect(_connectionString))
            {
                MySql.Open();
                Assert.AreEqual(1, MySql.TableRowsCount("test"));
                MySql.Close();
            }
        }
    }
}

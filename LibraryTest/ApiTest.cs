using Microsoft.VisualStudio.TestTools.UnitTesting;
using movie_list.ApiClient;

namespace Library.Tests
{
    [TestClass]
    public class ApiTest
    {

        public class MockedResponse
        {
            public string? testString { get; set; }
        }

        [TestMethod]
        public void TestApiDeserilization()
        {
            MovieApiClient<MockedResponse> client = new();
            var json = @"{
                          'data': {
                            'upcoming': [
                              {
                                'testString': 'test1'
                              },
                              {
                                'testString': 'test2'
                              },
                                ]
                            }
                         }";
            var res = client.CreateMoviesFromJson(json);
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("test1", res[0].testString);
            Assert.AreEqual("test2", res[1].testString);
        }
    }
}

using System.Collections.Generic;

namespace RESTApiTest.ResponseModels
{
    public class AuthorBooksResponseModel
    {
        public string Author { get; set; }
        public int CountBooks { get; set; }
        public List<string> Books { get; set; }
    }
}

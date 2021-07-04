using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RESTApiTest.DbModels;
using RESTApiTest.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        #region ShowResult
        [HttpGet]
        [Route("ShowAuthorsBooks")]
        public List<AuthorBooksResponseModel> ShowAuthorsBooks()
        {
            List<AuthorBooksResponseModel> booksResponseModels = new List<AuthorBooksResponseModel>();

            using (ApplicationContext db = new ApplicationContext())
            {
                var AuthorsBooksIDs = db.BookAuthor
                    .AsEnumerable()
                    .GroupBy(x => x.AuthorID)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.BookID));

                foreach (var keyValuePair in AuthorsBooksIDs)
                {
                    AuthorBooksResponseModel booksResponseModel = new AuthorBooksResponseModel();
                    booksResponseModel.Author = db.Authors.Single(x => x.ID == keyValuePair.Key).Author;
                    booksResponseModel.Books = db.Books
                        .Where(x => keyValuePair.Value.Contains(x.ID))
                        .Select(x => x.Book)
                        .ToList();

                    booksResponseModel.CountBooks = booksResponseModel.Books.Count;
                    booksResponseModels.Add(booksResponseModel);
                }
            }

            return booksResponseModels;
        }

        [HttpGet]
        [Route("ShowAuthors")]
        public List<AuthorResponseModel> ShowAuthors()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Authors
                    .Select(x => new AuthorResponseModel() { Author = x.Author })
                    .ToList();
            }
        }

        [HttpGet]
        [Route("ShowBooks")]
        public List<BookResponseModel> ShowBooks()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Books
                    .Select(x => new BookResponseModel() { Book = x.Book })
                    .ToList();
            }
        }
        #endregion

        #region AddData
        [HttpPost]
        [Route("AddAuthor")]
        public string AddAuthor(AuthorRequestModel request)
        {
            if (string.IsNullOrEmpty(request.AuthorName))
                return Constans.GetEmptyName(Constans.AuthorName);

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора уже будет, но с другим регистром
                if (db.Authors.Any(x => x.Author.ToLower() == request.AuthorName.ToLower()))
                    return Constans.AlreadyAddData(Constans.AuthorName, request.AuthorName);

                AuthorDbModel author = new AuthorDbModel();
                author.Author = request.AuthorName;
                db.Add(author);
                db.SaveChanges();
            }

            return Constans.SuccessAddData(Constans.AuthorName, request.AuthorName);
        }

        [HttpPost]
        [Route("AddBook")]
        public string AddBook(BookRequestModel request)
        {
            if (string.IsNullOrEmpty(request.BookName))
                return Constans.GetEmptyName(Constans.BookName); ;

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если название книги уже будет, но с другим регистром
                if (db.Books.Any(x => x.Book.ToLower() == request.BookName.ToLower()))
                    return Constans.AlreadyAddData(Constans.BookName, request.BookName);

                BookDbModel book = new BookDbModel();
                book.Book = request.BookName;
                db.Add(book);
                db.SaveChanges();
            }

            return Constans.SuccessAddData(Constans.BookName, request.BookName);
        }
        #endregion

        #region EditData
        [HttpPost]
        [Route("EditAuthor")]
        public string EditAuthor(EditAuthorRequestModel request)
        {
            if (string.IsNullOrEmpty(request.AuthorName) || string.IsNullOrEmpty(request.NewAuthorName))
                return Constans.GetEmptyName(Constans.AuthorName);

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора отсутствует в базе
                if (!db.Authors.Any(x => x.Author.ToLower() == request.AuthorName.ToLower()))
                    return Constans.GetMissingName(Constans.AuthorName, request.AuthorName);

                // Если имя автора уже будет, но с другим регистром
                if (db.Authors.Any(x => x.Author.ToLower() == request.NewAuthorName.ToLower()))
                    return Constans.AlreadyAddData(Constans.AuthorName, request.AuthorName);

                AuthorDbModel author = db.Authors.Single(x => x.Author.ToLower() == request.AuthorName.ToLower());
                author.Author = request.NewAuthorName;

                db.Authors.Update(author);
                db.SaveChanges();
            }

            return Constans.SuccessEditData(Constans.AuthorName, request.NewAuthorName, request.AuthorName);
        }

        [HttpPost]
        [Route("EditBook")]
        public string EditBook(EditBookRequestModel request)
        {
            if (string.IsNullOrEmpty(request.BookName) || string.IsNullOrEmpty(request.NewBookName))
                return Constans.GetEmptyName(Constans.BookName);

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если название книги отсутствует в базе
                if (!db.Authors.Any(x => x.Author.ToLower() == request.BookName.ToLower()))
                    return Constans.GetMissingName(Constans.BookName, request.BookName);

                // Если название книги уже будет, но с другим регистром
                if (db.Books.Any(x => x.Book.ToLower() == request.NewBookName.ToLower()))
                    return Constans.AlreadyAddData(Constans.BookName, request.BookName);

                BookDbModel book = db.Books.Single(x => x.Book.ToLower() == request.BookName.ToLower());
                book.Book = request.NewBookName;

                db.Books.Update(book);
                db.SaveChanges();
            }

            return Constans.SuccessEditData(Constans.BookName, request.NewBookName, request.BookName);
        }   
        #endregion

        #region RemoveData
        [HttpPost]
        [Route("RemoveAuthor")]
        public string RemoveAuthor(AuthorRequestModel request)
        {
            if (string.IsNullOrEmpty(request.AuthorName))
                return Constans.GetEmptyName(Constans.AuthorName);

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора отсутствует в базе
                if (!db.Authors.Any(x => x.Author.ToLower() == request.AuthorName.ToLower()))
                    return Constans.GetMissingName(Constans.AuthorName, request.AuthorName);

                // Если будет другой регистр у имени автора
                AuthorDbModel author = db.Authors.Single(x => x.Author.ToLower() == request.AuthorName.ToLower());
                List<AuthorBooksDbModel> authorsBooks = db.BookAuthor.Where(x => x.AuthorID == author.ID).ToList();

                db.Authors.Remove(author);
                db.BookAuthor.RemoveRange(authorsBooks);

                db.SaveChanges();
            }

            return Constans.SuccessRemoveData(Constans.AuthorName, request.AuthorName);
        }

        [HttpPost]
        [Route("RemoveBook")]
        public string RemoveBook(BookRequestModel request)
        {
            if (string.IsNullOrEmpty(request.BookName))
                return Constans.GetEmptyName(Constans.BookName);

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора отсутствует в базе
                if (!db.Books.Any(x => x.Book.ToLower() == request.BookName.ToLower()))
                    return Constans.GetMissingName(Constans.BookName, request.BookName);

                // Если будет другой регистр у названия книги
                BookDbModel book = db.Books.Single(x => x.Book.ToLower() == request.BookName.ToLower());
                List<AuthorBooksDbModel> authorsBooks = db.BookAuthor.Where(x => x.BookID == book.ID).ToList();

                db.Books.Remove(book);
                db.BookAuthor.RemoveRange(authorsBooks);

                db.SaveChanges();
            }

            return Constans.SuccessRemoveData(Constans.BookName, request.BookName);
        }
        #endregion

        #region WorkWithLink
        [HttpPost]
        [Route("AddLink")]
        public string AddLink(LinkRequestModel request)
        {
            if (string.IsNullOrEmpty(request.AuthorName) || string.IsNullOrEmpty(request.BookName))
                return Constans.GetEmptyLink();

            using (ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора или книги отсутствует в базе
                if ((!db.Authors.Any(x => x.Author.ToLower() == request.AuthorName.ToLower())) || (!db.Books.Any(x => x.Book.ToLower() == request.BookName.ToLower())))
                    return Constans.GetMissingLink();

                // Если будет другой регистр у имени автора
                AuthorDbModel author = db.Authors.Single(x => x.Author.ToLower() == request.AuthorName.ToLower());
                // Если будет другой регистр у названия книги
                BookDbModel book = db.Books.Single(x => x.Book.ToLower() == request.BookName.ToLower());

                // Если уже добавлена связь
                if (db.BookAuthor.Any(x => x.AuthorID == author.ID && x.BookID == book.ID))
                    return Constans.AlreadyLink(request.AuthorName, request.BookName);

                AuthorBooksDbModel authorBooks = new AuthorBooksDbModel()
                {
                    AuthorID = author.ID,
                    BookID = book.ID
                };

                db.BookAuthor.Add(authorBooks);
                db.SaveChanges();
            }

            return Constans.SuccessAddLink(request.AuthorName, request.BookName);
        }

        [HttpPost]
        [Route("RemoveLink")]
        public string RemoveLink(LinkRequestModel request)
        {
            if (string.IsNullOrEmpty(request.AuthorName) && string.IsNullOrEmpty(request.BookName))
                return Constans.GetEmptyLink();

            using(ApplicationContext db = new ApplicationContext())
            {
                // Если имя автора или книги отсутствует в базе
                if ((!db.Authors.Any(x => x.Author.ToLower() == request.AuthorName.ToLower())) || (!db.Books.Any(x => x.Book.ToLower() == request.BookName.ToLower())))
                    return Constans.GetMissingLink();

                // Если будет другой регистр у имени автора
                AuthorDbModel author = db.Authors.Single(x => x.Author.ToLower() == request.AuthorName.ToLower());
                // Если будет другой регистр у названия книги
                BookDbModel book = db.Books.Single(x => x.Book.ToLower() == request.BookName.ToLower());

                // Если связи не будет
                if (db.BookAuthor.Any(x => x.AuthorID == author.ID && x.BookID == book.ID))
                    return Constans.GetMissingLink(request.AuthorName, request.BookName);

                AuthorBooksDbModel authorBooks = new AuthorBooksDbModel()
                {
                    AuthorID = author.ID,
                    BookID = book.ID
                };

                db.Remove(authorBooks);
                db.SaveChanges();
            }

            return Constans.SuccessRemoveLInk(request.AuthorName, request.BookName);
        }
        #endregion
    }
}

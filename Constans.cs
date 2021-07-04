namespace RESTApiTest
{
    public static class Constans
    {
        public static string AuthorName = "Имя автора";
        public static string BookName = "Название книги";

        public static string GetEmptyName(string typeName)
        {
            return $"Невозможно добавить/изменить/удалить пустое {typeName.ToLower()}!";
        }

        public static string GetMissingName(string typeName, string currentName)
        {
            return $"{typeName} {currentName} отсутствует в базе!";
        }

        public static string AlreadyAddData(string typeName, string currentName)
        {
            return $"{typeName} {currentName} уже добавлено в базу!";
        }

        public static string SuccessAddData(string typeName, string currentName)
        {
            return $"{typeName} {currentName} успешно добавлено в базу!";
        }

        public static string SuccessEditData(string typeName, string currentName, string lastName)
        {
            return $"{typeName} {lastName} было успешно заменено на {currentName}!";
        }

        public static string SuccessRemoveData(string typeName, string currentName)
        {
            return $"{typeName} {currentName} успешно удалено из базы!";
        }

        public static string GetEmptyLink()
        {
            return $"{Constans.AuthorName} и/или {Constans.BookName.ToLower()} не содержат значений!";
        }

        public static string GetMissingLink()
        {
            return $"{Constans.AuthorName} и/или {Constans.BookName.ToLower()} отсутствует в базе!";
        }

        public static string AlreadyLink(string authorName, string bookName)
        {
            return $"Связь между {authorName} и {bookName} уже существует!";
        }

        public static string SuccessAddLink(string authorName, string bookName)
        {
            return $"Связь между {authorName} и {bookName} успешно установлена! ";
        }

        public static string GetMissingLink(string authorName, string bookName)
        {
            return $"Связь между {authorName} и {bookName} отсутствует в базе! ";
        }

        public static string SuccessRemoveLInk(string authorName, string bookName)
        {
            return $"Связь между {authorName} и {bookName} успешно разорвана!";
        }
    }
}

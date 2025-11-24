using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class BookUser
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime MembershipExpiry { get; set; }

        public BookUser(int id, string name)
        {
            Id = id;
            Name = name;
            MembershipExpiry = DateTime.Now.AddYears(1);
        }

        public void ExtendMembership(int months)
        {
            MembershipExpiry.AddMonths(months);
        }

        public bool IsMembershipValid => DateTime.Now <= MembershipExpiry;
    }

    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Content { get; private set; }

        public Book(int id, string title, string author, string content)
        {
            Id =id;
            Title = title;
            Author = author;
            Content = content;
        }

        public void DisplaySummary()
        {
            Console.WriteLine($"{Title} by {Author}");
        }

        public void Read()
        {
            Console.WriteLine($"Reading Book: {Title}\n\n{Content.Substring(0, Math.Min(200, Content.Length))}...");
        }
    }

    public class Library
    {
        private List<Book> books = new List<Book>();

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public List<Book> Search(string keyword)
        {
            return books.Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Book GetBookById(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }
    }

    public class BookReaderSystem
    {
        private BookUser activeUser;
        private Book activeBook;
        private Library library = new Library();
        private Dictionary<int, BookUser> users = new Dictionary<int, BookUser>();

        public void RegisterUser(int id, string name)
        {
            if (!users.ContainsKey(id))
            {
                users[id] = new BookUser(id, name);
                Console.WriteLine($"User {name} registered successfully.");
            }
        }

        public bool LoginUser(int id) { 
            if(users.ContainsKey(id) && users[id].IsMembershipValid)
            {
                activeUser = users[id];
                return true;
            }

            Console.WriteLine("Login failed: Invalid user or expired membership.");
            return false;
        }

        public void LogoutUser() {
            if (activeUser != null) {
                activeUser = null;
                activeBook = null;
            }
        }

        public void AddBook(Book book) { 
            library.AddBook(book);
        }

        public void SearchBooks(string query)
        {
            var results = library.Search(query);
            if (!results.Any())
            {
                Console.WriteLine("No books found");
                return;
            }
            Console.WriteLine("Search Results:");
            foreach (var book in results) {
                book.DisplaySummary();
            }
        }

        public void ReadBook(int bookId) {
            if(activeUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }

            var book = library.GetBookById(bookId);

            if (book == null) {
                Console.WriteLine("Book not found");
                return;
            }

            activeBook = book;
            book.Read();
        }
    }
}

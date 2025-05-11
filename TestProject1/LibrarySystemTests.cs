using Microsoft.VisualBasic;
using UnitTesting;
namespace MsTestLibrary;

[TestClass]
public class LibrarySystemTests
{
    // IF test fail means u can add a book with the same ISBN number (example) (Not Good) 
    [TestMethod]
    [TestCategory("Normally")]
    [DataRow("The Great Gatsby", "F. Scott Fitzgerald", "9780743273121", 1924, "Book is valid")]
    public void AddBook_CanAddBook_ReturnTrue(string title, string author, string isbn, int year, string message)
    {
        //Create a instance of LibrarySystem
        var library = new LibrarySystem();     
        
        //Add a book with data 
        var actual = library.AddBook(new Book(title, author, isbn, year));

        //Check if the book was added
        Assert.IsTrue(actual, message);

    }

    [DataTestMethod]// for multiple test cases
    [TestCategory("Edge cases")]
    [DataRow("978074327")]
    [DataRow("sssssssssssss")]
    public void AddBook_AddWrongISBNNumber_ReturnFalse(string isbn)
    {
        var library = new LibrarySystem();
        //Add wrong ISBN number
        var actual = library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn, 1925));
        //Check if ISBN number was added 
        Assert.IsFalse(actual);
    }

    [TestMethod]
    [TestCategory("Edge cases")]
    [DataRow("9780743273565", "9780743273565")] 
    public void AddBook_DuplicateISBNNumber_ReturnFalse(string isbn, string isbn2)
    {
        var library = new LibrarySystem();
        //Add a book with the same ISBN number
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn, 1925));
        var result = library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn2, 1925));
        //Should not add same ISBN number 
        Assert.IsFalse(result);
      
    }
    /// Remove tests

    [TestMethod]
    [TestCategory("Normally")]
    public void RemoveBook_CanRemoveBook_ReturnTrue()
    {
        var library = new LibrarySystem();
        //Add a book to remove
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));      
        //Remove the book
        var result = library.RemoveBook("9780743273565");
        //Check if the book was removed
        Assert.IsTrue(result);
        Assert.IsNull(library.SearchByISBN("9780743273565"));
    }

    [TestMethod]
    [TestCategory("Edge cases")]
    public void RemoveBook_RemoveBookWhenBorrowed_ReturnFalse()
    {
        var library = new LibrarySystem();
        //Add a book to remove
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Borrow the book
        library.BorrowBook("9780743273565");
        //Try to remove the book
        var result = library.RemoveBook("9780743273565");
        //Check if the book was removed
        Assert.IsFalse(result);
    }

    [DataTestMethod]
    [TestCategory("Normally")]
    [DataRow("the great gatsby", "F. Scott Fitzgerald", "9780743273565", "Matches title")]
    [DataRow("The Great Gatsby", "F. S", "9780743273565", "Matches ISBN")]
    [DataRow("The Great Gatsby", "F. Scott Fitzgerald", "978074", "Matches author")]

    public void SearchBooks_CanSearchBooksWithMultipleOptions_ReturnTrue(string title, string author, string isbn, string message)
    {
        var library = new LibrarySystem();
        //Add a book to search
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Search by title
        var result = library.SearchByTitle(title);
        //Check if the book was found
        Assert.IsNotNull(result);
        Assert.AreEqual("The Great Gatsby", result[0].Title);
        //Search by author
        var result2 = library.SearchByAuthor(author);
        //Check if the book was found
        Assert.IsNotNull(result2);  
        Assert.AreEqual("F. Scott Fitzgerald", result2[0].Author);
        //Search by ISBN 
        var result3 = library.SearchByISBNPartial(isbn);
        //Check if the book was found
        Assert.IsNotNull(result3);
        Assert.AreEqual("9780743273565", result3[0].ISBN);
    }

    [TestMethod]
    [TestCategory("Normally")]
    public void BorrowBook_CanBorrowBook_ReturnTrue()
    {
        var library = new LibrarySystem();
        //Add a book to borrow
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Borrow the book
        var result = library.BorrowBook("9780743273565");
        //Check if the book was borrowed
        Assert.IsTrue(result);
        Assert.IsTrue(library.SearchByISBN("9780743273565").IsBorrowed == true);
    }

    [TestMethod]
    [TestCategory("Edge case")]
    public void BorrowBook_BorrowBookThatAlreadyIsBorrowed_ReturnFalse()
    {
        var library = new LibrarySystem();
        // Add a book to borrow
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Borrow the book
        var borrow = library.BorrowBook("9780743273565");
        var result = library.BorrowBook("9780743273565");
        //Check if the book was borrowed again 
        Assert.IsFalse(result); 
    }

    [TestMethod]
    [TestCategory("Normally")]
    public void BorrowBook_ReturnDateShouldBeSet_ReturnTrue()
    {
        var library = new LibrarySystem();
        //Add a book to borrow
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Borrow the book
        library.BorrowBook("9780743273565");
        //Check if the borrow date is set
        Assert.IsNotNull(library.SearchByISBN("9780743273565").BorrowDate);
    }

    [TestMethod]
    [TestCategory("Normally")]
    public void ReturnBook_ReturnDateShouldReset_ReturnTrue()
    {
        var library = new LibrarySystem();
        //Add a book to borrow
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Borrow the book
        library.BorrowBook("9780743273565");
        //Return the book
        library.ReturnBook("9780743273565");
        //Check if the borrow date is reset
        Assert.IsNull(library.SearchByISBN("9780743273565").BorrowDate);
    }

    [TestMethod]
    [TestCategory("Edge case")]
    public void ReturnBook_ReturnBookThatIsNotBorrowed_ReturnFalse()
    {
        var library = new LibrarySystem();
        //Add a book to borrow
        library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", 1925));
        //Return the book
        var result = library.ReturnBook("9780743273565");
        //Check if the book was returned
        Assert.IsFalse(result);
    }

    [TestMethod]
    [TestCategory("Normally")]
    public void IsBookOverDue_MakeSureBookCanBeOverDue_ReturnTrue()
    {
        var library = new LibrarySystem();
        string isbn = "9780743273565";
        //Add a book to borrow
        var bokk = new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn, 1925);
       
        library.AddBook(bokk);
        //Borrow the book
        library.BorrowBook(isbn);

        var isn = library.SearchByISBN(isbn);
        isn.BorrowDate = new DateTime(2025, 05, 01);
        
        var result = library.IsBookOverdue(isbn, 3);
        //Check if the book is overdue
        Assert.IsTrue(result);      
        
    }

    [TestMethod]
    [TestCategory("Normally")]
    public void CalculateLateFee_IsLateFeeCalculatedCorrect_ReturnTrue()
    {
        var library = new LibrarySystem();
        string isbn = "9780743273565";
        //Add book to borrow
        var bokk = new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn, 1925);
        library.AddBook(bokk);
       
        //Borrow the book
        library.BorrowBook(isbn);
        var isn = library.SearchByISBN(isbn);
        isn.BorrowDate = new DateTime(2025, 05, 02);
        decimal result = library.CalculateLateFee(isbn, 3);
        //Check if the late fee is correct
        decimal expected = 1.5m;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Edge case")]
    public void CalculateLateFee_CalculateLateFeeWithUnBorrowedBook_ReturnFalse()
    {
        var library = new LibrarySystem();
        string isbn = "9780743273565";
        //Add book to borrow
        var bokk = new Book("The Great Gatsby", "F. Scott Fitzgerald", isbn, 1925);
        library.AddBook(bokk);
        //Calculate late fee with unborrowed book
        bool result = Convert.ToBoolean(library.CalculateLateFee(isbn, 3));
        //Check if late fee was calculated 
        Assert.IsFalse(result);

    }
}

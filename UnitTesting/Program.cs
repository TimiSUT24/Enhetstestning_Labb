namespace UnitTesting
{
    public class Program
    {
        static void Main(string[] args)
        {
            LibrarySystem library = new LibrarySystem();
            UserInterface.DisplayMenu(library);
        }
    }
}

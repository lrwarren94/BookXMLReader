using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace BookXMLReader
{
    class ConsoleInterface
    {
        private class QueryData
        {
            private string author;
            private string title;
            private string genre;
            private string priceLow;
            private string priceHigh;
            private bool priceLesserThan;
            private bool priceGreaterThan;
            private DateTime publish_dateLow;
            private DateTime publish_dateHigh;
            private bool publish_dateLesserThan;
            private bool publish_dateGreaterThan;
            private string description;

            public string Author { get; set; }
            public string Title { get; set; }
            public string Genre { get; set; }
            public string PriceLow { get; set; }
            public string PriceHigh { get; set; }
            public bool PriceLesserThan { get; set; }
            public bool PriceGreaterThan { get; set; }
            public DateTime Publish_dateLow { get; set; }
            public DateTime Publish_dateHigh { get; set; }
            public bool Publish_dateLesserThan { get; set; }
            public bool Publish_dateGreaterThan { get; set; }
            public string Description { get; set; }

            public QueryData()
            {
                Author = "";
                Title = "";
                Genre = "";
                PriceLow = "";
                PriceHigh = "";
                PriceLesserThan = false;
                PriceGreaterThan = false;
                Publish_dateLow = DateTime.MinValue;
                Publish_dateHigh = DateTime.MinValue;
                publish_dateLesserThan = false;
                publish_dateGreaterThan = false;
                Description = "";
            }
        }

        private bool looping;
        private string menuState;
        private string subMenuState;
        private string userInput;
        private QueryData queryData;
        private List<string> authors;
        private List<string> titles;
        private List<string> genres;

        public ConsoleInterface()
        {
            looping = true;
            menuState = "begin";
            subMenuState = "";
            queryData = new QueryData();
            using (FileStream fs = new FileStream("Books.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(catalog));
                catalog books = (catalog)xs.Deserialize(fs);

                authors = books.book.Select(b => b.author).Distinct().ToList();
                titles = books.book.Select(b => b.title).Distinct().ToList();
                genres = books.book.Select(b => b.genre).Distinct().ToList();
            }
        }

        public void Start()
        {
            while (looping)
            {
                Console.Clear();

                if (menuState.Equals("begin"))
                {
                    menuState = "MainMenu";
                }
                else if (menuState.Equals("MainMenu"))
                {
                    DisplayMainMenu();

                    if (int.TryParse(userInput, out int selectedOption))
                    {
                        switch (selectedOption)
                        {
                            case 1:
                                menuState = "Author";
                                break;

                            case 2:
                                menuState = "Title";
                                break;

                            case 3:
                                menuState = "Genre";
                                break;

                            case 4:
                                menuState = "Price";
                                break;

                            case 5:
                                menuState = "Publish Date";
                                break;

                            case 6:
                                menuState = "Description";
                                break;

                            case 7:
                                menuState = "ExecuteQuery";
                                break;
                        }
                    }
                    else
                    {
                        DisplayErrorMessage();
                        DisplayMainMenu();
                    }
                }
                else if (menuState.Equals("Author"))
                {
                    DisplayAuthorMenu();

                    if (int.TryParse(userInput, out int selectedOption))
                    {
                        if (subMenuState.Equals(""))
                        {
                            switch (selectedOption)
                            {
                                case 1:
                                    subMenuState = "pick";
                                    break;

                                case 2:
                                    subMenuState = "search";
                                    break;

                                default:
                                    DisplayErrorMessage();
                                    break;
                            }
                        }
                        else if (subMenuState.Equals("pick"))
                        {
                            if (selectedOption - 1 < authors.Count)
                            {
                                queryData.Author = authors.ElementAt(selectedOption - 1);
                                menuState = "MainMenu";
                                subMenuState = "";
                            }
                            else
                            {
                                DisplayErrorMessage();
                            }
                        }
                    }
                    else
                    {
                        if (!subMenuState.Equals("search"))
                        {
                            DisplayErrorMessage();
                        }
                        else
                        {
                            queryData.Author = userInput;
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                    }
                }
                else if (menuState.Equals("Title"))
                {
                    DisplayTitleMenu();

                    if (int.TryParse(userInput, out int selectedOption))
                    {
                        if (subMenuState.Equals(""))
                        {
                            switch (selectedOption)
                            {
                                case 1:
                                    subMenuState = "pick";
                                    break;

                                case 2:
                                    subMenuState = "search";
                                    break;

                                default:
                                    DisplayErrorMessage();
                                    break;
                            }
                        }
                        else if (subMenuState.Equals("pick"))
                        {
                            if (selectedOption - 1 < titles.Count)
                            {
                                queryData.Title = titles.ElementAt(selectedOption - 1);
                                menuState = "MainMenu";
                                subMenuState = "";
                            }
                            else
                            {
                                DisplayErrorMessage();
                            }
                        }
                    }
                    else
                    {
                        if (!subMenuState.Equals("search"))
                        {
                            DisplayErrorMessage();
                        }
                        else
                        {
                            queryData.Title = userInput;
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                    }
                }
                else if (menuState.Equals("Genre"))
                {
                    DisplayGenreMenu();

                    if (int.TryParse(userInput, out int selectedOption))
                    {
                        if (subMenuState.Equals(""))
                        {
                            switch (selectedOption)
                            {
                                case 1:
                                    subMenuState = "pick";
                                    break;

                                case 2:
                                    subMenuState = "search";
                                    break;

                                default:
                                    DisplayErrorMessage();
                                    break;
                            }
                        }
                        else if (subMenuState.Equals("pick"))
                        {
                            if (selectedOption - 1 < genres.Count)
                            {
                                queryData.Genre = genres.ElementAt(selectedOption - 1);
                                menuState = "MainMenu";
                                subMenuState = "";
                            }
                            else
                            {
                                DisplayErrorMessage();
                            }
                        }
                    }
                    else
                    {
                        if (!subMenuState.Equals("search"))
                        {
                            DisplayErrorMessage();
                        }
                        else
                        {
                            queryData.Genre = userInput;
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                    }
                }
                else if (menuState.Equals("Price"))
                {
                    DisplayPriceMenu();

                    /*
                     * Identifies the following input formats:
                     * 1, 1.2
                     * <1, <1.2
                     * >1, >1.2
                     * 1-1, 1.2-3.4
                     */
                    Regex regex = new Regex(@"^[0-9]+[\.]?([0-9]?)+$|^>[0-9]+[\.]?([0-9]?)+$|^<[0-9]+[\.]?([0-9]?)+$|^[0-9]+[\.]?([0-9]?)+-[0-9]+[\.]?([0-9]?)+$");

                    if (regex.IsMatch(userInput))
                    {
                        if (Regex.IsMatch(userInput, @"<"))
                        {
                            queryData.PriceLesserThan = true;
                            queryData.PriceHigh = Regex.Match(userInput, @"[0-9]+[\.]?([0-9]?)+$").ToString();
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                        else if (Regex.IsMatch(userInput, @">"))
                        {
                            queryData.PriceGreaterThan = true;
                            queryData.PriceLow = Regex.Match(userInput, @"[0-9]+[\.]?([0-9]?)+$").ToString();
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                        else if (Regex.IsMatch(userInput, @"-"))
                        {
                            queryData.PriceLesserThan = true;
                            queryData.PriceGreaterThan = true;
                            string[] numbers = userInput.Split("-");
                            queryData.PriceLow = numbers[0];
                            queryData.PriceHigh = numbers[1];
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                        else
                        {
                            queryData.PriceLow = Regex.Match(userInput, @"^[0-9]+[\.]?([0-9]?)+$").ToString();
                            queryData.PriceHigh = Regex.Match(userInput, @"^[0-9]+[\.]?([0-9]?)+$").ToString();
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                    }
                    else
                    {
                        DisplayErrorMessage();
                    }
                }
                else if (menuState.Equals("Publish Date"))
                {
                    DisplayPublishDateMenu();
                    /*
                     * Identifies the following input formats:
                     * 1969-08-15
                     * <1969-08-15
                     * >1969-08-15
                     * 1969-08-15-1969-08-18
                     */
                    Regex regex = new Regex(@"^[0-9]{4}-[0-1][0-9]-[0-3][0-9]$|<^[0-9]{4}-[0-1][0-9]-[0-3][0-9]$|>^[0-9]{4}-[0-1][0-9]-[0-3][0-9]$|^[0-9]{4}-[0-1][0-9]-[0-3][0-9]-[0-9]{4}-[0-1][0-9]-[0-3][0-9]$");

                    if (Regex.IsMatch(userInput, @"<"))
                    {
                        queryData.Publish_dateLesserThan = true;
                        queryData.Publish_dateHigh = DateTime.Parse(Regex.Match(userInput, @"[0-9]{4}-[0-1][0-9]-[0-3][0-9]").ToString());
                        menuState = "MainMenu";
                        subMenuState = "";
                    }
                    else if (Regex.IsMatch(userInput, @">"))
                    {
                        queryData.Publish_dateGreaterThan = true;
                        queryData.Publish_dateHigh = DateTime.Parse(Regex.Match(userInput, @"[0-9]{4}-[0-1][0-9]-[0-3][0-9]").ToString());
                        menuState = "MainMenu";
                        subMenuState = "";
                    }
                    else if (Regex.IsMatch(userInput, @"-"))
                    {
                        queryData.Publish_dateLesserThan = true;
                        queryData.Publish_dateGreaterThan = true;
                        queryData.Publish_dateLow = DateTime.Parse(Regex.Match(userInput, @"^[0-9]{4}-[0-1][0-9]-[0-3][0-9]").ToString());
                        queryData.Publish_dateHigh = DateTime.Parse(Regex.Match(userInput, @"[0-9]{4}-[0-1][0-9]-[0-3][0-9]$").ToString());
                        menuState = "MainMenu";
                        subMenuState = "";
                    }
                    else
                    {
                        try
                        {
                            queryData.Publish_dateLow = DateTime.Parse(Regex.Match(userInput, @"^[0-9]+[\.]?([0-9]?)+$").ToString());
                            queryData.Publish_dateHigh = DateTime.Parse(Regex.Match(userInput, @"^[0-9]+[\.]?([0-9]?)+$").ToString());
                            menuState = "MainMenu";
                            subMenuState = "";
                        }
                        catch (Exception)
                        {
                            DisplayErrorMessage();
                        }
                    }
                }
                else if (menuState.Equals("Description"))
                {
                    DisplayDescriptionMenu();
                    queryData.Description = userInput;
                    menuState = "MainMenu";
                    subMenuState = "";
                }
                else if (menuState.Equals("ExecuteQuery"))
                {
                    ExecuteQuery();
                    menuState = "MainMenu";
                    subMenuState = "";
                }
                else
                {
                    Console.Clear();
                    Console.Write("A fatal error has occurred.");
                    Thread.Sleep(10000);
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.Write("Welcome to BookQuery, International Paper's primary "
                        + "internal book-searching system. Please select your "
                        + "desired paramaters below, and when you're ready to "
                        + "search, select the Execute Query option.\n\n");
            Console.Write("\t1. Author\n"
                        + "\t2. Title\n"
                        + "\t3. Genre\n"
                        + "\t4. Price\n"
                        + "\t5. Publish Date\n"
                        + "\t6. Description\n"
                        + "\t7. Execute Query\n");
            userInput = Console.ReadLine();
        }

        private void DisplayAuthorMenu()
        {
            Console.Clear();
            if (subMenuState.Equals(""))
            {
                Console.Write("Would you like to select from a list of authors, "
                            + "or would you like to search for a specific author?\n\n");
                Console.Write("\t1. Pick from a list\n"
                            + "\t2. Search specific\n");
                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("pick"))
            {
                Console.Write("Please select an author from below.\n\n");

                int counter = 1;
                foreach (string author in authors)
                {
                    Console.Write($"\t{counter}. {author}\n");
                    counter++;
                }

                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("search"))
            {
                Console.Write("Please enter part or all of the author's name below.\n\n");

                userInput = Console.ReadLine();
            }
        }

        private void DisplayTitleMenu()
        {
            Console.Clear();
            if (subMenuState.Equals(""))
            {
                Console.Write("Would you like to select from a list of titles, "
                            + "or would you like to search for a specific title?\n\n");
                Console.Write("\t1. Pick from a list\n"
                            + "\t2. Search specific\n");
                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("pick"))
            {
                Console.Write("Please select a title from below.\n\n");

                int counter = 1;
                foreach (string title in titles)
                {
                    Console.Write($"\t{counter}. {title}\n");
                    counter++;
                }

                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("search"))
            {
                Console.Write("Please enter part or all of the title's name below.\n\n");

                userInput = Console.ReadLine();
            }
        }

        private void DisplayGenreMenu()
        {
            Console.Clear();
            if (subMenuState.Equals(""))
            {
                Console.Write("Would you like to select from a list of genres, "
                            + "or would you like to search for a specific genre?\n\n");
                Console.Write("\t1. Pick from a list\n"
                            + "\t2. Search specific\n");
                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("pick"))
            {
                Console.Write("Please select a genre from below.\n\n");

                int counter = 1;
                foreach (string genre in genres)
                {
                    Console.Write($"\t{counter}. {genre}\n");
                    counter++;
                }

                userInput = Console.ReadLine();
            }
            else if (subMenuState.Equals("search"))
            {
                Console.Write("Please enter part or all of the genre's name below.\n\n");

                userInput = Console.ReadLine();
            }
        }

        private void DisplayPriceMenu()
        {
            Console.Clear();
            Console.Write("Please enter a price range (Format: 1.2, <1.2, >1.2, 1.1-2.2).\n\n");

            userInput = Console.ReadLine();
        }

        private void DisplayPublishDateMenu()
        {
            Console.Clear();
            Console.Write("Please enter a valid Publish Date (Format: YYYY-MM-DD, <YYYY-MM-DD, >YYYY-MM-DD, YYYY-MM-DD-YYYY-MM-DD). \n\n");

            userInput = Console.ReadLine();
        }

        private void DisplayDescriptionMenu()
        {
            Console.Clear();
            Console.Write("Please enter part of the book description.\n\n");

            userInput = Console.ReadLine();
        }

        private void ExecuteQuery()
        {
            QueryParser queryParser = new QueryParser();

            queryParser.parseQuery(queryData.Author,
                                queryData.Title,
                                queryData.Genre,
                                queryData.PriceLow, queryData.PriceHigh, queryData.PriceLesserThan, queryData.PriceGreaterThan,
                                queryData.Publish_dateLow, queryData.Publish_dateHigh, queryData.Publish_dateLesserThan, queryData.Publish_dateGreaterThan,
                                queryData.Description);

            queryData = new QueryData();
            userInput = "";

            Console.ReadLine();
        }

        private static void DisplayErrorMessage()
        {
            Console.Clear();
            Console.WriteLine("Error: invalid input. Please re-enter.");
            Thread.Sleep(4000);
            Console.Clear();
        }

    }
}
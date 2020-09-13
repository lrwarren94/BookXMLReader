using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace BookXMLReader
{
    class QueryParser
    {
        public void parseQuery(string query)
        {
            //placeholder       
        }

        public void parseQuery(string author,
                                string title,
                                string genre,
                                string priceLow, string priceHigh, bool priceLesserThan, bool priceGreaterThan,
                                DateTime publish_dateLow, DateTime publish_dateHigh, bool publish_dateLesserThan, bool publish_dateGreaterThan,
                                string description)
        {
            List<catalogBook> data = getDataLINQ(author,
                                        title,
                                        genre,
                                        priceLow, priceHigh, priceLesserThan, priceGreaterThan,
                                        publish_dateLow, publish_dateHigh, publish_dateLesserThan, publish_dateGreaterThan,
                                        description);

            Console.Write("+-----------------------------------------------------------------------------------+\n");
            Console.Write("| Author          | Title           | Genre      | Price    | Publish Date          |\n");
            Console.Write("+-----------------------------------------------------------------------------------+\n");
            foreach (catalogBook book in data)
            {
                Console.Write("| ");
                for (int i = 0; i <= 14; i++)
                {
                    if (i < book.author.Length)
                        Console.Write(book.author[i]);
                    else
                        Console.Write(" ");
                }

                Console.Write(" | ");
                for (int i = 0; i <= 14; i++)
                {
                    if (i < book.title.Length)
                        Console.Write(book.title[i]);
                    else
                        Console.Write(" ");
                }

                Console.Write(" | ");
                for (int i = 0; i <= 9; i++)
                {
                    if (i < book.genre.Length)
                        Console.Write(book.genre[i]);
                    else
                        Console.Write(" ");
                }

                Console.Write(" | ");
                for (int i = 0; i <= 7; i++)
                {
                    if (i < book.price.ToString().Length)
                        Console.Write(book.price.ToString()[i]);
                    else
                        Console.Write(" ");
                }

                Console.Write(" | ");
                for (int i = 0; i <= 20; i++)
                {
                    if (i < book.publish_date.ToString().Length)
                        Console.Write(book.publish_date.ToString()[i]);
                    else
                        Console.Write(" ");
                }

                Console.Write(" |\n");
                Console.Write("+-----------------------------------------------------------------------------------+\n");
            }
        }

        public List<catalogBook> getDataLINQ(string author,
                                    string title,
                                    string genre,
                                    string priceLow, string priceHigh, bool priceLesserThan, bool priceGreaterThan,
                                    DateTime publish_dateLow, DateTime publish_dateHigh, bool publish_dateLesserThan, bool publish_dateGreaterThan,
                                    string description)
        {
            List<catalogBook> list = new List<catalogBook>();

            using (FileStream fs = new FileStream("Books.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(catalog));
                catalog books = (catalog)xs.Deserialize(fs);

                author = author.ToLower();
                title = title.ToLower();
                genre = genre.ToLower();
                description = description.ToLower();
                var query = (from b in books.book select b);

                if (!String.IsNullOrEmpty(author))
                {
                    query = query.Where(book => book.author.ToLower().Contains(author));
                }

                if (!String.IsNullOrEmpty(title))
                {
                    query = query.Where(book => book.title.ToLower().Contains(title));
                }

                if (!String.IsNullOrEmpty(genre))
                {
                    query = query.Where(book => book.genre.ToLower().Contains(genre));
                }

                if (!(String.IsNullOrEmpty(priceLow) && String.IsNullOrEmpty(priceHigh)))
                {
                    int.TryParse(priceLow, out int lowNum);
                    int.TryParse(priceHigh, out int highNum);

                    if (priceLesserThan == true && priceGreaterThan == true)
                    {
                        query = query.Where(book => book.price >= lowNum && book.price <= highNum);
                    }
                    else if (priceLesserThan == true && priceGreaterThan == false)
                    {
                        query = query.Where(book => book.price <= highNum);
                    }
                    else if (priceLesserThan == false && priceGreaterThan == true)
                    {
                        query = query.Where(book => book.price >= lowNum);
                    }
                    else
                    {
                        query = query.Where(book => book.price == lowNum || book.price == highNum);
                    }
                }

                if (!(publish_dateLow == DateTime.MinValue) && !(publish_dateHigh == DateTime.MinValue))
                {
                    if (publish_dateLesserThan == true && publish_dateGreaterThan == true)
                    {
                        query = query.Where(book => book.publish_date >= publish_dateLow && book.publish_date <= publish_dateHigh);
                    }
                    else if (publish_dateLesserThan == true && publish_dateGreaterThan == false)
                    {
                        query = query.Where(book => book.publish_date <= publish_dateHigh);
                    }
                    else if (publish_dateLesserThan == false && publish_dateGreaterThan == true)
                    {
                        query = query.Where(book => book.publish_date >= publish_dateLow);
                    }
                    else
                    {
                        query = query.Where(book => book.publish_date == publish_dateLow || book.publish_date == publish_dateHigh);
                    }
                }

                if (!String.IsNullOrEmpty(description))
                {
                    query = query.Where(book => book.description.ToLower().Contains(description));
                }

                list = query.Select(book => new catalogBook
                {
                    author = book.author,
                    title = book.title,
                    genre = book.genre,
                    price = book.price,
                    publish_date = book.publish_date,
                    description = book.description
                }).ToList();
            }

            return list;
        }

        public List<catalogBook> getData(string author,
                                    string title,
                                    string genre,
                                    string priceLow, string priceHigh, bool priceLesserThan, bool priceGreaterThan,
                                    DateTime publish_dateLow, DateTime publish_dateHigh, bool publish_dateLesserThan, bool publish_dateGreaterThan,
                                    string description)

        {
            List<catalogBook> list = new List<catalogBook>();
            List<List<catalogBook>> lists = new List<List<catalogBook>>();
            bool authorPresent = (!String.IsNullOrEmpty(author));
            bool titlePresent = (!String.IsNullOrEmpty(title));
            bool genrePresent = (!String.IsNullOrEmpty(genre));
            bool pricePresent = (!(String.IsNullOrEmpty(priceLow) && String.IsNullOrEmpty(priceHigh)));
            bool publish_datePresent = (!(publish_dateLow == DateTime.MinValue) && !(publish_dateHigh == DateTime.MinValue));
            bool descriptionPresent = (!String.IsNullOrEmpty(description));
            List<catalogBook> authorList = new List<catalogBook>();
            List<catalogBook> titleList = new List<catalogBook>();
            List<catalogBook> genreList = new List<catalogBook>();
            List<catalogBook> priceList = new List<catalogBook>();
            List<catalogBook> publish_dateList = new List<catalogBook>();
            List<catalogBook> descriptionList = new List<catalogBook>();

            author = author.ToLower();
            title = title.ToLower();
            genre = genre.ToLower();
            description = description.ToLower();

            if (authorPresent)
            {
                authorList = getDataHelper(author, "author");
                lists.Add(authorList);
            }
            if (titlePresent)
            {
                titleList = getDataHelper(title, "title");
                lists.Add(titleList);
            }
            if (genrePresent)
            {
                genreList = getDataHelper(genre, "genre");
                lists.Add(genreList);
            }
            if (pricePresent)
            {
                priceList = getDataHelper(priceLow, priceHigh, priceLesserThan, priceGreaterThan);
                lists.Add(priceList);
            }
            if (publish_datePresent)
            {
                publish_dateList = getDataHelper(publish_dateLow, publish_dateHigh, publish_dateLesserThan, publish_dateGreaterThan);
                lists.Add(publish_dateList);
            }
            if (descriptionPresent)
            {
                descriptionList = getDataHelper(description, "description");
                lists.Add(descriptionList);
            }

            list = Combine(lists);

            return list;
        }

        private List<catalogBook> getDataHelper(string property, string propertyType)
        {
            List<catalogBook> bookList = new List<catalogBook>();

            using (FileStream fs = new FileStream("Books.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(catalog));
                catalog books = (catalog)xs.Deserialize(fs);
                switch (propertyType)
                {
                    case "author":

                        foreach (catalogBook book in books.book)
                        {
                            if (book.author.ToLower().Contains(property))
                                bookList.Add(book);
                        }
                        break;

                    case "title":
                        foreach (catalogBook book in books.book)
                        {
                            if (book.title.ToLower().Contains(property))
                                bookList.Add(book);
                        }
                        break;

                    case "genre":
                        foreach (catalogBook book in books.book)
                        {
                            if (book.genre.ToLower().Contains(property))
                                bookList.Add(book);
                        }
                        break;

                    case "description":
                        foreach (catalogBook book in books.book)
                        {
                            if (book.description.ToLower().Contains(property))
                                bookList.Add(book);
                        }
                        break;
                }
            }
            return bookList;
        }

        private List<catalogBook> getDataHelper(string low, string high, bool lesserThan, bool GreaterThan)
        {
            List<catalogBook> bookList = new List<catalogBook>();

            int.TryParse(low, out int lowNum);
            int.TryParse(high, out int highNum);

            using (FileStream fs = new FileStream("Books.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(catalog));
                catalog books = (catalog)xs.Deserialize(fs);

                if (lesserThan == true && GreaterThan == true)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.price >= lowNum && book.price <= highNum)
                            bookList.Add(book);
                    }
                }
                else if (lesserThan == true && GreaterThan == false)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.price <= highNum)
                            bookList.Add(book);
                    }
                }
                else if (lesserThan == false && GreaterThan == true)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.price >= lowNum)
                            bookList.Add(book);
                    }
                }
                else
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.price == lowNum || book.price == highNum)
                            bookList.Add(book);
                    }
                }
            }

            return bookList;
        }

        private List<catalogBook> getDataHelper(DateTime low, DateTime high, bool lesserThan, bool GreaterThan)
        {
            List<catalogBook> bookList = new List<catalogBook>();

            using (FileStream fs = new FileStream("Books.xml", FileMode.Open))
            {
                XmlSerializer xs = new XmlSerializer(typeof(catalog));
                catalog books = (catalog)xs.Deserialize(fs);

                if (lesserThan == true && GreaterThan == true)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.publish_date >= low && book.publish_date <= high)
                            bookList.Add(book);
                    }
                }
                else if (lesserThan == true && GreaterThan == false)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.publish_date <= high)
                            bookList.Add(book);
                    }
                }
                else if (lesserThan == false && GreaterThan == true)
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.publish_date >= low)
                            bookList.Add(book);
                    }
                }
                else
                {
                    foreach (catalogBook book in books.book)
                    {
                        if (book.publish_date == low || book.publish_date == high)
                            bookList.Add(book);
                    }
                }
            }

            return bookList;
        }

        private List<catalogBook> Combine(List<List<catalogBook>> lists)
        {
            List<catalogBook> combinedList = new List<catalogBook>();

            if (lists.Count >= 1)
            {
                foreach (catalogBook book in lists.ElementAt(0))
                {
                    combinedList.Add(book);
                }
            }
            for (int i = 1; i < lists.Count; i++)
            {
                combinedList = (from b in combinedList
                                join books in lists.ElementAt(i)
                                on b.title equals books.title
                                select b).Distinct().ToList();
            }

            return combinedList;
        }

        // to do later
        private List<string> getKeywords(string query)
        {
            List<string> keywords = Regex.Matches(query, @"\w+")
                .Cast<Match>()
                .Select(match => match.Value)
                .ToList();

            return keywords;
        }
    }
}

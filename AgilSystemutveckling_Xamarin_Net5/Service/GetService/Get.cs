using AgilSystemutveckling_Xamarin_Net5.Models;
using AgilSystemutveckling_Xamarin_Net5.Pages;
using Dapper;
using MySqlConnector;
using System.Data;
using static AgilSystemutveckling_Xamarin_Net5.Constants.Constant;
using static AgilSystemutveckling_Xamarin_Net5.Methods.Methods;

namespace AgilSystemutveckling_Xamarin_Net5.Service.GetService
{
    // Static class to prevent instantiations of the class - it is only used to provide methods.
    public static class Get
    {
        /*
         * Class set to static to prevent instantiations.
         * Methods used to get lists with data from the database.
         * All the lists contains instances of respective classes found in
         * the Models folder.
         */

        #region Category related

        /// <summary>
        /// Gets all entries from Categories table in database.
        /// </summary>
        /// <returns>A list of all categories in the database.</returns>
        public static List<Categories?>? GetAllCategories()
        {
            // Set sql query to the desired information to be fetched from MySQL database.
            string? sql = @$"SELECT Id, CategoryName
                             FROM Categories";

            // Using statement passing in mySqlConnection with connection string.
            using (var connection = new MySqlConnection(ConnectionString))
            {
                // Open MySQL connection.
                connection.Open();

                // Check if connection state is open.
                if (connection.State == ConnectionState.Open)
                {
                    // Add mysql data to a new list.
                    var categories = connection.Query<Categories?>(sql).ToList();

                    // Close the current connection.
                    connection.Close();

                    // Return list of items.
                    return categories;
                }
            }
            return null;
        }

        #endregion

        #region Subcategory related
        /// <summary>
        /// Gets all entries from SubCategories table in database.
        /// </summary>
        /// <returns>A list of all subcategories in the database.</returns>
        public static List<SubCategories?>? GetAllSubCategories()
        {
            string? sql = @$"SELECT Id, SubCategoryName 
                            FROM SubCategories";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var subCategories = connection.Query<SubCategories?>(sql).ToList();
                    connection.Close();

                    return subCategories;
                }

                return null;
            }
        }

        #endregion

        #region Product related methods

        /// <summary>
        /// Gets all products in a list of instances of Products.
        /// </summary>
        /// <returns>A list of all Products in database.</returns>
        public static List<Products?>? GetAllProducts()
        {
            string? sql = @$"SELECT Products.Id, Products.Title, Products.Description,
                            Authors.AuthorName, Categories.CategoryName, SubCategories.SubCategoryName,
                            Products.UnitsInStock, Products.InStock, Products.ImgUrl, Products.Active
                            FROM Products
                            INNER JOIN Authors ON Products.AuthorId = Authors.Id
                            INNER JOIN Categories ON Products.CategoryId = Categories.Id
                            INNER JOIN SubCategories ON Products.SubCategoryId = SubCategories.Id";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var products = connection.Query<Products?>(sql).ToList();

                    connection.Close();

                    return products;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of the most loaned products, excluding booked events and limited 
        /// by 'limitBy' parameter.
        /// </summary>
        /// <param name="limitBy">The number of popular products to get.</param>
        /// <returns>A list with a specified amount of Products ordered by ID.</returns>
        public static List<Products?>? RecentlyAddedProducts(int limitBy)
        {
            if (limitBy <= 0) { throw new ArgumentOutOfRangeException(nameof(limitBy), "Limit cannot be less than 1."); }

            string? sql = @$"SELECT Products.Id, Products.Title, Products.Description,
                            Authors.AuthorName, Categories.CategoryName, SubCategories.SubCategoryName,
                            Products.UnitsInStock, Products.InStock, Products.ImgUrl
                            FROM Products
                            INNER JOIN Authors on Products.AuthorId = Authors.Id
                            INNER JOIN Categories on Products.CategoryId = Categories.Id
                            INNER JOIN SubCategories on Products.SubCategoryId = SubCategories.Id
                            WHERE Categories.CategoryName != 'Event'
                            ORDER BY Id desc
                            LIMIT {limitBy}";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var products = connection.Query<Products?>(sql).ToList();

                    connection.Close();

                    return products;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets specific product by ID. The object is an instance of the class Product found in Models folder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specified Products object based on argument ID matched with ID in database.</returns>
        public static Products? GetProductById(int id)
        {
            if (id <= 0) { throw new ArgumentOutOfRangeException(nameof(id), "Product ID cannot be less than 1."); }

            string? sql = @$"SELECT Products.Id, Products.Title, Products.Description,
                            Authors.AuthorName, Categories.CategoryName, SubCategories.SubCategoryName,
                            Products.UnitsInStock, Products.InStock, Products.ImgUrl, Products.Active
                            FROM Products
                            INNER JOIN Authors on Products.AuthorId = Authors.Id
                            INNER JOIN Categories on Products.CategoryId = Categories.Id
                            INNER JOIN SubCategories on Products.SubCategoryId = SubCategories.Id
                            WHERE Products.Id = {id}";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var product = connection.QuerySingle<Products?>(sql);

                    connection.Close();

                    return product;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all products from the database with a specified CategoryName.
        /// </summary>
        /// <param name="categoryName">The specified CategoryName string.</param>
        /// <returns>A list of products with a specified CategoryName.</returns>
        public static List<Products?>? GetAllByCategory(string categoryName)
        {
            if (categoryName is null) { throw new ArgumentNullException(nameof(categoryName)); }
            CheckStringFormat(categoryName);

            string? sql = @$"SELECT Products.Id, Products.Title, Products.Description,
				            Authors.AuthorName, Categories.CategoryName, SubCategories.SubCategoryName,
				            Products.UnitsInStock, Products.InStock, Products.ImgUrl
				            FROM Products
                            INNER JOIN Authors on Products.AuthorId = Authors.Id
                            INNER JOIN Categories on Products.CategoryId = Categories.Id
                            INNER JOIN SubCategories on Products.SubCategoryId = SubCategories.Id
                            WHERE Categories.CategoryName = '{categoryName}';";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var products = connection.Query<Products?>(sql).ToList();

                    connection.Close();
                    return products;
                }
            }

            return null;
        }
        #endregion

        #region User related methods
        /// <summary>
        /// Gets all users in a list.
        /// </summary>
        /// <returns>A list of all Users in database.</returns>
        public static List<Users?>? GetAllUsers()
        {
            var sql = @$"Select Users.Id, Users.Username, Users.Password, Users.Address, Users.Blocked, 
                        FirstNames.FirstName, LastNames.LastName, Access.Level
                        FROM Users
                        INNER JOIN FullNames on Users.FullNameId = FullNames.Id
                        INNER JOIN FirstNames on FullNames.FirstNameId = FirstNames.Id
                        INNER JOIN LastNames on FullNames.LastNameId = LastNames.Id
                        INNER JOIN Access on Users.AccessId = Access.Id";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var users = connection.Query<Users?>(sql).ToList();
                    connection.Close();

                    return users;
                }
            }
            return null;
        }
        #endregion

        #region Author related methods
        /// <summary>
        /// Get all authors.
        /// </summary>
        /// <returns>A list of all authors in database.</returns>
        public static List<Authors?>? GetAllAuthors()
        {
            var sql = @$"Select Id, AuthorName 
                                From Authors";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var authors = connection.Query<Authors?>(sql).ToList();
                    connection.Close();

                    return authors;
                }
            }
            return null;
        }
        #endregion

        #region Names related

        /// <summary>
        /// Gets all first names from FirstNames table.
        /// The list is instances of the class FirstNames found in Models folder.
        /// </summary>
        /// <returns>A list of all FirstNames in database.</returns>
        public static List<FirstNames?>? GetAllFirstNames()
        {
            // If someone just wants to search for any user starting with a letter, this will
            // sort based on the first letter of the username.
            var sql = @$"SELECT Id, FirstName
                                FROM FirstNames";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var firstNames = connection.Query<FirstNames?>(sql).ToList();

                    connection.Close();
                    return firstNames;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all last names from LastNames table.
        /// The list is instances of the class LastNames found in Models folder.
        /// </summary>
        /// <returns>A list of all LastNames in database.</returns>
        public static List<LastNames?>? GetAllLastNames()
        {
            // If someone just wants to search for any user starting with a letter, this will
            // sort based on the first letter of the username.
            var sql = @$"Select Id, LastName
                                From LastNames";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var lastName = connection.Query<LastNames?>(sql).ToList();
                    connection.Close();

                    return lastName;
                }
            }
            return null;
        }

        #endregion

        #region Loan related
        /// <summary>
        /// Gets a list of all Histories with action ID 1 (lent to a user).
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of all Histories with ActionId = 1 (lent).</returns>
        public static List<History?>? ActiveLoans(int userId)
        {
            var sql = $@"Select Title, Categories.CategoryName, Datetime, ProductId
                        FROM History
		                INNER JOIN Products on ProductId =  Products.Id
		                INNER JOIN Actions on ActionId = Actions.Id
                        INNER JOIN Categories on CategoryId = Categories.Id
                        INNER JOIN Users on UserId = Users.Id
                        WHERE ActionId = 1 And UserId = {userId}";



            List<History?>? historiesLent = new();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    historiesLent = connection.Query<History?>(sql).ToList();

                    connection.Close();
                }
            }

            List<History?>? historiesReturned = new();

            var sql2 = $@"SELECT Title, Categories.CategoryName, Datetime
                                FROM History
		                        INNER JOIN Products ON ProductId =  Products.Id
		                        INNER JOIN Actions ON ActionId = Actions.Id
                                INNER JOIN Categories ON CategoryId = Categories.Id
                                INNER JOIN Users ON UserId = Users.Id
                                WHERE ActionId = 2 AND UserId ={userId}";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    historiesReturned = connection.Query<History?>(sql2).ToList();

                    connection.Close();


                }
            }

            List<History?>? activeLoans = new();

            bool isReturned;

            foreach (var lent in historiesLent)
            {
                if (lent is null) { throw new NullReferenceException(nameof(lent)); }

                isReturned = false;

                foreach (var returned in historiesReturned)
                {
                    if (returned is null) { throw new NullReferenceException(nameof(returned)); }

                    if (lent.Title == returned.Title && lent.DateTime < returned.DateTime)
                    {
                        isReturned = true;
                        break;
                    }
                }

                // If a product is not returned, add to the list of active loans.
                if (!isReturned) { activeLoans.Add(lent); }
            }

            return activeLoans;
        }
        #endregion

        #region History related
        /// <summary>
        /// Displays a list of all Histories with action ID 7 (Booked to a user) with instances of Histories.
        /// Method to see Active bookings.
        /// </summary>
        /// <returns>A list of all Histories with action id 7 (booked to a user).</returns>
        public static List<History?>? ActiveBookings()
        {
            var sql = $@"SELECT FirstNames.FirstName, LastNames.LastName, Products.Title, History.Datetime, History.ProductId, Actions.Action
                                FROM History
		                        INNER JOIN Products on ProductId =  Products.Id
		                        INNER JOIN Actions on ActionId = Actions.Id
                                INNER JOIN Users on UserId = Users.Id
                                INNER JOIN FullNames on FullNameId = FullNames.Id
                                INNER JOIN FirstNames on FirstNameId = FirstNames.Id
                                INNER JOIN LastNames on LastNameId = LastNames.Id
                                WHERE ActionId = 7";

            List<History?>? historiesBooked = new();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    historiesBooked = connection.Query<History?>(sql).ToList();

                    connection.Close();
                }
            }

            var sqlLent = $@"SELECT Title, Datetime, ProductId
                                    FROM History
		                            INNER JOIN Products ON ProductId =  Products.Id
		                            INNER JOIN Actions ON ActionId = Actions.Id
                                    INNER JOIN Users ON UserId = Users.Id
                                    WHERE ActionId = 1";

            List<History?>? historiesLent = new();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    historiesLent = connection.Query<History?>(sqlLent).ToList();

                    connection.Close();
                }
            }


            List<History?>? activeBookings = new();

            bool isLent;

            foreach (var booked in historiesBooked)
            {
                if (booked is null) { throw new NullReferenceException(nameof(booked)); }
                isLent = false;

                foreach (var lent in historiesLent)
                {
                    if (lent is null) { throw new NullReferenceException(nameof(lent)); }

                    if (booked.Title == lent.Title && booked.DateTime < lent.DateTime)
                    {
                        isLent = true;
                        break;
                    }
                }
                // If a product is not loaned, add to the list of active bookings.
                if (!isLent) { activeBookings.Add(booked); }
            }
            return activeBookings;
        }


        /// <summary>
        /// Gets all histories.
        /// </summary>
        /// <returns></returns>
        public static List<History?>? GetAllHistories()
        {
            string? sql = @$"SELECT History.Id, FirstNames.FirstName, LastNames.LastName, Products.Title,
                            Actions.Action, History.Datetime, Categories.CategoryName
                            FROM History
                            INNER JOIN Users on History.UserId = Users.Id
                            INNER JOIN Actions on History.ActionId = Actions.Id
                            INNER JOIN Products on History.ProductId =  Products.Id
                            INNER JOIN Categories on Products.CategoryId =  Categories.Id
                            INNER JOIN FullNames on Users.FullNameId = FullNames.Id
                            INNER JOIN FirstNames on FullNames.FirstNameId = FirstNames.Id
                            INNER JOIN LastNames on FullNames.LastNameId = LastNames.Id;";


            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var histories = connection.Query<History?>(sql).ToList();
                    connection.Close();

                    return histories;
                }
            }
            return null;
        }


        /// <summary>
        /// Gets all Histories with loans that are overdue.
        /// </summary> set as Loaned (ActionId = 1) that are overdue (based on DateTime difference)
        /// <returns>A list of all Histories set as Loaned (ActionId = 1) that are overdue (based on DateTime difference).</returns>
        public static List<History?>? LateReturns()
        {
            List<History?>? allHistories = GetAllHistories();

            if (allHistories is null) { throw new NullReferenceException(nameof(allHistories)); }

            List<History?>? late = new();

            foreach (History? hist in allHistories)
            {
                if (hist is null) { throw new NullReferenceException(nameof(hist)); }

                // If datetime of history is less than or equal to today -14 days, it is overdue.
                if (hist.DateTime <= DateTime.Today.AddDays(-14))
                {
                    // Add History to the list of Histories that are overdue.
                    late.Add(hist);
                }

            }

            return late;
        }
        #endregion
    }
}
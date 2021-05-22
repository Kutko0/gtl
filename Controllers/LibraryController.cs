using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace GTL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private static string _connString = "Server=DESKTOP-GKCGP3R\\MSSQLSERVER01;Database=GTLDB;User ID=dbo;Integrated Security=true";
        private SqlConnection _conn = new SqlConnection(_connString);
        // Singelton didn't work, probably wrong approach
        // private SqlConnection _conn = ConnInstance.Instance;


        [HttpGet]
        [Route("data/expose/books")]
        public IActionResult GetAllDataExposedForBooks()
        {
            string query = "Select * from Book";
            var Books = QueryExecution(query);

            return Ok(Books);
        }

        [HttpGet]
        [Route("data/expose/members")]
        [Route("data/expose/members/{active}")]
        [Route("data/expose/members/{active}/{hascard}")]
        public IActionResult GetAllDataExposedForMembers(int active = 0, int hascard = 0)
        {
            string query = "Select * from Members ";
            if(active > 0 ) {
                query += "Where ActiveMember = 1 ";
            }
            if(hascard > 0 && active > 0 ) {
                query += "AND CardID IS NOT NULL";
            }else if (hascard > 0 && active == 0 ) {
                query += "Where CardID IS NOT NULL";
            }
            var Members = QueryExecution(query);

            return Ok(Members);
        }

        [HttpGet]
        [Route("data/expose/list")]
        public IActionResult GetAllDataExposedAcquireList()
        {
            string query = "SELECT Distinct ISBN, count(ID) as \"Number of instances\" from AcquireList Group By ISBN";
            var ToAcquire = QueryExecution(query);

            return Ok(ToAcquire);
        }

        [HttpGet]
        [Route("book/{limit}")]
        [Route("book")]
        public IActionResult GetBooksByLimit(int limit = 50)
        {
            string query = "Select TOP(" + limit + ") * from Book";
            return Ok(QueryExecution(query));
        }

        [HttpGet]
        [Route("procedure/booksinfo")]
        public IActionResult GetBooksInfo()
        {
            string query = "EXEC SelectBooksWithInfo;";
            return Ok(QueryExecution(query));
        }

        [HttpGet]
        [Route("procedure/membersinfo")]
        public IActionResult GetMembersInfo()
        {
            string query = "EXEC SelectMembersWithInfo;";
            return Ok(QueryExecution(query));
        }

        [HttpGet]
        [Route("procedure/finishedorders")]
        public IActionResult GetFinishedOrderLastYear()
        {
            string query = "EXEC SelectFinishedOrdersInfoForPastYear;";
            return Ok(QueryExecution(query));
        }

        [HttpGet]
        [Route("topauthorsyear")]
        public IActionResult GetTopAuthors()
        {
            string query = @"Select TOP (3) DTB.FullName, count(DTB.ISBN) as BooksLoaned from
                                (Select ID, concat(Fname,' ', Lname) as FullName, DT.ISBN From AuthorName
                                inner Join 
                                (
                                    SELECT Loan.ISBN, LoanDate, Book.AuthorID from Loan
                                    inner join Book
                                    On Loan.ISBN = Book.ISBN
                                    Where LoanDate >= DATEADD(year, -1, getdate())
                                    ) DT
                                On DT.AuthorID = AuthorName.id
                                ) DTB
                            Group by DTB.FullName
                            Order by BooksLoaned desc";
            return Ok(QueryExecution(query));
        }

        [HttpGet]
        [Route("averagecampus")]
        public IActionResult GetAverageBooksPerYearOnCampus()
        {
            string query = @"Select TOP(10)  DT.AddressLine1,(Count(DT.CampusAddress)) / (Count(DISTINCT DT.CampusAddress)) as AveragePerCampus from 
                            (
                            SELECT Loan.CardID, LoanDate, Members.CampusAddress, Addresses.AddressLine1 from Loan
                                Inner Join Members
                                On Loan.CardID = Members.CardID
                                Inner Join Addresses
                                On Members.CampusAddress = Addresses.ID
                                Where LoanDate >= DATEADD(year, -1, getdate())
                            ) DT
                            Group by DT.AddressLine1
                            Order by AveragePerCampus desc";
            return Ok(QueryExecution(query));
        }


        [HttpGet]
        [Route("lastquarterloaned")]
        public IActionResult GetInfoAboutLastQuarterOfLoanedBooks()
        {
            string query = @"SELECT Datename(QUARTER, DATEADD(month, -3, getdate())) as Quarter,
                                Count(Loan.ID) as NumberOfBooksPerMonth, 
                                Count(Distinct Loan.CardID) as PeopleLoaned, 
                                Count(Distinct Loan.ISBN) DifferentBooks from Loan
                                Where LoanDate >= DATEADD(QUARTER, -1, getdate())";
            return Ok(QueryExecution(query));
        }


        [HttpGet]
        [Route("pendingorders")]
        public IActionResult GetPendingOrdersByDateAsc()
        {
            string query = @"Select ISBN, CardID, RequiredDate from Orders
                                Where LoanID IS NULL 
                                AND RequiredDate IS NOT NULL
                                Order by RequiredDate ASC";
            return Ok(QueryExecution(query));
        }


        [HttpGet]
        [Route("book/tobeordered")]
        public IActionResult GetAmountOfToBeOrderedBooks()
        {
            string query = @"SELECT ISBN, Count(AcquireList.Resolved) as UnresolvedStock from AcquireList
                                where Resolved=0
                                group by ISBN
                                order by UnresolvedStock desc";
            return Ok(QueryExecution(query));
        }

        [HttpPost]
        [Route("loan/insert")]
        public IActionResult PostNewLoan()
        {
            string query = @"INSERT INTO Loan(LoanDate, ReturnDate, CardID, ISBN) 
                            VALUES(@fdate, @ldate, @card, @isbn)";

            using (this._conn)
            {

                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    command.Parameters.AddWithValue("@fdate", DateTime.Today);
                    command.Parameters.AddWithValue("@ldate", DateTime.Today.AddDays(60));
                    command.Parameters.AddWithValue("@card", "22");
                    command.Parameters.AddWithValue("@isbn", "0000565023117");

                    _conn.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
            }
            return Ok("Executed.");
        }


        [HttpPost]
        [Route("loan/update/{id}")]
        public IActionResult PostUpdatedLoan(int id)
        {
            string query = @"UPDATE Loan 
                            SET ActualReturnDate = @adate
                            WHERE ID = @id";

            using (this._conn)
            {

                using (SqlCommand command = new SqlCommand(query, _conn))
                {
                    command.Parameters.AddWithValue("@adate", DateTime.Today.AddDays(10));
                    command.Parameters.AddWithValue("@id", id);

                    _conn.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }
            }
            return Ok("Executed.");
        }

        // HELPER FUNCTIONS
        private string QueryExecution(string query)
        {
            string json;

            using (this._conn)
            {
                SqlCommand oCmd = new SqlCommand(query, this._conn);
                this._conn.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var data = Serialize(oReader);
                    json = JsonConvert.SerializeObject(data, Formatting.Indented);

                    this._conn.Close();
                }
            }

            return json;
        }

        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
    }
}

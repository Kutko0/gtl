using System;
using System.Collections.Generic;
using GTL.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GTL.Tests
{
    public class Tests
    {
        [Fact]
        public void GetFiftyBooksTest()
        {
            var sut = new LibraryController();

            var result = sut.GetBooksByLimit();
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(50, result.Count);
        }

        [Fact]
        public void GetAllBooksTest()
        {
            var sut = new LibraryController();

            List<Dictionary<string, object>> result = sut.GetAllDataExposedForBooks();
            Assert.Equal(10000, result.Count);
        }
    }
}

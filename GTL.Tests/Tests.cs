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

        [Fact]
        public void GetAllUsersTest()
        {
            var sut = new LibraryController();

            List<Dictionary<string, object>> result = sut.GetAllDataExposedForMembers();
            Assert.Equal(9000, result.Count);
        }

        [Fact]
        public void GetAllActiveUsersTest()
        {
            var sut = new LibraryController();

            List<Dictionary<string, object>> result = sut.GetAllDataExposedForMembers(1,0);
            Assert.Equal(4568, result.Count);
        }

        [Fact]
        public void GetAllActiveWithCardUsersTest()
        {
            var sut = new LibraryController();

            List<Dictionary<string, object>> result = sut.GetAllDataExposedForMembers(1,1);
            Assert.Equal(3434, result.Count);
        }

        [Fact]
        public void GetAllWithCardUsersTest()
        {
            var sut = new LibraryController();

            List<Dictionary<string, object>> result = sut.GetAllDataExposedForMembers(0,1);
            Assert.Equal(6798, result.Count);
        }
    }
}

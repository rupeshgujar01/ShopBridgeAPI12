using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBridgeAPI.Controllers;
using ShopBridgeAPI.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShopBridgeAPI.Test
{
    public class ShopBridgeAPIControllerTest
    {
        private readonly ItemController _controller;
        private readonly ItemContext _context;
        public static DbContextOptions<ItemContext> _dbContextOptions;
        public static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = ProductDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";


        public ShopBridgeAPIControllerTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ItemContext>()
                .UseSqlServer(connectionString)
                .Options;
            _context = new ItemContext(_dbContextOptions);
            _controller = new ItemController(_context);
        }

        #region Get All Items.
        [Fact]
        public async void Task_GetItems_Return_OkResult()
        {
            //Act  
            var data = await _controller.GetItems();

            //Assert  
            Assert.IsType<List<ItemDTO>>(data.Value);
        }
        #endregion

        #region Get By Id.
        [Fact]
        public async void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = await _controller.GetItem(43443443);
            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }
        [Fact]
        public async void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var testId = 1;
            // Act
            var okResult = await _controller.GetItem(testId);
            // Assert
            Assert.IsType<ItemDTO>(okResult.Value);
        }
        [Fact]
        public async void GetById_ExistingGIdPassed_ReturnsRightItem()
        {
            // Arrange
            var testId = 1;
            // Act
            var okResult = await _controller.GetItem(testId);
            // Assert
            Assert.IsType<ItemDTO>(okResult.Value);
            Assert.Equal(testId, okResult.Value.Id);
        }
        #endregion

        #region Add Item.
        [Fact]
        public async void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            ItemDTO testItem = new ItemDTO()
            {
                Name = "Guinness Original 6 Pack",
                Description = "Guinness",
                Price = "12"
            };
            // Act
            var createdResponse = await _controller.CreateItem(testItem);
            // Assert
            Assert.IsType<ItemDTO>(((ObjectResult)createdResponse.Result).Value);
        }
        [Fact]
        public async void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new ItemDTO()
            {
                Name = "Guinness Original 6 Pack32",
                Description = "Guinnessadsf",
                Price = "122323"
            };
            
            // Act
            var createdResponse = await _controller.CreateItem(testItem);
            ItemDTO item = ((ObjectResult)createdResponse.Result).Value as ItemDTO;

            // Assert
            Assert.IsType<ItemDTO>(((ObjectResult)createdResponse.Result).Value);
            Assert.Equal("Guinness Original 6 Pack32", item.Name);
        }
        #endregion
    }
}

using FluentAssertions;
using GoogleKeep.Controllers;
using GoogleKeep.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoogleKeep.Tests
{
    public class GoogleKeepTest
    {
        [Fact]
        public void TestGet()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.GetSpecificNote(1)).Returns(GetTestSessionsAsync());
            var controller = new NotesController(mockRepo.Object);
          
            // Act
            var result = controller.GetNote(1);

            var objectresult = result.Result as OkObjectResult;
            var objectvalue = objectresult.Value as Note;

            // Assert
            Assert.Equal(2, objectvalue.ID);
        }

        private async Task<Note> GetTestSessionsAsync()
        {
            var sessions = new Note()
            
            {
                ID = 2,
                Title = "Note 1"
            };
           
            return await Task.FromResult(sessions);
        }
    }
}

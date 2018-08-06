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
        public async void TestGetNoteById()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.GetSpecificNote(1)).Returns(GetTestNoteAsync());
            var controller = new NotesController(mockRepo.Object);
          
            // Act
            var result = await controller.GetNote(1);

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as Note;

            // Assert
            Assert.Equal(3, objectvalue.ID);
        }

        [Fact]
        public void TestGetAllNotes()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.GetAll("",null,"Note 1")).Returns(GetTestNotes());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = controller.GetNote("",null,"Note 1");

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as List<Note>;

            // Assert
            Assert.Equal(2, objectvalue.Count);
        }

        [Fact]
        public void TestPostNote()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
            mockRepo.Setup(repo => repo.AddNote(note)).Returns(GetTestNoteAsync());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = controller.PostNote(note);

            var objectresult = result.IsCompleted;

            // Assert
            Assert.True(objectresult);
        }

        private Task<Note> GetTestNoteAsync()
        {
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
           
            return Task.FromResult(note);
        }

        private IEnumerable<Note> GetTestNotes()
        {
            var notes = new List<Note>
            { 
            { new Note()
                {
                    ID = 2,
                    Title = "Note 1"
                }
            }, new Note()
            {
                ID = 2,
                Title = "Note 1"
            }
            };

            return notes;

        }
    }
}

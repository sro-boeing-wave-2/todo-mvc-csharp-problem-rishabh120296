using FluentAssertions;
using GoogleKeep.Controllers;
using GoogleKeep.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            mockRepo.Setup(repo => repo.GetSpecificNote(3)).Returns(GetSingleNote());
            var controller = new NotesController(mockRepo.Object);
          
            // Act
            var result = await controller.GetNote(3);

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as Note;

            // Assert
            Assert.Equal(3, objectvalue.ID);
            Assert.Equal(200, objectresult.StatusCode);
        }

        [Fact]
        public void TestGetAllNotes()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.GetAll("",null,"Note 1")).Returns(GetSingleNoteListWithoutTask());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = controller.GetNote("",null,"Note 1");

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as List<Note>;

            // Assert
            Assert.Single(objectvalue);
            Assert.Equal("Note 1", objectvalue[0].Title);
            Assert.Equal(200, objectresult.StatusCode);
        }

        [Fact]
        public async void TestPostNote()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
            mockRepo.Setup(repo => repo.AddNote(note)).Returns(GetSingleNote());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.PostNote(note);

            var objectresult = result as CreatedAtActionResult;

            // Assert
            Assert.Equal(201, objectresult.StatusCode);
        }

        [Fact]
        public async void TestDeleteNoteById()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.DeleteNoteById(3)).Returns(GetSingleNote());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteNote(3);

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as Note;

            // Assert
            Assert.Equal(3, objectvalue.ID);
            Assert.Equal(200, objectresult.StatusCode);
        }

        [Fact]
        public async void TestDeleteNoteByTitle()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.DeleteNotesByTitle("Note 1")).Returns(GetSingleNoteList());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteNote("Note 1");

            var objectresult = result as OkObjectResult;
            var objectvalue = objectresult.Value as List<Note>;

            // Assert
            Assert.Single(objectvalue);
            Assert.Equal(200, objectresult.StatusCode);
        }

        [Fact]
        public async void TestEditNote()
        {
            // Arrange
            var mockRepo = new Mock<INotesService>();
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
            mockRepo.Setup(repo => repo.EditNote(note)).Returns(GetSingleNote());
            var controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.PutNote(3, note);

            var objectresult = result as NoContentResult;

            // Assert
            Assert.Equal(204, objectresult.StatusCode);
        }

        private Task<Note> GetSingleNote()
        {
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
           
            return Task.FromResult(note);
        }

        private IEnumerable<Note> GetTestNotesWithoutTask()
        {
            var notes = new List<Note>
            { 
            { new Note()
                {
                    ID = 1,
                    Title = "Note 1"
                }
            }, new Note()
            {
                ID = 2,
                Title = "Note 2"
            }
            };
            return notes;
        }

        private IEnumerable<Note> GetSingleNoteListWithoutTask()
        {
            var note = new List<Note>
            {
            { new Note()
                {
                    ID = 1,
                    Title = "Note 1"
                }
            }
            };
            return note;
        }
        private Task<IEnumerable<Note>> GetTestNotes()
        {
            var notes = new List<Note>
            {
            { new Note()
                {
                    ID = 1,
                    Title = "Note 1"
                }
            }, new Note()
            {
                ID = 2,
                Title = "Note 2"
            }
            };

            return Task.FromResult(notes.AsEnumerable());

        }

        private Task<IEnumerable<Note>> GetSingleNoteList()
        {
            var note = new List<Note>
            {
            { new Note()
                {
                    ID = 1,
                    Title = "Note 1"
                }
            }
            };

            return Task.FromResult(note.AsEnumerable());

        }
    }
}

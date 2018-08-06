//using GoogleKeep.Controllers;
//using GoogleKeep.Models;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace GoogleKeep.Tests
//{
//    public class GoogleKeepTest
//    {
//        [Fact]
//        public async Task TestGet()
//        {
//            Arrange
//           var mockRepo = new Mock<NotesContext>();
//            mockRepo.Setup(repo => repo.ListAsync()).Returns(Task.FromResult(GetTestSessions()));
//            var controller = new NotesController(mockRepo.Object);

//            Act
//           var result = await controller.GetNote();

//            Assert
//            Assert.Equal(2, result.Count());
//        }

//        private List<Note> GetTestSessions()
//        {
//            var sessions = new List<Note>();
//            sessions.Add(new Note()
//            {
//                ID = 1,
//                Title = "Note 1"
//            });
//            sessions.Add(new Note()
//            {
//                ID = 2,
//                Title = "Note 2"
//            });
//            return sessions;
//        }
//    }
//}

using System;
using NSuperTest;
using Xunit;

namespace GoogleKeep.Tests
{
    public class NoteAPITest
    {
        private Server server;

        public NoteAPITest()
        {
            server = new Server("https://localhost:44360");
        }

        [Fact]
        public void ShouldGiveValues()
        {
            server
                .Get("/api/Notes")
                .Expect(200)
                .End();
        }
    }
}

using System.Net.Http.Json;
using static Application.IntegrationTest.Testing;

namespace Application.IntegrationTest.Courses.Commands
{
    public class CreateCourseTests
    {
        [Test]
        public async Task Create_New_Course_Should_Success()
        {
            await RunAsAdmin();
            var response = await Client.PostAsJsonAsync("courses", new {title= "Title", content="Content"});
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.That(response.IsSuccessStatusCode, $"Response {response.StatusCode}: {content}");
            Assert.That(Guid.TryParse(content, out var id), "Response content incorrect, inspect Guid");

            var newCourse = DbContext.Courses.FirstOrDefault(x => x.Id == id);
            
            Assert.That(newCourse, Is.Not.Null);
            Assert.That(newCourse.Title, Is.EqualTo("Title"));
            Assert.That(newCourse.Content, Is.EqualTo("Content"));
        }
    }
}
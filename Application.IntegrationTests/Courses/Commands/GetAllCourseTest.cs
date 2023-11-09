using System.Net.Http.Json;
using Application.Courses.Models;
using Newtonsoft.Json;
using static Application.IntegrationTest.Testing;

namespace Application.IntegrationTest.Courses.Commands
{
    public class GetAllCourseTest
    {
        [Test]
        public async Task Get_All_Courses_Should_Return_A_List_Of_Course_Model()
        {
            // Seeding list of courses
            var courseList = new List<object>()
            {
                new { Title = "Course 1", Content = "Content" },
                new { Title = "Course 2", Content = "Content" }
            };
            foreach (var course in courseList)
            {
                var createCourseResponse = await Client.PostAsJsonAsync("course", course);
                Assert.That(createCourseResponse.IsSuccessStatusCode, $"Cannot create ${JsonConvert.SerializeObject(course)}");
            }

            var res = await Client.GetAsync("courses");
            Assert.That(res.IsSuccessStatusCode);

            var courseViewModel = await res.Content.ReadFromJsonAsync<ICollection<CourseViewModel>>();

            Assert.That(courseViewModel, Is.Not.Null);
            Assert.That(courseViewModel, Has.Count.EqualTo(2), "List of course incorrect");

            var firstCourse = courseViewModel?.First();
            Assert.Multiple(() =>
            {
                Assert.That(firstCourse.Title, Is.EqualTo("Course 1"));
                Assert.That(firstCourse.Content, Is.EqualTo("Content"));
                Assert.That(firstCourse.ViewCount, Is.EqualTo(0));
            });
        }
    }
}
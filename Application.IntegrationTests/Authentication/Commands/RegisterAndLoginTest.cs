using System.Net.Http.Json;
using Newtonsoft.Json;
using static Application.IntegrationTest.Testing;

namespace Application.IntegrationTest.Authentication.Commands
{
    public class RegisterAndLoginTest
    {

        [Test]
        public async Task Register_New_User_Then_Login_Should_Success()
        {
            var registerRes = await Client.PostAsJsonAsync("auth/register",
                new { username = "paneee", password = "123123", firstName = "Nguyen", lastName = "Hoang" });
            Assert.That(registerRes.IsSuccessStatusCode);

            var user = DbContext.Users.FirstOrDefault(x => x.UserName == "paneee");
            Assert.That(user, Is.Not.Null);

            var loginRes = await Client.PostAsJsonAsync("auth/login", new { username = "paneee", password="123123" });
            Assert.That(loginRes.IsSuccessStatusCode);

            var loginResContent = await loginRes.Content.ReadAsStringAsync();
            Assert.That(loginResContent, Is.TypeOf<string>().And.Not.Empty);
        }
    }
}
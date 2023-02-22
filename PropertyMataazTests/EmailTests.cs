using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using PropertyMataaz.Utilities;
using SendGrid;
using Xunit;

namespace PropertyMataazTests
{
    public class EmailTests
    {
        private readonly Mock<IOptions<Globals>> _globals;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<ISendGridClient> _sendGrid;

        public EmailTests()
        {
            _sendGrid = new Mock<ISendGridClient>();
            _env = new Mock<IWebHostEnvironment>();
            _globals = new Mock<IOptions<Globals>>();
        }

        public EmailHandler Subject()
        {
            return new EmailHandler(_globals.Object,_env.Object,_sendGrid.Object);
        }
        [Fact]
        public void ComposeFromTemplateTest()
        {
            var Service = Subject();

            _env.Setup(e => e.WebRootPath).Returns("C:\\Users\\Adelowo Ajibola\\source\\repos\\PropertyMataaz\\PropertyMataaz");

            List<KeyValuePair<string, string>> EmailParameters = new List<KeyValuePair<string, string>>();
            EmailParameters.Add(new KeyValuePair<string, string>("URL", "http://verify.com"));
            var EmailTemplate = Service.ComposeFromTemplate("new-user.html", EmailParameters);

            Assert.NotNull(EmailTemplate);
        }

        [Fact]
        public void SendEmailTest()
        {
            var Service = Subject();

            _globals.Setup(g => g.Value.SenderEmail).Returns("adelowomi@gmail.com");
        }
    }
}

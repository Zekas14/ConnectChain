using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Settings
{
    [Route("api/[controller]")]
    [ApiController]

    public class MailSetting
    {
        public string? Server { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? SenderName { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
    }  
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using ClientServerSyncRabbitMq.Application.DTOs;
using ClientServerSyncRabbitMq.Domain.Entities;

namespace ClientServerSyncRabbitMq.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ServerDbContext _serverDbContext;
        private readonly RabbitMQSender _rabbitMQSender;
        private readonly RabbitMQReceiver _rabbitMQReceiver;
        private readonly ClientDbContext _clientDbContext;
        private readonly IMapper _mapper; // افزودن IMapper برای استفاده از AutoMapper

        public UserController(ILogger<UserController> logger, RabbitMQReceiver rabbitMQReceiver, ServerDbContext serverDbContext, RabbitMQSender rabbitMQSender, ClientDbContext clientDbContext, IMapper mapper)
        {
            _logger = logger;
            _rabbitMQReceiver = rabbitMQReceiver;
            _serverDbContext = serverDbContext;
            _rabbitMQSender = rabbitMQSender;
            _clientDbContext = clientDbContext;
            _mapper = mapper; // تزریق IMapper
        }

        // ارسال پیام به سرور از سمت کلاینت (UserController)
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto CreateUserDto)
        {
            // استفاده از AutoMapper برای تبدیل CreateUserDto به User
            var user = _mapper.Map<User>(CreateUserDto);

            _clientDbContext.Users.Add(user);
            await _clientDbContext.SaveChangesAsync();

            var message = JsonConvert.SerializeObject(new
            {
                user.SyncGuid,
                user.Name,
                user.Email
            });

            _rabbitMQSender.SendMessage("ClientToServerQueue", message);

            return Ok(user);
        }
    }
}

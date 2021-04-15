using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using freelancerzy.Hubs;
using freelancerzy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace freelancerzy.Controllers
{
    [Authorize(AuthenticationSchemes = "CookieAuthentication")]
    public class ChatController : Controller
    {
        private readonly cb2020freedbContext _context;

        public ChatController(cb2020freedbContext context)
        {
            _context = context;
        }

        public async Task  <IActionResult> Index()
        {
            var currUser = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress == HttpContext.User.Identity.Name);
            var chats = await _context.Chats.Include(c => c.Messages)
                .Where(c => c.ChatUsers.FirstOrDefault(c => c.UserId == currUser.Userid) != null).ToListAsync();
            List<UserWithLastMessage> userWithLastMessages = new List<UserWithLastMessage>();
            foreach(var chat in chats)
            {
                var userTo = await _context.ChatUsers.Include(c => c.User)
                    .Where(c => c.ChatId == chat.Id).FirstOrDefaultAsync(c => c.UserId != currUser.Userid);
                var lastMessage = chat.Messages.OrderByDescending(m => m.Date).Take(1).First();
                userWithLastMessages.Add(new UserWithLastMessage()
                {
                    ChatId = chat.Id,
                    User = userTo.User,
                    LastMessage = lastMessage
                });
            }
            return View(userWithLastMessages);
        }

        // GET: ChatController/Create
        public async Task<IActionResult> Message(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var user = await _context.PageUser.FirstOrDefaultAsync(u => u.Userid == id);
            if(user == null)
            {
                return NotFound();
            }
            Message message = new Message();
            message.UserTo = user;
            message.UserToId = user.Userid;
            return View(message);
        }

        // POST: ChatController1cs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Message(Message message)
        {
            if (ModelState.IsValid)
            {
                message.Date = DateTime.Now;
                message.UserFrom = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress == HttpContext.User.Identity.Name);
                message.Status = "Wysłana";
                //first user chats
                var chats = await _context.ChatUsers.Where(user => user.UserId == message.UserFrom.Userid).Select(c => c.ChatId).ToListAsync();
                var commonChat = await _context.ChatUsers.Include(c => c.Chat).FirstOrDefaultAsync(c => chats.Contains(c.ChatId) && c.UserId == message.UserToId);
                if (commonChat == null)
                {
                    Chat newChat = new Chat();
                    ChatUser chatUser1 = new ChatUser()
                    {
                        Chat = newChat,
                        UserId = message.UserFrom.Userid
                    };
                    ChatUser chatUser2 = new ChatUser()
                    {
                        Chat = newChat,
                        UserId = message.UserToId
                    };
                    message.Chat = newChat;
                    _context.Add(newChat);
                    _context.Add(chatUser1);
                    _context.Add(chatUser2);
                    
                 }
                else
                {

                    message.Chat = commonChat.Chat;
                }
                _context.Message.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction("Chat", new { chatId = message.ChatId });
            }
            else return View();
        }


        public async Task<IActionResult> Chat(int chatId)
        {
            var currentUser = await _context.PageUser.FirstOrDefaultAsync(u => u.EmailAddress
            == HttpContext.User.Identity.Name);
            var chat = await _context.Chats.Include(c => c.ChatUsers).FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null) return NotFound();
            var Chatuserto = await _context.ChatUsers.Include(u => u.User).
                FirstOrDefaultAsync(u => u.ChatId == chatId && u.UserId != currentUser.Userid);
            var userTo = Chatuserto.User;
            var userId = userTo.Userid;
            var messages = await _context.Message.Include(m => m.UserFrom).Include(m => m.UserFrom)
                .Where(m => m.ChatId == chatId).ToListAsync();
            if (messages.Count == 0) return RedirectToAction("Message", new { id = userId });
            foreach(var message in messages)
            {
                message.Seen = true;
                message.Status = "Wyświetlona";
                _context.Update(message);
            }
            await _context.SaveChangesAsync();
            ViewBag.chatId = chatId;
            ViewBag.CurrentUserName = currentUser.EmailAddress;
            ViewBag.UserToId = userId;
            ViewBag.UserToName = userTo.EmailAddress;
            ViewBag.Username = userTo.FirstName +" "+ userTo.Surname;
            return View(messages);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Message message,[FromServices] IHubContext<ChatHub> chat)
        {
            if (ModelState.IsValid)
            {
                message.UserFrom = await _context.PageUser.FirstOrDefaultAsync
                    (u => u.EmailAddress == HttpContext.User.Identity.Name);
                message.Date = DateTime.Now;
                message.Seen = false;
                message.Status = "Wysłana";

                await _context.AddAsync(message);
                await _context.SaveChangesAsync();

                await chat.Clients.Group(message.ChatId.ToString())
                    .SendAsync("RecieveMessage", new
                    {
                        text = message.Content,
                        userName = message.UserFrom.EmailAddress,
                        date = message.Date
                    });

                return Ok();
            }
            return BadRequest();
        }

       
    }
}

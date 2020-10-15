using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace CrocCSarpBot
{
    /// <summary>
    /// Основной модуль бота
    /// </summary>
    class Bot
    {
        /// <summary>
        /// Клиент Telegram
        /// </summary>
        private TelegramBotClient client;
        /// <summary>
        /// Ведение журнала событий
        /// </summary>
        private NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public Bot()
        {
            // Создание клиента для Telegram
            var token = Properties.Settings.Default.Token;
            client = new TelegramBotClient(token);
            var user = client.GetMeAsync();
            var userResult = user.Result;
            client.OnMessage += MessageProcessor;
        }

        /// <summary>
        /// Обработчик команд
        /// </summary>
        /// <param name="message"></param>
        private void CommandProcessor(Telegram.Bot.Types.Message message)
        {
            string command = message.Text.Substring(1).ToLower();

            switch (command)
            {
                case "start":
                    var button = new KeyboardButton("Поделись телефоном");
                    button.RequestContact = true;
                    var array = new KeyboardButton[] {button};
                    var reply = new ReplyKeyboardMarkup(array, true, true);
                    client.SendTextMessageAsync(message.Chat.Id, $"Привет, {message.Chat.FirstName}, скажи мне свой телефон", replyMarkup: reply);

                    break;
                default:
                    client.SendTextMessageAsync(message.Chat.Id, $"Я пока не понимаю команду {command}");
                    break;
            }
        }

        /// <summary>
        /// Обработка входящего сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageProcessor(object sender, MessageEventArgs e)
        {
            try
            {


                _log.Trace("|<- MessageProcessor");
                switch (e.Message.Type)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Unknown:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Text:
                        if (e.Message.Text.Substring(0, 1) == "/")
                        {
                            CommandProcessor(e.Message);
                        }
                        else
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты мне сказал: {e.Message.Text}");
                            _log.Trace(e.Message.Text);
                        }

                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Photo:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Audio:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Video:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Voice:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Document:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Sticker:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Location:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Contact:
                        var phone = e.Message.Contact.PhoneNumber;
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Твой телефон: {phone}");
                        _log.Trace(phone);
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Venue:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Game:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.VideoNote:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Invoice:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.SuccessfulPayment:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.WebsiteConnected:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatMemberLeft:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatTitleChanged:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatPhotoChanged:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MessagePinned:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChatPhotoDeleted:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.GroupCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.SupergroupCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.ChannelCreated:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MigratedToSupergroup:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.MigratedFromGroup:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Animation:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Poll:
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Dice:
                        var diceMessage = client.SendDiceAsync(e.Message.Chat.Id).Result;
                        if (e.Message.Dice.Value > diceMessage.Dice.Value)
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты выиграл!");
                        }

                        if (e.Message.Dice.Value < diceMessage.Dice.Value)
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты проиграл(");
                        }

                        if (e.Message.Dice.Value == diceMessage.Dice.Value)
                        {
                            client.SendTextMessageAsync(e.Message.Chat.Id, $"Ничья. Кидай еще раз!");
                        }

                        break;
                    default:
                        client.SendTextMessageAsync(e.Message.Chat.Id, $"Ты прислал мне: {e.Message.Type}. Я такой тип не понимаю.");
                        _log.Info(e.Message.Text);
                        break;
                }
            }
            catch (Exception ex)
            {
                _log.Warn(ex);
            }
            finally
            {

            }
            _log.Trace("|-> MessageProcessor");
        }

        public void Run()
        {
            // Запуск приема сообщений
            client.StartReceiving();
        }
    }
}

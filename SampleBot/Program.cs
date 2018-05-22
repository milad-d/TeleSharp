using System;
using System.Collections.Generic;
using System.IO;
using TeleSharp.Entities;
using TeleSharp.Entities.Inline;
using TeleSharp.Entities.SendEntities;

namespace SampleBot
{
    internal class Program
    {
        public static TeleSharp.TeleSharp Bot;

        private static void Main()
        {
            Bot = new TeleSharp.TeleSharp("152529427:AAFOizfzWycWHnJoQghmRAbN5IlBInd-wSe8");
            Bot.SendMessage(new SendMessageParams
            {
                ChatId = "39699831",
                Text = "Test msg !",
                InlineKeyboard = new InlineKeyboardMarkup
                {
                    InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton>
                        {
                            new InlineKeyboardButton
                            {
                                Text = "CallbackData",
                                CallbackData = "Ok",
                                SwitchInlineQuery = string.Empty,
                                SwitchInlineQueryCurrentChat = string.Empty,
                                Url = string.Empty
                            },
                        },
                        new List<InlineKeyboardButton>
                        {
                            new InlineKeyboardButton
                            {
                                Text = "SwitchInlineQueryCurrentChat",
                                CallbackData = string.Empty,
                                SwitchInlineQuery = string.Empty,
                                SwitchInlineQueryCurrentChat = "OK",
                                Url = string.Empty
                            },
                        },
                        new List<InlineKeyboardButton>
                        {
                            new InlineKeyboardButton
                            {
                                Text = "Url",
                                CallbackData = string.Empty,
                                SwitchInlineQuery = string.Empty,
                                SwitchInlineQueryCurrentChat = string.Empty,
                                Url = "http://dualp.ir"
                            },
                        },
                        new List<InlineKeyboardButton>
                        {
                            new InlineKeyboardButton
                            {
                                Text = "SwitchInlineQuery",
                                SwitchInlineQuery = "سلام",
                                Url = string.Empty,
                                CallbackData = string.Empty,
                                SwitchInlineQueryCurrentChat = string.Empty
                            }
                        }
                    }
                }
            });
            Bot.OnMessage += MessageReader.OnMessage;
            Bot.OnInlineQuery += OnInlineQuery;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;

            Console.WriteLine(@"TeleSharp initialized");
            Console.WriteLine($"Hi, My Name is : {Bot.Me.Username}");
            Console.ReadLine();
        }

        private static void Bot_OnCallbackQuery(CallbackQuery callbackQuery)
        {
            Bot.AnswerCallbackQuery(callbackQuery, "Hello");
            Bot.EditMessageText(new SendMessageParams
            {
                ChatId = callbackQuery.Message.Chat.Id.ToString(),
                MessageId = callbackQuery.Message.MessageId.ToString(),
                Text = callbackQuery.Message.Text,
                InlineKeyboard = new InlineKeyboardMarkup
                {
                    InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton>
                        {
                            new InlineKeyboardButton
                            {
                                Text = "Test",
                                CallbackData = "OK",
                                SwitchInlineQuery = string.Empty,
                                SwitchInlineQueryCurrentChat = string.Empty,
                                Url = string.Empty
                            }
                        }
                    }
                }
            });
        }

        private static void OnInlineQuery(InlineQuery inlinequery)
        {
            Bot.AnswerInlineQuery(new AnswerInlineQuery
            {
                InlineQueryId = inlinequery.Id,
                Results = new List<InlineQueryResult>
                {
                    new InlineQueryResultArticle
                    {
                        Id = inlinequery.Query,
                        Title = DateTime.Now.ToLongDateString(),
                        MessageText = Guid.NewGuid().ToString(),
                        ParseMode = "",
                        Url = "",
                        DisableWebPagePreview = false,
                        Description = "",
                        HideUrl = false,
                        ThumbHeight = 0,
                        ThumbWidth = 0,
                        ThumbUrl = ""
                    }
                },
                IsPersonal = false,
                CacheTime = 300,
                NextOffset = "0"
            });
        }
    }
}

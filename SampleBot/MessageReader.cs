using System;
using System.Collections.Generic;
using System.IO;
using TeleSharp.Entities;
using TeleSharp.Entities.SendEntities;

namespace SampleBot
{
    public class MessageReader
    {
        /// <summary>
        /// Read received messages of bot in infinity loop
        /// </summary>
        public static void OnMessage(Message message)
        {
            // Get mesage sender information
            var sender = (MessageSender) message.Chat ?? message.From;

            Console.WriteLine(message.Text ?? "");
            // If user joined to bot say welcome
            if ((!string.IsNullOrEmpty(message.Text)) && (message.Text == "/start"))
            {
                string welcomeMessage =
                    $"Welcome {message.From.Username} !{Environment.NewLine}My name is {Program.Bot.Me.Username}{Environment.NewLine}I made using TeleBot : http://www.github.com/Fel0ny/TeleSharp";

                Program.Bot.SendMessage(new SendMessageParams
                {
                    ChatId = sender.Id.ToString(),
                    Text = welcomeMessage
                });
                return;
            }

            var baseStoragePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            // If any file exists in message download it
            DownloadFileFromMessage(message, baseStoragePath);
            // If Send Location or Contact
            GetLocationContactFromMessage(message, sender);

            if (string.IsNullOrEmpty(message.Text) || string.IsNullOrEmpty(baseStoragePath))
                return;

            try
            {
                var sampleData = Path.Combine(baseStoragePath, "SampleData");

                if (!string.IsNullOrEmpty(message.Text))
                    switch (message.Text.ToLower())
                    {
                        case "time":
                        {
                            Program.Bot.SendMessage(new SendMessageParams
                            {
                                ChatId = sender.Id.ToString(),
                                Text = DateTime.Now.ToLongDateString()
                            });
                            break;
                        }

                        case "location":
                        {
                            Program.Bot.SendLocation(sender, "50.69421", "3.17456");
                            break;
                        }

                        case "sticker":
                        {
                            Program.Bot.SendSticker(sender,
                                System.IO.File.ReadAllBytes(Path.Combine(sampleData, "sticker.png")));
                            break;
                        }

                        case "photo":
                        {
                            var photoFilePath = Path.Combine(sampleData, "sticker.png");

                            Program.Bot.SetCurrentAction(sender, ChatAction.UploadPhoto);
                            Program.Bot.SendPhoto(sender,
                                System.IO.File.ReadAllBytes(photoFilePath),
                                Path.GetFileName(photoFilePath),
                                "This is sample photo");
                            break;
                        }

                        case "video":
                        {
                            var videoFilePath = Path.Combine(sampleData, "video.mp4");

                            Program.Bot.SetCurrentAction(sender, ChatAction.UploadVideo);
                            Program.Bot.SendVideo(sender,
                                System.IO.File.ReadAllBytes(videoFilePath),
                                Path.GetFileName(videoFilePath),
                                "This is sample video");
                            break;
                        }

                        case "audio":
                        {
                            var audioFilePath = Path.Combine(sampleData, "audio.mp3");

                            Program.Bot.SetCurrentAction(sender, ChatAction.UploadAudio);
                            Program.Bot.SendAudio(sender,
                                System.IO.File.ReadAllBytes(audioFilePath),
                                Path.GetFileName(audioFilePath));
                            break;
                        }

                        case "document":
                        {
                            var documentFilePath = Path.Combine(sampleData, "Document.txt");

                            Program.Bot.SetCurrentAction(sender, ChatAction.UploadDocument);
                            Program.Bot.SendDocument(sender,
                                System.IO.File.ReadAllBytes(documentFilePath),
                                Path.GetFileName(documentFilePath));
                            break;
                        }

                        case "keyboard":
                        {
                            Program.Bot.SendMessage(new SendMessageParams
                            {
                                ChatId = sender.Id.ToString(),
                                Text = "This is sample keyboard :",
                                CustomKeyboard = new ReplyKeyboardMarkup
                                {
                                    Keyboard = new List<List<KeyboardButton>>
                                    {
                                        new List<KeyboardButton>
                                        {
                                            new KeyboardButton
                                            {
                                                Text = "send location",
                                                RequestContact = false,
                                                RequestLocation = true
                                            }
                                            ,
                                            new KeyboardButton
                                            {
                                                Text = "cancel",
                                                RequestContact = false,
                                                RequestLocation = false
                                            }

                                        }
                                    },
                                    ResizeKeyboard = true
                                },
                                ReplyToMessage = message
                            });

                            break;
                        }
                        case "cancel":
                        {
                            Program.Bot.SendMessage(new SendMessageParams
                            {
                                ChatId = sender.Id.ToString(),
                                Text = $"You choose keyboard command : {message.Text}",
                            });
                            break;
                        }

                        default:
                        {
                            Program.Bot.SendMessage(new SendMessageParams
                            {
                                ChatId = sender.Id.ToString(),
                                Text = "Unknown command !",
                            });

                            break;
                        }
                    }
            }
            catch (Exception exception)
            {
                throw new Exception("OnMessage Exception!", exception);
            }
        }

        private static void GetLocationContactFromMessage(Message message, MessageSender sender)
        {
            if (message.Location != null)
            {
                GetLocationFromMessage(message, sender);
            }
            else if (message.Contact != null)
            {
                GetContactFromMessage(message, sender);
            }
        }

        private static void GetContactFromMessage(Message message, MessageSender sender)
        {
            Console.WriteLine(
                $"Contact :({message.Contact.FirstName},{message.Contact.LastName},{message.Contact.PhoneNumber})");
            Program.Bot.SendMessage(new SendMessageParams
            {
                ChatId = sender.Id.ToString(),
                Text = "You Send Contact",
                ReplyToMessage = message
            });
        }

        private static void GetLocationFromMessage(Message message, MessageSender sender)
        {
            Console.WriteLine($"Location :({message.Location.Latitude},{message.Location.Longitude})");
            Program.Bot.SendMessage(new SendMessageParams
            {
                ChatId = sender.Id.ToString(),
                Text = "You Send location",
                ReplyToMessage = message
            });
        }

        public static void DownloadFileFromMessage(Message message, string savePath)
        {
            savePath = GetSavePath(message, savePath);

            var fileInfo = GetFileInfo(message, savePath);
            fileInfo = AttachVideoToFile(message, savePath, fileInfo);
            fileInfo = AttachAudioToFile(message, savePath, fileInfo);
            fileInfo = AttachImageToFile(message, savePath, fileInfo);
            fileInfo = AttachStickerToFile(message, savePath, fileInfo);

            if (fileInfo != null)
                Console.WriteLine($"File : {fileInfo.FilePath} Size : {fileInfo.FileSize} was downloaded successfully");
        }

        private static FileDownloadResult AttachStickerToFile(
            Message message,
            string savePath,
            FileDownloadResult fileInfo)
        {
            if (message.Sticker != null)
                fileInfo = Program.Bot.DownloadFileById(message.Sticker.FileId, savePath);
            return fileInfo;
        }

        private static FileDownloadResult AttachImageToFile(
            Message message,
            string savePath,
            FileDownloadResult fileInfo)
        {
            if (message.Photo != null)
                foreach (var photoSize in message.Photo)
                    fileInfo = Program.Bot.DownloadFileById(photoSize.FileId, savePath);

            return fileInfo;
        }

        private static FileDownloadResult AttachAudioToFile(
            Message message,
            string savePath,
            FileDownloadResult fileInfo)
        {
            if (message.Audio != null)
                fileInfo = Program.Bot.DownloadFileById(message.Audio.FileId, savePath);
            return fileInfo;
        }

        private static FileDownloadResult AttachVideoToFile(
            Message message,
            string savePath,
            FileDownloadResult fileInfo)
        {
            if (message.Video != null)
                fileInfo = Program.Bot.DownloadFileById(message.Video.FileId, savePath);
            return fileInfo;
        }

        private static string GetSavePath(Message message, string savePath)
        {
            savePath = Path.Combine(savePath, "Storage");
            savePath = Path.Combine(savePath, message.From.Username ?? message.From.Id.ToString());
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            return savePath;
        }

        private static FileDownloadResult GetFileInfo(Message message, string savePath)
        {
            FileDownloadResult fileInfo = null;
            if (message.Document != null)
                fileInfo = Program.Bot.DownloadFileById(message.Document.FileId, savePath);
            return fileInfo;
        }
    }
}
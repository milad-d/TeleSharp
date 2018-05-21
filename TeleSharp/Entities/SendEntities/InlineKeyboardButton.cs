namespace TeleSharp.Entities.SendEntities
{
    public class InlineKeyboardButton
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string CallbackData { get; set; }
        public string SwitchInlineQuery { get; set; }
        public string SwitchInlineQueryCurrentChat { get; set; }
    }
}

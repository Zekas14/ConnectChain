namespace ConnectChain.ViewModel.Notification.GetNotification
{
    public class GetNotificationResponseViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public bool IsRead { get; set; }
        public string? Date { get; set; }
        public string? Type { get; set; }
    }
}

namespace Common
{
    public class Response
    {
        public Response(string status, string message, int totalRecords, string result)
        {
            TotalRecords = totalRecords;
            Status = status;
            Message = message;
            Result = result;
        }
        public Response(string status, string message, string result)
        {
            Status = status;
            Message = message;
            Result = result;
        }
        public Response(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public Response()
        {
        }

        public int TotalRecords { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }

    }

    public class ReferralReward
    {
        public int Id { get; set; }
        public int ReferralPoint { get; set; }
        public string Reward { get; set; }
    }

    //public class ReferralRewardSlab
    //{
    //    public Guid Id { get; set; }
    //    public int ReferralPoint { get; set; }
    //    public string? RewardName { get; set; }
    //    public string? RewardImage { get; set; }
    //    public DateTime? StartDate { get; set; }
    //    public DateTime? EndDate { get; set; }
    //    public int Quantity { get; set; }
    //    public int SelectedQuantity { get; set; }
    //}
}
namespace idk.Models
{
    public class UserPic
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public byte[] PicData { get; set; }

        private string picName;
        public string PicName
        {
            get => picName ?? string.Empty;
            set
            {
                picName = value;
                PicFormat = value[(value.LastIndexOf('.') + 1)..];
            }
        }
        public string PicFormat { get; set; }

        public string ConvertToHtmlBase64String()
            => $"data:image/{PicFormat};base64,{Convert.ToBase64String(PicData, 0, PicData.Length)}";
    }
}

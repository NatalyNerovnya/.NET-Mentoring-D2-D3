namespace QueueClient
{
    public class PdfMessage
    {
        public byte[] Data { get; set; }
        public int Size { get; set; }
        public int Position { get; set; }
    }

}

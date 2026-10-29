namespace webdammay.dammay.Domain.Entities
{
    public class StoredFile
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public long FileSize { get; private set; }
        public string UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public StoredFile(string fileName, string filePath, long fileSize, string userId)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Tên file không được trống.");
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Đường dẫn file không được trống.");
           
            Id = Guid.NewGuid();
            FileName = fileName;
            FilePath = filePath;
            FileSize = fileSize;
            UserId = userId;
            CreatedAt = DateTime.Now;
        }
        public void UpdateFileName(string newFileName)
        {
            if(string.IsNullOrWhiteSpace(newFileName))
                throw new ArgumentException("Tên file mới không được để trống.");
            FileName = newFileName.Trim();

        }
        private StoredFile() { }
    }
}

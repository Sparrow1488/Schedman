namespace Schedman.Tools.IO
{
    public class SaveFileInfo
    {
        public string Extension { get; private set; }
        public string FileName { get; private set; }
        public string FullFileName { get; private set; }

        public static SaveFileInfo Name(string name)
        {
            var saveFileInfo = new SaveFileInfo();
            saveFileInfo.FileName = name;
            return saveFileInfo;
        }

        public SaveFileInfo Jpeg()
        {
            Extension = ".jpeg";
            return this;
        }

        public SaveFileInfo Jpg()
        {
            Extension = ".jpg";
            return this;
        }

        public SaveFileInfo Png()
        {
            Extension = ".png";
            return this;
        }

        public SaveFileInfo Mp4()
        {
            Extension = ".mp4";
            return this;
        }

        public SaveFileInfo Mp3()
        {
            Extension = ".mp3";
            return this;
        }
    }
}

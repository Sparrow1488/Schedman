namespace VideoSchedman.Entities
{
    public class OutputAdditionalSettings
    {
        public OutputAdditionalSettings(string originalSource)
        {
            OriginalSource = originalSource;
            _manipulations = new List<FileManipulation>();
        }

        public OutputAdditionalSettings(string originalSource, IEnumerable<FileManipulation> manipulations) : this(originalSource)
        {
            AddSettingsRange(manipulations);
        }

        public string OriginalSource { get; private set; }
        public IEnumerable<FileManipulation> Manipulations { get => _manipulations; }
        private List<FileManipulation> _manipulations;

        public void AddSettings(FileManipulation manipulation)
        {
            if (manipulation != null)
                _manipulations.Add(manipulation);
        }

        public void AddSettingsRange(IEnumerable<FileManipulation> manipulations)
        {
            if (manipulations != null)
                _manipulations.AddRange(manipulations);
        }
    }

    public class FileManipulation
    {
        public int Loop { get; set; }
    }
}

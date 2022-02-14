﻿using System.Text;
using VkSchedman.Video.Abstractions;

namespace VkSchedman.Video.Options
{
    public class InputOptions : IInputOptions
    {
        public TimeSpan _startPosition;
        public TimeSpan _endPosition;
        private StringBuilder _builder = new StringBuilder();
        private List<string> _inputSourceList = new List<string>();
        private List<string> _additionalCommands = new List<string>();
        private InputOptionsSettings _settings = new InputOptionsSettings();

        public void AddSource(string sourcePath)
        {
            if(string.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException($"{nameof(sourcePath)} cannot be empty or null!");
            _inputSourceList.Add(sourcePath);
        }

        public void AddCommand(string command)
        {
            _additionalCommands.Add(command);
        }

        public void SetStartPosition(TimeSpan start) => _startPosition = start;
        public void SetEndPosition(TimeSpan end) => _endPosition = end;
        public IEnumerable<string> GetSources() => _inputSourceList;
        public InputOptionsSettings GetOptionsSettings() => _settings;

        public string Build()
        {
            var commands = new List<string>();
            if(_startPosition > TimeSpan.Zero)
                commands.Add($"-ss {_startPosition}");
            if(_endPosition > TimeSpan.Zero)
                commands.Add($"-t {_endPosition}");
            if (_settings.IsCombineSourcesInTxt)
            {
                var txtPath = _settings.CombineSourcesInTxt(_inputSourceList);
                commands.Add($"-i \"{txtPath}\"");
            }
            else
                _inputSourceList.ForEach(src => commands.Add($"-i \"{src}\""));

            string space = " ";
            _additionalCommands.ForEach(cmd => _builder.Append(cmd + space));
            commands.ForEach(cmd => _builder.Append(cmd + space));

            return _builder.ToString().Trim();
        }

        
    }
}

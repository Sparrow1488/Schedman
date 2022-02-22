namespace VideoSchedman.Enums
{
    public class ConcatType
    {
        private ConcatType(string type)
        {
            _type = type;
        }

        private string _type = string.Empty;

        public static readonly ConcatType ReencodingConcat = new ConcatType(nameof(ReencodingConcat));
        public static readonly ConcatType ReencodingComplexFilter = new ConcatType(nameof(ReencodingComplexFilter));
        public static readonly ConcatType Demuxer = new ConcatType(nameof(Demuxer));

        public override string ToString() => _type;
        public override bool Equals(object? obj)
        {
            bool result = false;
            if (obj is ConcatType input)
                if (input._type == _type)
                    result = true;
            return result;
        }
    }
}

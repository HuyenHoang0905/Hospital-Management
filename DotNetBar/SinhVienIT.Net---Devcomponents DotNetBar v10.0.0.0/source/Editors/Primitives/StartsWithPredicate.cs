#if FRAMEWORK20
using System;
using System.Collections.Generic;
using System.Text;

namespace DevComponents.Editors.Primitives
{
    internal class StartsWithPredicate
    {
        private string _Prefix;
        private int _MaxMatches = 0;
        private int _MatchCount = 0;

        // Initializes with prefix we want to match.
        public StartsWithPredicate(string prefix)
        {
            _Prefix = prefix;
        }

        // Initializes with prefix we want to match.
        public StartsWithPredicate(string prefix, int maxMatches)
        {
            _Prefix = prefix;
            _MaxMatches = maxMatches;
        }

        // Sets a different prefix to match.
        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        public void ResetMatchCount()
        {
            _MatchCount = 0;
        }

        public int MaxMatches
        {
            get { return _MaxMatches; }
            set
            {
                _MaxMatches = value;
            }
        }

        // Gets the predicate.
        public Predicate<string> Match
        {
            get { return IsMatch; }
        }

        private bool IsMatch(string s)
        {
            return s.ToLower().StartsWith(_Prefix);
        }

        public Predicate<string> MatchTop
        {
            get
            {
                return IsMatchTop;
            }
        }

        private bool IsMatchTop(string s)
        {
            if (_MatchCount > _MaxMatches) return false;
            bool b = s.ToLower().StartsWith(_Prefix);
            if (b && _MaxMatches > 0) _MatchCount++;
            return b;
        }
    }
}
#endif


using System;
using System.Text;

namespace DevComponents.DotNetBar
{
#if FRAMEWORK20
    internal static class StringHelper
#else
	internal class StringHelper
#endif
    {
        internal static string Capitalize(string s)
        {
            if (s.Length > 1)
                return s.Substring(0, 1).ToUpper() + s.Substring(1);
            return s;
        }

        internal static string GetFriendlyName(string fieldName)
        {
            fieldName = fieldName.Replace('_', ' ');
#if FRAMEWORK20
            if (fieldName.Contains(" "))
#else
			if (fieldName.IndexOf(" ") >= 0)
#endif
            {
                string[] words = fieldName.Split(' ');
                StringBuilder builder = new StringBuilder(fieldName.Length);
                for (int i = 0; i < words.Length; i++)
                {
                    builder.Append(Capitalize(words[i]));
                    if (i < words.Length - 1)
                        builder.Append(' ');
                }
                return builder.ToString();
            }
            else if (fieldName.Length > 2)
            {
                bool hadUpperCase = false;
                int lastUpperCaseIndex = -1;
                StringBuilder builder = new StringBuilder(fieldName.Length + 10);
                for (int i = 0; i < fieldName.Length; i++)
                {
                    if (IsUpperCase(fieldName[i]))
                    {
                        if (hadUpperCase && i - lastUpperCaseIndex > 1)
                        {
                            builder.Append(' ');
                            builder.Append(fieldName[i]);
                            lastUpperCaseIndex = i;
                        }
                        else if (!hadUpperCase)
                        {
                            lastUpperCaseIndex = i;
                            hadUpperCase = true;
                            if (i > 0) builder.Append(' ');
                            builder.Append(fieldName[i]);
                        }
                        else
                        {
                            lastUpperCaseIndex = i;
                            builder.Append(fieldName[i].ToString().ToLower());
                            hadUpperCase = true;
                        }
                    }
                    else
                    {
                        if (hadUpperCase)
                            builder.Append(fieldName[i]);
                        else
                        {
                            builder.Append(fieldName[i].ToString().ToUpper());
                            hadUpperCase = true;
                            lastUpperCaseIndex = i;
                        }
                    }
                }

                return builder.ToString();
            }

            return fieldName;
        }

        internal static bool IsUpperCase(char c)
        {
            return IsUpperCase(c.ToString());
        }

        internal static bool IsUpperCase(string s)
        {
            return s.ToUpper() == s;
        }

        internal static string Repeat(char repeatCharacter, int count)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(repeatCharacter, count);
            
            return builder.ToString();
        }
    }
}

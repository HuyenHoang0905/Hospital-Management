using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

#if TREEGX
namespace DevComponents.Tree.TextMarkup
#elif DOTNETBAR
namespace DevComponents.DotNetBar.TextMarkup
#elif SUPERGRID
namespace DevComponents.SuperGrid.TextMarkup
#endif
{
    internal class MarkupParser
    {
        #region Private Variables
        private static string TextElementName = "text";
        private static string BodyTag = "body";
        #endregion

        #region Internal Implementation
        public static BodyElement Parse(string text)
        {
            StringBuilder plainText = new StringBuilder(text.Length);
            BodyElement root = new BodyElement();
            root.HasExpandElement = false;
            MarkupElement currentParent = root;
            Stack openTags = new Stack();
            openTags.Push(root);
            // Input text is not wrapped into the container tag so we wrap it here
            text = text.Replace("&nbsp;", "{ent_nbsp}");
            text = text.Replace("&zwsp;", "{ent_zwsp}");
            text = text.Replace("&lt;", "{ent_lt}");
            text = text.Replace("&gt;", "{ent_gt}");
            text = text.Replace("&amp;", "{ent_amp}");
            text = text.Replace("|", "{ent_l}");
            text = text.Replace("&", "|");
            text = text.Replace("{ent_nbsp}", "&nbsp;");
            text = text.Replace("{ent_zwsp}", "&zwsp;"); 
            text = text.Replace("{ent_lt}", "&lt;");
            text = text.Replace("{ent_gt}", "&gt;");
            StringReader sr = new StringReader("<"+BodyTag+">" + text + "</"+BodyTag+">");
#if !DEBUG
            try
#endif
            {
                XmlTextReader reader = new XmlTextReader(sr);
                //reader.EntityHandling = EntityHandling.ExpandCharEntities;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == BodyTag)
                            continue;
                        MarkupElement el = CreateMarkupElement(reader.Name);
                        if (el == null)
                        {
                            reader.Skip();
                            continue;
                        }
                        else if (el is ExpandElement)
                            root.HasExpandElement = true;

                        if (el is IActiveMarkupElement)
                            root.ActiveElements.Add(el);

                        // Parse any attributes here
                        if (reader.AttributeCount > 0)
                        {
                            el.ReadAttributes(reader);
                            reader.MoveToElement();
                        }

                        currentParent.Elements.Add(el);

                        if (el is ContainerElement)
                            currentParent = el;

                        if (!reader.IsEmptyElement)
                            openTags.Push(el);
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        if (reader.Value.Length == 1)
                        {
                            TextElement el = CreateMarkupElement(TextElementName) as TextElement;
                            if (reader.Value == " ")
                            {
                                el.TrailingSpace = true;
                                plainText.Append(' ');
                            }
                            else
                            {
                                el.Text = reader.Value;
                                el.Text = el.Text.Replace("|", "&");
                                el.Text = el.Text.Replace("{ent_l}", "|");
                                el.Text = el.Text.Replace("{ent_amp}", "&&");
                                plainText.Append(el.Text+" ");
                            }
                            currentParent.Elements.Add(el);
                        }
                        else
                        {
                            string s = reader.Value;
                            if (s.StartsWith("\r\n"))
                                s = s.TrimStart(new char[] { '\r', '\n' });
                            s = s.Replace("\r\n", " ");
                            string[] words = s.Split(' ');
                            bool space = false;
                            if (currentParent.Elements.Count > 0 && currentParent.Elements[currentParent.Elements.Count - 1] is NewLine)
                                space = true;
                            for (int i = 0; i < words.Length; i++)
                            {
                                if (words[i].Length == 0)
                                {
                                    if (space)
                                        continue;
                                    space = true;
                                }
                                else
                                    space = false;

                                TextElement el = CreateMarkupElement(TextElementName) as TextElement;
                                el.Text = words[i].Replace("|","&");
                                el.Text = el.Text.Replace("{ent_l}", "|");
                                el.Text = el.Text.Replace("{ent_amp}", "&&");
                                plainText.Append(el.Text + " ");
                                if (i < words.Length - 1)
                                {
                                    el.TrailingSpace = true;
                                    space = true;
                                }

                                currentParent.Elements.Add(el);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Whitespace)
                    {
                        if (reader.Value.IndexOf(' ') >= 0)
                        {
                            TextElement el = CreateMarkupElement(TextElementName) as TextElement;
                            el.TrailingSpace = true;
                            currentParent.Elements.Add(el);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EntityReference)
                    {
                        TextElement el = CreateMarkupElement(TextElementName) as TextElement;
                        if (reader.Name == "nbsp")
                        {
                            el.TrailingSpace = true;
                        }
                        else if (reader.Name == "zwsp")
                        {
                            el.TrailingSpace = false;
                        }
                        else
                            el.Text = reader.Name;
                        el.EnablePrefixHandling = false;
                        currentParent.Elements.Add(el);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        MarkupElement el = openTags.Pop() as MarkupElement;
                        if (el != currentParent)
                        {
                            currentParent.Elements.Add(new EndMarkupElement(el));
                        }
                        else
                        {
                            if (currentParent != root)
                                currentParent = currentParent.Parent;
                        }
                    }
                }
            }
#if !DEBUG
            catch
            {
                return null;
            }
#endif
            root.PlainText = plainText.ToString();
            return root;
        }

        public static MarkupElement CreateMarkupElement(string elementName)
        {
            if (elementName == "b" || elementName == "strong")
                return new Strong();
            else if (elementName == "i" || elementName == "em")
                return new Italic();
            else if (elementName == "u")
                return new Underline();
            else if (elementName == "br")
                return new NewLine();
            else if (elementName == "expand")
                return new ExpandElement();
            else if (elementName == "a")
                return new HyperLink();
            else if (elementName == "p")
                return new Paragraph();
            else if (elementName == "div")
                return new Div();
            else if (elementName == "span")
                return new Span();
            else if (elementName == "img")
                return new ImageElement();
            else if (elementName == "h1")
                return new Heading();
            else if (elementName == "h2")
                return new Heading(2);
            else if (elementName == "h3")
                return new Heading(3);
            else if (elementName == "h4")
                return new Heading(4);
            else if (elementName == "h5")
                return new Heading(5);
            else if (elementName == "h6")
                return new Heading(6);
            else if (elementName == "font")
                return new FontElement();
            else if (elementName == "s" || elementName == "strike")
                return new Strike();
            else if (elementName == TextElementName)
                return new TextElement();

            return null;
        }

        /// <summary>
        /// Tests whether input text could be markup text.
        /// </summary>
        /// <param name="text">Text to test.</param>
        /// <returns>true if text could be markup, otherwise false</returns>
        public static bool IsMarkup(ref string text)
        {
            if (text.IndexOf("</") < 0 && text.IndexOf("/>") < 0)
                return false;
            return true;
        }

        internal static string RemoveExpand(string text)
        {
            return Regex.Replace(text, "<expand.*?>", "", RegexOptions.IgnoreCase);
        }
        #endregion
    }
}

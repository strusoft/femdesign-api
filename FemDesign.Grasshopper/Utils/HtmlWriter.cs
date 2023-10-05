using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace FemDesign.Utils
{
    public sealed class HtmlWriter
    {
        private class Tag
        {
            private string string_0;

            public Tag(string name)
            {
                this.string_0 = name;
            }

            public string CloseTag()
            {
                return this.string_0;
            }
        }

        private StringBuilder stringBuilder_0 = new StringBuilder();

        private Stack<HtmlWriter.Tag> stack_0 = new Stack<HtmlWriter.Tag>(12);

        private string string_0 = "FEM-Design API";


        public bool AutoClose
        {
            get;
            set;
        }

        public string Generator
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public string Title
        {
            get;
            set;
        }

        public HtmlWriter(string string_2, string string_3, HtmlCss htmlCss)
        {
            this.AutoClose = true;
            this.Title = (string_2 ?? "Weaverbird HTML file");
            this.Generator = (string_3 ?? this.string_0);
            this.AddLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">").AddLine();
            this.AddOpenTag("html", "xmlns=\"http://www.w3.org/1999/xhtml\"");
            this.AddOpenTag("head").AddLine();
            this.AddClosedTag("meta", "http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"");
            this.AddClosedTag("title", "", this.Title);
            this.AddClosedTag("meta", "name=\"GENERATOR\" content=\"" + this.Generator + "\"");
            if (htmlCss != null)
            {
                this.AddOpenTag("style", "type=\"text/css\"", "<!--", "-->");
                while (htmlCss.NextExists())
                {
                    this.AddLine(htmlCss.Next());
                }
                this.CloseLastTag();
            }
            this.AddLine().CloseLastTag();
        }

        public static string Concat(string string_2, params string[] string_3)
        {
            if (string_3 != null && string_3.Length != 0)
            {
                StringBuilder stringBuilder = new StringBuilder(string_3[0]);
                for (int i = 1; i < string_3.Length; i++)
                {
                    stringBuilder.Append(string_2).Append(string_3[i]);
                }
                return stringBuilder.ToString();
            }
            return string.Empty;
        }

        public HtmlWriter AddClosedTag(string string_2)
        {
            this.AddLine("<" + string_2 + " />");
            return this;
        }

        public HtmlWriter AddClosedTag(string string_2, string string_3)
        {
            this.AddLine("<" + string_2 + (string.IsNullOrEmpty(string_3) ? " />" : (" " + string_3 + " />")));
            return this;
        }

        public HtmlWriter AddClosedTag(string string_2, string string_3, string string_4)
        {
            this.AddLine(string.Concat(new string[]
            {
                "<",
                string_2,
                string.IsNullOrEmpty(string_3) ? ">" : (" " + string_3 + ">"),
                string_4,
                "</",
                string_2,
                ">"
            }));
            return this;
        }

        public HtmlWriter AddImageTag(Bitmap bitmap_0, string string_2, int int_0, int int_1)
        {
            if (bitmap_0 != null)
            {
                string text;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap_0.Save(memoryStream, ImageFormat.Png);
                    text = Convert.ToBase64String(memoryStream.GetBuffer());
                }
                this.stringBuilder_0.Append("<img src=\"").Append("data:image/png;base64,").Append(text).Append("\"");
                if (!string.IsNullOrEmpty(string_2))
                {
                    this.stringBuilder_0.Append(" alt=\"").Append(string_2).Append("\"");
                }
                if (int_0 > 0)
                {
                    this.stringBuilder_0.Append(" width=\"").Append(int_0).Append("\"");
                }
                if (int_1 > 0)
                {
                    this.stringBuilder_0.Append(" height=\"").Append(int_1).Append("\"");
                }
                this.stringBuilder_0.Append(" />");
                this.AddLine();
            }
            return this;
        }

        public HtmlWriter AddLine(string string_2)
        {
            this.method_1();
            this.stringBuilder_0.AppendLine(string_2);
            return this;
        }

        public HtmlWriter AddLine()
        {
            this.stringBuilder_0.AppendLine();
            return this;
        }

        public HtmlWriter AddOpenTag(string string_2, string string_3, string string_4, string string_5)
        {
            this.AddLine("<" + string_2 + (string.IsNullOrEmpty(string_3) ? ">" : (" " + string_3 + ">")) + string_4);
            this.stack_0.Push(new HtmlWriter.Tag(string_5 + "</" + string_2 + ">"));
            return this;
        }

        public HtmlWriter AddOpenTag(string string_2, string string_3, string string_4)
        {
            return this.AddOpenTag(string_2, string_3, string_4, string.Empty);
        }

        public HtmlWriter AddOpenTag(string string_2, string string_3)
        {
            this.AddLine("<" + string_2 + (string.IsNullOrEmpty(string_3) ? ">" : (" " + string_3 + ">")));
            this.stack_0.Push(new HtmlWriter.Tag("</" + string_2 + ">"));
            return this;
        }

        public HtmlWriter AddOpenTag(string string_2)
        {
            this.AddLine("<" + string_2 + ">");
            this.stack_0.Push(new HtmlWriter.Tag("</" + string_2 + ">"));
            return this;
        }

        public HtmlWriter CloseLastTag()
        {
            HtmlWriter.Tag tag = this.stack_0.Pop();
            this.AddLine(tag.CloseTag());
            return this;
        }

        public override string ToString()
        {
            if (this.AutoClose)
            {
                this.method_0();
            }
            return this.stringBuilder_0.ToString();
        }

        private void method_0()
        {
            while (this.stack_0.Count > 0)
            {
                this.CloseLastTag();
            }
        }

        private void method_1()
        {
            this.stringBuilder_0.Append(' ', this.stack_0.Count);
        }
        public static string ParseTextIntoHtml(string string_2)
        {
            if (string.IsNullOrEmpty(string_2))
            {
                return string.Empty;
            }
            string arg_31_1 = "[\r\n&<>]";
            if (HtmlWriter.matchEvaluator_0 == null)
            {
                HtmlWriter.matchEvaluator_0 = new MatchEvaluator(HtmlWriter.smethod_0);
            }
            return Regex.Replace(string_2, arg_31_1, HtmlWriter.matchEvaluator_0);
        }

        private static MatchEvaluator matchEvaluator_0;

        private static string smethod_0(Match match_0)
        {
            string value = match_0.Value;
            if (value == "\r")
            {
                return string.Empty;
            }
            if (value == "\n")
            {
                return "<br />\n";
            }
            if (value == "&")
            {
                return "&amp;";
            }
            if (value == "<")
            {
                return "&lt;";
            }
            if (value == ">")
            {
                return "gt;";
            }
            return match_0.Value;
        }
    }


    public sealed class HtmlCss
    {
        private Queue<string> queue_0;

        public static HtmlCss Standard
        {
            get
            {
                HtmlCss htmlCss = new HtmlCss();
                htmlCss.AddEntry("body, table, td", "background-color: #f5f5f5; font-family: \"Helvetica LT\", Helvetica, Arial;");
                htmlCss.AddEntry("h1, h2, h3, h4, h5", "font-family: \"Vectora LH 55\", Vectora, \"Helvetica LT\", Helvetica, Arial; font-weight: normal; color: #014983; padding-top: 3px; padding-bottom: 3px; margin: 0px; ");
                htmlCss.AddEntry(".medium", "font-family: \"Helvetica LT\", Helvetica, Arial; font-size: 0.8em; color: #000;");
                htmlCss.AddEntry(".mPale", "font-family: \"Helvetica LT\", Helvetica, Arial; font-size: 0.8em; color: #555;");
                htmlCss.AddEntry(".small", "font-family: \"Helvetica LT\", Helvetica, Arial; font-size: 0.68em; color: #999; text-transform: uppercase;");
                htmlCss.AddEntry(".ultrasmall", "font-family: \"Helvetica LT\", Helvetica, Arial; font-size: 0.7em; color: #999;");
                htmlCss.AddEntry("hr", "color: #ccc; height: 1px;");
                htmlCss.AddEntry("a", "font-family: \"Helvetica LT\", Helvetica, Arial; font-size: 0.68em; color: #E29E41; text-transform: uppercase;");
                htmlCss.AddEntry("a:hover", "color: #ef9000;");
                return htmlCss;
            }
        }

        public HtmlCss()
        {
            this.queue_0 = new Queue<string>(9);
        }

        public HtmlCss AddEntry(string string_0, string string_1)
        {
            this.queue_0.Enqueue(string_0 + "\t{ " + string_1 + " }");
            return this;
        }

        public string Next()
        {
            return this.queue_0.Dequeue();
        }

        public bool NextExists()
        {
            return this.queue_0.Count > 0;
        }
    }


}

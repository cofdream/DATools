using System;

namespace DATools
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class SearchToolsAttribute : Attribute
    {
        public string Name { get; private set; }
        public string[] Keywords { get; private set; }

        public SearchToolsAttribute(string keyword)
        {
            Name = keyword;
            Keywords = new string[] { keyword.ToLower() };
        }
        public SearchToolsAttribute(string name, string keyword)
        {
            Name = name;
            Keywords = new string[] { keyword.ToLower() };
        }
        public SearchToolsAttribute(string name, string[] keywords)
        {
            Name = name;
            if (keywords == null)
                Keywords = new string[0];
            else
            {
                int index = 0;
                foreach (var keyword in keywords)
                {
                    Keywords[index] = keyword.ToLower();
                    index++;
                }
            }
        }
    }
}
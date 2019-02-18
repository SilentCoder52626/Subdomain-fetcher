
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using static System.Net.WebRequestMethods;
using System.Linq;

namespace StringManipulation
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            string path = @"C:\Users\Common\source\repos\Subdomain-fetcher\Subdomain-fetcher\domainLists.txt";
            string[] lines = System.IO.File.ReadAllLines(@path);

            foreach (var line in lines)
            {
                try
                {
                    LinkExtractor(line);
                }
                catch
                {
                    continue;
                }
                
            }            
        }     
        
        private static void LinkExtractor(string path)
        {
          
                

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                List<String> linkList = new List<String>();

                

                doc = web.Load("https://stackexchange.com/sites?view=list#name");
                
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];
                    String v;

                    if (att.Value.Contains("#"))
                    {
                        string[] substring = att.Value.Split('#');
                        v = substring[0];
                    }
                    else
                    {
                        v = att.Value;

                    }
                    if (v != "")
                    {
                        if (v.StartsWith("http"))
                        {
                            linkList.Add(v);
                        }
                    }

                }

            string maindomain = getmaindomain(path);
            Console.WriteLine("Serching For: " + maindomain);
            List<string> subdomain = ExtractDomainNameFromURL(linkList, maindomain);
            List<string> distinct = subdomain.Distinct().ToList();
            CreateFileFromList(distinct, "subexchange.txt");

        }

        public static void CreateFileFromList(List<string> data, string path)
        {
            try
            {
               
                    using (StreamWriter file = System.IO.File.AppendText(@path))
                    {
                        data.ForEach(v => file.WriteLine(v));
                    }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured:" + e);
            }
        }

        public static string getmaindomain(string url)
        {
            string maindomain = url.Replace("http://www.", "").Replace("https://www.", "");            
            return maindomain;
        }

        public static List<string> ExtractDomainNameFromURL(List<string> Url,string hostdomain)
        {

            List<string> shorturl = new List<string>();
            
            foreach(var line in Url)
            {

                Uri uri = new Uri(line);

                if(uri.Host.EndsWith("."+hostdomain))
                {
                    Console.WriteLine("Found: " + line);
                    shorturl.Add(uri.Host);
                }
                
            }          
            return shorturl;
        }



        //public static List<string> DomainExtractor(string Path, string prefix = "Prefix")
        //{
        //    try
        //    {
        //        string[] lines = System.IO.File.ReadAllLines(@Path);
        //        List<string> list = new List<string>();
        //        foreach (string line in lines)
        //        {
        //            string[] data = line.Split(',');
        //            string[] domain_parts = data[0].Trim().Split('"');
        //            string domain = domain_parts[1];
        //            if (domain.IndexOf('.') == 0)
        //            {
        //                list.Add(prefix + domain);
        //            }
        //            else
        //            {
        //                list.Add(prefix + '.' + domain);
        //            }
        //        }
        //        return list;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Error occured:" + e);
        //    }
        //}





    }
}

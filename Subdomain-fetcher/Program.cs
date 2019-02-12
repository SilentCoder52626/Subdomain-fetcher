
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
            // string path = @"C:\Users\Common\Desktop\StringManipulation-master\domainLists.txt";
            //var link = "https://stackoverflow.com";
            //string prefix = "https://www";
            //List<string> domains = DomainExtractor(path, prefix);
            //CreateFileFromList(domains, "domainLists.txt");
            //List<string> domains = LinkExtractor(path);
            LinkExtractor();
           
           
            //CreateFileFromList(domains, "subdomainLists.txt");
        }

        

        

        public static void CreateFileFromString(string data, string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, true))
                    {
                        file.Write(data);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured:" + e);
            }

        }

        

        
        private static void LinkExtractor()
        {

            
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            List<String> linkList = new List<String>();
            doc = web.Load("http://stackoverflow.com");
            foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                String v;

                // x.com


                //if (v.Contains("stackexchange.com")==false)
                //{
                //    continue;
                //}

                if (att.Value.Contains("#"))
                {
                    string[] substring = att.Value.Split('#');
                    v = substring[0];
                }
                else
                {
                    v = att.Value;                

                }
                if(v!= "")
                {
                    if (v.StartsWith("http"))
                    {
                        linkList.Add(v);
                    }
                    
                }
                
            }

            List<string> subdomain = ExtractDomainNameFromURL(linkList);
            
            foreach (var line in subdomain)
            {
                Console.WriteLine(line);
            }

            CreateFileFromList(subdomain, "subdomain.txt");
            
            Console.ReadKey();
        }

        public static void CreateFileFromList(List<string> data, string path)
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    using (var file = new StreamWriter(@path))
                    {
                        data.ForEach(v => file.WriteLine(v));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occured:" + e);
            }
        }

        public static List<string> ExtractDomainNameFromURL(List<string> Url)
        {
            List<string> shorturl = new List<string>();
            

            foreach(var line in Url)
            {

                Uri uri = new Uri(line);

                if(uri.Host.EndsWith(".stackexchange.com"))
                {
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

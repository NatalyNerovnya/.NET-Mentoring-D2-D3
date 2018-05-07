using System;
using System.Xml.Xsl;

namespace RSSGenerator
{
    public class Generator
    {
        public void Generate(string xmlPath)
        {
            var xsltPath = "../../../RSSGenerator/XmlToRss.xslt";
            var resultPath = @"C:\temp";

            var xsl = new XslCompiledTransform();
            xsl.Load(xsltPath);

            var resultFullPath = $"{resultPath}/result.xml";
            xsl.Transform(xmlPath, resultFullPath);
        }
    }
}

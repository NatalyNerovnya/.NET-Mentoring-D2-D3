using System.Xml.Xsl;

namespace RSSGenerator
{
    public class Generator
    {
        private string resultFolder = @"C:\temp";
        public void GenerateRss(string xmlPath)
        {
            var resultFullPath = $"{resultFolder}/result.xml";

            Generate(xmlPath, resultFullPath, "../../../RSSGenerator/XmlToRss.xslt");
        }

        public void GenerateHtml(string xmlPath)
        {
            var resultFullPath = $"{resultFolder}/result.html";

            Generate(xmlPath, resultFullPath, "../../../RSSGenerator/XmlToHtml.xslt");
        }

        private void Generate(string xmlPath, string resultPath, string xsltPath)
        {
            var xsl = new XslCompiledTransform();

            xsl.Load(xsltPath, new XsltSettings() { EnableScript = true}, null);
            xsl.Transform(xmlPath, resultPath);
        }
    }
}

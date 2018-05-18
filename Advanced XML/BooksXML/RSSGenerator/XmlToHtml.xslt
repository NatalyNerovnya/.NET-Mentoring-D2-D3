<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:b="http://library.by/catalog"
  xmlns:user="urn:my-scripts"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  extension-element-prefixes="b"
  exclude-result-prefixes="msxsl user">

  <xsl:output method="html" doctype-system="about:legacy-compat"/>

  <msxsl:script implements-prefix='user' language='CSharp'>
    <![CDATA[
    public string curDate() {
      return System.DateTime.Now.ToLongDateString();
    }]]>
  </msxsl:script>

  <xsl:variable name="book" select="b:book" />

  <xsl:template match="/">
    <html>
      <head>
        <title>Library</title>
      </head>
      <body>
        <h1>Books by genre</h1>
        <h3>
          <xsl:value-of select="user:curDate()"/>
        </h3>
        <xsl:variable name="genres" select="//b:genre[not(preceding::b:genre/. = .)]" />
        <xsl:for-each select="$genres">
          <table style="width: 100%; border: 1px solid">
            <caption>
              <xsl:value-of select="text()"/>
            </caption>
            <thead>
              <tr>
                <th>Author</th>
                <th>Name</th>
                <th>Published date</th>
                <th>Registration date</th>
              </tr>
            </thead>
            <tbody>
              <xsl:apply-templates select="//$book[b:genre = current()]" />
            </tbody>
            <tfoot>
              <tr>
                <td colspan="3">
                  Total
                </td>
                <td>
                  <xsl:value-of select="count(//$book[b:genre = current()])"/>
                </td>
              </tr>
            </tfoot>
          </table>
        </xsl:for-each>
        <xsl:value-of select="concat('Total number of books: ', count(//$book))" />
      </body>   
    </html>
  </xsl:template>

  <xsl:template match="b:book">
    <tr>
      <xsl:apply-templates />
    </tr>
  </xsl:template>

  <xsl:template match="b:author | b:title | b:publish_date | b:registration_date">
    <td>
      <xsl:value-of select="text()" />
    </td>
  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>

</xsl:stylesheet>

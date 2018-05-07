<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:b="http://library.by/catalog"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  extension-element-prefixes="b">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <rss version="2.0">
      <channel>
        <xsl:apply-templates />
      </channel>
    </rss>
  </xsl:template>

  <xsl:template match="b:book">
    <item>
      <xsl:apply-templates />
    </item>
  </xsl:template>

  <xsl:template match="b:catalog">
    <title>
      RSS library
    </title>
    <link>
      www.library.com
    </link>
    <description>
      Description
    </description>
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>

  <xsl:template match="b:registration_date">
    <pubDate>
      <xsl:value-of select="msxsl:format-date(text(), 'ddd, dd MMM yyyy 00:00:00 EST')"/>
    </pubDate>
  </xsl:template>

  <xsl:template match="b:title">
    <title>
      <xsl:value-of select="text()" />
    </title>
  </xsl:template>

  <xsl:template match="b:description">
    <description>
      <xsl:value-of select="text()" />
    </description>
  </xsl:template>

  <xsl:template match="b:isbn">
    <xsl:if test="text() and ../b:genre = 'Computer'">
      <link>
        <xsl:value-of select="concat('http://my.safaribooksonline.com/', text() ,'/')"/>
      </link>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:b="http://library.by/catalog"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  extension-element-prefixes="b">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/">
    <xsl:element name="rss">
      <xsl:attribute name="version">2.0</xsl:attribute>
      <xsl:element name="channel">
        <xsl:apply-templates />
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="b:book">
    <xsl:element name="item">
      <xsl:element name="title">
        <xsl:value-of select="b:title"/>
      </xsl:element>
        <xsl:if test="b:isbn and b:genre = 'Computer'">
          <xsl:element name="link">
            <xsl:value-of select="concat('http://my.safaribooksonline.com/', b:isbn ,'/')"/>
          </xsl:element>
        </xsl:if>
        <xsl:element name="pubDate">
          <xsl:value-of select="msxsl:format-date(b:registration_date, 'ddd, dd MMM yyyy 00:00:00 EST')"/>
        </xsl:element>
        <xsl:element name="description">
          <xsl:value-of select="b:description"/>
        </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="b:catalog">
    <xsl:element name="title">
      <xsl:value-of select="'RSS library'" />
    </xsl:element>
    <xsl:element name="link">
      <xsl:value-of select="'www.library.com'" />
    </xsl:element>
    <xsl:element name="description">
      <xsl:value-of select="'Description'" />
    </xsl:element>
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="@* | node()">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>
  
</xsl:stylesheet>

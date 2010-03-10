using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test
{
    /// <summary>
    /// Service class used for unit test
    /// </summary>
    public class MockSDataService : ISDataService, ISDataRequestSettings
    {
        #region Constants

        private const string AtomFeedString =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<feed xmlns:sme=\"http://schemas.sage.com/sdata/sme/2007\" xmlns:sdata=\"http://schemas.sage.com/sdata/2008/1\" xmlns:cf=\"http://www.microsoft.com/schemas/rss/core/2005\" xmlns=\"http://www.w3.org/2005/Atom\">" +
            "  <author>" +
            "    <name>slx</name>" +
            "    <uri />" +
            "    <email />" +
            "</author>" +
            "<category term=\"\" scheme=\"\" label=\"\" />" +
            "<generator uri=\"\" version=\"1.0\">Sage Dynamic Integration Adapter</generator>" +
            " <id />" +
            " <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />" +
            " <subtitle>Provides a feed containing Employee details</subtitle>" +
            " <title>Sage | employees</title>" +
            " <entry>" +
            "   <author>" +
            "  <name>slx</name>" +
            " <uri />" +
            "<email />" +
            "</author>" +
            "<content type=\"html\">" +
            " <![CDATA[<html>" +
            "<head>" +
            "  <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "</head>" +
            "<body style=\"font-family: Verdana;\">" +
            " <table cellpadding=\"4\" cellspacing=\"0\" style=\"background-color: white; font-family: Arial;border: 1px solid darkgray;font-size: x-small;\">" +
            "  <tr>" +
            "   <td nowrap colspan=\"3\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">Employee Details - Production Technician - WC60</td>" +
            "  <td colspan=\"31\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">&#x00A0;</td>" +
            "</tr>" +
            "<tr style=\"background-color: lavender;\">" +
            "  <td colspan=\"32\">&#x00A0;</td>" +
            "</tr>" +
            "</table>" +
            "</body>" +
            "</html>]]>" +
            "</content>" +
            "<id>http://localhost:8001/sdata/aw/dynamic/-/employees(1)</id>" +
            "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)?format=html\" rel=\"alternate\" type=\"text/html\" title=\"\" />" +
            "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />" +
            "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"edit\" type=\"application/atom+xml\" title=\"\" />" +
            "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"via\" type=\"application/atom+xml\" title=\"\" />" +
            " <published>0001-01-01T00:00:00+00:00</published>" +
            "<sdata:payload>" +
            " <Employee>" +
            "  <Title>Production Technician - WC60</Title>" +
            "</Employee>" +
            "</sdata:payload>" +
            " <summary />" +
            "<title>14417807</title>" +
            "<updated>0001-01-01T00:00:00+00:00</updated>" +
            "<sdata:payload>" +
            "  <Employee>" +
            "    <NationalIdNumber>14417807</NationalIdNumber>" +
            "    <ContactId>1209</ContactId>" +
            "    <LoginId>adventure-works\\guy1</LoginId>" +
            "    <ManagerId>16</ManagerId>" +
            "    <BirthDate>1972-05-15T00:00:00+00:00</BirthDate>" +
            "    <MaritalStatus>False</MaritalStatus>" +
            "    <Gender>False</Gender>" +
            "    <HireDate>1996-07-31T00:00:00+00:00</HireDate>" +
            "    <Salariedflag>False</Salariedflag>" +
            "    <VacationHours>21</VacationHours>" +
            "    <SickleaveHours>30</SickleaveHours>" +
            "    <Currentflag>True</Currentflag>" +
            "    <RowGuid>aae1d04a-c237-4974-b4d5-935247737718</RowGuid>" +
            "    <ModifiedDate>2004-07-31T00:00:00+00:00</ModifiedDate>" +
            "</Employee>" +
            "</sdata:payload>" +
            "</entry>" +
            " <entry>" +
            "   <author>" +
            "      <name>slx</name>" +
            "      <uri />" +
            "      <email />" +
            "</author>" +
            "    <content type=\"html\">" +
            "      <![CDATA[<html>" +
            "  <head>" +
            "    <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
            "  </head>" +
            "  <body style=\"font-family: Verdana;\">" +
            "    <table cellpadding=\"4\" cellspacing=\"0\" style=\"background-color: white; font-family: Arial;border: 1px solid darkgray;font-size: x-small;\">" +
            "      <tr>" +
            "        <td nowrap colspan=\"3\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">Employee Details - Marketing Assistant</td>" +
            "        <td colspan=\"31\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">&#x00A0;</td>" +
            "      </tr>" +
            "      <tr style=\"background-color: lavender;\">" +
            "        <td colspan=\"32\">&#x00A0;</td>" +
            "      </tr>" +
            "    </table>" +
            "  </body>" +
            "</html>]]>" +
            "</content>" +
            "    <id>http://localhost:8001/sdata/aw/dynamic/-/employees(2)</id>" +
            "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)?format=html\" rel=\"alternate\" type=\"text/html\" title=\"\" />" +
            "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />" +
            "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)\" rel=\"edit\" type=\"application/atom+xml\" title=\"\" />" +
            "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"via\" type=\"application/atom+xml\" title=\"\" />" +
            "    <published>0001-01-01T00:00:00+00:00</published>" +
            "    <sdata:payload>" +
            "      <Employee>" +
            "        <Title>Marketing Assistant</Title>" +
            "          </Employee>" +
            "          </sdata:payload>" +
            "         <summary />" +
            "         <title>253022876</title>" +
            "         <updated>0001-01-01T00:00:00+00:00</updated>" +
            "         <sdata:payload>" +
            "      <Employee>" +
            "        <NationalIdNumber>253022876</NationalIdNumber>" +
            "        <ContactId>1030</ContactId>" +
            "        <LoginId>adventure-works\\kevin0</LoginId>" +
            "        <ManagerId>6</ManagerId>" +
            "        <BirthDate>1977-06-03T00:00:00+00:00</BirthDate>" +
            "        <MaritalStatus>False</MaritalStatus>" +
            "        <Gender>False</Gender>" +
            "        <HireDate>1997-02-26T00:00:00+00:00</HireDate>" +
            "        <Salariedflag>False</Salariedflag>" +
            "        <VacationHours>42</VacationHours>" +
            "        <SickleaveHours>41</SickleaveHours>" +
            "        <Currentflag>True</Currentflag>" +
            "        <RowGuid>1b480240-95c0-410f-a717-eb29943c8886</RowGuid>" +
            "        <ModifiedDate>2004-07-31T00:00:00+00:00</ModifiedDate>" +
            "    </Employee>" +
            "  </sdata:payload>" +
            "</entry>" +
            "</feed>";

        private const string AtomEntryString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                               "  <entry xmlns:sme=\"http://schemas.sage.com/sdata/sme/2007\" xmlns:sdata=\"http://schemas.sage.com/sdata/2008/1\" xmlns:cf=\"http://www.microsoft.com/schemas/rss/core/2005\" xmlns=\"http://www.w3.org/2005/Atom\"><content type=\"html\"><![CDATA[" +
                                               "  <html>" +
                                               "  <head>" +
                                               "    <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                                               "  </head>" +
                                               "  <body style=\"font-family: Verdana;\">" +
                                               "    <table cellpadding=\"4\" cellspacing=\"0\" style=\"background-color: white; font-family: Arial;border: 1px solid darkgray;font-size: x-small;\">" +
                                               "      <tr>" +
                                               "        <td nowrap colspan=\"3\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">Employee Details - Production Technician - WC60</td>" +
                                               "        <td colspan=\"31\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">&#x00A0;</td>" +
                                               "      </tr>" +
                                               "     <tr style=\"background-color: lavender;\">" +
                                               "        <td colspan=\"32\">&#x00A0;</td>" +
                                               "      </tr>" +
                                               "    </table>" +
                                               "  </body>" +
                                               "</html>]]>" +
                                               "</content>" +
                                               "  <id>http://localhost:8001/sdata/aw/dynamic/-/employees(1)</id>" +
                                               "  <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)?format=html\" rel=\"alternate\" type=\"text/html\" title=\"\" />" +
                                               "  <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />" +
                                               "  <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"edit\" type=\"application/atom+xml\" title=\"\" />" +
                                               "  <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)?format=atomentry\" rel=\"via\" type=\"application/atom+xml\" title=\"\" />" +
                                               "  <published>0001-01-01T00:00:00+00:00</published>" +
                                               "  <sdata:payload>" +
                                               "    <Employee xmlns=\"http://schemas.sage.com/dynamic/2007\">" +
                                               "      <Title>Production Technician - WC60</Title>" +
                                               "      <NationalIdNumber>14417807</NationalIdNumber>" +
                                               "      <ContactId>1209</ContactId>" +
                                               "      <LoginId>adventure-works\\guy1</LoginId>" +
                                               "      <ManagerId>16</ManagerId>" +
                                               "      <BirthDate>1972-05-15T00:00:00+00:00</BirthDate>" +
                                               "      <MaritalStatus>False</MaritalStatus>" +
                                               "      <Gender>False</Gender>" +
                                               "      <HireDate>1996-07-31T00:00:00+00:00</HireDate>" +
                                               "      <SalariedFlag>False</SalariedFlag>" +
                                               "      <VacationHours>21</VacationHours>" +
                                               "      <SickleaveHours>30</SickleaveHours>" +
                                               "      <CurrentFlag>True</CurrentFlag>" +
                                               "      <RowGuid>aae1d04a-c237-4974-b4d5-935247737718</RowGuid>" +
                                               "      <ModifiedDate>2004-07-31T00:00:00+00:00</ModifiedDate>" +
                                               "    </Employee>" +
                                               "  </sdata:payload>" +
                                               "</entry>";

        private const string XsdString =
            "<?xml version=\"1.0\"?>" +
            "            <xs:schema xmlns:tns=\"http://schemas.sage.com/demoErp\" " +
            "           targetNamespace=\"http://schemas.sage.com/demoErp\" " +
            "           xmlns:sme=\"http://schemas.sage.com/sdata/sme/2007\" " +
            "           xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"" +
            "           elementFormDefault=\"qualified\" >" +
            "  <!-- Resource Kind Definition -->" +
            "  <xs:element name=\"product\" type=\"tns:product--type\" " +
            "    sme:role=\"resourceKind\" sme:pluralName=\"products\" sme:label=\"Product\"" +
            "    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\"" +
            "    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" " +
            "    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />" +
            "  <xs:complexType name=\"product--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"productNumber\" type=\"xs:string\"" +
            "                  sme:label=\"#\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" />" +
            "      <xs:element name=\"productID\" type=\"xs:integer\" " +
            "                  sme:label=\"ID\" sme:isIdentifier=\"true\"" +
            "                  sme:isReadOnly=\"true\"/>" +
            "     <xs:element name=\"name\" type=\"xs:string\" sme:isDescriptor=\"true\"" +
            "                sme:label=\"Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\"/>" +
            " </xs:all>" +
            "</xs:complexType>" +
            "  <xs:complexType name=\"product--list\">" +
            "     <xs:sequence>" +
            "       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"product\" type=\"tns:product--type\" />" +
            "    </xs:sequence>" +
            "  </xs:complexType>" +
            "  <!-- Resource Kind Definition -->" +
            "  <xs:element name=\"salesOrder\" type=\"tns:salesOrder--type\" " +
            "    sme:role=\"resourceKind\" sme:pluralName=\"salesOrders\" sme:label=\"Sales Order\"" +
            "    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\"" +
            "    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" " +
            "    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />" +
            "  <xs:complexType name=\"salesOrder--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />" +
            "      <xs:element name=\"salesOrderID\" type=\"xs:integer\" minOccurs=\"0\"" +
            "                  sme:label=\"ID\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" " +
            "                  sme:isIdentifier=\"true\" sme:isReadOnly=\"true\"/>" +
            "      <xs:element name=\"orderDate\" type=\"xs:date\" minOccurs=\"0\"" +
            "                  sme:label=\"Date\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"2\" />" +
            "      <xs:element name=\"shipDate\" type=\"xs:date\" minOccurs=\"0\" nillable=\"true\"" +
            "                  sme:label=\"Shipping Date\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"3\" />" +
            "      <xs:element name=\"contactID\" type=\"xs:integer\" minOccurs=\"0\"" +
            "                  sme:copiedFrom=\"contact/contactID\"/>" +
            "      <xs:element name=\"subTotal\" type=\"xs:decimal\" minOccurs=\"0\"" +
            "                  sme:label=\"Sub-total\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"2\" " +
            "                  sme:isReadOnly=\"true\" />" +
            "      <xs:element name=\"billAddress\" type=\"tns:address--type\" minOccurs=\"0\"" +
            "                 sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Billing Address\" " +
            "                  sme:canGet=\"true\" sme:canPut=\"true\" />" +
            "      <xs:element name=\"shipAddress\" type=\"tns:address--type\" minOccurs=\"0\"" +
            "                  sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Shipping Address\" " +
            "                  sme:canGet=\"true\" sme:canPut=\"true\" />" +
            "      <xs:element name=\"orderLines\" type=\"tns:salesOrderLine--list\" minOccurs=\"0\"" +
            "                  sme:relationship=\"child\" sme:isCollection=\"true\" sme:label=\"Order Lines\" " +
            "                  sme:canGet=\"true\" sme:canPost=\"true\" />" +
            "      <xs:element name=\"contact\" type=\"tns:contact--type\" minOccurs=\"0\"" +
            "                  sme:relationship=\"reference\" sme:label=\"Contact\" " +
            "                  sme:canGet=\"true\" />" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"salesOrder--list\">" +
            "     <xs:sequence>" +
            "       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"salesOrder\" type=\"tns:salesOrder--type\" />" +
            "    </xs:sequence>" +
            "  </xs:complexType>" +
            "  <!-- Resource Kind Definition -->" +
            "  <xs:element name=\"salesOrderLine\" type=\"tns:salesOrderLine--type\" " +
            "    sme:role=\"resourceKind\" sme:pluralName=\"salesOrderLines\" sme:label=\"Sales Order Line\"" +
            "    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\"" +
            "    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" " +
            "    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />" +
            "  <xs:complexType name=\"salesOrderLine--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />" +
            "      <xs:element name=\"salesOrderID\" type=\"xs:integer\" minOccurs=\"0\"" +
            "                  sme:label=\"Order ID\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" " +
            "                  sme:isReadOnly=\"true\" />" +
            "      <xs:element name=\"salesOrderDetailID\" type=\"xs:integer\" minOccurs=\"0\"  " +
            "                  sme:label=\"ID\" sme:isIdentifier=\"true\"" +
            "                  sme:isReadOnly=\"true\"/>" +
            "      <xs:element name=\"productID\" type=\"xs:integer\" minOccurs=\"0\"" +
            "                  sme:label=\"Product ID\" sme:copiedFrom=\"product/productID\" />" +
            "      <xs:element name=\"orderQty\" type=\"xs:decimal\" minOccurs=\"0\"" +
            "                  sme:label=\"Qty\" />" +
            "      <xs:element name=\"unitPrice\" type=\"xs:decimal\" minOccurs=\"0\"" +
            "                  sme:label=\"Unit Price\" />" +
            "      <xs:element name=\"order\" type=\"tns:salesOrder--type\" minOccurs=\"0\"" +
            "                  sme:relationship=\"parent\" sme:label=\"Order\" />" +
            "      <xs:element name=\"product\" type=\"tns:product--type\" minOccurs=\"0\"" +
            "                  sme:relationship=\"reference\" sme:label=\"Product\" />" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"salesOrderLine--list\">" +
            "     <xs:sequence>" +
            "       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"salesOrderLine\" type=\"tns:salesOrderLine--type\" />" +
            "    </xs:sequence>" +
            "  </xs:complexType>" +
            "  <!-- Resource Kind Definition -->" +
            "  <xs:element name=\"contact\" type=\"tns:contact--type\"" +
            "    sme:role=\"resourceKind\" sme:pluralName=\"contacts\" sme:label=\"Contact\"" +
            "    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\"" +
            "    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" " +
            "    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />" +
            "  <xs:complexType name=\"contact--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />" +
            "      <xs:element name=\"contactID\" type=\"xs:integer\" minOccurs=\"0\"" +
            "                  sme:label=\"ID\" sme:isIdentifier=\"true\"" +
            "                  sme:isReadOnly=\"true\"/> " +
            "      <xs:element name=\"civility\" type=\"tns:civility--type\" minOccurs=\"0\"" +
            "                  sme:label=\"Civility\" />" +
            "      <xs:element name=\"firstName\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"First Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" /> " +
            "      <xs:element name=\"lastName\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"Last Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\"" +
            "                  sme:isDescriptor=\"true\"/> " +
            "      <xs:element name=\"address\" type=\"tns:address--type\" minOccurs=\"0\"" +
            "                  sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Address\" /> " +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"contact--list\">" +
            "     <xs:sequence>" +
            "       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"contact\" type=\"tns:contact--type\" />" +
            "    </xs:sequence>" +
            "  </xs:complexType>" +
            "  <!-- Resource Kind Definition -->" +
            "  <xs:element name=\"address\" type=\"tns:address--type\"" +
            "    sme:role=\"resourceKind\" sme:pluralName=\"addresses\" sme:label=\"Address\"" +
            "    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\"" +
            "    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" " +
            "    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />" +
            "  <xs:complexType name=\"address--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />" +
            "      <xs:element name=\"street\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"Street\" /> " +
            "      <xs:element name=\"city\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"City\" />" +
            "      <xs:element name=\"postalCode\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"Postal Code\" />" +
            "      <xs:element name=\"countryCode\" type=\"xs:string\" minOccurs=\"0\"" +
            "                  sme:label=\"County Code\" />" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"address--list\">" +
            "     <xs:sequence>" +
            "       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"product\" type=\"tns:address--type\" />" +
            "    </xs:sequence>" +
            "  </xs:complexType>" +
            "  <!-- Auxiliary Type Definition -->" +
            "  <xs:simpleType name=\"civility--type\">" +
            "    <xs:restriction base=\"xs:string\">" +
            "      <xs:enumeration value=\"Mr\"/>" +
            "      <xs:enumeration value=\"Mrs\"/>" +
            "      <xs:enumeration value=\"Ms\"/>" +
            "    </xs:restriction>" +
            "  </xs:simpleType>" +
            "  <!-- Service Operation Definition -->" +
            "  <xs:element name=\"productComputeSimplePrice\" " +
            "              type=\"tns:productComputeSimplePrice--type\"" +
            "              sme:role=\"serviceOperation\"" +
            "              sme:path=\"products/$service/computeSimplePrice\"" +
            "              sme:invocationMode=\"sync\" />" +
            "  <xs:complexType name=\"productComputeSimplePrice--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"request\" type=\"tns:simplePriceRequest--type\" minOccurs=\"0\"" +
            "                  sme:envelope=\"entry\"/>" +
            "      <xs:element name=\"response\" type=\"tns:simplePriceResponse--type\" minOccurs=\"0\"" +
            "                  sme:envelope=\"entry\"/>" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"simplePriceRequest--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"productID\" type=\"xs:string\" sme:label=\"Product ID\" />" +
            "      <xs:element name=\"customerID\" type=\"xs:string\" sme:label=\"Customer ID\" />" +
            "      <xs:element name=\"quantity\" type=\"xs:decimal\" sme:label=\"Quantity\" />" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "  <xs:complexType name=\"simplePriceResponse--type\">" +
            "    <xs:all>" +
            "      <xs:element name=\"unitPrice\" type=\"xs:decimal\" sme:label=\"Unit Price\" />" +
            "      <xs:element name=\"quantityPrice\" type=\"xs:decimal\" sme:label=\"Quantity Price\" />" +
            "      <xs:element name=\"discount\" type=\"xs:decimal\" sme:label=\"Discount\" />" +
            "      <xs:element name=\"tax\" type=\"xs:decimal\" sme:label=\"Tax\" />" +
            "    </xs:all>" +
            "  </xs:complexType>" +
            "</xs:schema>";

        #endregion

        public bool Initialized { get; set; }
        public string Url { get; set; }
        public string Protocol { get; set; }
        public string ServerName { get; set; }
        public int? Port { get; set; }
        public string VirtualDirectory { get; set; }
        public string ApplicationName { get; set; }
        public string ContractName { get; set; }
        public string DataSet { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Timeout { get; set; }
        public CookieContainer Cookies { get; set; }
        public string UserAgent { get; set; }

        public AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed)
        {
            var createdFeed = new AtomFeed();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(AtomFeedString)))
            {
                createdFeed.Load(stream);
            }

            return createdFeed;
        }

        public AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed, out string eTag)
        {
            throw new NotImplementedException();
        }

        public AtomEntry CreateEntry(SDataBaseRequest request, AtomEntry entry)
        {
            var createdEntry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(AtomEntryString)))
            {
                createdEntry.Load(stream);
            }

            return createdEntry;
        }

        public AsyncRequest CreateAsync(SDataBaseRequest request, ISyndicationResource resource)
        {
            const string trackingUrl = "http://www.example.com/sdata/sageApp/test/-/products/$service/computeSimplePrice";
            var tracking = new SDataTracking
                           {
                               Phase = "Initializing",
                               PhaseDetail = "StartingThread",
                               Progress = 0.0,
                               ElapsedSeconds = 0,
                               RemainingSeconds = 10,
                               PollingMillis = 500
                           };
            return new AsyncRequest(this, trackingUrl, tracking);
        }

        public bool Delete(string url)
        {
            return true;
        }

        public bool DeleteEntry(SDataBaseRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEntry(SDataBaseRequest request, AtomEntry entry)
        {
            return true;
        }

        public object Read(string url)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                   "<tracking xmlns=\"http://schemas.sage.com/sdata/2008/1\">" +
                   "<phase>Initialization</phase>" +
                   "<phaseDetail>Starting thread</phaseDetail>" +
                   "<progress>0.0</progress>" +
                   "<elapsedSeconds>0</elapsedSeconds>" +
                   "<remainingSeconds>7</remainingSeconds>" +
                   "<pollingMillis>500</pollingMillis>" +
                   "</tracking>";
        }

        public AtomFeed ReadFeed(SDataBaseRequest request)
        {
            var feed = new AtomFeed();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(AtomFeedString)))
            {
                feed.Load(stream);
            }

            return feed;
        }

        public AtomFeed ReadFeed(SDataBaseRequest request, ref string eTag)
        {
            throw new NotImplementedException();
        }

        public AtomEntry ReadEntry(SDataBaseRequest request)
        {
            var entry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(AtomEntryString)))
            {
                entry.Load(stream);
            }

            return entry;
        }

        public AtomEntry ReadEntry(SDataBaseRequest request, AtomEntry entry)
        {
            throw new NotImplementedException();
        }

        public XmlSchema ReadSchema(SDataResourceSchemaRequest request)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(XsdString)))
            {
                return XmlSchema.Read(stream, null);
            }
        }

        public AtomEntry UpdateEntry(SDataBaseRequest request, AtomEntry entry)
        {
            var updatedEntry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(AtomEntryString)))
            {
                updatedEntry.Load(stream);
            }

            return updatedEntry;
        }

        public void Initialize()
        {
        }

        public MockSDataService(string url, string userName, string password)
        {
            Url = url;
            UserName = userName;
            Password = password;

            var uri = new SDataUri(url);
            Protocol = uri.Scheme;
            ServerName = uri.Host;
            Port = uri.Port;
            VirtualDirectory = uri.Server;
            ApplicationName = uri.Product;
            ContractName = uri.Contract;
            DataSet = uri.CompanyDataset;
        }
    }
}
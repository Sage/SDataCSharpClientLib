using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Schema;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Core
{

    /// <summary>
    /// Service class used for unit test
    /// </summary>
    public class SDataServiceTest : ISDataService, ISDataRequestSettings
    {

        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================

        private static string atomfeed_string =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+
"<feed xmlns:sme=\"http://schemas.sage.com/sdata/sme/2007\" xmlns:sdata=\"http://schemas.sage.com/sdata/2008/1\" xmlns:cf=\"http://www.microsoft.com/schemas/rss/core/2005\" xmlns=\"http://www.w3.org/2005/Atom\">"+
  "  <author>"+
  "    <name>slx</name>"+
  "    <uri />"+
  "    <email />"+
  "</author>"+
  "<category term=\"\" scheme=\"\" label=\"\" />"+
  "<generator uri=\"\" version=\"1.0\">Sage Dynamic Integration Adapter</generator>"+
  " <id />"+
  " <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />"+
  " <subtitle>Provides a feed containing Employee details</subtitle>"+
  " <title>Sage | employees</title>"+
  " <entry>"+
    "   <author>"+
      "  <name>slx</name>"+
      " <uri />"+
      "<email />"+
      "</author>"+
    "<content type=\"html\">"+
      " <![CDATA[<html>"+
  "<head>"+
  "  <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">"+
  "</head>"+
  "<body style=\"font-family: Verdana;\">"+
   " <table cellpadding=\"4\" cellspacing=\"0\" style=\"background-color: white; font-family: Arial;border: 1px solid darkgray;font-size: x-small;\">"+
    "  <tr>"+
     "   <td nowrap colspan=\"3\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">Employee Details - Production Technician - WC60</td>"+
      "  <td colspan=\"31\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">&#x00A0;</td>"+
      "</tr>"+
      "<tr style=\"background-color: lavender;\">"+
      "  <td colspan=\"32\">&#x00A0;</td>"+
      "</tr>"+
    "</table>"+
  "</body>"+
"</html>]]>"+
    "</content>"+
    "<id>http://localhost:8001/sdata/aw/dynamic/-/employees(1)</id>"+
    "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)?format=html\" rel=\"alternate\" type=\"text/html\" title=\"\" />"+
    "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />"+
    "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(1)\" rel=\"edit\" type=\"application/atom+xml\" title=\"\" />"+
    "<link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"via\" type=\"application/atom+xml\" title=\"\" />"+
    " <published>0001-01-01T00:00:00+00:00</published>"+
    "<sdata:payload>"+
      " <Employee>"+
       "  <Title>Production Technician - WC60</Title>"+
       "</Employee>"+
      "</sdata:payload>"+
    " <summary />"+
    "<title>14417807</title>"+
    "<updated>0001-01-01T00:00:00+00:00</updated>"+
    "<sdata:payload>"+
      "  <Employee>"+
      "    <NationalIdNumber>14417807</NationalIdNumber>"+
      "    <ContactId>1209</ContactId>"+
      "    <LoginId>adventure-works\\guy1</LoginId>"+
      "    <ManagerId>16</ManagerId>"+
      "    <BirthDate>1972-05-15T00:00:00+00:00</BirthDate>"+
      "    <MaritalStatus>False</MaritalStatus>"+
      "    <Gender>False</Gender>"+
      "    <HireDate>1996-07-31T00:00:00+00:00</HireDate>"+
      "    <Salariedflag>False</Salariedflag>"+
      "    <VacationHours>21</VacationHours>"+
      "    <SickleaveHours>30</SickleaveHours>"+
      "    <Currentflag>True</Currentflag>"+
      "    <RowGuid>aae1d04a-c237-4974-b4d5-935247737718</RowGuid>"+
      "    <ModifiedDate>2004-07-31T00:00:00+00:00</ModifiedDate>"+
      "</Employee>"+
      "</sdata:payload>"+
    "</entry>"+
  " <entry>"+
  "   <author>"+
    "      <name>slx</name>"+
    "      <uri />"+
    "      <email />"+
    "</author>"+
  "    <content type=\"html\">"+
    "      <![CDATA[<html>"+
"  <head>"+
"    <META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">"+
"  </head>"+
"  <body style=\"font-family: Verdana;\">"+
"    <table cellpadding=\"4\" cellspacing=\"0\" style=\"background-color: white; font-family: Arial;border: 1px solid darkgray;font-size: x-small;\">"+
"      <tr>"+
"        <td nowrap colspan=\"3\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">Employee Details - Marketing Assistant</td>"+
"        <td colspan=\"31\" valign=\"middle\" style=\"font-size: x-small;font-weight: bold;color: black;border-bottom-style: solid;border-bottom-width: 1px;border-bottom-color: darkgray;vertical-align: middle;\">&#x00A0;</td>"+
"      </tr>"+
"      <tr style=\"background-color: lavender;\">"+
"        <td colspan=\"32\">&#x00A0;</td>"+
"      </tr>"+
"    </table>"+
"  </body>"+
"</html>]]>"+
  "</content>"+
  "    <id>http://localhost:8001/sdata/aw/dynamic/-/employees(2)</id>"+
  "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)?format=html\" rel=\"alternate\" type=\"text/html\" title=\"\" />"+
  "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)\" rel=\"self\" type=\"application/atom+xml\" title=\"\" />"+
  "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees(2)\" rel=\"edit\" type=\"application/atom+xml\" title=\"\" />"+
  "    <link href=\"http://localhost:8001/sdata/aw/dynamic/-/employees?startIndex=1&amp;count=2\" rel=\"via\" type=\"application/atom+xml\" title=\"\" />"+
  "    <published>0001-01-01T00:00:00+00:00</published>"+
  "    <sdata:payload>"+
    "      <Employee>"+
      "        <Title>Marketing Assistant</Title>"+
 "          </Employee>"+
 "          </sdata:payload>"+
  "         <summary />"+
  "         <title>253022876</title>"+
  "         <updated>0001-01-01T00:00:00+00:00</updated>"+
  "         <sdata:payload>"+
    "      <Employee>"+
      "        <NationalIdNumber>253022876</NationalIdNumber>"+
      "        <ContactId>1030</ContactId>"+
      "        <LoginId>adventure-works\\kevin0</LoginId>"+
      "        <ManagerId>6</ManagerId>"+
      "        <BirthDate>1977-06-03T00:00:00+00:00</BirthDate>"+
      "        <MaritalStatus>False</MaritalStatus>"+
      "        <Gender>False</Gender>"+
      "        <HireDate>1997-02-26T00:00:00+00:00</HireDate>"+
      "        <Salariedflag>False</Salariedflag>"+
      "        <VacationHours>42</VacationHours>"+
      "        <SickleaveHours>41</SickleaveHours>"+
      "        <Currentflag>True</Currentflag>"+
      "        <RowGuid>1b480240-95c0-410f-a717-eb29943c8886</RowGuid>"+
      "        <ModifiedDate>2004-07-31T00:00:00+00:00</ModifiedDate>"+
"    </Employee>"+
"  </sdata:payload>"+
"</entry>"+
"</feed>";



        private static string atomentry_string = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
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


        private static string xsd_string = 
"<?xml version=\"1.0\"?>"+
"            <xs:schema xmlns:tns=\"http://schemas.sage.com/demoErp\" "+
"           targetNamespace=\"http://schemas.sage.com/demoErp\" "+
"           xmlns:sme=\"http://schemas.sage.com/sdata/sme/2007\" "+
"           xmlns:xs=\"http://www.w3.org/2001/XMLSchema\""+
"           elementFormDefault=\"qualified\" >"+
"  <!-- Resource Kind Definition -->"+
"  <xs:element name=\"product\" type=\"tns:product--type\" "+
"    sme:role=\"resourceKind\" sme:pluralName=\"products\" sme:label=\"Product\""+
"    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\""+
"    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" "+
"    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />"+
"  <xs:complexType name=\"product--type\">"+
"    <xs:all>"+
"      <xs:element name=\"productNumber\" type=\"xs:string\""+
"                  sme:label=\"#\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" />"+
"      <xs:element name=\"productID\" type=\"xs:integer\" "+
"                  sme:label=\"ID\" sme:isIdentifier=\"true\""+
"                  sme:isReadOnly=\"true\"/>"+
"     <xs:element name=\"name\" type=\"xs:string\" sme:isDescriptor=\"true\""+
"                sme:label=\"Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\"/>"+
" </xs:all>"+
"</xs:complexType>"+
"  <xs:complexType name=\"product--list\">"+
"     <xs:sequence>"+
"       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"product\" type=\"tns:product--type\" />"+
"    </xs:sequence>"+
"  </xs:complexType>"+
"  <!-- Resource Kind Definition -->"+
"  <xs:element name=\"salesOrder\" type=\"tns:salesOrder--type\" "+
"    sme:role=\"resourceKind\" sme:pluralName=\"salesOrders\" sme:label=\"Sales Order\""+
"    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\""+
"    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" "+
"    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />"+
"  <xs:complexType name=\"salesOrder--type\">"+
"    <xs:all>"+
"      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />"+
"      <xs:element name=\"salesOrderID\" type=\"xs:integer\" minOccurs=\"0\""+
"                  sme:label=\"ID\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" "+
"                  sme:isIdentifier=\"true\" sme:isReadOnly=\"true\"/>"+
"      <xs:element name=\"orderDate\" type=\"xs:date\" minOccurs=\"0\""+
"                  sme:label=\"Date\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"2\" />"+
"      <xs:element name=\"shipDate\" type=\"xs:date\" minOccurs=\"0\" nillable=\"true\""+
"                  sme:label=\"Shipping Date\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"3\" />"+
"      <xs:element name=\"contactID\" type=\"xs:integer\" minOccurs=\"0\""+
"                  sme:copiedFrom=\"contact/contactID\"/>"+
"      <xs:element name=\"subTotal\" type=\"xs:decimal\" minOccurs=\"0\""+
"                  sme:label=\"Sub-total\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"2\" "+
"                  sme:isReadOnly=\"true\" />"+
"      <xs:element name=\"billAddress\" type=\"tns:address--type\" minOccurs=\"0\""+
"                 sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Billing Address\" "+
"                  sme:canGet=\"true\" sme:canPut=\"true\" />"+
"      <xs:element name=\"shipAddress\" type=\"tns:address--type\" minOccurs=\"0\""+
"                  sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Shipping Address\" "+
"                  sme:canGet=\"true\" sme:canPut=\"true\" />"+
"      <xs:element name=\"orderLines\" type=\"tns:salesOrderLine--list\" minOccurs=\"0\""+
"                  sme:relationship=\"child\" sme:isCollection=\"true\" sme:label=\"Order Lines\" "+
"                  sme:canGet=\"true\" sme:canPost=\"true\" />"+
"      <xs:element name=\"contact\" type=\"tns:contact--type\" minOccurs=\"0\""+
"                  sme:relationship=\"reference\" sme:label=\"Contact\" "+
"                  sme:canGet=\"true\" />"+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"salesOrder--list\">"+
"     <xs:sequence>"+
"       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"salesOrder\" type=\"tns:salesOrder--type\" />"+
"    </xs:sequence>"+
"  </xs:complexType>"+
"  <!-- Resource Kind Definition -->"+
"  <xs:element name=\"salesOrderLine\" type=\"tns:salesOrderLine--type\" "+
"    sme:role=\"resourceKind\" sme:pluralName=\"salesOrderLines\" sme:label=\"Sales Order Line\""+
"    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\""+
"    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" "+
"    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />"+
"  <xs:complexType name=\"salesOrderLine--type\">"+
"    <xs:all>"+
"      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />"+
"      <xs:element name=\"salesOrderID\" type=\"xs:integer\" minOccurs=\"0\""+
"                  sme:label=\"Order ID\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" "+
"                  sme:isReadOnly=\"true\" />"+
"      <xs:element name=\"salesOrderDetailID\" type=\"xs:integer\" minOccurs=\"0\"  "+
"                  sme:label=\"ID\" sme:isIdentifier=\"true\""+
"                  sme:isReadOnly=\"true\"/>"+
"      <xs:element name=\"productID\" type=\"xs:integer\" minOccurs=\"0\""+
"                  sme:label=\"Product ID\" sme:copiedFrom=\"product/productID\" />"+
"      <xs:element name=\"orderQty\" type=\"xs:decimal\" minOccurs=\"0\""+
"                  sme:label=\"Qty\" />"+
"      <xs:element name=\"unitPrice\" type=\"xs:decimal\" minOccurs=\"0\""+
"                  sme:label=\"Unit Price\" />"+
"      <xs:element name=\"order\" type=\"tns:salesOrder--type\" minOccurs=\"0\""+
"                  sme:relationship=\"parent\" sme:label=\"Order\" />"+
"      <xs:element name=\"product\" type=\"tns:product--type\" minOccurs=\"0\""+
"                  sme:relationship=\"reference\" sme:label=\"Product\" />"+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"salesOrderLine--list\">"+
"     <xs:sequence>"+
"       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"salesOrderLine\" type=\"tns:salesOrderLine--type\" />"+
"    </xs:sequence>"+
"  </xs:complexType>"+
"  <!-- Resource Kind Definition -->"+
"  <xs:element name=\"contact\" type=\"tns:contact--type\""+
"    sme:role=\"resourceKind\" sme:pluralName=\"contacts\" sme:label=\"Contact\""+
"    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\""+
"    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" "+
"    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />"+
"  <xs:complexType name=\"contact--type\">"+
"    <xs:all>"+
"      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />"+
"      <xs:element name=\"contactID\" type=\"xs:integer\" minOccurs=\"0\""+
"                  sme:label=\"ID\" sme:isIdentifier=\"true\""+
"                  sme:isReadOnly=\"true\"/> "+
"      <xs:element name=\"civility\" type=\"tns:civility--type\" minOccurs=\"0\""+
"                  sme:label=\"Civility\" />"+
"      <xs:element name=\"firstName\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"First Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\" /> "+
"      <xs:element name=\"lastName\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"Last Name\" sme:canSort=\"true\" sme:canFilter=\"true\" sme:precedence=\"1\""+
"                  sme:isDescriptor=\"true\"/> "+
"      <xs:element name=\"address\" type=\"tns:address--type\" minOccurs=\"0\""+
"                  sme:relationship=\"child\" sme:isCollection=\"false\" sme:label=\"Address\" /> "+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"contact--list\">"+
"     <xs:sequence>"+
"       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"contact\" type=\"tns:contact--type\" />"+
"    </xs:sequence>"+
"  </xs:complexType>"+
"  <!-- Resource Kind Definition -->"+
"  <xs:element name=\"address\" type=\"tns:address--type\""+
"    sme:role=\"resourceKind\" sme:pluralName=\"addresses\" sme:label=\"Address\""+
"    sme:canGet=\"true\" sme:canPost=\"true\" sme:canPut=\"true\" sme:canDelete=\"true\""+
"    sme:canPageNext=\"true\" sme:canPagePrevious=\"true\" sme:canPageIndex=\"true\" "+
"    sme:supportsETag=\"true\" sme:batchingMode=\"syncOrAsync\" />"+
"  <xs:complexType name=\"address--type\">"+
"    <xs:all>"+
"      <xs:element name=\"uuid\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"UUID\" sme:isGlobalId=\"true\" />"+
"      <xs:element name=\"street\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"Street\" /> "+
"      <xs:element name=\"city\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"City\" />"+
"      <xs:element name=\"postalCode\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"Postal Code\" />"+
"      <xs:element name=\"countryCode\" type=\"xs:string\" minOccurs=\"0\""+
"                  sme:label=\"County Code\" />"+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"address--list\">"+
"     <xs:sequence>"+
"       <xs:element minOccurs=\"0\" maxOccurs=\"unbounded\" name=\"product\" type=\"tns:address--type\" />"+
"    </xs:sequence>"+
"  </xs:complexType>"+
"  <!-- Auxiliary Type Definition -->"+
"  <xs:simpleType name=\"civility--type\">"+
"    <xs:restriction base=\"xs:string\">"+
"      <xs:enumeration value=\"Mr\"/>"+
"      <xs:enumeration value=\"Mrs\"/>"+
"      <xs:enumeration value=\"Ms\"/>"+
"    </xs:restriction>"+
"  </xs:simpleType>"+
"  <!-- Service Operation Definition -->"+
"  <xs:element name=\"productComputeSimplePrice\" "+
"              type=\"tns:productComputeSimplePrice--type\""+
"              sme:role=\"serviceOperation\""+
"              sme:path=\"products/$service/computeSimplePrice\""+
"              sme:invocationMode=\"sync\" />"+
"  <xs:complexType name=\"productComputeSimplePrice--type\">"+
"    <xs:all>"+
"      <xs:element name=\"request\" type=\"tns:simplePriceRequest--type\" minOccurs=\"0\""+
"                  sme:envelope=\"entry\"/>"+
"      <xs:element name=\"response\" type=\"tns:simplePriceResponse--type\" minOccurs=\"0\""+
"                  sme:envelope=\"entry\"/>"+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"simplePriceRequest--type\">"+
"    <xs:all>"+
"      <xs:element name=\"productID\" type=\"xs:string\" sme:label=\"Product ID\" />"+
"      <xs:element name=\"customerID\" type=\"xs:string\" sme:label=\"Customer ID\" />"+
"      <xs:element name=\"quantity\" type=\"xs:decimal\" sme:label=\"Quantity\" />"+
"    </xs:all>"+
"  </xs:complexType>"+
"  <xs:complexType name=\"simplePriceResponse--type\">"+
"    <xs:all>"+
"      <xs:element name=\"unitPrice\" type=\"xs:decimal\" sme:label=\"Unit Price\" />"+
"      <xs:element name=\"quantityPrice\" type=\"xs:decimal\" sme:label=\"Quantity Price\" />"+
"      <xs:element name=\"discount\" type=\"xs:decimal\" sme:label=\"Discount\" />"+
"      <xs:element name=\"tax\" type=\"xs:decimal\" sme:label=\"Tax\" />"+
"    </xs:all>"+
"  </xs:complexType>"+
"</xs:schema>";


        private string _applicationName;
        private string _serverName;
        private string _protocol;
        private string _virtualDirectory;
        private string _dataSet;
        private string _contractName;
        private string _url;

        

        private string _userName;
        private string _passWord;

        private bool _initialized;
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        /// <summary>
        /// Accessor method to determine if service has been initialized
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set{ _initialized = value;}
        }
        #region ApplicationName
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        /// <remarks>
        ///     The <see cref="ApplicationName"/> is used to identify users specific to an application. That is, the same syndication resource can exist in the data store 
        ///     for multiple applications that specify a different <see cref="ApplicationName"/>. This enables multiple applications to use the same data store to store resource 
        ///     information without running into duplicate syndication resource conflicts. Alternatively, multiple applications can use the same syndication resource data store 
        ///     by specifying the same <see cref="ApplicationName"/>. The <see cref="ApplicationName"/> can be set programmatically or declaratively in the configuration for the application.
        /// </remarks>
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        #endregion


        /// <summary>
        /// Acessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }


        /// <remarks>
        /// Creates the service with predefined values for the url
        /// </remarks>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }


        /// <remarks>IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }


        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        public string VirtualDirectory
        {
            get { return _virtualDirectory; }
            set { _virtualDirectory = value; }
        }


        /// <summary>
        /// Accessor method for dataSet
        /// </summary>
        /// <remarks>Identifies the dataset when the application gives access to several datasets, such as several companies and production/test datasets.
        /// If the application can only handle a single dataset, or if it can be configured with a default dataset, 
        /// a hyphen can be used as a placeholder for the default dataset. 
        /// For example, if prod is the default dataset in the example above, the URL could be shortened as:
        /// http://www.example.com/sdata/sageApp/test/-/accounts?startIndex=21&amp;count=10 
        /// If several parameters are required to specify the dataset (for example database name and company id), 
        /// they should be formatted as a single segment in the URL. For example, sageApp/test/demodb;acme/accounts -- the semicolon separator is application specific, not imposed by SData.
        ///</remarks>
        public string DataSet
        {
            get { return _dataSet; }
            set { _dataSet = value; }
        }



        /// <summary>
        /// Accessor method for contractName
        /// </summary>
        /// <remarks>An SData service can support several “integration contracts” side-by-side. 
        /// For example, a typical Sage ERP service will support a crmErp contract which exposes 
        /// the resources required by CRM integration (with schemas imposed by the CRM/ERP contract) 
        /// and a native or default contract which exposes all the resources of the ERP in their native format.
        /// </remarks>
        public string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }


        /// <summary>
        /// Get set for the user name to authenticate with
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Get/set for the password to authenticate with
        /// </summary>
        public string Password
        {
            get { return _passWord; }
            set { _passWord = value; }
        }





        //============================================================
        //	PUBLIC METHODS
        //============================================================

        #region Create(SDataBaseURL, ISyndicationResource resource)
        /// <summary>
        /// Adds a new syndication resource to the data source.
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="resource">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        public ISyndicationResource CreateFeed(SDataBaseRequest request, XPathNavigator  resource)
        {
            AtomFeed feed = new AtomFeed();
            feed.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(atomfeed_string)));

            return feed;
        }
        #endregion




        #region Create(SDataBaseURL, ISyndicationResource resource)
        /// <summary>
        /// Adds a new syndication resource to the data source.
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="resource">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        public ISyndicationResource Create(SDataBaseRequest request, ISyndicationResource resource)
        {
            AtomEntry entry = new AtomEntry();
            entry.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(atomentry_string)));

            return entry;
        }
        #endregion


        #region CreateAsync(SDataBaseURL, ISyndicationResource resource, string trackingId)
        /// <summary>
        /// Asynchronous PUT to the server
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>

        public AsyncRequest CreateAsync(SDataBaseRequest request)
        {
            AsyncRequest asyncRequest = new AsyncRequest();

            asyncRequest.Phase = "Initializing";
            asyncRequest.PhaseDetail = "StartingThread";
            asyncRequest.Progress = (decimal)0.0;
            asyncRequest.ElapsedSeconds = 0;
            asyncRequest.RemainingSeconds = 10;
            asyncRequest.PollingMilliseconds = 500;
            //asyncRequest.XmlDoc = document;
            asyncRequest.TrackingUrl = "http://www.example.com/sdata/sageApp/test/-/products/$service/computeSimplePrice";

            return asyncRequest;
        }
        #endregion



        #region Delete(string url)

        /// <summary>
        /// a delete method NOTE: not used
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Delete(string url)
        {
            return true;
        }



        /// <summary>
        /// Generic delete from server
        /// </summary>
        /// <param name="request">the url for the operation</param>
        /// <param name="resource">the rersource being deleted</param>
        /// <returns><b>true</b> returns true if the operation was successful</returns>
        public bool Delete(SDataBaseRequest request, ISyndicationResource resource)
        {
            return true;
        }
        #endregion





       

        #region Read(string url)
        /// <summary>
        /// generic read from the specified url
        /// </summary>
        /// <param name="url">url to read from </param>
        /// <returns>string response from server</returns>
        public string Read(string url)
        {


            string result =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<tracking xmlns=\"http://schemas.sage.com/sdata/2008/1\">" +
                "<phase>Initialization</phase>" +
                "<phaseDetail>Starting thread</phaseDetail>" +
                "<progress>0.0</progress>" +
                "<elapsedSeconds>0</elapsedSeconds>" +
                "<remainingSeconds>7</remainingSeconds>" +
                "<pollingMillis>500</pollingMillis>" +
                "</tracking>";

            return result;


        }

        #endregion




        #region Read(SDataBaseURL)
        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>AtomFeed <see cref="AtomFeed"/> populated with the specified resources's information from the data source.</returns>
        public AtomFeed ReadFeed(SDataBaseRequest request)
        {
            AtomFeed feed = new AtomFeed();
            feed.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(atomfeed_string)));
            return feed;

        }
        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>An AtomEntry <see cref="AtomEntry"/> populated with the specified resources's information from the data source.</returns>
        public AtomEntry ReadEntry(SDataBaseRequest request)
        {
            AtomEntry entry = new AtomEntry();
            entry.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(atomentry_string)));
            return entry;

        }
        #endregion


        #region Read(SDataBaseURL)
        /// <summary>
        /// Reads xsd from a $schema request
        /// </summary>
        /// <param name="request">url for the syndication resource to get information for.</param>
        /// <returns>XmlSchema </returns>
        public XmlSchema Read(SDataResourceSchemaRequest request)
        {
            

            XmlTextReader reader = new XmlTextReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xsd_string)));

            XmlSchema schema = XmlSchema.Read(reader, null);
            return schema;

        }
        #endregion


        
        #region Update(SDataBaseURL url, ISyndicationResource resource)
        /// <summary>
        /// Updates information about a syndication resource in the data source.
        /// </summary>
        /// <param name="request">The url from the syndication data source for the resource to be updated.</param>
        /// <param name="resource">
        ///     An object that implements the <see cref="ISyndicationResource"/> interface that represents the updated information for the resource.
        /// </param>
        public ISyndicationResource Update(SDataBaseRequest request, ISyndicationResource resource)
        {
            AtomEntry entry = new AtomEntry();
            entry.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(atomentry_string)));
            return entry;
        }
        #endregion



        /// <summary>
        /// Default Constructor
        /// </summary>
        public SDataServiceTest()
        {
        }

        /// <summary>
        /// Constructor with pre-set url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName">the user name to initialize with</param>
        /// <param name="password">password for the specified user</param>
        public SDataServiceTest(string url, string userName, string password)
        {
            _url = url;
            _userName = userName;
            _passWord = password;


            // now lets parse the url
            string[] urlparts = url.Split(new string[] {"://", "/"}, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < urlparts.Length; x++)
            {

                switch (x)
                {
                    case 0:
                        {
                            Protocol = urlparts[x];
                            break;
                        }
                    case 1:
                        {
                            ServerName = urlparts[x];
                            break;
                        }
                    case 2:
                        {
                            VirtualDirectory = urlparts[x];
                            break;
                        }
                    case 3:
                        {
                            ApplicationName = urlparts[x];
                            break;
                        }
                    case 4:
                        {
                            ContractName = urlparts[x];
                            break;
                        }
                    case 5:
                        {
                            DataSet = urlparts[x];
                            break;
                        }
                    default:
                        break;
                }
            }

        }


        //============================================================
        //	PRIVATE METHODS
        //============================================================
        #region Initialize()
        /// <summary>
        /// Initializes the <see cref="SDataService"/> 
        /// </summary>
        /// <remarks>sett the User Name and Password to authenticate with and build the url</remarks>
        public void Initialize()
        {
            
            if (_url == null)
            {
                _url = this.Protocol + "://" +
                    this.ServerName + "/" +
                    this.VirtualDirectory + "/" +
                    this.ApplicationName + "/" +
                    this.ContractName + "/" +
                    this.DataSet + "/";
            }

            Initialized = true;
        }
        #endregion
    }

}

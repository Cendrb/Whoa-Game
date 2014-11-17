using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// To prepare a Google Spreadsheet for this:
// make sure the spreadsheet has been published to web (FILE >> PUBLISH TO WEB), which is NOT the same as sharing to web

// To use in a different script:
// 1) include "using Google.GData.Client;"
// 2) include "using Google.GData.Spreadsheets;"
// 3) call "GDocService.GetSpreadsheet( string spreadsheetKey );", see DebugTest section for sample usage

using UnityEngine;
using System.Collections;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class GDriveManager
{
    // grab your spreadsheet's ID / "key" from the URL to access your doc...
    // make sure stop as soon as you hit an ampersand, those are additional URL parameters we don't need
    public static ListFeed GetSpreadsheet(string spreadsheetID, int gridId)
    {
        // We need this fake certificate to trick Mono's security to use HTTPS... doesn't work in webplayer's security sandbox
        InsecureSecurityCertificatePolicy.Instate();

        SpreadsheetsService service = new SpreadsheetsService("UnityConnect");

        string url = "https://spreadsheets.google.com/feeds/list/" + spreadsheetID + "/" + gridId + "/public/values";
        ListQuery listQuery = new ListQuery(url);

        ListFeed feed = service.Query(listQuery);

        return feed;
    }
}

// from http://answers.unity3d.com/questions/249052/accessing-google-apis-in-unity.html
public class InsecureSecurityCertificatePolicy
{
    public static bool Validator(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors policyErrors)
    {
        // Just accept and move on...
        return true;
    }

    public static void Instate()
    {
        ServicePointManager.ServerCertificateValidationCallback = Validator;
    }
}

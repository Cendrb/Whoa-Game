using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class GameSettings
{
    public int ObstaclesPerSection { get; set; }
    public float SpaceBetweenObstacles { get; set; }

    public float FreeAreaSize { get; set; }

    public float FreeAreaEntityOffset { get; set; }


    // TEMPORARY!
    public int currentAspect;
    public int CADuration;
    public int CAAmplifier;

    public static GameSettings LoadFromDrive()
    {
        GameSettings settings = new GameSettings();
        ListFeed list = GDriveManager.GetSpreadsheet(WhoaPlayerProperties.DRIVE_DOCUMENT_URL, 6);
        ListEntry row = (ListEntry)list.Entries[0];
        settings.ObstaclesPerSection = int.Parse(row.Elements[0].Value);
        settings.SpaceBetweenObstacles = float.Parse(row.Elements[1].Value);
        settings.FreeAreaSize = float.Parse(row.Elements[2].Value);
        settings.FreeAreaEntityOffset = float.Parse(row.Elements[3].Value);
        return settings;
    }
}

